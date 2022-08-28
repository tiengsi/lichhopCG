import { LocationService } from './../../../../../core/services/location.service';
import {
  DateOfWeekModel,
  EScheduleStatus,
  QueryParamsModel,
  ScheduleModel,
  UserForSelectModel,
} from './../../../../../shared/';
import {
  LayoutUtilsService,
  UserService,
  SubheaderService,
  ScheduleService,
} from './../../../../../core/';
import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import {
  NgbDateStruct,
  NgbDropdownConfig,
  NgbModal,
} from '@ng-bootstrap/ng-bootstrap';
import { SelectOption } from 'src/app/shared/models/SelectOption.model';
import { ROLES } from 'src/app/shared/models/permission';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';
import { SchedulePauseComponent } from '../components/schedule-pause/schedule-pause.component';
import { ToastrService } from 'ngx-toastr';
import { ScheduleChangeComponent } from '../components/schedule-change/schedule-change.component';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { SendEmailSmsComponent } from '../components/send-email-sms/send-email-sms.component';
import {
  EScheduleAddType,
  ScheduleByWeekModel,
} from 'src/app/shared/models/schedule.model';
import * as $ from 'jquery';
import { DatePipe, Location } from '@angular/common';
import { ScheduleQrcodeComponent } from '../components/schedule-qrcode/schedule-qrcode.component';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';

@Component({
  selector: 'app-schedule-list',
  templateUrl: './schedule-list.component.html',
  styleUrls: ['./schedule-list.component.scss'],
  providers: [NgbDropdownConfig],
})
export class ScheduleListComponent implements OnInit, OnDestroy {
  roles = ROLES;
  schedules: ScheduleByWeekModel[] = [];
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
  startDateOfWeekSelected: Date;
  endDateOfWeek: Date;
  allDayOfWeek: DateOfWeekModel[] = [];
  txtAllDayOfWeek = 'Tất cả ngày trong tuần';
  dayOfWeekSelected: any = this.txtAllDayOfWeek;
  selectAllWeek = true;
  selectedDateFromAddPage;
  weekSelectedFromAddPage = 0;
  daySelectedToShow;

  UIPermissions: any = {};
  accessable = true;  
  showCreateBtn = true;  
  showEditBtn = true;
  showDelayBtn = true;
  showSendEmailBtn = true;
  showCopyBtn = true;
  showApproveBtn = true;
  showChangeDateBtn = true;
  showQRBtn = true;  
  showDeleteBtn = true;
  showPublishBtn = true;

  

  constructor(
    private subheaderService: SubheaderService,
    private scheduleService: ScheduleService,
    private userService: UserService,
    private toastr: ToastrService,
    private layoutUtilsService: LayoutUtilsService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private modalService: NgbModal,
    private locationService: LocationService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList,
    btnconfig: NgbDropdownConfig
  ) {
    this.selectedDateFromAddPage =
      this.activatedRoute.snapshot.queryParams?.startDateOfWeek;
    const weekParam =
      this.activatedRoute.snapshot.queryParams?.week || 0;
    this.weekSelectedFromAddPage = parseInt(weekParam, 10);
 
    btnconfig.placement = 'bottom';
    btnconfig.autoClose = true;
  }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_CREATE];;  
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_EDIT];
        this.showDelayBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_DELAY];
        this.showSendEmailBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_SENDMAIL];
        this.showCopyBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_COPY];
        this.showApproveBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_APPROVE];
        this.showChangeDateBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_CHANGEDATE];
        this.showQRBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_QR];  
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_DELETE];
        this.showPublishBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_PUBLISH];       
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
      { title: 'Quản lý lịch họp', page: `/admin/schedule` },
    ]);
  }

  get scheduleStatus(): typeof EScheduleStatus {
    return EScheduleStatus;
  }

  get scheduleAddTypes(): typeof EScheduleAddType {
    return EScheduleAddType;
  }

  loadWeeks(isOtherWeek = false): void {
    for (let index = 1; index < 54; index++) {
      this.weeks.push({
        id: index,
        name: 'Tuần ' + index,
      });
    }

    const now = new Date();
    this.weekSelected = this.weekSelectedFromAddPage ? this.weekSelectedFromAddPage : this.getWeekNumber(now);

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

      this.dayOfWeekSelected = this.daySelectedToShow;
      this.loadSchedule(false, false);
    } else {
      if (isOtherWeek) {
        this.dayOfWeekSelected = 'Tất cả ngày trong tuần';
        this.weekSelectedFromAddPage = this.weekSelected;
        this.getAllDateOfWeek();
      }
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

  onSelectWeek(week: number): void {
    const now = new Date();
    const oneJan = new Date(now.getFullYear(), 0, 1);
    const w =
      oneJan.getTime() -
      3600000 * 24 * (oneJan.getDay() - 1) +
      604800000 * (week - 1);
    this.startDateOfWeek = new Date(w);
    this.endDateOfWeek = new Date(w + 518400000);
    this.dayOfWeekSelected = 'Tất cả ngày trong tuần';
    this.getAllDateOfWeek();
    this.deleteQueryParamFromUrl();

    const queryParams = {
      week: this.weekSelected
    };
    const urlTree = this.router.createUrlTree(['/admin/schedule'], {
      queryParams,
    });
    this.router.navigateByUrl(urlTree);

    this.weekSelectedFromAddPage = this.weekSelected;
    this.loadSchedule(true, true);
  }

  onSelectDayOfWeek(dayOfWeek: any): void {
    this.selectedDateFromAddPage = null;
    if (typeof dayOfWeek === 'string' && dayOfWeek === this.txtAllDayOfWeek) {
      this.deleteQueryParamFromUrl();
      this.loadWeeks(true);
    } else {
      // chỉ tìm 1 ngày trong tuần
      this.startDateOfWeek = dayOfWeek;
      this.endDateOfWeek = dayOfWeek;
      this.startDateOfWeekSelected = dayOfWeek;
      const datePipe = new DatePipe('en-US');
      this.daySelectedToShow =
        this.getDayNameOfWeek(dayOfWeek) +
        ' ngày ' +
        datePipe.transform(dayOfWeek, 'dd/MM/yyyy');
      this.loadSchedule(false, false);

      this.router.navigate([], {
        relativeTo: this.activatedRoute,
        queryParams: {
          startDateOfWeek: datePipe.transform(dayOfWeek, 'yyyy/MM/dd'),
          week: this.weekSelected
        },
        queryParamsHandling: 'merge', // remove to replace all query params by provided
      });
    }
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
    if (isWeekSelectEvent) {
      this.selectedDateFromAddPage = null;
    }

    if (this.selectedDateFromAddPage) {
      this.startDateOfWeek = new Date(this.selectedDateFromAddPage);
      this.endDateOfWeek = new Date(this.selectedDateFromAddPage);
    }
    let startDate = '';
    if (this.startDateOfWeek) {
      startDate = `${this.startDateOfWeek.getMonth() + 1
        }/${this.startDateOfWeek.getDate()}/${this.startDateOfWeek.getFullYear()}`;
    }

    let endDate = '';
    if (this.endDateOfWeek) {
      endDate = `${this.endDateOfWeek.getMonth() + 1
        }/${this.endDateOfWeek.getDate()}/${this.endDateOfWeek.getFullYear()}`;
    }

    this.selectAllWeek = startDate === endDate ? false : selectAllWeek;

    const filter = {
      host: this.hostSelected,
      startDate,
      endDate,
      active: this.activeSelected,
      status: this.statusSelected,
      locationId: this.locationSelected,
      selectAllWeek: this.selectAllWeek,
    };
    const queryParams = new QueryParamsModel(
      filter,
      this.sortEvent.direction,
      this.sortEvent.column,
      this.page,
      this.pageSize
    );

    const subScription = this.scheduleService
      .getAllByWeek(queryParams)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.schedules = response.result; 
          this.schedules.forEach(day => {
            day.morning = day.morning.sort(this.compareByTime);
            day.afternoon = day.afternoon.sort(this.compareByTime);
            day.evening = day.evening.sort(this.compareByTime);
          });
          
          this.schedules.forEach(day =>{
            day.morning.forEach(e =>{
              if(e.scheduleEndDate == null) e.scheduleEndDate = e.scheduleDate;
              if(e.scheduleEndTime == null) e.scheduleEndTime = e.scheduleTime;
            });
            day.afternoon.forEach(e =>{
              if(e.scheduleEndDate == null) e.scheduleEndDate = e.scheduleDate;
              if(e.scheduleEndTime == null) e.scheduleEndTime = e.scheduleTime;
            });
            day.evening.forEach(e =>{
              if(e.scheduleEndDate == null) e.scheduleEndDate = e.scheduleDate;
              if(e.scheduleEndTime == null) e.scheduleEndTime = e.scheduleTime;
            });
          })
          
        } else {
          this.toastr.error(response.message);
        }
      });

    this.subscriptions.push(subScription);
  }

  compareByTime(a: ScheduleModel, b: ScheduleModel){
    var a_time = +a.scheduleTime.split(':')[0];
    var b_time = +b.scheduleTime.split(':')[0];
    var a_min = +a.scheduleTime.split(':')[1];
    var b_min = +b.scheduleTime.split(':')[1];
    console.log(a_time + ' - zz - ' + b_time);    
    if ( a_time < b_time ){
    return -1;
    }
    if ( a_time > b_time ){
      return 1;
    }
    else{
      if(a_min > b_min) return -1;
      if(a_min < b_min) return 1;
      return 0;
    }
  }

  editSchedule(id, sendMail:boolean = false): void {
    // this.router.navigate(['./edit', id], { relativeTo: this.activatedRoute });
    var toRoute = sendMail ? './invite' : './edit';
    const paramDate = this.startDateOfWeekSelected
      ? `${this.startDateOfWeekSelected.getMonth() + 1
      }/${this.startDateOfWeekSelected.getDate()}/${this.startDateOfWeekSelected.getFullYear()}`
      : null;
    this.router.navigate([toRoute, id], {
      relativeTo: this.activatedRoute,
      queryParams: { 
        startDateOfWeek: paramDate,
         week: this.weekSelected },
      queryParamsHandling: 'merge',
    });
  }

  redirectAddPage(): void {
    const paramDate = this.startDateOfWeekSelected
      ? `${this.startDateOfWeekSelected.getMonth() + 1
      }/${this.startDateOfWeekSelected.getDate()}/${this.startDateOfWeekSelected.getFullYear()}`
      : null;

    this.router.navigate(['./add'], {
      relativeTo: this.activatedRoute,
      queryParams: { startDateOfWeek: paramDate, week: this.weekSelected },
      queryParamsHandling: 'merge',
    });
  }

  copySchedule(scheduleId: number): void {
    if (this.selectAllWeek) {
      this.router.navigate(['./edit', scheduleId], {
        relativeTo: this.activatedRoute,
        queryParams: {
          isCopy: 1,
          week: this.weekSelected
        },
        queryParamsHandling: 'merge',
      });
    } else {
      const paramDate = this.startDateOfWeek
        ? `${this.startDateOfWeek.getMonth() + 1
        }/${this.startDateOfWeek.getDate()}/${this.startDateOfWeek.getFullYear()}`
        : null;

      this.router.navigate(['./edit', scheduleId], {
        relativeTo: this.activatedRoute,
        queryParams: {
          startDateOfWeek: paramDate,
          week: this.weekSelected,
          isCopy: 1,
        },
        queryParamsHandling: 'merge',
      });
    }
  }

  deleteSchedule(item: ScheduleModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.scheduleService
          .deleteSchedule(item.scheduleId)
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

  onChangeStatus(item: ScheduleModel): void {
    this.scheduleService.updateStatus(item.scheduleId).subscribe((response) => {
      if (response.isSuccess) {
        this.toastr.success(
          item.isActive
            ? 'Ẩn lịch thành công!'
            : 'Bạn đã hiển thị lịch thành công!'
        );
        this.loadSchedule();
      } else {
        this.toastr.error('Thay đổi thất bại, xin vui lòng thử lại!');
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

  openSchedulePauseModal(scheduleId: number): void {
    const modalRef = this.modalService.open(SchedulePauseComponent, {
      size: 'xl',
    });
    modalRef.componentInstance.scheduleId = scheduleId;
    modalRef.result.then((result) => {
      if (result.submit) {
        this.loadSchedule();
      }
    });
  }

  openQRCodeInfoModal(scheduleId: number): void{
    const modalRef = this.modalService.open(ScheduleQrcodeComponent, {
      size: 'xl'
    });
    modalRef.componentInstance.scheduleId = scheduleId;    
  }

  openScheduleChangeModal(scheduleId: number): void {
    const modalRef = this.modalService.open(ScheduleChangeComponent, {
      size: 'xl',
    });
    modalRef.componentInstance.scheduleId = scheduleId;
    modalRef.result.then((result) => {
      if (result.submit) {
        this.loadSchedule();
      }
    });
  }

  approve(schedule: ScheduleModel): void {
    if (schedule.scheduleStatus === EScheduleStatus.Approve) {
      this.toastr.warning('Lịch này đã được duyệt trước đó!');
      return;
    }
    const subApprove = this.scheduleService
      .approve(schedule.scheduleId)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastr.success('Bạn đã duyệt lịch thành công');
          this.loadSchedule();
        } else {
          this.toastr.error('Duyệt lịch thất bại, xin vui lòng thử lại!');
        }
      });

    this.subscriptions.push(subApprove);
  }

  releaseSchedule(): void {
    const conf = confirm(
      'Bạn có thực sự muốn phát hành tất cả lịch trong tuần không?'
    );
    if (!conf) {
      return;
    }

    let startDate = '';
    if (this.startDateOfWeek) {
      startDate = `${this.startDateOfWeek.getMonth() + 1
        }/${this.startDateOfWeek.getDate()}/${this.startDateOfWeek.getFullYear()}`;
    }

    let endDate = '';
    if (this.endDateOfWeek) {
      endDate = `${this.endDateOfWeek.getMonth() + 1
        }/${this.endDateOfWeek.getDate()}/${this.endDateOfWeek.getFullYear()}`;
    }

    const payload = {
      startDate,
      endDate,
    };

    this.scheduleService.release(payload).subscribe((response) => {
      if (response.isSuccess) {
        this.toastr.success('Bạn đã phát hành lịch tuần thành công');
        this.loadSchedule();
      } else {
        this.toastr.error(response.message);
      }
    });
  }

  releaseSingleSchedule(schedule: ScheduleModel){
    const conf = confirm(
      `Bạn muốn phát hành lịch ${schedule.scheduleTitle}?`
    );
    if (!conf) {
      return;
    }
    this.scheduleService.releaseById(schedule.scheduleId).subscribe((res) => {
      if(res.isSuccess){
        this.toastr.success('Đã phát hành lịch thành công')
        this.loadSchedule();
      }else{
        this.toastr.error(res.message);
      }
    })
  }

  openSendScheduleModel(schedule: ScheduleModel): void {
    const modalRef = this.modalService.open(SendEmailSmsComponent, {
      size: 'xl',
    });
    modalRef.componentInstance.schedule = schedule;
    modalRef.result.then((result) => {
      if (result.submit) {
        this.loadSchedule();
      }
    });
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

  redirectAddPageWithType(date: string, type: any): void {
    this.router.navigate(['./add'], {
      relativeTo: this.activatedRoute,
      queryParams: {
        typeAdd: type,
        startDateOfWeek: this.getDateFromSchedule(date),
        week: this.weekSelected
      },
      replaceUrl: true,
    });
  }

  redirectAddTemplate(date: string): void {
    const url = `/admin/schedule-template`;
    const queryParams = {
      startDateOfWeek: this.getDateFromSchedule(date),
      week: this.weekSelected
    };
    // const urlTree = this.serializer.serialize(
    //   this.router.createUrlTree([url], {
    //     queryParams,
    //   })
    // );
    const urlTree = this.router.createUrlTree([url], {
      queryParams,
    });
    this.router.navigateByUrl(urlTree, { replaceUrl: true });
  }

  private getDateFromSchedule(date: string): string {
    const scheduleDate = date
      ? date.substring(date.length - 5, date.length)
      : '';
    let day = 0;
    let month = 0;
    if (scheduleDate) {
      day = parseInt(scheduleDate.split('/')[0], 10);
      month = parseInt(scheduleDate.split('/')[1], 10);
    }

    const selectedDate = this.allDayOfWeek.find((r) => {
      const dateSelected = new Date(r.fullDay);
      return (
        dateSelected.getDate() === day && dateSelected.getMonth() + 1 === month
      );
    });

    let resultDate = '';
    if (selectedDate) {
      resultDate = `${selectedDate.fullDay.getMonth() + 1
        }/${selectedDate.fullDay.getDate()}/${selectedDate.fullDay.getFullYear()}`;
    }

    if (!selectedDate && this.startDateOfWeek) {
      resultDate = `${this.startDateOfWeek.getMonth() + 1
        }/${this.startDateOfWeek.getDate()}/${this.startDateOfWeek.getFullYear()}`;
    }

    return resultDate;
  }
}
