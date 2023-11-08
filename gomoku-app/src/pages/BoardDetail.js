import React, { useState } from 'react';
import styled from 'styled-components';
import data from '../data/data.json'



const BoardDetail = () => {

  const post = data.post[0];

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
