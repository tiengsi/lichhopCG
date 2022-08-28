import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticalComponent } from './statistical.component';
import { StatisticalRoutingModule } from './statistical-routing.module';
import { StatisticalDayComponent } from './statistical-day/statistical-day.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoadingBarModule } from '@ngx-loading-bar/core';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import {
  NgbDropdownModule,
  NgbModule,
  NgbTooltipModule,
  NgbNavModule,
} from '@ng-bootstrap/ng-bootstrap';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { TranslateModule } from '@ngx-translate/core';
import { UserEffects } from 'src/app/core/ngrx-store/user/user.effects';
import { RoleEffects } from 'src/app/core/ngrx-store/role/role.effects';
import { usersReducer } from 'src/app/core/ngrx-store/user/user.reducers';
import { RolesReducer } from 'src/app/core/ngrx-store/role/role.reducers';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { StatisticalMonthComponent } from './statistical-month/statistical-month.component';
import { StatisticalYearComponent } from './statistical-year/statistical-year.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    LoadingBarModule,
    LoadingBarHttpClientModule,
    TranslateModule.forChild(),
    NgbModule,
    NgbDropdownModule,
    NgbNavModule,
    NgbTooltipModule,
    NgSelectModule,
    StoreModule.forFeature('users', usersReducer),
    StoreModule.forFeature('roles', RolesReducer),
    EffectsModule.forFeature([UserEffects, RoleEffects]),
    StatisticalRoutingModule,
    NgxChartsModule,
  ],
  declarations: [
    StatisticalComponent,
    StatisticalDayComponent,
    StatisticalMonthComponent,
    StatisticalYearComponent,
  ],
})
export class StatisticalModule {}
