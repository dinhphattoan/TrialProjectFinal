import { Outlet } from 'react-router';
import { DashboardLayout } from '@toolpad/core/DashboardLayout';
import HomeLogo from '../assets/mainlogo.png';
export default function Layout() {
  return (
    <DashboardLayout
    
     disableCollapsibleSidebar branding={{ title: 'Card Compliant',
      'logo': <img src={HomeLogo} alt="Home Logo" />,
      homeUrl: '/',}}>
        <Outlet />
    </DashboardLayout>
  );
}
