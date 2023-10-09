class RenjuRule:
    def __init__(self, board):
        self.board = board

    def is_open_three(self, player, row, col):
        directions = [(0, 1), (1, 0), (1, -1), (1, 1)]
        for dx, dy in directions:
            line = ''
            for i in range(-5, 6):
                x = row + dx * i
                y = col + dy * i
                if not (0 <= x < len(self.board) and 0 <= y < len(self.board)):
                    line += ' '
                elif self.board[x][y] == player:
                    line += 'O'
                else:
                    line += 'X'
            if 'XOOOX' in line:
                return True
        return False

    def is_open_four(self, player, row, col):
        directions = [(0, 1), (1, 0), (1, -1), (1, 1)]
        for dx, dy in directions:
            line = ''
            for i in range(-5, 6):
                x = row + dx * i
                y = col + dy * i
                if not (0 <= x < len(self.board) and 0 <= y < len(self.board)):
                    line += ' '
                elif self.board[x][y] == player:
                    line += 'O'
                else:
                    line += 'X'
            if 'XOOOOX' in line:
                return True
        return False

    def is_overline(self, player, row, col):
        directions = [(0, 1), (1, 0), (1, -1), (1, 1)]
        for dx, dy in directions:
            line = ''
            for i in range(-5, 6):
                x = row + dx * i
                y = col + dy * i
                if not (0 <= x < len(self.board) and 0 <= y < len(self.board)):
                    line += ' '
                elif self.board[x][y] == player:
                    line += 'O'
                else:
                    line += 'X'
            if 'OOOOOO' in line:
                return True
        return False

    def is_double_three(self, player, row, col):
        if not self.is_open_three(' ', row, col):
            return False
        count = 0
        self.board[row][col] = player
        directions = [(0, -1), (0, +1), (+1, -1), (+1, +1), (-2, +2), (+2, -2)]

        for dx, dy in directions:
            x = row + dx
            y = col + dy

            if not (0 <= x < len(self.board) and 0 <= y < len(self.board)):
                continue

            if self.board[x][y] == player and self.is_open_three(' ', x, y):
                count += 10

        self.board[row][col] = ' '

        return count >= 20

    def is_double_four(self, player, row, col):
        if not self.is_open_four(' ', row, col):
            return False
        count = 0
        self.board[row][col] = player
        directions = [(0, -1), (0, +1), (+1, -1), (+1, +1), (-2, +2), (+2, -2)]

        for dx, dy in directions:
            x = row + dx
            y = col + dy

            if not (0 <= x < len(self.board) and 0 <= y < len(self.board)):
                continue

            if self.board[x][y] == player and self.is_open_four(' ', x, y):
                count += 10

        self.board[row][col] = ' '

        return count >= 20

    def check_renju_rules_violation(self, player, row, col):
        # 첫 번째 수를 놓는 흑돌의 경우 확인합니다.
        if player == "X" and self.turn == 0:
            return not (row == self.size // 2 and col == self.size // 2)

        # 금지된 수를 놓는 흑돌의 경우 확인합니다.
        double_three = self.is_double_three(player, row, col)
        double_four = self.is_double_four(player, row, col)
        overline = self.is_overline(player, row, col)

        return double_three or double_four or overline
