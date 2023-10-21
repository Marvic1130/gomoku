import React from 'react';
import styled from 'styled-components';

const BannerContainer = styled.div`
  background-color: #dedede;
  color: white;
  padding: 50px;
  text-align: center;
`;

const SearchInput = styled.input`
  width: 300px;
  padding: 10px;
  border: none;
  border-radius: 5px;
  margin: 10px;
`;

const Banner = () => {
  return (
    <BannerContainer>
      <h1> GO!MOKU </h1>
      <SearchInput type="text" placeholder="검색 내용을 입력해 주세요." />
    </BannerContainer>
  );
};

export default Banner;
