import { BaseResponseModel, PageResponseModel, PostModel, QueryParamsModel } from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class LocationService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  constructor(private http: HttpClient) { }

  getAll(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<PageResponseModel<ILocationModel[]>>> {
    const apiUrl = `${this.BASE_URL}location`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      .set('filter', queryParams.filter?.toString())
      .set('index', queryParams.pageNumber?.toString())
      .set('pageSize', queryParams.pageSize?.toString())
      .set('sortOrder', queryParams.sortOrder?.toString())
      .set('organizeId', this.organizeId)
      .set('sortField', queryParams.sortField?.toString());
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<PageResponseModel<ILocationModel[]>>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  getAllActive(): Observable<BaseResponseModel<ILocationModel[]>> {
    const apiUrl = `${this.BASE_URL}location/get-all-select-active`;
    let params = new HttpParams().set('organizeId', this.organizeId);
    return this.http.get<BaseResponseModel<ILocationModel[]>>(apiUrl, { params, headers: this.headersConfig.httpOptions.headers});
  }

  create(location: ILocationModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}location`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, location, {headers: this.headersConfig.httpOptions.headers});
  }

  delete(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}location/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  getById(id: number): Observable<BaseResponseModel<ILocationModel>> {
    const apiUrl = `${this.BASE_URL}location/${id}`;
    return this.http.get<BaseResponseModel<ILocationModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  update(location: ILocationModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}location`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, location, {headers: this.headersConfig.httpOptions.headers});
  }
}
