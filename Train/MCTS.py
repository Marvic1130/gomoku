from Rules.Board import Board, Stone
from Rules.Rules import check_win
from Utils import parameters as param
import random
import math


class MCTSNode:
    def __init__(self, board: Board, parent=None):
        self.board = board
        self.parent = parent
        self.children = []
        self.visits = 0
        self.value = 0
        self.stone = Stone.B if board.turn % 2 == 1 else Stone.W


def uct_function(node):
    return node.value / node.visits + param.EXPLORATION_WEIGHT * math.sqrt(math.log(node.parent.visits) / node.visits)


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


def mcts_search(board, num_iterations):
    root = MCTSNode(board)
    for _ in range(num_iterations):
        node = root
        while node.children:
            node = select_child(node)

        if not node.board.is_full() and not check_win(node.board.board, Stone.B, node.board.turn_deque[0][1]):
            child_board = node.board.copy()

            if child_board.turn == 0:  # 첫 번째 턴인 경우 (7, 7)에 놓기
                move = (7, 7)
            elif child_board.turn == 1:  # 두 번째 턴인 경우 주변 8개 위치 중 하나에 놓기
                first_turn_moves = child_board.turn_deque[0]
                second_turn_moves = []
                for r in range(-1, 2):
                    for c in range(-1, 2):
                        if r != 0 and c != 0:
                            second_turn_moves.append((first_turn_moves[0] + r, first_turn_moves[1] + c))
                move = random.choice(second_turn_moves)
            elif child_board.turn == 2:  # 세 번째 턴인 경우 주변 23개 위치 중 하나에 놓기
                first_turn_moves = child_board.turn_deque[0]
                second_turn_moves = child_board.turn_deque[1]

                relative_position = (second_turn_moves[0] - first_turn_moves[0],
                                     second_turn_moves[1] - first_turn_moves[1])
                third_turn_moves = []
                for r in range(-2, 3):
                    for c in range(-2, 3):
                        if (r, c) not in [(0, 0), relative_position]:
                            third_turn_moves.append((second_turn_moves[0] + r, second_turn_moves[1] + c))
                for i in [(-2, -2), (-2, 2), (2, -2), (2, 2)]:
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

    best_child = select_best_child(root)
    return best_child.board


def simulate(node):
    board = node.board.copy()
    current_player = Stone.B
    while not board.is_full() and not check_win(board.board, current_player, node.board.turn_deque[0][1]):
        legal_moves = [(r, c) for r in range(board.size) for c in range(board.size) if
                       board.is_valid(r, c, current_player)]
        move = random.choice(legal_moves) if legal_moves else (-1, -1)
        board.set(move[0], move[1], current_player)
        current_player = Stone.W if current_player == Stone.B else Stone.B
    result = 1 if check_win(board.board, Stone.B, node.board.turn_deque[0][1]) else 0
    return result


def backpropagate(node, result):
    while node:
        node.visits += 1
        node.value += result
        node = node.parent
