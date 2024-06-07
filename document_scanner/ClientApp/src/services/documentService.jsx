import axios from './api';

const REACT_BASE_URL = process.env.REACT_APP_API_URL

// Functie om het document te uploaden
export const uploadDocument = async (formData, csrfToken) => {
  const headers = {
    'Content-Type': 'multipart/form-data',
    'X-CSRF-TOKEN': csrfToken,
  };

  return axios.post(`${REACT_BASE_URL}/document/upload`, formData, {
    headers: headers,
  });
};
