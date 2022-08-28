import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { LoadingBarService } from '@ngx-loading-bar/core';
import * as _ from 'lodash';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PermissionList } from 'src/app/configs/permission';
import {
  SubheaderService,
  UserService,
  ParticipantService,
  ScheduleService,
  AuthService,
} from 'src/app/core';
import { DepartmentService } from 'src/app/core/services/department.service';
import { LocationService } from 'src/app/core/services/location.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { TitleTemplateService } from 'src/app/core/services/title-template.service';
import {
  IParticipant,
  UserForSelectModel,
  ITitleTemplate,
  IParticipantIsSelected,
  IOtherParticipantSelected,
  AuthModel,
  EParticipantType,
  QueryParamsModel,
  EScheduleStatus,
} from 'src/app/shared';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { EditorConfig } from 'src/app/shared/models/post.model';
import { ScheduleTemplateModel } from 'src/app/shared/models/schedule-template.model';
import { environment } from 'src/environments/environment';
import { constCollect } from 'src/app/configs/const';

@Component({
  selector: 'app-schedule-add-template',
  templateUrl: './schedule-add-template.component.html',
  styleUrls: ['./schedule-add-template.component.scss'],
})
export class ScheduleAddTemplateComponent implements OnInit, OnDestroy {
  hasFormErrors = false;
  schedule: ScheduleTemplateModel = new ScheduleTemplateModel();
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  listParticipant: IParticipant[];
  locations: ILocationModel[] = [
    {
      id: 0,
      title: '--- Chọn địa điểm ---',
    },
  ];
  scheduleDate: NgbDateStruct;
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
  minuteStep = 15;

  parentDepartments: IDepartmentModel[] = [
    {
      id: 0,
      name: '--- Chọn phòng ban ---',
    },
  ];
  seletedDepartment = 0;
  participantIsSelected: IParticipantIsSelected[] = [];
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
  uploader: FileUploader;
  scheduleForCreateUpdate = new ScheduleTemplateModel();
  loader = this.loadingBar.useRef();

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;

  // Subscriptions
  private subscriptions: Subscription[] = [];
  weekSelected: any;

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
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) {
    this.scheduleId = +this.activatedRoute.snapshot.params?.id;
    this.selectedDateFromListPage =
      this.activatedRoute.snapshot.queryParams?.startDateOfWeek;
    this.weekSelected = this.activatedRoute.snapshot.queryParams?.week || 1;
    this.isCopy = this.activatedRoute.snapshot.queryParams?.isCopy || 0; // copy lịch
    this.isCopy = parseInt(this.isCopy.toString(), 0);
    this.weekSelected = parseInt(this.weekSelected.toString(), 10);
    this.isModify = !!this.scheduleId;
    this.initBreadCrumbs();
    
  }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);  
        if(this.isModify){
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_TEMPLATE_EDIT];
        } else{
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_TEMPLATE_ADD];
        }     
        if(!this.accessable) this.location.back();        
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_TEMPLATE_EDIT];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_SCHEDULE_TEMPLATE_ADD];                       
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
    this.loadPermissions();
    this.initializeUploader();
    this.loadLocation();
    this.loadParticipant();
    this.loadUser(null);
    this.loadTitleTemplate();
    // this.smsPreview();

    if (this.isModify) {
      this.loadSchedule();
    }
  }

  initializeUploader(): void {
    const BASE_URL = environment.base_url;
    const localToken = this.authService.getToken();
    const tokenObj: AuthModel = JSON.parse(localToken);
    this.uploader = new FileUploader({
      url: `${BASE_URL}uploaders/attachment`,
      authToken: 'Bearer ' + tokenObj.accessToken,
      isHTML5: true,
      allowedFileType: ['image', 'pdf'],
      removeAfterUpload: false,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024, // 10 MB
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res = JSON.parse(response);

        this.scheduleForCreateUpdate.filePath = res.result.url;
        this.scheduleForCreateUpdate.cloudinaryPublicId = res.result.publicId;
        this.loader.complete();
        this.updateOrCreate();
      }
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
      user = '<CHU_TRI>';
    } else {
      if (this.schedule.id === 0) {
        user = this.schedule.otherHost;
      } else {
        const foundTemplate = this.users.find(
          (item) => item.id === this.schedule.id
        );
        if (foundTemplate !== undefined) {
          user = foundTemplate.positionName;
        }
      }
    }

    let location = '';
    if (
      this.schedule.locationId === 0 &&
      (this.schedule.otherLocation === null ||
        this.schedule.otherLocation === '')
    ) {
      location = '<DIA_DIEM_HOP>';
    } else {
      if (this.schedule.locationId === 0) {
        location = this.schedule.otherLocation;
      } else {
        const foundTemplate = this.locations.find(
          (item) => item.id === this.schedule.locationId
        );
        if (foundTemplate !== undefined) {
          location = foundTemplate.title;
        }
      }
    }
    return `${constCollect.prefixTitleSchedule}: ${scheduleTitleTemplate} ${scheduleTitle}, vào lúc ${hour} ngày ${scheduleDate}, do ${user} chủ trì, tại ${location}. (vui long xem email)`;
  }

  get participantType(): typeof EParticipantType {
    return EParticipantType;
  }

  loadSchedule(): void {
    const scheduleSub = this.scheduleService
      .getScheduleTemplateById(this.scheduleId)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.schedule = response.result;

          this.otherParticipantSelected = this.schedule.otherParticipants;
          if (this.otherParticipantSelected.length > 0) {
            this.schedule.isOtherParticipant = true;
          }

          this.participantIsSelected =
            this.schedule.participantIsSelected === null
              ? []
              : this.schedule.participantIsSelected;
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

  loadTitleTemplate(): void {
    const queryParams = new QueryParamsModel({}, '', '', 1, 50);

    this.titleTemplateService.getAllScheduleTitleTemplateByOrganizeId(queryParams, this.organizeId).subscribe((response) => {
      if (response.isSuccess) {
        this.titleTemplates = this.titleTemplates.concat(response.result.items);
      } else {
        // this.toastService.error(response.message);
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

  onSumbit(isApprove: boolean, isSendSMS = false): void {
    const participantSelected = [];
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.participantIsSelected.length; i++) {
      if (this.participantIsSelected[i].participantId !== -99) {
        participantSelected.push(this.participantIsSelected[i].participantId);
      }
    }

    this.schedule.userIds = participantSelected;

    this.schedule.otherParticipants = this.otherParticipantSelected;

    let scheduleTimeHour = this.scheduleTime.hour.toString();
    if (this.scheduleTime.hour < 10) {
      scheduleTimeHour = `0${this.scheduleTime.hour}`;
    }

    let scheduleTimeMinute = this.scheduleTime.minute.toString();
    if (this.scheduleTime.minute < 10) {
      scheduleTimeMinute = `0${this.scheduleTime.minute}`;
    }
    this.schedule.scheduleTime = `${scheduleTimeHour}:${scheduleTimeMinute}`;

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

    // upload file trước rồi sau đó mới thêm mới hoặc sửa
    if (this.uploader?.queue?.length > 0) {
      this.loader.start();
      this.uploader.uploadAll();
    } else {
      this.updateOrCreate();
    }
  }

  private updateOrCreate(): void {
    if (this.isModify) {
      // update
      const updateSub = this.scheduleService
        .updateScheduleTemplate(this.scheduleForCreateUpdate)
        .subscribe((response) => {
          if (response.isSuccess) {
            this.toastService.success('Bạn đã sửa lịch họp thành công!');
            if (this.selectedDateFromListPage) {
              this.router.navigate(['/admin/schedule-template'], {
                relativeTo: this.activatedRoute,
                queryParams: {
                  startDateOfWeek: this.selectedDateFromListPage,
                  week: this.weekSelected
                },
                queryParamsHandling: 'merge',
              });
            } else {
              this.router.navigate(['/admin/schedule-template'], {
                relativeTo: this.activatedRoute,
                queryParamsHandling: 'merge',
              });
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
        .createScheduleTemplate(this.scheduleForCreateUpdate)
        .subscribe((response) => {
          if (response.isSuccess) {
            this.toastService.success('Bạn đã tạo lịch họp thành công!');
            if (this.selectedDateFromListPage) {
              this.router.navigate(['/admin/schedule-template'], {
                relativeTo: this.activatedRoute,
                queryParams: {
                  startDateOfWeek: this.selectedDateFromListPage,
                },
                queryParamsHandling: 'merge',
              });
            } else {
              this.router.navigate(['/admin/schedule-template'], {
                relativeTo: this.activatedRoute,
                queryParamsHandling: 'merge',
              });
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

    let scheduleTimeHour = this.scheduleTime.hour.toString();
    if (this.scheduleTime.hour < 10) {
      scheduleTimeHour = `0${this.scheduleTime.hour}`;
    }

    let scheduleTimeMinute = this.scheduleTime.minute.toString();
    if (this.scheduleTime.minute < 10) {
      scheduleTimeMinute = `0${this.scheduleTime.minute}`;
    }
    this.schedule.scheduleTime = `${scheduleTimeHour}:${scheduleTimeMinute}`;

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

    this.scheduleService
      .createScheduleTemplate(scheduleObj)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastService.success('Bạn đã sao chép lịch họp thành công!');
          if (this.selectedDateFromListPage) {
            this.router.navigate(['/admin/schedule-template'], {
              relativeTo: this.activatedRoute,
              queryParams: { startDateOfWeek: this.selectedDateFromListPage },
              queryParamsHandling: 'merge',
            });
          } else {
            this.router.navigate(['/admin/schedule-template'], {
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
    }
  }
}
