import { Component, OnDestroy, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { StatisticalService } from 'src/app/core/services/statistical.service';
import { IStatisticalDay, QueryParamsModel } from 'src/app/shared';

@Component({
  selector: 'app-statistical-year',
  templateUrl: './statistical-year.component.html',
})
export class StatisticalYearComponent implements OnInit, OnDestroy {
  yearSelected = 2021;
  years = [];
  chartView = [1200, 800];
  statisticalData: IStatisticalDay;
  yAxisLabel = `Tháng (năm ${this.yearSelected})`;
  private subscriptions: Subscription[] = [];
  constructor(
    private statisticalService: StatisticalService,
    private toastr: ToastrService,
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.initYear();
    this.search();
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
      year: this.yearSelected,
    };
    const queryParams = new QueryParamsModel(
      filter,
    );

    const subScription = this.statisticalService.byYear(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        this.statisticalData = response.result;
      } else {
        this.toastr.error(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }

}
