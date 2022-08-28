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
import { RoleEffects } from 'src/app/core/ngrx-store/role/role.effects';
import { RolesReducer } from 'src/app/core/ngrx-store/role/role.reducers';
import { UserEffects } from 'src/app/core/ngrx-store/user/user.effects';
import { usersReducer } from 'src/app/core/ngrx-store/user/user.reducers';
import { SharedModule } from 'src/app/shared/shared.module';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { UserAddComponent } from './user-add/user-add.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';

@NgModule({
  declarations: [
    UserComponent,
    UserListComponent,
    UserAddComponent,
    UserEditComponent,
    ChangePasswordComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    LoadingBarModule,
    LoadingBarHttpClientModule,
    UserRoutingModule,
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

export class UserModule { }
