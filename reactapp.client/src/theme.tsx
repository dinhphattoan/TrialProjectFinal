import { createTheme } from '@mui/material/styles';

const theme = createTheme({
    palette: {
        mode: 'light', 
        primary: {
            main: '#013157', 
            light: '#0288d1',
            dark: '#01579b',
            contrastText: '#fff',
        },
        secondary: {
            main: '#636262',
            light: '#f3e5f5',
            dark: '#ab47bc',
            contrastText: '#fff',
        },
       
        background: {
            default: '#ffffff', 
            paper: '#f5f5f5',   
        },
        text: {
            primary: '#212121', 
            secondary: '#757575', 

        },
    },
    typography: {
        fontFamily: 'Roboto, sans-serif', 
        h1: {
            fontSize: '2.5rem',
            fontWeight: 500,
        },
    },
    spacing: 8, 
    components: { 
        MuiButton: {
            styleOverrides: {
                root: {
                    textTransform: 'none', 
                },
                contained: { 
                    backgroundColor: '#90caf9',
                    '&:hover': {
                        backgroundColor: '#7986cb',
                    },
                },
            },
        },
        MuiAppBar: {
            styleOverrides: {
                root: {
                    backgroundColor: '#fff',
                    color: '#212121',
                },
            },
        },
    },
});

export default theme;