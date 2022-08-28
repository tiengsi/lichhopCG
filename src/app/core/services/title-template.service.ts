import { BaseResponseModel, ITitleTemplate, PageResponseModel, PostModel, QueryParamsModel } from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class TitleTemplateService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  constructor(private http: HttpClient) { }

  getAll(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<PageResponseModel<ITitleTemplate[]>>> {
    const apiUrl = `${this.BASE_URL}title-templates`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      .set('index', queryParams.pageNumber?.toString())
      .set('pageSize', queryParams.pageSize?.toString());
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<PageResponseModel<ITitleTemplate[]>>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  getAllScheduleTitleTemplateByOrganizeId(
    queryParams: QueryParamsModel,
    organizeId: number
  ): Observable<BaseResponseModel<PageResponseModel<ITitleTemplate[]>>> {
    const apiUrl = `${this.BASE_URL}title-templates/getAllScheduleTitleTemplateByOrganizeId`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      .set('index', queryParams.pageNumber?.toString())
      .set('pageSize', queryParams.pageSize?.toString())
      .set('organizeId', organizeId?.toString());
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<PageResponseModel<ITitleTemplate[]>>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers }
    );
  }

  create(payload: ITitleTemplate): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}title-templates`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }

  delete(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}title-templates/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  getById(id: number): Observable<BaseResponseModel<ITitleTemplate>> {
    const apiUrl = `${this.BASE_URL}title-templates/${id}`;
    return this.http.get<BaseResponseModel<ITitleTemplate>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  update(payload: ITitleTemplate): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}title-templates`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }
}
