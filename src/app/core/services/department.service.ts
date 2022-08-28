import { BaseResponseModel, QueryParamsModel } from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { GetOfficerRequest } from 'src/app/shared/models/user.model';
import { ITreeDepartmentOfficer } from 'src/app/shared/models/TreeDepartmentOfficer';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class DepartmentService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  constructor(private http: HttpClient) { }

  create(location: IDepartmentModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}department`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, location, {headers: this.headersConfig.httpOptions.headers});
  }

  delete(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}department/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  getById(id: number): Observable<BaseResponseModel<IDepartmentModel>> {
    const apiUrl = `${this.BASE_URL}department/${id}`;
    return this.http.get<BaseResponseModel<IDepartmentModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  update(location: IDepartmentModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}department`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, location, {headers: this.headersConfig.httpOptions.headers});
  }

  getAll(queryParams: QueryParamsModel): Observable<BaseResponseModel<IDepartmentModel[]>> {
    const apiUrl = `${this.BASE_URL}department`;
    let params = new HttpParams()
    .set('sortOrder', queryParams.sortOrder)
    .set('sortField', queryParams.sortField)
    .set('organizeId', this.organizeId);

    Object.keys(queryParams.filter).forEach((key) => {
      params = params.set(key, queryParams.filter[key]);
    });

    return this.http.get<BaseResponseModel<IDepartmentModel[]>>(
      apiUrl,
      { params,
      headers: this.headersConfig.httpOptions.headers} 
    );
  }

  getByOrganizeId():Observable<BaseResponseModel<IDepartmentModel[]>> {
    const apiUrl = `${this.BASE_URL}department/GetListDepartmentByOrganizeId`;
    let params = new HttpParams()    
    .set('organizeId', this.organizeId);    
    return this.http.get<BaseResponseModel<IDepartmentModel[]>>(
      apiUrl,
      { params: params,
      headers: this.headersConfig.httpOptions.headers} 
    );
  }

  getAllForSelect(): Observable<BaseResponseModel<IDepartmentModel[]>> {
    const apiUrl = `${this.BASE_URL}department/get-all-active`;
    let params = new HttpParams()    
    .set('organizeId', this.organizeId);
    return this.http.get<BaseResponseModel<IDepartmentModel[]>>(apiUrl, {params: params, headers: this.headersConfig.httpOptions.headers});
  }

  getDepartmentOfficer(model: GetOfficerRequest): Observable<BaseResponseModel<ITreeDepartmentOfficer[]>> {
    const apiUrl = `${this.BASE_URL}department/get-department-officer`;
    return this.http.post<BaseResponseModel<ITreeDepartmentOfficer[]>>(apiUrl, model, {headers: this.headersConfig.httpOptions.headers});
  }

  patchUpdate(
    id: number,
    department: any
  ): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}department/${id}`;
    return this.http.patch<BaseResponseModel<number>>(apiUrl, department, {headers: this.headersConfig.httpOptions.headers});
  }

  updateRepresentative(payload: any): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}department/representative`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }
}
