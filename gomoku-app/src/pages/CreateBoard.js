import React, { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

const PostList = ({ posts }) => {
  return (
    <PostGrid>
      {posts.map((post) => (
        <PostItemStyle key={post.id}>
          <PostTitleStyle>{post.title}</PostTitleStyle>
          <PostContentStyle>{post.content}</PostContentStyle>
        </PostItemStyle>
      ))}
    </PostGrid>
  );
};


const CreateBoard = () => {
  const [posts, setPosts] = useState([]);
  const [newPost, setNewPost] = useState({ title: '', content: '' });

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewPost((prev) => ({ ...prev, [name]: value }));
  };

  const handlePostSubmit = (e) => {
    e.preventDefault();
    setPosts((prevPosts) => [...prevPosts, newPost]);
    setNewPost({ title: '', content: '' });
  };

  return (
    <CommunityContainer>
      <CreatePostFormStyle>
        <h2>게시글 작성</h2>
        <input
          type="text"
          name="title"
          placeholder="제목을 작성해주세요"
          value={newPost.title}
          onChange={handleInputChange}
          style={{ marginBottom: '10px' }}
        />
        <textarea
          name="content"
          placeholder="내용을 작성해주세요"
          value={newPost.content}
          onChange={handleInputChange}
          style={{ marginBottom: '10px' }}
        />
        <ButtonWrapper>
          <button type="submit" onChange={handlePostSubmit}>등록</button>
          <a href="/community"><button >취소</button></a>

        </ButtonWrapper>
      </CreatePostFormStyle>
      <PostList posts={posts} />
    </CommunityContainer>
  );
};

export default CreateBoard;


const PostGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  grid-gap: 20px;
`;

const PostItemStyle = styled.div`
  padding: 10px;
  margin: 10px 0;
  background-color: white;
  border-radius: 5px;
  box-shadow: 0 0 3px rgba(0, 0, 0, 0.1);
`;

const PostTitleStyle = styled.h2`
  font-size: 1.5rem;
`;

const PostContentStyle = styled.p`
  font-size: 1rem;
`;

const CommunityContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;


const CreatePostFormStyle = styled.div`
  display: flex;
  flex-direction: column;
  width: 80%;
  margin: 2% auto;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 10px;
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
  input {
    font-size: 15px;
    height: 50px;
    padding: 5px;
  }
  textarea {
    font-size: 15px;
    height: 500px;
    resize: vertical;
    margin-bottom: 10px;
    padding: 10px;
  }
  button {
    background:black;
    color:white;
    width: 80px;
    height: 50px;
    border:grey;
    margin:3px;
  }
  button:hover{
    background:lightgrey;
    color:black;
  }
`;

const ButtonWrapper = styled.div`
 
  display: flex;
  justify-content: right;

`
