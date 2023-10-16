from Board import Stone


def is_open_three(board, player: Stone, row, col):
    # 주어진 위치에서 3개의 돌이 연속된 패턴을 검사.
    directions = [(0, 1), (1, 0), (1, -1), (1, 1)]  # 가로, 세로, 대각선 방향
    for dx, dy in directions:
        line = ''
        for i in range(-5, 6):
            x = row + dx * i
            y = col + dy * i
            if not (0 <= x < len(board) and 0 <= y < len(board)):
                line += ' '  # 보드 바깥의 위치는 공백으로 처리
            elif board[x][y] == player:
                line += 'O'  # 플레이어의 돌은 'O'
            else:
                line += 'X'  # 상대방 돌은 'X'
        if 'XOOOX' in line:
            return True  # 'XOOOX' 패턴이 발견되면 True 반환
    return False


def is_open_four(board, player: Stone, row, col):
    # 주어진 위치에서 4개의 돌이 연속된 패턴을 검사.
    directions = [(0, 1), (1, 0), (1, -1), (1, 1)]  # 가로, 세로, 대각선 방향
    for dx, dy in directions:
        line = ''
        for i in range(-5, 6):
            x = row + dx * i
            y = col + dy * i
            if not (0 <= x < len(board) and 0 <= y < len(board)):
                line += ' '  # 보드 바깥의 위치는 공백으로 처리
            elif board[x][y] == player:
                line += 'O'  # 플레이어의 돌은 'O'
            else:
                line += 'X'  # 상대방 돌은 'X'
        if 'XOOOOX' in line:
            return True  # 'XOOOOX' 패턴이 발견되면 True 반환
    return False


def is_overline(board, player: Stone, row, col):
    # 주어진 위치에서 6개의 돌이 연속된 패턴을 검사.
    directions = [(0, 1), (1, 0), (1, -1), (1, 1)]  # 가로, 세로, 대각선 방향
    for dx, dy in directions:
        line = ''
        for i in range(-5, 6):
            x = row + dx * i
            y = col + dy * i
            if not (0 <= x < len(board) and 0 <= y < len(board)):
                line += ' '  # 보드 바깥의 위치는 공백으로 처리
            elif board[x][y] == player:
                line += 'O'  # 플레이어의 돌은 'O'
            else:
                line += 'X'  # 상대방 돌은 'X'
        if 'OOOOOO' in line:
            return True  # 'OOOOOO' 패턴이 발견되면 True 반환
    return False


def is_double_three(board, player: Stone, row, col):
    # 주어진 위치에서 두 번 이상 3개의 돌이 연속된 패턴을 검사.
    if not is_open_three(board, player, row, col):
        return False
    count = 0
    _board = board.copy()
    _board[row][col] = player
    directions = [(0, -1), (0, +1), (+1, -1), (+1, +1), (-2, +2), (+2, -2)]

    for dx, dy in directions:
        x = row + dx
        y = col + dy

        if not (0 <= x < len(board) and 0 <= y < len(board)):
            continue

        if board[x][y] == player and is_open_three(board, player, x, y):
            count += 10  # 두 번 이상 3개의 돌이 연속된 경우, count를 증가.

    _board[row][col] = None

    return count >= 20  # count가 20 이상이면 True 반환


def is_double_four(board, player: Stone, row, col):
    # 주어진 위치에서 두 번 이상 4개의 돌이 연속된 패턴을 검사.
    if not is_open_four(board, player, row, col):
        return False
    count = 0
    _board = board.copy()
    _board[row][col] = player
    directions = [(0, -1), (0, +1), (+1, -1), (+1, +1), (-2, +2), (+2, -2)]

    for dx, dy in directions:
        x = row + dx
        y = col + dy

        if not (0 <= x < len(board) and 0 <= y < len(board)):
            continue

        if board[x][y] == player and is_open_four(board, player, x, y):
            count += 10  # 두 번 이상 4개의 돌이 연속된 경우, count를 증가.

    _board[row][col] = 'None'

    return count >= 20  # count가 20 이상이면 True 반환


def check_violation(board, player: Stone, row, col):
    double_three = is_double_three(board, player, row, col)
    double_four = is_double_four(board, player, row, col)
    overline = is_overline(board, player, row, col)

    return double_three or double_four or overline


def check_win(board, row, col, stone: Stone):
    directions = [(0, 1), (1, 0), (1, 1), (1, -1)]  # 가로, 세로, 대각선 방향
    for dx, dy in directions:
        count = 1  # 현재 놓은 돌을 기준으로 1개를 센다.
        # 현재 위치로부터 양 방향으로 돌을 센다.
        for i in range(1, 5):
            x = row + dx * i
            y = col + dy * i
            if not (0 <= x < board.size and 0 <= y < board.size):
                break
            if board[x][y] == stone:
                count += 1
            else:
                break

        # 현재 위치로부터 반대 방향으로 돌을 센다.
        for i in range(1, 5):
            x = row - dx * i
            y = col - dy * i
            if not (0 <= x < board.size and 0 <= y < board.size):
                break
            if board[x][y] == stone:
                count += 1
            else:
                break

        if stone is Stone.W and count >= 5:
            return True  # 5개 이상의 돌을 놓았으면 승리
        elif stone is Stone.B and count == 5:
            return True

    return False  # 승리 조건을 만족하지 않음
