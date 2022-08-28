import { SettingModel } from './../../../../shared/models/setting.model';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SettingService } from 'src/app/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  bannerSetting: SettingModel;
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
    const subBanner = this.settingService.getByKey('SettingPageBanner').subscribe(response => {
      if (response.isSuccess) {
        this.bannerSetting = response.result;
      }
    });
    this.subscriptions.push(subBanner);
  }
}
