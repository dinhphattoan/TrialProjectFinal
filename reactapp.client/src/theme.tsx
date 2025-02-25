import { Box, CssBaseline, useMediaQuery } from '@mui/material';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { useMemo } from 'react';

function ThemeApp({ children }: { children: React.ReactNode }) {
    const isSmallScreen = useMediaQuery('(max-width: 1366px)');
    const isTallScreen = useMediaQuery('(min-height: 1366px)');
    const theme = useMemo(() =>
        createTheme({
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
        }),
        [isSmallScreen, isTallScreen]
    );
    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <Box>
                {children}
            </Box>
        </ThemeProvider>
    )
}


export default ThemeApp;