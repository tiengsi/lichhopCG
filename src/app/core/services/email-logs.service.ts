import { BaseResponseModel, QueryParamsModel } from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IEmailLogs, IEmailSmsStatus } from 'src/app/shared/models/email-logs.mode';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class EmailLogsService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  constructor(private http: HttpClient) { }

  getAll(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<IEmailSmsStatus>> {
    const apiUrl = `${this.BASE_URL}emailLogs`;
    let _params = new HttpParams();
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<IEmailSmsStatus>>(
      apiUrl,
      { params: _params,
      headers: this.headersConfig.httpOptions.headers} 
    );
  }
}
