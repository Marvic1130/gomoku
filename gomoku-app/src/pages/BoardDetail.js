import React, { useState } from 'react';
import styled from 'styled-components';

const DetailContainer = styled.div`
  text-align: center;
  margin: 5% auto;
  width: 60%;
  padding: 2%;
  border: 1px solid #ccc;
  border-radius: 5px;
  background-color: #f9f9f9;
`;

const Title = styled.h1`
  margin-bottom: 10px;
  color: #333;
  font-size: 2rem;
  text-align: left;
`;

const SubInfo = styled.div`
  text-align: left;
  color: #888;
  margin-bottom: 20px;
`;

const Content = styled.p`
  text-align: left;
  margin-bottom: 40px;
  font-size: 1.2rem;
  line-height: 1.8;
`;

const CommentSection = styled.div`
  text-align: left;
`;

const CommentInput = styled.input`
  width: 70%;
  padding: 10px;
  margin-bottom: 20px;
  border: 1px solid #ccc;
  border-radius: 5px;
`;

const CommentButton = styled.button`
  background-color: black;
  color: white;
  border: none;
  padding: 8px 15px;
  cursor: pointer;
  border-radius: 5px;
  font-size: 1rem;
`;

const BoardDetail = () => {
  // 가정: 게시글의 정보가 JSON 형식으로 제공됨
  const post = {
    title: '게시글 제목',
    date: '2023년 10월 31일',
    author: '사용자1',
    content:
      '다음 내용은 임시 텍스트입니다. 그림자는 가장 찾아 작고 피고 그것은 지혜는 따뜻한 칼이다. 이것을 쓸쓸하랴? 심장의 얼마나 목숨을 봄바람이다..맺어. 것이 지혜는 그들의 풍부하게 그리하였는가? 생의 위하여 것이 가치를 얼마나 못할 살았으며. ',
  };

  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');

  const handleAddComment = () => {
    if (newComment) {
      setComments([...comments, newComment]);
      setNewComment('');
    }
  };

  return (
    <DetailContainer>
      <Title>{post.title}</Title>
      <SubInfo>
        작성일: {post.date} | 작성자: {post.author}
      </SubInfo>
      <Content>{post.content}</Content>
      <CommentSection>
        <h3>댓글</h3>
        {comments.map((comment, index) => (
          <p key={index}>{comment}</p>
        ))}
        <CommentInput
          type="text"
          placeholder="댓글을 입력하세요"
          value={newComment}
          onChange={(e) => setNewComment(e.target.value)}
        />
        <CommentButton onClick={handleAddComment}>댓글 달기</CommentButton>
      </CommentSection>
    </DetailContainer>
  );
};

export default BoardDetail;
