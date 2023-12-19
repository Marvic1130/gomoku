package com.gamza.gomoku.repository;

import com.gamza.gomoku.entity.CommentEntity;
import com.gamza.gomoku.entity.PostEntity;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CommentRepository extends JpaRepository<CommentEntity, Long> {
}
