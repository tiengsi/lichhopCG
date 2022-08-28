import { BaseResponseModel, PageResponseModel, CategoryModel, ECategoryType } from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class CategoryService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig
  constructor(private http: HttpClient) {}

  getAllForSelect(
    type: ECategoryType
  ): Observable<BaseResponseModel<CategoryModel[]>> {
    const apiUrl = `${this.BASE_URL}categories/${type}/select`;

    return this.http.get<BaseResponseModel<CategoryModel[]>>(
      apiUrl, {headers: this.headersConfig.httpOptions.headers}
    );
  }

  getAllByMenu(
    menu: string
  ): Observable<BaseResponseModel<CategoryModel[]>> {
    const apiUrl = `${this.BASE_URL}categories/${menu}/menu`;

    return this.http.get<BaseResponseModel<CategoryModel[]>>(
      apiUrl, {headers: this.headersConfig.httpOptions.headers}
    );
  }
}
