import { ITitleTemplate } from './../../../../../shared/models/title-template.model';
import { TitleTemplateService } from './../../../../../core/services/title-template.service';
import {
  AuthModel,
  EParticipantType,
  EScheduleStatus,
  IOtherParticipantSelected,
  IParticipant,
  IParticipantIsSelected,
  QueryParamsModel,
  ScheduleModel,
  UserForSelectModel,
} from './../../../../../shared/';
import {
  SubheaderService,
  UserService,
  ScheduleService,
  ParticipantService,
  AuthService,
} from './../../../../../core/';
import { Component, EventEmitter, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { NgbDatepickerConfig, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { LocationService } from 'src/app/core/services/location.service';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { DepartmentService } from 'src/app/core/services/department.service';
import * as _ from 'lodash';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { EditorConfig } from 'src/app/shared/models/post.model';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { FileUploader } from 'ng2-file-upload';
import { LoadingBarService } from '@ngx-loading-bar/core';
import {
  EScheduleAddType,
  ScheduleFilesAttachment,
} from 'src/app/shared/models/schedule.model';
import { ScheduleTemplateModel } from 'src/app/shared/models/schedule-template.model';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';
import { constCollect } from 'src/app/configs/const';
import { BrandNameService } from 'src/app/core/services/brandname.service';
import { BrandNameModel } from 'src/app/shared/models/brand-name.model';

@Component({
  selector: 'app-schedule-add',
  templateUrl: './schedule-add.component.html',
  styleUrls: ['./schedule-add.component.scss'],
})
export class ScheduleAddComponent implements OnInit, OnDestroy {
  hasFormErrors = false;    
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  userId = this.userInfo?.userId != undefined ? this.userInfo.userId : 0;
  schedule: ScheduleModel = new ScheduleModel();  
  scheduleTemplate: ScheduleTemplateModel = new ScheduleTemplateModel();
  isInvite = false;
  listParticipant: IParticipant[];
  brandNames: BrandNameModel[] = [];
  locations: ILocationModel[] = [
    {
      id: 0,
      title: '--- Chọn địa điểm ---',
    },
  ];
  scheduleDate: NgbDateStruct;
  scheduleEndDate: NgbDateStruct;
  sendMailDate: NgbDateStruct;
  users: UserForSelectModel[] = [
    {
      id: 0,
      fullName: '--- Chọn Người Chủ Trì ---',
      officerPosition: '',
    },
  ];
  titleTemplates: ITitleTemplate[] = [
    {
      id: 0,
      template: '--- Chọn mẫu tiêu đề ---',
      organizeId:0
    },
  ];
  officers: UserForSelectModel[];
  scheduleTime = { hour: 13, minute: 0 };
  scheduleEndTime = { hour: 15, minute: 0 };
  sendMailTime = {hour: 13, minute: 0};
  minuteStep = 15;

  parentDepartments: IDepartmentModel[] = [
    {
      id: 0,
      name: '--- Chọn phòng ban ---',
    },
  ];
  seletedDepartment = 0;
  participantIsSelected: IParticipantIsSelected[] = [];z
  otherParticipantSelected: IOtherParticipantSelected[] = [
    {
      name: null,
      email: null,
      phoneNumber: null,
    },
  ];
  scheduleId?: number = null;
  isModify = false;
  config: AngularEditorConfig = EditorConfig;
  selectedDateFromListPage;
  isCopy = 0;
  typeAdd = 0;
  scheduleAddDate: Date;
  uploader: FileUploader;
  scheduleForCreateUpdate = new ScheduleModel();
  loader = this.loadingBar.useRef();
  templateId = 0;

  UIPermissions: any = {};
  accessable = true;  
  showCreateBtn = true;  
  showEditBtn = true;  
  showCopyBtn = true;
  showSendEmailBtn = true;

  compareDate = 0;
  // Subscriptions
  private subscriptions: Subscription[] = [];
  files: File[] = [];
  filesAttachment: ScheduleFilesAttachment[] = [];
  weekSelected = 1;  

  constructor(
    private subheaderService: SubheaderService,
    private toastService: ToastrService,
    private userService: UserService,
    private locationService: LocationService,
    private participantService: ParticipantService,
    private scheduleService: ScheduleService,
    private router: Router,
    private departmentService: DepartmentService,
    private activatedRoute: ActivatedRoute,
    private titleTemplateService: TitleTemplateService,
    private authService: AuthService,
    private loadingBar: LoadingBarService,
    private brandNameService: BrandNameService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList,
    private datePickerConfig: NgbDatepickerConfig
  ) {
    const current = new Date();
    datePickerConfig.minDate = { year: current.getFullYear(), month: 
    current.getMonth() + 1, day: current.getDate() };
    datePickerConfig.outsideDays = 'hidden';

    this.scheduleId = +this.activatedRoute.snapshot.params?.id;
    this.selectedDateFromListPage =
    this.activatedRoute.snapshot.queryParams?.startDateOfWeek;
    this.weekSelected = this.activatedRoute.snapshot.queryParams?.week || 1;
    this.isCopy = this.activatedRoute.snapshot.queryParams?.isCopy || 0; // copy lịch
    this.typeAdd = this.activatedRoute.snapshot.queryParams?.typeAdd || 0; // kiểu thêm
    this.templateId = this.activatedRoute.snapshot.queryParams?.template || 0; // template Id
    this.scheduleAddDate =
      this.activatedRoute.snapshot.queryParams?.scheduleDate || new Date(); // scheduleAddDate
    this.weekSelected = parseInt(this.weekSelected.toString(), 0);
    this.typeAdd = parseInt(this.typeAdd.toString(), 0);
    this.isCopy = parseInt(this.isCopy.toString(), 0);
    this.templateId = parseInt(this.templateId.toString(), 0);
    this.scheduleAddDate = new Date(this.scheduleAddDate);
    this.isModify = !!this.scheduleId;
    this.isInvite = (window.location.href.indexOf("invite") > -1);
    console.log(this.isInvite);
    this.initBreadCrumbs();
  }

  showLog(){
    
    this.scheduleEndDate = this.scheduleDate;
    console.log(this.scheduleDate);
    var now = new Date();
    var selectedDate = new Date(this.scheduleDate.year, this.scheduleDate.month-1, this.scheduleDate.day);
    

    now.setHours(0,0,0,0);    

    if(selectedDate.getTime() > now.getTime()){
      this.compareDate = 1;
    }else if(selectedDate.getTime() == now.getTime()){
      this.compareDate = 0;
    }else{
      this.compareDate = -1;
    }

    console.log(this.compareDate);
  }

  checkPickDate(){
    this.showLog();
    if(this.compareDate == 0){
      var now = new Date();
      console.log(this.scheduleTime);
      console.log(now.getHours());      
      if(this.scheduleTime.hour <= now.getHours()) this.scheduleTime = {hour: now.getHours(), minute: this.scheduleTime.minute};     
    }
  }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_CREATE];;  
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_EDIT];   
        this.showCopyBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_COPY]; 
        this.showSendEmailBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_SENDMAIL];           
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
    this.initializeUploader();
    this.loadLocation();
    this.loadParticipant();
    this.loadUser(null);
    this.loadTitleTemplate();
    this.loadBrandNames();
    // this.smsPreview();

    if (this.isModify) {
      this.loadSchedule();
    }

    if (this.typeAdd) {
      this.initialScheduleTime();
    }

    if (this.templateId) {
      this.initFormAddByTemplate();
    }

    if (this.selectedDateFromListPage) {
      const scheduleDate = new Date(this.selectedDateFromListPage);
      this.scheduleDate = {
        year: scheduleDate.getFullYear(),
        month: scheduleDate.getMonth() + 1,
        day: scheduleDate.getDate(),
      };

      this.scheduleEndDate = {
        year: scheduleDate.getFullYear(),
        month: scheduleDate.getMonth() + 1,
        day: scheduleDate.getDate(),
      };
    }
  }

  initialScheduleTime(): void {
    switch (this.typeAdd) {
      case EScheduleAddType.Morning:
        this.scheduleTime.hour = 8;
        this.scheduleEndTime.hour = 10;
        break;
      case EScheduleAddType.Afternoon:
        this.scheduleTime.hour = 14;
        this.scheduleEndTime.hour = 16;
        break;
      case EScheduleAddType.Evening:
        this.scheduleTime.hour = 20;
        this.scheduleEndTime.hour = 21;
        break;
      default:
        break;
    }

    if (this.scheduleAddDate) {
      this.scheduleDate = {
        year: this.scheduleAddDate.getFullYear(),
        month: this.scheduleAddDate.getMonth() + 1,
        day: this.scheduleAddDate.getDate(),
      };
      this.scheduleEndDate = {
        year: this.scheduleAddDate.getFullYear(),
        month: this.scheduleAddDate.getMonth() + 1,
        day: this.scheduleAddDate.getDate(),
      };
    }
  }

  initializeUploader(): void {
    const BASE_URL = environment.base_url;
    const localToken = this.authService.getToken();
    const tokenObj: AuthModel = JSON.parse(localToken);
    this.uploader = new FileUploader({
      url: `${BASE_URL}uploaders/attachmentFile/v2`,
      authToken: 'Bearer ' + tokenObj.accessToken,
      isHTML5: true,
      allowedFileType: ['image', 'pdf', 'doc', 'docx', 'compress'],
      removeAfterUpload: false,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024, // 10 MB
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onBuildItemForm = (fileItem, form) => {
      console.log(fileItem.file.rawFile);
      form.append('fileUpload', fileItem.file.rawFile);
      form.append('organizeId', this.organizeId);
      form.append('userId', this.userId);
      form.append('mode', 'Schedule')
    };    

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res = JSON.parse(response);
        if (res.result.getData && this.files.find(r => r.name === item?._file?.name)) {
          this.scheduleForCreateUpdate.filePath = res.result.getData;
          this.scheduleForCreateUpdate.cloudinaryPublicId = res.result.getData;
          const filesAttachment = this.filesAttachment.find(r => r.fileName === item._file.name);
          filesAttachment.filePath = res.result.getData;
          filesAttachment.cloudinaryPublicId = res.result.getData;
          filesAttachment.fileName = item?.file.name;
          filesAttachment.releaseDate = new Date(
            `${filesAttachment.releaseDateStr.month}/${filesAttachment.releaseDateStr.day + 1}/${filesAttachment.releaseDateStr.year}`
          );
        }

      }
    };

    this.uploader.onCompleteAll = () => {
      this.loader.complete();
      this.scheduleForCreateUpdate.scheduleFilesAttachment = this.filesAttachment;
      this.updateOrCreate();
    };

    this.uploader._onErrorItem = (item, response, status, header) => {
      this.toastService.error('Đã xảy ra lỗi trong quá tình tải file!');
      this.loader.complete();
    };
  }

  get smsPreview(): string {
    let scheduleTitleTemplate = '';
    if (this.schedule.scheduleTitleTemplateId === 0) {
      scheduleTitleTemplate = '<MAU_TIEU_DE>';
    } else {
      const foundTemplate = this.titleTemplates.find(
        (item) => item.id === this.schedule.scheduleTitleTemplateId
      );
      if (foundTemplate !== undefined) {
        scheduleTitleTemplate = foundTemplate.template;
      }
    }

    let scheduleTitle = '';
    if (
      this.schedule.scheduleTitle === null ||
      this.schedule.scheduleTitle === ''
    ) {
      scheduleTitle = '<TIEU_DE_CUOC_HOP>';
    } else {
      scheduleTitle = this.schedule.scheduleTitle;
    }

    let scheduleDate = '';
    if (this.scheduleDate === undefined) {
      scheduleDate = '<THOI_GIAN_HOP>';
    } else {
      scheduleDate = `${this.scheduleDate.day}/${this.scheduleDate.month}/${this.scheduleDate.year}`;
    }

    let scheduleTimeHour = this.scheduleTime.hour.toString();
    if (this.scheduleTime.hour < 10) {
      scheduleTimeHour = `0${this.scheduleTime.hour}`;
    }

    let scheduleTimeMinute = this.scheduleTime.minute.toString();
    if (this.scheduleTime.minute < 10) {
      scheduleTimeMinute = `0${this.scheduleTime.minute}`;
    }

    const hour = `${scheduleTimeHour} giờ ${scheduleTimeMinute}`;

    let user = '';
    // tslint:disable-next-line:max-line-length
    if (
      this.schedule.id === 0 &&
      (this.schedule.otherHost === undefined ||
        this.schedule.otherHost === '' ||
        this.schedule.otherHost === null)
    ) {
      user = '';
    } else {
      if (this.schedule.id === 0) {
        user = `, do ${this.schedule.otherHost} chủ trì`;

      } else {
        const foundTemplate = this.users.find(
          (item) => item.id === this.schedule.id
        );
        if (foundTemplate !== undefined) {
          user = `, do ${foundTemplate.positionName} chủ trì`;
        }
      }
    }

    let location = '';
    if (
      this.schedule.locationId === 0 &&
      (this.schedule.otherLocation === null ||
        this.schedule.otherLocation === '')
    ) {
      location = '';
    } else {
      if (this.schedule.locationId === 0) {
        location = `, tại ${this.schedule.otherLocation}`;

      } else {
        const foundTemplate = this.locations.find(
          (item) => item.id === this.schedule.locationId
        );
        if (foundTemplate !== undefined) {
          location = `, tại ${foundTemplate.title}`;
        }
      }
    }
    return `${constCollect.prefixTitleSchedule}: ${scheduleTitleTemplate} ${scheduleTitle}, vào lúc ${hour} ngày ${scheduleDate}${user}${location}. Cac don vi xem chi tiet noi dung tai dia chi ${window.location.origin + '/scheduler/shared-documents'}?sid=${this.scheduleId}`;
  }

  get participantType(): typeof EParticipantType {
    return EParticipantType;
  }

  onFileSelected(event: EventEmitter<File[]>, index: number): any {
    const file: File = event[0];
    this.files.push(file);
    this.filesAttachment[index].fileName = file.name;
  }

  addFileAttachment(): void {
    const filesAttachment: ScheduleFilesAttachment = {
      id: 0,
      filePath: '',
      scheduleId: this.schedule.scheduleId,
      fileName: '',
      cloudinaryPublicId: '',
      notationNumber: '',
      releaseDate: new Date(),
      quote: '',
      isShare: true
    };

    this.filesAttachment.push(
      filesAttachment
    );
  }

  removeFileAttachment(index: number): void {
    this.filesAttachment.splice(index, 1);
    this.files.splice(index, 1);
  }

  loadSchedule(): void {
    const scheduleSub = this.scheduleService
      .getScheduleById(this.scheduleId)
      .subscribe((response) => {
        if (response.isSuccess) {        
          this.schedule = response.result;
          console.log(this.schedule.scheduleDate);
          console.log(new Date(this.schedule.scheduleDate).toLocaleDateString());
          console.log(new Date(this.schedule.scheduleDate).toLocaleTimeString());
          console.log(this.schedule.scheduleTime);
          this.otherParticipantSelected = this.schedule.otherParticipants;
          if (this.otherParticipantSelected.length > 0) {
            this.schedule.isOtherParticipant = true;
          }

          if(this.schedule.brandNameId == 0){
            this.schedule.brandNameId = this.brandNames[0].brandNameId;
          }

          this.participantIsSelected =
            this.schedule.participantIsSelected === null
              ? []
              : this.schedule.participantIsSelected;

          this.filesAttachment = this.schedule.scheduleFilesAttachment;
          this.filesAttachment.forEach(element => {
            const releaseDate = new Date(element.releaseDate);
            element.releaseDateStr = {
              year: releaseDate.getFullYear(),
              month: releaseDate.getMonth() + 1,
              day: releaseDate.getDate(),
            };
          });
          const date = new Date(this.schedule.scheduleDate);
          this.scheduleDate = {
            year: date.getFullYear(),
            month: date.getMonth() + 1,
            day: date.getDate(),
          };
          if(this.schedule.scheduleEndDate == null || this.schedule.scheduleEndDate == undefined){
            this.scheduleEndDate = this.scheduleDate;
          }else{
            const endDate = new Date(this.schedule.scheduleEndDate);
            this.scheduleEndDate = {
              year: endDate.getFullYear(),
              month: endDate.getMonth() + 1,
              day: endDate.getDate()
            };
          }
          if(this.schedule.scheduleTimeForScheduleJob != null){
            const date2 = new Date(this.schedule.scheduleTimeForScheduleJob);
            this.sendMailDate = {
              year: date2.getFullYear(),
              month: date2.getMonth() + 1,
              day: date2.getDate(),
            }
            this.splitTime2(date2.toLocaleTimeString());
          }
          this.splitTime();
        }
      });

    this.subscriptions.push(scheduleSub);
  }

  loadDepartment(): void {
    this.departmentService.getAllForSelect().subscribe((response) => {
      if (response.isSuccess) {
        this.parentDepartments = this.parentDepartments.concat(response.result);
      }
    });
  }

  loadLocation(): void {
    const locationSub = this.locationService
      .getAllActive()
      .subscribe((response) => {
        if (response.isSuccess) {
          this.locations = this.locations.concat(response.result);
        } else {
          this.toastService.error(
            'Lấy danh sách địa điểm thất bại, vui lòng thử lại!'
          );
        }
      });

    this.subscriptions.push(locationSub);
  }

  loadParticipant(): void {
    this.participantService.getAllForSelect().subscribe((response) => {
      if (response.isSuccess) {
        this.listParticipant = response.result;
      } else {
        this.toastService.error(
          'Lấy danh sách nhóm cán bộ thất bại, vui lòng thử lại!'
        );
      }
    });
  }

  loadUser($event: any): void {
    const departmentId = $event == null ? 0 : $event;

    // chỉ lấy cán bộ được gán cờ là chủ trì
    const userSub = this.userService
      .getUserForSelect(1, departmentId)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.users = this.users.concat(response.result);
          this.officers = response.result;
        } else {
          this.toastService.error(
            'Lấy danh sách cán bộ thất bại, vui lòng thử lại!'
          );
        }
      });

    this.subscriptions.push(userSub);
  }

  loadBrandNames(): void {
    this.brandNameService.GetVNPTBrandName().subscribe((res) => {
      if(res.isSuccess){
        var vnpts = res.result as BrandNameModel[];
        vnpts.forEach(x => {
          x.branchName = 'VNPT - ' + x.branchName;
          this.brandNames.push(x);
        })
      }
    });
    this.brandNameService.GetViettelBrandName().subscribe((res) => {
      if(res.isSuccess){
        var viettels = res.result as BrandNameModel[];
        viettels.forEach(x => {
          x.branchName = 'Viettel - ' + x.branchName;
          this.brandNames.push(x);
        })
      }
    });
  }

  loadTitleTemplate(): void {
    const queryParams = new QueryParamsModel({}, '', '', 1, 50);
    //code123
    this.titleTemplateService.getAllScheduleTitleTemplateByOrganizeId(queryParams, this.organizeId).subscribe((response) => {
      if (response.isSuccess) {
        this.titleTemplates = this.titleTemplates.concat(response.result.items);
      } else {
        this.toastService.error(response.message);
      }
    });
  }

  initBreadCrumbs(): void {
    if (this.isModify) {
      this.subheaderService.setBreadcrumbs([
        { title: 'Quản lý lịch họp', page: `admin/schedule` },
        { title: 'Chỉnh sửa', page: `admin/schedule/edit/` + this.scheduleId },
      ]);
    } else {
      this.subheaderService.setBreadcrumbs([
        { title: 'Quản lý lịch họp', page: `admin/schedule` },
        { title: 'Thêm mới', page: `admin/schedule/add` },
      ]);
    }
  }

  onSubmit(isApprove: boolean, isSendSMS = false): void {
    const participantSelected = [];
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.participantIsSelected.length; i++) {
      if (this.participantIsSelected[i].participantId !== -99) {
        participantSelected.push(this.participantIsSelected[i].participantId);
      }
    }

    this.schedule.userIds = participantSelected;

    this.schedule.otherParticipants = this.otherParticipantSelected;

    this.schedule.scheduleDate = new Date(
      `${this.scheduleDate.month}/${this.scheduleDate.day}/${this.scheduleDate.year} 07:00`
    );
    this.schedule.scheduleEndDate = new Date(
      `${this.scheduleEndDate.month}/${this.scheduleEndDate.day}/${this.scheduleEndDate.year} 07:00`
    )

    try{
      this.schedule.scheduleTimeForScheduleJob = new Date(
        `${this.sendMailDate?.month}/${this.sendMailDate?.day}/${this.sendMailDate?.year} ${this.sendMailTime.hour}:${this.sendMailTime.minute}`
      );
    }catch{

    }
    if(!this.schedule.isAutoSendAtScheduledTime){
      try{this.schedule.scheduleTimeForScheduleJob = null;}
      catch{}
    }
    let scheduleTimeHour = this.scheduleTime.hour.toString();
    if (this.scheduleTime.hour < 10) {
      scheduleTimeHour = `0${this.scheduleTime.hour}`;
    }

    let scheduleTimeMinute = this.scheduleTime.minute.toString();
    if (this.scheduleTime.minute < 10) {
      scheduleTimeMinute = `0${this.scheduleTime.minute}`;
    }
    this.schedule.scheduleTime = `${scheduleTimeHour}:${scheduleTimeMinute}`;

    let scheduleEndTimeHour = this.scheduleEndTime.hour.toString();
    if (this.scheduleEndTime.hour < 10) {
      scheduleEndTimeHour = `0${this.scheduleEndTime.hour}`;
    }

    let scheduleEndTimeMinute = this.scheduleEndTime.minute.toString();
    if (this.scheduleEndTime.minute < 10) {
      scheduleEndTimeMinute = `0${this.scheduleEndTime.minute}`;
    }
    this.schedule.scheduleEndTime = `${scheduleEndTimeHour}:${scheduleEndTimeMinute}`;

    // duyệt lịch
    if (isApprove) {
      this.schedule.scheduleStatus = EScheduleStatus.Approve;
    }

    if (this.schedule.scheduleTitleTemplateId === 0) {
      this.schedule.scheduleTitleTemplateId = null;
    }

    this.scheduleForCreateUpdate = Object.assign({}, this.schedule);

    if (this.schedule.id === 0) {
      this.scheduleForCreateUpdate.id = null;
    }

    if (this.schedule.locationId === 0) {
      this.scheduleForCreateUpdate.locationId = null;
    }

    if (this.schedule.id !== 0 && this.schedule.otherHost !== '') {
      this.toastService.warning(
        'Bạn chỉ được chọn một người chủ trì có sẵn hoặc điền vào người chủ trì khác!'
      );
      return;
    }

    if (this.schedule.locationId !== 0 && this.schedule.otherLocation !== '') {
      this.toastService.warning(
        'Bạn chỉ được chọn một địa điểm có sẵn hoặc điền vào địa điểm tổ chức khác!'
      );
      return;
    }

    this.scheduleForCreateUpdate.isSendSMSInvite = this.schedule.isSendSMSInvite ? this.schedule.isSendSMSInvite : isSendSMS;
    this.scheduleForCreateUpdate.sendSMSFlagForJob = isSendSMS;
    this.scheduleForCreateUpdate.messageContent = this.smsPreview;
    

    // upload file trước rồi sau đó mới thêm mới hoặc sửa
    if (this.uploader?.queue?.length > 0) {
      this.loader.start();
      this.uploader.uploadAll();
    } else {
      this.updateOrCreate();
    }
    

    //console.log(this.scheduleForCreateUpdate);
  }

  validateFilesAttachment(): boolean {
    return this.filesAttachment.some(r => !r.fileName);
  }

  private updateOrCreate(): void {
    if (this.isModify) {
      // update
      this.scheduleForCreateUpdate.organizeId = this.organizeId;
      
      const updateSub = this.scheduleService
        .updateSchedule(this.scheduleForCreateUpdate)
        .subscribe((response) => {
          if (response.isSuccess) {
            this.toastService.success('Bạn đã sửa lịch họp thành công!');

            if (this.selectedDateFromListPage || this.weekSelected) {
              const queryParams = {
                startDateOfWeek: this.selectedDateFromListPage,
                week: this.weekSelected
              };
              const urlTree = this.router.createUrlTree(['/admin/schedule'], {
                queryParams,
              });
              this.router.navigateByUrl(urlTree);
            } else {
              this.router.navigateByUrl('/admin/schedule');
            }
          } else {
            this.toastService.error('Tạo lịch họp thất bại, vui lòng thử lại!');
          }
        });

      this.subscriptions.push(updateSub);
    } else {
      // create
      this.scheduleForCreateUpdate.organizeId = this.organizeId;
      const createSub = this.scheduleService
        .createSchedule(this.scheduleForCreateUpdate)
        .subscribe((response) => {
          if (response.isSuccess) {
            this.toastService.success('Bạn đã tạo lịch họp thành công!');
            if (this.selectedDateFromListPage || this.weekSelected) {
              const queryParams = {
                startDateOfWeek: this.selectedDateFromListPage,              
                week: this.weekSelected
              };

              const urlTree = this.router.createUrlTree(['/admin/schedule'], {
                queryParams,
              });

              this.router.navigateByUrl(urlTree);
            } else {
              this.router.navigateByUrl('/admin/schedule');
            }
          } else {
            this.toastService.error('Tạo lịch họp thất bại, vui lòng thử lại!');
          }
        });

      this.subscriptions.push(createSub);
    }
  }

  onCopy(): void {
    const participantSelected = [];
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.participantIsSelected.length; i++) {
      if (this.participantIsSelected[i].participantId !== -99) {
        participantSelected.push(this.participantIsSelected[i].participantId);
      }
    }

    this.schedule.userIds = participantSelected;

    this.schedule.otherParticipants = this.otherParticipantSelected;

    this.schedule.scheduleDate = new Date(
      `${this.scheduleDate.month}/${this.scheduleDate.day + 1}/${this.scheduleDate.year
      }`
    );
    this.schedule.scheduleEndDate = new Date(
      `${this.scheduleEndDate.month}/${this.scheduleEndDate.day + 1}/${this.scheduleEndDate.year
      }`
    );
    let scheduleTimeHour = this.scheduleTime.hour.toString();
    if (this.scheduleTime.hour < 10) {
      scheduleTimeHour = `0${this.scheduleTime.hour}`;
    }

    let scheduleTimeMinute = this.scheduleTime.minute.toString();
    if (this.scheduleTime.minute < 10) {
      scheduleTimeMinute = `0${this.scheduleTime.minute}`;
    }
    this.schedule.scheduleTime = `${scheduleTimeHour}:${scheduleTimeMinute}`;

    let scheduleEndTimeHour = this.scheduleEndTime.hour.toString();
    if (this.scheduleEndTime.hour < 10) {
      scheduleEndTimeHour = `0${this.scheduleEndTime.hour}`;
    }

    let scheduleEndTimeMinute = this.scheduleEndTime.minute.toString();
    if (this.scheduleEndTime.minute < 10) {
      scheduleEndTimeMinute = `0${this.scheduleEndTime.minute}`;
    }
    this.schedule.scheduleEndTime = `${scheduleEndTimeHour}:${scheduleEndTimeMinute}`;

    if (this.schedule.scheduleTitleTemplateId === 0) {
      this.schedule.scheduleTitleTemplateId = null;
    }

    const scheduleObj = Object.assign({}, this.schedule);

    if (this.schedule.id === 0) {
      scheduleObj.id = null;
    }

    if (this.schedule.locationId === 0) {
      scheduleObj.locationId = null;
    }

    if (this.schedule.id !== 0 && this.schedule.otherHost !== '') {
      this.toastService.warning(
        'Bạn chỉ được chọn một người chủ trì có sẵn hoặc điền vào người chủ trì khác!'
      );
      return;
    }

    if (this.schedule.locationId !== 0 && this.schedule.otherLocation !== '') {
      this.toastService.warning(
        'Bạn chỉ được chọn một địa điểm có sẵn hoặc điền vào địa điểm tổ chức khác!'
      );
      return;
    }

    delete scheduleObj.scheduleId;

    scheduleObj.scheduleStatus = EScheduleStatus.Pending;

    this.scheduleService.createSchedule(scheduleObj).subscribe((response) => {
      if (response.isSuccess) {
        this.toastService.success('Bạn đã sao chép lịch họp thành công!');
        if (this.selectedDateFromListPage) {
          this.router.navigate(['/admin/schedule'], {
            relativeTo: this.activatedRoute,
            queryParams: { startDateOfWeek: this.selectedDateFromListPage },
            queryParamsHandling: 'merge',
          });
        } else {
          this.router.navigate(['/admin/schedule'], {
            relativeTo: this.activatedRoute,
            queryParamsHandling: 'merge',
          });
        }
      } else {
        this.toastService.error(
          'Sao chép lịch họp thất bại, vui lòng thử lại!'
        );
      }
    });
  }

  onChange(participantId: string): void {
    if (participantId === undefined) {
      return;
    }
    const subChange = this.participantService
      .chooseParticipant(participantId)
      .subscribe((response) => {
        if (response.isSuccess) {
          // không add item đã tồn tại trong participantIsSelected
          const participants = response.result;
          participants.forEach((item) => {
            const filter = _.filter(this.participantIsSelected, {
              participantId: item.participantId,
              receiverName: item.receiverName,
            });

            if (filter.length === 0) {
              this.participantIsSelected.push(item);
            }
          });
        }
      });

    this.subscriptions.push(subChange);

    // thành phần tham dự hiển thị
    const foundParticipant = this.listParticipant.find(
      (item) => item.id === participantId
    );
    if (foundParticipant !== undefined) {
      if (this.schedule.participantDisplay === null) {
        this.schedule.participantDisplay = '';
        this.schedule.participantDisplay = foundParticipant.shortName;
      } else {
        this.schedule.participantDisplay =
          this.schedule.participantDisplay + ', ' + foundParticipant.shortName;
      }
    }
  }

  deleteParticipant(item: IParticipantIsSelected): void {
    const index = this.participantIsSelected.indexOf(item);
    this.participantIsSelected.splice(index, 1);
  }

  addOtherParticipant(): void {
    this.otherParticipantSelected.push({
      name: null,
      email: null,
      phoneNumber: null,
    });
  }

  trackByIndex(index: number, value: any): number {
    return index;
  }

  deleteOtherParticipant(item: IOtherParticipantSelected): void {
    const index = this.otherParticipantSelected.indexOf(item);
    this.otherParticipantSelected.splice(index, 1);
  }

  private splitTime(): void {
    if (this.schedule) {
      const timeIsSplit = this.schedule.scheduleTime.split(':');
      this.scheduleTime = {
        // tslint:disable-next-line:radix
        hour: Number.parseInt(timeIsSplit[0].toString()),
        // tslint:disable-next-line:radix
        minute: Number.parseInt(timeIsSplit[1].toString()),
      };
      try{
        const timeEndIsSplit = this.schedule.scheduleEndTime.split(':');
        this.scheduleEndTime = {
          // tslint:disable-next-line:radix
          hour: Number.parseInt(timeEndIsSplit[0].toString()),
          // tslint:disable-next-line:radix
          minute: Number.parseInt(timeEndIsSplit[1].toString()),
        };
      }catch{
        this.scheduleEndTime = {
          // tslint:disable-next-line:radix
          hour: Number.parseInt(timeIsSplit[0].toString()) + 2,
          // tslint:disable-next-line:radix
          minute: Number.parseInt(timeIsSplit[1].toString()),
        };
      }
    }
  }

  private splitTime2(z: string): void {
    if (this.schedule) {
      const timeIsSplit = z.split(':');
      this.sendMailTime = {
        // tslint:disable-next-line:radix
        hour: Number.parseInt(timeIsSplit[0].toString()),
        // tslint:disable-next-line:radix
        minute: Number.parseInt(timeIsSplit[1].toString()),
      };
    }
  }

  initFormAddByTemplate(): void {
    const scheduleSub = this.scheduleService
      .getScheduleTemplateById(this.templateId)
      .subscribe((response) => {
        if (response.isSuccess) {
          const scheduleTemplate = response.result as ScheduleTemplateModel;

          this.schedule = this.loadScheduleByTemplate(
            scheduleTemplate
          ) as ScheduleModel;

          this.otherParticipantSelected =
            this.scheduleTemplate.otherParticipants;

          if (this.otherParticipantSelected.length > 0) {
            this.schedule.isOtherParticipant = true;
          }

          this.participantIsSelected = !this.schedule.participantIsSelected
            ? []
            : this.schedule.participantIsSelected;
          this.splitTime();
        }
      });

    this.subscriptions.push(scheduleSub);
  }

  loadScheduleByTemplate(scheduleTemplate: ScheduleTemplateModel): any {
    const schedule = new ScheduleModel();
    schedule.scheduleTime = scheduleTemplate.scheduleTime;
    schedule.scheduleContent = scheduleTemplate.scheduleContent;
    schedule.scheduleTitle = scheduleTemplate.scheduleTitle;
    schedule.otherLocation = scheduleTemplate.otherLocation;
    schedule.otherHost = scheduleTemplate.otherHost;
    schedule.locationId = scheduleTemplate.locationId;
    schedule.id = scheduleTemplate.id;
    schedule.isActive = scheduleTemplate.isActive;
    schedule.createdDate = new Date();
    schedule.userIds = scheduleTemplate.userIds;
    schedule.groupMeetingIds = scheduleTemplate.groupMeetingIds;
    schedule.iSendSMS = scheduleTemplate.iSendSMS;
    schedule.isSendEmail = scheduleTemplate.isSendEmail;
    schedule.participantDisplay = scheduleTemplate.participantDisplay;
    schedule.isOtherParticipant = scheduleTemplate.isOtherParticipant;
    schedule.otherParticipants = scheduleTemplate.otherParticipants;
    schedule.reasonChangeSchedule = scheduleTemplate.reasonChangeSchedule;
    schedule.scheduleTitleTemplateId = scheduleTemplate.scheduleTitleTemplateId;
    schedule.departmentPrepare = scheduleTemplate.departmentPrepare;
    schedule.isChangeLocation = scheduleTemplate.isChangeLocation;
    schedule.filePath = scheduleTemplate.filePath;
    schedule.cloudinaryPublicId = scheduleTemplate.cloudinaryPublicId;
    schedule.participantIsSelected = scheduleTemplate.participantIsSelected;
    schedule.scheduleStatus = scheduleTemplate.scheduleStatus;

    return schedule;
  }
}
