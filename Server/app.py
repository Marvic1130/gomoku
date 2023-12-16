import socket
from flask import Flask, request, jsonify
import json
import random
from Train.MCTS import MCTSPlayer
from Train.PolicyValueNet import PolicyResNet
from Utils.parameters import C_PUCT, N_PLAYOUT
from Rules.Board import Board

# AI 모델 초기화 및 가중치 로딩
policy_value_net = PolicyResNet(board_width=15, board_height=15)
policy_value_net.load_model('path_to_your_model_file.model')
mcts_player = MCTSPlayer(policy_value_net.policy_value_fn, c_puct=C_PUCT, n_playout=N_PLAYOUT)

def transform_board_state(board_state):
    """ 보드 상태 변환: 빈 공간(-1)을 0으로, 백돌(0)을 2로 변환 """
    transformed_state = {}
    for move, player in board_state.items():
        if player == -1:
            transformed_state[move] = 0
        elif player == 0:
            transformed_state[move] = 2
        else:
            transformed_state[move] = player
    return transformed_state

def ai_predict(board_state, forbidden_locations, current_player):
    """ AI 모델을 사용하여 다음 수를 예측하는 함수. """
    # 보드 상태 변환
    board_state = transform_board_state(board_state)

    # Board 객체 생성 및 초기화
    board = Board()
    board.init_board()
    board.states = board_state
    board.unable_locations = forbidden_locations
    board.current_player = current_player

    # 처음 3수 처리
    non_empty_spots = len([spot for spot in board_state.values() if spot != 0])
    if non_empty_spots < 3:
        if non_empty_spots == 0:
            return 112  # 첫 번째 수
        elif non_empty_spots == 1:
            last_move = next(move for move, player in board_state.items() if player != 0)
            position = [-16, -15, -14, -1, 1, 14, 15, 16]
            return last_move + random.choice(position)
        else:  # non_empty_spots == 2
            first_move = next(move for move, player in board_state.items() if player == 1)
            while True:
                position = [-31, -30, -29, -17, -16, -15, -14, -13, -2, -1, 1, 2, 13, 14, 15, 16, 17, 29, 30, 31]
                next_move = first_move + random.choice(position)
                if next_move not in board_state:
                    return next_move

    # MCTSPlayer를 사용하여 다음 수 예측
    move = mcts_player.get_action(board, temp=0)
    return move

# 소켓 서버를 시작하는 함수
def start_server(host, port):
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind((host, port))
    s.listen()
    print(f"서버가 {host}:{port}에서 실행 중입니다.")

    while True:
        client, addr = s.accept()
        print(f"{addr}에서 연결됨")

        data = client.recv(1024).decode('utf-8')
        game_data = json.loads(data)
        board_state = game_data['board_state']
        forbidden_locations = game_data['forbidden_locations']
        current_player = game_data['current_player']

        next_move = ai_predict(board_state, forbidden_locations, current_player)

        client.sendall(str(next_move).encode('utf-8'))
        client.close()

# 서버 시작
start_server('127.0.0.1', 65432)
