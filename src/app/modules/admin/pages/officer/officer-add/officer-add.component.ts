import {
  getAddUserFail,
  getAddUserSuccess,
} from './../../../../../core/ngrx-store/user/user.selectors';
import { UserAdd } from './../../../../../core/ngrx-store/user/user.actions';
import { getAllRoles } from './../../../../../core/ngrx-store/role/role.selectors';
import { RolesRequested } from './../../../../../core/ngrx-store/role/role.actions';
import {
  BaseResponseModel,
  QueryParamsModel,
  RoleModel,
} from './../../../../../shared/';
import { SubheaderService, ToastService } from './../../../../../core/';
import { AppState } from './../../../../../core/ngrx-store/reducers';
import { UserModel } from './../../../../../shared/models/user.model';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { select, Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { DepartmentService } from 'src/app/core/services/department.service';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { Location } from '@angular/common';
import { PermissionList } from 'src/app/configs/permission';

@Component({
  selector: 'app-officer-add',
  templateUrl: './officer-add.component.html',
})
export class OfficerAddComponent implements OnInit, OnDestroy {
  userForm: FormGroup;
  user: UserModel;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : -1;
  departments: IDepartmentModel[] = [
    {
      id: 0,
      name: '--- Chọn phòng ban ---',
    },
  ];
  hasFormErrors = false;

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  
  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private store: Store<AppState>,
    private router: Router,
    private userFB: FormBuilder,
    private departmentService: DepartmentService,
    private subheaderService: SubheaderService,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) {
    this.createForm();
  }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);  
     
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_USER_OFFICER_CREATE];        
        if(!this.accessable) this.location.back();        
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_USER_EDIT];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_USER_OFFICER_CREATE];                       
      }
      else {
        //this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }

  nospaces(event){
    var key = event.keyCode;    
    return (key !== 32);
  }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }
  ngOnInit(): void {
    this.loadPermissions();
    this.initBreadCrumbs();
    this.getDepartment();

    const addSuccessSubscription = this.store
      .pipe(select(getAddUserSuccess))
      .subscribe((isAddSuccess) => {
        if (isAddSuccess) {
          this.toastService.showSuccess('Thêm mới thành công!');
          this.cdr.markForCheck();
          this.router.navigateByUrl('/admin/user');
        }
      });
    this.subscriptions.push(addSuccessSubscription);

    const addFailSubscription = this.store
      .pipe(select(getAddUserFail))
      .subscribe((isAddFail) => {
        if (isAddFail) {
          this.toastService.showError('Thêm mới thất bại, đã có lỗi xảy ra!');
          this.cdr.markForCheck();
        }
      });
    this.subscriptions.push(addFailSubscription);
  }

  getDepartment(): void {
    this.departmentService
      .getAllForSelect()
      .subscribe((res: BaseResponseModel<IDepartmentModel[]>) => {
        if (res && res.isSuccess) {
          this.departments = this.departments.concat(res.result);
        }
      });
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý cán bộ', page: `/admin/user` },
      { title: 'Thêm mới', page: `admin/officer/add` },
    ]);
  }

  /**
   * Create form
   */
  createForm(): void {
    this.user = new UserModel();
    this.user.roles = ['User'];
    this.user.password = 'Canbo1234';
    this.userForm = this.userFB.group({
      userName: [
        this.user.userName,
        Validators.compose([
          Validators.required,
          // Validators.minLength(6),
          Validators.maxLength(20), // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
        ]),
      ],
      fullName: [this.user.fullName, Validators.required],
      shortName: [this.user.shortName],
      officerPosition: [this.user.officerPosition],
      email: [this.user.email, Validators.email],
      phoneNumber: [this.user.phoneNumber, Validators.required],
      password: [
        this.user.password,
        Validators.compose([
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(20), // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
          Validators.pattern(
            /^(?=\D*\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z]).{8,30}$/
          ),
        ]),
      ],
      roles: [this.user.roles],
      dptId: [0],
      isHost: [false],
      isShow: [true]
    });
  }

  /**
   * Checking control validation
   *
   * @param controlName: string => Equals to formControlName
   * @param validationType: string => Equals to valitors name
   */
  isControlHasError(controlName: string, validationType: string): boolean {
    const control = this.userForm.controls[controlName];
    if (!control) {
      return false;
    }

    const result =
      control.hasError(validationType) && (control.dirty || control.touched);
    return result;
  }

  /**
   * On Destroy
   */
  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  /**
   * Save data
   */
  onSumbit(): void {
    this.hasFormErrors = false;
    const controls = this.userForm.controls;
    /** check form */
    if (this.userForm.invalid) {
      Object.keys(controls).forEach((controlName) =>
        controls[controlName].markAsTouched()
      );

      this.hasFormErrors = true;
      return;
    }

    // tslint:disable-next-line:prefer-const
    let userModel = this.prepareUser();

    this.store.dispatch(new UserAdd({ user: userModel }));
  }

  /**
   * Returns object for saving
   */
  prepareUser(): UserModel {
    const controls = this.userForm.controls;
    // tslint:disable-next-line: variable-name
    const _user = new UserModel();
    _user.userName = controls.userName.value;
    _user.fullName = controls.fullName.value;
    _user.shortName = controls.shortName.value;
    _user.password = controls.password.value;
    _user.phoneNumber = controls.phoneNumber.value;
    _user.email = controls.email.value;
    _user.officerPosition = controls.officerPosition.value;
    _user.roles = controls.roles.value;
    _user.dptId = controls.dptId.value;
    _user.isHost = controls.isHost.value;
    // _user.isShow = controls.isShow.value;
    _user.isShow = true;
    _user.organizeId = this.organizeId != -1 ? this.organizeId : null;
    return _user;
  }
}
