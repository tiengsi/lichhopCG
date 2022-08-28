import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { OfficerAddComponent } from './officer-add/officer-add.component';
import { OfficerEditComponent } from './officer-edit/officer-edit.component';
import { OfficerListComponent } from './officer-list/officer-list.component';
import { OfficerComponent } from './officer.component';

export const routes: Routes = [
  {
    path: '',
    component: OfficerComponent,
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
        component: OfficerListComponent,
      },
      {
        path: 'add',
        component: OfficerAddComponent,
      },
      {
        path: 'edit/:id',
        component: OfficerEditComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class OfficerRoutingModule { }
