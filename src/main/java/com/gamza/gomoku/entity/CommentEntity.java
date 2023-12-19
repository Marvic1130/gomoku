package com.gamza.gomoku.entity;

import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;

@Entity
@NoArgsConstructor
@Getter
public class CommentEntity extends BaseTimeEntity{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;
    @Column
    private String text;
    @Column
    private String userEmail;
    @ManyToOne
    @JoinColumn(name = "post", nullable = false)
    private PostEntity postEntity;

}
