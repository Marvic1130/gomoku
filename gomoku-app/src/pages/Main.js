import React, { useState, useEffect } from 'react';
import Banner from '../components/Banner';
import PostList from '../components/PostList';
import UserSection from '../components/UserSection';
import RankingSection from '../components/RankingSection';
import axios from 'axios';
import styled from 'styled-components';

const Main = () => {
  // const [responseData, setResponseData] = useState(null);

  // useEffect(() => {
  //   axios.get('/main')
  //     .then(response => {
  //       setResponseData(response.data);
  //     })
  //     .catch(error => {
  //       console.error('Error fetching data:', error);
  //     });
  // }, []);
  return (
    <PageContainer>
      <Banner/>
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
  flex: 2;
  margin-right: 20px;
`;

const RightContainer = styled.div`
  flex: 1;
  display: flex;
  flex-direction: column;
  margin-left: 20px;
`;

const PageContainer = styled.div`
  background-color: #fbfbfb;
`;

const ContentContainer = styled.div`
  
  padding: 20px;
  margin: 20px;
`;
