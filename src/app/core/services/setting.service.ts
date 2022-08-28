import { BaseResponseModel, PageResponseModel, SettingModel, QueryParamsModel } from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class SettingService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig
  constructor(private http: HttpClient) {}

  getAll(): Observable<BaseResponseModel<SettingModel[]>> {
    const apiUrl = `${this.BASE_URL}settings`;
    return this.http.get<BaseResponseModel<SettingModel[]>>(
      apiUrl, {headers: this.headersConfig.httpOptions.headers}
    );
  }

  update(settings: SettingModel[]): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}settings`;
    return this.http.put<BaseResponseModel<string>>(apiUrl, settings, {headers: this.headersConfig.httpOptions.headers});
  }

  getByKey(key: string): Observable<BaseResponseModel<SettingModel>> {
    const apiUrl = `${this.BASE_URL}settings/${key}`;
    return this.http.get<BaseResponseModel<SettingModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }
}
