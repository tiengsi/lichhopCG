import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';

import { AppState } from './../ngrx-store/reducers/index';
import { isLoggedIn } from '../ngrx-store/auth/_selectors/auth.selectors';
import { tap } from 'rxjs/operators';

@Injectable()
export class AdminGuardService implements CanActivate {
  constructor(private store: Store<AppState>, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.store
      .pipe(
        select(isLoggedIn),
        tap(loggedIn => {
          if (!loggedIn) {
            this.router.navigateByUrl('/auth/login');
          }
        })
      );
  }
}
