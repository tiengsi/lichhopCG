import { SubheaderService } from './../../../../../core/services/subheader.service';
import { ToastService } from './../../../../../core/services/toast.service';
import { LayoutUtilsService } from './../../../../../core/services/layout-utils.service';
import {
  getAllUsers,
  getDeleteUserFail,
  getDeleteUserSuccess,
  getTotalUsers,
} from './../../../../../core/ngrx-store/user/user.selectors';
import {
  UserDeleted,
  UsersPageRequested,
} from './../../../../../core/ngrx-store/user/user.actions';
import { AppState } from './../../../../../core/ngrx-store/reducers';
import { UserModel, QueryParamsModel, ChangePasswordModel, AuthModel } from './../../../../../shared';
import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subscription } from 'rxjs';
import { select, Store } from '@ngrx/store';
import { filter, mergeMap, map } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';
import { ROLES } from 'src/app/shared/models/permission';
import { ToastrService } from 'ngx-toastr';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';
import { environment } from 'src/environments/environment';
import { ResetPasswordModel } from 'src/app/shared/models/reset-password.model';
import { UserService } from 'src/app/core';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit, OnDestroy {
  data$: Observable<UserModel[]>;
  data: UserModel[] = [];
  total$: Observable<number>;
  count$: Observable<number>;
  page = 1;
  pageSize = 10;
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : -1;
  @ViewChild('searchInput', { static: true }) searchInput: ElementRef;
  sortEvent: SortEvent = {
    direction: 'asc',
    column: 'UserName'
  };

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showCreateAdminBtn = true;
  showEditBtn = true;
  showDeleteBtn = true;

  resetPasswordModel: ResetPasswordModel;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private translate: TranslateService,
    private store: Store<AppState>,
    private layoutUtilsService: LayoutUtilsService,
    private userService: UserService,
    private toastService: ToastrService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private subheaderService: SubheaderService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) { }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_USER];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_USER_OFFICER_CREATE];
        this.showCreateAdminBtn = this.UIPermissions[this.permissionList.ADMIN_USER_ADMIN_CREATE];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_USER_EDIT];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_USER_DELETE];        
      }
      else {
        this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }
  ngOnInit(): void {
    this.loadPermissions();
    this.initBreadCrumbs();
    this.loadUsersList();

    const deleteSuccessSubscription = this.store
      .pipe(select(getDeleteUserSuccess))
      .subscribe((res) => {
        if (res) {
          this.loadUsersList();
          this.toastService.success('Xóa thành công!');
          this.cdr.markForCheck();
        }
      });
    this.subscriptions.push(deleteSuccessSubscription);

    const deleteFailSubscription = this.store
      .pipe(select(getDeleteUserFail))
      .subscribe((res) => {
        if (res) {
          this.toastService.error('Xóa thất bại, đã có lỗi xảy ra!');
          this.cdr.markForCheck();
        }
      });
    this.subscriptions.push(deleteFailSubscription);
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý người dùng', page: `admin/user` },
    ]);
  }

  /**
   * On Destroy
   */
  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  changeRoleName(arr: []){
    var z = [];
    arr.forEach(x => {
      if(x == 'SuperAdmin') z.push('SuperAdmin');
      else if (x == 'Normal-Admin') z.push('Thư ký');
      else if (x == 'Admin') z.push('Quản trị viên đơn vị');
      else if (x == 'Scheduler') z.push('Quản lý lịch họp');
      else if(x == 'User') z.push('Cán bộ');
    });
    return z;
  }

  /**
   * Load users list
   */
  loadUsersList(): void {
    const filter = {
      filter: this.searchInput.nativeElement.value
    };
    const queryParams = new QueryParamsModel(
      filter,
      this.sortEvent.direction,
      this.sortEvent.column,
      this.page,
      this.pageSize
    );
    this.store.dispatch(new UsersPageRequested({ query: queryParams }));

    this.data$ = this.store.pipe(select(getAllUsers));    
    this.total$ = this.store.pipe(select(getTotalUsers));
    this.count$ = this.store.pipe(select(getTotalUsers));
  }

  /** ACTIONS */
  /**
   * Delete user
   *
   * @param item: User
   */
  deleteUser(item: UserModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      console.log(`${result}`);
      if (result === 'Ok') {
        this.store.dispatch(new UserDeleted({ id: item.id }));
      }
    });
  }

  // /**
  //  * Redirect to edit page
  //  *
  //  * @param id
  //  */
  editUser(item: UserModel): void {
    this.router.navigate(['admin/officer/edit', item.id]);
    // if (item.roles.includes(ROLES.User) && item.roles.length == 1) {
    //   this.router.navigate(['admin/officer/edit', item.id]);
    // }
    // else {
    //   this.router.navigate(['./edit', item.id], { relativeTo: this.activatedRoute });
    // }
  }

  changePassword(item: UserModel): void{        
    this.resetPasswordModel = new ResetPasswordModel();
    this.resetPasswordModel.userId = item.id;
    this.resetPasswordModel.newPassword = 'Canbo1234';
    console.log(this.resetPasswordModel);
    if(confirm(`Mật khẩu của tài khoản ${item.userName} - ${item.fullName} sẽ được đổi thành Canbo1234. Chọn OK để xác nhận.`)){
      const reset = this.userService.resetPassword(this.resetPasswordModel).subscribe(res=>{
        console.log(res);
        if(res.isSuccess){
          this.toastService.success(`Reset mật khẩu tài khoản ${item.userName} thành công`)
        }else{
          this.toastService.error(`Reset mật khẩu thất bại`)
        }
      })
    }
  }

  changeRole(item: UserModel):void{
    this.router.navigate(['./edit', item.id], { relativeTo: this.activatedRoute });
  }

  onSort(event: SortEvent): void {
    if (!event || !event.column || !event.direction) { return; }

    this.sortEvent = event;
    this.loadUsersList();
  }

  addOfficer(): void {
    this.router.navigateByUrl('admin/officer/add');
  }
}
