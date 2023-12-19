package com.gamza.gomoku.dto.post;

import com.gamza.gomoku.entity.PostEntity;
import lombok.Getter;
import lombok.Setter;

import java.time.LocalDateTime;

@Getter
@Setter
public class PostResponseDto {
    private long id;
    private String title;
    private LocalDateTime createTime;
    private String writer;
    private String text;

    public PostResponseDto(PostEntity postEntity) {
        this.id = postEntity.getId();
        this.title = postEntity.getTitle();
        this.createTime = postEntity.getCreatedDate();
        this.writer = postEntity.getWriter();
        this.text = postEntity.getText();
    }
}
