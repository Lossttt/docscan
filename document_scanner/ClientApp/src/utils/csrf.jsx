import axios from "../services/api";
import { getCsrfTokenUrl } from "../services/api";

// Functie om de CSRF-token op te halen
export const getCsrfToken = async () => {
  try {
    const response = await axios.get(getCsrfTokenUrl());
    console.log("CSRF Token Response:", response); // Log het volledige response object

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
