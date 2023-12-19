package com.gamza.gomoku.dto.user;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class SimpleUserInfoResponseDto {
    private String userName;
    private String tier;

    public SimpleUserInfoResponseDto(TokenParsingUserInfo tokenParsingUserInfo) {
        this.userName = tokenParsingUserInfo.getUserName();
        this.tier = tokenParsingUserInfo.getTier();
    }
}
