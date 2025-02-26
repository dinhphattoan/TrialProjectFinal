import { Outlet } from 'react-router';
import { DashboardLayout } from '@toolpad/core/DashboardLayout';

export default function Layout() {
  return (
    <DashboardLayout
     disableCollapsibleSidebar>
        <Outlet />
    </DashboardLayout>
  );
}
