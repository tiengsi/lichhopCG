import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PermissionList } from 'src/app/configs/permission';
import { SubheaderService } from 'src/app/core';
import { BrandNameService } from 'src/app/core/services/brandname.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { ViettelBrandNameModel } from 'src/app/shared/models/brand-name.model';

@Component({
  selector: 'app-viettel-add',
  templateUrl: './viettel-add.component.html',
  styleUrls: ['./viettel-add.component.scss']
})
export class ViettelAddComponent implements OnInit, OnDestroy {

  form: FormGroup;
  viettel: ViettelBrandNameModel;
  viettelId?: number = null;
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
    private brandNameService: BrandNameService,
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
    this.viettelId = this.activatedRoute.snapshot.params?.id;
    this.isModify = !!this.viettelId;
   }
  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);  
        if(this.isModify){
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];
        } else{
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];
        }     
        if(!this.accessable) this.locationBack.back();        
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_BRANDNAME];                       
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
    this.loadData();
  }

  loadData(): void {
    if (!this.viettelId) { return; }

    this.brandNameService.GetViettelBrandNameById(this.viettelId).subscribe((response) => {
      if (response.isSuccess) {
        this.viettel = response.result;
        console.log(this.viettel);
        this.pathValue();
      } else {
        this.toastService.error(
          'Lấy tin thất bại, xin vui lòng thử lại!'
        );
      }
    });
  }

  pathValue(): void {
    Object.keys(this.viettel).forEach((key: string) => {
      if (this.form.contains(key)) {
        const formControl = this.form.get(key);
        if (this.viettel[key] && formControl) {
          formControl.patchValue(this.viettel[key]);
        }
      }
    });
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý Viettel Brandname', page: `admin/brandname/viettel` },
      { title: this.isModify ? 'Chỉnh sửa' : 'Thêm mới'},
    ]);
  }

  createForm(): void {
    this.form = this.postFB.group({
      contractType: ['', Validators.compose([Validators.required])],
      apiLink: ['', Validators.compose([Validators.required])],
      apiPass: ['', Validators.compose([Validators.required])],
      apiUser: ['', Validators.compose([Validators.required])],
      branchName: ['', Validators.compose([Validators.required])],
      cpCode: ['', Validators.compose([Validators.required])],      
      isActive: [true]
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

    var payLoad = this.preparePost();
    this.brandNameService.CreateViettelBrandName(payLoad).subscribe((res) => {
      if (res.isSuccess) {
        this.toastService.success('Thêm mới thành công!');
        this.router.navigateByUrl('/admin/brandname/viettel');
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

    var payLoad = this.preparePost();
    payLoad.brandNameId = this.viettelId;
    this.brandNameService.UpdateViettelBrandName(payLoad)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastService.success('Bạn đã cập nhật tin thành công!');
          this.router.navigateByUrl('/admin/brandname/viettel');
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

  preparePost(): ViettelBrandNameModel {
    const controls = this.form.controls;
    const payload: ViettelBrandNameModel = {
      apiLink: controls.apiLink.value,
      apiPass: controls.apiPass.value,
      brandNameId: 0,
      apiUser: controls.apiUser.value,
      branchName: controls.branchName.value ,     
      contractType: controls.contractType.value,
      cpCode: controls.cpCode.value,      
      isActive: controls.isActive.value,
      organizeId: this.organizeId
    };

    return payload;
  }

}
