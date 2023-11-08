import React, { useState } from 'react';
import styled from 'styled-components';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = () => {
    console.log('Email:', email);
    console.log('Password:', password);
  };

  return (
    <LoginContainer>
      <h2>Login</h2>
      <LoginInput
        type="email"
        placeholder="이메일을 입력하세요"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <LoginInput
        type="password"
        placeholder="비밀번호를 입력하세요"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <LoginButton onClick={handleLogin}>Login</LoginButton>
      <RegistLink href="/regist">회원이 아니십니까? 회원가입하기</RegistLink>
    </LoginContainer>
  );
};

export default Login;


const LoginContainer = styled.div`
  text-align: center;
  margin: 5% auto;
  width: 300px;
  padding: 5%;
  border: 1px solid #ccc;
  border-radius: 5px;
  background-color: white;
`;

const LoginInput = styled.input`
  width: 100%;
  margin-bottom: 10px;
  padding: 10px;
`;

const LoginButton = styled.button`
  background-color: black;
  color: white;
  border: none;
  padding: 5px 10px;
  cursor: pointer;
`;

const RegistLink = styled.a`
  display: block;
  color: black;
  margin-top: 10px;
  text-decoration: none;
  &:hover {
    text-decoration: underline;
  }
`;
