import { BaseResponseModel, PageResponseModel, PostModel, QueryParamsModel } from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class PostService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig
  constructor(private http: HttpClient) {}

  getAll(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<PageResponseModel<PostModel[]>>> {
    const apiUrl = `${this.BASE_URL}posts`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      // .set('filter', queryParams.filter)
      .set('sortOrder', queryParams.sortOrder)
      .set('sortField', queryParams.sortField)
      .set('index', queryParams.pageNumber.toString())
      .set('pageSize', queryParams.pageSize.toString());
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<PageResponseModel<PostModel[]>>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  createUser(post: PostModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}posts`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, post, {headers: this.headersConfig.httpOptions.headers});
  }

  deletePost(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}posts/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  getPostById(id: number): Observable<BaseResponseModel<PostModel>> {
    const apiUrl = `${this.BASE_URL}posts/${id}`;
    return this.http.get<BaseResponseModel<PostModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  updatePost(post: PostModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}posts`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, post, {headers: this.headersConfig.httpOptions.headers});
  }
}
