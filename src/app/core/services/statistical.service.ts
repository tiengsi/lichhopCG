import { BaseResponseModel, PageResponseModel, QueryParamsModel, ScheduleGroupByHostModel, IStatisticalDay } from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class StatisticalService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  constructor(private http: HttpClient) { }

  byDay(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<IStatisticalDay>> {
    const apiUrl = `${this.BASE_URL}schedules/statistical-day`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams();
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<IStatisticalDay>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers }
    );
  }

  byMonth(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<IStatisticalDay>> {
    const apiUrl = `${this.BASE_URL}schedules/statistical-month`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams();
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<IStatisticalDay>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  byYear(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<IStatisticalDay>> {
    const apiUrl = `${this.BASE_URL}schedules/statistical-year`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams();
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<IStatisticalDay>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers }
    );
  }

}
