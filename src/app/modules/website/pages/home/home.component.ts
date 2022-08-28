import {
  UserService,
  ScheduleService,
  SettingService,
} from './../../../../core/';
import {
  DateOfWeekModel,
  QueryParamsModel,
  SettingModel,
  UserForSelectModel,
} from './../../../../shared/';
import { AfterContentInit, Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import * as $ from 'jquery';
import { Subscription } from 'rxjs';
import { LocationService } from 'src/app/core/services/location.service';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { AuditScheduleModel, EScheduleStatus, ScheduleByWeekModel, ScheduleFilesAttachment, ScheduleModel } from 'src/app/shared/models/schedule.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { ScheduleDetailTabComponent } from '../../components/schedule-detail-tab/schedule-detail-tab.component';
import promise from 'src/assets/plugins/formvalidation/src/js/validators/promise';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, AfterContentInit, OnDestroy {
  scheduleFilesAttachment: ScheduleFilesAttachment[] = [];

  get scheduleStatus(): typeof EScheduleStatus {
    return EScheduleStatus;
  }

  constructor(
    private userService: UserService,
    private settingService: SettingService,
    private scheduleService: ScheduleService,
    private locationService: LocationService,
    private modalService: NgbModal,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }
  weeks = [];
  weekSelected = 1;
  allDayOfWeek: DateOfWeekModel[] = [];
  hosts: UserForSelectModel[] = [
    {
      id: 0,
      fullName: 'Người Chủ Trì',
    },
  ]; // Người chủ trì
  hostSelected = 0;
  startDateOfWeek: Date;
  endDateOfWeek: Date;
  position = '';
  schedules: ScheduleByWeekModel[] = [];
  noticeWeeklySetting: SettingModel;
  locationSelected = -1;
  locations: ILocationModel[] = [
    {
      id: -1,
      title: '--- Chọn địa điểm ---',
    },
  ];
  isAllDayOfWeekSelected = false;

  scheduleDetail: ScheduleModel;
  todayVar = new Date();

  showPrev = false;
  loadPrevWeek = false;
  haveHid = false;

  // Subscriptions
  private subscriptions: Subscription[] = [];
  scheduleHistory: AuditScheduleModel[] = [];

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadHost();
    this.loadWeeks();
    this.loadScheduleByAllDayOfWeek();
    this.loadPrevWeek = false;
    this.loadSetting();
    this.loadLocation();    
  }

  ngAfterContentInit(): void {
    const scheduleIdstr = this.activatedRoute.snapshot.queryParams.sid;
    if (scheduleIdstr) {
      const scheduleId = parseInt(scheduleIdstr, 10);
      this.initModalDetail(scheduleId);
    }
  }

  initModalDetail(scheduleId: number): void {
    const detail = this.scheduleService.getScheduleById(scheduleId).toPromise();
    const history = this.scheduleService.getScheduleHistory(scheduleId).toPromise();
    const attachments = this.scheduleService.getAllFilesAttachments(scheduleId).toPromise();
    Promise.all([detail, history, attachments]).then((res) => {
      if (res[0].isSuccess) {
        this.scheduleDetail = null;
        this.scheduleDetail = res[0].result as ScheduleModel;
      }

      if (res[1].isSuccess) {
        this.scheduleHistory = res[1].result;
      }

      if (res[2].isSuccess) {
        this.scheduleFilesAttachment = [];
        this.scheduleFilesAttachment = res[2].result as ScheduleFilesAttachment[];
      }

      const modelref = this.modalService.open(ScheduleDetailTabComponent, { size: 'lg', scrollable: true, backdrop: 'static' });
      modelref.componentInstance.scheduleDetail = this.scheduleDetail;
      modelref.componentInstance.scheduleHistory = this.scheduleHistory;
      modelref.componentInstance.scheduleFilesAttachment = this.scheduleFilesAttachment;
    });
  }

  loadSetting(): void {
    const subBanner = this.settingService
      .getByKey('SettingPageNoticeWeekly')
      .subscribe((response) => {
        if (response.isSuccess) {
          this.noticeWeeklySetting = response.result;
        }
      });
    this.subscriptions.push(subBanner);
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
  }

  checkPrevDays(dow: string): boolean{    
    if(this.loadPrevWeek && dowVal != -1) return true; 
    var dowVal = -1;
    if(dow.includes('Thứ hai')) dowVal = 1;
    else if(dow.includes('Thứ ba')) dowVal = 2;
    else if(dow.includes('Thứ tư')) dowVal = 3;
    else if(dow.includes('Thứ năm')) dowVal = 4;
    else if(dow.includes('Thứ sáu')) dowVal = 5;
    else if(dow.includes('Thứ bảy')) dowVal = 6;
    else if(dow.includes('Chủ nhật')) dowVal = 0; 
    else return true;  
    var curVal = new Date().getDay();
    console.log(dowVal);
    console.log(curVal);
    if(dowVal < curVal) return false;
    this.haveHid = true;
    return true;
  }

  toggleShowPrev(){
    this.showPrev = !this.showPrev;
  }

  loadHost(): void {
    const loadSub = this.userService
      .getUserForSelect(1, null)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.hosts = this.hosts.concat(response.result);
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
        }
      });

    this.subscriptions.push(locationSub);
  }

  loadSchedule(itemSelected?: DateOfWeekModel): void {
    let today = new Date();

    if (itemSelected) {
      // click to day tab
      today = itemSelected.fullDay;
      // tslint:disable-next-line:prefer-for-of
      for (let index = 0; index < this.allDayOfWeek.length; index++) {
        // reset isActive to false
        const element = this.allDayOfWeek[index];
        element.isActive = false;
      }
      itemSelected.isActive = true;
    } else {
      this.getAllDateOfWeek();
      if (this.weekSelected !== this.getWeekNumber(today)) {
        today = this.startDateOfWeek; // get schedule by monday
      }
    }
    this.isAllDayOfWeekSelected = false;

    const filter = {
      host: this.hostSelected,
      locationId: this.locationSelected,
      startDate: `${today.getMonth() + 1
        }/${today.getDate()}/${today.getFullYear()}`,
      endDate: `${this.endDateOfWeek.getMonth() + 1
        }/${this.endDateOfWeek.getDate()}/${this.endDateOfWeek.getFullYear()}`,
      selectAllWeek: false,
      active: 1,
      status: -1,
    };    
    const queryParams = new QueryParamsModel(filter);
    const loadSub = this.scheduleService
      .getAllByWeekForFE(queryParams)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.schedules = response.result;
          this.schedules.forEach(day => {
            day.morning = day.morning.sort(this.compareByTime);
            day.afternoon = day.afternoon.sort(this.compareByTime);
            day.evening = day.evening.sort(this.compareByTime);
          }); 
        }
      });

    this.subscriptions.push(loadSub);
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

  loadScheduleByAllDayOfWeek(): void {
    this.loadPrevWeek = true;
    this.haveHid = false;
    const today = new Date();
    this.getAllDateOfWeek();
    // tslint:disable-next-line:prefer-for-of
    for (let index = 0; index < this.allDayOfWeek.length; index++) {
      // reset isActive to false
      const element = this.allDayOfWeek[index];
      element.isActive = false;
    }

    this.isAllDayOfWeekSelected = true;

    const filter = {
      host: this.hostSelected,
      locationId: this.locationSelected,
      startDate: `${this.startDateOfWeek.getMonth() + 1
        }/${this.startDateOfWeek.getDate()}/${this.startDateOfWeek.getFullYear()}`,
      endDate: `${this.endDateOfWeek.getMonth() + 1
        }/${this.endDateOfWeek.getDate()}/${this.endDateOfWeek.getFullYear()}`,
      selectAllWeek: true,
      active: 1, //Change 1 to -1
      status: -1,
    };
    const queryParams = new QueryParamsModel(filter);
    const loadSub = this.scheduleService
      .getAllByWeekForFE(queryParams)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.schedules = response.result;
          this.schedules.forEach(day => {
            day.morning = day.morning.sort(this.compareByTime);
            day.afternoon = day.afternoon.sort(this.compareByTime);
            day.evening = day.evening.sort(this.compareByTime);
          }); 
        }
      });

    this.subscriptions.push(loadSub);
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

  getAllDateOfWeek(): void {
    this.allDayOfWeek = [];
    this.onSelectWeek(this.weekSelected);

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

    // get isActive
    const now = new Date();
    // tslint:disable-next-line:prefer-for-of
    for (let index = 0; index < this.allDayOfWeek.length; index++) {
      const element = this.allDayOfWeek[index];
      if (element.fullDay.getDay() === now.getDay()) {
        element.isActive = true;
      }
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

  setActiveDate(day: Date): boolean {
    const today = new Date();
    return (
      today.getDate() === day.getDate() && today.getMonth() === day.getMonth()
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
  }

  viewScheduleContent(id: any): void {
    const hasClass = $('#view-content-' + id).hasClass('hide');
    if (hasClass) {
      $('#view-content-' + id)
        .removeClass('hide')
        .addClass('show');
      $('#btn-view-content-' + id).text('Đóng lại');
    } else {
      $('#view-content-' + id)
        .addClass('hide')
        .removeClass('show');
      $('#btn-view-content-' + id).text('Chi tiết...');
    }
  }

  onViewDetailSchedule(item: ScheduleModel): void {
    // this.router.navigate([], {
    //   relativeTo: this.activatedRoute,
    //   queryParams: {
    //     sid: item.scheduleId,
    //   },
    //   queryParamsHandling: 'merge', // remove to replace all query params by provided
    // });
    // this.initModalDetail(item.scheduleId);

    this.router.navigate([]).then(res =>{
      window.open(`/scheduler/schedule-detail/?sid=${item.scheduleId}`, '_blank');
    });

    // this.scheduleService.getScheduleById(item.scheduleId).subscribe((response) => {
    //   if (response.isSuccess) {
    //     this.scheduleDetail = null;
    //     this.scheduleDetail = response.result as ScheduleModel;
    //     this.modalService.open(content, { size: 'lg', scrollable: true });
    //   }
    // });

    // this.scheduleService.getScheduleHistory(item.scheduleId).subscribe((response) => {
    //   if (response.isSuccess) {
    //     this.scheduleHistory = response.result;
    //   }
    // });

    // this.scheduleService.getAllFilesAttachments(item.scheduleId).subscribe((response) => {
    //   if (response.isSuccess) {
    //     this.scheduleFilesAttachment = [];
    //     this.scheduleFilesAttachment = response.result as ScheduleFilesAttachment[];
    //   }
    // });
  }

  mapScheduleStatus(value): string {
    if (value === EScheduleStatus.Pending) {
      return 'Đang soạn thảo';
    } else if (value === EScheduleStatus.Approve) {
      return 'Đã duyệt';
    } else if (value === EScheduleStatus.Pause) {
      return 'Lịch bị hoãn';
    } else if (value === EScheduleStatus.Changed) {
      return 'Lịch bị dời';
    } else if (value === EScheduleStatus.Release) {
      return 'Đã phát hành';
    }
  }

  printDiv(): void {
    let printContents: any;
    let popupWin: any;
    const stylesHtml = this.getTagsHtml('style');
    const linksHtml = this.getTagsHtml('link');
    printContents = document.getElementById('printDiv').innerHTML;
    popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
      <html>
        <head>
          <title>Lich hop CG</title>
            ${linksHtml}
            ${stylesHtml}
<style>
 body{
font-family:'Times New Roman';
font-size:13px;
}
  .wb-notice-weekly {
	padding: 10px 0;
	line-height: 25px;
}

.wb-week-tab {
	margin: 20px 0;
	display: flex;
	flex-direction: row;
}

.wb-week-tab ul {
	margin: 0px;
	padding: 0px;
	list-style: none;
}

.wb-week-tab ul li {
	display: inline-block;
}

.wb-week-tab ul li a {
	display: block;
	padding: 5px 20px;
	color: #2971a4;
	text-align: center;
	background: #ebeded;
}

.wb-week-tab ul li span {
	color: #202124;
	display: block;
}

.wb-week-tab ul li:first-child a {
	border-radius: 5px 0px 0px 0px;
}

.wb-week-tab ul li:last-child a {
	border-radius: 0px 5px 0px 0px;
}

.wb-schedule-table {
	display: flex;
	flex-direction: row;
	width: 100%;
	flex-wrap: wrap;
}

.wb-schedule-table.date_left {
	background: #ffffcc;
	text-align: center;
	padding: 5px 0;
}

.wb-schedule-table.lich_Header {
	background-color: #ffffcc;
	border-right: solid 2px #81a3d0;
	height: 40px;
}

.wb-schedule-table.clscontentLich tbody {
	flex-wrap: wrap;
	tr {
		width: 100%;
		td {
			vertical-align: middle;
		}
	}
}

.wb-schedule-table.clscontentLich.date_left {
	border-bottom: 2px solid #81a3d0;
	background-color: white;
	padding: 0;
	border-left: 2px solid #81a3d0;
	border-right: 2px solid #81a3d0;
}

.wb-schedule-table.clscontentLich.date_right {
	border-bottom: 2px solid #81a3d0;
	background-color: white;
	padding: 0;
	border-right: 2px solid #81a3d0;
	line-height: 25px;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet_End {
	border-right: 2px solid #c6dbff;
	padding: 5px;
	height: 40px;
	color: #003399;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet {
	border-right: 1px solid #c6dbff;
	font: 13px Arial;
	padding: 5px;
	color: #003399;
	line-height: 21px;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet.btn-expan-all {
	text-align: right;
	float: right;
	font-size: 11px;
	color: #28a745;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet.hide {
	display: none;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet.show {
	display: block;
}

.wrapper-content {
	width: 100%; // chiều rộng bằng khối bao nó
	white-space: pre-wrap;
	overflow: hidden; // ẩn các nội dung khi kích thước lớn hơn chiều rộng khối bên ngoài
	text-overflow: ellipsis; //thêm 3 dấu chấm ở cuối
	-webkit-line-clamp: 3; // số dòng muốn hiển thị
	-webkit-box-orient: vertical;
	display: -webkit-box;
}

.detail-table-schedule tbody {}
		.detail-table-schedule tbody tr {}
			.detail-table-schedule tbody tr td {
				border-bottom: 1px solid #c6dbff;
			}

		.detail-table-schedule tbody tr:last-child {}
			.detail-table-schedule tbody tr:last-child  td {
				border-bottom: none;
			}

.wb-no-data {
	display: flex;
	flex-direction: row;
	width: 100%;
	padding: 20px 0px;
	text-align: center;
	font-weight: bold;
	font-size: 13px;
}

.dark-modal .modal-content {
	background-color: #292b2c;
	color: white;
}

.dark-modal .close {
	color: white;
}

.light-blue-backdrop {
	background-color: #5cb3fd;
}
</style>
        </head>
          <body>${printContents}</body>
      </html>`
    );
    popupWin.document.close();
    // <body onload="window.print();window.close()" > ${ printContents } </body>
  }

  printWord(): void {
    var html = document.getElementById('printDiv').innerHTML;
    var styles = `<style>
 body{
font-family:'Times New Roman';
font-size:13px;
}
  .wb-notice-weekly {
	padding: 10px 0;
	line-height: 25px;
}

.wb-week-tab {
	margin: 20px 0;
	display: flex;
	flex-direction: row;
}

.wb-week-tab ul {
	margin: 0px;
	padding: 0px;
	list-style: none;
}

.wb-week-tab ul li {
	display: inline-block;
}

.wb-week-tab ul li a {
	display: block;
	padding: 5px 20px;
	color: #2971a4;
	text-align: center;
	background: #ebeded;
}

.wb-week-tab ul li span {
	color: #202124;
	display: block;
}

.wb-week-tab ul li:first-child a {
	border-radius: 5px 0px 0px 0px;
}

.wb-week-tab ul li:last-child a {
	border-radius: 0px 5px 0px 0px;
}

.wb-schedule-table {
	display: flex;
	flex-direction: row;
	width: 100%;
	flex-wrap: wrap;
}

.wb-schedule-table.date_left {
	background: #ffffcc;
	text-align: center;
	padding: 5px 0;
}

.wb-schedule-table.lich_Header {
	background-color: #ffffcc;
	border-right: solid 2px #81a3d0;
	height: 40px;
}

.wb-schedule-table.clscontentLich tbody {
	flex-wrap: wrap;
	tr {
		width: 100%;
		td {
			vertical-align: middle;
		}
	}
}

.wb-schedule-table.clscontentLich.date_left {
	border-bottom: 2px solid #81a3d0;
	background-color: white;
	padding: 0;
	border-left: 2px solid #81a3d0;
	border-right: 2px solid #81a3d0;
}

.wb-schedule-table.clscontentLich.date_right {
	border-bottom: 2px solid #81a3d0;
	background-color: white;
	padding: 0;
	border-right: 2px solid #81a3d0;
	line-height: 25px;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet_End {
	border-right: 2px solid #c6dbff;
	padding: 5px;
	height: 40px;
	color: #003399;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet {
	border-right: 1px solid #c6dbff;
	font: 13px Arial;
	padding: 5px;
	color: #003399;
	line-height: 21px;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet.btn-expan-all {
	text-align: right;
	float: right;
	font-size: 11px;
	color: #28a745;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet.hide {
	display: none;
}

.wb-schedule-table.clscontentLich.date_right.Lich_ChiTiet.show {
	display: block;
}

.wrapper-content {
	width: 100%; // chiều rộng bằng khối bao nó
	white-space: pre-wrap;
	overflow: hidden; // ẩn các nội dung khi kích thước lớn hơn chiều rộng khối bên ngoài
	text-overflow: ellipsis; //thêm 3 dấu chấm ở cuối
	-webkit-line-clamp: 3; // số dòng muốn hiển thị
	-webkit-box-orient: vertical;
	display: -webkit-box;
}

.detail-table-schedule tbody {}
		.detail-table-schedule tbody tr {}
			.detail-table-schedule tbody tr td {
				border-bottom: 1px solid #c6dbff;
			}

		.detail-table-schedule tbody tr:last-child {}
			.detail-table-schedule tbody tr:last-child  td {
				border-bottom: none;
			}

.wb-no-data {
	display: flex;
	flex-direction: row;
	width: 100%;
	padding: 20px 0px;
	text-align: center;
	font-weight: bold;
	font-size: 13px;
}

.dark-modal .modal-content {
	background-color: #292b2c;
	color: white;
}

.dark-modal .close {
	color: white;
}

.light-blue-backdrop {
	background-color: #5cb3fd;
}
</style>`;
    var header = "<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
      "xmlns:w='urn:schemas-microsoft-com:office:word' " +
      "xmlns='http://www.w3.org/TR/REC-html40'>" +
      "<head><meta charset='utf-8'><title>Export Table to Word</title>" + styles + "</head><body>";
    var footer = "</body></html>";
    var sourceHTML = header + html + footer;
    if (navigator.msSaveBlob) { // IE 10+
      navigator.msSaveBlob(new Blob([sourceHTML], { type: 'application/vnd.ms-word' }), "lichCG.doc");
    } else {
      var source = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(sourceHTML);
      var fileDownload = document.createElement("a");
      document.body.appendChild(fileDownload);
      fileDownload.href = source;
      fileDownload.download = 'lichCG.doc';
      fileDownload.click();
      document.body.removeChild(fileDownload);
    }
  }
  private getTagsHtml(tagName: keyof HTMLElementTagNameMap): string {
    const htmlStr: string[] = [];
    const elements = document.getElementsByTagName(tagName);
    for (let idx = 0; idx < elements.length; idx++) {
      htmlStr.push(elements[idx].outerHTML);
    }

    return htmlStr.join('');
  }
}
