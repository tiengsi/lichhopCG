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
import { ToastrService } from 'ngx-toastr';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss'],
})
export class UserEditComponent implements OnInit, OnDestroy {
  userForm: FormGroup;
  user: UserModel;
  hasFormErrors = false;
  roles$: Observable<RoleModel[]>;
  rolesToText = [
      {value: 'User', text: 'Cán bộ'},
      {value: 'Normal-Admin', text: 'Thư ký'},
      {value: 'Scheduler', text: 'Quản lý lịch họp'},
      {value: 'Admin', text: 'Quản trị viên đơn vị'},      
    ];
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;

  UIPermissions: any = {};
  accessable = true;
  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private store: Store<AppState>,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private userFB: FormBuilder,
    private subheaderService: SubheaderService,
    private toastService: ToastrService,
    private cdr: ChangeDetectorRef,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) {}

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);  
        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_USER_ADMIN_CREATE];
       
        if(!this.accessable) this.location.back();                
      }
      else {
        this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }
  ngOnInit() {
    this.initBreadCrumbs();
    this.loadUser();

    const successSubscription = this.store
      .pipe(select(getUpdateUserSuccess))
      .subscribe((isUpdateSuccess) => {
        if (isUpdateSuccess) {
          this.toastService.success('Chỉnh sửa thành công!');
          this.cdr.markForCheck();
          setTimeout(() => {
            this.router.navigateByUrl('/admin/user');
          }, 1000);
        }
      });
    this.subscriptions.push(successSubscription);

    const failSubscription = this.store
      .pipe(select(getUpdateUserFail))
      .subscribe((errorMessage) => {
        if (errorMessage) {
          this.toastService.error(errorMessage);
          this.cdr.markForCheck();
        }
      });
    this.subscriptions.push(failSubscription);
  }

  loadUser(): void {
    const routeSubscription = this.activatedRoute.params.subscribe((params) => {
      const id = params.id;
      this.store.dispatch(new GetUserById({ id }));
      this.store.pipe(select(selectUserById)).subscribe((res) => {
        if (res) {
          this.user = res;
          this.createForm();
          this.loadRoles();
        }
      });
    });
    this.subscriptions.push(routeSubscription);
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý người dùng', page: `admin/user` },
      { title: 'Sửa', page: `admin/user/edit` },
    ]);
  }

  loadRoles(): void {
    const queryParams = new QueryParamsModel(null, 'Asc', 'Name', 1, 10);
    this.store.dispatch(new RolesRequested({ query: queryParams }));
    this.roles$ = this.store.pipe(select(getAllRoles));
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
          Validators.maxLength(20), // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
        ]),
      ],
      fullName: [this.user.fullName, Validators.required],
      email: [this.user.email, Validators.email],
      roles: [this.user.roles],
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
    _user.organizeId = this.user.organizeId;
    _user.userName = controls.userName.value;
    _user.fullName = controls.fullName.value;
    // _user.password = controls.password.value;
    _user.email = controls.email.value;
    _user.roles = controls.roles.value;
    _user.phoneNumber = '0123456789';
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
