// Angular
import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';
import { OffCanvasOptions } from '../index';

/**
 * Setup off Convas
 */
@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[ktOffcanvas]',
  exportAs: 'ktOffcanvas',
})
export class OffCanvasDirective implements AfterViewInit {
  // Public properties
  @Input() options: OffCanvasOptions;
  // Private properties
  private offcanvas: any;

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
    setTimeout(() => {
      this.offcanvas = new KTOffcanvas(this.el.nativeElement, this.options);
    });
  }

  /**
   * Returns the offCanvas
   */
  getOffcanvas() {
    return this.offcanvas;
  }
}
