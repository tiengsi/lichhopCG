import { createFeatureSelector, createSelector } from '@ngrx/store';
import { RolesState, selectAll } from './role.reducers';

export const userFeatureSelector = createFeatureSelector<RolesState>('roles');

export const getAllRoles = createSelector(userFeatureSelector, selectAll);
