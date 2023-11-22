import random
import numpy as np
from collections import defaultdict, deque
from Rules.Board import Board, Game
from MCTS import MCTSPlayer
from PolicyValueNet import PolicyValueNet
from datetime import datetime
import pickle
from tqdm import tqdm
import torch
import torch.optim as optim
from torch.utils.data import Dataset, DataLoader
from time import sleep
from Utils.parameters import *

# 사용 가능한 GPU 설정
device = torch.device("cuda" if torch.cuda.is_available() else
                      "mps" if torch.backends.mps.is_available() else
                      "cpu")
# torch.autograd.set_detect_anomaly(True)


class SelfPlayDataset(Dataset):
    def __init__(self, data_buffer):
        self.data_buffer = list(data_buffer)

    def __len__(self):
        return len(self.data_buffer)

    def __getitem__(self, idx):
        states, mcts_probs, winners = self.data_buffer[idx]
        # 데이터를 GPU로 이동
        return torch.from_numpy(states).float().to(device), torch.from_numpy(mcts_probs).float().to(device), torch.from_numpy(
            np.array([winners])).float().to(device)

class TrainPipeline():
    def __init__(self):
        # 게임(오목)에 대한 변수들
        self.board_width, self.board_height = 15, 15
        self.n_in_row = 5
        self.board = Board(width=self.board_width, height=self.board_height, n_in_row=self.n_in_row)
        self.game = Game(self.board)

        # 학습에 대한 변수들
        # self.learn_rate = 2e-3 # parameters.py
        self.lr_multiplier = 1.0  # KL에 기반하여 학습 계수를 적응적으로 조정
        # self.temp = 1.0  # the temperature param parameters.py
        # self.n_playout = 400  # num of simulations for each move parameters.py
        # self.c_puct = 5 # parameters.py
        # self.buffer_size = 10000

        self.data_buffer = deque(maxlen=BUFFER_SIZE)
        # self.batch_size = 512  # mini-batch size : 버퍼 안의 데이터 중 512개를 추출
        # self.play_batch_size = 1
        # self.epochs = 5  # num of train_steps for each update
        # self.kl_targ = 0.02
        # self.check_freq = 25  # 지정 횟수마다 모델을 체크하고 저장. 원래는 100이었음.
        # self.game_batch_num = 3000  # 최대 학습 횟수
        self.train_num = 0  # 현재 학습 횟수

        # policy-value net에서 학습 시작
        self.policy_value_net = PolicyValueNet(self.board_width, self.board_height)

        self.mcts_player = MCTSPlayer(self.policy_value_net.policy_value_fn, c_puct=C_PUCT,
                                      n_playout=N_PLAYOUT, is_selfplay=1)

    def get_equi_data(self, play_data):
        """
        회전 및 뒤집기로 데이터set 확대
        play_data: [(state, mcts_prob, winner_z), ..., ...]
        """
        extend_data = []
        for state, mcts_porb, winner in play_data:
            for i in [1, 2, 3, 4]:
                # 반시계 방향으로 회전
                equi_state = np.array([np.rot90(s, i) for s in state])
                equi_mcts_prob = np.rot90(np.flipud(mcts_porb.reshape(self.board_height, self.board_width)), i)
                extend_data.append((equi_state, np.flipud(equi_mcts_prob).flatten(), winner))
                # 수평으로 뒤집기
                equi_state = np.array([np.fliplr(s) for s in equi_state])
                equi_mcts_prob = np.fliplr(equi_mcts_prob)
                extend_data.append((equi_state, np.flipud(equi_mcts_prob).flatten(), winner))

        return extend_data

    def collect_selfplay_data(self, n_games=1):
        """collect self-play data for training"""
        for i in range(n_games):
            winner, play_data = self.game.start_self_play(self.mcts_player, temp=TEMPERATURE)
            play_data = list(play_data)[:]
            self.episode_len = len(play_data)
            # 데이터를 확대
            play_data = self.get_equi_data(play_data)
            self.data_buffer.extend(play_data)  # deque의 오른쪽(마지막)에 삽입

    def policy_update(self):
        """update the policy-value net"""
        dataset = SelfPlayDataset(self.data_buffer)
        dataloader = DataLoader(dataset, batch_size=BATCH_SIZE, shuffle=True, num_workers=0)

        optimizer = optim.Adam(self.policy_value_net.parameters(), lr=LEARNING_RATE * self.lr_multiplier)
        value_loss = torch.nn.MSELoss()
        policy_loss = torch.nn.KLDivLoss()

        for i in range(EPOCHS):
            total_value_loss = 0.0
            total_policy_loss = 0.0
            total_entropy = 0.0

            for states, mcts_probs, winners in tqdm(dataloader):
                optimizer.zero_grad()

                states = states.float()
                mcts_probs = mcts_probs.float()
                winners = winners.float()

                old_probs, old_values = self.policy_value_net(states)
                new_probs, new_values = self.policy_value_net(states)

                # print(old_values.size())
                # print(winners.size())
                # print(old_values)
                # print(winners)

                winners = winners.view(-1).to(device)
                value_loss_val = value_loss(old_values, winners)
                policy_loss_val = policy_loss(torch.log(old_probs), new_probs)

                total_value_loss += value_loss_val.item()
                total_policy_loss += policy_loss_val.item()
                total_entropy += torch.mean(torch.sum(-(new_probs * torch.log(old_probs + 1e-10)), axis=1)).item()

                loss = value_loss_val + policy_loss_val

                loss.backward()
                optimizer.step()

            total_value_loss /= len(dataloader)
            total_policy_loss /= len(dataloader)
            total_entropy /= len(dataloader)

            kl = torch.mean(
                torch.sum(new_probs * (torch.log(new_probs + 1e-10) - torch.log(old_probs + 1e-10)), axis=1)).item()

            # D_KL diverges 가 나쁘면 빠른 중지
            if kl > KL_TARG * 4: break

        # learning rate를 적응적으로 조절
        if kl > KL_TARG * 2 and self.lr_multiplier > 0.1:
            self.lr_multiplier /= 1.5
        elif kl < KL_TARG / 2 and self.lr_multiplier < 10:
            self.lr_multiplier *= 1.5

        explained_var_old = 1 - np.var(winners.cpu().numpy() - old_values.view(-1).detach().cpu().numpy()) / np.var(winners.cpu().numpy())
        explained_var_new = 1 - np.var(winners.cpu().numpy() - new_values.view(-1).detach().cpu().numpy()) / np.var(winners.cpu().numpy())

        print(
            f"kl:{kl:5f}, lr_multiplier:{self.lr_multiplier:3f}, value_loss:{total_value_loss:8f}, policy_loss:{total_policy_loss}, entropy:{total_entropy:5f}, explained_var_old:{explained_var_old:3f}, explained_var_new:{explained_var_new:3f}")
        sleep(0.01)
        return total_value_loss, total_policy_loss

    def run(self):
        for i in tqdm(range(GAME_BATCH_NUM)):
            self.collect_selfplay_data(PLAY_BATCH_SIZE)
            self.train_num += 1
            print(f"batch i:{self.train_num}, episode_len:{self.episode_len}")

            if len(self.data_buffer) > BATCH_SIZE:
                value_loss, policy_loss = self.policy_update()

            # 현재 model의 성능을 체크, 모델 속성을 저장
            if (i + 1) % CHECK_FREQ == 0:
                print(f"★ {self.train_num}번째 batch에서 모델 저장 : {datetime.now()}")
                self.policy_value_net.save_model(f'{model_path}/policy_15_{self.train_num}.model')
                self.policy_value_net.cpu()
                pickle.dump(self, open(f'{train_path}/train_15_{self.train_num}.pickle', 'wb'), protocol=2)
                self.policy_value_net.to(device)
                sleep(0.01)

if __name__ == '__main__':
    print("15x15 환경에서 학습을 진행합니다.")
    train_path = "../save/train_15"
    model_path = "../save/model_15"

    init_num = int(input('현재까지 저장된 모델의 학습 수 : '))
    model_file = f'{model_path}/policy_15_{init_num}.model' if init_num != 0 else None
    if init_num == 0 or init_num == None:
        training_pipeline = TrainPipeline()
    else:
        with open(f'{train_path}/train_15_{init_num}.pickle', 'rb') as f:
            training_pipeline: TrainPipeline = pickle.load(f)
        # 객체를 GPU로 이동
            training_pipeline.policy_value_net.to(device)
    print(f"★ 학습시작 : {datetime.now()}")
    training_pipeline.run()
