import { AuthModel } from './../../shared/models/auth.model';
import { Injectable, Injector } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

import { AuthService } from '../services/auth.service';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private authService: AuthService;
  constructor(private injector: Injector) {}
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    this.authService = this.injector.get(AuthService);
    const userToken: string = this.authService.getToken();
    const objUser: AuthModel = JSON.parse(userToken);
    const accessToken = userToken == null ? '' : objUser.accessToken;
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`,
        'Content-Type': 'application/json',
      },
    });
    return next.handle(request);
  }
}

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastService: ToastrService) {}
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      tap(
        (event) => {
          if (event instanceof HttpResponse && event.status === 401) {
            this.toastService.warning('Bạn không có đủ quyền để thực hiện thao tác này!');
            localStorage.removeItem(environment.authTokenKey);
            this.router.navigateByUrl('/auth/login');
          }
        },
        (error) => {
          console.error(error);          
          if (error.status === 401) {
            localStorage.removeItem(environment.authTokenKey);
            this.router.navigateByUrl('/auth/login');
          } else if (
            error.status === 409 ||
            error.status === 404 ||
            error.status === 208
          ) {
            this.toastService.warning(error.error.Message);
          } else {
            try{
              if(error.error[0].includes('đã tồn tại')){
                this.toastService.info('Tài khoản này đã tồn tại, xin vui lòng thử lại!');
              }
              else{
                this.toastService.error('Đã có lỗi xảy ra, xin vui lòng thử lại!');    
              }
            }
            catch{
              this.toastService.error('Đã có lỗi xảy ra, xin vui lòng thử lại!');
            }
          }
        }
      )
    );
  }
}
