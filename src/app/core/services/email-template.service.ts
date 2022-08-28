import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HeadersConfig } from "src/app/configs/headers-config";
import { BaseResponseModel } from "src/app/shared";
import { EmailTemplateModel } from "src/app/shared/models/email-template.model";
import { environment } from "src/environments/environment";

@Injectable()
export class EmailTemplateService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  _params = new HttpParams().set('organizeId', this.organizeId);

  constructor(private http: HttpClient) {}

  getByOrganizeId(): Observable<BaseResponseModel<EmailTemplateModel[]>> {
    const apiUrl = `${this.BASE_URL}emailtemplate/GetListEmailTemplateByOrganizeId`;
    return this.http.get<BaseResponseModel<EmailTemplateModel[]>>(
      apiUrl, { params: this._params, headers: this.headersConfig.httpOptions.headers}
    );
  }

  getById(id: number): Observable<BaseResponseModel<EmailTemplateModel>> {
    const apiUrl = `${this.BASE_URL}emailtemplate/GetEmailTemplateById`;
    var params = new HttpParams().set('emailTemplateId', id.toString());
    return this.http.get<BaseResponseModel<EmailTemplateModel>>(
      apiUrl, { params: params, headers: this.headersConfig.httpOptions.headers}
    );
  }

  create(payload: EmailTemplateModel): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}emailtemplate/CreateEmailTemplate`;
    return this.http.post<BaseResponseModel<number>>(
      apiUrl, payload, {headers: this.headersConfig.httpOptions.headers}
    );
  }

  update(payload: EmailTemplateModel): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}emailtemplate/UpdateEmailTemplate`;
    return this.http.put<BaseResponseModel<number>>(
      apiUrl, payload, {headers: this.headersConfig.httpOptions.headers}
    );
  }

  delete(id: number): Observable<BaseResponseModel<string>>{
      const apiUrl = `${this.BASE_URL}emailtemplate/DeleteEmailTemplate/${id}`;
      return this.http.delete<BaseResponseModel<string>>(apiUrl);
  }
}