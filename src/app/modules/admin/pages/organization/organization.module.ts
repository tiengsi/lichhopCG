import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrganizationRoutingModule } from './organization-routing.module';
import { OrganizationComponent } from './organization.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { LocationRoutingModule } from '../location/location-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { FileUploadModule } from 'ng2-file-upload';
import { NgbDropdownModule, NgbModule, NgbNavModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { usersReducer } from 'src/app/core/ngrx-store/user/user.reducers';
import { RolesReducer } from 'src/app/core/ngrx-store/role/role.reducers';
import { UserEffects } from 'src/app/core/ngrx-store/user/user.effects';
import { RoleEffects } from 'src/app/core/ngrx-store/role/role.effects';


@NgModule({
  declarations: [OrganizationComponent],
  imports: [
    CommonModule,
    OrganizationRoutingModule,    
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    LoadingBarModule,
    LoadingBarHttpClientModule,
    LocationRoutingModule,
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
export class OrganizationModule { }
