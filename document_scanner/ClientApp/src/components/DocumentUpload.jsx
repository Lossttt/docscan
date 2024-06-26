import React, { useState } from 'react';
import { Button, Box } from '@mui/material';
import CloudUploadIcon from '@mui/icons-material/CloudUpload';
import { uploadDocument } from '../services/documentService'; // Bestand voor het verwerken van API-verzoeken
import { validateFileType } from '../utils/fileUtils'; // Bestand voor het controleren van bestandstypen
import { getCsrfToken } from '../utils/csrf';
import CustomSnackbar from './validation/snackbar';

const DocumentUpload = ({ onUploadComplete, onUploadStart }) => {
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [openSnackbar, setOpenSnackbar] = useState(false);

  const handleFileChange = async (event) => {
    const file = event.target.files[0];
    if (!file) return;

    const allowedTypes = ['image/jpeg', 'image/png', 'image/jpg'];
    if (!validateFileType(file, allowedTypes)) {
      setErrorMessage('Ongeldig bestandstype ingediend. Probeer het opnieuw met een correct bestandstype');
      setOpenSnackbar(true);
      return;
    }

    setLoading(true);
    onUploadStart();

    try {
      const formData = new FormData();
      formData.append('file', file);

      const csrfToken = (await getCsrfToken()).data.token;

      // Upload het document
      const response = await uploadDocument(formData, csrfToken);

      onUploadComplete(response.data);
    } catch (err) {
      console.error('Failed to upload file:', err);
      if (err.response) {
        onUploadComplete(err.response.data);
      } else {
        console.error('Error', err.message);
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box textAlign="center">
      <input
        accept="image/*"
        style={{ display: 'none' }}
        id="raised-button-file"
        type="file"
        onChange={handleFileChange}
      />
      <label htmlFor="raised-button-file">
        <Button
          variant="contained"
          component="span"
          startIcon={<CloudUploadIcon />}
          disabled={loading}
        >
          Upload Document
        </Button>
      </label>
      <CustomSnackbar open={openSnackbar} message={errorMessage} onClose={() => setOpenSnackbar(false)} />
    </Box>
  );
};

export default DocumentUpload;
