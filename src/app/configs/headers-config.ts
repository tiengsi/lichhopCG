import { HttpHeaders } from "@angular/common/http";

export class HeadersConfig{
    private userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
    userId = this.userInfo?.userId != undefined ? this.userInfo.userId : -1;
    httpOptions: any = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json',
            'UserId': this.userId.toString(),
            'OrganizeId': '1'
        })
    }
}