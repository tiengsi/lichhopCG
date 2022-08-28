import { GroupMeetingService } from './../../../../../core/services/group-meeting.service';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { LayoutUtilsService, SubheaderService, ToastService } from 'src/app/core';
import { QueryParamsModel, GroupParticipantForListModel } from 'src/app/shared';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-group-meeting-list',
  templateUrl: './group-meeting-list.component.html',
})
export class GroupMeetingListComponent implements OnInit, OnDestroy {
  groupMeetings: GroupParticipantForListModel[];
  totalCount = 0;
  count = 0;
  page = 1;
  pageSize = 10;
  sortEvent: SortEvent = {
    direction: 'desc',
    column: 'Name'
  };
  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  showDeleteBtn = true;
  @ViewChild('searchInput', { static: true }) searchInput: ElementRef;
  private subscriptions: Subscription[] = [];

  constructor(
    private subheaderService: SubheaderService,
    private groupMeetingService: GroupMeetingService,
    private toastService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private layoutUtilsService: LayoutUtilsService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) { }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_GROUP];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_GROUP_CREATE];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_GROUP_EDIT];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_GROUP_DELETE];        
      }
      else {
        this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }
  toggleList(i: number, y: number){
    var z: any;
    if(y == 1){
      z = document.getElementById(`show-${i}`);    
    }else if(y==2){
      z = document.getElementById(`show2-${i}`);   
    }else if(y==3){
      z = document.getElementById(`show3-${i}`);   
    }
    var show = <HTMLAnchorElement>z;
    show.innerHTML = show.innerHTML == 'Hiển thị' ? 'Ẩn' : 'Hiển thị';
    var toShow: HTMLDivElement;
    if(y == 1){
      toShow = <HTMLDivElement>(document.getElementById(`to-show-${i}`))
    }else if(y == 2){
      toShow = <HTMLDivElement>(document.getElementById(`to-show2-${i}`))
    }else if(y == 3){
      toShow = <HTMLDivElement>(document.getElementById(`to-show3-${i}`))
    }
    if (toShow.style.display === "none") {
      toShow.style.display = "block";
    } else {
      toShow.style.display = "none";
    }
  }
  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadPermissions()
    this.initBreadCrumbs();
    this.loadGroupMeeting();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý nhóm được mời họp', page: `admin/group-meeting` },
    ]);
  }

  loadGroupMeeting(): void {
    const filter = {
      name: this.searchInput.nativeElement.value,
    };
    const queryParams = new QueryParamsModel(
      filter,
      this.sortEvent.direction,
      this.sortEvent.column,
      this.page,
      this.pageSize
    );

    const subScription = this.groupMeetingService.getAll(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        this.groupMeetings = response.result.items;
        this.totalCount = response.result.totalCount;
        this.count = response.result.count;
      } else {
        this.toastService.error(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }

  edit(id: number): void {
    this.router.navigate(['./edit', id], { relativeTo: this.activatedRoute });
  }

  delete(item: GroupParticipantForListModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.groupMeetingService.delete(item.groupParticipantId).subscribe(response => {
          if (response.isSuccess) {
            this.toastService.success('Xóa thành công!');
            this.loadGroupMeeting();
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
    this.loadGroupMeeting();
  }
}
