// Angular
import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
// RxJS
import { from, Subscription } from 'rxjs';
// Auth
import { AuthNoticeService } from '../../../../core/authentication/auth-notice/auth-notice.service';
import { AuthNotice } from '../../../../core/authentication/auth-notice/auth-notice.interface';

@Component({
  selector: 'app-auth-notice',
  templateUrl: './auth-notice.component.html',
})
export class AuthNoticeComponent implements OnInit, OnDestroy {
  @Output() type: any;
  // tslint:disable-next-line: no-output-native
  @Output() message: any = '';

  // Private properties
  private subscriptions: Subscription[] = [];

  /**
   * Component Constructors
   *
   * @param authNoticeService
   * @param cdr
   */
  constructor(
    public authNoticeService: AuthNoticeService,
    private cdr: ChangeDetectorRef
  ) {}

  /*
   * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
   */

  /**
   * On init
   */
  // tslint:disable-next-line:typedef
  ngOnInit() {
    this.subscriptions.push(
      this.authNoticeService.onNoticeChanged$.subscribe(
        (notice: AuthNotice) => {
          notice = Object.assign({}, { message: '', type: '' }, notice);
          this.message = notice.message;
          this.type = notice.type;
          this.cdr.markForCheck();
        }
      )
    );
  }

  /**
   * On destroy
   */
  ngOnDestroy(): void {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }
}
