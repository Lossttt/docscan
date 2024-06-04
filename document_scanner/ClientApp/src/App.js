import React, { useState, useEffect } from 'react';
import {
  Container,
  CssBaseline,
  Typography,
  AppBar,
  Toolbar,
  Button,
} from '@mui/material';
import DocumentUpload from './components/DocumentUpload';
import DocumentResult from './components/DocumentResult';
import {
  StyledFooter,
  StyledNavbar,
  HeaderWrapper,
  StyledContainer,
} from './styles/styles';
import { getCsrfToken } from './utils/csrf';

const App = () => {
  const [result, setResult] = useState(null);
  const [csrfToken, setCsrfToken] = useState(null);

  useEffect(() => {
    const fetchCsrfToken = async () => {
      try {
        const response = await getCsrfToken();
        console.log('CSRF Token Response:', response);
        if (response && response.data && response.data.token) {
          setCsrfToken(response.data.token);
        } else {
          console.error('Invalid CSRF token response:', response);
        }
      } catch (error) {
        console.error('Failed to fetch CSRF token:', error);
      }
    };
    fetchCsrfToken();
  }, []);
  
  const handleUploadComplete = (data) => {
    setResult(data);
  };

  return (
    <div>
      <StyledContainer>
        <StyledNavbar position="static">
          <Toolbar>
            <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
              GetMore Systems
            </Typography>
            <Button color="inherit">Home</Button>
            <Button color="inherit">Features</Button>
            <Button color="inherit">Pricing</Button>
            <Button color="inherit">Contact</Button>
          </Toolbar>
        </StyledNavbar>
        <Container component="main" maxWidth="md">
          <CssBaseline />
          <HeaderWrapper>
            <Typography variant="h2" component="h1" align="center" gutterBottom>
              Document Scanner
            </Typography>
          </HeaderWrapper>
          <DocumentUpload csrfToken={csrfToken} onUploadComplete={handleUploadComplete} />
          <DocumentResult result={result} />
        </Container>
        <StyledFooter>
          <p>&copy; 2024 GetMore Systems. All rights reserved.</p>
        </StyledFooter>
      </StyledContainer>
    </div>
  );
};

export default App;
