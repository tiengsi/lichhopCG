// Angular
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
// NgBootstrap
import { NgbProgressbarModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
// Translation
import { TranslateModule } from '@ngx-translate/core';
// Loading bar
import { LoadingBarModule } from '@ngx-loading-bar/core';
// Perfect Scroll bar
import { PERFECT_SCROLLBAR_CONFIG, PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
// Ngx DatePicker
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
// Perfect Scrollbar
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
// SVG inline
import { InlineSVGModule } from 'ng-inline-svg';
// Material
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
// Shared Module
import { SharedModule } from './../../shared/shared.module';

import { HeaderComponent } from './components/header/header.component';
import { AsideLeftComponent } from './components/aside/aside-left.component';
import { FooterComponent } from './components/footer/footer.component';
import { SubheaderComponent } from './components/subheader/subheader.component';
import { BrandComponent } from './components/brand/brand.component';
import { TopbarComponent } from './components/header/topbar/topbar.component';
import { MenuHorizontalComponent } from './components/header/menu-horizontal/menu-horizontal.component';
import { BaseComponent } from './components/base/base.component';
import { HeaderMobileComponent } from './components/header/header-mobile/header-mobile.component';
// Service
import { HtmlClassService, LayoutConfigService } from '../../core';
import { LayoutConfig } from '../../configs/admin-layout/layout.config';
import { PermissionUIService } from 'src/app/core/services/permission.service';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  wheelSpeed: 0.5,
  swipeEasing: true,
  minScrollbarLength: 40,
  maxScrollbarLength: 300
};

// tslint:disable-next-line:typedef
export function initializeLayoutConfig(appConfig: LayoutConfigService) {
  // initialize app by loading default demo layout config
  return () => {
    if (appConfig.getConfig() === null) {
      appConfig.loadConfigs(new LayoutConfig().configs);
    }
  };
}

@NgModule({
  declarations: [
    BaseComponent,
    FooterComponent,

    // headers
    HeaderComponent,
    BrandComponent,
    HeaderMobileComponent,

    // subheader
    SubheaderComponent,

    // topbar components
    TopbarComponent,

    // aside left menu components
    AsideLeftComponent,

    // horizontal menu components
    MenuHorizontalComponent,
  ],
  exports: [
    BaseComponent,
    FooterComponent,

    // headers
    HeaderComponent,
    BrandComponent,
    HeaderMobileComponent,

    // subheader
    SubheaderComponent,

    // topbar components
    TopbarComponent,

    // aside left menu components
    AsideLeftComponent,

    // horizontal menu components
    MenuHorizontalComponent,
  ],
  providers: [
    HtmlClassService,
    LayoutConfigService,    
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    },
    {
      // layout config initializer
      provide: APP_INITIALIZER,
      useFactory: initializeLayoutConfig,
      deps: [LayoutConfigService],
      multi: true
    },
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    PerfectScrollbarModule,
    FormsModule,
    MatProgressBarModule,
    MatTabsModule,
    MatButtonModule,
    MatTooltipModule,
    TranslateModule.forChild(),
    LoadingBarModule,
    NgxDaterangepickerMd,
    InlineSVGModule,

    // ng-bootstrap modules
    NgbProgressbarModule,
    NgbTooltipModule,
  ]
})
export class ThemeModule {
}
