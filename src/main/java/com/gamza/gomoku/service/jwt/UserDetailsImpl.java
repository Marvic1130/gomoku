package com.gamza.gomoku.service.jwt;

import com.gamza.gomoku.entity.UserEntity;
import lombok.RequiredArgsConstructor;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

import javax.transaction.Transactional;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

@RequiredArgsConstructor
public class UserDetailsImpl implements UserDetails {
    private final UserEntity userEntity;
    @Override
    @Transactional
    public Collection<? extends GrantedAuthority> getAuthorities() {
        List<String> userRole = new ArrayList<>();
        userRole.add(userEntity.getUserRole().toString());
        String authority = userRole.toString();

        SimpleGrantedAuthority simpleAuthority = new SimpleGrantedAuthority(authority);
        Collection<GrantedAuthority> authorities = new ArrayList<>();
        authorities.add(simpleAuthority);

        return authorities;
    }

    @Override
    public String getPassword() {
        return userEntity.getUserEmail();
    }

    @Override
    public String getUsername() {
        return userEntity.getUserName();
    }

    @Override
    public boolean isAccountNonExpired() {
        return true;
    }

    @Override
    public boolean isAccountNonLocked() {
        return true;
    }

    @Override
    public boolean isCredentialsNonExpired() {
        return true;
    }

    @Override
    public boolean isEnabled() {
        return true;
    }
}
