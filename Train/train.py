from time import sleep

import torch
import torch.nn as nn
import torch.optim as optim
from torch.utils.data import Dataset, DataLoader
from sklearn.model_selection import train_test_split
from tqdm import tqdm
from Model import ResNet
import pandas as pd
import numpy as np

# 데이터 로드 및 전처리
csv_file_path = '../Data/gameData.csv'  # 데이터 파일 경로
df = pd.read_csv(csv_file_path)
board_size = 15
x = []  # 보드 상태를 저장할 목록
y = []  # 대상 움직임을 저장할 목록


def move_to_label(move, board_size):
    col, row = ord(move[0]) - ord('a'), int(move[1:]) - 1
    return row * board_size + col


for index, row in df.iterrows():
    moves = row['moves'].split()  # 움직임 데이터를 목록으로 분할
    result = row['bresult']  # 게임 결과 (0, 0.5, 또는 1)

    board = np.zeros((board_size, board_size), dtype=int)  # 게임 보드 초기화
    game_x = []  # 이 게임의 보드 상태를 저장할 목록
    game_y = []  # 이 게임의 대상 움직임을 저장할 목록

    for i, move in enumerate(moves):
        # 움직임 데이터 변환
        label = move_to_label(move, board_size)

        # 게임 결과를 확인하고 데이터를 이에 따라 수집합니다.
        if result == 0 and i % 2 == 0:
            game_x.append(list(board))  # 현재 보드 상태를 추가
            game_y.append(label)  # 변환된 움직임을 추가
        if result == 0.5:
            game_x.append(list(board))  # 현재 보드 상태를 추가
            game_y.append(label)  # 변환된 움직임을 추가
        if result == 1 and i % 2 == 1:
            game_x.append(list(board))  # 현재 보드 상태를 추가
            game_y.append(label)  # 변환된 움직임을 추가

        col, row = ord(move[0]) - ord('a'), int(move[1:]) - 1  # 움직임을 좌표로 변환

        if board[row, col] == 0:
            if i % 2 == 0:
                board[row, col] = 1
            else:
                board[row, col] = -1
        else:
            raise ValueError("이 위치에 이미 돌이 있습니다.")

    # 게임 데이터를 주 x 및 y 목록에 추가합니다.
    x.extend(game_x)
    y.extend(game_y)

# 데이터셋 및 데이터로더 설정
class CustomDataset(Dataset):
    def __init__(self, data, labels):
        self.data = data
        self.labels = labels

    def __len__(self):
        return len(self.data)

    def __getitem__(self, idx):
        x = torch.tensor(np.array(self.data[idx]), dtype=torch.float32).unsqueeze(0)  # 채널 차원 추가
        y = torch.tensor(self.labels[idx], dtype=torch.long)
        return x, y

# 데이터를 학습 및 검증 세트로 분할
x_train, x_val, y_train, y_val = train_test_split(x, y, test_size=0.2, random_state=42)

train_dataset = CustomDataset(x_train, y_train)
val_dataset = CustomDataset(x_val, y_val)

batch_size = 128
train_loader = DataLoader(train_dataset, batch_size=batch_size, shuffle=True)
val_loader = DataLoader(val_dataset, batch_size=batch_size, shuffle=False)

# ResNet 모델 빌드
in_channels = 1  # 입력 이미지 채널 수
num_classes = 15 * 15  # 클래스 수 (착수 위치 수)
resnet_model = ResNet(in_channels, num_classes)

# 모델 요약
print(resnet_model)

# 모델을 선택한 장치로 이동 (CPU 또는 GPU)
device = torch.device("cuda" if torch.cuda.is_available() else 'mps:0' if torch.backends.mps.is_available() else 'cpu')
resnet_model.to(device)

# 손실 함수 및 옵티마이저 설정
criterion = nn.CrossEntropyLoss()
optimizer = optim.Adam(resnet_model.parameters(), lr=0.001)

# 학습 루프
num_epochs = 10

for epoch in range(num_epochs):
    resnet_model.train()
    total_loss = 0.0

    for inputs, labels in tqdm(train_loader, desc=f"Epoch {epoch + 1}/{num_epochs}"):
        inputs, labels = inputs.to(device), labels.to(device)

        optimizer.zero_grad()

        outputs = resnet_model(inputs)
        loss = criterion(outputs, labels)
        loss.backward()
        optimizer.step()

        total_loss += loss.item()

    avg_loss = total_loss / len(train_loader)
    print(f"Epoch [{epoch + 1}/{num_epochs}] - Loss: {avg_loss:.4f}")
    sleep(1)

# 평가
resnet_model.eval()
correct = 0
total = 0

with torch.no_grad():
    for inputs, labels in val_loader:
        inputs, labels = inputs.to(device), labels.to(device)
        outputs = resnet_model(inputs)
        _, predicted = torch.max(outputs.data, 1)
        total += labels.size(0)
        correct += (predicted == labels).sum().item()

accuracy = 100 * correct / total
print(f"Validation Accuracy: {accuracy:.2f}%")
