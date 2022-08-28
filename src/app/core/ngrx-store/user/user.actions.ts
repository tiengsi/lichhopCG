import { Update } from '@ngrx/entity';
import { Action } from '@ngrx/store';
import {
  PageResponseModel,
  UserModel,
  QueryParamsModel,
} from './../../../shared/';

export enum UserActionTypes {
  UsersPageRequested = '[Users List Page] Users Page Requested',
  UsersPageLoaded = '[Users API] Users Page Loaded',

  UserDeleted = '[Users List Page] User Deleted',
  UserDeleteSuccess = '[USER List Page] Delete Success',
  UserDeleteFail = '[USER List Page] Delete Fail',

  UserAdd = '[USER Add Page] Add User',
  UserAddSuccess = '[USER Add Page] Add User Success',
  UserAddFail = '[USER Add Page] Add User Fail',

  GetUserById = '[USER Edit Page] Get User By Id',
  GetUserByIdSuccess = '[USER Edit Page] Get User By Id Success',
  GetUserByIdFail = '[USER Edit Page] Get User By Id Fail',

  UpdateUser = '[USER Edit Page] Update User',
  UpdateUserSuccess = '[USER Edit Page] Update User Success',
  UpdateUserFail = '[USER Edit Page] Update User Fail',
}

export class UsersPageRequested implements Action {
  readonly type = UserActionTypes.UsersPageRequested;
  constructor(public payload: { query: QueryParamsModel }) {}
}

export class UsersPageLoaded implements Action {
  readonly type = UserActionTypes.UsersPageLoaded;
  constructor(public payload: { data: PageResponseModel<UserModel[]> }) {}
}

export class UserDeleted implements Action {
  readonly type = UserActionTypes.UserDeleted;
  constructor(public payload: { id: number }) {}
}

export class UserDeleteSuccess implements Action {
  public readonly type = UserActionTypes.UserDeleteSuccess;
  constructor(public payload: string) {}
}

export class UserDeleteFail implements Action {
  public readonly type = UserActionTypes.UserDeleteFail;
  constructor() {}
}

export class UserAdd implements Action {
  public readonly type = UserActionTypes.UserAdd;
  constructor(public payload: { user: UserModel }) {}
}

export class UserAddSuccess implements Action {
  public readonly type = UserActionTypes.UserAddSuccess;
  constructor() {}
}

export class UserAddFail implements Action {
  public readonly type = UserActionTypes.UserAddFail;
  constructor(public error: string) {}
}

export class GetUserById implements Action {
  public readonly type = UserActionTypes.GetUserById;
  constructor(public payload: { id: number }) {}
}

export class GetUserByIdSuccess implements Action {
  public readonly type = UserActionTypes.GetUserByIdSuccess;
  constructor(public payload: { user: UserModel }) {}
}

export class GetUserByIdFail implements Action {
  public readonly type = UserActionTypes.GetUserByIdFail;
  constructor(public payload: { errorMessage: string }) {}
}

export class UpdateUser implements Action {
  public readonly type = UserActionTypes.UpdateUser;
  constructor(
    public payload: {
      partialUser: Update<UserModel>;
      user: UserModel; // For Server update (through service)
    }
  ) {}
}

export class UpdateUserSuccess implements Action {
  public readonly type = UserActionTypes.UpdateUserSuccess;
  constructor() {}
}

export class UpdateUserFail implements Action {
  public readonly type = UserActionTypes.UpdateUserFail;
  constructor(public payload: { errorMessage: string }) {}
}

export type UserActions =
  | UsersPageRequested
  | UsersPageLoaded
  | UserDeleted
  | UserDeleteSuccess
  | UserDeleteFail
  | UserAdd
  | UserAddSuccess
  | UserAddFail
  | GetUserById
  | GetUserByIdSuccess
  | GetUserByIdFail
  | UpdateUser
  | UpdateUserSuccess
  | UpdateUserFail;
