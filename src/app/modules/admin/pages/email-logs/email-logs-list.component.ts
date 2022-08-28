import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PermissionList } from 'src/app/configs/permission';
import { ScheduleService, SubheaderService, ToastService } from 'src/app/core';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { QueryParamsModel, ScheduleModel } from 'src/app/shared';

@Component({
  selector: 'app-email-logs-list',
  templateUrl: './email-logs-list.component.html',
  styleUrls: ['./email-logs-list.component.scss'],
})
export class EmailLogsListComponent implements OnInit, OnDestroy {
  schedules: ScheduleModel[] = [];
  schedule: ScheduleModel = null;
  scheduleTotalCount = 0;
  scheduleCount = 0;
  schedulePage = 1;
  schedulePageSize = 10;

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  showDeleteBtn = true;

  // tabs
  tabSeleted = 2;
  tabs = [
    {
      id: 1,
      name: 'Chi tiết lịch',
    },
    {
      id: 2,
      name: 'Trạng thái gửi email/sms',
    },
  ];

  private subscriptions: Subscription[] = [];

  constructor(
    private subheaderService: SubheaderService,
    private toastService: ToastrService,
    private scheduleService: ScheduleService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) { }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_EMAIL_LOGS];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_EMAIL_LOGS];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_EMAIL_LOGS];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_EMAIL_LOGS];        
      }
      else {
        this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }
  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }


  ngOnInit(): void {
    this.initBreadCrumbs();
    this.loadSchedule();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý email logs', page: `admin/email-logs` },
    ]);
  }

  loadSchedule(): void {
    const filter = {
      host: 0,
      scheduleDate: '',
      active: -1,
      status: -1,
      locationId: -1
    };
    const queryParams = new QueryParamsModel(
      filter,
      "desc",
      'CreatedDate',
      this.schedulePage,
      this.schedulePageSize
    );

    const subScription = this.scheduleService.getAll(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        this.schedules = response.result.items;
        this.scheduleTotalCount = response.result.totalCount;
        this.scheduleCount = response.result.count;
      } else {
        this.toastService.error(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }

  getSchedule(scheduleId: number): void {
    const subScription = this.scheduleService.getScheduleById(scheduleId).subscribe((response) => {
      if (response.isSuccess) {
        this.schedule = response.result;
      } else {
        this.toastService.error(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }

  selectTab(id: number): void {
    this.tabSeleted = id;
  }

  getActive(scheduleId: number): boolean {
    if (this.schedule === null) {
      return false;
    }

    if (this.schedule.scheduleId === scheduleId) {
      return true;
    }

    return false;
  }
}
