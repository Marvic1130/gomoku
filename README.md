# Unity Cross Platform Gomoku

> Unity로 Cross Platform 오목게임을 개발.

> 유저 1:1 대결 and 인공지능 대결.

### 프로젝트 파트 분배

|개발자|담당 파트|개발환경 및 언어|
|:------:|:---:|:---:|
|강예성|AI인공지능|TensorFlow, Python|
|박희준|클라이언트|Unity, C#|
|김지현|프론트|React, JavaScript|
|김경민|백앤드|Flask, Python|

## 게임 규칙

게임 규칙은 **Renju**룰을 따른다.

**Renju**룰은 아래와 같다.

1. 흑은 삼삼, 사사, 장목(육목, 칠목 등) 금지

2. 백은 장목도 승리조건으로 인정한다.

3. 처음 3수는 26주형중 하나를 고른다. 처음 수(흑)는 천원(바둑), 두 번째 수(백)는 천원을 중심으로 한 3x3 정사각형, 세 번째 수(흑)는 천원을 중심으로 5x5정사각형에 두는 배치를 주형이라고 하며 모두 26가지이다.

<img src='https://github.com/Marvic1130/gomoku/assets/63188145/6ccdff41-f9e8-4127-b4aa-b1b0b9b17144' width=300>
<img src='https://github.com/Marvic1130/gomoku/assets/63188145/0516d793-79bd-4481-8cd1-b98f18b17520' width=300>

***

## 게임 개발 구조도

<img width="674" alt="스크린샷 2023-09-26 오후 7 33 47" src="https://github.com/Marvic1130/gomoku/assets/63188145/479fedfa-d051-45cf-a4ca-23231c3b1d5f">

***

## 프로젝트 기획 및 개발 계획

### 1. 프로젝트 기획

#### Game Client

* 게임의 전체적인 UI/UX 디자인

* 게임 규칙 및 플레이어 대 플레이어 간의 상호작용 정의

* 게임 규칙 및 플레이어 대 AI 간의 상호작용 정의

<img width="300" alt="board" src="https://github.com/Marvic1130/gomoku/assets/63188145/6b313313-4e55-44ca-b494-e72605248567">

#### Back-End

* 서버 아키텍처 및 기술 스택 선정

* 클라이언트와의 통신 방식 등 선정

#### Front-End(Web)

* 웹 페이지에 시각화 요소 구상

* 전체적인 UI/UX 디자인 구상

* 메인페이지, 로그인-회원가입, 마이페이지, 랭킹, 커뮤니티, 전적검색 등 구현하기로 함

<img width=200 alt="login" src=https://github.com/Marvic1130/gomoku/assets/63188145/cabb2f8b-09ad-4abb-a3cc-37cdb9175cba>
<img width=200 alt="Home" src=https://github.com/Marvic1130/gomoku/assets/63188145/e5103a94-f773-4acf-bac0-7a2b84b403f2>
<img width=200 alt="Ranking" src=https://github.com/Marvic1130/gomoku/assets/63188145/bdd65d38-6d41-4c6c-bdce-0d347c942576>


* **테마 컬러: Black&White**

#### Machine Learning

* 오목 AI 개발에 적합한 강화학습 모델 선정

* 오목 AI 개발을 위한 환경 세팅

<br/>

### 2. 개발 계획

#### Game Client

* 게임 로직 구현 및 검수 (승리조건과 흑돌의 장목, 삼삼, 사사 등)

* 사용자 인터페이스 및 게임 상태 업데이트 구현

* 서버와의 통신을 위한 Socket.io 구현

#### Back-End

* 유니티 클라이언트와 api연결로 로그인/회원가입 구현

* Page내 랭킹, 커뮤니티, 전적검색 등 구현

#### Front-End(Web)

* Page - 랭킹, 커뮤니티, 전적검색 등 UI/UX 개발

#### Machine Learning

* 강화학습 모델 구현

* 플레이 데이터가 들어오면, 학습 모델로 예측한 최적의 수를 백엔드를 통해 Game Client 전송

<br/>

### 3. 테스트

#### Game Client

* 빌드 파일의 동작 검수

* 게임 내 버그 및 이상 동작 테스트

* UI/UX 검토 및 수정

* 딜레이와 메모리 사용량 체크 (temp)

#### Back-End

* 웹 페이지 통합 기능 테스트

* 보안 설정 및 서버 옵티마이징

#### Front-End(Web)

* 웹 페이지 통합 기능 테스트

#### Machine Learning

* 학습 로그를 분석하여 레이어 및 환경변수 최적화

* 에피소드 수 조절(오버피팅&언더피팅 방지, 최적화의 연장선)

* 일정 에피소드 수 마다 학습모델 저장(게임 AI 난이도 설정)

* 실제 게임 플레이로 게임 난이도 테스트
