import React from 'react';
import { Card, CardContent, Typography, Box } from '@mui/material';
import { CheckCircleOutline, ErrorOutline } from '@mui/icons-material'; // Iconen voor goedgekeurd en afgekeurd

const DocumentResult = ({ result }) => {
    if (!result) return null;

    const { success, message, checks, image } = result;

    return (
        <Card>
            <CardContent>
                <Box display="flex" alignItems="center" justifyContent="center">
                    {success ? <CheckCircleOutline sx={{ color: 'green', fontSize: 40 }} /> : <ErrorOutline sx={{ color: 'red', fontSize: 40 }} />}
                    <Typography variant="h5" component="div" sx={{ ml: 2 }}>
                        {success ? 'Document goedgekeurd' : 'Document niet goedgekeurd'}
                    </Typography>
                </Box>
                <Typography color="text.secondary" align="center" mt={2} mb={4}>
                    {message}
                </Typography>
                {checks.length > 0 && (
                    <Box textAlign="left" mt={2}>
                        <Typography variant="h6" gutterBottom>
                            Controleer de volgende punten:
                        </Typography>
                        <ul>
                            {checks.map((check, index) => (
                                <li key={index}>
                                    <Typography color={check.passed ? 'textPrimary' : 'error'}>
                                        {check.message}
                                    </Typography>
                                </li>
                            ))}
                        </ul>
                    </Box>
                )}
                {image && (
                    <Box mt={4} display="flex" justifyContent="center">
                        <img src={`data:image/jpeg;base64,${image}`} alt="Gescand Document" style={{ maxWidth: '100%' }} />
                    </Box>
                )}
            </CardContent>
        </Card>
    );
};

export default DocumentResult;
