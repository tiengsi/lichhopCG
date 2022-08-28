import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HeadersConfig } from 'src/app/configs/headers-config';
import { BaseResponseModel, GroupParticipantForCreateModel, GroupParticipantForListModel, PageResponseModel, QueryParamsModel } from 'src/app/shared';
import { IGroupParticipantForSelectScheduleModel } from 'src/app/shared/models/group-participant.model';
import { environment } from 'src/environments/environment';
@Injectable()
export class GroupMeetingService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  constructor(private http: HttpClient) { }

  create(group: GroupParticipantForCreateModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}group-participants`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, group, {headers: this.headersConfig.httpOptions.headers});
  }

  delete(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}group-participants/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl);
  }

  getById(id: number): Observable<BaseResponseModel<GroupParticipantForCreateModel>> {
    const apiUrl = `${this.BASE_URL}group-participants/${id}`;
    return this.http.get<BaseResponseModel<GroupParticipantForCreateModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  update(group: GroupParticipantForCreateModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}group-participants`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, group, {headers: this.headersConfig.httpOptions.headers});
  }

  getAll(queryParams: QueryParamsModel): Observable<BaseResponseModel<PageResponseModel<GroupParticipantForListModel[]>>> {
    const apiUrl = `${this.BASE_URL}group-participants`;
    let params = new HttpParams()
    .set('sortOrder', queryParams.sortOrder)
    .set('sortField', queryParams.sortField)
    .set('index', queryParams.pageNumber.toString())
    .set('organizeId', this.organizeId);
    Object.keys(queryParams.filter).forEach((key) => {
      params = params.set(key, queryParams.filter[key]);
    });

    return this.http.get<BaseResponseModel<PageResponseModel<GroupParticipantForListModel[]>>>(
      apiUrl,
      { params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  getAllForSelect(): Observable<BaseResponseModel<IGroupParticipantForSelectScheduleModel[]>> {
    const apiUrl = `${this.BASE_URL}group-participants/get-all-for-select`;
    let params = new HttpParams()    
    .set('organizeId', this.organizeId);
    return this.http.get<BaseResponseModel<IGroupParticipantForSelectScheduleModel[]>>(apiUrl, {params, headers: this.headersConfig.httpOptions.headers});
  }
}
