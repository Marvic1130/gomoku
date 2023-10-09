from collections import deque
from enum import Enum
import Rules

class Stone(Enum):
    B = 1
    W = 2

    def __str__(self):
        return 'Black' if self is Stone.B else 'White'

class Board:
    def __init__(self, size=15):
        self.size = size
        self._board = [[None for _ in range(size)] for _ in range(size)]
        self.turn = 0
        self.turn_deque = deque()

    @property
    def board(self):
        return self._board

    def get(self, row, col):
        return self._board[row][col]

    def is_valid(self, row, col, stone):
        return Rules.check_violation(self._board, stone, row, col)

    def is_empty(self, row, col):
        return self._board[row][col] is None

    def is_full(self):
        return self.turn == self.size ** 2

    def is_win(self, row, col, stone):
        return Rules.check_win(self._board, row, col, stone)

    def set(self, row, col, stone):
        if self._board[row][col] is not None:
            return False, None
        elif stone is Stone.B:
            if self.is_valid(row, col, stone):
                self._board[row][col] = stone
                self.turn_deque.appendleft((row, col))

                self.turn += 1
            else:
                return False, None

        elif stone is Stone.W:
            self._board[row][col] = stone
            self.turn += 1

        win = self.is_win(row, col, stone)
        if win:
            return True, str(stone)
        elif self.is_full():
            return True,  'Draw'

        return True, None

    def undo(self):
        for _ in range(2):
            row, col = self.turn_deque.popleft()
            self._board[row][col] = None
        self.turn -= 2

    def get_moves(self):
        result = []
        for i in range(self.turn):
            row, col = self.turn_deque[i]
            result.append(f'{str(Stone((i%2)+1))}{i + 1}. ({row}, {col})')

        return result
