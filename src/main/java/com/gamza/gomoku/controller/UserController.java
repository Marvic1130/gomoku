package com.gamza.gomoku.controller;

import com.gamza.gomoku.dto.user.LoginRequestDto;
import com.gamza.gomoku.dto.user.SignupRequestDto;
import com.gamza.gomoku.dto.user.SimpleUserInfoResponseDto;
import com.gamza.gomoku.dto.user.UserInfoResponseDto;
import com.gamza.gomoku.service.UserService;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@RestController
@RequiredArgsConstructor
@Tag(name = "로그인 & UserInfo API")
public class UserController {
    private final UserService userService;
    @PostMapping("/auth/register")
    public ResponseEntity<String> signUp(@RequestBody SignupRequestDto signupRequestDto, HttpServletResponse response) {
        return userService.signUp(signupRequestDto,response);
    }
    @PostMapping("/auth/login")
    public ResponseEntity<String> login(@RequestBody LoginRequestDto loginRequestDto, HttpServletResponse response) {
        return userService.login(loginRequestDto, response);
    }
    @GetMapping("/auth/refresh")
    public ResponseEntity<String> refreshAT(HttpServletRequest request, HttpServletResponse response) {
        return userService.refreshAccessToken(request,response);
    }
    @GetMapping("/user/info/simple")
    public SimpleUserInfoResponseDto simpleUserInfo(HttpServletRequest request) {
        return userService.getSimpleUserInfo(request);
    }

    @GetMapping("/user/info")
    public UserInfoResponseDto userInfo(HttpServletRequest request) {
        return userService.getUserInfo(request);
    }
}
