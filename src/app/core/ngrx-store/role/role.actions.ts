import { Action } from '@ngrx/store';
import {
  PageResponseModel,
  QueryParamsModel,
  RoleModel
} from './../../../shared/';

export enum RoleActionTypes {
  RolesRequested = '[USER Add Page] Roles Requested',
  RoleLoaded = '[USER Add Page] Roles Loaded',
}

export class RolesRequested implements Action {
  readonly type = RoleActionTypes.RolesRequested;
  constructor(public payload: { query: QueryParamsModel }) {}
}

export class RoleLoaded implements Action {
  readonly type = RoleActionTypes.RoleLoaded;
  constructor(public payload: { data: PageResponseModel<RoleModel[]> }) {}
}

export type RoleActions =
  | RolesRequested
  | RoleLoaded;
