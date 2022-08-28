import {
  QueryParamsModel,
} from './../../../../../shared/';
import { SubheaderService, LayoutUtilsService } from './../../../../../core';
import {
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  OnDestroy,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { LocationService } from 'src/app/core/services/location.service';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';
import { ToastrService } from 'ngx-toastr';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-location-list',
  templateUrl: './location-list.component.html',
})
export class LocationListComponent implements OnInit, OnDestroy {
  locations: ILocationModel[];
  totalCount = 0;
  count = 0;
  page = 1;
  pageSize = 10;
  sortEvent: SortEvent = {
    direction: 'asc',
    column: 'title'
  };
  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  showDeleteBtn = true;
  @ViewChild('searchInput', { static: true }) searchInput: ElementRef;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private subheaderService: SubheaderService,
    private locationService: LocationService,
    private toastService: ToastrService,
    private layoutUtilsService: LayoutUtilsService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) { }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_LOCATION];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_LOCATION_ADD];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_LOCATION_EDIT];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_LOCATION_DELETE];        
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
    this.loadPermissions()
    this.initBreadCrumbs();
    this.loadLocation();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý địa điểm', page: `admin/location` },
    ]);
  }

  loadLocation(): void {
    const filter = {
      filter: this.searchInput.nativeElement.value,
    };
    const queryParams = new QueryParamsModel(
      filter,
      this.sortEvent.direction,
      this.sortEvent.column,
      this.page,
      this.pageSize
    );

    const subScription = this.locationService.getAll(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        this.locations = response.result.items;
        this.totalCount = response.result.totalCount;
        this.count = response.result.count;
      }
      else {
        this.toastService.error(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }

  edit(id: number): void {
    this.router.navigate(['./edit', id], { relativeTo: this.activatedRoute });
  }

  delete(item: ILocationModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.locationService.delete(item.id).subscribe(response => {
          if (response.isSuccess) {
            this.toastService.success('Xóa địa điểm thành công!');
            this.loadLocation();
          } else {
            this.toastService.error('Xóa thất bại, xin vui lòng thử lại!');
          }
        });
      }
    });
  }

  onSort(event: SortEvent): void {
    if (!event || !event.column || !event.direction) { return; }

    this.sortEvent = event;
    this.loadLocation();
  }
}
