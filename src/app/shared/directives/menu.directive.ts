// Angular
import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';
// Object-Path
import * as objectPath from 'object-path';
import { MenuOptions } from '../index';

/**
 * Configure menu
 */
@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[ktMenu]',
  exportAs: 'ktMenu',
})
export class MenuDirective implements AfterViewInit {
  // Public properties
  @Input() options: MenuOptions;
  // Private properties
  private menu: any;

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
    this.setupOptions();
    this.menu = new KTMenu(this.el.nativeElement, this.options);
  }

  /**
   * Return the menu
   */
  getMenu() {
    return this.menu;
  }

  /**
   * Setup menu options
   */
  private setupOptions() {
    // init aside menu
    let menuDesktopMode = 'accordion';
    if (this.el.nativeElement.getAttribute('data-menu-dropdown') === '1') {
      menuDesktopMode = 'dropdown';
    }

    if (typeof objectPath.get(this.options, 'submenu.desktop') === 'object') {
      objectPath.set(this.options, 'submenu.desktop', menuDesktopMode);
    }
  }
}
