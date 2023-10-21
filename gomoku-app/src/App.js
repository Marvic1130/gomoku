import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'; // 'Switch' 대신 'Routes' 사용
import Navbar from './components/Navbar';
import Footer from './components/Footer';

import Main from './pages/Main';
import Login from './pages/Login';
import MyPage from './pages/MyPage';
import Ranking from './pages/Ranking';
import Community from './pages/Community';

import styled from 'styled-components';

//컴포넌트로 바꾸기
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
          </Routes>
        <Footer />
      </PageContainer>
    </Router>
  );
};

export default App;
