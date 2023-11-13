import React, { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

const Navbar = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false); // 로그인 상태

  const handleLogin = () => {
    setIsLoggedIn(true); // 로그인 처리
  };

  return (
    <NavbarContainer>
      <Link to="/main">
        <Logo src="/go_logo.png" alt="Logo" />
      </Link>
      <NavList>
        <NavItem>
          <NavLink href="/main">홈</NavLink>
        </NavItem>
        <NavItem>
          <NavLink href="/ranking">랭킹</NavLink>
        </NavItem>
        <NavItem>
          <NavLink href="/community">커뮤니티</NavLink>
        </NavItem>
        <NavItem>
          <NavLink href="/mypage">마이페이지</NavLink>
        </NavItem>
      </NavList>
      {!isLoggedIn && (
        <Link to="/login">
          <LoginButton onClick={handleLogin}>Login</LoginButton>
        </Link>
      )}
    </NavbarContainer>
  );
};

export default Navbar;

const NavbarContainer = styled.nav`
  background-color: black;
  color: white;
  padding: 30px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const Logo = styled.img`
  width: 180px;
  margin-left: 10px;
`;

const NavList = styled.div`
  display: flex;
  list-style: none;
  margin: 0;
  padding: 0;
  margin-left: auto;
  cursor: pointer;

  &:not(:hover) a{
    color: white;
  }
`;

const NavItem = styled.div`
  margin-right: 20px;
  display: flex;
  align-items: center;
  font-size: 15px;
  &:hover a{
    color: white;
  }
`;

const NavLink = styled.a`
  text-decoration: none;
  color: grey;
`;

const LoginButton = styled.button`
  background-color: white;
  color: black;
  border: none;
  border-radius: 10px;
  padding: 10px 20px;
  cursor: pointer;
  font-size: 15px;
  text-decoration: none;

  &:hover {
    background: black;
    color: white;
  }
`;
