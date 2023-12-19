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

  const totalPages = Math.ceil(posts.length / pageSize);

  // 페이지네이션 범위
  const paginationRange = () => {
    const maxPagesToShow = 5;
    const middlePage = Math.floor(maxPagesToShow / 2);

    if (currentPage <= middlePage) {
      return Array.from({ length: maxPagesToShow }, (_, index) => index + 1);
    }

    if (currentPage + middlePage >= totalPages) {
      return Array.from(
        { length: maxPagesToShow },
        (_, index) => totalPages - maxPagesToShow + index + 1
      );
    }

    return Array.from({ length: maxPagesToShow }, (_, index) => currentPage - middlePage + index);
  };

  return (
    <Container>
      <PostGrid>
        {currentPosts.map((post) => (
          <PostItem key={post.id}>
            <h2>{post.title}</h2>
            <p>{post.content}</p>
          </PostItem>
        ))}
      </PostGrid>

      <PaginationContainer>
        <Pagination>
          {currentPage > 1 && (
            <PageNumber onClick={() => paginate(currentPage - 1)}>
              &lt;
            </PageNumber>
          )}

          {paginationRange().map((pageNumber) => (
            <PageNumber
              key={pageNumber}
              onClick={() => paginate(pageNumber)}
              className={currentPage === pageNumber ? 'active' : ''}
            >
              {pageNumber}
            </PageNumber>
          ))}

          {currentPage < totalPages && (
            <PageNumber onClick={() => paginate(currentPage + 1)}>
              &gt;
            </PageNumber>
          )}
        </Pagination>
      </PaginationContainer>
    </Container>
  );
};

export default PostList;


const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  border: 1px solid #ccc;
  border-radius: 10px;
  box-shadow: 0px 1px 10px 10px rgba(0, 0, 0, 0.05);  
  overflow: hidden;
  background:white;
`;

const PostGrid = styled.div`
  display: grid;
  grid-template-columns: 1fr; 
  grid-gap: 10px;
  border-bottom: 2px solid #ccc;
  overflow: hidden;
`;

const PostItem = styled.div`
  background-color: white;
  padding: 15px; 
  border-bottom: 1px solid #eee;

`;

const PaginationContainer = styled.div`
  display: flex;
  justify-content: center;
  margin: 20px;
`;

const Pagination = styled.div`
  display: flex;
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