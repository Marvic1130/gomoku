import numpy as np
import pandas as pd

def convert_moves(moves):
    board = np.zeros((board_size, board_size), dtype=int)
    turn = np.full((board_size, board_size), -1, dtype=int)

    for i, move in enumerate(moves):
        col, row = ord(move[0]) - ord('a'), int(move[1:]) - 1  # 이동을 좌표로 변환
        if board[row, col] == 0:  # 해당 위치에 돌이 없는 경우에만 놓기
            if i % 2 == 0:
                board[row, col] = 1  # 홀수 번째 수에는 흑돌을 나타내는 1을 해당 위치에 설정
            else:
                board[row, col] = -1  # 짝수 번째 수에는 백돌을 나타내는 -1을 해당 위치에 설정
            turn[row, col] = i  # 해당 위치에 착수 순서 저장
        else:
            raise ValueError("This location already exists.")

    # 상하반전 적용
    board = np.flipud(board)
    turn = np.flipud(turn)

    return board, turn

csv_file_path = '../Data/gameData.csv'
df = pd.read_csv(csv_file_path)

# 보드 크기 (15x15) 정의
board_size = 15

# CSV 데이터 처리
augmented_data = []

for index, row in df.iterrows():
    moves = row['moves'].split()  # 움직임 데이터를 리스트로 변환
    result = row['bresult']  # 게임 결과 (0 또는 1)
    # 게임 보드 초기화
    board, turn = convert_moves(moves)

    for i in range(4):  # 회전과 반전을 고려한 데이터 확장
        rotated_board = np.rot90(board, k=i)
        rotated_turn = np.rot90(turn, k=i)
        augmented_data.append([rotated_board, rotated_turn, result])

        flipped_board = np.fliplr(rotated_board)
        flipped_turn = np.fliplr(rotated_turn)
        augmented_data.append([flipped_board, flipped_turn, result])

# 데이터를 NumPy 배열로 변환
augmented_data = np.array(augmented_data, dtype=object)

# 데이터와 레이블로 분리
x = np.array(augmented_data[:, :2].tolist(), dtype=np.float32)
y = np.array(augmented_data[:, 2], dtype=np.float32)

print(x.shape)  # (N*8, 15, 15, 2)
print(y.shape)  # (N*8,)

print("첫 번째 데이터:")
print(x[0])
print("첫 번째 레이블:")
print(y[0])
