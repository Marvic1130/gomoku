from Rules.Board import Board, Stone
from Train.MCTS import mcts_search


def play_game():
    board = Board(size=15)  # 오목 게임 보드 생성
    num_iterations = 100  # MCTS 탐색 반복 횟수

    while not board.is_full():
        # MCTS를 사용하여 다음 수 예측
        best_next_board = mcts_search(board, num_iterations)

        # MCTS를 통해 선택된 보드 상태로 업데이트
        board = best_next_board

        # 보드 출력 (게임 시각화)
        board.show()

        while True:
            x, y = map(int, input("위치를 입력하세요:").split())
            state, _ = board.set(x, y, Stone.W)
            if state:
                break
            else:
                print("잘못된 위치입니다.")

        board.show()

        # 승자 확인
        for c in range(15):
            for r in range(15):
                if board.is_win(r, c, Stone.B):
                    print("흑돌 승리!")
                    return
                elif board.is_win(r, c, Stone.W):
                    print("백돌 승리!")
                    return
            else:
                continue

    if board.is_full():
        print("무승부!")

if __name__ == "__main__":
    play_game()