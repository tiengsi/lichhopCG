// Actions
import { AuthActions, AuthActionTypes } from '../_actions/auth.actions';
// Models
import { AuthModel } from '../../../../shared/models/auth.model';
import { partial } from 'lodash';

export interface AuthState {
  loggedIn: boolean;
  user: AuthModel;
  isUserLoaded: boolean;
}

export const initialAuthState: Partial<AuthState> = {
  loggedIn: false,
  isUserLoaded: false,
  user: undefined
};

export function authReducer(state = initialAuthState, action: AuthActions): Partial<AuthState> {
  switch (action.type) {
    case AuthActionTypes.Login: {
      const auth: AuthModel = action.payload;
      return {
        loggedIn: true,
        user: auth,
        isUserLoaded: false
      };
    }

    case AuthActionTypes.Register: {
      const token: string = action.payload.authToken;
      return {
        loggedIn: true,
        user: undefined,
        isUserLoaded: false
      };
    }

    case AuthActionTypes.Logout:
      return initialAuthState;

    default:
      return state;
  }
}
