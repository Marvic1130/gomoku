from Rules.Board import Board, Stone
from Rules.Rules import check_win
from Utils import parameters as param
import random
import math

# MCTS 노드 클래스 정의
class MCTSNode:
    def __init__(self, board: Board, parent=None):
        self.board = board  # 현재 게임판 상태
        self.parent = parent  # 부모 노드
        self.children = []  # 자식 노드 리스트
        self.visits = 0  # 노드를 방문한 횟수
        self.value = 0  # 노드의 가치 (게임 승리 여부에 따라)
        self.stone = Stone.B if board.turn % 2 == 1 else Stone.W  # 현재 플레이어의 돌 색상

# UCT 평가 함수 정의
def uct_function(node):
    return node.value / node.visits + param.EXPLORATION_WEIGHT * math.sqrt(math.log(node.parent.visits) / node.visits)

# UCT 알고리즘을 사용하여 자식 노드 선택
def select_child(node):
    max_score = -float('inf')
    selected_child = None
    for child in node.children:
        if child.visits == 0:
            return child
        score = uct_function(child)
        if score > max_score:
            max_score = score
            selected_child = child
    return selected_child

# 가치가 가장 높은 자식 노드 선택
def select_best_child(node):
    max_value = -float('inf')
    best_child = None
    for child in node.children:
        if child.visits > 0:
            value = child.value / child.visits
            if value > max_value:
                max_value = value
                best_child = child
    return best_child

# MCTS 탐색 메인 함수
def mcts_search(board, num_iterations):
    root = MCTSNode(board)
    for _ in range(num_iterations):
        node = root
        while node.children:
            node = select_child(node)

        if not node.board.is_full():
            child_board = node.board.copy()

            if child_board.turn == 0:  # 첫 번째 턴: 항상 중심 (7, 7)에 놓음
                move = (7, 7)
            elif child_board.turn == 1:  # 두 번째 턴: 주변 8개 위치 중 하나 무작위 선택
                first_turn_moves = child_board.turn_deque[0]
                second_turn_moves = []
                for r in range(-1, 2):
                    for c in range(-1, 2):
                        if r != 0 and c != 0:
                            second_turn_moves.append((first_turn_moves[0] + r, first_turn_moves[1] + c))
                move = random.choice(second_turn_moves)
            elif child_board.turn == 2:  # 세 번째 턴: 주변 23개 위치 중 하나 무작위 선택
                first_turn_moves = child_board.turn_deque[1]
                second_turn_moves = child_board.turn_deque[0]

                relative_position = (second_turn_moves[0] - first_turn_moves[0],
                                     second_turn_moves[1] - first_turn_moves[1])
                third_turn_moves = []
                for r in range(-2, 3):
                    for c in range(-2, 3):
                        if (r, c) not in [(0, 0), relative_position]:
                            third_turn_moves.append((second_turn_moves[0] + r, second_turn_moves[1] + c))
                for i in [(-2, -2), (-2, 2), (2, -2), (2, 2)]:  # 26주형중 흑돌이 불리하거나 백돌과 동등한 위치 제거
                    third_turn_moves.remove(i)

                move = random.choice(third_turn_moves)
            else:
                legal_moves = [(r, c) for r in range(board.size) for c in range(board.size) if
                               child_board.is_valid(r, c, child_board.stone)]
                move = random.choice(legal_moves) if legal_moves else (-1, -1)

            child_board.set(move[0], move[1], Stone.B)
            child_node = MCTSNode(child_board, parent=node)
            node.children.append(child_node)
            result = simulate(child_node)
            backpropagate(child_node, result)
        else:
            break

    best_child = select_best_child(root)
    return best_child.board

# 게임 시뮬레이션 함수
def simulate(node):
    board = node.board.copy()
    current_player = node.stone
    while not board.is_full():
        legal_moves = [(r, c) for r in range(board.size) for c in range(board.size) if
                       board.is_valid(r, c, current_player)]
        move = random.choice(legal_moves) if legal_moves else (-1, -1)
        board.set(move[0], move[1], current_player)
        current_player = Stone.W if current_player == Stone.B else Stone.B

        if check_win(board.board, move[0], move[1], current_player):
            result = 1
            return result

    result = 0
    return result

# 역전파 함수
def backpropagate(node, result):
    while node:
        node.visits += 1
        node.value += result
        node = node.parent
