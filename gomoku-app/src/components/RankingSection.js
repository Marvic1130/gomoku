import React from 'react';
import styled from 'styled-components';
import rankingdata from '../data/data.json';

const RankingSection = () => {
  const users = rankingdata.ranking.slice(0, 5); // 상위 여섯 명만 가져오기

  return (
    <RankingContainer>
      <RankingTitle>Ranking</RankingTitle>
      <RankingList>
        {users.map((ranking, index) => (
         <RankItem key={index}>
         <RankItemContent>{ranking.rank}</RankItemContent>
         <RankItemContent>{ranking.user}</RankItemContent>
         <RankItemContent>{ranking.tier}</RankItemContent>
       </RankItem>
        ))}
      </RankingList>
    </RankingContainer>
  );
};

export default RankingSection;

const RankingContainer = styled.div`
  background-color: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0px 1px 10px 10px rgba(0, 0, 0, 0.05);  
  margin-top: 40px;
  text-align: center;
`;

const RankingTitle = styled.h3`
  color: #333;
`;

const RankingList = styled.ul`
  list-style: none;
  padding: 0;
  padding-inline-start: 20px;
`;

const RankItem = styled.li`
  margin: 20px 0; 
  color: #666;
  
`;

const RankItemContent = styled.span`
  padding: 0 20px;
`;

