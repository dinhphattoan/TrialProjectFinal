import { Box, Card, CardMedia, CardContent, FormControl, FormLabel, TextField, Typography, Button, Link, CircularProgress } from '@mui/material';
import * as React from 'react';
import homeLogoImage from '../assets/mainlogo.png'
import microsoftIcon from '../assets/icons8-microsoft-48.png';
import { Navigate, useNavigate } from 'react-router-dom';
import LoadingComponent from './Loading';
import UserSessionValidation from '../validation';

const LogInFormContent: React.FC = () => {
    const [username, SetUsername] = React.useState<string>("");
    const [password, SetPassword] = React.useState<string>("");
    const [isLogInLoading, SetIsSignInLoading] = React.useState<boolean>(false);
    const navigate = useNavigate();
    const handleLogin = async (username: string, password: string) => {
        SetIsSignInLoading(true);
        try {
            const response = await fetch('/api/Accounts/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, password }),
                credentials: 'include'
            });

            if (!response.ok) {
                if (response.status === 401) {
                    console.log("No request endpoint");
                    return;
                }
                if (response.status === 400) {
                    console.log("Bad request:", response.status, response.statusText);
                    return;
                }
                const errorData = await response.json();
                const errorMessage = errorData.message || 'Login failed';
                throw new Error(errorMessage);
            }
            const data = await response.json();
            localStorage.setItem('jwt', data.token);
            navigate('/', { replace: true });
        }
        catch (error: any) {
            console.error('Login error', error);
            alert(error.message);
        }

    };
    return (
        <Box
            sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                flexGrow: 1,
                // backgroundColor: theme.palette.background.default,
                width: '100vw',
                minWidth: '100px',
                height: '100vh',
            }}
        >
            <Card
                elevation={10}
                sx={{
                    width: 500,
                    borderRadius: '20px',
                    maxHeight: 700,
                    marginX: '20px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    paddingTop: '10px',
                }}
            >
                <CardMedia
                    image={homeLogoImage}
                    component="img"
                    alt="My Logo"
                    sx={{
                        height: 150,
                        width: 'auto',
                        objectFit: 'contain',
                        flexGrow: 0,
                    }}
                />
                <CardContent
                    sx={{
                        flexGrow: 1,
                        width: 1,
                        display: 'flex',
                        flexDirection: 'column',
                        paddingX: '50px',
                    }}
                >
                    <FormControl sx={{ padding: 1 }} margin="normal">
                        <FormLabel
                            id="email-label" // Added ID
                            sx={{ color: 'black', fontWeight: 'bold' }}
                        >
                            Sign in with your email address
                        </FormLabel>
                        <TextField label="Email" id="email" margin="dense" fullWidth
                            onChange={(e) => SetUsername(e.target.value)}
                        />
                        <TextField label="Password" id="password" margin="normal" type="password" fullWidth
                            onChange={(e) => SetPassword(e.target.value)}
                        />
                        <Typography
                            color="primary"
                        >
                            <Link href={'#'} underline="hover">
                                Forgot your password?
                            </Link>
                        </Typography>
                        <Button
                            type="submit"
                            variant="contained"
                            loading={isLogInLoading}
                            loadingIndicator={<CircularProgress size={16} color="inherit" />}
                            disabled={isLogInLoading}
                            sx={{
                                maxWidth: '200px',
                                marginTop: 2,
                                backgroundColor: (theme) => theme.palette.primary.dark
                            }}
                            onClick={async () => {
                                await handleLogin(username, password)
                                SetIsSignInLoading(false);

                            }}
                        >
                            {isLogInLoading ? "Signing In..." : "Sign In"}
                        </Button>

                    </FormControl>

                    <FormControl margin="dense"
                    >
                        <FormLabel sx={{ color: 'black', fontWeight: 'bold', marginBottom: 1, fontSize: '15px' }}>
                            Sign In With SSO
                        </FormLabel>
                        <Button
                            variant="outlined"
                            startIcon={
                                <img
                                    src={microsoftIcon}
                                    alt="Microsoft Icon"
                                    style={{
                                        height: "20px",
                                        width: "20px",
                                        justifySelf: 'flex-start',
                                        alignSelf: 'flex-start',
                                        paddingTop: 5,
                                        paddingBottom: 5,

                                    }}
                                />
                            }
                            sx={{
                                borderRadius: '50px',
                                boxShadow: '0px 0px 15px rgba(0, 0, 0, 0.3)',
                                borderColor: 'white',
                                marginBottom: 1,
                                width: '100%',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                paddingLeft: '16px',
                                paddingRight: '16px'
                            }}
                            color="primary"
                        >
                            <Typography
                                fontSize={15}
                                color={'black'}
                                fontWeight={'bold'}
                            >
                                Microsoft Account
                            </Typography>
                        </Button>

                    </FormControl>
                    <FormControl 
                    margin='normal'
                    // {{ margin: theme.spacing(5), marginTop: 1 }}
                    >
                        <FormLabel sx={{ color: 'black', fontWeight: 'bold', marginBottom: 1, fontSize: '15px' }}>
                            Sign In With Card Compliant
                        </FormLabel>
                        <Button
                            variant="outlined"
                            startIcon={
                                <img
                                    src={microsoftIcon}
                                    alt="Microsoft Icon"
                                    style={{
                                        height: "20px",
                                        width: "20px",
                                        marginRight: '8px',
                                        justifySelf: 'start',
                                        paddingTop: 5,
                                        paddingBottom: 5
                                    }}
                                />
                            }
                            sx={{
                                borderRadius: '50px',
                                boxShadow: '0px 0px 15px rgba(0, 0, 0, 0.3)',
                                borderColor: 'white',
                                marginBottom: 1,
                                width: '100%',
                                display: 'flex',
                                alignItems: 'center',
                                justifyContent: 'center',
                                paddingLeft: '16px',
                                paddingRight: '16px'
                            }}
                        >
                            <Typography
                                fontSize={15}
                                color={'black'}
                                fontWeight={'bold'}
                            >
                                Card Compliant
                            </Typography>

                        </Button>
                        <FormLabel
                            id="email-label"
                            sx={{
                                color: 'black', textAlign: 'center',
                                fontWeight: 'light',
                                fontSize: '10px',
                                marginTop: 2
                            }}

                        >
                            Copyright 2024 @ Card Compliant. All right reserved.
                        </FormLabel>

                    </FormControl>

                </CardContent>
            </Card>
        </Box>
    );
}
const LoginPage: React.FC = () => {
    const [isLoading, SetIsLoading] = React.useState<boolean>(true);
    const [isUserLoggedIn, SetIsUserLoggedIn] = React.useState<boolean>(false);
    const checkIsLoggedIn = async (): Promise<boolean> => {
        return UserSessionValidation();
    };

    React.useEffect(() => {
        const checkLoginStatus = async () => {
            SetIsLoading(true);
            const isLoggedIn = await checkIsLoggedIn();
            SetIsLoading(false);
            SetIsUserLoggedIn(isLoggedIn);
        }
        checkLoginStatus();
    }, [])

    if (isUserLoggedIn === true) {
        return <Navigate to="/home" replace />;
    }
    if (isLoading) {
        return <LoadingComponent />
    }
    return (<LogInFormContent />);
}
export default LoginPage;