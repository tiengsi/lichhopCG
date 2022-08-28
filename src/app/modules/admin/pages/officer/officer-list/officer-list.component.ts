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
import { UserModel, QueryParamsModel } from './../../../../../shared';
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
import { ActivatedRoute, Router } from '@angular/router';
import { IUserForListModel } from 'src/app/shared/models/user-for-list.model';

@Component({
  selector: 'app-officer-list',
  templateUrl: './officer-list.component.html',
})
export class OfficerListComponent implements OnInit, OnDestroy {

  data$: Observable<IUserForListModel[]>;
  total$: Observable<number>;
  count$: Observable<number>;
  page = 1;
  pageSize = 10;
  @ViewChild('searchInput', { static: true }) searchInput: ElementRef;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private translate: TranslateService,
    private store: Store<AppState>,
    private layoutUtilsService: LayoutUtilsService,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private subheaderService: SubheaderService,
  ) { }

  ngOnInit(): void {
    this.loadUsersList();

    const deleteSuccessSubscription = this.store
      .pipe(select(getDeleteUserSuccess))
      .subscribe((res) => {
        if (res) {
          this.loadUsersList();
          this.toastService.showSuccess('Xóa thành công!');
          this.cdr.markForCheck();
        }
      });
    this.subscriptions.push(deleteSuccessSubscription);

    const deleteFailSubscription = this.store
      .pipe(select(getDeleteUserFail))
      .subscribe((res) => {
        if (res) {
          this.toastService.showError('Xóa thất bại, đã có lỗi xảy ra!');
          this.cdr.markForCheck();
        }
      });
    this.subscriptions.push(deleteFailSubscription);
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý cán bộ', page: `admin/officer` },
    ]);
  }


  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }


  loadUsersList(): void {
    const filter = {
      filter: this.searchInput.nativeElement.value,
      isOfficer: true
    };
    const queryParams = new QueryParamsModel(
      filter,
      'Asc',
      'UserName',
      this.page,
      this.pageSize
    );
    this.store.dispatch(new UsersPageRequested({ query: queryParams }));

    this.data$ = this.store.pipe(select(getAllUsers));
    this.total$ = this.store.pipe(select(getTotalUsers));
    this.count$ = this.store.pipe(select(getTotalUsers));
  }


  deleteUser(item: UserModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      console.log(`${result}`);
      if (result === 'Ok') {
        this.store.dispatch(new UserDeleted({ id: item.id }));
      }
    });
  }

  editUser(id): void {
    this.router.navigate(['./edit', id], { relativeTo: this.activatedRoute });
  }

}
