import styled from 'styled-components';
import {
    Container,
    CssBaseline,
    Typography,
    AppBar,
    Toolbar,
    Button,
} from '@mui/material';

export const StyledFooter = styled.footer`
    background-color: #333;
    color: #fff;
    padding: 20px;
    width: 100%;
    box-shadow: 0 -2px 4px rgba(0, 0, 0, 0.1); /* Voeg een subtiele schaduw toe */
    text-align: center; /* Centreren van de tekst */
`;

export const HeaderWrapper = styled.div`
    margin-top: 20px; /* Verhoog de marge tussen de header en de navbar */
    text-align: center; /* Centreer de tekst */
    background-color: #fff; /* Zorg ervoor dat de achtergrond van de HeaderWrapper wit is */
`;

export const StyledNavbar = styled(AppBar)`
    && {
        background-color: #e1e8ea;
        color: #333;
        padding-left: 20px;
        padding-right: 20px;
    }

    .MuiButton-root {
        text-transform: none;
        margin-left: 10px; /* Ruimte tussen de knoppen */
    }
`;

export const StyledContainer = styled.div`
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    justify-content: space-between; /* Verdeel de ruimte tussen de header en de footer */
    align-items: center; /* Centreer de inhoud horizontaal */
`;

export const ContentWrapper = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
`;
