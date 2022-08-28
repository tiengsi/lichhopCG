import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SettingModel } from 'src/app/shared';
import { SettingService, SubheaderService, ToastService } from 'src/app/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-setting-common',
  templateUrl: './setting-common.component.html',
  styleUrls: ['./setting-common.component.scss'],
})
export class SettingCommonComponent implements OnInit, OnDestroy {
  settings: SettingModel[];

  userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));     
  isSuperAdmin = this.userInfo.roles.includes('SuperAdmin');
  

  UIPermissions: any = {};
  accessable = true;
  // Subscriptions
  private subscriptions: Subscription[] = [];

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '15rem',
    minHeight: '5rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [[]],
    customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText',
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
  };

  constructor(
    private subheaderService: SubheaderService,
    private settingService: SettingService,
    private toastService: ToastrService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList,
    private router: Router) {}

    loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_SETTINGS_GENERAL];
        if(!this.accessable) this.location.back();       
        //this.router.navigate(['/error/404']); 
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
    this.loadSetting();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Cấu hình website', page: `admin/setting` },
    ]);
  }

  loadSetting(): void {
    const loadSettingSubcript = this.settingService.getAll().subscribe(response => {
      if (response.isSuccess) {
        this.settings = response.result;
      }
    });
    this.subscriptions.push(loadSettingSubcript);
  }

  onSumbit(): void {
    const updateSub = this.settingService.update(this.settings).subscribe(response => {
      if (response.isSuccess) {
        this.toastService.success('Cập nhật thành công!');
      } else {
        this.toastService.error('Cập nhật thất bại!');
      }
    });

    this.subscriptions.push(updateSub);
  }
}
