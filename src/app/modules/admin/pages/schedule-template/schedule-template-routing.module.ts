import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ScheduleTemplateComponent } from './schedule-template.component';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { ScheduleListTemplateComponent } from './schedule-list-template/schedule-list-template.component';
import { ScheduleAddTemplateComponent } from './schedule-add-template/schedule-add-template.component';

export const routes: Routes = [
  {
    path: '',
    component: ScheduleTemplateComponent,
    canActivate: [NgxPermissionsGuard],
    data: {
      permissions: {
        only: [ROLES.Admin, ROLES.SuperAdmin, ROLES.Scheduler],
        redirectTo: '/error/403',
      },
    },
    children: [
      {
        path: '',
        component: ScheduleListTemplateComponent,
      },
      {
        path: 'add',
        component: ScheduleAddTemplateComponent,
      },
      {
        path: 'edit/:id',
        component: ScheduleAddTemplateComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ScheduleTemplateRoutingModule {}
