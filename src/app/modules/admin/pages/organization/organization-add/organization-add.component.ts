import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PermissionList } from 'src/app/configs/permission';
import { SubheaderService } from 'src/app/core';
import { LocationService } from 'src/app/core/services/location.service';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { Organization, OrganizationTree } from 'src/app/shared/models/organization.model';

@Component({
  selector: 'app-organization-add',
  templateUrl: './organization-add.component.html',
  styleUrls: ['./organization-add.component.scss']
})
export class OrganizationAddComponent implements OnInit, OnDestroy {

  form: FormGroup;
  organization: Organization;
  organizationList: OrganizationTree[];
  selectList: any = [{id: 0, name: '- -'}];
  organizationId?: number = null;
  isModify = false;

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;

  private subscriptions: Subscription[] = [];
  
  constructor(
    private subheaderService: SubheaderService,
    private organizationService: OrganizationService,
    private toastService: ToastrService,
    private postFB: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    protected sanitizer: DomSanitizer,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) {
    this.initBreadCrumbs();
    this.createForm();
    this.organizationId = this.activatedRoute.snapshot.params?.id;
    this.isModify = !!this.organizationId;
   }

   loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);  
        if(this.isModify){
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_ORGANIZATION_EDIT];
        } else{
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_ORGANIZATION_CREATE];
        }     
        if(!this.accessable) this.location.back();        
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_ORGANIZATION_EDIT];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_ORGANIZATION_CREATE];                       
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
    this.loadSelectListData();
    //console.log(this.selectList);
    this.loadData();
  }

   loadData(): void {
    if (!this.organizationId) { return; }    
    
    this.organizationService.GetById(this.organizationId).subscribe((response) => {
      if (response.isSuccess) {
        this.organization = response.result as Organization;
        console.log(this.organization);
        this.selectList.forEach((e) => {
          e.selected = e.organizeId == this.organization.organizeId
        });
        this.pathValue();
      } else {
        this.toastService.error(
          'Lấy tin thất bại, xin vui lòng thử lại!'
        );
      }
    });
  }

  loadSelectListData(){
    const dashdash = '- -  ';
    this.organizationService.GetAllAsTree().subscribe((res) => {
      if(res.isSuccess){
        this.organizationList = res.result as OrganizationTree[];
        this.organizationList.forEach((e) => {
          this.selectList.push({
            id: e.organizeId,
            name: e.name,
          });
          if(e.subOrganizeList != null){
            e.subOrganizeList.forEach((e2) => {
              this.selectList.push({
                id: e2.organizeId,
                name: dashdash + e2.name,                
              });
              // if(e2.subOrganizeList!= null){
              //   e2.subOrganizeList.forEach((e3) => {
              //     this.selectList.push({
              //       id: e3.organizeId,
              //       name: dashdash + dashdash + e3.name
              //     });                
              //   });
              // }
            });
          }
        });
      } else{
        this.toastService.error(
          'Lấy thông tin thất bại, xin vui lòng thử lại!'
        );
      }
    })
  }

  pathValue(): void {
    Object.keys(this.organization).forEach((key: string) => {
      if (this.form.contains(key)) {
        const formControl = this.form.get(key);
        if (this.organization[key] && formControl) {
          formControl.patchValue(this.organization[key]);
        }
      }
    });
  }

    initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý đơn vị', page: `admin/organization` },
      { title: 'Thêm mới', page: `admin/organization/add` },
    ]);
  }

  createForm(): void {
    this.form = this.postFB.group({
      name: [null, Validators.compose([Validators.required])],
      organizeParentId: [null],
      codeName: [null],
      otherName: [null],
      address: [null],
      phone: [null],
      order: [0],
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
    this.organizationService.CreateOrganiztion(payLoad).subscribe((res) => {
      if (res.isSuccess) {
        this.toastService.success('Thêm mới thành công!');
        this.router.navigateByUrl('/admin/organization');
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
    console.log(payLoad); 
    payLoad.organizeId = this.organizationId;    
    this.organizationService.UpdateOrganization(payLoad)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastService.success('Bạn đã cập nhật tin thành công!');
          this.router.navigateByUrl('/admin/organization');
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

  preparePost(): Organization {
    const controls = this.form.controls;
    const organization: Organization = {
      name: controls.name.value,
      organizeParentId: controls.organizeParentId.value == 0 ? null : controls.organizeParentId.value,
      codeName: controls.codeName.value,
      otherName: controls.otherName.value,
      address: controls.address.value,
      phone: controls.phone.value,
      order: controls.order.value,
      isActive: controls.isActive.value
    };
    
    return organization;    
  }

  
}
