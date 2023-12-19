import styled from 'styled-components';
import  { useState } from 'react';
import data from '../data/data.json';


const sortData = (data) => {
    const sortedData = data.sort((a, b) => {
      if (a.rank !== b.rank) {
        return a.rank - b.rank;
      }
      return parseInt(b.winRate) - parseInt(a.winRate);
    });
  
    return sortedData;
  
};

const Ranking = () => {
  
  const rankingData = data.ranking;
  const sortedData = sortData(rankingData);
  const [searchTerm, setSearchTerm] = useState('');

  const handleSearch = (event) => {
    setSearchTerm(event.target.value);
  };

  return (
    <RankingContainer>
      <ListContainer>
      <SearchInput
          type="text"
          placeholder="유저명을 검색해 주세요."
          value={searchTerm}
          onChange={handleSearch}
        />
        <TitleContainer>
          {/* 수정 반드시 필요..... */}
          <Title style={{ minWidth: "10%" }}>Ranking</Title>
          <Title style={{ minWidth: "50%" }}>User</Title>
          <Title style={{ minWidth: "10%" }}>Tier</Title>
          <Title>Win Rate</Title>
        </TitleContainer>
        {sortedData.map((item, index) => (
          <div key={index}>
            <ListItem>
              <Rank>{item.rank}</Rank>
              <User>{item.user}</User>
              <Tier>{item.tier}</Tier>
              <WinRate>{item.winRate}</WinRate>
            </ListItem>
            {index < sortedData.length - 1 && <Divider />}
          </div>
        ))}
        
      </ListContainer>
    </RankingContainer>
  );
};

export default Ranking;


const RankingContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const ListContainer = styled.div`
  background-color: white;
  width: 80%;
  margin: 2% auto;
  padding: 20px;
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
`;

const ListItem = styled.div`
  display: flex;
  align-items: center;
  border-radius: 10px;
  margin: 0 2%;
  padding: 20px;
  flex-wrap: wrap;

  @media (max-width: 768px) {
    width: 100%;
  }
  `


const Rank = styled.div`
  font-weight: bold;
  flex: 1;
  min-width: 50px;
`;

const User = styled.div`
  flex: 3.3;
  min-width: 100px;
`;

const Tier = styled.div`
  flex: 1;
  min-width: 100px;
`;

const WinRate = styled.div`
  flex: 1;
  min-width: 100px;

`;
const Divider = styled.div`
  border-top: 1px solid #f1f1f1;
`;

const Title = styled.div`
   font-weight: bold;
     flex-grow: 1;
`;

const TitleContainer = styled.div`
  display: flex;
  align-items: center;
  padding: 20px;
  @media (max-width: 900px) {
    display:fixed;
    }
  
`;
const SearchInput = styled.input`
  padding: 10px;
  margin: 10px;
  width: 50%;
  align-self: flex-end;
`;