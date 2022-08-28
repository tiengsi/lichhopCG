import { Component, ElementRef, OnInit, Renderer2, ViewEncapsulation } from '@angular/core';
import { LayoutConfigService, SplashScreenService, TranslationService } from './../../core';
import { AuthNoticeService } from '../../core/authentication/auth-notice/auth-notice.service';

@Component({
  selector: 'app-auth',
  styleUrls: ['./auth.component.scss'],
  templateUrl: 'auth.component.html',
  encapsulation: ViewEncapsulation.None
})

export class AuthComponent implements OnInit {
    // Public properties
    today: number = Date.now();
    headerLogo: string;

    /**
     * Component constructor
     *
     * @param el
     * @param render
     * @param layoutConfigService: LayoutConfigService
     * @param authNoticeService: authNoticeService
     * @param translationService: TranslationService
     * @param splashScreenService: SplashScreenService
     */
    constructor(
      private el: ElementRef,
      private render: Renderer2,
      private layoutConfigService: LayoutConfigService,
      public authNoticeService: AuthNoticeService,
      private splashScreenService: SplashScreenService,
      private translationService: TranslationService) {
    }

    /**
     * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
     */

    /**
     * On init
     */
    ngOnInit(): void {
      this.translationService.setLanguage(this.translationService.getSelectedLanguage());
      this.headerLogo = this.layoutConfigService.getLogo();

      this.splashScreenService.hide();
    }

}
