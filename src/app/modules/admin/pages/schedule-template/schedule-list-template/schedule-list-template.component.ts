import { DatePipe, Location } from '@angular/common';
import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbDateStruct, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import {
  SubheaderService,
  ScheduleService,
  UserService,
  LayoutUtilsService,
} from 'src/app/core';
import { LocationService } from 'src/app/core/services/location.service';
import {
  UserForSelectModel,
  EScheduleStatus,
  DateOfWeekModel,
  QueryParamsModel,
} from 'src/app/shared';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { ROLES } from 'src/app/shared/models/permission';
import {
  ScheduleByWeekModel,
  ScheduleModel,
  ScheduleTemplateByWeekModel,
} from 'src/app/shared/models/schedule.model';
import { SelectOption } from 'src/app/shared/models/SelectOption.model';
import * as $ from 'jquery';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';

@Component({
  selector: 'app-schedule-list-template',
  templateUrl: './schedule-list-template.component.html',
  styleUrls: ['./schedule-list-template.component.scss'],
})
export class ScheduleListTemplateComponent implements OnInit, OnDestroy {
  roles = ROLES;
  schedules: ScheduleTemplateByWeekModel[] = [];
  totalCount = 0;
  count = 0;
  page = 1;
  pageSize = 10;
  schedulePosition = '';
  scheduleDate: NgbDateStruct;
  hostSelected = 0;
  activeSelected = -1;
  hosts: UserForSelectModel[] = [
    {
      id: 0,
      fullName: '--- Chọn Người Chủ Trì ---',
    },
  ];
  lstActive: SelectOption[] = [
    {
      value: -1,
      label: '--- Hiển thị/ Ẩn ---',
    },
    {
      value: 1,
      label: 'Hiển thị',
    },
    {
      value: 0,
      label: 'Bị ẩn',
    },
  ];
  statusSelected = -1;
  lstStatus: SelectOption[] = [
    {
      value: -1,
      label: '--- Chọn trạng thái ---',
    },
    {
      value: EScheduleStatus.Pending,
      label: 'Chưa được duyệt',
    },
    {
      value: EScheduleStatus.Approve,
      label: 'Đã duyệt',
    },
    {
      value: EScheduleStatus.Pause,
      label: 'Bị hoãn lịch',
    },
    {
      value: EScheduleStatus.Changed,
      label: 'Bị dời lịch',
    },
  ];
  locationSelected = -1;
  locations: ILocationModel[] = [
    {
      id: -1,
      title: '--- Chọn địa điểm ---',
    },
  ];
  private subscriptions: Subscription[] = [];
  @ViewChild('searchInput', { static: true }) searchInput: ElementRef;
  sortEvent: SortEvent = {
    direction: 'desc',
    column: 'ScheduleDate',
  };
  weeks = [];
  weekSelected = 1;
  startDateOfWeek: Date;
  endDateOfWeek: Date;
  allDayOfWeek: DateOfWeekModel[] = [];
  txtAllDayOfWeek = 'Tất cả ngày trong tuần';
  dayOfWeekSelected: any = this.txtAllDayOfWeek;
  selectAllWeek = true;
  selectedDateFromAddPage;
  weekSelectedFromAddPage = 1;
  daySelectedToShow;

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  showDeleteBtn = true;

  constructor(
    private subheaderService: SubheaderService,
    private scheduleService: ScheduleService,
    private userService: UserService,
    private toastr: ToastrService,
    private layoutUtilsService: LayoutUtilsService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private locationService: LocationService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) {
    this.selectedDateFromAddPage =
      this.activatedRoute.snapshot.queryParams?.startDateOfWeek;
    const weekParam =
      this.activatedRoute.snapshot.queryParams?.week || 0;
    this.weekSelectedFromAddPage = parseInt(weekParam, 10);

  }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_TEMPLATE];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_TEMPLATE_ADD];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_TEMPLATE_EDIT];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_TEMPLATE_DELETE];        
      }
      else {
        //this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadPermissions();
    this.initBreadCrumbs();
    this.loadUser();
    this.loadLocation();
    this.loadWeeks();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý lịch họp mẫu', page: `/admin/schedule-template` },
    ]);
  }

  get scheduleStatus(): typeof EScheduleStatus {
    return EScheduleStatus;
  }

  hasSchedule(schedule: any): boolean {
    return (
      schedule.morning.length > 0 ||
      schedule.afternoon.length > 0 ||
      schedule.evening.length > 0
    );
  }

  loadWeeks(): void {
    for (let index = 1; index < 54; index++) {
      this.weeks.push({
        id: index,
        name: 'Tuần ' + index,
      });
    }
    const now = new Date();
    this.weekSelected = this.getWeekNumber(now);

    const oneJan = new Date(now.getFullYear(), 0, 1);
    const w =
      oneJan.getTime() -
      3600000 * 24 * (oneJan.getDay() - 1) +
      604800000 * (this.weekSelected - 1);
    this.startDateOfWeek = new Date(w);
    this.endDateOfWeek = new Date(w + 518400000);
    this.getAllDateOfWeek();

    if (this.selectedDateFromAddPage) {
      this.startDateOfWeek = new Date(this.selectedDateFromAddPage);
      const datePipe = new DatePipe('en-US');
      this.daySelectedToShow =
        this.getDayNameOfWeek(this.startDateOfWeek) +
        ' ngày ' +
        datePipe.transform(this.startDateOfWeek, 'dd/MM/yyyy');
      this.loadSchedule(false, false);
    } else {
      this.loadSchedule(false, true);
    }
  }

  // Get week number of the year
  getWeekNumber(date: Date): number {
    const oneJan = new Date(date.getFullYear(), 0, 1);

    // adding 1 since this.getDay()
    // returns value starting from 0
    return Math.ceil(
      ((date.getTime() - oneJan.getTime()) / 86400000 + oneJan.getDay()) / 7
    );
  }

  deleteQueryParamFromUrl(): void {
    // delete startDateOfWeek from url
    const snapshot = this.activatedRoute.snapshot;
    const params = { ...snapshot.queryParams };
    delete params.startDateOfWeek;
    this.router.navigate([], { queryParams: params });
  }

  loadUser(): void {
    const loadSub = this.userService
      .getUserForSelect(1, null)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.hosts = this.hosts.concat(response.result);
        } else {
          this.toastr.error('Lấy danh sách cán bộ thất bại, vui lòng thử lại!');
        }
      });

    this.subscriptions.push(loadSub);
  }

  loadLocation(): void {
    const locationSub = this.locationService
      .getAllActive()
      .subscribe((response) => {
        if (response.isSuccess) {
          this.locations = this.locations.concat(response.result);
        } else {
          this.toastr.error(
            'Lấy danh sách địa điểm thất bại, vui lòng thử lại!'
          );
        }
      });

    this.subscriptions.push(locationSub);
  }

  loadSchedule(isWeekSelectEvent = false, selectAllWeek: boolean = true): void {
    this.selectAllWeek = selectAllWeek;

    const filter = {
      host: this.hostSelected,
      active: this.activeSelected,
      status: this.statusSelected,
      locationId: this.locationSelected,
      selectAllWeek,
    };
    const queryParams = new QueryParamsModel(
      filter,
      this.sortEvent.direction,
      this.sortEvent.column,
      this.page,
      this.pageSize
    );

    const subScription = this.scheduleService
      .getAllTemplateByWeek(queryParams)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.schedules = response.result;
        } else {
          this.toastr.error(response.message);
        }
      });

    this.subscriptions.push(subScription);
  }

  editSchedule(id): void {
    this.router.navigate(['./edit', id], {
      relativeTo: this.activatedRoute,
      queryParamsHandling: 'merge',
    });
  }

  redirectAddPage(): void {
    this.router.navigate(['./add'], {
      relativeTo: this.activatedRoute,
      queryParamsHandling: 'merge',
    });
  }

  deleteSchedule(item: ScheduleModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.scheduleService
          .deleteScheduleTemplate(item.scheduleId)
          .subscribe((response) => {
            if (response.isSuccess) {
              this.toastr.success('Xóa thành công!');
              this.loadSchedule();
            } else {
              this.toastr.error('Xóa thất bại, xin vui lòng thử lại!');
            }
          });
      }
    });
  }

  onSort(event: SortEvent): void {
    if (!event || !event.column || !event.direction) {
      return;
    }

    this.sortEvent = event;
    this.loadSchedule();
  }

  expandAll(id: any): void {
    const hasClass = $('#wrapper-content-' + id).hasClass('span-all');
    if (hasClass) {
      $('#wrapper-content-' + id).removeClass('span-all');
      $('#btn-expan-all-' + id).text('Xem tất cả');
    } else {
      $('#wrapper-content-' + id).addClass('span-all');
      $('#btn-expan-all-' + id).text('Đóng lại');
    }
  }

  getAllDateOfWeek(): void {
    this.allDayOfWeek = [];

    this.allDayOfWeek.push({
      fullDay: this.startDateOfWeek,
      day: this.getDayNameOfWeek(this.startDateOfWeek),
      isActive: false,
    });

    for (let index = 1; index < 7; index++) {
      const nextDay = new Date(
        new Date(this.startDateOfWeek).setDate(
          this.startDateOfWeek.getDate() + index
        )
      );
      this.allDayOfWeek.push({
        fullDay: nextDay,
        day: this.getDayNameOfWeek(nextDay),
        isActive: false,
      });
    }
  }

  getDayNameOfWeek(day: Date): string {
    let dayName = 'Chủ Nhật';
    switch (day.getDay()) {
      case 1:
        dayName = 'Thứ 2';
        break;
      case 2:
        dayName = 'Thứ 3';
        break;
      case 3:
        dayName = 'Thứ 4';
        break;
      case 4:
        dayName = 'Thứ 5';
        break;
      case 5:
        dayName = 'Thứ 6';
        break;
      case 6:
        dayName = 'Thứ 7';
        break;
      default:
        break;
    }

    return dayName;
  }

  redirectAddSchedule(templateid: any): void {
    const paramDate = this.startDateOfWeek
      ? `${
          this.startDateOfWeek.getMonth() + 1
        }/${this.startDateOfWeek.getDate()}/${this.startDateOfWeek.getFullYear()}`
      : null;

    const url = `/admin/schedule/add?template=${templateid}&startDateOfWeek=${paramDate}&week=${this.weekSelectedFromAddPage}`;
    this.router.navigateByUrl(url, {
      replaceUrl: true,
    });
  }
}
