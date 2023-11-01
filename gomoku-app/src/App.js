import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'; // 'Switch' 대신 'Routes' 사용
import Navbar from './components/Navbar';
import Footer from './components/Footer';

import Main from './pages/Main';
import Login from './pages/Login';
import Regist from './pages/Regist';
import MyPage from './pages/MyPage';
import Ranking from './pages/Ranking';
import Community from './pages/Community';
import CreateBoard from './pages/CreateBoard';
import BoardDetail from './pages/BoardDetail';

import styled from 'styled-components';

//컴포넌트로 바꾸기!!
const PageContainer = styled.div`
  background-color: #f2f2f2;
`;


const App = () => {
  return (
    <Router>
      <PageContainer>
        <Navbar />
          <Routes>
            <Route path="/main" element={<Main />} />
            <Route path="/mypage" element={<MyPage />} />
            <Route path="/ranking" element={<Ranking />} />
            <Route path="/community" element={<Community />} />
            <Route path="/login" element={<Login />} />
            <Route path="/createboard" element={<CreateBoard />} />
            <Route path="/regist" element={<Regist />} />

            <Route path="/boardetail" element={<BoardDetail />} />
            <Route path="/mypage" element={<MyPage />} />
          </Routes>
        <Footer />
      </PageContainer>
    </Router>
  );
};

export default App;
