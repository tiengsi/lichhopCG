import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WebsiteComponent } from './website.component';
import { PreloadAllModules, RouterModule } from '@angular/router';
import { routes, WebsiteRoutingModule } from './website-routing.module';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';

import { HeaderComponent } from './components/header/header.component';
import { MenuTopComponent } from './components/menu-top/menu-top.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FooterComponent } from './components/footer/footer.component';
import { ScheduleDetailTabComponent } from './components/schedule-detail-tab/schedule-detail-tab.component';

@NgModule({
  imports: [
    CommonModule,
    WebsiteRoutingModule,
    FormsModule,
    NgbModule,
    LoadingBarHttpClientModule
  ],
  declarations: [
    WebsiteComponent,
    HeaderComponent,
    MenuTopComponent,
    FooterComponent,
    ScheduleDetailTabComponent
  ],
})
export class WebsiteModule {
  static forRoot() {
    return {
      ngModule: WebsiteModule,
      providers: [],
    };
  }
}
