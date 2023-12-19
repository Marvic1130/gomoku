package com.gamza.gomoku.dto.user;

import lombok.Getter;

@Getter
public class SignupRequestDto {
    private String userEmail;
    private String password;
    private String userName;
    private String phoneNumber;
}
