import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./pages/dashboard/dashboard')
        .then(m => m.Dashboard)
  },
  {
    path: 'career-details/:careerProfileId',
    loadComponent: () =>
      import('./pages/career-details/career-details')
        .then(m => m.CareerDetails)
  },
  {
    path: 'skills',
    loadComponent: () =>
      import('./pages/skill-profile/skill-profile')
        .then(m => m.SkillProfile)
  },
  {
    path: 'career-comparison',
    loadComponent: () =>
      import('./pages/career-comparison/career-comparison')
        .then(m => m.CareerComparisonPage)
  },
  {
    path: 'assessment/:skillId',
    loadComponent: () =>
      import('./pages/assessment/assessment')
        .then(m => m.Assessment)
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  }
];