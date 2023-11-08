import React from 'react';
import Banner from '../components/Banner';
import PostList from '../components/PostList';
import UserSection from '../components/UserSection';
import RankingSection from '../components/RankingSection';

import styled from 'styled-components';

const Main = () => {
  return (
    <PageContainer>
      <Banner />
      <ContentContainer>
        <MainContainer>
          <LeftContainer>
            <PostList />
          </LeftContainer>
          <RightContainer>
            <UserSection />
            <RankingSection />
          </RightContainer>
        </MainContainer>
      </ContentContainer>
    </PageContainer>
  );
};

export default Main;

const MainContainer = styled.div`
  display: flex;
`;

const LeftContainer = styled.div`
  flex: 2; /* 커뮤니티 페이지를 2/3로 설정 */
`;

const RightContainer = styled.div`
  flex: 1; /* 개인정보와 랭킹을 1/3로 설정 */
  display: flex;
  flex-direction: column;
  margin-left: 20px;
`;

const PageContainer = styled.div`
  background-color: #f2f2f2;
`;

const ContentContainer = styled.div`
  border-radius: 10px;
  background-color: white;
  padding: 20px;
  margin: 20px;
`;