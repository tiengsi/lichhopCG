import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SubordinateComponent } from './components/subordinate/subordinate.component';
import { OrganContactComponent } from './containers/organ-contact/organ-contact.component';
import { PostDetailComponent } from './containers/post-detail/post-detail.component';
import { PostRoutingModule, routes } from './post-routing.module';
import { PostComponent } from './post.component';

@NgModule({
  declarations: [
    PostComponent,
    PostDetailComponent,
    OrganContactComponent,
    SubordinateComponent
  ],
  imports: [
    CommonModule,
    PostRoutingModule,
    FormsModule,
    NgbModule,
  ],
  exports: [RouterModule]
})
export class PostModule { }
