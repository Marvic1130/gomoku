package com.gamza.gomoku.controller;

import com.gamza.gomoku.dto.game.GameDto;
import com.gamza.gomoku.dto.user.LoginRequestDto;
import com.gamza.gomoku.jwt.JwtProvider;
import com.gamza.gomoku.repository.UserRepository;
import com.gamza.gomoku.service.GameService;
import com.gamza.gomoku.service.UserService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@RestController
@RequiredArgsConstructor
public class GameController {
    private final UserService userService;
    private final GameService gameService;

    private final UserRepository userRepository;
    private final JwtProvider jwtProvider;




    @PostMapping("/game/login")
    public ResponseEntity<String> receiveJson(@RequestBody LoginRequestDto loginRequestDto, HttpServletResponse response
    ) {
        String userEmail = loginRequestDto.getUserEmail();
        String password = loginRequestDto.getPassword();
        ResponseEntity.ok("JSON received successfully");
        return userService.login(loginRequestDto, response);
    }

    @PostMapping("/game/end")
    public ResponseEntity<String> receiveGameInfo(@RequestBody GameDto gameDto, HttpServletRequest request) {
        return gameService.getGameData(gameDto, request);
        
    }
}
