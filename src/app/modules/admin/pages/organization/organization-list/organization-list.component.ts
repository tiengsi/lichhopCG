import { Location } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LayoutUtilsService } from 'src/app/core/services/layout-utils.service';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { SubheaderService } from 'src/app/core/services/subheader.service';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';
import { Organization, OrganizationTree } from 'src/app/shared/models/organization.model';
import { PermissionList } from 'src/app/configs/permission';

@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.scss']
})
export class OrganizationListComponent implements OnInit, OnDestroy {

  @ViewChild('searchInput', { static: true }) searchInput: ElementRef;
  private subscriptions: Subscription[] = [];
  organizationTree: OrganizationTree[];
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

  constructor(
    private subheaderService: SubheaderService,
    private organizationService: OrganizationService,
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
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_ORGANIZATION];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_ORGANIZATION_CREATE];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_ORGANIZATION_EDIT];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_ORGANIZATION_DELETE];        
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
    this.loadPermissions();
    this.initBreadCrumbs();
    this.loadData();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý đơn vị', page: `admin/orgaization` },
    ]);
  }

  loadData(): void {
    // const filter = {
    //   filter: this.searchInput.nativeElement.value
    // }
    const subScription = this.organizationService.GetAllAsTree().subscribe((response) => {
      if (response.isSuccess) {
        this.organizationTree = response.result as OrganizationTree[];
        console.log(this.organizationTree)        ;
      }
      else {
        this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }

  edit(id: number){
    this.router.navigate(['./edit', id], { relativeTo: this.activatedRoute });
  }
  delete(item: Organization){
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.organizationService.DeleteOrganization(item.organizeId).subscribe(response => {
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
  onSort(event: SortEvent): void {
    if (!event || !event.column || !event.direction) { return; }

    this.sortEvent = event;
    this.loadData();
  }

  showRows(i: number, j: number): void{    
    console.log(i + ' - ' + j);
    if(j<0){
      var firstTierChildren = document.querySelectorAll('.child-of-'+i);    
      var clicky = document.getElementById(`parent-${i}`).children[1];      
      firstTierChildren.forEach((e) => {
        if(e.classList.length > 1){   
          (<HTMLTableRowElement>e).style.display = 'none';       
        }
        else{
          if((<HTMLTableRowElement>e).style.display == 'none'){
            (<HTMLTableCellElement>clicky).style.fontWeight = '600';
            (<HTMLTableRowElement>e).style.display = 'table-row';
            (<HTMLTableCellElement>(<HTMLTableRowElement>e).children[1]).style.fontWeight = 'normal';             
          } 
          else 
          {
            (<HTMLTableRowElement>e).style.display = 'none';
            (<HTMLTableCellElement>clicky).style.fontWeight = 'normal';        
          }
        }
      });  
    }
    else{
      var secondTierChildren = document.querySelectorAll(`.child-of-${i}-${j}`);
      var clicky = document.getElementById(`parent-${i}`).children[1];
      var clicky2 = document.getElementById(`parent-${i}-${j}`).children[1];
      secondTierChildren.forEach((e) => {
        if((<HTMLTableRowElement>e).style.display == 'none'){
          (<HTMLTableRowElement>e).style.display = 'table-row';
          (<HTMLTableCellElement>clicky).style.fontWeight = '900';
          (<HTMLTableCellElement>clicky2).style.fontWeight = '600';
        }else{
          // (<HTMLTableCellElement>clicky).style.fontWeight = 'normal';
          (<HTMLTableCellElement>clicky2).style.fontWeight = 'normal';
          (<HTMLTableRowElement>e).style.display = 'none';
        }
      });
    }
  }
}
