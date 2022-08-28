import {
  SubheaderService,
  ToastService,
  UserService,
} from './../../../../../core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, of, Subscription } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { IDepartmentModel } from 'src/app/shared/models/department.model';
import { DepartmentService } from 'src/app/core/services/department.service';
import { BaseResponseModel, UserForSelectModel } from 'src/app/shared';
import { switchMap, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-department-add',
  templateUrl: './department-add.component.html',
  styleUrls: ['./department-add.component.scss'],
})
export class DepartmentAddComponent implements OnInit, OnDestroy {
  form: FormGroup;
  department: IDepartmentModel;
  parentDepartments: IDepartmentModel[] = [
    {
      id: 0,
      name: '--- Chọn thuộc phòng ban ---',
    },
  ];
  departmentId?: number = null;
  isModify = false;
  userRepresentative: UserForSelectModel[] = [];
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  userId = this.userInfo?.userId != undefined ? this.userInfo.userId : 0;
  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private subheaderService: SubheaderService,
    private departmentService: DepartmentService,
    private userService: UserService,
    private toastService: ToastrService,
    private postFB: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    protected sanitizer: DomSanitizer,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) {
    this.departmentId = +this.activatedRoute.snapshot.params?.id;
    this.isModify = !!this.departmentId;
    this.initBreadCrumbs();
    this.createForm();
  }
loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        if(this.isModify){
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_DEPARTMENT_CREATE];
        } else{
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_DEPARTMENT_EDIT];
        }  
        if(!this.accessable) this.location.back();        
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_DEPARTMENT_EDIT];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_DEPARTMENT_CREATE];                       
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
    this.loadDepartment();
    this.loadRepresentative();
  }

  loadDepartment(): void {
    this.departmentService
      .getAllForSelect()
      .pipe(
        tap((res: BaseResponseModel<IDepartmentModel[]>) => {
          if (res.isSuccess) {
            this.parentDepartments = this.isModify
              ? this.parentDepartments.concat(
                  res.result?.filter((d) => d.id !== this.departmentId)
                )
              : this.parentDepartments.concat(res.result);
          }
        }),
        switchMap((res: BaseResponseModel<IDepartmentModel[]>) => {
          if (res.isSuccess && this.isModify) {
            return this.departmentService.getById(this.departmentId);
          } else {
            return of(null);
          }
        })
      )
      .subscribe((res: BaseResponseModel<IDepartmentModel>) => {
        if (res && res.isSuccess) {
          this.department = res.result;
          this.pathValue();
        }
      });
  }

  loadRepresentative(): void {
    if (this.isModify) {
      const representativeSub = this.userService
        .getUserForSelect(-1, this.departmentId)
        .subscribe((response) => {
          if (response.isSuccess) {
            this.userRepresentative = this.userRepresentative.concat(
              response.result
            );
          }
        });

      this.subscriptions.push(representativeSub);
    }
  }

  pathValue(): void {
    Object.keys(this.department).forEach((key: string) => {
      if (this.form.contains(key)) {
        const formControl = this.form.get(key);
        if (this.department[key] && formControl) {
          formControl.patchValue(this.department[key]);
        }
      }
    });
  }

  initBreadCrumbs(): void {
    if (this.isModify) {
      this.subheaderService.setBreadcrumbs([
        { title: 'Quản lý phòng ban', page: `admin/department` },
        {
          title: 'Chỉnh sửa',
          page: `admin/department/edit/` + this.departmentId,
        },
      ]);
    } else {
      this.subheaderService.setBreadcrumbs([
        { title: 'Quản lý phòng ban', page: `admin/department` },
        { title: 'Thêm mới', page: `admin/department/add` },
      ]);
    }
  }

  createForm(): void {
    this.form = this.postFB.group({
      name: [
        null,
        Validators.compose([Validators.required, Validators.maxLength(500)]),
      ],
      shortName: [
        null,
      ],
      adress: [null, Validators.maxLength(1000)],
      email: [
        null,
        Validators.compose([Validators.email, Validators.maxLength(500)]),
      ],
      phoneNumber: [null, Validators.maxLength(40)],
      fax: [null, Validators.maxLength(20)],
      parentId: [0],
      isActive: [true],
      userRepresentative: [null],
      sortOrder: [1],
    });
  }

  isControlHasError(controlName: string, validationType: string): boolean {
    const control = this.form.controls[controlName];
    if (!control) {
      return false;
    }

    const result =
      control.hasError(validationType) && (control.dirty || control.touched);
    return result;
  }

  onCreate(): void {
    if (!this.isCheckValiForm()) {
      return;
    }

    var payLoad = this.preparePayLoad();        
    payLoad.userRepresentative = [this.userId];
    this.departmentService.create(payLoad).subscribe((res) => {
      if (res.isSuccess) {
        this.toastService.success('Thêm mới thành công!');
        this.router.navigateByUrl('/admin/department');
      } else {
        this.toastService.error('Thêm mới thất bại, xin vui lòng thử lại!');
      }
    });
  }

  onSave(): void {
    if (!this.isCheckValiForm()) {
      return;
    }

    const payLoad = this.preparePayLoad();
    this.departmentService.update(payLoad).subscribe((response) => {
      if (response.isSuccess) {
        this.toastService.success('Bạn đã cập nhật tin thành công!');
        this.router.navigateByUrl('/admin/department');
      } else {
        this.toastService.error(
          'Đã xảy ra lỗi trong quá trình lưu địa điểm, xin vui lòng thử lại!'
        );
      }
    });
  }

  isCheckValiForm(): boolean {
    const controls = this.form.controls;
    if (this.form.invalid) {
      Object.keys(controls).forEach((controlName) =>
        controls[controlName].markAsTouched()
      );

      return false;
    }

    return true;
  }

  preparePayLoad(): IDepartmentModel {
    const controls = this.form.controls;
    const department: IDepartmentModel = {
      id: this.departmentId,
      name: controls.name.value,
      shortName: controls.shortName.value,
      adress: controls.adress.value,
      email: controls.email.value,
      phoneNumber: controls.phoneNumber.value,
      fax: controls.fax.value,
      parentId: controls.parentId.value === 0 ? null : controls.parentId.value,
      isActive: controls.isActive.value,
      userRepresentative: controls.userRepresentative.value,
      sortOrder: controls.sortOrder.value,
      organizeId: this.organizeId
    };

    return department;
  }
}
