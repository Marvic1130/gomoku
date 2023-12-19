import socket
import sys
import time
import torch
import pickle
import socket
from Rules.Board import Board, Game
from Train.MCTS import MCTSPlayer
from Train.PolicyValueNet import PolicyValueNet, PolicyResNet
from Utils.parameters import C_PUCT, N_PLAYOUT


class Human(object):
    def __init__(self, server_mode=False, client_socket=None):
        self.player = None
        self.server_mode = server_mode
        self.client_socket = client_socket
    def set_player_ind(self, p):
        self.player = p

    def get_action(self, board):
        try:
            if self.server_mode:
                time.sleep(1)
                location = self.client_socket.recv(1024).decode()
                print(location)
                # self.client_socket.sendall(location.encode())

            else:
                print("돌을 둘 좌표를 입력하세요.")
                location = input()
            if isinstance(location, str):
                row, col = location.split(',')
                row = ord(row) - ord('a')
                col = int(col)-1
                location = [col, row]
                # location = [int(location[0], 16), int(location[1], 16)]
            move = board.location_to_move(location)

        except ValueError as e:
            sys.exit(e)
        except Exception as e:
            move = -1
            print(e)

        if move == -1 or move in board.states.keys():
            print(f"다시 입력하세요.")
            move = self.get_action(board)
        elif board.is_you_black() and (row, board.height-(col+1)) in board.unable_locations:
            print("금수 자리에 돌을 놓을 수 없습니다.")
            move = self.get_action(board)

        return move

    def __str__(self):
        return "Human {}".format(self.player)


def run(server_mode=False, client_socket=None):
    n = 5
    size = width = height = 15
    print(f"이 오목 인공지능은 {width}x{height} 환경에서 동작합니다.")

    if server_mode:
        hard = 650
    else:
        print("현재 가능한 난이도(정책망의 학습 횟수) 목록 : [ 2500, 5000, 7500, 10000, 12500, 15000, 17500, 20000 ]")
        print("난이도를 입력하세요.")
        hard = int(input())
    path = './save/alpha_0.2v'
    model_file = f'{path}/model_{size}/policy_{size}_{hard}.model'    # colab

    if server_mode:
        order = 0
    else:
        print("자신이 선공(흑)인 경우에 0, 후공(백)인 경우에 1을 입력하세요.")
        order = int(input())
        if order not in [0, 1]:
            return "강제 종료"

    board = Board(width=width, height=height, n_in_row=n)
    game = Game(board)

    # 이미 제공된 model을 불러와서 학습된 policy_value_net을 얻는다.
    # policy_value_net = PolicyValueNet(board_width=width, board_height=height)
    policy_value_net = PolicyResNet(board_width=width, board_height=height)
    policy_value_net.load_model(model_file)

    mcts_player = MCTSPlayer(policy_value_net.policy_value_fn, c_puct=C_PUCT, n_playout=N_PLAYOUT)
    human = Human(server_mode=server_mode, client_socket=client_socket)

    # start_player = 0 → 사람 선공 / 1 → AI 선공
    game.start_play(
        human, mcts_player, start_player=order, is_shown=1,
        server_mode=server_mode, client_socket=client_socket)


if __name__ == '__main__':
    run()
