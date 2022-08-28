import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { StatisticalDayComponent } from './statistical-day/statistical-day.component';
import { StatisticalComponent } from './statistical.component';
import { StatisticalMonthComponent } from './statistical-month/statistical-month.component';
import { StatisticalYearComponent } from './statistical-year/statistical-year.component';

export const routes: Routes = [
  {
    path: '',
    component: StatisticalComponent,
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
        component: StatisticalDayComponent,
      },
      {
        path: 'month',
        component: StatisticalMonthComponent,
      },
      {
        path: 'year',
        component: StatisticalYearComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class StatisticalRoutingModule { }
