import { useState } from 'react';
import styled from 'styled-components';

const CommunityContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const PostList = styled.div`
  width: 80%;
  margin: 2% auto;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 10px;
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
`;

const PostItem = styled.div`
  padding: 10px;
  margin: 10px 0;
  background-color: white;
  border-radius: 5px;
  box-shadow: 0 0 3px rgba(0, 0, 0, 0.1);
`;

const PostTitle = styled.h2`
  font-size: 1.5rem;
`;

const PostContent = styled.p`
  font-size: 1rem;
`;

const SearchInput = styled.input`
  padding: 10px;
  margin: 10px;
  width: 80%;
  
  align-items: flex-end;
`;

const Community = () => {
    //예시 데이터
    //추후DB에서 가져오기
  const [posts, setPosts] = useState([
    { id: 1, title: '글1', content: '내용1' },
    { id: 2, title: '글2', content: '내용2' },
    { id: 3, title: '글3', content: '내용3' },
  ]);

  return (
    <CommunityContainer>
     {/* <h1>커뮤니티</h1> */}

      <PostList>
      <SearchInput
          type="text"
          placeholder="게시글을 검색해 주세요."
        />
        {posts.map((post) => (
          <PostItem key={post.id}>
            <PostTitle>{post.title}</PostTitle>
            <PostContent>{post.content}</PostContent>
          </PostItem>
        ))}
      </PostList>
    </CommunityContainer>
  );
};

export default Community;
