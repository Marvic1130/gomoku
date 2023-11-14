import torch
import torch.nn.functional as F
import numpy as np
import copy

class TreeNode(object):
    """MCTS 트리의 노드.
    Q : 자신의 가치
    P : 사전 확률
    u : 방문 횟수에 따라 조정된 사전 점수
    """

    def __init__(self, parent, prior_p):
        self._parent = parent
        self._children = {}  # 액션과 TreeNode를 매핑하는 딕셔너리
        self._n_visits = 0
        self._Q = 0
        self._u = 0
        self._P = prior_p

    def expand(self, action_priors, forbidden_moves, is_you_black):
        """액션과 사전 확률에 따라 새로운 자식 노드를 생성하여 트리를 확장.
        action_priors: 정책 함수에 따른 액션과 해당 액션의 사전 확률로 이루어진 튜플의 리스트.
        """
        for action, prob in action_priors:
            # 흑돌일 때 금수 위치는 트리 탐색을 하지 않도록 한다.
            if is_you_black and action in forbidden_moves:
                continue
            if action not in self._children:
                self._children[action] = TreeNode(self, prob)

    def select(self, c_puct):
        """Q 값에 보너스 u(P)를 더한 값이 최대인 액션을 선택.
        반환값: (액션, 다음 노드)로 이루어진 튜플
        """
        return max(self._children.items(), key=lambda act_node: act_node[1].get_value(c_puct))

    def update(self, leaf_value):
        """리프 노드의 평가 결과로부터 노드 값을 업데이트.
        leaf_value: 현재 플레이어의 관점에서의 서브트리 평가 값.
        """
        # 방문 횟수 증가.
        self._n_visits += 1
        # Q 값은 모든 방문에 대한 값의 평균.
        self._Q += 1.0 * (leaf_value - self._Q) / self._n_visits

    def update_recursive(self, leaf_value):
        """update() 호출과 유사하지만, 모든 조상 노드에 대해 재귀적으로 적용."""
        # 루트 노드가 아닌 경우, 먼저 부모 노드를 업데이트.
        if self._parent:
            self._parent.update_recursive(-leaf_value)
        self.update(leaf_value)

    def get_value(self, c_puct):
        """이 노드의 값을 계산하여 반환.
        이는 리프 평가 값 Q와 방문 횟수에 대해 조정된 사전 확률 P의 조합.
        c_puct: 이 노드의 점수에 대한 값 Q와 사전 확률 P의 상대적인 영향을 조절하는 상수.
        """
        self._u = (c_puct * self._P *
                   torch.sqrt(self._parent._n_visits) / (1 + self._n_visits))
        return self._Q + self._u

    def is_leaf(self):
        """리프 노드인지 확인. (즉, 이 노드 아래에 노드가 없는지)"""
        return self._children == {}

    def is_root(self):
        return self._parent is None


class MCTS(object):
    """Monte Carlo Tree Search의 구현체."""

    def __init__(self, policy_value_fn, c_puct=5, n_playout=10000):
        """
        policy_value_fn: 현재 상태에 대한 액션과 확률, 현재 플레이어의 예상 게임 종료 점수(-1에서 1 사이)를 출력하는 함수.
        c_puct: 최대값 정책에 대한 탐색이 얼마나 빠르게 수렴되는지를 제어하는 상수. 높은 값은 사전 확률에 더 의존.
        """
        self._root = TreeNode(None, 1.0)
        self._policy = policy_value_fn
        self._c_puct = c_puct
        self._n_playout = n_playout

    def _playout(self, state):
        """루트에서 리프까지 단일 플레이아웃을 실행하고, 리프에서부터 상위 노드로 결과를 전파.
        상태는 직접 수정되므로 복사본을 제공해야 한다.
        """
        node = self._root
        while True:
            if node.is_leaf():
                break
            # 탐욕적으로 다음 수를 선택.
            action, node = node.select(self._c_puct)
            state.do_move(action)

        # 현재 플레이어의 관점에서 게임 종료 점수를 출력하는 네트워크를 사용하여 리프를 평가.
        action_probs, leaf_value = self._policy(state)
        end, winner = state.game_end()
        if not end:
            node.expand(action_probs, state.forbidden_moves, state.is_you_black())
        else:
            # 종료 상태인 경우 "실제" 리프 평가 값을 반환.
            if winner == -1:  # 무승부
                leaf_value = 0.0
            else:
                leaf_value = (1.0 if winner == state.get_current_player() else -1.0)

        node.update_recursive(-leaf_value)

    def get_move_probs(self, state, temp=1e-3):
        for _ in range(self._n_playout):
            state_copy = copy.deepcopy(state)
            self._playout(state_copy)

        act_visits = [(act, node._n_visits) for act, node in self._root._children.items()]

        # 각 액션에 대한 방문 횟수를 이용하여 확률을 계산.
        acts, visits = zip(*act_visits)
        act_probs = F.softmax(torch.tensor(visits, dtype=torch.float32) / temp, dim=0).numpy()

        return acts, act_probs

    def update_with_move(self, last_move):
        if last_move in self._root._children:
            self._root = self._root._children[last_move]  # 돌을 둔 위치가 루트 노드.
            self._root._parent = None
        else:
            self._root = TreeNode(None, 1.0)

    def __str__(self):
        return "MCTS"


class MCTSPlayer(object):
    def __init__(self, policy_value_function, c_puct=5, n_playout=2000, is_selfplay=0):
        self.mcts = MCTS(policy_value_function, c_puct, n_playout)
        self._is_selfplay = is_selfplay

    def set_player_ind(self, p):
        self.player = p

    def reset_player(self):
        self.mcts.update_with_move(-1)

    def get_action(self, board, temp=1e-3, return_prob=0):
        move_probs = torch.zeros(board.width * board.height)
        if board.width * board.height - len(board.states) > 0:
            acts, probs = self.mcts.get_move_probs(board, temp)
            move_probs[list(acts)] = torch.tensor(probs)
            if self._is_selfplay:
                # (자가 학습을 할 때는) 디리클레 노이즈를 추가하여 탐색.
                move = torch.tensor(np.random.choice(
                    acts, p=0.75 * probs + 0.25 * np.random.dirichlet(0.3 * np.ones(len(probs)))
                ))
                self.mcts.update_with_move(move.item())
            else:
                move = torch.tensor(np.random.choice(acts, p=probs))
                self.mcts.update_with_move(-1)

            if return_prob:
                return move.item(), move_probs
            else:
                return move.item()

        else:
            print("Warring: Board is full!!!")
