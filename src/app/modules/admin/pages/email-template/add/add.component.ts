import { Location } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { PermissionList } from 'src/app/configs/permission';
import { AuthService, SubheaderService } from 'src/app/core';
import { EmailTemplateService } from 'src/app/core/services/email-template.service';
import { PermissionUIService } from 'src/app/core/services/permission.service';
import { AuthModel } from 'src/app/shared';
import { EmailTemplateModel } from 'src/app/shared/models/email-template.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit, OnDestroy {

  form: FormGroup;
  template: EmailTemplateModel;
  templateId?: number = null;
  isModify = false;

  modalReference: NgbModalRef;
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;

  fileUrl = '';
  fileName = '';

  UIPermissions: any = {};
  accessable = true;
  showCreateBtn = true;
  showEditBtn = true;

  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  userId = this.userInfo?.userId != undefined ? this.userInfo.userId : 0;
  
  private subscriptions: Subscription[] = [];
  constructor(
    private subheaderService: SubheaderService,
    private emailTemplateService: EmailTemplateService,
    private toastService: ToastrService,
    private postFB: FormBuilder,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    protected sanitizer: DomSanitizer,

    private modalService: NgbModal,
    private authService: AuthService,
    private cdr: ChangeDetectorRef,

    private permissionService: PermissionUIService,
    private location: Location,
    private permissionList: PermissionList
  ) {
    this.initBreadCrumbs();
    this.createForm();
    this.templateId = this.activatedRoute.snapshot.params?.id;
    this.isModify = !!this.templateId;
  }

  loadPermissions():void{
    const subScription = this.permissionService.getPermissionList().subscribe((response) => {
      if (response.isSuccess) {
        this.UIPermissions = this.permissionService.transformPermission(response.result);  
        if(this.isModify){
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_EMAIL_TEMPLATE];
        } else{
          this.accessable = this.UIPermissions[this.permissionList.ADMIN_EMAIL_TEMPLATE];
        }     
        if(!this.accessable) this.location.back();        
        this.showEditBtn = this.UIPermissions[this.permissionList.ADMIN_EMAIL_TEMPLATE];      
        this.showCreateBtn = this.UIPermissions[this.permissionList.ADMIN_EMAIL_TEMPLATE];                       
      }
      else {
        this.toastService.error(response.message);
      }
    });
    this.subscriptions.push(subScription);
  }
  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản Lý Mẫu Email', page: `admin/email-template` },
      { 
        title: this.isModify ? 'Chỉnh sửa' : 'Thêm mới', 
        page: `admin/email-template/${this.isModify ? 'edit/'+this.templateId:'add'}` 
      },
    ]);
  }

  createForm(): void {
    this.form = this.postFB.group({
      title: [null, Validators.compose([Validators.required])],
      typeEmail: ['Approve'],
      isActvie: [false]
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
    this.emailTemplateService.create(payLoad).subscribe((res) => {
      if (res.isSuccess) {
        this.toastService.success('Thêm mới thành công!');
        this.router.navigateByUrl('/admin/email-template');
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
    payLoad.emailTemplateId = this.templateId;
    this.emailTemplateService.update(payLoad)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastService.success('Bạn đã cập nhật tin thành công!');
          this.router.navigateByUrl('/admin/email-template');
        }
        else {
          this.toastService.error(
            'Đã xảy ra lỗi trong quá trình lưu địa điểm, xin vui lòng thử lại!'
          );
        }
      });
  }
  preparePost(): EmailTemplateModel {
    const controls = this.form.controls;
    const template: EmailTemplateModel = {
      title: controls.title.value,
      isActvie: controls.isActvie.value,
      fileName: this.fileName,
      filePath: this.fileUrl,
      typeEmail: controls.typeEmail.value,
      cloudinaryPublicId: this.fileUrl,
      organizeId: this.organizeId
    };

    return template;
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

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }
  ngOnInit(): void {
    this.loadPermissions();
    this.loadData();
  }

  loadData(): void {
    if (!this.templateId) { return; }

    this.emailTemplateService.getById(this.templateId).subscribe((response) => {
      if (response.isSuccess) {
        this.template = response.result as EmailTemplateModel;
        this.fileUrl = this.template.filePath;
        this.fileName = this.template.fileName;
        this.pathValue();
      } else {
        this.toastService.error(
          'Lấy tin thất bại, xin vui lòng thử lại!'
        );
      }
    });
  }

  pathValue(): void {
    Object.keys(this.template).forEach((key: string) => {
      if (this.form.contains(key)) {
        const formControl = this.form.get(key);
        if (this.template[key] && formControl) {
          formControl.patchValue(this.template[key]);
        }
      }
    });
  }


  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader(): void {
    const BASE_URL = environment.base_url;
    const localToken = this.authService.getToken();
    const tokenObj: AuthModel = JSON.parse(localToken);
    this.uploader = new FileUploader({
      url: `${BASE_URL}uploaders/attachmentFile/v2`,
      authToken: 'Bearer ' + tokenObj.accessToken,
      isHTML5: true,
      allowedFileType: ['doc', 'docx'],
      removeAfterUpload: true,
      autoUpload: false,
      queueLimit: 1,
      maxFileSize: 10 * 1024 * 1024, // 10 MB      
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
      this.fileName = file.file.name;      
    };
    
    this.uploader.onBuildItemForm = (fileItem, form) => {
      console.log(fileItem.file.rawFile);
      form.append('fileUpload', fileItem.file.rawFile);
      form.append('organizeId', this.organizeId);
      form.append('userId', this.userId);
      form.append('mode', 'EmailTemplate')
    };    


    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res = JSON.parse(response);
        var settingValue = res.result.getData;
        console.log(settingValue);        
        this.fileUrl = settingValue;
        this.toastService.success('Bạn đã cập nhật ảnh thành công!');
        this.modalReference.close();
        this.cdr.markForCheck();
      }
    };

    this.uploader._onErrorItem = (item, response, status, header) => {
      this.toastService.error('Đã xảy ra lỗi trong quá tình tải ảnh!');
      this.cdr.markForCheck();
    };
  }

  openUpdateImageModal(content: any): void {    
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
      this.toastService.warning('Bạn phải chọn ít nhất một ảnh!');
      return;
    }
    this.uploader.uploadAll();
  }
}
