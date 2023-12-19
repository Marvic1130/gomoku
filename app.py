import socket
import threading
import json
import time
from flask import Flask, request, jsonify
from flask_cors import CORS
from Play import run

app = Flask(__name__)
CORS(app)
lock = threading.Lock()
servers = {}  # 서버 및 클라이언트 소켓을 관리하는 딕셔너리

# 소켓 서버를 시작하는 함수
def socket_server_thread(s, host, port):
    with lock:
        servers[(host, port)] = {"server": s, "clients": [], "active": True}
    print(f"Socket listening on {host} port {port}")

    while servers[(host, port)]["active"]:
        try:
            client_socket, addr = s.accept()
            print(f"Connected by {addr}")
            with lock:
                servers[(host, port)]["clients"].append(client_socket)  # 클라이언트 소켓 추가
            while True:
                run(server_mode=True, client_socket=client_socket)
        except socket.error:
            break

    # 서버 종료 시 클라이언트 소켓들을 닫음
    with lock:
        for client in servers[(host, port)]["clients"]:
            client.close()
    s.close()
    with lock:
        del servers[(host, port)]
    print(f"Socket server on {host} port {port} stopped")

# 서버 시작 함수
def start_server():
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind(('0.0.0.0', 0))
    s.listen(1)
    host = '192.168.0.14'
    _, port = s.getsockname()
    threading.Thread(target=socket_server_thread, args=(s, host, port)).start()

@app.route('/start', methods=['get'])
def start():
    thread = threading.Thread(target=start_server)
    thread.start()
    time.sleep(1)
    with lock:
        host, port = list(servers.keys())[-1]
        return jsonify({"message": "Server is starting", "host": host, "port": port}), 200

    return jsonify({"error": "Failed to start the server"}), 500

@app.route('/list')
def list_servers():
    with lock:
        active_servers = [{"host": host, "port": port} for (host, port), server in servers.items() if server["active"]]
    return jsonify(active_servers), 200

@app.route('/stop', methods=['DELETE'])
def stop():
    data = request.json
    host, port = data['host'], int(data['port'])
    with lock:
        if (host, port) in servers and servers[(host, port)]["active"]:
            servers[(host, port)]["active"] = False
            # 클라이언트 소켓들을 닫음
            for client in servers[(host, port)]["clients"]:
                client.close()
            return jsonify({"message": "Server stopping"}), 200
        else:
            return jsonify({"error": "Server not found"}), 404

@app.route('/shutdown', methods=['DELETE'])
def shutdown():
    with lock:
        for key, server in servers.items():
            if server["active"]:
                server["active"] = False
                # 연결된 클라이언트 소켓들을 닫음
                for client in server["clients"]:
                    client.close()
                # 서버 소켓을 닫음
                server["server"].close()
        servers.clear()  # 서버 목록 초기화
    return jsonify({"message": "All servers shutting down"}), 200

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5001, debug=True)
