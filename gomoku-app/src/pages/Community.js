import { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom'; 
import data from '../data/data.json';

const Community = () => {
  const post = data.post;

  return (
    <CommunityContainer>
      <PostList>
        <SearchInput
          type="text"
          placeholder="게시글을 검색해 주세요."
        />
        {post.map((post) => (
          <PostItem key={post.id}>
            <PostTitle>{post.title}</PostTitle>
            <PostContent>{post.content}</PostContent>
          </PostItem>
        ))}
      </PostList>
      <CreateBoardButton to="/createboard">게시글 작성</CreateBoardButton>
    </CommunityContainer>
  );
};

export default Community;



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
`;

const CreateBoardButton = styled(Link)`
  position: fixed;
  bottom: 50px;
  right: 100px;
  padding: 10px 20px;
  background: black;
  color: white;
  border: none;
  border-radius: 10px;
  text-decoration: none;
`;