import React, { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

const Navbar = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(true); // 로그인 상태

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
        {!isLoggedIn && (
          <NavItem>
            <Link to="/login">
              <LoginButton onClick={handleLogin}>Login</LoginButton>
            </Link>
          </NavItem>
        )}
      </NavList>
    </NavbarContainer>
  );
};

export default Navbar;

const NavbarContainer = styled.nav`
  background-color: #222;
  color: white;
  padding: 10px 0;
  display: flex;
  justify-content: space-between;
  padding: 15px;
  align-items: center;

`;

const Logo = styled.img`
  width: 150px;
  margin-left: 10px;
`;

const NavList = styled.ul`
  list-style: none;
  display: flex;
  padding: 0;
  margin-right: 20px;
`;

const NavItem = styled.li`
  margin: 0;
  padding: 0;
  margin-right: 20px;
`;

const NavLink = styled.a`
  text-decoration: none;
  color: white;
  &:hover {
    text-decoration: underline;
  }
`;

const LoginButton = styled.button`
  background-color: white;
  color: black;
  border: none;
  padding: 5px 10px;
  cursor: pointer;
`;
