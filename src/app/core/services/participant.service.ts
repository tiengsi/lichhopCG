import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HeadersConfig } from "src/app/configs/headers-config";
import { BaseResponseModel, IParticipant, IParticipantIsSelected, IReceiver } from "src/app/shared";
import { environment } from "src/environments/environment";

@Injectable()
export class ParticipantService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  constructor(private http: HttpClient) { }

  getAllForSelect(): Observable<BaseResponseModel<IParticipant[]>> {
    const apiUrl = `${this.BASE_URL}participants/select`;
    let params = new HttpParams()    
    .set('organizeId', this.organizeId);
    return this.http.get<BaseResponseModel<IParticipant[]>>(apiUrl, {params, headers: this.headersConfig.httpOptions.headers});
  }

  chooseParticipant(participantId: string): Observable<BaseResponseModel<IParticipantIsSelected[]>> {
    const apiUrl = `${this.BASE_URL}participants/${participantId}/choose`;

    return this.http.get<BaseResponseModel<IParticipantIsSelected[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  chooseReceiver(participantId: string): Observable<BaseResponseModel<IReceiver[]>> {
    const apiUrl = `${this.BASE_URL}participants/${participantId}/receiver`;

    return this.http.get<BaseResponseModel<IReceiver[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  sendSMS(phoneNumbers: string[]): Observable<BaseResponseModel<string>> {
    const apiUrl = `${this.BASE_URL}participants/sms`;
    return this.http.post<BaseResponseModel<string>>(apiUrl, phoneNumbers, {headers: this.headersConfig.httpOptions.headers});
  }
}
