package com.gamza.gomoku.enumcustom;

import lombok.RequiredArgsConstructor;

@RequiredArgsConstructor
public enum UserRole {
    ADMIN("관리자",0),
    USER("유저",1);

    private final String title;
    private final int key;
}
