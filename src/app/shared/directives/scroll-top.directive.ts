// Angular
import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';
import { ScrollTopOptions } from '../index';
/**
 * Scroll to top
 */
@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[ktScrollTop]',
})
export class ScrollTopDirective implements AfterViewInit {
  // Public properties
  @Input() options: ScrollTopOptions;
  // Private properties
  private scrollTop: any;

  /**
   * Directive Constructor
   * @param el: ElementRef
   */
  constructor(private el: ElementRef) {}

  /**
   * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
   */

  /**
   * After view init
   */
  ngAfterViewInit(): void {
    this.scrollTop = new KTScrolltop(this.el.nativeElement, this.options);
  }

  /**
   * Returns ScrollTop
   */
  getScrollTop() {
    return this.scrollTop;
  }
}
