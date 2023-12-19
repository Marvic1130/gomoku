package com.gamza.gomoku.enumcustom;

import lombok.Getter;
import lombok.RequiredArgsConstructor;

@Getter
@RequiredArgsConstructor
public enum Tier {
    UNRANKED("언랭크",0,99),
    BRONZE("브론즈",100,199),
    SILVER("실버",200,299),
    GOLD("골드",300,399),
    DIAMOND("다이아",400,599),
    MASTER("마스터",600,-1);

    private final String title;
    private final int minScore;
    private final int maxScore;
}
