import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HeadersConfig } from "src/app/configs/headers-config";
import { BaseResponseModel } from "src/app/shared";
import { BrandNameModel, ViettelBrandNameModel, VNPTBrandNameModel } from "src/app/shared/models/brand-name.model";
import { environment } from "src/environments/environment";

@Injectable()
export class BrandNameService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  _params = new HttpParams().set('organizeId', this.organizeId);

  constructor(private http: HttpClient) {}

  GetViettelBrandName():Observable<BaseResponseModel<ViettelBrandNameModel[]>>
  {
      const apiUrl = `${this.BASE_URL}brandname/GetListBrandNameByOrganizeId/viettel`;
      return this.http.get<BaseResponseModel<ViettelBrandNameModel[]>>(apiUrl, {params: this._params, headers: this.headersConfig.httpOptions.headers});
  }

  GetVNPTBrandName():Observable<BaseResponseModel<VNPTBrandNameModel[]>>
  {
      const apiUrl = `${this.BASE_URL}brandname/GetListBrandNameByOrganizeId/vnpt`;
      return this.http.get<BaseResponseModel<VNPTBrandNameModel[]>>(apiUrl, {params: this._params, headers: this.headersConfig.httpOptions.headers});
  }

  GetViettelBrandNameById(id: number):Observable<BaseResponseModel<ViettelBrandNameModel>>
  {
      const apiUrl = `${this.BASE_URL}brandname/GetBrandNameById/viettel`;
      var params = this._params.set('brandNameId', id.toString());
      return this.http.get<BaseResponseModel<ViettelBrandNameModel>>(apiUrl, {params, headers: this.headersConfig.httpOptions.headers});
  }

  GetVNPTBrandNameById(id: number):Observable<BaseResponseModel<VNPTBrandNameModel>>
  {
      const apiUrl = `${this.BASE_URL}brandname/GetBrandNameById/vnpt`;
      var params = this._params.set('brandNameId', id.toString());
      return this.http.get<BaseResponseModel<VNPTBrandNameModel>>(apiUrl, {params, headers: this.headersConfig.httpOptions.headers});
  }

  CreateViettelBrandName(payload: ViettelBrandNameModel): Observable<BaseResponseModel<number>>
  {
      const apiUrl = `${this.BASE_URL}brandname/CreateBrandName/viettel`;
      return this.http.post<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }

  CreateVNPTBrandName(payload: VNPTBrandNameModel): Observable<BaseResponseModel<number>>
  {
      const apiUrl = `${this.BASE_URL}brandname/CreateBrandName/vnpt`;
      return this.http.post<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }

  UpdateViettelBrandName(payload: ViettelBrandNameModel): Observable<BaseResponseModel<number>>
  {
      const apiUrl = `${this.BASE_URL}brandname/UpdateBrandName/viettel`;
      return this.http.put<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }

  UpdateVNPTBrandName(payload: VNPTBrandNameModel): Observable<BaseResponseModel<number>>
  {
      const apiUrl = `${this.BASE_URL}brandname/UpdateBrandName/vnpt`;
      return this.http.put<BaseResponseModel<number>>(apiUrl, payload, {headers: this.headersConfig.httpOptions.headers});
  }

  DeleteViettelBrandName(id: number): Observable<BaseResponseModel<string>>{
      const apiUrl = `${this.BASE_URL}brandname/DeleteBrandNameById/viettel/${id}`;
      return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  DeleteVNPTBrandName(id: number): Observable<BaseResponseModel<string>>{
      const apiUrl = `${this.BASE_URL}brandname/DeleteBrandNameById/vnpt/${id}`;
      return this.http.delete<BaseResponseModel<string>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }
}