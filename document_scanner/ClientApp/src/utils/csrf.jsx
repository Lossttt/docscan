import axios from "../services/api";

export const getCsrfToken = async () => {
  try {
    const response = await axios.get('/csrf/token');
    console.log("CSRF Token Response:", response); // Log the entire response object

    if (response && response.data && response.data.token) {
      return response;
    } else {
      console.error('CSRF token not found in response:', response);
      return null;
    }
  } catch (error) {
    console.error('Failed to get CSRF token:', error);
    return null;
  }
};
