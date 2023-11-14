from Utils.parameters import EMPTY

class Renju_Rule(object):
    def __init__(self, board, board_size):
        self.board = board
        self.board_size = board_size

    def is_invalid(self, x, y):
        # 주어진 좌표가 바둑판의 범위를 벗어나는지 검사.
        return (x < 0 or x >= self.board_size or y < 0 or y >= self.board_size)

    def set_stone(self, x, y, stone):
        # 주어진 좌표에 돌을 놓는다.
        self.board[y][x] = stone

    def get_xy(self, direction):
        # 주어진 방향에 따른 x, y 이동 벡터를 반환.
        # 0: 좌, 1: 우, 2: 좌상, 3: 우하, 4: 상, 5: 하, 6: 우상, 7: 좌
        list_dx = [-1, 1, -1, 1, 0, 0, 1, -1]
        list_dy = [0, 0, -1, 1, -1, 1, -1, 1]
        return list_dx[direction], list_dy[direction]

    def get_stone_count(self, x, y, stone, direction):
        # 주어진 위치에서 주어진 방향으로 연속된 돌의 개수를 반환.
        x1, y1 = x, y
        cnt = 1
        for i in range(2):
            dx, dy = self.get_xy(direction * 2 + i)
            x, y = x1, y1
            while True:
                x, y = x + dx, y + dy
                if self.is_invalid(x, y) or self.board[y][x] != stone:
                    break;
                else:
                    cnt += 1
        return cnt

    def is_gameover(self, x, y, stone):
        # 주어진 위치에서 승리 조건을 만족하는지 확인.
        for i in range(4):
            cnt = self.get_stone_count(x, y, stone, i)
            if cnt >= 5:
                return True
        return False

    def is_six(self, x, y, stone):
        # 주어진 위치에서 6개의 돌이 연속된 패턴을 검사.
        for i in range(4):
            cnt = self.get_stone_count(x, y, stone, i)
            if cnt > 5:
                return True
        return False

    def is_five(self, x, y, stone):
        # 주어진 위치에서 5개의 돌이 연속된 패턴을 검사.
        for i in range(4):
            cnt = self.get_stone_count(x, y, stone, i)
            if cnt == 5:
                return True
        return False

    def find_empty_point(self, x, y, stone, direction):
        # 주어진 위치와 방향에서 비어있는 좌표를 찾아 반환.
        dx, dy = self.get_xy(direction)
        while True:
            x, y = x + dx, y + dy
            if self.is_invalid(x, y) or self.board[y][x] != stone:
                break
        if not self.is_invalid(x, y) and self.board[y][x] == EMPTY:
            return x, y
        else:
            return None

    def open_three(self, x, y, stone, direction):
        # 주어진 위치와 방향에서 3개의 돌이 연속된 패턴을 만들 수 있는지 확인.
        for i in range(2):
            coord = self.find_empty_point(x, y, stone, direction * 2 + i)
            if coord:
                dx, dy = coord
                self.set_stone(dx, dy, stone)
                if 1 == self.open_four(dx, dy, stone, direction):
                    if not self.forbidden_point(dx, dy, stone):
                        self.set_stone(dx, dy, EMPTY)
                        return True
                self.set_stone(dx, dy, EMPTY)
        return False

    def open_four(self, x, y, stone, direction):
        # 주어진 위치와 방향에서 4개의 돌이 연속된 패턴을 만들 수 있는지 확인.
        if self.is_five(x, y, stone):
            return False
        cnt = 0
        for i in range(2):
            coord = self.find_empty_point(x, y, stone, direction * 2 + i)
            if coord:
                if self.five(coord[0], coord[1], stone, direction):
                    cnt += 1
        if cnt == 2:
            if 4 == self.get_stone_count(x, y, stone, direction):
                cnt = 1
        else:
            cnt = 0
        return cnt

    def four(self, x, y, stone, direction):
        # 주어진 위치와 방향에서 4개의 돌이 연속된 패턴을 만드는지 확인.
        for i in range(2):
            coord = self.find_empty_point(x, y, stone, direction * 2 + i)
            if coord:
                if self.five(coord[0], coord[1], stone, direction):
                    return True
        return False

    def five(self, x, y, stone, direction):
        # 주어진 위치와 방향에서 5개의 돌이 연속된 패턴을 만드는지 확인.
        if 5 == self.get_stone_count(x, y, stone, direction):
            return True
        return False

    def double_three(self, x, y, stone):
        # 주어진 위치에서 두 번 이상 3개의 돌이 연속된 패턴을 만들 수 있는지 확인.
        cnt = 0
        self.set_stone(x, y, stone)
        for i in range(4):
            if self.open_three(x, y, stone, i):
                cnt += 1
        self.set_stone(x, y, EMPTY)
        if cnt >= 2:
            return True
        return False

    def double_four(self, x, y, stone):
        # 주어진 위치에서 두 번 이상 4개의 돌이 연속된 패턴을 만들 수 있는지 확인.
        cnt = 0
        self.set_stone(x, y, stone)
        for i in range(4):
            if self.open_four(x, y, stone, i) == 2:
                cnt += 2
            elif self.four(x, y, stone, i):
                cnt += 1
        self.set_stone(x, y, EMPTY)
        if cnt >= 2:
            return True
        return False

    def forbidden_point(self, x, y, stone):
        # 주어진 위치가 금수(불가능한 수)인지 확인.
        if self.is_five(x, y, stone):
            return False
        elif self.is_six(x, y, stone):
            return True
        elif self.double_three(x, y, stone) or self.double_four(x, y, stone):
            return True
        return False

    def get_forbidden_points(self, stone):
        # 주어진 돌의 입장에서 금수인 좌표들을 반환.
        coords = []
        for y in range(len(self.board)):
             for x in range(len(self.board[0])):
                if self.board[y][x]: continue
                if self.forbidden_point(x, y, stone): coords.append((x, y))
        return [(y, x) for x, y in coords]
