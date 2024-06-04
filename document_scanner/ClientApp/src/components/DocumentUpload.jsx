import React, { useState, useEffect } from "react";
import { Button, Box } from "@mui/material";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import axios from "../services/api";
import { getCsrfToken } from "../utils/csrf";

const DocumentUpload = ({ onUploadComplete }) => {
  const [loading, setLoading] = useState(false);
  const [progress, setProgress] = useState(0);

  const handleFileChange = async (event) => {
    const file = event.target.files[0];
    if (!file) return;
  
    setLoading(true);
    setProgress(0);
  
    try {
      const formData = new FormData();
      formData.append("file", file);
  
      const csrfToken = (await getCsrfToken()).data.token; // Access the token string
      console.log("Token:", csrfToken);
  
      const headers = {
        "Content-Type": "multipart/form-data",
        "X-CSRF-TOKEN": csrfToken,
      };
      console.log("Request Headers:", headers); // Log the headers
  
      const response = await axios.post("/document/upload", formData, {
        headers: headers,
        onUploadProgress: (progressEvent) => {
          const progress = Math.round(
            (progressEvent.loaded / progressEvent.total) * 100
          );
          setProgress(progress);
        },
      });
      onUploadComplete(response.data);
    } catch (err) {
      console.error("Failed to upload file:", err);
    } finally {
      setLoading(false);
    }
  };
  

  return (
    <Box textAlign="center">
      <input
        accept="image/*"
        style={{ display: "none" }}
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
      {loading && <p>{progress}% uploaded...</p>}
    </Box>
  );
};

export default DocumentUpload;
