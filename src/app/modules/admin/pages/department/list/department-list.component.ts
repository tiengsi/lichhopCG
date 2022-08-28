import {
  SubheaderService,
  ToastService,
  LayoutUtilsService,
} from './../../../../../core';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DepartmentService } from 'src/app/core/services/department.service';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { SortEvent } from 'src/app/shared/directives/sortable-header.directive';
import { QueryParamsModel } from 'src/app/shared';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RepresentativeModalComponent } from '../../../components/representative-modal/representative-modal.component';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-location-list',
  templateUrl: './department-list.component.html',
})
export class DepartmentListComponent implements OnInit {
  sortEvent: SortEvent = {
    direction: 'asc',
    column: 'title',
  };
  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  showDeleteBtn = true;

  constructor(
    private subheaderService: SubheaderService,
    private departmentService: DepartmentService,
    private toastService: ToastrService,
    private layoutUtilsService: LayoutUtilsService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private modalService: NgbModal,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) {}
  departments: IDepartmentModel[] = [];

  @ViewChild('searchInput', { static: true }) searchInput: ElementRef;

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_DEPARTMENT];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_DEPARTMENT_CREATE];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_DEPARTMENT_EDIT];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_DEPARTMENT_DELETE];        
      }
      else {
        this.toastService.error(response.message);
      }
    });    
  }

  loadDepartment(): void {
    const filter = {
      name: this.searchInput.nativeElement.value,
    };
    const queryParams = new QueryParamsModel(
      filter,
      this.sortEvent.direction,
      this.sortEvent.column
    );

    this.departmentService.getAll(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        this.departments = response.result;
      } else {
        this.toastService.error(
          'Lấy danh sách phòng ban thất bại, vui lòng thử lại!'
        );
      }
    });
  }

  ngOnInit(): void {
    this.loadPermissions();
    this.initBreadCrumbs();
    this.loadDepartment();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý phòng ban', page: `admin/department` },
    ]);
  }

  editDepartment(id: number): void {
    this.router.navigate(['./edit', id], { relativeTo: this.activatedRoute });
  }

  deleteDepartment(item: IDepartmentModel): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.departmentService.delete(item.id).subscribe((response) => {
          if (response.isSuccess) {
            this.toastService.success('Xóa thành công!');
            this.loadDepartment();
          } else {
            this.toastService.error('Xóa thất bại, xin vui lòng thử lại!');
          }
        });
      }
    });
  }

  onSort(event: SortEvent): void {
    if (!event || !event.column || !event.direction) {
      return;
    }

    this.sortEvent = event;
    this.loadDepartment();
  }

  updateSortOrder(department: IDepartmentModel): void {
    const body = [
      {
        op: 'replace',
        path: '/sortOrder',
        value: department.sortOrder,
      },
    ];
    this.departmentService
      .patchUpdate(department.id, body)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastService.success('Bạn đã lưu số thứ tự thành công!');
          this.loadDepartment();
        } else {
          this.toastService.error('Lưu thất bại, xin vui lòng thử lại!');
        }
      });
  }

  openRepresentativeModal(department: IDepartmentModel): void {
    const modalRef = this.modalService.open(RepresentativeModalComponent);
    modalRef.componentInstance.department = department;
    modalRef.result.then((result) => {
      if (result.submit) {
        this.loadDepartment();
      }
    });
  }
}
