import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { OrganizationAddComponent } from './organization-add/organization-add.component';
import { OrganizationListComponent } from './organization-list/organization-list.component';
import { OrganizationComponent } from './organization.component';

const routes: Routes = [
  {
    path: '',
    component: OrganizationComponent,
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
        component: OrganizationListComponent,
      },
      {
        path: 'add',
        component: OrganizationAddComponent,
      },
      {
        path: 'edit/:id',
        component: OrganizationAddComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrganizationRoutingModule { }
