import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';

import { TestWebsiteRoutingModule } from './test-website-routing.module';
import { TestWebsiteComponent } from './test-website.component';
import { HeaderComponent } from './test-components/header/header.component';
import { FooterComponent } from './test-components/footer/footer.component';
import { MatTabsModule } from '@angular/material/tabs';
import { PollComponent } from './test-components/poll/poll.component';
import { RecentScheduleComponent } from './test-components/recent-schedule/recent-schedule.component';
import { PersonalScheduleComponent } from './test-components/personal-schedule/personal-schedule.component';
import { PersonalDocumentsComponent } from './test-components/personal-documents/personal-documents.component';
import { DepartmentScheduleComponent } from './test-components/department-schedule/department-schedule.component';
import { DocumentsComponent } from './test-components/documents/documents.component';
import { MeetingInfoComponent } from './test-components/recent-schedule/meeting-info/meeting-info.component';
import { MeetingDocumentsComponent } from './test-components/recent-schedule/meeting-documents/meeting-documents.component';
import { MeetingNotesComponent } from './test-components/recent-schedule/meeting-notes/meeting-notes.component';
import { MeetingResultComponent } from './test-components/recent-schedule/meeting-result/meeting-result.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { SelfScheduleComponent } from './test-components/personal-schedule/self-schedule/self-schedule.component';
import { AssignedScheduleComponent } from './test-components/personal-schedule/assigned-schedule/assigned-schedule.component';
import { WeeklyScheduleComponent } from './test-components/personal-schedule/weekly-schedule/weekly-schedule.component';
import { ModalFormComponent } from './test-components/personal-schedule/weekly-schedule/modal-form/modal-form.component';
import { ModalDetailComponent } from './test-components/personal-schedule/weekly-schedule/modal-detail/modal-detail.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { FileUploadModule } from 'ng2-file-upload';
import { QrTestComponent } from './test-components/qr-test/qr-test.component';
import { ChangePasswordComponent } from './test-components/change-password/change-password.component';



@NgModule({
  declarations: [TestWebsiteComponent, HeaderComponent, FooterComponent, PollComponent, RecentScheduleComponent, PersonalScheduleComponent, PersonalDocumentsComponent, DepartmentScheduleComponent, DocumentsComponent, MeetingInfoComponent, MeetingDocumentsComponent, MeetingNotesComponent, MeetingResultComponent, SelfScheduleComponent, AssignedScheduleComponent, WeeklyScheduleComponent, ModalFormComponent, ModalDetailComponent, QrTestComponent, ChangePasswordComponent],
  imports: [
    CommonModule,
    TestWebsiteRoutingModule,
    LoadingBarHttpClientModule,
    NgbModule,
    MatTabsModule,    
    FormsModule,
    CKEditorModule,
    NgSelectModule,
    FileUploadModule,    
    ReactiveFormsModule
  ]
})
export class TestWebsiteModule {
  // static forRoot(){
  //   return {
  //     ngModule: TestWebsiteModule,
  //     providers: [],
  //   };
  // }
 }
