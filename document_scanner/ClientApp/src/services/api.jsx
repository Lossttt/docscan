import axios from 'axios';

console.log(process.env.REACT_APP_API_URL); // Voeg dit toe om te controleren of de URL correct wordt ingelezen

const api = axios.create(); // Geen baseURL op dit niveau

export const getCsrfTokenUrl = () => `${process.env.REACT_APP_API_URL}/Csrf/token`;
export const uploadDocumentUrl = () => `${process.env.REACT_APP_API_URL}/document/upload`;

export default api;
