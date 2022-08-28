import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { StatisticalService } from 'src/app/core/services/statistical.service';
import { IStatisticalDay, QueryParamsModel } from 'src/app/shared';

@Component({
  selector: 'app-statistical-month',
  templateUrl: './statistical-month.component.html',
})
export class StatisticalMonthComponent implements OnInit, OnDestroy {
  chartView = [1200, 1000];
  private subscriptions: Subscription[] = [];
  statisticalData: IStatisticalDay;
  monthSelected = 1;
  months = [];
  yearSelected = 2021;
  years = [];

  constructor(
    private statisticalService: StatisticalService,
    private toastr: ToastrService,
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.initMonth();
    this.initYear();
    this.search();
  }

  initMonth(): void {
    for (let index = 1; index < 13; index++) {
      this.months.push({
        value: index,
        name: 'Tháng ' + index
      });
    }
  }

  initYear(): void {
    for (let index = 0; index < 11; index++) {
      const year = 2021 + index;
      this.years.push({
        value: year,
        name: 'Năm ' + year
      });
    }
  }

  search(): void {
    const filter = {
      month: this.monthSelected,
      year: this.yearSelected,
    };
    const queryParams = new QueryParamsModel(
      filter,
    );

    const subScription = this.statisticalService.byMonth(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        this.statisticalData = response.result;
      } else {
        this.toastr.error(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }

}
