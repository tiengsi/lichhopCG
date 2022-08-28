import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmailTemplateRoutingModule } from './email-template-routing.module';
import { ListComponent } from './list/list.component';
import { AddComponent } from './add/add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { TranslateModule } from '@ngx-translate/core';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { FileUploadModule } from 'ng2-file-upload';
import { NgbDropdownModule, NgbModule, NgbNavModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { RoleEffects } from 'src/app/core/ngrx-store/role/role.effects';
import { RolesReducer } from 'src/app/core/ngrx-store/role/role.reducers';
import { UserEffects } from 'src/app/core/ngrx-store/user/user.effects';
import { usersReducer } from 'src/app/core/ngrx-store/user/user.reducers';


@NgModule({
  declarations: [ListComponent, AddComponent],
  imports: [
    CommonModule,
    EmailTemplateRoutingModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    LoadingBarModule,
    LoadingBarHttpClientModule,    
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
  ]
})
export class EmailTemplateModule { }
