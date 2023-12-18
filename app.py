import socket
import threading
import random
import json
import time
from flask import Flask, request, jsonify
from flask_cors import CORS
from Train.MCTS import MCTSPlayer
from Train.PolicyValueNet import PolicyResNet
from Utils.parameters import C_PUCT, N_PLAYOUT, TRAIN_PATH
from Rules.Board import Board

app = Flask(__name__)
CORS(app)
lock = threading.Lock()
servers = {}

class Server:
    def __init__(self, size=15, path=TRAIN_PATH):
        # self.host = ''
        # self.port = -1
        # AI 모델 초기화 및 가중치 로딩
        self.policy_value_net = PolicyResNet(board_width=size, board_height=size)
        self.policy_value_net.load_model('path_to_your_model_file.model')
        self.mcts_player = MCTSPlayer(self.policy_value_net.policy_value_fn, c_puct=C_PUCT, n_playout=N_PLAYOUT)

    def transform_board_state(self, board_state):
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

    def ai_predict(self, board_state, forbidden_locations, current_player):
        """ AI 모델을 사용하여 다음 수를 예측하는 함수. """
        # 보드 상태 변환
        board_state = self.transform_board_state(board_state)

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
        move = self.mcts_player.get_action(board, temp=0)
        return move

# 소켓 서버를 시작하는 함수

def start_server():
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind(('localhost', 0))
    s.listen(1)
    host, port = s.getsockname()
    print(f"Socket server started on {host} port {port}")

    servers[(host, port)] = {"server": s, "active": True, "thread": threading.current_thread()}

    client, addr = s.accept()
    print(f"{addr}에서 연결됨")
    data = client.recv(1024).decode('utf-8')
    game_data = json.loads(data)
    dic = {
        'command': 'init',
        'first': 'Player',
        'mode': 'Test',
        'size': 15,
        'game_id': 1
    }
    if game_data['command'] == 'init':
        size = game_data['size']
        first = game_data['first']
        game_id = game_data['game_id']
        if game_data['mode'] == 'Test':
            path = f"{TRAIN_PATH}/model_{size}/policy_{size}_650.model"
        else:
            client.sendall(f"Not Found {game_data['mode']} Mode".encode('utf-8'))
            servers[(host, port)]["active"] = False
            return s.close()
    else:
        client.sendall(f"Not Found {game_data['command']} Command".encode('utf-8'))
        servers[(host, port)]["active"] = False
        s.close()
    server = Server(size=size, path=path)
    if first == 'Player':
        data = client.recv(1024).decode('utf-8')

    while servers[(host, port)]["active"]:

        pass

    s.close()
    print(f"Socket server on {host} port {port} stopped")


# def start_server(host, port):
#     s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
#     s.bind((host, port))
#     s.listen()
#     print(f"서버가 {host}:{port}에서 실행 중입니다.")
#
#     while True:
#         client, addr = s.accept()
#         print(f"{addr}에서 연결됨")
#
#         data = client.recv(1024).decode('utf-8')
#         game_data = json.loads(data)
#         board_state = game_data['board_state']
#         forbidden_locations = game_data['forbidden_locations']
#         current_player = game_data['current_player']
#
#         next_move = ai_predict(board_state, forbidden_locations, current_player)
#
#         client.sendall(str(next_move).encode('utf-8'))
#         client.close()

@app.route('/start', methods=['get'])
def start():
    thread = threading.Thread(target=start_server)
    thread.start()
    time.sleep(1)
    with lock:
        host, port = list(servers.keys())[-1]
        return jsonify({"message": "Server is starting", "host": host, "port": port}), 200

    return jsonify({"error": "Failed to start the server"}), 500


if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5001, debug=True)

