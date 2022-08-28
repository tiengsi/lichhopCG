import { Location } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PermissionList } from 'src/app/configs/permission';
import { LayoutUtilsService, SubheaderService } from 'src/app/core';
import { BrandNameService } from 'src/app/core/services/brandname.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';
import { ViettelBrandNameModel } from 'src/app/shared/models/brand-name.model';

@Component({
  selector: 'app-viettel',
  templateUrl: './viettel.component.html',
  styleUrls: ['./viettel.component.scss']
})
export class ViettelComponent implements OnInit, OnDestroy {

  viettels: ViettelBrandNameModel[];
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
    private brandnameService: BrandNameService,
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
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];        
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
  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý Viettel Brandname', page: 'brandname/viettel'},
    ]);
  }
  loadData(): void {        
    const subScription = this.brandnameService.GetViettelBrandName().subscribe((response) => {
      if (response.isSuccess) {
        this.viettels = response.result;
        console.log(this.viettels);
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

  delete(item: ViettelBrandNameModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.brandnameService.DeleteViettelBrandName(item.brandNameId).subscribe(response => {
          if (response.isSuccess) {
            this.toastService.success('Xóa địa điểm thành công!');
            this.loadData();
          } else {
            this.toastService.error('Xóa thất bại, xin vui lòng thử lại!');
          }
        });
      }
    });
  }
  ngOnInit(): void {
    this.loadPermissions();
    this.initBreadCrumbs();
    this.loadData();
  }

}
