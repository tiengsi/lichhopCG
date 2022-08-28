import { AuthModel, ChangePasswordModel } from './../../../../../shared/';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SubheaderService, UserService } from 'src/app/core';
import { Subscription } from 'rxjs';
import { environment } from '../../../../../../environments/environment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
})
export class ChangePasswordComponent implements OnInit, OnDestroy {
  changePasswordForm: FormGroup;
  changePasswordModel: ChangePasswordModel;
  hasFormErrors = false;

  // Subscriptions
  private subscriptions: Subscription[] = [];
  constructor(
    private subheaderService: SubheaderService,
    private toastService: ToastrService,
    private userService: UserService,
    private cdr: ChangeDetectorRef,
    private formBuilder: FormBuilder
  ) {}

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit() {
    this.initBreadCrumbs();
    this.createForm();
  }

  initBreadCrumbs(): void {
    this.subheaderService.setBreadcrumbs([
      { title: 'Trang chủ', page: `admin/` },
      { title: 'Đổi mật khẩu', page: `admin/user/change-password` },
    ]);
  }

  /**
   * Create form
   */
  createForm(): void {
    this.changePasswordModel = new ChangePasswordModel();

    const token = localStorage.getItem(environment.authTokenKey);
    if (token) {
      const tokenObj: AuthModel = JSON.parse(token);
      this.changePasswordModel.userName = tokenObj.userName;
    }

    this.changePasswordForm = this.formBuilder.group({
      userName: [
        { value: this.changePasswordModel.userName, disabled: true },
        Validators.compose([
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(20),
        ]),
      ],
      oldPassword: [this.changePasswordModel.oldPassword, Validators.required],
      newPassword: [
        this.changePasswordModel.newPassword,
        Validators.compose([
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(20), // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
          Validators.pattern(
            /^(?=\D*\d)(?=[^a-z]*[a-z])(?=[^A-Z]*[A-Z]).{8,30}$/
          ),
        ]),
      ],
    });
  }

  onSumbit(): void {
    this.hasFormErrors = false;
    const controls = this.changePasswordForm.controls;
    /** check form */
    if (this.changePasswordForm.invalid) {
      Object.keys(controls).forEach((controlName) =>
        controls[controlName].markAsTouched()
      );

      this.hasFormErrors = true;
      return;
    }

    this.changePasswordModel.oldPassword = controls.oldPassword.value;
    this.changePasswordModel.newPassword = controls.newPassword.value;
    const changePasswordSubscription = this.userService
      .changePassword(this.changePasswordModel)
      .subscribe(
        (response) => {
          if (response.isSuccess) {
            this.toastService.success('Bạn đã đổi mật khẩu thành công!');
          }
          else {
            this.toastService.error(response.message);
          }
        },
      );
    this.subscriptions.push(changePasswordSubscription);
  }

  /**
   * Checking control validation
   *
   * @param controlName: string => Equals to formControlName
   * @param validationType: string => Equals to valitors name
   */
  isControlHasError(controlName: string, validationType: string): boolean {
    const control = this.changePasswordForm.controls[controlName];
    if (!control) {
      return false;
    }

    const result =
      control.hasError(validationType) && (control.dirty || control.touched);
    return result;
  }
}
