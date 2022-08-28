import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupMeetingComponent } from './group-meeting.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbDropdownModule, NgbModule, NgbNavModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { GroupMeetingRoutingModule } from './group-meeting-routing.module';
import { StoreModule } from '@ngrx/store';
import { usersReducer } from 'src/app/core/ngrx-store/user/user.reducers';
import { RolesReducer } from 'src/app/core/ngrx-store/role/role.reducers';
import { UserEffects } from 'src/app/core/ngrx-store/user/user.effects';
import { EffectsModule } from '@ngrx/effects';
import { RoleEffects } from 'src/app/core/ngrx-store/role/role.effects';
import { GroupMeetingListComponent } from './group-meeting-list/group-meeting-list.component';
import { GroupMeetingAddComponent } from './group-meeting-add/group-meeting-add.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    LoadingBarModule,
    LoadingBarHttpClientModule,
    GroupMeetingRoutingModule,
    TranslateModule.forChild(),
    NgbModule,
    NgbDropdownModule,
    NgbNavModule,
    NgbTooltipModule,
    NgSelectModule,
    StoreModule.forFeature('users', usersReducer),
    StoreModule.forFeature('roles', RolesReducer),
    EffectsModule.forFeature([UserEffects, RoleEffects]),
  ],
  declarations: [GroupMeetingComponent, GroupMeetingListComponent, GroupMeetingAddComponent]
})
export class GroupMeetingModule { }
