import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { ScheduleAddComponent } from './schedule-add/schedule-add.component';
import { ScheduleListComponent } from './schedule-list/schedule-list.component';
import { ScheduleComponent } from './schedule.component';
import { SendSmsComponent } from './send-sms/send-sms.component';

export const routes: Routes = [
  {
    path: '',
    component: ScheduleComponent,
    canActivate: [NgxPermissionsGuard],
    data: {
      permissions: {
        only: [ROLES.Admin, ROLES.SuperAdmin, ROLES.NormalAdmin, ROLES.Scheduler],
        redirectTo: '/error/403',
      },
    },
    children: [
      {
        path: '',
        component: ScheduleListComponent,
      },
      {
        path: 'add',
        component: ScheduleAddComponent,
      },
      {
        path: 'add/:typeAdd',
        component: ScheduleAddComponent,
      },
      {
        path: 'add/:template',
        component: ScheduleAddComponent,
      },
      {
        path: 'edit/:id',
        component: ScheduleAddComponent,
      },
      {
        path: 'invite/:id',
        component: ScheduleAddComponent,
      },
      {
        path: 'send-sms',
        component: SendSmsComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ScheduleRoutingModule {}
