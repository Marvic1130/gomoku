import React from 'react';
import styled from 'styled-components';

const FooterContainer = styled.footer`
  background-color: #333;
  color: white;
  padding: 20px 0;
  text-align: center;
`;

const FooterText = styled.p`
  margin: 0;
`;

const Footer = () => {
  return (
    <FooterContainer>
      <FooterText>&copy; 2023 go!moku Website. All Rights Reserved.</FooterText>
    </FooterContainer>
  );
};

export default Footer;
