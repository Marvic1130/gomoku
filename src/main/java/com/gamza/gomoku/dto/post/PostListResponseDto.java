package com.gamza.gomoku.dto.post;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.gamza.gomoku.entity.PostEntity;
import lombok.Getter;

import java.time.LocalDateTime;

@Getter
public class PostListResponseDto {
    private long id;
    private String title;
    private String writer;
    @JsonFormat(pattern = "yyyy-MM-dd HH:mm")
    private LocalDateTime createDateTime;

    public PostListResponseDto(PostEntity postEntity) {
        this.id = postEntity.getId();
        this.title = postEntity.getTitle();
        this.writer = postEntity.getWriter();
        this.createDateTime = postEntity.getCreatedDate();
    }
}
