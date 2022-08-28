import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { map, mergeMap } from 'rxjs/operators';
import { BaseResponseModel, PageResponseModel, RoleModel } from '../../../shared';
import { UserService } from '../..';
import { AppState } from '../reducers';
import { RoleActionTypes, RoleLoaded, RolesRequested } from './role.actions';

@Injectable()
export class RoleEffects {
  @Effect()
  loadRolePage$ = this.actions$.pipe(
    ofType<RolesRequested>(RoleActionTypes.RolesRequested),
    mergeMap(({ payload }) => {
      return this.userService.getAllRole(payload.query);
    }),
    map((response) => {
      const data: BaseResponseModel<PageResponseModel<RoleModel[]>> = response;
      return new RoleLoaded({
        data: data.result,
      });
    })
  );

  constructor(
    private actions$: Actions,
    private userService: UserService,
    private store: Store<AppState>
  ) {}
}
