import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { DepartmentAddComponent } from './add/department-add.component';
import { DepartmentComponent } from './department.component';
import { DepartmentListComponent } from './list/department-list.component';

export const routes: Routes = [
  {
    path: '',
    component: DepartmentComponent,
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
        component: DepartmentListComponent,
      },
      {
        path: 'add',
        component: DepartmentAddComponent,
      },
      {
        path: 'edit/:id',
        component: DepartmentAddComponent
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class DepartmentRoutingModule { }
