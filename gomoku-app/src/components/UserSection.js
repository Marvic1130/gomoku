import React from 'react';
import styled from 'styled-components';
import userdata from '../data/data.json';

// 임의의 이미지 URL
const dummyImageUrl = 'https://via.placeholder.com/150';

const UserSection = () => {
  const user = userdata.user[0]; // 첫 번째 사용자 데이터 가져오기

  return (
    <UserContainer>
      <UserInfo>
        <UserImage src={dummyImageUrl} alt="User Avatar" />
        <UserName>{user.name}</UserName>
        <UserDetails>
          <UserDetail>
            <DetailLabel>Tier</DetailLabel>
            <DetailValue>{user.tier}</DetailValue>
          </UserDetail>
          <UserDetail>
            <DetailLabel>Win Rate</DetailLabel>
            <DetailValue>{user.winRate}</DetailValue>
          </UserDetail>
        </UserDetails>
      </UserInfo>
    </UserContainer>
  );
};

export default UserSection;

const UserContainer = styled.div`
  background-color: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0px 1px 10px 10px rgba(0, 0, 0, 0.05);  
`;

const UserInfo = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const UserImage = styled.img`
  width: 80px;
  height: 80px;
  border-radius: 50%;
  margin-bottom: 10px;
`;

const UserName = styled.h3`
  margin-bottom: 5px;
  color: #333;
`;

const UserDetails = styled.div`
  display: flex;
  justify-content: center;
`;

const UserDetail = styled.div`
  margin: 0 10px;
  text-align: center;
`;

const DetailLabel = styled.p`
  color: #666;
  margin: 0;
`;

const DetailValue = styled.p`
  color: #333;
  font-weight: bold;
  margin: 0;
`;
