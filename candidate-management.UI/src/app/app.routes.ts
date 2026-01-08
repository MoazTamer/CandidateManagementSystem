import { Routes } from '@angular/router';
import { CandidatesListComponent } from './components/candidates-list/candidates-list.component';
import { CandidateFormComponent } from './components/candidate-form/candidate-form.component';
import { SkillsManagementComponent } from './components/skills-management/skills-management.component';
import { UploadComponent } from './components/upload/upload.component';

export const routes: Routes = [
  { path: '', redirectTo: '/candidates', pathMatch: 'full' },
  { path: 'candidates', component: CandidatesListComponent },
  { path: 'candidate/new', component: CandidateFormComponent },
  { path: 'candidate/:id', component: CandidateFormComponent },
  { path: 'candidate/:id/skills', component: SkillsManagementComponent },
  { path: 'upload', component: UploadComponent },
  { path: '**', redirectTo: '/candidates' }
];
