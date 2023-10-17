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
        self.size = size # 바둑판의 크기
        self._board = [[None for _ in range(size)] for _ in range(size)] # 바둑판 초기화
        self.turn = 0
        self.turn_deque = deque() # 바둑판에 놓인 돌의 위치를 기록

    def copy(self):
        new_board = Board(self.size)
        new_board._board = [row[:] for row in self._board]
        new_board.turn = self.turn
        new_board.turn_deque = deque(self.turn_deque)
        return new_board

    @property
    def board(self):
        return self._board

    def get(self, row, col): # 바둑판의 특정 위치의 돌을 반환
        return self._board[row][col]

    def is_valid(self, row, col, stone): # 바둑판의 특정 위치에 돌을 놓을 수 있는지 검사
        if stone == Stone.B:
            return Rules.check_violation(self._board, stone, row, col)
        else:
            return True

    def is_empty(self, row, col): # 바둑판의 특정 위치가 비어있는지 검사
        return self._board[row][col] is None

    def is_full(self): # 바둑판이 가득 찼는지 검사
        return self.turn == self.size ** 2

    def is_win(self, row, col, stone): # 바둑판의 특정 위치에 돌을 놓았을 때 승리했는지 검사
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
