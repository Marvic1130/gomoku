package com.gamza.gomoku.config;


import com.gamza.gomoku.jwt.JwtAuthorizationTokenFilter;
import lombok.RequiredArgsConstructor;
import org.springframework.context.annotation.Bean;
import org.springframework.http.HttpStatus;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.HttpStatusEntryPoint;
import org.springframework.security.web.authentication.www.BasicAuthenticationFilter;

@EnableWebSecurity
@RequiredArgsConstructor
public class SecurityConfig {
    private final JwtAuthorizationTokenFilter jwtAuthorizationTokenFilter;
    @Bean
    public SecurityFilterChain filterChain(HttpSecurity httpSecurity) throws Exception {
        httpSecurity
                .httpBasic().disable()
                .csrf().disable()
                .cors()
                .and()
                .headers().frameOptions().disable()
                .and()
                .formLogin().disable()
                .authorizeRequests()
                //권한 필요한 api들 여기에 넣기
                .antMatchers("/post/**").permitAll()
                .antMatchers("/auth/**").permitAll()
                .antMatchers("/game/**").permitAll()
                //나머지 요청에서는 제한없이 호출
                .anyRequest().permitAll()
                .and()
                .addFilterBefore(jwtAuthorizationTokenFilter, BasicAuthenticationFilter.class)
                .exceptionHandling()
                .authenticationEntryPoint(new HttpStatusEntryPoint(HttpStatus.UNAUTHORIZED));

        return httpSecurity.build();
    }
}
