import { Action } from '@ngrx/store';
import { AuthModel } from '../../../../shared/models/auth.model';

export enum AuthActionTypes {
    Login = '[Login] Action',
    Logout = '[Logout] Action',
    Register = '[Register] Action',
    UserRequested = '[Request User] Action'
}

export class Login implements Action {
    readonly type = AuthActionTypes.Login;
    constructor(public payload: AuthModel) { }
}

export class Logout implements Action {
    readonly type = AuthActionTypes.Logout;
}

export class Register implements Action {
    readonly type = AuthActionTypes.Register;
    constructor(public payload: { authToken: string }) { }
}

export class UserRequested implements Action {
    readonly type = AuthActionTypes.UserRequested;
}


export type AuthActions = Login | Logout | Register | UserRequested ;
