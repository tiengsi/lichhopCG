import { UserModel } from './../../../shared/models/user.model';
import { PageResponseModel } from './../../../shared/models/page-response.model';
import { BaseResponseModel } from './../../../shared/models/base-response.model';
import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { map, mergeMap, tap } from 'rxjs/operators';
import { UserService } from '../../index';
import {
  GetUserById,
  GetUserByIdFail,
  GetUserByIdSuccess,
  UpdateUser,
  UpdateUserFail,
  UpdateUserSuccess,
  UserActionTypes,
  UserAdd,
  UserAddFail,
  UserAddSuccess,
  UserDeleted,
  UserDeleteFail,
  UserDeleteSuccess,
  UsersPageLoaded,
  UsersPageRequested,
} from './user.actions';

@Injectable()
export class UserEffects {
  @Effect()
  loadUsersPage$ = this.actions$.pipe(
    ofType<UsersPageRequested>(UserActionTypes.UsersPageRequested),
    mergeMap(({ payload }) => {
      return this.userService.getAll(payload.query);
    }),
    map((response) => {
      const data: BaseResponseModel<PageResponseModel<UserModel[]>> = response;
      return new UsersPageLoaded({
        data: data.result,
      });
    })
  );

  @Effect()
  deleteUser$ = this.actions$.pipe(
    ofType<UserDeleted>(UserActionTypes.UserDeleted),
    mergeMap(({ payload }) => {
      return this.userService.deleteUser(payload.id);
    }),
    map((response) => {
      if (response.isSuccess) {
        return new UserDeleteSuccess(response.message);
      } else {
        return new UserDeleteFail();
      }
    })
  );

  @Effect()
  createUser$ = this.actions$.pipe(
    ofType<UserAdd>(UserActionTypes.UserAdd),
    mergeMap(({ payload }) => {
      return this.userService.createUser(payload.user);
    }),
    map((response) => {
      console.log(response);
      if (response.isSuccess) {
        return new UserAddSuccess();
      } else {
        return new UserAddFail(response.toString());
      }
    })
  );

  @Effect()
  getUser$ = this.actions$.pipe(
    ofType<GetUserById>(UserActionTypes.GetUserById),
    mergeMap(({ payload }) => {
      return this.userService.getUserById(payload.id);
    }),
    map((response) => {
      if (response.isSuccess) {
        const payload = {
          user: response.result
        };
        return new GetUserByIdSuccess(payload);
      } else {
        const payload = {
          errorMessage: response.message
        };
        return new GetUserByIdFail(payload);
      }
    })
  );

  @Effect()
  updateUser$ = this.actions$.pipe(
    ofType<UpdateUser>(UserActionTypes.UpdateUser),
    mergeMap(({ payload }) => {
      return this.userService.updateUser(payload.user);
    }),
    map((response) => {
      if (response.isSuccess) {
        return new UpdateUserSuccess();
      } else {
        const payload = {
          errorMessage: response.message
        };
        return new UpdateUserFail(payload);
      }
    })
  );

  constructor(
    private actions$: Actions,
    private userService: UserService,
  ) {}
}
