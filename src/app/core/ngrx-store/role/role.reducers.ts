import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { RoleModel } from './../../../shared/';
import { createFeatureSelector } from '@ngrx/store';
import { RoleActions, RoleActionTypes } from './role.actions';

export interface RolesState extends EntityState<RoleModel> {
  isLoading: boolean;
  totalCount: number;
  page: number;
  count: number; // number of data is loaded
}

export const adapter: EntityAdapter<RoleModel> = createEntityAdapter<RoleModel>();

export const initialRolesState: RolesState = adapter.getInitialState({
  isLoading: false,
  totalCount: 0,
  page: 0,
  count: 0,
});

export function RolesReducer(
  state = initialRolesState,
  action: RoleActions
): RolesState {
  switch (action.type) {
    case RoleActionTypes.RoleLoaded:
      return adapter.addMany(action.payload.data.items, {
        ...initialRolesState,
        totalCount: action.payload.data.totalCount,
        page: action.payload.data.page,
        count: action.payload.data.count,
        isLoading: false,
      });
    default:
      return state;
  }
}

export const getRoleState = createFeatureSelector<RolesState>('roles');

export const {
  selectAll,
  selectEntities,
  selectIds,
  selectTotal,
} = adapter.getSelectors();
