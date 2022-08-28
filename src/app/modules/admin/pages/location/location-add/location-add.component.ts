import {
  SubheaderService, ToastService,
} from './../../../../../core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Subscription } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { LocationService } from 'src/app/core/services/location.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-location-add',
  templateUrl: './location-add.component.html',
  styleUrls: ['./location-add.component.scss'],
})
export class LocationAddComponent implements OnInit, OnDestroy {
  form: FormGroup;
  location: ILocationModel;
  locationId?: number = null;
  isModify = false;

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  // Subscriptions
  private subscriptions: Subscription[] = [];

  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;

  constructor(
    private subheaderService: SubheaderService,
    private locationService: LocationService,
    private toastService: ToastrService,
    private postFB: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    protected sanitizer: DomSanitizer,
    private permissionService: PermissionUIService,
    private locationBack: Location,
    private permissionList: PermissionList
  ) {
    this.initBreadCrumbs();
    this.createForm();
    this.locationId = this.activatedRoute.snapshot.params?.id;
    this.isModify = !!this.locationId;
  }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);  
        if(this.isModify){
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_LOCATION_EDIT];
        } else{
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_LOCATION_ADD];
        }     
        if(!this.accessable) this.locationBack.back();        
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_LOCATION_EDIT];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_LOCATION_ADD];                       
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
    this.loadLocation();
  }

  loadLocation(): void {
    if (!this.locationId) { return; }

    this.locationService.getById(this.locationId).subscribe((response) => {
      if (response.isSuccess) {
        this.location = response.result;
        this.pathValue();
      } else {
        this.toastService.error(
          'Lấy tin thất bại, xin vui lòng thử lại!'
        );
      }
    });
  }

  pathValue(): void {
    Object.keys(this.location).forEach((key: string) => {
      if (this.form.contains(key)) {
        const formControl = this.form.get(key);
        if (this.location[key] && formControl) {
          formControl.patchValue(this.location[key]);
        }
      }
    });
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý địa điểm', page: `admin/location` },
      { title: 'Thêm mới', page: `admin/location/add` },
    ]);
  }

  createForm(): void {
    this.form = this.postFB.group({
      title: [null, Validators.compose([Validators.required])],
      isActive: [false]
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
    if (!this.isCheckValiForm()) { return; }

    const payLoad = this.preparePost();
    this.locationService.create(payLoad).subscribe((res) => {
      if (res.isSuccess) {
        this.toastService.success('Thêm mới thành công!');
        this.router.navigateByUrl('/admin/location');
      }
      else {
        this.toastService.error(
          'Thêm mới thất bại, xin vui lòng thử lại!'
        );
      }
    });
  }

  onSave(): void {
    if (!this.isCheckValiForm()) { return; }

    const payLoad = this.preparePost();
    payLoad.id = this.locationId;
    this.locationService.update(payLoad)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastService.success('Bạn đã cập nhật tin thành công!');
          this.router.navigateByUrl('/admin/location');
        }
        else {
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

  preparePost(): ILocationModel {
    const controls = this.form.controls;
    const location: ILocationModel = {
      title: controls.title.value,
      isActive: controls.isActive.value,
      organizeId: this.organizeId
    };

    return location;
  }
}
