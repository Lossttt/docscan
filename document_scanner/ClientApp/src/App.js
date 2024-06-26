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
import LoadingPage from './components/LoadingPage';
import {
  StyledFooter,
  StyledNavbar,
  HeaderWrapper,
  StyledContainer,
} from './styles/styles';
import { getCsrfToken } from './utils/csrf';
import delay from './utils/delay'; // Import the delay function

const App = () => {
  const [result, setResult] = useState(null);
  const [csrfToken, setCsrfToken] = useState(null);
  const [loading, setLoading] = useState(false);
  const [showResult, setShowResult] = useState(false);
  const [loadingComplete, setLoadingComplete] = useState(false); // State for loading completion

  useEffect(() => {
    const fetchCsrfToken = async () => {
      try {
        const response = await getCsrfToken();
        if (response && response.data && response.data.token) {
          setCsrfToken(response.data.token);
        }
      } catch (error) {
        console.error('Failed to fetch CSRF token:', error);
      }
    };
    fetchCsrfToken();
  }, []);
  
  const handleUploadComplete = async (data) => {
    setResult(data);
    await delay(2000); // Add a delay of 2 seconds
    setLoading(false); // Set loading to false after delay
    setShowResult(true); // Show result page after delay
    setLoadingComplete(true); // Set loadingComplete to true after delay
  };

  const handleUploadStart = () => {
    setLoading(true); // Show loading page on upload start
    setShowResult(false); // Hide result page on new upload start
    setLoadingComplete(false); // Reset loadingComplete state on new upload start
  };

  const handleNewScan = () => {
    setResult(null);
    setShowResult(false);
    setLoadingComplete(false); // Reset loadingComplete state on new scan
  };

  return (
    <div>
      {loading && <LoadingPage />}
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
          {!showResult ? (
            <DocumentUpload csrfToken={csrfToken} onUploadComplete={handleUploadComplete} onUploadStart={handleUploadStart} />
          ) : (
            <DocumentResult result={result} onNewScan={handleNewScan} />
          )}
        </Container>
        <StyledFooter>
          <p>&copy; 2024 GetMore Systems. All rights reserved.</p>
        </StyledFooter>
      </StyledContainer>
    </div>
  );
};

export default App;
