// Angular
import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
// RxJS
import { Observable } from 'rxjs';
// Portlet
import { PortletBodyComponent } from '../portlet-body/portlet-body.component';
import { PortletHeaderComponent } from '../portlet-header/portlet-header.component';
import { PortletFooterComponent } from '../portlet-footer/portlet-footer.component';
export interface PortletOptions {
  test?: any;
}

@Component({
  selector: 'app-portlet',
  templateUrl: './portlet.component.html',
  exportAs: 'ktPortlet',
})
export class PortletComponent implements OnInit, AfterViewInit {
  // Public properties
  @Input() loading$: Observable<boolean>;
  // portlet extra options
  @Input() options: PortletOptions;
  // portlet root classes
  @Input() class: string;

  @ViewChild('portlet', { static: true }) portlet: ElementRef;

  // portlet header component template
  @ViewChild(PortletHeaderComponent, { static: true })
  header: PortletHeaderComponent;
  // portlet body component template
  @ViewChild(PortletBodyComponent, { static: true }) body: PortletBodyComponent;
  // portlet footer component template
  @ViewChild(PortletFooterComponent, { static: true })
  footer: PortletFooterComponent;

  /**
   * Component constructor
   *
   * @param el: ElementRef
   * @param loadingBar: LoadingBarService
   */
  constructor(private el: ElementRef) {}

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
  ngAfterViewInit() {}
}
