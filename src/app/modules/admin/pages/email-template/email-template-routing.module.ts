import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { AddComponent } from './add/add.component';
import { EmailTemplateComponent } from './email-template.component';
import { ListComponent } from './list/list.component';

const routes: Routes = [
  {
    path: '',
    component: EmailTemplateComponent,
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
        component: ListComponent,
      },
      {
        path: 'add',
        component: AddComponent,
      },
      {
        path: 'edit/:id',
        component: AddComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmailTemplateRoutingModule { }
