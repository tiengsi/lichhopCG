import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { HeadersConfig } from "src/app/configs/headers-config";
import { BaseResponseModel } from "src/app/shared";
import { environment } from "src/environments/environment";

@Injectable()
export class PermissionUIService {
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  userId = this.userInfo?.userId != undefined ? this.userInfo.userId : 0;
  constructor(private http: HttpClient) {}
    
  getPermissionList():Observable<BaseResponseModel<any>>{
    const apiUrl = `${this.BASE_URL}Permissions/PermissionOfUIByUserId/${this.userId}`;
    return this.http.get<BaseResponseModel<any>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  }

  transformPermission(permissionList: any){
    var permissionObj: any = {};
    permissionList.forEach((e: any) => {
      permissionObj[e.namePermission] = e.isAllow;
    });
    return permissionObj;
  }
}
