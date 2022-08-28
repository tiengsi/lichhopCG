import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';

// service
import { LayoutConfigService, SplashScreenService, TranslationService } from './core';

// language list
import { locale as viLang } from './configs/i18n/vi';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'body[kt-root]',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent implements OnInit, OnDestroy{
  title = 'Admin';
  loader: boolean;
  private unsubscribe: Subscription[] = [];

  /**
   * Component constructor
   *
   * @param translationService: TranslationService
   * @param router: Router
   * @param layoutConfigService: LayoutConfigService
   * @param splashScreenService: SplashScreenService
   */
  constructor(
    private router: Router,
    private layoutConfigService: LayoutConfigService,
    private splashScreenService: SplashScreenService,
    private translationService: TranslationService) {

    // register translations
    this.translationService.loadTranslations(viLang);
  }

  /**
   * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
   */

  /**
   * On init
   */
  ngOnInit(): void {
    // enable/disable loader
    this.loader = this.layoutConfigService.getConfig('page-loader.type');

    const routerSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        // hide splash screen
        this.splashScreenService.hide();

        // scroll to top on every route change
        window.scrollTo(0, 0);

        // to display back the body content
        setTimeout(() => {
          document.body.classList.add('page-loaded');
        }, 500);
      }
    });
    this.unsubscribe.push(routerSubscription);
  }

  /**
   * On Destroy
   */
  // tslint:disable-next-line: typedef
  ngOnDestroy() {
    this.unsubscribe.forEach(sb => sb.unsubscribe());
  }
}
