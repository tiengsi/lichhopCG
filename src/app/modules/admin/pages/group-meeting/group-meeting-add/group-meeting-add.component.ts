import { UserService } from './../../../../../core/services/user.service';
import { GroupMeetingService } from './../../../../../core/services/group-meeting.service';
import { GroupParticipantForCreateModel, IOtherParticipantSelected, UserForSelectModel } from './../../../../../shared/';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { SubheaderService, ToastService } from 'src/app/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { DepartmentService } from 'src/app/core/services/department.service';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { ToastrService } from 'ngx-toastr';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { Location } from '@angular/common';
import { PermissionList } from 'src/app/configs/permission';

@Component({
  selector: 'app-group-meeting-add',
  templateUrl: './group-meeting-add.component.html',
})
export class GroupMeetingAddComponent implements OnInit, OnDestroy {
  form: FormGroup;
  groupMeeting: GroupParticipantForCreateModel;
  deparments: IDepartmentModel[] = [];
  groupMeetingId?: number = null;
  isModify = false;
  users: UserForSelectModel[] = [];
  otherParticipantSelected: IOtherParticipantSelected[] = [
    {
      name: null,
      email: null,
      phoneNumber: null,
    },
  ];
  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;

  private subscriptions: Subscription[] = [];

  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;

  constructor(
    private subheaderService: SubheaderService,
    private groupMeetingService: GroupMeetingService,
    private toastService: ToastrService,
    private departmentService: DepartmentService,
    private userService: UserService,
    private postFB: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    protected sanitizer: DomSanitizer,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) { }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);  
        if(this.isModify){
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_GROUP_EDIT];
        } else{
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_GROUP_CREATE];
        }     
        if(!this.accessable) this.location.back();        
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_GROUP_EDIT];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_GROUP_CREATE];                       
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
    this.groupMeetingId = this.activatedRoute.snapshot.params?.id;
    this.isModify = !!this.groupMeetingId;
    this.loadPermissions();
    this.initBreadCrumbs();
    this.createForm();
    this.loadGroupMeeting();
    this.loadDepartment();
    this.loadUser();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý nhóm được mời họp', page: `admin/group-meeting` },
      { title: 'Thêm mới', page: `admin/location/add` },
    ]);
  }

  loadUser(): void {
    const loadSub = this.userService.getUserForSelect(-1, null).subscribe((response) => {
      if (response.isSuccess) {
        this.users = this.users.concat(response.result);
      }
    });

    this.subscriptions.push(loadSub);
  }

  createForm(): void {
    this.groupMeeting = new GroupParticipantForCreateModel();
    this.form = this.postFB.group({
      name: [null, Validators.compose([Validators.required])],
      isActive: [true],
      departmentIds: [this.groupMeeting.departmentIds],
      userIds: [this.groupMeeting.userIds]
    });
  }

  loadDepartment(): void {
    const loadSub = this.departmentService.getAllForSelect().subscribe((response) => {
      if (response.isSuccess) {
        this.deparments = this.deparments.concat(response.result);
      } else {
        this.toastService.error(
          'Lấy danh sách cán bộ thất bại, vui lòng thử lại!'
        );
      }
    });

    this.subscriptions.push(loadSub);
  }

  loadGroupMeeting(): void {
    if (!this.groupMeetingId) { return; }

    this.groupMeetingService.getById(this.groupMeetingId).subscribe((response) => {
      if (response.isSuccess) {
        this.groupMeeting = response.result;

        if (this.groupMeeting.otherParticipants !== null) {
          this.otherParticipantSelected = this.groupMeeting.otherParticipants;
        }

        this.pathValue();
      } else {
        this.toastService.error(
          'Lấy tin thất bại, xin vui lòng thử lại!'
        );
      }
    });
  }

  pathValue(): void {
    Object.keys(this.groupMeeting).forEach((key: string) => {
      if (this.form.contains(key)) {
        const formControl = this.form.get(key);
        if (this.groupMeeting[key] && formControl) {
          formControl.patchValue(this.groupMeeting[key]);
        }
      }
    });
  }

  isControlHasError(controlName: string, validationType: string): boolean {
    const control = this.form.controls[controlName];
    if (!control) {
      return false;
    }

    const result =
      control.hasError(validationType) && (control.dirty || control.touched);
    return result;
  }

  onCreate(): void {
    // tslint:disable-next-line:prefer-for-of
    for (let index = 0; index < this.otherParticipantSelected.length; index++) {
      const item = this.otherParticipantSelected[index];
      if (item.name === null || item.name === '') {
        const position = this.otherParticipantSelected.indexOf(item);
        this.otherParticipantSelected.splice(position, 1);
        // this.toastService.warning('Tên cán bộ ngoài hệ thống không được bỏ trống!');
        // return;
      }
    }

    const payLoad = this.prepareData();
    const add = this.groupMeetingService.create(payLoad).subscribe((res) => {
      if (res.isSuccess) {
        this.toastService.success('Thêm mới thành công!');
        this.router.navigateByUrl('/admin/group-meeting');
      }
      else {
        this.toastService.error(
          res.message
        );
      }
    });

    this.subscriptions.push(add);
  }

  onSave(): void {
    // tslint:disable-next-line:prefer-for-of
    for (let index = 0; index < this.otherParticipantSelected.length; index++) {
      const item = this.otherParticipantSelected[index];
      if (item.name === null || item.name === '') {
        this.toastService.warning('Tên cán bộ ngoài hệ thống không được bỏ trống!');
        return;
      }
    }

    const payLoad = this.prepareData();
    payLoad.id = this.groupMeetingId;
    const edit = this.groupMeetingService.update(payLoad)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastService.success('Bạn đã cập nhật tin thành công!');
          this.router.navigateByUrl('/admin/group-meeting');
        }
        else {
          this.toastService.error(
            'Đã xảy ra lỗi trong quá trình lưu địa điểm, xin vui lòng thử lại!'
          );
        }
      });

    this.subscriptions.push(edit);
  }

  prepareData(): GroupParticipantForCreateModel {
    const controls = this.form.controls;
    const location: GroupParticipantForCreateModel = {
      name: controls.name.value,
      departmentIds: controls.departmentIds.value == null ? [] : controls.departmentIds.value,
      userIds: controls.userIds.value == null ? [] : controls.userIds.value,
      otherParticipants: this.otherParticipantSelected,
      organizeId: this.organizeId
    };

    return location;
  }

  addOtherParticipant(): void {
    this.otherParticipantSelected.push({
      name: null,
      email: null,
      phoneNumber: null,
    });
  }

  deleteOtherParticipant(item: IOtherParticipantSelected): void {
    const index = this.otherParticipantSelected.indexOf(item);
    this.otherParticipantSelected.splice(index, 1);
  }

  trackByIndex(index: number, value: any): number {
    return index;
  }
}
