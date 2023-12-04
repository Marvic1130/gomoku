from typing import Final

EXPLORATION_WEIGHT: Final = 1.0
EMPTY: Final = 0
LEARNING_RATE: Final = 2e-3
BUFFER_SIZE: Final = 10000
L2_CONST: Final = 1e-4

C_PUCT: Final = 5
N_PLAYOUT: Final = 400
TEMPERATURE: Final = 1.0
BATCH_SIZE: Final = 512   # mini-batch size : 버퍼 안의 데이터 중 512개를 추출
PLAY_BATCH_SIZE: Final = 1
EPOCHS: Final = 10
KL_TARG: Final = 0.02
CHECK_FREQ: Final = 25  # 지정 횟수마다 모델을 체크하고 저장. 원래는 100이었음.
GAME_BATCH_NUM: Final = 3000  # 최대 학습 횟수

SELF_PLAY_SHOW: Final = False

TRAIN_PATH: Final = "../save/alpha_0.2v"

