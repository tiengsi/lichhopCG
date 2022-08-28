import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { NgbDropdownModule, NgbModule, NgbNavModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { TranslateModule } from '@ngx-translate/core';
import { FileUploadModule } from 'ng2-file-upload';
import { NgxPermissionsModule } from 'ngx-permissions';
import { RoleEffects } from 'src/app/core/ngrx-store/role/role.effects';
import { RolesReducer } from 'src/app/core/ngrx-store/role/role.reducers';
import { UserEffects } from 'src/app/core/ngrx-store/user/user.effects';
import { usersReducer } from 'src/app/core/ngrx-store/user/user.reducers';
import { SharedModule } from 'src/app/shared/shared.module';
import { PostAddComponent } from './post-add/post-add.component';
import { PostEditComponent } from './post-edit/post-edit.component';
import { PostListComponent } from './post-list/post-list.component';
import { PostRoutingModule } from './post-routing.module';
import { PostComponent } from './post.component';

@NgModule({
  declarations: [
    PostComponent,
    PostListComponent,
    PostEditComponent,
    PostAddComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    LoadingBarModule,
    LoadingBarHttpClientModule,
    PostRoutingModule,
    TranslateModule.forChild(),
    AngularEditorModule,
    FileUploadModule,
    NgbModule,
    NgbDropdownModule,
    NgbNavModule,
    NgbTooltipModule,
    NgSelectModule,
    StoreModule.forFeature('users', usersReducer),
    StoreModule.forFeature('roles', RolesReducer),
    EffectsModule.forFeature([UserEffects, RoleEffects])
  ],
})

export class PostModule { }
