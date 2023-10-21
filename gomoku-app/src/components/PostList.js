import React from 'react';
import styled from 'styled-components';

const PostGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(3, 1fr); /* 세 개의 열 */
  grid-gap: 20px; /* 열과 행 사이의 간격 */
`;

const PostItem = styled.div`
  background-color: white;
  padding: 20px;
  border-radius: 10px;
`;

const PostList = () => {
  // 글 목록 임시 데이터
  const posts = [
    { id: 1, title: '글 1', content: '내용 1' },
    { id: 2, title: '글 2', content: '내용 2' },
    { id: 3, title: '글 3', content: '내용 3' },
    { id: 4, title: '글 4', content: '내용 4' },
    { id: 5, title: '글 5', content: '내용 5' },
    { id: 6, title: '글 6', content: '내용 6' },
    // 추가 글 목록 데이터
  ];

  return (
    <PostGrid>
      {posts.map((post) => (
        <PostItem key={post.id}>
          <h2>{post.title}</h2>
          <p>{post.content}</p>
        </PostItem>
      ))}
    </PostGrid>
  );
};

export default PostList;
