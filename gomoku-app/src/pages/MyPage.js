import React from 'react';
import styled from 'styled-components';
import data from '../data/data.json';


class MyPage extends React.Component {

  handleEditInfo = () => {
    // 정보 수정 기능 구현 하기ㅎ..
  };
  

  render() {
    const userData  = data.user[0];
    return (
      <MyPageContainer>
        <h2>마이페이지</h2>
        <UserInfoWrapper>
            <UserImage src="https://via.placeholder.com/150" alt="프로필 이미지" />
          <UserInfoList>
            <UserInfoItem>이름: {userData.name}</UserInfoItem>
            <UserInfoItem>이메일: {userData.email}</UserInfoItem>
            <UserInfoItem>전화번호: {userData.phoneNumber}</UserInfoItem>
            <UserInfoItem>티어: {userData.tier}</UserInfoItem>
            <UserInfoItem>승률: {userData.winRate}</UserInfoItem>
          </UserInfoList>
        </UserInfoWrapper>
        <EditButton onClick={this.handleEditInfo}>정보 수정</EditButton>
      </MyPageContainer>
    );
  }
}

export default MyPage;



const UserInfoWrapper = styled.div`
  display: flex;
  align-items: center;
  gap: 100px;
`;

const UserInfoList = styled.div`
  display: flex;
  flex-direction: column;
  align-items: flex-start;
`;

const MyPageContainer = styled.div`
  text-align: left;
  margin: 5% auto;
  width: 80%;
  max-width: 800px;
  padding: 5%;
  border-radius: 10px;
  background-color: white;
  box-shadow: 0px 1px 10px 10px rgba(0, 0, 0, 0.05);  
`;

const UserImage = styled.img`
  width: 150px;
  height: 150px;
  border-radius: 50%;
  object-fit: cover;
`;

const UserInfoItem = styled.p`
  margin: 15px 0;
`;

const EditButton = styled.button`
  background-color: black;
  color: white;
  padding: 10px 20px;
  margin-left:700px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
  &:hover {
    background-color: #808080;
  }
`;