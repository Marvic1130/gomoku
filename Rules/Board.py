# -*- coding: utf-8 -*-
import socket
import sys

import numpy as np
from Rules.Rules import Rules
from IPython.display import clear_output
import os
import random
from Utils.parameters import TEMPERATURE


class Board(object):
    def __init__(self, **kwargs):
        self.width = int(kwargs.get('width', 15))
        self.height = int(kwargs.get('height', 15))
        self.n_in_row = int(kwargs.get('n_in_row', 5))
        self.players = [1, 2]  # player1 and player2

    def init_board(self, start_player=0):
        self.order = start_player  # order = 0 → 사람 선공(흑돌) / 1 → AI 선공(흑돌)
        self.current_player = self.players[start_player]  # current_player = 1 → 사람 / 2 → AI
        self.last_move, self.last_loc = -1, -1

        self.states, self.states_loc = {}, [[0] * self.width for _ in range(self.height)]
        self.unable_locations, self.unable_moves = [], []

    def move_to_location(self, move):
        """ 3*3 보드를 예로 들면 : move 5 는 좌표 (2,1)를 의미한다."""  # ex) 6 7 8
        h = self.height - 1 -(move // self.width)  # 3 4 5
        w = move % self.width  # 0 1 2
        return [h, w]

    def location_to_move(self, location):
        if len(location) != 2: return -1
        h, w = location[0], location[1]
        move = h * self.width + w
        if move not in range(self.width * self.height): return -1
        return move

    def current_state(self):
        """현재 플레이어의 관점에서 보드 상태(state)를 return한다.
        state shape: 4 * [width*height] """
        square_state = np.zeros((4, self.width, self.height))
        if self.states:
            moves, players = np.array(list(zip(*self.states.items())))
            move_curr = moves[players == self.current_player]
            move_oppo = moves[players != self.current_player]
            square_state[0][move_curr // self.width, move_curr % self.height] = 1.0  # 내가 둔 돌의 위치를 1로 표현
            square_state[1][move_oppo // self.width, move_oppo % self.height] = 1.0  # 적이 둔 돌의 위치를 1로 표현
            square_state[2][self.last_move // self.width, self.last_move % self.height] = 1.0  # 마지막 돌의 위치

        if len(self.states) % 2 == 0: square_state[3][:, :] = 1.0  # indicate the colour to play

        return square_state.copy()  # square_state를 복사하여 반환


    def do_move(self, move):
        self.states[move] = self.current_player
        loc = self.move_to_location(move)
        self.states_loc[loc[0]][loc[1]] = 1 if self.is_you_black() else 2
        self.current_player = (self.players[0] if self.current_player == self.players[1] else self.players[1])
        self.last_move, self.last_loc = move, loc

    def has_a_winner(self):
        width = self.width
        height = self.height
        states = self.states
        n = self.n_in_row

        # moved : 이미 돌이 놓인 자리들
        moved = list(self.states.keys())
        if len(moved) < self.n_in_row * 2 - 1: return False, -1

        for m in moved:
            h = m // height
            w = m % width
            player = states[m]

            if (w in range(width - n + 1) and
                    len(set(states.get(i, -1) for i in range(m, m + n))) == 1):
                return True, player

            if (h in range(height - n + 1) and
                    len(set(states.get(i, -1) for i in range(m, m + n * width, width))) == 1):
                return True, player

            if (w in range(width - n + 1) and h in range(height - n + 1) and
                    len(set(states.get(i, -1) for i in range(m, m + n * (width + 1), width + 1))) == 1):
                return True, player

            if (w in range(n - 1, width) and h in range(height - n + 1) and
                    len(set(states.get(i, -1) for i in range(m, m + n * (width - 1), width - 1))) == 1):
                return True, player

        return False, -1

    def game_end(self):
        win, winner = self.has_a_winner()
        if win:
            return True, winner
        elif len(self.states) == self.width * self.height:
            return True, -1
        return False, -1

    def get_current_player(self):
        return self.current_player

    def set_unable(self):
        # forbidden_locations : 흑돌 기준에서 금수의 위치
        rule = Rules(self.states_loc, self.width)
        if self.order == 0:
            self.unable_locations = rule.get_unable_points(stone=1)
        else:
            self.unable_locations = rule.get_unable_points(stone=2)
        self.unable_moves = [self.location_to_move(loc) for loc in self.unable_locations]

    def is_you_black(self):
        # order, current_player
        # (0,1) → 사람(흑돌)
        # (0,2) → AI(백돌)
        # (1,1) → 사람(백돌)
        # (1,2) → AI(흑돌)
        if self.order == 0 and self.current_player == 1:
            return True
        elif self.order == 1 and self.current_player == 2:
            return True
        else:
            return False


class Game(object):
    def __init__(self, board, **kwargs):
        self.board = board
        self.turn = []

    def graphic(self, board, player1, player2, server_mode=False, s: socket.socket=None):
        width = board.width
        height = board.height

        clear_output(wait=True)
        os.system('clear')
        # print("\033c\033[3J")

        print()
        if board.order == 0:
            print("흑돌(●) : 플레이어")
            print("백돌(○) : AI")
        else:
            print("흑돌(●) : AI")
            print("백돌(○) : 플레이어")
        print("--------------------------------")

        if board.current_player == 1:
            print("당신의 차례입니다.")
        else:
            print("AI가 수를 두는 중...")

        for i in range(height-1, -1, -1):
            print(f'{i+1:02d}', end=' ')
            for j in range(width):
                loc = (i) * width + j
                p = board.states.get(loc, -1)
                if p == player1:
                    print('● ' if board.order == 0 else '○ ', end='')
                elif p == player2:
                    print('○ ' if board.order == 0 else '● ', end='')
                elif board.is_you_black() and (j, 14-i) in board.unable_locations:
                    print('Ⅹ ', end='')
                else:
                    print('+ ', end='')
            print()
        print('   ', end='')
        for i in range(height):
            print(chr(ord('a') + i), end=' ')
        print()
        if board.last_loc != -1:
            print(f"마지막 돌의 위치 : ({chr(ord('a') + board.last_loc[1])},{15-board.last_loc[0]})")
        if server_mode and s is not None:
            s.sendall(f"{chr(ord('a') + board.last_loc[1])},{15-board.last_loc[0]}")

    def start_play(self, player1, player2, start_player=0, is_shown=1, server_mode=False, client_socket=None):
        self.board.init_board(start_player)
        p1, p2 = self.board.players
        player1.set_player_ind(p1)
        player2.set_player_ind(p2)
        players = {p1: player1, p2: player2}
        while True:
            # 흑돌일 때, 금수 위치 확인하기
            if self.board.is_you_black(): self.board.set_unable()
            if is_shown: self.graphic(self.board, player1.player, player2.player)

            current_player = self.board.get_current_player()
            player_in_turn = players[current_player]

            if current_player == 1:  # 사람일 때
                move = player_in_turn.get_action(self.board)
                self.turn.append(move)
            else:  # AI일 때
                if len(self.turn) == 0:
                    move = 112
                elif len(self.turn) == 1:
                    position = [-16, -15, -14, -1, 1, 14, 15, 16]
                    move = self.turn[0] + random.choice(position)
                elif len(self.turn) == 2:
                    while True:
                        position = [-31, -30, -29,
                                    -17, -16, -15, -14, -13,
                                    -2, -1, 1, 2,
                                    13, 14, 15, 16, 17,
                                    29, 30, 31]
                        move = self.turn[0] + random.choice(position)
                        if move not in self.turn:
                            break
                else:
                    move = player_in_turn.get_action(self.board)
                self.turn.append(move)
            self.board.do_move(move)
            end, winner = self.board.game_end()
            if end:
                if is_shown:
                    self.graphic(self.board, player1.player, player2.player, server_mode=server_mode, s=client_socket)
                    if winner != -1:
                        print("Game end. Winner is", players[winner])
                    else:
                        print("Game end. Tie")
                return winner, sys.exit()
            if current_player != 1 and server_mode and client_socket is not None:
                send_data = f"{chr(ord('a') + self.board.last_loc[1])},{15 - self.board.last_loc[0]}"
                print(send_data)
                client_socket.sendall(send_data.encode())

    def start_self_play(self, player, is_shown=0, temp=TEMPERATURE):
        """ 스스로 자가 대국하여 학습 데이터(state, mcts_probs, z) 생성 """
        self.board.init_board()
        p1, p2 = self.board.players
        states, mcts_probs, current_players = [], [], []
        while True:
            # 흑돌일 때, 금수 위치 확인하기
            if self.board.is_you_black(): self.board.set_unable()
            if is_shown: self.graphic(self.board, p1, p2)

            move, move_probs = player.get_action(self.board, temp=temp, return_prob=1)
            # store the data
            states.append(self.board.current_state())
            mcts_probs.append(move_probs)
            current_players.append(self.board.current_player)

            # perform a move
            self.board.do_move(move)

            end, winner = self.board.game_end()
            if end:
                # winner from the perspective of the current player of each state
                winners_z = np.zeros(len(current_players))
                if winner != -1:
                    winners_z[np.array(current_players) == winner] = 1.0
                    winners_z[np.array(current_players) != winner] = -1.0
                # reset MCTS root node
                player.reset_player()
                if is_shown:
                    self.graphic(self.board, p1, p2)
                    if winner != -1:
                        print("Game end. Winner is player:", winner)
                    else:
                        print("Game end. Tie")
                return winner, zip(states, mcts_probs, winners_z)
