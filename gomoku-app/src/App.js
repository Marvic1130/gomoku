import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
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
import axios from 'axios';
import styled from 'styled-components';


axios.get('/post')
.then(response => {
  // 백엔드 응답 처리
  <h1>asfdasdf
  </h1>
})
.catch(error => {
  // 에러 처리
});
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
          <Footer/>
      </PageContainer>
    </Router>
  );
};

export default App;



const PageContainer = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 100vh;
`;