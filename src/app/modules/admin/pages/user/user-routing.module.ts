import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { UserAddComponent } from './user-add/user-add.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserComponent } from './user.component';

export const routes: Routes = [
  {
    path: '',
    component: UserComponent,
    canActivate: [NgxPermissionsGuard],
    data: {
      permissions: {
        only: [ROLES.Admin, ROLES.SuperAdmin],
        redirectTo: '/error/403'
      }
    },
    children: [
      {
        path: '',
        component: UserListComponent,
      },
      {
        path: 'add',
        component: UserAddComponent,
      },
      {
        path: 'edit/:id',
        component: UserEditComponent,
      },
      {
        path: 'change-password',
        component: ChangePasswordComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class UserRoutingModule { }
