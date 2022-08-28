import { SettingModel } from './../../../../shared/models/setting.model';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SettingService } from 'src/app/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, OnDestroy {
  footerSetting: SettingModel;
  // Subscriptions
  private subscriptions: Subscription[] = [];
  constructor(
    private settingService: SettingService
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    const subBanner = this.settingService.getByKey('SettingPageFooter').subscribe(response => {
      if (response.isSuccess) {
        this.footerSetting = response.result;
      }
    });
    this.subscriptions.push(subBanner);
  }

}
