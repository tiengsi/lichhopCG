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
import { VNPTBrandNameModel } from 'src/app/shared/models/brand-name.model';

@Component({
  selector: 'app-vnpt',
  templateUrl: './vnpt.component.html',
  styleUrls: ['./vnpt.component.scss']
})

export class VnptComponent implements OnInit, OnDestroy {

  VNPTs: VNPTBrandNameModel[];
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
      { title: 'Qua??n ly?? VNPT Brandname', page: 'brandname/VNPT'},
    ]);
  }
  loadData(): void {        
    const subScription = this.brandnameService.GetVNPTBrandName().subscribe((response) => {
      if (response.isSuccess) {
        this.VNPTs = response.result;
        console.log(this.VNPTs);
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

  delete(item: VNPTBrandNameModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.brandnameService.DeleteVNPTBrandName(item.brandNameId).subscribe(response => {
          if (response.isSuccess) {
            this.toastService.success('X??a ?????a ??i???m th??nh c??ng!');
            this.loadData();
          } else {
            this.toastService.error('X??a th???t b???i, xin vui l??ng th??? l???i!');
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
