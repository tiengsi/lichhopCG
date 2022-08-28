import { ChangePasswordModel } from './../../shared/models/change-password.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

// Environment
import { environment } from '../../../environments/environment';

// Models
import {
  UserModel,
  BaseResponseModel,
  PageResponseModel,
  QueryParamsModel,
  RoleModel,
  UserForSelectModel,
} from '../../shared';
import { HeadersConfig } from 'src/app/configs/headers-config';
import { ResetPasswordModel } from 'src/app/shared/models/reset-password.model';

@Injectable()
export class UserService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  //_params = new HttpParams().set('organizeId', this.organizeId);
  constructor(private http: HttpClient) {}

  getAll(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<PageResponseModel<UserModel[]>>> {
    const apiUrl = `${this.BASE_URL}users/GetListUserByOrganizeId`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      .set('filter', queryParams.filter)
      .set('sortOrder', queryParams.sortOrder)
      .set('sortField', queryParams.sortField)
      .set('index', queryParams.pageNumber.toString())
      .set('pageSize', queryParams.pageSize.toString())
      .set('organizeId', this.organizeId);
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<PageResponseModel<UserModel[]>>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }  


  getUserForSelect(isHost: number, departmentId?: number): Observable<BaseResponseModel<UserForSelectModel[]>> {
    const apiUrl = `${this.BASE_URL}users/select`;

    let _params = new HttpParams();
    if (departmentId !== null) {
      _params = _params.set('departmentId', departmentId.toString());
    } else {
      _params = _params.set('departmentId', '0');
    }

    _params = _params.set('isHost', isHost.toString());
    _params = _params.set('organizeId', this.organizeId);
    return this.http.get<BaseResponseModel<UserForSelectModel[]>>(
      apiUrl,
      { params : _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  getAllRole(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<PageResponseModel<RoleModel[]>>> {
    const apiUrl = `${this.BASE_URL}roles`;
    // tslint:disable-next-line:variable-name
    const _params = new HttpParams()
      .set('index', queryParams.pageNumber.toString())
      .set('pageSize', queryParams.pageSize.toString());
    return this.http.get<BaseResponseModel<PageResponseModel<RoleModel[]>>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  deleteUser(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}users/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  createUser(user: UserModel): Observable<BaseResponseModel<UserModel>> {
    //user.organizeId = this.organizeId;
    const apiUrl = `${this.BASE_URL}users`;
    return this.http.post<BaseResponseModel<UserModel>>(apiUrl, user, {headers: this.headersConfig.httpOptions.headers});
  }

  getUserById(id: number): Observable<BaseResponseModel<UserModel>> {
    const apiUrl = `${this.BASE_URL}users/${id}`;
    return this.http.get<BaseResponseModel<UserModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  updateUser(user: UserModel): Observable<BaseResponseModel<number>> {
    //user.organizeId = this.organizeId;
    const apiUrl = `${this.BASE_URL}users`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, user, {headers: this.headersConfig.httpOptions.headers});
  }

  changePassword(model: ChangePasswordModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}users/changepassword`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, model, {headers: this.headersConfig.httpOptions.headers});
  }

  resetPassword(model: ResetPasswordModel): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}users/resetpassword`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, model, {headers: this.headersConfig.httpOptions.headers});
  }
}
