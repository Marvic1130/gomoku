package com.gamza.gomoku.service;

import com.gamza.gomoku.dto.rank.RankingResponseDto;
import com.gamza.gomoku.entity.QUserEntity;
import com.gamza.gomoku.entity.UserEntity;
import com.gamza.gomoku.repository.UserRepository;
import com.querydsl.jpa.impl.JPAQueryFactory;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class RankService {
    private final JPAQueryFactory jpaQueryFactory;

    public Page<RankingResponseDto> rankingOrderByTier(int page) {
        PageRequest pageRequest = PageRequest.of(page-1,20);

        QUserEntity qUserEntity = QUserEntity.userEntity;

        List<UserEntity> userEntityOrderByRanking = jpaQueryFactory
                .selectFrom(qUserEntity)
                .orderBy(qUserEntity.score.desc(),qUserEntity.winRate.desc())
                .offset(pageRequest.getOffset())
                .limit(pageRequest.getPageSize())
                .fetch();

        // 쿼리를 통해 총 레코드 수를 가져옵니다.
        long total = jpaQueryFactory
                .select(qUserEntity.count())
                .from(qUserEntity)
                .fetchOne();

        // UserEntity 리스트를 RankingResponseDto 리스트로 변환합니다.
        List<RankingResponseDto> dtos = userEntityOrderByRanking.stream()
                .map(userEntity -> new RankingResponseDto(userEntity))
                .collect(Collectors.toList());

        // PageImpl을 사용하여 Page<RankingResponseDto> 객체를 생성하고 반환합니다.
        return new PageImpl<>(dtos, pageRequest, total);
    }

    public RankingResponseDto searchUserRankingForUserName(String userName) {
        QUserEntity qUserEntity = QUserEntity.userEntity;

        UserEntity result = jpaQueryFactory
                .selectFrom(qUserEntity)
                .where(qUserEntity.userName.eq(userName))
                .fetchOne(); // unique 값은 fetchOne 처리해줘야해

        if (result == null)
            return null;

        return new RankingResponseDto(result);
    }
}
