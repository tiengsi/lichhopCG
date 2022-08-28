import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { GroupMeetingAddComponent } from './group-meeting-add/group-meeting-add.component';
import { GroupMeetingListComponent } from './group-meeting-list/group-meeting-list.component';
import { GroupMeetingComponent } from './group-meeting.component';

export const routes: Routes = [
  {
    path: '',
    component: GroupMeetingComponent,
    canActivate: [NgxPermissionsGuard],
    data: {
      permissions: {
        only: [ROLES.Admin, ROLES.SuperAdmin, ROLES.Scheduler],
        redirectTo: '/error/403'
      }
    },
    children: [
      {
        path: '',
        component: GroupMeetingListComponent,
      },
      {
        path: 'add',
        component: GroupMeetingAddComponent,
      },
      {
        path: 'edit/:id',
        component: GroupMeetingAddComponent
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class GroupMeetingRoutingModule { }
