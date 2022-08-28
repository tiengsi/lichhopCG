import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { SettingCommonComponent } from './seting-common/setting-common.component';
import { SettingImageComponent } from './setting-image/setting-image.component';
import { SettingComponent } from './setting.component';
export const routes: Routes = [
  {
    path: '',
    component: SettingComponent,
    canActivate: [NgxPermissionsGuard],
    data: {
      permissions: {
        only: [ROLES.Admin, ROLES.SuperAdmin, ROLES.NormalAdmin],
        redirectTo: '/error/403'
      }
    },
    children: [
      {
        path: '',
        component: SettingCommonComponent,
      },
      {
        path: 'setting-image',
        component: SettingImageComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class SettingRoutingModule { }
