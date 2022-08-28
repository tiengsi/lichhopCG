import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { BrandnameComponent } from './brandname.component';
import { ViettelAddComponent } from './viettel-add/viettel-add.component';
import { ViettelComponent } from './viettel/viettel.component';
import { VnptAddComponent } from './vnpt-add/vnpt-add.component';
import { VnptComponent } from './vnpt/vnpt.component';

const routes: Routes = [
  {
    path: '',
    component: BrandnameComponent,
    children: [
      {
        path: 'viettel',
        component: ViettelComponent,
      },
      {
        path: 'viettel/add',
        component: ViettelAddComponent,
      },
      {
        path: 'viettel/edit/:id',
        component: ViettelAddComponent,
      },
      {
        path: 'vnpt',
        component: VnptComponent,
      },
      {
        path: 'vnpt/add',
        component: VnptAddComponent,
      },
      {
        path: 'vnpt/edit/:id',
        component: VnptAddComponent,
      },
    ]    
  },  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BrandnameRoutingModule { }
