import { AuthModel } from './../../../../../shared/';
import { environment } from './../../../../../../environments/environment';
import { AuthService, SettingService, SubheaderService, ToastService } from './../../../../../core/';
import { SettingModel } from './../../../../../shared/';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { FileUploader } from 'ng2-file-upload';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { PermissionList } from 'src/app/configs/permission';
import { Location } from '@angular/common';

@Component({
  selector: 'app-setting-image',
  templateUrl: './setting-image.component.html',
  styleUrls: ['./setting-image.component.scss']
})
export class SettingImageComponent implements OnInit, OnDestroy {
  faviconSetting: SettingModel;
  bannerSetting: SettingModel;
  settingSelect: string;
  modalReference: NgbModalRef;
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  UIPermissions: any = {};
  accessable = true;
  // Subscriptions
  private subscriptions: Subscription[] = [];
  constructor(
    private subheaderService: SubheaderService,
    private settingService: SettingService,
    private toastService: ToastService,
    private modalService: NgbModal,
    private authService: AuthService,
    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList,
    private cdr: ChangeDetectorRef) { }
  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }
loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);        
        this.accessable = this.UIPermissions[this.permissionList.ADMIN_SETTINGS_BANNER_FAVICON];
        if(!this.accessable) this.location.back();        
      }
      else {
        //this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }
  ngOnInit(): void {
    this.loadPermissions();
    this.initBreadCrumbs();
    this.loadData();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Cấu hình website', page: `admin/setting` },
      { title: 'Banner và Favicon', page: `admin/setting/setting-image` },
    ]);
  }

  loadData(): void {
    const subFavicon = this.settingService.getByKey('SettingPageFavicon').subscribe(response => {
      if (response.isSuccess) {
        this.faviconSetting = response.result;
      }
    });
    this.subscriptions.push(subFavicon);

    const subBanner = this.settingService.getByKey('SettingPageBanner').subscribe(response => {
      if (response.isSuccess) {
        this.bannerSetting = response.result;
      }
    });
    this.subscriptions.push(subBanner);
  }

  initializeUploader(): void {
    const BASE_URL = environment.base_url;
    const localToken = this.authService.getToken();
    const tokenObj: AuthModel = JSON.parse(localToken);
    this.uploader = new FileUploader({
      url: `${BASE_URL}uploaders/${this.settingSelect}/setting`,
      authToken: 'Bearer ' + tokenObj.accessToken,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024, // 10 MB
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res = JSON.parse(response);
        if (this.settingSelect === this.faviconSetting.settingKey) {
          this.faviconSetting.settingValue = res.result.url;
        } else if (this.settingSelect === this.bannerSetting.settingKey) {
          this.bannerSetting.settingValue = res.result.url;
        }

        this.toastService.showSuccess('Bạn đã cập nhật ảnh thành công!');
        this.modalReference.close();
        this.cdr.markForCheck();
      }
    };

    this.uploader._onErrorItem = (item, response, status, header) => {
      this.toastService.showError('Đã xảy ra lỗi trong quá tình tải ảnh!');
      this.cdr.markForCheck();
    };
  }

  openUpdateImageModal(setting: string, content: any): void {
    this.settingSelect = setting;
    this.initializeUploader();
    this.modalReference = this.modalService.open(content, {
      ariaLabelledBy: 'modal-basic-title',
      size: 'lg',
    });
    this.modalReference.result.then(
      (result) => {
        console.log(`Closed with: ${result}`);
      },
      (reason) => {
        console.log(`reason: ${reason}`);
      }
    );
  }

  onSaveImage(): void {
    if (this.uploader.queue.length === 0) {
      this.toastService.showWarning('Bạn phải chọn ít nhất một ảnh!');
      return;
    }
    this.uploader.uploadAll();
  }
}
