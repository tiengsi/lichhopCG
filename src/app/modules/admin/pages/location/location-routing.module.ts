import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { LocationAddComponent } from './location-add/location-add.component';
import { LocationListComponent } from './location-list/location-list.component';
import { LocationComponent } from './location.component';

export const routes: Routes = [
  {
    path: '',
    component: LocationComponent,
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
        component: LocationListComponent,
      },
      {
        path: 'add',
        component: LocationAddComponent,
      },
      {
        path: 'edit/:id',
        component: LocationAddComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class LocationRoutingModule { }
