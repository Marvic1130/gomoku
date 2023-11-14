import { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import data from '../data/data.json';

const pageSize = 5;

const Community = () => {
  const posts = data.post;
  const [currentPage, setCurrentPage] = useState(1);

  const indexOfLastPost = currentPage * pageSize;
  const indexOfFirstPost = indexOfLastPost - pageSize;
  const currentPosts = posts.slice(indexOfFirstPost, indexOfLastPost);

  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  return (
    <CommunityContainer>
      <PostList>
        <SearchInput type="text" placeholder="게시글을 검색해 주세요." />
        {currentPosts.map((post) => (
          <PostItem key={post.id}>
            <PostTitle>{post.title}</PostTitle>
            <Postinfo>{post.author} | {post.date}</Postinfo>
            <PostContent>{post.content}</PostContent>
          </PostItem>
        ))}
        {/* 페이지네이션 컴포넌트 추가 */}
        <Pagination>
          {Array.from({ length: Math.ceil(posts.length / pageSize) }, (_, index) => (
            <PageNumber key={index + 1} onClick={() => handlePageChange(index + 1)}>
              {index + 1}
            </PageNumber>
          ))}
        </Pagination>
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

const Postinfo = styled.p`
  font-size: 1rem;
  color: grey;
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

const Pagination = styled.div`
  display: flex;
  justify-content: center;
  margin-top: 20px;
`;

const PageNumber = styled.div`
  cursor: pointer;
  margin: 0 5px;
  padding: 5px 10px;
  border: 1px solid #ccc;
  border-radius: 5px;

  &:hover {
    background-color: #f1f1f1;
  }
`;
