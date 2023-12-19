package com.gamza.gomoku.service;

import com.gamza.gomoku.dto.post.PostListResponseDto;
import com.gamza.gomoku.dto.post.PostRequestDto;
import com.gamza.gomoku.dto.post.PostResponseDto;
import com.gamza.gomoku.entity.PostEntity;
import com.gamza.gomoku.entity.QPostEntity;
import com.gamza.gomoku.entity.UserEntity;
import com.gamza.gomoku.error.ErrorCode;
import com.gamza.gomoku.error.execption.NotFoundException;
import com.gamza.gomoku.jwt.JwtProvider;
import com.gamza.gomoku.repository.PostRepository;
import com.gamza.gomoku.repository.UserRepository;
import com.querydsl.jpa.impl.JPAQueryFactory;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.PageRequest;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import javax.servlet.http.HttpServletRequest;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class PostService {
    private final PostRepository postRepository;
    private final UserRepository userRepository;
    private final JwtProvider jwtTokenProvider;
    private final JPAQueryFactory jpaQueryFactory;

    public ResponseEntity<String> createPost(PostRequestDto postRequestDto, HttpServletRequest request) {
        UserEntity userEntity = userRepository
                .findByUserEmail(jwtTokenProvider.getUserEmailOfAccessTokenByHttpRequest(request))
                .orElseThrow(()->{throw new NotFoundException(ErrorCode.NOT_FOUND_EXCEPTION.getMessage(), ErrorCode.NOT_FOUND_EXCEPTION);});

        PostEntity postEntity = new PostEntity().builder()
                .title(postRequestDto.getTitle())
                .writer(userEntity.getUserName())
                .text(postRequestDto.getText())
                .userEntity(userEntity)
                .commentEntityList(new ArrayList<>())
                .build();

        postRepository.save(postEntity);
        return ResponseEntity.ok("200ok");
    }
    public PostResponseDto getOnePostWithId(long id) {
        PostEntity postEntity = postRepository.findById(id).orElseThrow(()->{throw new NotFoundException(ErrorCode.NOT_FOUND_EXCEPTION.getMessage(), ErrorCode.NOT_FOUND_EXCEPTION);});
        return new PostResponseDto(postEntity);
    }
    public Page<PostListResponseDto> getPostList(int page) {
        PageRequest pageRequest = PageRequest.of(page-1,20);

        QPostEntity qPostEntity = QPostEntity.postEntity;
        List<PostEntity> queryResult = jpaQueryFactory
                .selectFrom(qPostEntity)
                .orderBy(qPostEntity.id.desc())
                .offset(pageRequest.getOffset())
                .limit(pageRequest.getPageSize())
                .fetch();

        long total = jpaQueryFactory
                .select(qPostEntity.count())
                .from(qPostEntity)
                .fetchOne();

        List<PostListResponseDto> result = queryResult.stream()
                .map(postEntity -> new PostListResponseDto(postEntity))
                .collect(Collectors.toList());

        return new PageImpl<>(result, pageRequest,total);
    }

    //이 API 대충 짰음!! 데이터 쌓일 시 O(n^2) 시간복잡도로 느려짐 리펙토링 요망
    public Page<PostListResponseDto> searchOfPostInTitleAndText(String key, int page) {
        PageRequest pageRequest = PageRequest.of(page-1,20);
        QPostEntity qPostEntity = QPostEntity.postEntity;


        List<PostEntity> queryResult = jpaQueryFactory
                .selectFrom(qPostEntity)
                .where(qPostEntity.title.like("%"+key+"%")
                        .or(qPostEntity.text.like("%"+key+"%")))
                .orderBy(qPostEntity.id.desc())
                .offset(pageRequest.getOffset())
                .limit(pageRequest.getPageSize())
                .fetch();

        long total = jpaQueryFactory
                .selectFrom(qPostEntity)
                .where(qPostEntity.title.like("%"+key+"%")
                        .or(qPostEntity.text.like("%"+key+"%")))
                .fetchCount();

        List<PostListResponseDto> result = queryResult.stream()
                .map(postEntity -> new PostListResponseDto(postEntity))
                .collect(Collectors.toList());

        // PageImpl 객체 생성 및 반환
        return new PageImpl<>(result, pageRequest, total);

    }
}
