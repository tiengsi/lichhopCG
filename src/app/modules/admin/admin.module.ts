import {
  ErrorInterceptor,
  TokenInterceptor,
} from './../../core/interceptors/http.token.interceptor';
import {
  ToastService,
  PostService,
  LayoutUtilsService,
  CategoryService,
  SettingService,
  ScheduleService,
  ParticipantService,
} from './../../core';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { AngularEditorModule } from '@kolkov/angular-editor';
// import { ModuleWithProviders } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { FileUploadModule } from 'ng2-file-upload';

import {
  NgbModule,
  NgbDropdownModule,
  NgbNavModule,
  NgbTooltipModule,
} from '@ng-bootstrap/ng-bootstrap';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

// NGRX
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

// User
import { usersReducer } from '../../core/ngrx-store/user/user.reducers';
import { UserEffects } from '../../core/ngrx-store/user/user.effects';

// Role
import { RolesReducer } from '../../core/ngrx-store/role/role.reducers';
import { RoleEffects } from '../../core/ngrx-store/role/role.effects';

import { routes } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { SharedModule } from 'src/app/shared/shared.module';

import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { KtDialogService, UserService } from '../../core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LocationService } from 'src/app/core/services/location.service';
import { DepartmentService } from 'src/app/core/services/department.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { GroupMeetingService } from 'src/app/core/services/group-meeting.service';
import { EmailLogsListComponent } from './pages/email-logs/email-logs-list.component';
import { EmailLogsService } from 'src/app/core/services/email-logs.service';
import { StatisticalService } from 'src/app/core/services/statistical.service';
import { ScheduleDetailComponent } from './components/schedule-detail/schedule-detail.component';
import { EmailSmsLogComponent } from './components/email-sms-log/email-sms-log.component';
import { RepresentativeModalComponent } from './components/representative-modal/representative-modal.component';
import { TitleTemplateService } from 'src/app/core/services/title-template.service';
import { TitleTemplateListComponent } from './pages/title-template/title-template-list/title-template-list.component';
import { AddEditTitleTemplateComponent } from './components/add-edit-title-template/add-edit-title-template.component';
import { OrganizationAddComponent } from './pages/organization/organization-add/organization-add.component';
import { OrganizationListComponent } from './pages/organization/organization-list/organization-list.component';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { BrandnameComponent } from './pages/brandname/brandname.component';
import { BrandNameService } from 'src/app/core/services/brandname.service';
import { EmailTemplateComponent } from './pages/email-template/email-template.component';
import { EmailTemplateService } from 'src/app/core/services/email-template.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { ViettelComponent } from './pages/brandname/viettel/viettel.component';
import { VnptComponent } from './pages/brandname/vnpt/vnpt.component';
import { ViettelAddComponent } from './pages/brandname/viettel-add/viettel-add.component';
import { VnptAddComponent } from './pages/brandname/vnpt-add/vnpt-add.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    PerfectScrollbarModule,
    LoadingBarModule,
    LoadingBarHttpClientModule,
    RouterModule.forChild(routes),
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
    EffectsModule.forFeature([UserEffects, RoleEffects]),
  ],
  declarations: [
    AdminComponent,
    DashboardComponent,
    EmailLogsListComponent,
    ScheduleDetailComponent,
    EmailSmsLogComponent,
    RepresentativeModalComponent,
    TitleTemplateListComponent,
    AddEditTitleTemplateComponent,
    OrganizationAddComponent,
    OrganizationListComponent,    
    BrandnameComponent, EmailTemplateComponent, ViettelComponent, VnptComponent, ViettelAddComponent, VnptAddComponent,    
  ],
  exports: [AdminComponent],
  providers: [
    KtDialogService,
    UserService,
    LayoutUtilsService,
    ToastService,
    PostService,
    CategoryService,
    SettingService,
    DepartmentService,
    LocationService,
    ScheduleService,
    GroupMeetingService,
    ParticipantService,
    EmailLogsService,
    StatisticalService,
    TitleTemplateService,
    OrganizationService,
    BrandNameService,
    EmailTemplateService,
    PermissionUIService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
    PermissionList
  ],
})
export class AdminModule {
  static forRoot(): any {
    return {
      ngModule: AdminModule,
      providers: [ToastService],
    };
  }
}
