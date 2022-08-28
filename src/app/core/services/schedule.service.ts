import {
  BaseResponseModel,
  PageResponseModel,
  ScheduleModel,
  QueryParamsModel,
  ScheduleGroupByHostModel,
  ISms,
} from './../../shared/';
import { environment } from './../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  AuditScheduleModel,
  ScheduleByWeekModel,
  ScheduleFilesAttachment,
  ScheduleTemplateByWeekModel,
} from 'src/app/shared/models/schedule.model';
import { ScheduleTemplateModel } from 'src/app/shared/models/schedule-template.model';
import { PersonalNoteModel } from 'src/app/modules/test-website/models/personalNote.model';
import { ScheduledResultDocumentModel } from 'src/app/modules/test-website/models/resultDocument.model';
import { ScheduledResultReportModel } from 'src/app/modules/test-website/models/resultReport.model';
import { PersonalScheduleModel } from 'src/app/modules/test-website/models/personalSchedule.model';
import { HeadersConfig } from 'src/app/configs/headers-config';

@Injectable()
export class ScheduleService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  constructor(private http: HttpClient) { }

  getAll(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<PageResponseModel<ScheduleModel[]>>> {
    const apiUrl = `${this.BASE_URL}schedules/GetScheduleByOrganizeId`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      // .set('filter', queryParams.filter)
      .set('sortOrder', queryParams.sortOrder)
      .set('sortField', queryParams.sortField)
      .set('index', queryParams.pageNumber.toString())
      .set('pageSize', queryParams.pageSize.toString())
      .set('organizeId', this.organizeId);
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<PageResponseModel<ScheduleModel[]>>>(
      apiUrl,       
      { params: _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  getAllByWeek(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<ScheduleByWeekModel[]>> {
    //const apiUrl = `${this.BASE_URL}schedules/new-version-by-week`;
    const apiUrl = `${this.BASE_URL}schedules/GetScheduleByWeekAndOrganizeId/new-version-by-week`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      .set('sortOrder', queryParams.sortOrder)
      .set('sortField', queryParams.sortField)
      .set('index', queryParams.pageNumber.toString())
      .set('pageSize', queryParams.pageSize.toString())
      .set('organizeId', this.organizeId);
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<ScheduleByWeekModel[]>>(apiUrl, {
      params: _params, headers: this.headersConfig.httpOptions.headers
    });
  }

  getAllTemplateByWeek(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<ScheduleTemplateByWeekModel[]>> {
    const apiUrl = `${this.BASE_URL}schedules-template/GetAllScheduleTemplateByOrganizeIdAndWeek`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      .set('sortOrder', queryParams.sortOrder)
      .set('sortField', queryParams.sortField)
      .set('index', queryParams.pageNumber.toString())
      .set('pageSize', queryParams.pageSize.toString())
      .set('organizeId', this.organizeId);
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<ScheduleTemplateByWeekModel[]>>(
      apiUrl,
      {
        params: _params, headers: this.headersConfig.httpOptions.headers       
      }
    );
  }

  getAllByWeekForFE(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<ScheduleByWeekModel[]>> {
    const apiUrl = `${this.BASE_URL}schedules/GetScheduleByWeekAndOrganizeId/new-version-by-week`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      .set('sortOrder', queryParams.sortOrder)
      .set('sortField', queryParams.sortField)
      .set('index', queryParams.pageNumber.toString())
      .set('pageSize', queryParams.pageSize.toString())
      .set('organizeId', this.organizeId);
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });

    return this.http.get<BaseResponseModel<ScheduleByWeekModel[]>>(apiUrl, {
      params: _params, headers: this.headersConfig.httpOptions.headers
    });
  }

  getAllGroupByHost(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<ScheduleGroupByHostModel[]>> {
    const apiUrl = `${this.BASE_URL}schedules/group`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams();
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });
    return this.http.get<BaseResponseModel<ScheduleGroupByHostModel[]>>(
      apiUrl,
      { params: _params, headers: this.headersConfig.httpOptions.headers} 
    );
  }

  createSchedule(
    schedule: ScheduleModel
  ): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, schedule, {headers: this.headersConfig.httpOptions.headers});
  }

  deleteSchedule(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}schedules/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  getScheduleById(id: number): Observable<BaseResponseModel<ScheduleModel>> {
    const apiUrl = `${this.BASE_URL}schedules/${id}`;
    return this.http.get<BaseResponseModel<ScheduleModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  getMessageContent(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}schedules/${id}/message-content`;
    return this.http.get<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  updateSchedule(
    schedule: ScheduleModel
  ): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, schedule, {headers: this.headersConfig.httpOptions.headers});
  }

  updateStatus(id: number): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules/update-status/${id}`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, null, {headers: this.headersConfig.httpOptions.headers});
  }

  approve(scheduleId: number): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules/${scheduleId}/approve`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, null, {headers: this.headersConfig.httpOptions.headers});
  }

  pause(schedule: ScheduleModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules/pause`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, schedule, {headers: this.headersConfig.httpOptions.headers});
  }

  // dời lịch
  change(schedule: ScheduleModel): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules/change`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, schedule, {headers: this.headersConfig.httpOptions.headers});
  }

  sendSms(body: ISms): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules/sms`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, body, {headers: this.headersConfig.httpOptions.headers});
  }

  release(payload: any): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules/release`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }

  releaseById(id: number): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}schedules/release-by-id/${id}`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, {id}, {headers: this.headersConfig.httpOptions.headers})
  }

  updateMessageContent(payload: any): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules/message-content`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }

  copySchedule(id: number): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules/${id}/copy`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, null, {headers: this.headersConfig.httpOptions.headers});
  }

  getScheduleHistory(
    scheduleId: number
  ): Observable<BaseResponseModel<AuditScheduleModel[]>> {
    const apiUrl = `${this.BASE_URL}schedules/history/${scheduleId}`;
    return this.http.get<BaseResponseModel<AuditScheduleModel[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  getAllFilesAttachments(
    scheduleId: number
  ): Observable<BaseResponseModel<ScheduleFilesAttachment[]>> {
    const apiUrl = `${this.BASE_URL}schedules/getAllFilesAttachmentByScheduleId/${scheduleId}`;
    return this.http.get<BaseResponseModel<ScheduleFilesAttachment[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  getScheduleTemplateById(
    id: number
  ): Observable<BaseResponseModel<ScheduleTemplateModel>> {
    const apiUrl = `${this.BASE_URL}schedules-template/${id}`;
    return this.http.get<BaseResponseModel<ScheduleTemplateModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  createScheduleTemplate(
    schedule: ScheduleTemplateModel
  ): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules-template`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, schedule, {headers: this.headersConfig.httpOptions.headers});
  }

  updateScheduleTemplate(
    schedule: ScheduleTemplateModel
  ): Observable<BaseResponseModel<number>> {
    const apiUrl = `${this.BASE_URL}schedules-template`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, schedule, {headers: this.headersConfig.httpOptions.headers});
  }

  deleteScheduleTemplate(id: number): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}schedules-template/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }


  //personal notes
  getPersonalNotesByScheduleIdAndUserId(scheduleId: number, userId: number): Observable<BaseResponseModel<PersonalNoteModel[]>>{
    const apiUrl = `${this.BASE_URL}schedules/GetPersonalNotes/${scheduleId}/${userId}`;
    return this.http.get<BaseResponseModel<PersonalNoteModel[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  createPersonalNote(note: PersonalNoteModel) : Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}schedules/CreatePersonalNotes`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, note, {headers: this.headersConfig.httpOptions.headers});
  }

  updatePersonalNote(note: PersonalNoteModel) : Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}schedules/UpdatePersonalNotes`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, note, {headers: this.headersConfig.httpOptions.headers});
  }

  //documents
  getScheduledResultDocuments(scheduleId: number): Observable<BaseResponseModel<ScheduledResultDocumentModel[]>>{
    const apiUrl = `${this.BASE_URL}schedules/GetScheduledResultDocuments/${scheduleId}`;
    return this.http.get<BaseResponseModel<ScheduledResultDocumentModel[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  createResultDocuments(doc: ScheduledResultDocumentModel): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}schedules/CreateScheduledResultDocument`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, doc, {headers: this.headersConfig.httpOptions.headers});
  }

  //reports
  getScheduledResultReports(scheduleId: number): Observable<BaseResponseModel<ScheduledResultReportModel[]>>{
    const apiUrl = `${this.BASE_URL}schedules/GetScheduledResultReports/${scheduleId}`;
    return this.http.get<BaseResponseModel<ScheduledResultReportModel[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }
  createResultReports(report: ScheduledResultReportModel): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}schedules/CreateScheduledResultReport`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, report, {headers: this.headersConfig.httpOptions.headers});
  }

  //personal schedule
  getPersonalScheduleByUserId(userId: number): Observable<BaseResponseModel<PersonalScheduleModel[]>>{
    const apiUrl = `${this.BASE_URL}schedules/GetPersonalSchedulesByUserId/${userId}`;
    return this.http.get<BaseResponseModel<PersonalScheduleModel[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  createPersonalSchedule(sched: PersonalScheduleModel): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}schedules/CreatePersonalSchedule`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, sched, {headers: this.headersConfig.httpOptions.headers});
  }

  //Get schedules base on login user
  getScheduleOfLoginUser(
    queryParams: QueryParamsModel
  ): Observable<BaseResponseModel<ScheduleByWeekModel[]>> {
    const apiUrl = `${this.BASE_URL}schedules/GetSchedulesByUserInPeriodDate`;
    // tslint:disable-next-line:variable-name
    let _params = new HttpParams()
      .set('sortOrder', queryParams.sortOrder)
      .set('sortField', queryParams.sortField)
      .set('index', queryParams.pageNumber.toString())
      .set('pageSize', queryParams.pageSize.toString());
    Object.keys(queryParams.filter).forEach((key) => {
      _params = _params.set(key, queryParams.filter[key]);
    });

    return this.http.get<BaseResponseModel<ScheduleByWeekModel[]>>(apiUrl, {
      params: _params, headers: this.headersConfig.httpOptions.headers
    });
  }

  getLastestScheduleOfLoggedInUser(userId: number, userEmail: string, currentDate: string): Observable<BaseResponseModel<ScheduleModel>>
  {
    const apiUrl = `${this.BASE_URL}schedules/GetLastestScheduleByLoggedInUser/${userId}/${userEmail}/${currentDate}`;
    return this.http.get<BaseResponseModel<ScheduleModel>>(apiUrl, {headers: this.headersConfig.httpOptions.headers})
  }


  //Get QR Code
  getQRCodeByScheduleId(schedId: number): Observable<BaseResponseModel<string>>{
    const apiUrl = `${this.BASE_URL}schedules/GetQRCodeByScheduleId/${schedId}`;
    return this.http.get<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  //SharedFilesUpdate
  updateSharedFiles(fileList: ScheduleFilesAttachment[]): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}schedules/UpdateScheduleFileAttachmentList`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, fileList, {headers: this.headersConfig.httpOptions.headers});
  }

  getSharedFiles(scheduleId: number): Observable<BaseResponseModel<ScheduleFilesAttachment[]>>{
    const apiUrl = `${this.BASE_URL}schedules/getAllFilesAttachmentForShareByScheduleId/${scheduleId}/Share`;
    return this.http.get<BaseResponseModel<ScheduleFilesAttachment[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }
}
