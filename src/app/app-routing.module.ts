import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
// Components
import { BaseComponent } from './modules/theme/components/base/base.component';
// Auth
import { AdminGuardService } from './core';

const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./modules/auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./modules/auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: 'error',
    loadChildren: () =>
      import('./modules/error/error.module').then((m) => m.ErrorModule),
  },
  {
    path: 'admin',
    component: BaseComponent,
    canActivate: [AdminGuardService],
    loadChildren: () =>
      import('./modules/admin/admin.module').then((m) => m.AdminModule),
  },
  // {
  //   path: '',
  //   loadChildren: () =>
  //     import('./modules/website/website.module').then((m) => m.WebsiteModule),
  // },
  {
    path: 'scheduler',
    loadChildren: () => 
      import('./modules/test-website/test-website.module').then((m) => m.TestWebsiteModule)
  },
  { path: '**', redirectTo: 'error/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
