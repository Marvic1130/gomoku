package com.gamza.gomoku.entity;

import com.gamza.gomoku.enumcustom.Tier;
import com.gamza.gomoku.enumcustom.UserRole;
import lombok.*;

import javax.persistence.*;
import java.util.UUID;

@Entity
@Builder
@AllArgsConstructor
@NoArgsConstructor
@Getter
public class UserEntity extends BaseTimeEntity{
    @Id
    private String uid;
    @Column(nullable = false,unique = true)
    private String userEmail;
    @Column(nullable = false,unique = true)
    private String userName;
    @Column(nullable = false)
    private String password;
    @Column
    private String phoneNumber;
    @Column(nullable = false)
    private UserRole userRole;
    @Column(nullable = false)
    private String refreshToken;
    @Column(nullable = false)
    private Tier tier;
    @Column
    private int score;
    @Column
    private long totalPlay;
    @Column
    private long totalWin;
    @Column
    private float winRate;

    public void setRefreshToken(String RT) {
        this.refreshToken = RT;
    }
    public void updateTotalWin() {
        this.totalWin += 1;
    }
    public void updateTotalPlay() {
        this.totalPlay += 1;
    }
}
