import React from 'react';
import { LinearProgress, Typography, Box } from '@mui/material';

const UploadProgress = ({ value }) => (
    <Box display="flex" alignItems="center" justifyContent="center">
        <Box width="100%" mr={1}>
            <LinearProgress variant="determinate" value={value} />
        </Box>
        <Box minWidth={35}>
            <Typography variant="body2" color="textSecondary">{`${Math.round(value)}%`}</Typography>
        </Box>
    </Box>
);

export default UploadProgress;
