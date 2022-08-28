import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PermissionList } from 'src/app/configs/permission';
import { LayoutUtilsService, SubheaderService } from 'src/app/core';
import { EmailTemplateService } from 'src/app/core/services/email-template.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { EmailTemplateModel } from 'src/app/shared/models/email-template.model';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit, OnDestroy {

  emailTemplates: EmailTemplateModel[];
  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;
  showDeleteBtn = true;
  emailTypes = {
    Approve: 'Thông báo duyệt lịch',
    Changed: 'Thông báo dời lịch',
    Pause: 'Thông báo hoãn lịch',
  }
  private subscriptions: Subscription[] = [];
  constructor(
    private subheaderService: SubheaderService,
    private emailTempalteService: EmailTemplateService,
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
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_EMAIL_TEMPLATE];
        if(!this.accessable) this.location.back();
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_EMAIL_TEMPLATE];
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_EMAIL_TEMPLATE];
        this.showDeleteBtn = this.UIPermissions[this.permissionList.ADMIN_EMAIL_TEMPLATE];        
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
      { title: 'Quản lý mẫu email', page: `admin/email-tempalte` },
    ]);
  }

  loadData():void{
    const subScription = this.emailTempalteService.getByOrganizeId().subscribe((response) => {
      if (response.isSuccess) {        
        this.emailTemplates = response.result as EmailTemplateModel[];    
        console.log(response.result)    ;
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

  delete(id: number): void {
    const dialogRef = this.layoutUtilsService.deleteElement();
    dialogRef.result.then((result) => {
      if (result === 'Ok') {
        this.emailTempalteService.delete(id).subscribe(response => {
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
}
