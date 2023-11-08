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
          <Title style={{ minWidth: "100px" }}>Ranking</Title>
          <Title style={{ marginLeft: "30px", minWidth: "150px" }}>User</Title>
          <Title style={{ marginLeft: "600px", minWidth: "100px" }}>Tier</Title>
          <Title style={{ marginLeft: "20px" }}>Win Rate</Title>
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
`;

const Rank = styled.div`
  font-weight: bold;
  min-width: 50px;
`;

const User = styled.div`
  margin-left: 50px;
  min-width: 150px;
`;

const Tier = styled.div`
  margin-left: 600px;
  min-width: 100px;
`;

const WinRate = styled.div`
  margin-left: 20px;
`;

const Divider = styled.div`
  border-top: 1px solid #f1f1f1;
`;

const Title = styled.div`
  font-weight: bold;rank
  min-width: 100px;
`;

const TitleContainer = styled.div`
  display: flex;
  align-items: center;
  padding: 20px;
`;
const SearchInput = styled.input`
  padding: 10px;
  margin: 10px;
  width: 50%;
  align-self: flex-end;
`;