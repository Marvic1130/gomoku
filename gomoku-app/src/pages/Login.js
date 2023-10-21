import React, { useState } from 'react';
import styled from 'styled-components';

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

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = () => {
    console.log('Username:', username);
    console.log('Password:', password);
  };

  return (
    <LoginContainer>
      <h2>Login</h2>
      <LoginInput
        type="text"
        placeholder="이메일을 입력하세요"
        value={username}
        onChange={(e) => 
          setUsername(e.target.value)}
      />
      <LoginInput
        type="password"
        placeholder="비밀번호를 입력하세요"
        value={password}
        onChange={(e) =>
          setPassword(e.target.value)}
      />
      <LoginButton onClick={handleLogin}>Login</LoginButton>
    </LoginContainer>
  );
};

export default Login;
