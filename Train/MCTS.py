from Rules.Board import Board, Stone
from Rules.Rules import check_win, check_violation
from Utils import parameters as param
import random
import math
import os
from multiprocessing import Pool


# MCTS 노드 클래스 정의
class MCTSNode:
    def __init__(self, board: Board, parent=None):
        self.board = board  # 현재 게임판 상태
        self.parent = parent  # 부모 노드
        self.children = []  # 자식 노드 리스트
        self.visits = 0  # 노드를 방문한 횟수
        self.value = 0  # 노드의 가치 (게임 승리 여부에 따라)
        self.stone = Stone.B if board.turn % 2 == 0 else Stone.W  # 현재 플레이어의 돌 색상


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

    # 만약 모든 자식노드의 방문횟수가 0이거나, 아예 자식노드가 없는 경우, None을 반환하지 않고 임의로 첫번째 자식노드를 반환하도록 함.
    if best_child is None and node.children:
        return node.children[0]

    return best_child


def simulate_parallel(node):
    return simulate(node), node


# MCTS 탐색 메인 함수
def mcts_search(board, num_iterations):
    root = MCTSNode(board)
    if not root.board.is_full():
        num_cores = os.cpu_count()  # 시스템의 CPU 코어 수를 얻음
        pool = Pool(processes=num_cores)

        for _ in range(num_iterations):
            node = root
            while node.children:
                node = select_child(node)

            if not node.board.is_full():
                child_board = node.board.copy()

                if child_board.turn == 0:  # 첫 번째 턴: 항상 중심 (7, 7)에 놓음
                    move = (7, 7)
                    child_board.set(move[0], move[1], node.stone)

                elif child_board.turn == 1:  # 두 번째 턴: 주변 8개 위치 중 하나 무작위 선택
                    first_turn_moves = child_board.turn_deque[0]
                    second_turn_moves = []
                    for r in range(-1, 2):
                        for c in range(-1, 2):
                            if not (r == 0 and c == 0):
                                second_turn_moves.append((first_turn_moves[0] + r,
                                                          first_turn_moves[1] + c))

                    move = random.choice(second_turn_moves)
                    child_board.set(move[0], move[1], node.stone)

                elif child_board.turn == 2:  # 세 번째 턴: 주변23개 위치 중 하나 무작위 선택
                    first_turn_moves = child_board.turn_deque[1]
                    second_turn_moves = child_board.turn_deque[0]

                    relative_position = (second_turn_moves[0] - first_turn_moves[0],
                                         second_turn_moves[1] - first_turn_moves[1])
                    third_tun_positions = []

                    for r in range(-2, 3):
                        for c in range(-2, 3):

                            if (r, c) not in [(0, 0), relative_position,
                                              (-2, -2), (-2, +2), (+2, -2), (+2, +2)]:
                                third_tun_positions.append((first_turn_moves[0] + r,
                                                            first_turn_moves[1] + c))

                    move = random.choice(third_tun_positions)
                    child_board.set(move[0], move[1], node.stone)

                else:
                    legal_move = [(r, c) for r in range(board.size) for c
                                  in range(board.size) if child_board.is_valid(r, c, node.stone)]
                    move = random.choice(legal_move) if legal_move else (-1, -1)
                    child_board.set(move[0], move[1], node.stone)

                child_node = MCTSNode(child_board, parent=node)
                node.children.append(child_node)

            else:
                break

        result_node_pairs = pool.map(simulate_parallel,
                                     [child for child in node.children])

        results, nodes = zip(*result_node_pairs)
        result = sum(results) / len(results)

        backpropagate(node, result)

        # multiprocessing pool 종료
        pool.close()

    else:  # 게임판이 이미 꽉 찬 경우, 현재 상태 그대로 반환함.
        return board

    best_child = select_best_child(root)

    if best_child is None:
        return board

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

        if check_win(board.board, move[0], move[1], current_player):
            result = 1 if current_player == Stone.B else -1  # Black wins: 1 / White wins: -1
            return result

        # Switch player after making a move.
        current_player = Stone.W if current_player == Stone.B else Stone.B

    result = 0  # Drawn game.
    return result


# 역전파 함수
def backpropagate(node, result):
    while node:
        node.visits += 1
        node.value += result
        node = node.parent
