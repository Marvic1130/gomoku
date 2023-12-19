package com.gamza.gomoku.service;

import com.gamza.gomoku.dto.game.GameDto;
import com.gamza.gomoku.dto.user.*;
import com.gamza.gomoku.entity.UserEntity;
import com.gamza.gomoku.enumcustom.Tier;
import com.gamza.gomoku.enumcustom.UserRole;
import com.gamza.gomoku.error.ErrorCode;
import com.gamza.gomoku.error.execption.DuplicateException;
import com.gamza.gomoku.error.execption.NotFoundException;
import com.gamza.gomoku.error.execption.UnAuthorizedException;
import com.gamza.gomoku.jwt.JwtProvider;
import com.gamza.gomoku.repository.UserRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.transaction.Transactional;
import java.util.UUID;


@Service
@Transactional
@RequiredArgsConstructor
public class UserService {
    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final JwtProvider jwtTokenProvider;

    public ResponseEntity<String> login(LoginRequestDto loginRequestDto, HttpServletResponse response){
        UserEntity userEntity = userRepository.findByUserEmail(loginRequestDto.getUserEmail())
                .orElseThrow(()-> new UnAuthorizedException(ErrorCode.INVALID_ACCESS.getMessage(), ErrorCode.INVALID_ACCESS));

        if ( !passwordEncoder.matches(loginRequestDto.getPassword(),userEntity.getPassword()) ) {
            throw new UnAuthorizedException(ErrorCode.INVALID_ACCESS.getMessage(),ErrorCode.INVALID_ACCESS);
        }

        String AT = jwtTokenProvider.createAT(userEntity);
        String RT = jwtTokenProvider.createRT(userEntity);

        userEntity.setRefreshToken(RT);

        response.setHeader("Authorization","Bearer " + AT);
        response.setHeader("RefreshToken","Bearer "+ RT);

        return ResponseEntity.ok("로그인 성공");
    }

    @Transactional
    public void processGameData(String userEmail, String outcome) {
        UserEntity userEntity = userRepository.findByUserEmail(userEmail)
                .orElseThrow(() -> new NotFoundException("User not found", ErrorCode.NOT_FOUND));

        userEntity.updateTotalPlay();

        if ("Win".equals(outcome)) {
            userEntity.updateTotalWin();
        }

    }

    @Transactional
    public ResponseEntity<String> signUp(SignupRequestDto signupRequestDto, HttpServletResponse response){
        if (userRepository.existsByUserEmail(signupRequestDto.getUserEmail())) {
            throw new DuplicateException(ErrorCode.DUPLICATE_EMAIL.getMessage(), ErrorCode.DUPLICATE_EMAIL);
        }
        UserEntity userEntity = UserEntity.builder()
                .uid(UUID.randomUUID().toString())
                .userName(signupRequestDto.getUserName())
                .userEmail(signupRequestDto.getUserEmail())
                .password(passwordEncoder.encode(signupRequestDto.getPassword()))
                .phoneNumber(signupRequestDto.getPhoneNumber())
                .userRole(UserRole.USER)
                .refreshToken("dummy")
                .tier(Tier.UNRANKED)
                .score(0)
                .totalPlay(0)
                .totalWin(0)
                .build();

        userRepository.save(userEntity);

        String AT = jwtTokenProvider.createAT(userEntity);
        String RT = jwtTokenProvider.createRT(userEntity);

        userEntity.setRefreshToken(RT);

        response.setHeader("Authorization","Bearer " + AT);
        response.setHeader("RefreshToken","Bearer "+ RT);

        return ResponseEntity.ok("회원가입 성공");
    }
    public ResponseEntity<String> refreshAccessToken(HttpServletRequest request, HttpServletResponse response) {
        String RT = jwtTokenProvider.resolveRT(request);

        if ( !jwtTokenProvider.validateToken(RT) ) {
            throw new UnAuthorizedException(ErrorCode.INVALID_TOKEN.getMessage(), ErrorCode.INVALID_TOKEN);
        }

        UserEntity userEntity = userRepository
                .findByUserEmail(jwtTokenProvider.getUserEmail(RT))
                .orElseThrow(()->{throw new NotFoundException(ErrorCode.NOT_FOUND_EXCEPTION.getMessage(), ErrorCode.NOT_FOUND_EXCEPTION);});

        if ( !(RT.equals(userEntity.getRefreshToken())) ) {
            throw new UnAuthorizedException("저장된 RT와 다릅니다.",ErrorCode.INVALID_TOKEN);
        }

        response.setHeader("Authorization","Bearer " + jwtTokenProvider.createAT(userEntity));
        return ResponseEntity.ok("good,check header");
    }

    //===========UserInfo 관련=======================

    public SimpleUserInfoResponseDto getSimpleUserInfo(HttpServletRequest request) {
        TokenParsingUserInfo userInfo =  jwtTokenProvider.getUserInfoOfAccessTokenByHttpRequest(request);
        return new SimpleUserInfoResponseDto(userInfo);
    }

    public UserInfoResponseDto getUserInfo(HttpServletRequest request) {
        UserEntity userEntity = userRepository
                .findByUserEmail(jwtTokenProvider.getUserEmailOfAccessTokenByHttpRequest(request))
                .orElseThrow(()->{throw new NotFoundException(ErrorCode.NOT_FOUND_EXCEPTION.getMessage(), ErrorCode.NOT_FOUND_EXCEPTION);});

        return new UserInfoResponseDto(userEntity);
    }

}
