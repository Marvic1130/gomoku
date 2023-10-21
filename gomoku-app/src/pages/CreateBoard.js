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

const CreatePostForm = styled.div`
  width: 80%;
  margin: 2% auto;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 10px;
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
`;

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
      <CreatePostForm>
        <h2>Create a new post</h2>
        <form onSubmit={handlePostSubmit}>
          <input
            type="text"
            name="title"
            placeholder="Title"
            value={newPost.title}
            onChange={handleInputChange}
          />
          <textarea
            name="content"
            placeholder="Write your post here"
            value={newPost.content}
            onChange={handleInputChange}
          />
          <button type="submit">Post</button>
        </form>
      </CreatePostForm>
      <PostList>
        {posts.map((post, index) => (
          <PostItem key={index}>
            <PostTitle>{post.title}</PostTitle>
            <PostContent>{post.content}</PostContent>
          </PostItem>
        ))}
      </PostList>
    </CommunityContainer>
  );
};

export default CreateBoard;
