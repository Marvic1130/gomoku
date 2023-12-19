package com.gamza.gomoku.controller;

import com.gamza.gomoku.dto.rank.RankingResponseDto;
import com.gamza.gomoku.service.RankService;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.data.domain.Page;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@Tag(name = "랭킹 API")
public class RankController {
    private final RankService rankService;
    //스코어 순으로 정렬하고, winRate로 다시 정렬해요.
    //이 service는 queryDSL을 통해 작성합니다 :)

    /*
    이유 -> QueryDSL을 사용하면 복잡한 쿼리를 타입 안전하게 구성할 수 있으며, 동적 쿼리를 보다 쉽게 작성할 수 있습니다.
     QueryDSL은 복잡한 정렬 조건과 페이징 요구사항을 매우 깔끔하게 처리할 수 있게 해주는데,
     특히 여러 필드에 걸쳐 정렬을 수행해야 할 때 매우 유용합니다.
     */
    //queryDSL build.gradle에서 적용후 앱 빌드하면 gomoku.build.generated.querydsl에서 @Entity에 맞춰 파일이 생긴 걸 볼 수 있어요 :D
    //마지막으로 Application파일에 @EnableJpaAuditing과 밑의 추가설정 적어주면 마무리.

    @GetMapping("/ranking/tier")
    public Page<RankingResponseDto> rankingOrderByTier(@RequestParam int page) {
        return rankService.rankingOrderByTier(page);
    }
    @GetMapping("/ranking/search")
    public RankingResponseDto searchUserRankingForUserName(@RequestParam String userName) {
        return rankService.searchUserRankingForUserName(userName);
    }
}
