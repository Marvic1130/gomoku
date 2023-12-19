package com.gamza.gomoku.dto.user;

import com.gamza.gomoku.entity.UserEntity;
import com.gamza.gomoku.enumcustom.Tier;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class UserInfoResponseDto {
    private String userName;
    private String userEmail;
    private Tier tier;
    private int score;
    private float winRate;
    private long totalPlay;
    private long totalWin;

    public UserInfoResponseDto(UserEntity userEntity) {
        this.userName = userEntity.getUserName();
        this.userEmail = userEntity.getUserEmail();
        this.tier = userEntity.getTier();
        this.score = userEntity.getScore();
        this.winRate = userEntity.getWinRate();
        this.totalPlay = userEntity.getTotalPlay();
        this.totalWin = userEntity.getTotalWin();
    }

    private float calWinRate() {
        if (totalPlay == 0) {return 0.0f;}
        return Math.round((float) totalPlay / totalPlay * 100) / 10.0f;
    }
}
