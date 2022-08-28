import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrganContactComponent } from './containers/organ-contact/organ-contact.component';
import { PostDetailComponent } from './containers/post-detail/post-detail.component';
import { PostComponent } from './post.component';

export const routes: Routes = [
  {
    path: '',
    component: PostComponent,
  },
  {
    path: 'danh-sach/:categoryCode',
    component: PostComponent,
  },
  {
    path: 'thong-tin-chi-dao-dieu-hanh',
    component: PostComponent,
  },
  {
    path: 'thong-bao',
    component: PostComponent,
  },
  {
    path: 'gioi-thieu',
    component: PostComponent,
  },
  {
    path: 'chi-tiet/:categoryCode/:id',
    component: PostDetailComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PostRoutingModule {}
