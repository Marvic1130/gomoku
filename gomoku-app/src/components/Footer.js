import React from 'react';
import styled from 'styled-components';



const Footer = () => {
  return (
    <FooterContainer>
      <FooterText>&copy; 2023 go!moku Website. All Rights Reserved.</FooterText>
    </FooterContainer>
  );
};

export default Footer;

const FooterContainer = styled.footer`
  background-color: black;
  color: white;
  padding: 20px 0;
  margin-top: 100px;
  text-align: center;
`;

const FooterText = styled.p`
  margin: 0;
`;
