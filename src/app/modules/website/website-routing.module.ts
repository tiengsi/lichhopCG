import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrganContactComponent } from './pages/post/containers/organ-contact/organ-contact.component';
import { WebsiteComponent } from './website.component';

export const routes: Routes = [
  {
    path: '',
    component: WebsiteComponent,
    children: [
      {
        path: '',
        loadChildren: () =>
          import('./pages/home/home.module').then((m) => m.HomeModule),
      },
      {
        path: 'tin',
        loadChildren: () =>
          import('./pages/post/post.module').then((m) => m.PostModule),
      },
      {
        path: 'danh-ba-co-quan',
        component: OrganContactComponent,
      },  
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WebsiteRoutingModule {}
