package com.gamza.gomoku.dto.rank;

import com.gamza.gomoku.entity.UserEntity;
import com.gamza.gomoku.enumcustom.Tier;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class RankingResponseDto {
    private long ranking;
    private String userName;
    private Tier tier;
    private float winRate;

    public RankingResponseDto(UserEntity userEntity) {
        this.userName = userEntity.getUserName();
        this.tier = userEntity.getTier();
        this.winRate = userEntity.getWinRate();
    }
}
