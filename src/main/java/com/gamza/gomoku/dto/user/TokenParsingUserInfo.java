package com.gamza.gomoku.dto.user;

import io.jsonwebtoken.Claims;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class TokenParsingUserInfo {
    private String userRole;
    private String tier;
    private String userName;

    public TokenParsingUserInfo (Claims claims) {
        this.userRole = claims.get("roles", String.class);
        this.tier = claims.get("tier", String.class);
        this.userName = claims.get("userName", String.class);
    }
}
