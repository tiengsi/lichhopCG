import {
  GetUserById,
  UpdateUser,
} from './../../../../../core/ngrx-store/user/user.actions';
import { getAllRoles } from './../../../../../core/ngrx-store/role/role.selectors';
import { RolesRequested } from './../../../../../core/ngrx-store/role/role.actions';
import { getUpdateUserFail, getUpdateUserSuccess, selectUserById } from './../../../../../core/ngrx-store/user/user.selectors';
import { SubheaderService, ToastService } from './../../../../../core/';
import { AppState } from './../../../../../core/ngrx-store/reducers/index';
import {
  BaseResponseModel,
  QueryParamsModel,
  RoleModel,
  UserModel,
} from './../../../../../shared/';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { select, Store } from '@ngrx/store';
import { ActivatedRoute, Router } from '@angular/router';
import { Update } from '@ngrx/entity';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { DepartmentService } from 'src/app/core/services/department.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { Location } from '@angular/common';
import { PermissionList } from 'src/app/configs/permission';

@Component({
  selector: 'app-officer-edit',
  templateUrl: './officer-edit.component.html',
})
export class OfficerEditComponent implements OnInit, OnDestroy {

  userForm: FormGroup;
  user: UserModel;
  hasFormErrors = false;
  departments: IDepartmentModel[] = [
    {
      id: 0,
      name: '--- Chọn phòng ban ---',
    },
  ];

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private store: Store<AppState>,
    private activatedRoute: ActivatedRoute,
    private departmentService: DepartmentService,
    private router: Router,
    private userFB: FormBuilder,
    private subheaderService: SubheaderService,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) { }

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
  ngOnInit(): void {
    this.loadPermissions();
    this.initBreadCrumbs();
    this.getDepartment();
    this.loadUser();

    const successSubscription = this.store
      .pipe(select(getUpdateUserSuccess))
      .subscribe((isUpdateSuccess) => {
        if (isUpdateSuccess) {
          this.toastService.showSuccess('Chỉnh sửa thành công!');
          this.cdr.markForCheck();
          this.router.navigateByUrl('/admin/user');
        }
      });
    this.subscriptions.push(successSubscription);

    const failSubscription = this.store
      .pipe(select(getUpdateUserFail))
      .subscribe((errorMessage) => {
        if (errorMessage) {
          this.toastService.showError(errorMessage);
          this.cdr.markForCheck();
        }
      });
    this.subscriptions.push(failSubscription);
  }

  getDepartment(): void {
    this.departmentService.getAllForSelect().subscribe((res: BaseResponseModel<IDepartmentModel[]>) => {
      if (res && res.isSuccess) {
        this.departments = this.departments.concat(res.result);
      }
    });
  }

  loadUser(): void {
    const routeSubscription = this.activatedRoute.params.subscribe((params) => {
      const id = params.id;
      this.store.dispatch(new GetUserById({ id }));
      this.store.pipe(select(selectUserById)).subscribe((res) => {
        if (res) {
          this.user = res;
          this.createForm();
        }
      });
    });
    this.subscriptions.push(routeSubscription);
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý cán bộ', page: `/admin/user` },
      { title: 'Sửa', page: `admin/officer/edit` },
    ]);
  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }

  /**
   * Create form
   */
  createForm(): void {
    this.userForm = this.userFB.group({
      userName: [
        { value: this.user.userName, disabled: true },
        Validators.compose([
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(20),
        ]),
      ],
      fullName: [this.user.fullName, Validators.required],
      shortName: [this.user.shortName],
      officerPosition: [this.user.officerPosition],
      email: [this.user.email, Validators.email],
      phoneNumber: [this.user.phoneNumber, Validators.required],
      roles: [this.user.roles],
      dptId: this.user.dptId,
      isHost: [this.user.isHost],
      isShow: [this.user.isShow]
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
    const updateUser: Update<UserModel> = {
      id: userModel.id,
      changes: userModel,
    };
    this.store.dispatch(
      new UpdateUser({ user: userModel, partialUser: updateUser })
    );
  }

  /**
   * Returns object for saving
   */
  prepareUser(): UserModel {
    const controls = this.userForm.controls;
    // tslint:disable-next-line: variable-name
    const _user = new UserModel();
    _user.id = this.user.id;
    _user.userName = controls.userName.value;
    _user.fullName = controls.fullName.value;
    _user.shortName = controls.shortName.value;
    _user.phoneNumber = controls.phoneNumber.value;
    _user.email = controls.email.value;
    _user.roles = controls.roles.value;
    _user.officerPosition = controls.officerPosition.value;
    _user.dptId = controls.dptId.value;
    _user.isHost = controls.isHost.value;
    _user.isShow = controls.isShow.value;
    _user.organizeId = this.user.organizeId;
    _user.password = 'Admin@111';
    return _user;
  }

  /**
   * On Destroy
   */
  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

}
