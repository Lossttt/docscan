import axios from 'axios';

const instance = axios.create({
  baseURL: 'https://localhost:7154/api',
});

export default instance;
