package com.gamza.gomoku.service;

import com.gamza.gomoku.dto.game.GameDto;
import com.gamza.gomoku.jwt.JwtProvider;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import javax.servlet.http.HttpServletRequest;
import javax.transaction.Transactional;

@Service
@Transactional
@RequiredArgsConstructor
public class GameService {

    private final JwtProvider jwtProvider;
    private final UserService userService;

    @Transactional
    public ResponseEntity<String> getGameData(GameDto gameDto, HttpServletRequest request){

            // 아래 두 줄의 순서를 바꾸어서 먼저 outcome을 가져오도록 합니다.
            String outcome = gameDto.getOutcome();
            String accessToken = jwtProvider.resolveAT(request);

            if (accessToken != null) {
                String userEmail = jwtProvider.getUserEmailFromToken(accessToken);
                userService.processGameData(userEmail, outcome);
                return ResponseEntity.ok("게임종료값 받음");
            } else {
                return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("토큰이 유효하지 않습니다.");
            }

    }
}
