import React from 'react';
import Banner from '../components/Banner';
import PostList from '../components/PostList';

import styled from 'styled-components';

const PageContainer = styled.div`
  background-color: #f2f2f2;
`;

const ContentContainer = styled.div`
  border-radius: 10px;
  background-color: white;
  padding: 20px;
  margin: 20px;
`;

const Main = () => {
  return (
    <PageContainer>
      <Banner />
      <ContentContainer>
        {/*글 목록 컴포넌트 */}
        <PostList />
      </ContentContainer>
    </PageContainer>
  );
};

export default Main;
