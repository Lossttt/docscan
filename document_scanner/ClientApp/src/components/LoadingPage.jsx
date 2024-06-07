import React, { useState, useEffect } from 'react';
import { Box, CircularProgress, Typography, Fade, Zoom } from '@mui/material';
import { CheckCircleOutline } from '@mui/icons-material';

const LoadingPage = ({ loading, showResult }) => {
  const [loadingComplete, setLoadingComplete] = useState(false);

  useEffect(() => {
    if (!loading && showResult) {
      setLoadingComplete(true);
    }
  }, [loading, showResult]);

  return (
    <Box
      display="flex"
      flexDirection="column"
      alignItems="center"
      justifyContent="center"
      height="100vh"
      position="fixed"
      top="0"
      left="0"
      right="0"
      bottom="0"
      zIndex="1000"
      style={{
        backgroundImage: "url('https://images.pexels.com/photos/1103970/pexels-photo-1103970.jpeg?cs=srgb&dl=pexels-jplenio-1103970.jpg&fm=jpg')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundColor: "rgba(0, 0, 0, 0.70)", // Adding a dark overlay
        backgroundBlendMode: "overlay", // Blending the overlay with the background image
      }}
    >
      <Fade in={!loadingComplete} timeout={100}>
        <Box display="flex" flexDirection="column" alignItems="center">
          <CircularProgress size={80} thickness={4} style={{ color: '#fff' }} />
          <Typography variant="h6" mt={2} color="#fff" sx={{ fontWeight: 'bold', textShadow: '1px 1px 2px rgba(0,0,0,0.7)' }}>
            Even geduld...
          </Typography>
        </Box>
      </Fade>
    </Box>
  );
};

export default LoadingPage;
