package com.gamza.gomoku.repository;

import com.gamza.gomoku.entity.PostEntity;
import org.springframework.data.jpa.repository.JpaRepository;

public interface PostRepository extends JpaRepository<PostEntity, Long> {
}
