import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from '../website/pages/home/home.component';
import { WebsiteComponent } from '../website/website.component';
import { ChangePasswordComponent } from './test-components/change-password/change-password.component';
import { DepartmentScheduleComponent } from './test-components/department-schedule/department-schedule.component';
import { DocumentsComponent } from './test-components/documents/documents.component';
import { PersonalDocumentsComponent } from './test-components/personal-documents/personal-documents.component';
import { PersonalScheduleComponent } from './test-components/personal-schedule/personal-schedule.component';
import { PollComponent } from './test-components/poll/poll.component';
import { QrTestComponent } from './test-components/qr-test/qr-test.component';
import { RecentScheduleComponent } from './test-components/recent-schedule/recent-schedule.component';
import { TestWebsiteComponent } from './test-website.component';

const routes: Routes = [
  {
    path: '',
    component: TestWebsiteComponent,
    children: [      
      {
        path: '',        
        component: HomeComponent,
        pathMatch: 'full'
      },
      {
        path: 'shared-documents',
        component: QrTestComponent,
        pathMatch: 'full'
      },
      {
        path: 'shared-documents/:sid',
        component: QrTestComponent,
        pathMatch: 'full'
      },
      {
        path: 'recent-schedule',
        //loadChildren: () => import('./test-pages/recent-schedule/recent-schedule.module').then((m) => m.RecentScheduleModule)        
        component: RecentScheduleComponent,
        pathMatch: 'full'
      },
      {
        path: 'schedule-detail',
        //loadChildren: () => import('./test-pages/recent-schedule/recent-schedule.module').then((m) => m.RecentScheduleModule)        
        component: RecentScheduleComponent,
        pathMatch: 'full'
      },
      {
        path: 'schedule-detail/:sid',
        component: RecentScheduleComponent,
        pathMatch: 'full'
      },
      {
        path: 'poll',
        component: PollComponent,
        pathMatch: 'full'
      },
      {
        path: 'personal-schedule',
        component: PersonalScheduleComponent,
        pathMatch: 'full'
      },
      // {
      //   path: 'department-schedule',
      //   component: DepartmentScheduleComponent,
      //   pathMatch: 'full'
      // },
      {
        path: 'department-schedule',
        component: HomeComponent,
        pathMatch: 'full'
      },
      {
        path: 'personal-documents',
        component: PersonalDocumentsComponent,
        pathMatch: 'full'
      },
      {
        path: 'documents',
        component: DocumentsComponent,
        pathMatch: 'full'
      },
      {
        path: 'change-password',
        component: ChangePasswordComponent,
        pathMatch: 'full'
      },
      { path: '**', redirectTo: 'error/404', pathMatch: 'full' },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TestWebsiteRoutingModule { }
