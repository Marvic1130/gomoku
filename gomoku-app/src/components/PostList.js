import React, { useState } from 'react';
import styled from 'styled-components';
import data from '../data/data.json';

const pageSize = 3; // 페이지 당 표시할 항목 수

const PostList = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const posts = data.post;

  const indexOfLastPost = currentPage * pageSize;
  const indexOfFirstPost = indexOfLastPost - pageSize;
  const currentPosts = posts.slice(indexOfFirstPost, indexOfLastPost);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  return (
    <div>
      <PostGrid>
        {currentPosts.map((post) => (
          <PostItem key={post.id}>
            <h2>{post.title}</h2>
            <p>{post.content}</p>
          </PostItem>
        ))}
      </PostGrid>

      <Pagination>
        {Array.from({ length: Math.ceil(posts.length / pageSize) }, (_, index) => (
          <PageNumber
            key={index}
            onClick={() => paginate(index + 1)}
            className={currentPage === index + 1 ? "active" : ""}
          >
            {index + 1}
          </PageNumber>
        ))}
      </Pagination>
    </div>
  );
};

export default PostList;

const PostGrid = styled.div`
  display: grid;
  grid-template-columns: 1fr; 
  grid-gap: 20px; 
`;

const PostItem = styled.div`
  background-color: white;
  padding: 15px; 
  border-radius: 10px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-bottom: 30px; 
`;

const Pagination = styled.div`
  display: flex;
  justify-content: center;
`;

const PageNumber = styled.div`
  border: 1px solid #ccc;
  border-radius: 5px;
  padding: 5px 10px;
  margin: 0 8px;
  cursor: pointer;

  &:hover {
    background-color: #ccc;
  }

  &:active {
    background-color: #999; 
  }

  &.active {
    background-color: #555;
    color: white; 
  }
`;
