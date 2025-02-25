import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './components/Login';
import UserSessionValidation from './validation';
import React from 'react';
import LoadingComponent from './components/Loading';
import HomePage from './components/Home';
import GlossaryPage from './pages/glossary';
import PlaceHolder from './pages/placeholder';
import ThemeApp from './theme';


const App: React.FC = () => {
    const [isLoggedIn, setIsLoggedIn] = React.useState<boolean | null>(null);
    const [isLoading, setIsLoading] = React.useState(true);

    React.useEffect(() => {
        const checkLoginStatus = async () => {
            setIsLoading(true);
            const isLoggedInResult = await UserSessionValidation();
            setIsLoggedIn(isLoggedInResult);
            setIsLoading(false);
        };

        checkLoginStatus();
    }, []);
    if (isLoading) {
        return <LoadingComponent />;
    }
    return (
        <ThemeApp>
            <BrowserRouter>
                    <Routes>
                        <Route
                            path="/"
                            element={<HomePage />}
                        >
                            
                            <Route path="glossary" element={<GlossaryPage />} />
                            <Route index element={<PlaceHolder />} />
                            <Route path="*" element={<PlaceHolder />
                        }/>
                        </Route>
                        <Route
                            path="/login"
                            element={!isLoggedIn ? <LoginPage /> : <Navigate to="/" replace />} />

                    </Routes>
            </BrowserRouter>
        </ThemeApp>
    );
};

export default App;