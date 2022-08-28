import { environment } from './../../../../../../environments/environment';
import {
  ECategoryType,
  PostModel,
  AuthModel,
  CategoryForSelectModel,
} from './../../../../../shared';
import {
  CategoryService,
  PostService,
  SubheaderService,
  AuthService,
} from './../../../../../core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Subscription } from 'rxjs';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { DomSanitizer } from '@angular/platform-browser';
import { FileUploader } from 'ng2-file-upload';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { EditorConfig } from 'src/app/shared/models/post.model';

@Component({
  selector: 'app-post-add',
  templateUrl: './post-add.component.html',
  styleUrls: ['./post-add.component.scss'],
})
export class PostAddComponent implements OnInit, OnDestroy {
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  postForm: FormGroup;
  hasFormErrors = false;
  loading = false;
  categories: CategoryForSelectModel[] = [
    {
      categoryId: 0,
      categoryName: '--- Chọn Danh Mục ---',
    },
  ];
  post: PostModel;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  config: AngularEditorConfig = EditorConfig;

  constructor(
    private subheaderService: SubheaderService,
    private authService: AuthService,
    private postService: PostService,
    private categoryService: CategoryService,
    private toastService: ToastrService,
    private postFB: FormBuilder,
    protected sanitizer: DomSanitizer,
    private router: Router,
  ) {}

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader(): void {
    const BASE_URL = environment.base_url;
    const localToken = this.authService.getToken();
    const tokenObj: AuthModel = JSON.parse(localToken);
    this.uploader = new FileUploader({
      url: `${BASE_URL}uploaders/`,
      authToken: 'Bearer ' + tokenObj.accessToken,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: false,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024, // 10 MB
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res = JSON.parse(response);

        const post = this.preparePost();
        post.imagePath = res.result.url;
        post.publicId = res.result.publicId;
        this.postService.createUser(post).subscribe((createResponse) => {
          if (createResponse.isSuccess) {
            this.toastService.success('Thêm mới thành công!');
            this.loading = false;
            setTimeout(() => {
              this.router.navigateByUrl('/admin/post');
            }, 1000);
          } else {
            this.loading = false;
            this.toastService.error(
              'Thêm mới thất bại, xin vui lòng thử lại!'
            );
          }
        });
      }
    };

    this.uploader._onErrorItem = (item, response, status, header) => {
      this.loading = false;
      this.toastService.error('Đã xảy ra lỗi trong quá tình tải ảnh!');
    };
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.initBreadCrumbs();
    this.createForm();
    this.loadCategories();
    this.initializeUploader();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý tin', page: `admin/post` },
      { title: 'Thêm mới', page: `admin/post/add` },
    ]);
  }

  /**
   * Create form
   */
  createForm(): void {
    this.post = new PostModel();
    this.postForm = this.postFB.group({
      title: [this.post.title, Validators.compose([Validators.required])],
      description: [this.post.description, Validators.required],
      body: [this.post.body],
      isActive: [this.post.isActive],
      categoryId: [this.post.categoryId],
    });
  }

  loadCategories(): void {
    this.categoryService
      .getAllForSelect(ECategoryType.Post)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.categories = this.categories.concat(response.result);
        } else {
          this.toastService.error(
            'Lấy danh mục thất bại, vui lòng thử lại!'
          );
        }
      });
  }

  /**
   * Checking control validation
   *
   * @param controlName: string => Equals to formControlName
   * @param validationType: string => Equals to valitors name
   */
  isControlHasError(controlName: string, validationType: string): boolean {
    const control = this.postForm.controls[controlName];
    if (!control) {
      return false;
    }

    const result =
      control.hasError(validationType) && (control.dirty || control.touched);
    return result;
  }

  onSumbit(): void {
    this.hasFormErrors = false;
    const controls = this.postForm.controls;
    /** check form */
    if (this.postForm.invalid) {
      Object.keys(controls).forEach((controlName) =>
        controls[controlName].markAsTouched()
      );

      this.hasFormErrors = true;
      return;
    }

    if (controls.categoryId.value === 0) {
      this.toastService.warning('Bạn phải chọn danh mục!');
      return;
    }

    if (this.uploader?.queue?.length === 0) {
      this.toastService.warning('Bạn phải chọn ít nhất một ảnh!');
      return;
    }

    this.loading = true;
    this.uploader.uploadAll();
  }

  /**
   * Returns object for saving
   */
  preparePost(): PostModel {
    const controls = this.postForm.controls;
    // tslint:disable-next-line: variable-name
    const _post = new PostModel();
    _post.title = controls.title.value;
    _post.description = controls.description.value;
    _post.categoryId = controls.categoryId.value;
    _post.body = controls.body.value;
    _post.isActive = controls.isActive.value;
    return _post;
  }
}
