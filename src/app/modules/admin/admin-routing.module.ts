import { Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { UserListComponent } from './pages/user/user-list/user-list.component';
import { UserAddComponent } from './pages/user/user-add/user-add.component';
import { UserEditComponent } from './pages/user/user-edit/user-edit.component';
import { OfficerListComponent } from './pages/officer/officer-list/officer-list.component';
import { OfficerAddComponent } from './pages/officer/officer-add/officer-add.component';
import { OfficerEditComponent } from './pages/officer/officer-edit/officer-edit.component';
import { ChangePasswordComponent } from './pages/user/change-password/change-password.component';
import { LocationListComponent } from './pages/location/location-list/location-list.component';
import { LocationAddComponent } from './pages/location/location-add/location-add.component';
import { DepartmentListComponent } from './pages/department/list/department-list.component';
import { DepartmentAddComponent } from './pages/department/add/department-add.component';
import { EmailLogsListComponent } from './pages/email-logs/email-logs-list.component';
import { TitleTemplateListComponent } from './pages/title-template/title-template-list/title-template-list.component';

export const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        redirectTo: 'schedule',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'title-template',
        component: TitleTemplateListComponent,
      },
      {
        path: 'user',
        loadChildren: () =>
          import('./pages/user/user.module').then((m) => m.UserModule),
      },
      {
        path: 'officer',
        loadChildren: () =>
          import('./pages/officer/officer.module').then((m) => m.OfficerModule),
      },
      {
        path: 'post',
        loadChildren: () =>
          import('./pages/post/post.module').then((m) => m.PostModule),
      },
      {
        path: 'setting',
        loadChildren: () =>
          import('./pages/setting/setting.module').then((m) => m.SettingModule),
      },
      {
        path: 'schedule',
        loadChildren: () =>
          import('./pages/schedule/schedule.module').then((m) => m.ScheduleModule),
      },
      {
        path: 'schedule-template',
        loadChildren: () =>
        import('./pages/schedule-template/schedule-template.module').then((m) => m.ScheduleTemplateModule),
      },
      {
        path: 'location',
        loadChildren: () =>
          import('./pages/location/location.module').then((m) => m.LocationModule),
      },
      {
        path: 'department',
        loadChildren: () =>
          import('./pages/department/department.module').then((m) => m.DepartmentModule),
      },
      {
        path: 'statistical',
        loadChildren: () =>
          import('./pages/statistical/statistical.module').then((m) => m.StatisticalModule),
      },
      {
        path: 'group-meeting',
        loadChildren: () =>
          import('./pages/group-meeting/group-meeting.module').then((m) => m.GroupMeetingModule),
      },
      {
        path: 'organization',
        loadChildren: () =>
          import('./pages/organization/organization.module').then((m) => m.OrganizationModule),        
      },
      {
        path: 'brandname',
        loadChildren: () =>
          import('./pages/brandname/brandname.module').then((m) => m.BrandnameModule),
      },
      {
        path: 'email-template',
        loadChildren: () =>
          import('./pages/email-template/email-template.module').then((m) => m.EmailTemplateModule),
      },
      {
        path: 'email-logs',
        children: [
          {
            path: '',
            component: EmailLogsListComponent,
          }
        ]
      },
    ],
  },
];
