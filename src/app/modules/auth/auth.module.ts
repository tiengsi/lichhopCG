import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ModuleWithProviders } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

// Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';

// NGRX
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

// Auth
import { AuthEffects, authReducer } from '../../core/ngrx-store/auth';

import { AuthComponent } from './auth.component';
import { AdminLoginComponent } from './pages/admin-login/admin-login.component';
import { AuthNoticeComponent } from './components/auth-notice/auth-notice.component';
import { AuthService } from '../../core';

const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
    children: [
      {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full',
      },
      {
        path: 'login',
        component: AdminLoginComponent,
        data: { returnUrl: window.location.pathname },
      },
    ],
  },
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    RouterModule.forChild(routes),
    TranslateModule.forChild(),
    StoreModule.forFeature('auth', authReducer),
    EffectsModule.forFeature([AuthEffects]),
  ],
  declarations: [AuthComponent, AdminLoginComponent, AuthNoticeComponent],
  exports: [AuthComponent],
})
export class AuthModule {
  static forRoot(): ModuleWithProviders<any> {
    return {
      ngModule: AuthModule,
      providers: [AuthService],
    };
  }
}
