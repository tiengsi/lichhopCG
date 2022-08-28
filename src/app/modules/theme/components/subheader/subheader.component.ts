import { TranslateService } from '@ngx-translate/core';
// Angular
import {
  AfterViewInit,
  Component,
  Input,
  OnDestroy,
  OnInit,
} from '@angular/core';
// RxJS
import { Subscription } from 'rxjs';
// Layout
import { SubheaderService } from '../../../../core';
import { Breadcrumb } from '../../../../core/services/subheader.service';

@Component({
  selector: 'app-subheader',
  templateUrl: './subheader.component.html',
  styleUrls: ['./subheader.component.scss'],
})
export class SubheaderComponent implements OnInit, OnDestroy, AfterViewInit {
  // Public properties
  @Input() fixed = true;
  @Input() clear = false;
  @Input() width = 'fluid';
  @Input() subheaderClasses = '';
  @Input() subheaderContainerClasses = '';
  @Input() displayDesc = false;
  @Input() displayDaterangepicker = true;

  today: number = Date.now();
  title = '';
  desc = '';
  breadcrumbs: Breadcrumb[] = [];

  // Private properties
  private subscriptions: Subscription[] = [];

  /**
   * Component constructor
   *
   * @param subheaderService: SubheaderService
   */
  constructor(
    public subheaderService: SubheaderService,
    private translate: TranslateService,) {}

  /**
   * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
   */

  /**
   * On init
   */
  ngOnInit() {}

  /**
   * After view init
   */
  ngAfterViewInit(): void {
    this.subscriptions.push(
      this.subheaderService.title$.subscribe((bt) => {
        // breadcrumbs title sometimes can be undefined
        if (bt) {
          this.title = bt.title;
          this.desc = bt.desc;
        }
      })
    );

    this.subscriptions.push(
      this.subheaderService.breadcrumbs$.subscribe((bc) => {
        this.breadcrumbs = bc;
      })
    );
  }

  /**
   * On destroy
   */
  ngOnDestroy(): void {
    this.subscriptions.forEach((sb) => sb.unsubscribe());
  }
}
