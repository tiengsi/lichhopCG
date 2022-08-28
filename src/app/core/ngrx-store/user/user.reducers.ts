import { EntityState, EntityAdapter, createEntityAdapter, Update } from '@ngrx/entity';
import { UserModel } from './../../../shared/models/user.model';
import { createFeatureSelector } from '@ngrx/store';
import { UserActions, UserActionTypes, UserAdd } from './user.actions';

export interface UsersState extends EntityState<UserModel> {
  listLoading: boolean;
  totalCount: number;
  page: number;
  count: number; // number of data is loaded
  isDeleteSuccess: boolean;
  isDeleteFail: boolean;
  isAddSuccess: boolean;
  isAddFail: boolean;
  selectedUser: UserModel;
  isUpdateSuccess: boolean;
  errorMessage: string;
}

export const adapter: EntityAdapter<UserModel> = createEntityAdapter<UserModel>();

export const initialUsersState: UsersState = adapter.getInitialState({
  listLoading: false,
  totalCount: 0,
  page: 0,
  count: 0,
  isDeleteSuccess: false,
  isDeleteFail: false,
  isAddSuccess: false,
  isAddFail: false,
  selectedUser: undefined,
  isUpdateSuccess: false,
  errorMessage: undefined,
});

export function usersReducer(
  state = initialUsersState,
  action: UserActions
): UsersState {
  switch (action.type) {
    case UserActionTypes.UsersPageLoaded:
      return adapter.addMany(action.payload.data.items, {
        ...initialUsersState,
        totalCount: action.payload.data.totalCount,
        page: action.payload.data.page,
        count: action.payload.data.count,
        listLoading: false,
      });
    case UserActionTypes.UserDeleted:
      return adapter.removeOne(action.payload.id, state);
    case UserActionTypes.UserDeleteSuccess:
      return {
        ...state,
        listLoading: false,
        isDeleteSuccess: true,
      };
    case UserActionTypes.UserDeleteFail:
      return {
        ...state,
        listLoading: false,
        isDeleteFail: true,
      };
    case UserActionTypes.UserAdd:
      return adapter.addOne(action.payload.user, {
        ...state,
      });
    case UserActionTypes.UserAddSuccess:
      return {
        ...state,
        listLoading: false,
        isAddSuccess: true,
      };
    case UserActionTypes.UserAddFail:
      return {
        ...state,
        listLoading: false,
        isAddFail: true,
      };
    case UserActionTypes.GetUserByIdSuccess:
      return {
        ...state,
        listLoading: false,
        selectedUser: action.payload.user,
      };
    case UserActionTypes.GetUserByIdFail:
      return {
        ...state,
        listLoading: false,
        errorMessage: action.payload.errorMessage,
      };
    case UserActionTypes.UpdateUser:
      return adapter.updateOne(action.payload.partialUser, state);
    case UserActionTypes.UpdateUserSuccess:
      return {
        ...state,
        listLoading: false,
        isUpdateSuccess: true,
      };
    case UserActionTypes.UpdateUserFail:
      return {
        ...state,
        listLoading: false,
        errorMessage: action.payload.errorMessage,
      };
    default:
      return state;
  }
}

export const getUserState = createFeatureSelector<UsersState>('users');

export const {
  selectAll,
  selectEntities,
  selectIds,
  selectTotal,
} = adapter.getSelectors();
