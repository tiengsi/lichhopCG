import { UserModel, PageResponseModel } from './../../../shared/';
import { createSelector, createFeatureSelector } from '@ngrx/store';
import { selectAll } from './user.reducers';
import { UsersState } from './user.reducers';
import { each } from 'lodash';

export const userFeatureSelector = createFeatureSelector<UsersState>('users');

export const getAllUsers = createSelector(userFeatureSelector, selectAll);



export const selectUserById = createSelector(
  userFeatureSelector,
  usersState => usersState.selectedUser
);

export const getTotalUsers = createSelector(userFeatureSelector, (state: UsersState) => state.totalCount);

export const getCountUsers = createSelector(userFeatureSelector, (state: UsersState) => state.count);

export const getDeleteUserSuccess = createSelector(userFeatureSelector, (state: UsersState) => state.isDeleteSuccess);

export const getDeleteUserFail = createSelector(userFeatureSelector, (state: UsersState) => state.isDeleteFail);

export const getAddUserSuccess = createSelector(userFeatureSelector, (state: UsersState) => state.isAddSuccess);

export const getAddUserFail = createSelector(userFeatureSelector, (state: UsersState) => state.isAddFail);

export const getUpdateUserSuccess = createSelector(userFeatureSelector, (state: UsersState) => state.isUpdateSuccess);

export const getUpdateUserFail = createSelector(userFeatureSelector, (state: UsersState) => state.errorMessage);
