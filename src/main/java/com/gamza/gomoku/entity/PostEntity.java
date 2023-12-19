package com.gamza.gomoku.entity;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;
import java.util.List;

@Entity
@NoArgsConstructor
@Builder
@AllArgsConstructor
@Getter
public class PostEntity extends BaseTimeEntity{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id", nullable = false)
    private Long id;
    @Column(nullable = false)
    private String title;
    @Column(nullable = false)
    private String writer;
    @Column(nullable = false)
    private String text;
    @ManyToOne
    @JoinColumn(name = "user", nullable = false)
    private UserEntity userEntity;
    @OneToMany(mappedBy = "postEntity",orphanRemoval = true)
    private List<CommentEntity> commentEntityList;

}
