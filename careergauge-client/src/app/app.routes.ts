import { Routes } from '@angular/router';

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
}
];