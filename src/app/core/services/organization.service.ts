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
import { Organization, OrganizationTree } from 'src/app/shared/models/organization.model';
import { HeadersConfig } from 'src/app/configs/headers-config';



@Injectable()
export class OrganizationService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  constructor(private http: HttpClient) { }  

  

  GetAllAsTree(): Observable<BaseResponseModel<OrganizationTree[]>>{
    const apiUrl = `${this.BASE_URL}organizes/OrganizesTree`;  
    //console.log(this.headerConfig.httpOptions.headers);      
    return this.http.get<BaseResponseModel<OrganizationTree[]>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  GetById(id: number): Observable<BaseResponseModel<Organization>>{
    const apiUrl = `${this.BASE_URL}organizes/GetOrganizeById/${id}`;
    return this.http.get<BaseResponseModel<Organization>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  CreateOrganiztion(model: Organization): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}organizes/CreateOrganize`;
    return this.http.post<BaseResponseModel<number>>(apiUrl, model, {headers: this.headersConfig.httpOptions.headers});
  }

  UpdateOrganization(model: Organization): Observable<BaseResponseModel<number>>{
    const apiUrl = `${this.BASE_URL}organizes/UpdateOrganize`;
    return this.http.put<BaseResponseModel<number>>(apiUrl, model, {headers: this.headersConfig.httpOptions.headers});
  }

  DeleteOrganization(id: number): Observable<BaseResponseModel<string>>{
    const apiUrl = `${this.BASE_URL}organizes/DeleteOrganizeById/${id}`;
    return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }
}

