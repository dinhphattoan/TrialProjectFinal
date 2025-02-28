import * as React from 'react';
import { Navigation } from '@toolpad/core/AppProvider';
import { GetResponseServerAPI, PostResponseServerAPI, UserSessionValidation } from '../validation';
import { Navigate, useNavigate } from 'react-router-dom';
import { Session } from '@toolpad/core/AppProvider';
import LoadingComponent from './Loading';
import { ReactRouterAppProvider } from '@toolpad/core/react-router';
import Layout from '../layouts/dashboard';

const NAVIGATION: Navigation = [
    {
        segment: 'dashboard',
        title: 'Dashboard',
        children: [
            {
                segment: 'sales',
                title: 'Sales',
            },
            {
                segment: 'traffic',
                title: 'Traffic',
            },
        ],
    },
    { kind: 'divider' },
    {
        segment: 'ad-hocquery',
        title: 'Ad-hoc Query',
    },
    { kind: 'divider' },
    {
        segment: 'reportlibrary',
        title: 'Report Library',
        children: [
            {
                segment: 'sales',
                title: 'Sales',
            },
            {
                segment: 'traffic',
                title: 'Traffic',
            },
        ],
    },
    { kind: 'divider' },
    {
        segment: 'user management',
        title: 'User Management',
    },
    { kind: 'divider' },
    {
        segment: 'admin',
        title: 'Admin',
        children: [
            {
                segment: 'sales',
                title: 'Sales',
            },
            {
                segment: 'traffic',
                title: 'Traffic',
            },
        ],
    },
    { kind: 'divider' },
    {
        segment: 'glossary',
        title: 'Glossary',
    },
    { kind: 'divider' },
    {
        segment: 'data management',
        title: 'Data Management',
        children: [
            {
                segment: 'sales',
                title: 'Sales',
            },
            {
                segment: 'traffic',
                title: 'Traffic',
            },
        ],
    },
    { kind: 'divider' },
    {
        segment: 'escheatment activities',
        title: 'Escheatment Activities',
        children: [
            {
                segment: 'sales',
                title: 'Sales',
            },
            {
                segment: 'traffic',
                title: 'Traffic',
            },
        ],
    },
    { kind: 'divider' },
    {
        segment: 'review & approvals',
        title: 'Reviews & Approvals',
    },
    { kind: 'divider' },

];

export default function DashboardLayoutBasic() {
    const [isLoading, SetIsLoading] = React.useState<boolean>(true);
    const [isUserLoggedIn, SetIsUserLoggedIn] = React.useState<boolean>(false);
    const checkIsLoggedIn = async (): Promise<boolean> => {
        return UserSessionValidation();
    }
    const handleloggout = async () => {
        const response = await PostResponseServerAPI("api/Accounts/logout", {});
        if (!response.ok) {
            return;
        }
        SetIsUserLoggedIn(false);
    }
    const navgiator = useNavigate();
    const [session, SetSession] = React.useState<Session | null>(null);
    const authentication = React.useMemo(() => {
        return {
            signIn: async () => {
                const response = await GetResponseServerAPI("api/Accounts/authenticated");
                const data = await response.json();
                SetSession({
                    user: {
                        name: data.username,
                        image: 'https://avatars.githubusercontent.com/u/19550456',
                    },
                });
            },
            signOut: async () => {
                SetSession(null);
                await handleloggout();
                navgiator('/login');
            },
        };
    }, []);
    React.useEffect(() => {
        const checkLoginStatus = async () => {
            SetIsLoading(true);
            const isLoggedIn = await checkIsLoggedIn();
            SetIsLoading(false);
            SetIsUserLoggedIn(isLoggedIn);
            if (isLoggedIn) {
                await authentication.signIn();
            }
        }
        
        checkLoginStatus();
    }, [])

    if (isLoading) {
        return <LoadingComponent />;
    }

    if (!isUserLoggedIn) {
        return <Navigate to="/login" replace />
    }

    return (
        <ReactRouterAppProvider session={session} authentication={authentication}  navigation={NAVIGATION}>
            <Layout />
        </ReactRouterAppProvider>

    );
}