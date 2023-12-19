package com.gamza.gomoku.jwt;


import com.gamza.gomoku.dto.user.TokenParsingUserInfo;
import com.gamza.gomoku.entity.UserEntity;
import com.gamza.gomoku.service.jwt.CustomUserDetailService;
import io.jsonwebtoken.*;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Component;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.transaction.Transactional;
import java.util.Date;

@Component
@RequiredArgsConstructor
@Transactional
public class JwtProvider {
    private final CustomUserDetailService customUserDetailService;
    @Value("${jwt.secretKey}")
    private String secretKey;

    @Value("${jwt.accessExpiration}")
    private long ATExpireTime;

    @Value("${jwt.refreshExpiration}")
    private long RTExpireTime ;


    public String createAT(UserEntity userEntity){
        return this.createToken(userEntity,ATExpireTime);
    }
    public String createRT(UserEntity userEntity){
        return this.createToken(userEntity,RTExpireTime);
    }


    public String createToken(UserEntity userEntity, long tokenTime){

        Claims claims = Jwts.claims().setSubject(userEntity.getUserEmail());
        //claims.put("roles",userEntity.getUserRole().toString());
        //claims.put("tier",userEntity.getTier().toString());
        claims.put("userName",userEntity.getUserName());

        Date date = new Date();

        return Jwts.builder()
                .setClaims(claims)
                .setIssuedAt(date)
                .setExpiration(new Date(date.getTime()+tokenTime))
                .signWith(SignatureAlgorithm.HS256, secretKey)
                .compact();
    }

    public String resolveAT(HttpServletRequest request) {
        String authorizationHeader = request.getHeader("Authorization");
        System.out.println("Authorization Header: " + authorizationHeader);
        if (request.getHeader("Authorization") != null){
            return request.getHeader("Authorization").substring(7);
        }
        return null;
    }
    public String resolveRT(HttpServletRequest request) {
        if (request.getHeader("Authorization") != null) {
            return request.getHeader("Authorization").substring(7);
        }
        return null;
    }


    public String getUserEmailFromToken(String token) {
        Claims claims = Jwts.parser()
                .setSigningKey(secretKey)
                .parseClaimsJws(token)
                .getBody();

        return claims.getSubject();
    }


    public boolean validateToken(String token) {
        try {
            Jwts.parser().setSigningKey(secretKey).parseClaimsJws(token);
            return true;
        } catch (MalformedJwtException e) {
            throw new MalformedJwtException("Invalid JWT token", e);
        } catch (ExpiredJwtException e) {
            throw new JwtExpiredException("JWT token has expired");
        } catch (UnsupportedJwtException e) {
            throw new UnsupportedJwtException("JWT token is unsupported", e);
        } catch (IllegalArgumentException e) {
            throw new IllegalArgumentException("JWT claims string is empty", e);
        } catch (io.jsonwebtoken.security.SignatureException e) {
            throw new SignatureException("JWT signature verification failed", e);
        }
    }

    public void setHeaderAT(HttpServletResponse response, String AT) {
        response.setHeader("Authorization","Bearer "+AT);
    }
    public void setHeaderRT(HttpServletResponse response, String RT) {
        response.setHeader("RefreshToken","Bearer "+RT);
    }

    public UsernamePasswordAuthenticationToken getAuthentication(String token) {
        UserDetails userDetails = customUserDetailService.loadUserByUsername(this.getUserEmail(token));
        return new UsernamePasswordAuthenticationToken(userDetails,"",userDetails.getAuthorities());
    }
    public String getUserEmail(String token) {

        return Jwts.parser().setSigningKey(secretKey).parseClaimsJws(token).getBody().getSubject();
    }
    public String getUserEmailOfAccessTokenByHttpRequest(HttpServletRequest request) {
        return this.getUserEmail(this.resolveAT(request));
    }
    public TokenParsingUserInfo getUserInfoOfAccessTokenByHttpRequest(HttpServletRequest request) {
        String AT = this.resolveAT(request);

        Claims claims = Jwts.parser()
                .setSigningKey(secretKey)
                .parseClaimsJws(AT)
                .getBody();

        return new TokenParsingUserInfo(claims);
    }
}
