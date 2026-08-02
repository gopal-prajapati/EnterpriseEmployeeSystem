import { Routes } from '@angular/router';
import { Layout } from './layout/layout/layout';
import { Dashboard } from './features/dashboard/pages/dashboard/dashboard';
import { EmployeeList } from './features/employees/pages/employee-list/employee-list';
import { EmployeeCreate } from './features/employees/pages/employee-create/employee-create';

export const routes: Routes = [
    {
    path: '',
    component: Layout,
    children: [
      {
        path: '',
        component: Dashboard
      },
      {
        path: 'employees',
        component: EmployeeList
      },
      { path: 'employees/create', component: EmployeeCreate },
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  }
];
