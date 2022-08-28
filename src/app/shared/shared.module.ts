import { ToastService } from './../core/services/toast.service';
// Angular
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// NgBootstrap
import { NgbDropdownModule, NgbModule, NgbNavModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
// Perfect Scrollbar
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

// SVG inline
import { InlineSVGModule } from 'ng-inline-svg';

// Layout Directives
import {
  // ContentAnimateDirective,
  HeaderDirective,
  MenuDirective,
  OffCanvasDirective,
  ScrollTopDirective,
  // SparklineChartDirective,
  StickyDirective,
  TabClickEventDirective,
  ToggleDirective,
} from './index';

// Components
import { ScrollTopComponent } from './components/scroll-top/scroll-top.component';
import { NotificationComponent } from './components/admin-topbar/notification/notification.component';
import { LanguageSelectorComponent } from './components/admin-topbar/language-selector/language-selector.component';
import { UserProfileComponent } from './components/admin-topbar/user-profile/user-profile.component';
import { SearchDropdownComponent } from './components/admin-topbar/search-dropdown/search-dropdown.component';
import { SearchResultComponent } from './components/admin-topbar/search-result/search-result.component';
import { PortletComponent } from './components/admin-portlet/portlet/portlet.component';
import { PortletHeaderComponent } from './components/admin-portlet/portlet-header/portlet-header.component';
import { PortletFooterComponent } from './components/admin-portlet/portlet-footer/portlet-footer.component';
import { PortletBodyComponent } from './components/admin-portlet/portlet-body/portlet-body.component';
import { DeleteEntityDialogComponent } from './components/content/delete-entity-dialog/delete-entity-dialog.component';
import { ToastComponent } from './components/toast/toast.component';

// Pipes
import { FirstLetterPipe } from './pipes/first-letter.pipe';
import { TranslateModule } from '@ngx-translate/core';
import { SortableHeaderDirective } from './directives/sortable-header.directive';
import { ScheduleStatusPipe } from './pipes/schedule-status.pipe';

@NgModule({
  imports: [
    CommonModule,
    InlineSVGModule,
    PerfectScrollbarModule,
    // ng-bootstrap modules
    NgbDropdownModule,
    NgbNavModule,
    NgbTooltipModule,
    TranslateModule.forChild(),
    NgbModule
  ],
  declarations: [
    // directives
    ScrollTopDirective,
    HeaderDirective,
    OffCanvasDirective,
    ToggleDirective,
    MenuDirective,
    TabClickEventDirective,
    // SparklineChartDirective,
    // ContentAnimateDirective,
    StickyDirective,
    // pipes
    // TimeElapsedPipe,
    // JoinPipe,
    // GetObjectPipe,
    ScheduleStatusPipe,
    FirstLetterPipe,
    // components
    ScrollTopComponent,
    NotificationComponent,
    LanguageSelectorComponent,
    UserProfileComponent,
    SearchDropdownComponent,
    SearchResultComponent,
    PortletComponent,
    PortletHeaderComponent,
    PortletFooterComponent,
    PortletBodyComponent,
    DeleteEntityDialogComponent,
    ToastComponent,
    SortableHeaderDirective
  ],
  exports: [
    // directives
    ScrollTopDirective,
    HeaderDirective,
    OffCanvasDirective,
    ToggleDirective,
    MenuDirective,
    TabClickEventDirective,
    // SparklineChartDirective,
    // ContentAnimateDirective,
    StickyDirective,
    // pipes
    // TimeElapsedPipe,
    // JoinPipe,
    // GetObjectPipe,
    ScheduleStatusPipe,
    FirstLetterPipe,
    // components
    ScrollTopComponent,
    NotificationComponent,
    LanguageSelectorComponent,
    UserProfileComponent,
    SearchDropdownComponent,
    SearchResultComponent,
    PortletComponent,
    PortletHeaderComponent,
    PortletFooterComponent,
    PortletBodyComponent,
    DeleteEntityDialogComponent,
    ToastComponent,
    SortableHeaderDirective
  ],
  providers: [ToastService],
})
export class SharedModule {}
