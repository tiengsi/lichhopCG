import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ErrorComponent } from './error.component';
import { Error404Component } from './components/error-404/error-404.component';
import { Error403Component } from './components/error-403/error-403.component';

@NgModule({
  declarations: [
    ErrorComponent,
    Error404Component,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {
        path: '',
        component: ErrorComponent,
        children: [
          {
            path: '404',
            component: Error404Component,
          },
          {
            path: '403',
            component: Error403Component,
          },
        ],
      },
    ]),
  ],
})
export class ErrorModule {}
