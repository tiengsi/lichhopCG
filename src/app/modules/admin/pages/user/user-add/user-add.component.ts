import { getAddUserFail, getAddUserSuccess } from './../../../../../core/ngrx-store/user/user.selectors';
import { UserAdd } from './../../../../../core/ngrx-store/user/user.actions';
import { getAllRoles } from './../../../../../core/ngrx-store/role/role.selectors';
import { RolesRequested } from './../../../../../core/ngrx-store/role/role.actions';
import { QueryParamsModel, RoleModel } from './../../../../../shared/';
import { SubheaderService, ToastService } from './../../../../../core/';
import { AppState } from './../../../../../core/ngrx-store/reducers';
import { UserModel } from './../../../../../shared/models/user.model';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { select, Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { OrganizationTree } from 'src/app/shared/models/organization.model';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-user-add',
  templateUrl: './user-add.component.html',
  styleUrls: ['./user-add.component.scss'],
})
export class UserAddComponent implements OnInit, OnDestroy {
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
  
  isSuperAdmin = this.userInfo.roles.includes('SuperAdmin');
  organizationList: OrganizationTree[];
  selectList: any = [{id: 0, name: '- -'}];
  selectedOrgan = 0;
  //selectList: any = [];

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
    private subheaderService: SubheaderService,
    private toastService: ToastrService,
    private cdr: ChangeDetectorRef,
    private organizationService: OrganizationService,
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
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_USER_EDIT];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_USER_DELETE];                       
      }
      else {
        this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }
  showSelected(event){
    this.selectedOrgan = event;
  }

  nospaces(event){
    var key = event.keyCode;    
    return (key !== 32);
  }

  ngOnInit(): void {
    this.loadPermissions();
    this.initBreadCrumbs();    
    this.createForm();
    this.loadRoles();
    this.loadOrganizationList();

    const addSuccessSubscription = this.store
      .pipe(select(getAddUserSuccess))
      .subscribe((isAddSuccess) => {
        if (isAddSuccess) {
          this.toastService.success('Thêm mới thành công!');
          this.cdr.markForCheck();
          setTimeout(() => {
            this.router.navigateByUrl('/admin/user');
          }, 1000);
        }
      });
    this.subscriptions.push(addSuccessSubscription);

    const addFailSubscription = this.store
      .pipe(select(getAddUserFail))
      .subscribe((isAddFail) => {
        if (isAddFail) {
          this.toastService.error('Thêm mới thất bại, đã có lỗi xảy ra!');
          this.cdr.markForCheck();
        }
      });
    this.subscriptions.push(addFailSubscription);
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý người dùng', page: `admin/user` },
      { title: 'Thêm mới', page: `admin/user/add` },
    ]);
  }

  loadRoles(): void {
    const queryParams = new QueryParamsModel(null, 'Asc', 'Name', 1, 10);
    this.store.dispatch(new RolesRequested({ query: queryParams }));
    this.roles$ = this.store.pipe(select(getAllRoles));
    

    
  }

  loadOrganizationList(){
    const dashdash = '- -  ';
    this.organizationService.GetAllAsTree().subscribe((res) => {
      if(res.isSuccess){
        this.organizationList = res.result as OrganizationTree[];
        this.organizationList.forEach((e) => {
          this.selectList.push({
            id: e.organizeId,
            name: e.name
          });
          if(e.subOrganizeList != null){
            e.subOrganizeList.forEach((e2) => {
              this.selectList.push({
                id: e2.organizeId,
                name: dashdash + e2.name
              });
              if(e2.subOrganizeList!= null){
                e2.subOrganizeList.forEach((e3) => {
                  this.selectList.push({
                    id: e3.organizeId,
                    name: dashdash + dashdash + e3.name
                  });                
                });
              }
            });
          }
        });
      } else{
        this.toastService.error(
          'Lấy thông tin thất bại, xin vui lòng thử lại!'
        );
      }
    })
  }

  /**
   * Create form
   */
  createForm(): void {
    this.user = new UserModel();
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
      //shortName: [this.user.shortName],
      //officerPosition: [this.user.officerPosition],
      email: [this.user.email, Validators.email],
      //phoneNumber: [this.user.phoneNumber, Validators.required],      
      password: [
        this.user.password,
        Validators.compose([
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(20), // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
          Validators.pattern(/^(?=\D*\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z]).{8,30}$/),
        ]),
      ],
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
    _user.password = controls.password.value;
    _user.email = controls.email.value;
    _user.roles = controls.roles.value;
    _user.organizeId = this.organizeId != 0 ? this.organizeId : null;

    _user.phoneNumber = '0000000000';
    // _user.shortName = controls.shortName.value;
    // _user.officerPosition = controls.officerPosition.value;

    if(this.selectedOrgan != 0) _user.organizeId = this.selectedOrgan;
    return _user;
  }
}
