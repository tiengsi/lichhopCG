// Angular
import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
// RxJS
import { mergeMap, tap } from 'rxjs/operators';
// NGRX
import { Actions, Effect, ofType } from '@ngrx/effects';
import { Action, select, Store } from '@ngrx/store';
// Auth actions
import { AuthActionTypes, Login, Logout, Register } from '../_actions/auth.actions';
import { AppState } from '../../reducers';
import { environment } from '../../../../../environments/environment';
import { defer, Observable, of } from 'rxjs';
import { AuthModel } from '../../../../shared';
import { ROLES_PERMISSIONS } from 'src/app/shared/models/permission';
import { NgxPermissionsService, NgxRolesService } from 'ngx-permissions';

@Injectable()
export class AuthEffects {
  @Effect({ dispatch: false })
  login$ = this.actions$.pipe(
    ofType<Login>(AuthActionTypes.Login),
    tap(action => {
      localStorage.setItem(environment.authTokenKey, JSON.stringify(action.payload));
      this.setPermissions(action?.payload?.roles);
    }),
  );

  @Effect({ dispatch: false })
  logout$ = this.actions$.pipe(
    ofType<Logout>(AuthActionTypes.Logout),
    tap(() => {
      localStorage.removeItem(environment.authTokenKey);      
      this.router.navigate(['/auth/login'], { queryParams: { returnUrl: this.returnUrl } });
    })
  );

  @Effect({ dispatch: false })
  register$ = this.actions$.pipe(
    ofType<Register>(AuthActionTypes.Register),
    tap(action => {
      localStorage.setItem(environment.authTokenKey, JSON.stringify(action.payload));
    })
  );

  // Keep login when reload
  @Effect()
  init$: Observable<Action> = defer(() => {
    const userToken = localStorage.getItem(environment.authTokenKey);
    const objUser: AuthModel = JSON.parse(userToken);
    let observableResult = of({ type: 'NO_ACTION' });
    if (userToken) {
      observableResult = of(new Login(objUser));
    }
    return observableResult;
  });

  private returnUrl: string;

  constructor(
    private actions$: Actions,
    private router: Router,
    private permissionsService: NgxPermissionsService,
    private rolesService: NgxRolesService,
    private store: Store<AppState>) {

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.returnUrl = event.url;
      }
    });
  }

  private setPermissions(roles: string[]): void {
    this.permissionsService.flushPermissions();
    this.rolesService.flushRoles();
    if (!roles) { return; }

    Object.entries(ROLES_PERMISSIONS).forEach(item => {
      const foundIndex = roles.indexOf(item[0]);
      if (foundIndex > -1) {
        this.permissionsService.addPermission(item[1]);
        this.rolesService.addRole(item[0], item[1]);
      }
    });
  }
}
