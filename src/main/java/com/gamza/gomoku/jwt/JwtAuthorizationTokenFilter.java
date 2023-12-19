package com.gamza.gomoku.jwt;

import com.gamza.gomoku.error.ErrorJwtCode;
import io.jsonwebtoken.ExpiredJwtException;
import io.jsonwebtoken.MalformedJwtException;
import io.jsonwebtoken.SignatureException;
import io.jsonwebtoken.UnsupportedJwtException;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.json.simple.JSONObject;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

@RequiredArgsConstructor
@Component
@Slf4j
public class JwtAuthorizationTokenFilter extends OncePerRequestFilter {
    private final JwtProvider jwtTokenProvider;
    @Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, FilterChain filterChain)
            throws ServletException, IOException {

        String path = request.getRequestURI();
        if (path.startsWith("/auth") || path.startsWith("/ranking")
                || path.contains("/swagger") || path.contains("/v3/api-docs")
                || path.startsWith("/game")
        ) {
            filterChain.doFilter(request, response);
            return;
        }

        String accessToken = jwtTokenProvider.resolveAT(request);
        ErrorJwtCode errorCode;

        if (accessToken == null || accessToken.trim().isEmpty()) {
            errorCode = ErrorJwtCode.EMPTY_TOKEN;
            setResponse(response,errorCode);
            return;
            // 위의 링크에 걸리지 않고 토큰이 없는 경우 에러처리
        }
        try {
            if ( jwtTokenProvider.validateToken(accessToken) ) {
                this.setAuthentication(accessToken);
            }
        } catch (MalformedJwtException e) {
            log.info("103 error");
            errorCode = ErrorJwtCode.INVALID_TOKEN;
            setResponse(response, errorCode);
            return;
        } catch (ExpiredJwtException e) {
            log.info("101 error");
            errorCode = ErrorJwtCode.EXPIRED_AT;
            setResponse(response, errorCode);
            return;
        } catch (UnsupportedJwtException e) {
            log.info("105 error");
            errorCode = ErrorJwtCode.INVALID_TOKEN;
            setResponse(response, errorCode);
            return;
        } catch (IllegalArgumentException e) {
            log.info("104 error");
            errorCode = ErrorJwtCode.EMPTY_TOKEN;
            setResponse(response, errorCode);
            return;
        } catch (SignatureException e) {
            log.info("106 error");
            errorCode = ErrorJwtCode.INVALID_TOKEN;
            setResponse(response, errorCode);
            return;
        } catch (RuntimeException e) {
            log.info("4006 error");
            errorCode = ErrorJwtCode.JWT_COMPLEX_ERROR;
            setResponse(response, errorCode);
            return;
        }

        filterChain.doFilter(request,response);
    }
    private void setAuthentication(String token) {
        //토큰에서 유저정보 빼기
        Authentication authentication = jwtTokenProvider.getAuthentication(token);
        //유저정보 뺀거 컨택스트에 저장
        SecurityContextHolder.getContext().setAuthentication(authentication);
    }
    private void setResponse(HttpServletResponse response, ErrorJwtCode errorCode) throws IOException {
        JSONObject json = new JSONObject();
        response.setContentType("application/json;charset=UTF-8");
        response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);

        json.put("code", errorCode.getCode());
        json.put("message", errorCode.getMessage());

        response.getWriter().print(json);
        response.getWriter().flush();
    }
}