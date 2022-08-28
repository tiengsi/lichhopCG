import { environment } from './../../../../../../environments/environment';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { FileUploader } from 'ng2-file-upload';
import { Subscription } from 'rxjs';
import {
  NgbModal,
  ModalDismissReasons,
  NgbModalRef,
} from '@ng-bootstrap/ng-bootstrap';
import {
  AuthService,
  CategoryService,
  PostService,
  SubheaderService,
  ToastService,
} from 'src/app/core';
import {
  AuthModel,
  CategoryForSelectModel,
  ECategoryType,
  PostModel,
} from '../../../../../shared';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-post-edit',
  templateUrl: './post-edit.component.html',
  styleUrls: ['./post-edit.component.scss'],
})
export class PostEditComponent implements OnInit, OnDestroy {
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  postForm: FormGroup;
  hasFormErrors = false;
  categories: CategoryForSelectModel[] = [
    {
      categoryId: 0,
      categoryName: '--- Chọn Danh Mục ---',
    },
  ];
  post: PostModel;
  modalReference: NgbModalRef;

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
    toolbarHiddenButtons: [['bold']],
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
    private authService: AuthService,
    private postService: PostService,
    private categoryService: CategoryService,
    private toastService: ToastrService,
    private postFB: FormBuilder,
    protected sanitizer: DomSanitizer,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private modalService: NgbModal,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.initBreadCrumbs();
    this.loadCategories();
    this.loadPost();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Quản lý tin', page: `admin/post` },
      { title: 'Chỉnh sửa tin', page: `admin/post/edit` },
    ]);
  }

  /**
   * Create form
   */
  createForm(): void {
    this.postForm = this.postFB.group({
      title: [this.post.title, Validators.compose([Validators.required])],
      description: [this.post.description, Validators.required],
      body: [this.post.body],
      isActive: [this.post.isActive],
      categoryId: [this.post.categoryId],
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

  loadCategories(): void {
    const loadCateSubcription = this.categoryService
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

    this.subscriptions.push(loadCateSubcription);
  }

  loadPost(): void {
    const routeSubscription = this.activatedRoute.params.subscribe((params) => {
      const id = params.id;
      this.postService.getPostById(id).subscribe((response) => {
        if (response.isSuccess) {
          this.post = response.result;
          this.createForm();
          this.initializeUploader();
        } else {
          this.toastService.error(
            'Lấy tin thất bại, xin vui lòng thử lại!'
          );
        }
      });
    });
    this.subscriptions.push(routeSubscription);
  }

  openUpdateImageModal(content): void {
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

  initializeUploader(): void {
    const BASE_URL = environment.base_url;
    const localToken = this.authService.getToken();
    const tokenObj: AuthModel = JSON.parse(localToken);
    this.uploader = new FileUploader({
      url: `${BASE_URL}uploaders/${this.post.postId}/post`,
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
        this.post.imagePath = res.result.url;
        this.post.publicId = res.result.publicId;
        this.toastService.success('Bạn đã cập nhật ảnh thành công!');
        this.modalReference.close();
        this.cdr.markForCheck();
      }
    };

    this.uploader._onErrorItem = (item, response, status, header) => {
      this.toastService.error('Đã xảy ra lỗi trong quá tình tải ảnh!');
    };
  }

  onSaveImage(): void {
    if (this.uploader.queue.length === 0) {
      this.toastService.warning('Bạn phải chọn ít nhất một ảnh!');
      return;
    }
    this.uploader.uploadAll();
  }

  onSubmit(): void {
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

    const postPrepare = this.preparePost();
    const saveSub = this.postService
      .updatePost(postPrepare)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastService.success('Bạn đã cập nhật tin thành công!');
          setTimeout(() => {
            this.router.navigateByUrl('/admin/post');
          }, 1000);
        } else {
          this.toastService.error(
            'Đã xảy ra lỗi trong quá trình lưu tin, xin vui lòng thử lại!'
          );
        }
      });

    this.subscriptions.push(saveSub);
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
    _post.imagePath = this.post.imagePath;
    _post.publicId = this.post.publicId;
    _post.postId = this.post.postId;
    return _post;
  }
}
