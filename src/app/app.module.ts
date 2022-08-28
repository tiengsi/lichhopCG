import { BrowserModule, HammerModule, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { NgxChartsModule } from '@swimlane/ngx-charts';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SplashScreenComponent } from './shared/components/splash-screen/splash-screen.component';

// Hammer JS
import 'hammerjs';

// SVG inline
import { InlineSVGModule } from 'ng-inline-svg';

// NGRX
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreRouterConnectingModule } from '@ngrx/router-store';

// State
import { metaReducers, reducers } from './core/ngrx-store/reducers';

// Interceptor
import {
  TokenInterceptor,
  ErrorInterceptor
} from './core/interceptors/http.token.interceptor';

// services
import {
  LayoutConfigService,
  SplashScreenService,
  AuthService,
  AdminGuardService,
  LayoutRefService,
  MenuAsideService,
  MenuConfigService,
  PageConfigService,
  MenuHorizontalService,
  SubheaderService,
} from './core';

// modules
import { AuthModule } from './modules/auth/auth.module';
import { AdminModule } from './modules/admin/admin.module';
import { ThemeModule } from './modules/theme/theme.module';
import { SharedModule } from './shared/shared.module';
import { WebsiteModule } from './modules/website/website.module';
import { HammerConfig } from './configs/admin-layout/hammer.config';
import { NgxPermissionsConfigurationStore, NgxPermissionsGuard, NgxPermissionsModule, NgxPermissionsStore, NgxRolesStore } from 'ngx-permissions';
import { ToastrModule } from 'ngx-toastr';
import { TestWebsiteModule } from './modules/test-website/test-website.module';


@NgModule({
  declarations: [
    AppComponent,
    SplashScreenComponent,    
  ],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    BrowserModule,
    HammerModule,
    AppRoutingModule,
    MatProgressSpinnerModule,
    StoreModule.forRoot(reducers, {metaReducers}),
    EffectsModule.forRoot([]),
    StoreRouterConnectingModule.forRoot({stateKey: 'router'}),
    StoreDevtoolsModule.instrument(),
    AuthModule.forRoot(),
    AdminModule.forRoot(),
    TranslateModule.forRoot(),
    HttpClientModule,
    InlineSVGModule.forRoot(),
    ThemeModule,
    SharedModule,
    WebsiteModule.forRoot(),
    NgxPermissionsModule.forChild(),
    ToastrModule.forRoot(),
    NgxChartsModule,
    //TestWebsiteModule.forRoot()
  ],
  providers: [
    LayoutConfigService,
    SplashScreenService,
    AuthService,
    AdminGuardService,
    LayoutRefService,
    MenuAsideService,
    MenuConfigService,
    PageConfigService,
    MenuHorizontalService,
    SubheaderService,
    NgxPermissionsGuard,
    NgxPermissionsStore,
    NgxRolesStore,
    NgxPermissionsConfigurationStore,
    {
      provide: HAMMER_GESTURE_CONFIG,
      useClass: HammerConfig
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
