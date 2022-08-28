import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PermissionUIService } from "src/app/core/services/permission.service";
import { AuthModel, BaseResponseModel } from "src/app/shared";
import { environment } from "src/environments/environment";
import { HeadersConfig } from "../headers-config";

export class MenuConfig {
  public defaults: any = {
    header: {
      self: {},
      items: [
        {
          title: 'Dashboards',
          root: true,
          alignment: 'left',
          page: '/admin',
          translate: 'MENU.DASHBOARD',
        },
      ],
    },
    aside: {
      self: {},
      items: [
        {
          title: 'Trang Chủ',
          root: true,
          icon: 'flaticon2-architecture-and-city',
          page: '/scheduler',
          translate: 'MENU.DASHBOARD',
          bullet: 'dot',
        },
        {
          section: 'Đối Tượng',
        },
        {
          title: 'Quản Lý Đơn Vị',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-sitemap',
          page: '/admin/organization',          
        },
        {
          title: 'Quản Lý phòng ban',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-building',
          page: '/admin/department',
        },
        {
          title: 'Quản Lý Người Dùng',
          root: true,
          bullet: 'dot',
          icon: 'flaticon2-user-outline-symbol',
          page: '/admin/user',
        },
        {
          title: 'Quản lý nhóm tham dự họp',
          root: true,
          bullet: 'dot',
          icon: 'flaticon2-browser-2',
          page: '/admin/group-meeting',
        },
        { section: 'Quản Lý Chung' },
        {
          title: 'Quản Lý Lịch Họp',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-calendar-check',
          page: '/admin/schedule',
        },
        {
          title: 'Quản Lý Lịch Họp Mẫu',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-calendar-check',
          page: '/admin/schedule-template',
        },
        // {
        //   title: 'Cấu Hình Website',
        //   bullet: 'dot',
        //   icon: 'fas fa-cogs',
        //   root: true,
        //   submenu: [
        //     {
        //       title: 'Cấu Hình Chung',
        //       page: '/admin/setting',
        //     },
        //     {
        //       title: 'Banner và Favicon',
        //       page: '/admin/setting/setting-image',
        //     },
        //   ],
        // },
        {
          title: 'Quản Lý mẫu tiêu đề lịch',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-synagogue',
          page: '/admin/title-template',
        },
        {
          title: 'Quản Lý địa điểm',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-map-marker-alt',
          page: '/admin/location',
        },
        // {
        //   title: 'Quản Lý Tin Tức',
        //   root: true,
        //   bullet: 'dot',
        //   icon: 'fas fa-newspaper',
        //   page: '/admin/post',
        // },
        {
          title: 'Gửi tin nhắn SMS',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-sms',
          page: '/admin/schedule/send-sms',
        },
        {
          title: 'Quản Lý email logs',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-envelope',
          page: '/admin/email-logs',
        },
        {
          title: 'Thống kê lịch họp',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-chart-bar',
          submenu: [
            {
              title: 'Theo ngày',
              page: '/admin/statistical',
            },
            {
              title: 'Theo tháng',
              page: '/admin/statistical/month',
            },
            {
              title: 'Theo Năm',
              page: '/admin/statistical/year',
            },
          ],
        },
        {
          title: 'Quản lý BrandName',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-sms',
          // page: '/admin/brandname'
          submenu: [
            {
              title: 'Viettel',
              page: '/admin/brandname/viettel',
            },
            {
              title: 'VNPT',
              page: '/admin/brandname/vnpt',
            }            
          ],
        },
        {
          title: 'Quản lý mẫu Email',
          root: true,
          bullet: 'dot',
          icon: 'fas fa-envelope',
          page: '/admin/email-template'
        },
      ],
    },
  };

  // constructor(private http: HttpClient) {}
  private BASE_URL = environment.base_url;
  headersConfig = new HeadersConfig;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  userId = this.userInfo?.userId != undefined ? this.userInfo.userId : 0;

  // getPermissionList():Observable<BaseResponseModel<any>>{
  //   const apiUrl = `${this.BASE_URL}Permissions/PermissionOfUIByUserId/${this.userId}`;
  //   return this.http.get<BaseResponseModel<any>>(apiUrl, {headers: this.headersConfig.httpOptions.headers});
  // }
  transformPermission(permissionList: any){
    var permissionObj: any = {};
    permissionList.forEach((e: any) => {
      permissionObj[e.namePermission] = e.isAllow;
    });
    return permissionObj;
  }

  // getApi = async() => {
  //   const localToken = localStorage.getItem(environment.authTokenKey);
  //   const tokenObj: AuthModel = JSON.parse(localToken);
  //   const authToken = 'Bearer ' + tokenObj.accessToken;
  //   const apiUrl = `${this.BASE_URL}Permissions/PermissionOfUIByUserId/${this.userId}`;
  //   const response = await fetch(apiUrl, {
  //     headers:{
  //       'Authorization': authToken
  //     }
  //   });
  //   const data = await response.json();
  //   if(data.isSuccess){
  //     const permissionList = this.transformPermission(data.result);
  //     //console.log(permissionList);
  //     return permissionList;
  //   }
  // }

  public get configs() {

    // var data: any = {};
    // var called = false;
    // var t = this.getApi().then(res => {
    //   data = res;
    //   called = true;
    // });       
    //console.log(data);

    var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));   
    var isSuperAdmin = userInfo.roles.includes('SuperAdmin');
    var isScheduler = userInfo.roles.includes('Scheduler')
    var isAdmin = userInfo.roles.includes('Admin')    
    if(isSuperAdmin){
      return this.defaults;
    }    
    else if(isAdmin){
      // this.defaults.aside.items[9].submenu.splice(0,1);
      this.defaults.aside.items.splice(2,1);
      return this.defaults;
    }
    else if(isScheduler){
      return {
        header: {
          self: {},
          items: [
            {
              title: 'Dashboards',
              root: true,
              alignment: 'left',
              page: '/admin',
              translate: 'MENU.DASHBOARD',
            },
          ],
        },
        aside: {
          self: {},
          items: [
            {
              title: 'Trang Chủ',
              root: true,
              icon: 'flaticon2-architecture-and-city',
              page: '/scheduler',
              translate: 'MENU.DASHBOARD',
              bullet: 'dot',
            },
            {
              section: 'Đối Tượng',
            },        
            {
              title: 'Quản lý nhóm tham dự họp',
              root: true,
              bullet: 'dot',
              icon: 'flaticon2-browser-2',
              page: '/admin/group-meeting',
            },
            { section: 'Quản Lý Chung' },
            {
              title: 'Quản Lý Lịch Họp',
              root: true,
              bullet: 'dot',
              icon: 'fas fa-calendar-check',
              page: '/admin/schedule',
            },
            {
              title: 'Quản Lý Lịch Họp Mẫu',
              root: true,
              bullet: 'dot',
              icon: 'fas fa-calendar-check',
              page: '/admin/schedule-template',
            },   
            {
              title: 'Quản Lý mẫu tiêu đề lịch',
              root: true,
              bullet: 'dot',
              icon: 'fas fa-synagogue',
              page: '/admin/title-template',
            },
            {
              title: 'Quản Lý địa điểm',
              root: true,
              bullet: 'dot',
              icon: 'fas fa-map-marker-alt',
              page: '/admin/location',
            },     
            {
              title: 'Thống kê lịch họp',
              root: true,
              bullet: 'dot',
              icon: 'fas fa-chart-bar',
              submenu: [
                {
                  title: 'Theo ngày',
                  page: '/admin/statistical',
                },
                {
                  title: 'Theo tháng',
                  page: '/admin/statistical/month',
                },
                {
                  title: 'Theo Năm',
                  page: '/admin/statistical/year',
                },
              ],
            },        
          ],
        },
      };
    }
    else{
      return {
        header: {
          self: {},
          items: [
            {
              title: 'Dashboards',
              root: true,
              alignment: 'left',
              page: '/admin',
              translate: 'MENU.DASHBOARD',
            },
          ],
        },
        aside: {
          self: {},
          items: [
            {
              title: 'Trang Chủ',
              root: true,
              icon: 'flaticon2-architecture-and-city',
              page: '/scheduler',
              translate: 'MENU.DASHBOARD',
              bullet: 'dot',
            },        
          ],
        },
      };
    }
  }
}
