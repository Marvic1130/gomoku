import torch
import torch.nn as nn


class ResNetBlock(nn.Module):
    def __init__(self, in_channels, out_channels, kernel_size, stride):
        super(ResNetBlock, self).__init__()
        self.conv1 = nn.Conv2d(in_channels, out_channels, kernel_size, stride, padding=2)
        self.bn1 = nn.BatchNorm2d(out_channels)
        self.relu1 = nn.LeakyReLU(0.1)
        self.conv2 = nn.Conv2d(out_channels, out_channels, kernel_size, stride, padding=2)
        self.bn2 = nn.BatchNorm2d(out_channels)
        self.relu2 = nn.LeakyReLU(0.1)

    def forward(self, x):
        identity = x
        out = self.conv1(x)
        out = self.bn1(out)
        out = self.relu1(out)
        out = self.conv2(out)
        out = self.bn2(out)
        out += identity
        out = self.relu2(out)
        return out


class ResNet(nn.Module):
    def __init__(self, in_channels, num_classes):
        super(ResNet, self).__init__()
        self.conv = nn.Conv2d(in_channels, 64, kernel_size=5, stride=1, padding=2)
        self.bn = nn.BatchNorm2d(64)
        self.relu = nn.LeakyReLU(0.1)
        self.blocks = nn.ModuleList([ResNetBlock(64, 64, kernel_size=5, stride=1) for _ in range(10)])
        self.global_pool = nn.AdaptiveAvgPool2d((1, 1))
        self.fc = nn.Linear(64, num_classes)

    def forward(self, x):
        out = self.conv(x)
        out = self.bn(out)
        out = self.relu(out)
        for block in self.blocks:
            out = block(out)
        out = self.global_pool(out)
        out = out.view(out.size(0), -1)
        out = self.fc(out)
        return out


class BiLSTM(nn.Module):
    def __init__(self, input_size, hidden_size, num_layers, num_classes):
        super(BiLSTM, self).__init__()
        self.hidden_size = hidden_size
        self.num_layers = num_layers
        self.lstm = nn.LSTM(input_size, hidden_size, num_layers, batch_first=True, bidirectional=True)
        self.fc = nn.Linear(hidden_size * 2, num_classes)

    def forward(self, x):
        h0 = torch.zeros(self.num_layers * 2, x.size(0), self.hidden_size).to(x.device)
        c0 = torch.zeros(self.num_layers * 2, x.size(0), self.hidden_size).to(x.device)

        out, _ = self.lstm(x, (h0, c0))
        out = self.fc(out[:, -1, :])  # 마지막 시퀀스의 출력만 사용
        return out


class DQN(nn.Module):
    def __init__(self, input_size, num_actions):
        super(DQN, self).__init__()
        self.dense1 = nn.Linear(input_size, 64)
        self.relu1 = nn.ReLU()
        self.dense2 = nn.Linear(64, 32)
        self.relu2 = nn.ReLU()
        self.output_layer = nn.Linear(32, num_actions)

    def forward(self, inputs):
        x = self.relu1(self.dense1(inputs))
        x = self.relu2(self.dense2(x))
        return self.output_layer(x)

class CNN(nn.Module):
    def __init__(self, in_channels, num_classes):
        super(CNN, self).__init__()
        self.conv1 = nn.Conv2d(in_channels, 32, kernel_size=5, stride=1, padding=2)
        self.bn1 = nn.BatchNorm2d(32)
        self.relu1 = nn.ReLU()
        self.conv2 = nn.Conv2d(32, 64, kernel_size=5, stride=1, padding=2)
        self.bn2 = nn.BatchNorm2d(64)
        self.relu2 = nn.ReLU()
        self.conv3 = nn.Conv2d(64, 128, kernel_size=5, stride=1, padding=2)
        self.bn3 = nn.BatchNorm2d(128)
        self.relu3 = nn.ReLU()
        self.conv4 = nn.Conv2d(128, 64, kernel_size=5, stride=1, padding=2)
        self.bn4 = nn.BatchNorm2d(64)
        self.relu4 = nn.ReLU()
        self.global_pool = nn.AdaptiveAvgPool2d((1, 1))
        self.fc = nn.Linear(128, num_classes)

    def forward(self, x):
        out = self.conv1(x)
        out = self.bn1(out)
        out = self.relu1(out)

        out = self.conv2(out)
        out = self.bn2(out)
        out = self.relu2(out)

        out = self.conv3(out)
        out = self.bn3(out)
        out = self.relu3(out)

        # out = self.conv4(out)
        # out = self.bn4(out)
        # out = self.relu4(out)

        out = self.global_pool(out)
        out = out.view(out.size(0), -1)
        out = self.fc(out)
        return out

# BiLSTM 모델 빌드
input_size = 15 * 15
hidden_size = 128  # LSTM hidden state 크기
num_layers = 1  # LSTM 레이어 수
num_classes = 15 * 15  # 가능한 착수 위치의 수
bilstm_model = BiLSTM(input_size, hidden_size, num_layers, num_classes)

# 모델 요약
print(bilstm_model)


# 모델 생성
in_channels = 2  # 입력 이미지 채널 수
num_classes = 2  # 클래스 수 (예: 게임 결과 - 승, 패)
resnet_model = ResNet(in_channels, num_classes)

# 모델 요약
print(resnet_model)
