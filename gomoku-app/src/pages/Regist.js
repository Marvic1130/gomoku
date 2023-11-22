import React, { useState } from 'react';
import styled from 'styled-components';

const Regist = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [phonenumber, setPhonenumber] = useState('');
  const [selectedEmail, setSelectedEmail] = useState('');

  const handleReigst = () => {
    if (password === confirmPassword) {
      console.log('Email:', selectedEmail ? `${email}@${selectedEmail}` : email);
      console.log('Password:', password);
      console.log('Confirm Password:', confirmPassword);
    } else {
      alert('비밀번호가 일치하지 않습니다.');
    }
  };

  return (
    <RegistContainer>
      <h2>회원 가입</h2>
      <RegistInputBox>
        <RegistInputLabel>이메일</RegistInputLabel>
        <RegistInputField
          type="text"
          placeholder="이메일을 입력하세요"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </RegistInputBox>
      <RegistInputBox>
        <RegistInputLabel>비밀번호</RegistInputLabel>
        <RegistInputField
          type="password"
          placeholder="비밀번호를 입력하세요"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </RegistInputBox>
      <RegistInputBox>
        <RegistInputLabel>비밀번호 확인</RegistInputLabel>
        <RegistInputField
          type="password"
          placeholder="비밀번호를 다시 입력하세요"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
        />
      </RegistInputBox>
      <RegistInputBox>
        <RegistInputLabel>핸드폰 번호</RegistInputLabel>
        <RegistInputField
          type="text"
          placeholder="핸드폰 번호를 입력하세요"
          value={phonenumber}
          onChange={(e) => setPhonenumber(e.target.value)}
        />
      </RegistInputBox>
      <RegistButton onClick={handleReigst}>회원가입</RegistButton>
    </RegistContainer>
  );
};

export default Regist;


const RegistContainer = styled.div`
  text-align: center;
  margin: 5% auto;
  width: 300px;
  padding: 5%;
  border: 1px solid #ccc;
  border-radius: 5px;
  background-color: white;
`;

const RegistInputBox = styled.div`
  width: 100%;
  margin-bottom: 20px;
  display: flex;
  flex-direction: column;
`;

const RegistInputLabel = styled.label`
  text-align: left;
  margin-bottom: 5px;
`;

const RegistInputField = styled.input`
  width: calc(100% - 22px);
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
`;

const RegistButton = styled.button`
  background-color: black;
  color: white;
  border: none;
  padding: 5px 10px;
  width: 100px;
  height: 50px;
  cursor: pointer;
`;