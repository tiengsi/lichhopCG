import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ToastService, UserService } from 'src/app/core';
import { AuthModel, ChangePasswordModel } from 'src/app/shared';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit, OnDestroy {

  changePasswordForm: FormGroup;
  changePasswordModel: ChangePasswordModel;
  hasFormErrors = false;
  private subscriptions: Subscription[] = [];
  constructor(
    
    private toastService: ToastrService,
    private userService: UserService,
    private cdr: ChangeDetectorRef,
    private formBuilder: FormBuilder
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit() {    
    this.createForm();
  }

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
