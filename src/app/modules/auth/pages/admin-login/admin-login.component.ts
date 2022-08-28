import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  ViewEncapsulation,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { from, Observable, Subject } from 'rxjs';
import { finalize, takeUntil, tap } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';

import { AuthNoticeService, AuthService, UserService } from '../../../../core';

// Store
import { AppState } from '../../../../core/ngrx-store/reducers';
import { Login } from '../../../../core/ngrx-store/auth';

/**
 * ! Just example => Should be removed in production
 */
const DEMO_PARAMS = {
  EMAIL: 'admin',
  PASSWORD: 'adminP@ss1',
};

@Component({
  selector: 'app-admin-login',
  templateUrl: 'admin-login.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AdminLoginComponent implements OnInit, OnDestroy {
  // Public params
  loginForm: FormGroup;
  loading = false;
  isLoggedIn$: Observable<boolean>;
  errors: any = [];
  private returnUrl: any;
  private unsubscribe: Subject<any>;

  constructor(
    private authNoticeService: AuthNoticeService,    
    private translate: TranslateService,
    private userService: UserService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
    private store: Store<AppState>
  ) {
    this.unsubscribe = new Subject();
  }

  reloadIfNecessary() {
    var isLoadedBefore = localStorage.getItem("IsLoadedBefore");
    if(isLoadedBefore=="true"){
      return;
    } else {
      localStorage.setItem("IsLoadedBefore","true");
      location.reload();
    }
  }
  // tslint:disable-next-line: typedef
  ngOnInit() {
    this.reloadIfNecessary();
    try{      
      var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));            
        if (userInfo.roles.includes('User') || userInfo.roles.includes('Normal-Admin')){
          this.returnUrl = 'scheduler/department-schedule';
          this.router.navigate([this.returnUrl]);          
        } else{
          this.returnUrl = 'admin';
          this.router.navigate([this.returnUrl]);    
        }
    }catch{

    }

    this.initLoginForm();

    // redirect back to the returnUrl before login
    this.route.queryParams.subscribe((params) => {
      this.returnUrl = params.returnUrl || 'admin';
    });
  }

  /**
   * On destroy
   */
  ngOnDestroy(): void {
    this.authNoticeService.setNotice(null);
    this.unsubscribe.next();
    this.unsubscribe.complete();
    this.loading = false;
  }

  /**
   * Form initalization
   * Default params, validators
   */
  // tslint:disable-next-line:typedef
  initLoginForm() {
    // demo message to show
    if (!this.authNoticeService.onNoticeChanged$.getValue()) {
      const initialNotice = `Use account
			<strong>${DEMO_PARAMS.EMAIL}</strong> and password
			<strong>${DEMO_PARAMS.PASSWORD}</strong> to continue.`;
      this.authNoticeService.setNotice(initialNotice, 'info');
    }

    this.loginForm = this.fb.group({
      username: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(320), // https://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
        ]),
      ],
      password: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(16),
        ]),
      ],
    });
  }

  /**
   * Form Submit
   */
  // tslint:disable-next-line:typedef
  submit() {
    const controls = this.loginForm.controls;
    /** check form */
    if (this.loginForm.invalid) {
      Object.keys(controls).forEach((controlName) =>
        controls[controlName].markAsTouched()
      );
      return;
    }

    this.loading = true;

    const authData = {
      username: controls.username.value,
      password: controls.password.value,
    };

    this.authService
      .login(authData.username, authData.password)
      .pipe(
        tap((response) => {
          if (response.isSuccess) {
            this.store.dispatch(new Login(response.result));

            var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));            
            if (userInfo.roles.includes('User') || userInfo.roles.includes('Normal-Admin')){
              this.returnUrl = 'scheduler/department-schedule';
              // sessionStorage.setItem('userInfo', JSON.stringify({id: userInfo?.userId, name: userInfo?.displayName}));
            } 
            else this.returnUrl = 'admin';
            this.router.navigateByUrl(this.returnUrl); // Main page

          } else {
            this.authNoticeService.setNotice(
              this.translate.instant('AUTH.VALIDATION.INVALID_LOGIN'),
              'danger'
            );
          }
        }),
        takeUntil(this.unsubscribe),
        finalize(() => {
          this.loading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe();
  }

  /**
   * Checking control validation
   *
   * @param controlName: string => Equals to formControlName
   * @param validationType: string => Equals to valitors name
   */
  isControlHasError(controlName: string, validationType: string): boolean {
    const control = this.loginForm.controls[controlName];
    if (!control) {
      return false;
    }

    const result =
      control.hasError(validationType) && (control.dirty || control.touched);
    return result;
  }
}
