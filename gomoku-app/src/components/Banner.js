import React from 'react';
import styled from 'styled-components';

const Banner = () => {
  return (
    <BannerContainer>
      <h1> GO! MOKU </h1>
      <SearchInput type="text" placeholder="검색 내용을 입력해 주세요." />
    </BannerContainer>
  );
};

export default Banner;


const BannerContainer = styled.div`
  background-color: #fbfbfb;
  color: #333;
  padding: 130px;
  text-align: center;
`;

const SearchInput = styled.input`
  width: 500px;
  height: 30px;
  padding: 10px;
  border: 2px solid #dddddd;
  color:black;
  border-radius: 5px;
  margin: 10px;
`;

