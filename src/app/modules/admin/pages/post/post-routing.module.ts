import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { ROLES } from 'src/app/shared/models/permission';
import { PostAddComponent } from './post-add/post-add.component';
import { PostEditComponent } from './post-edit/post-edit.component';
import { PostListComponent } from './post-list/post-list.component';
import { PostComponent } from './post.component';

export const routes: Routes = [
  {
    path: '',
    component: PostComponent,
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
        component: PostListComponent,
      },
      {
        path: 'add',
        component: PostAddComponent,
      },
      {
        path: 'edit/:id',
        component: PostEditComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class PostRoutingModule { }
