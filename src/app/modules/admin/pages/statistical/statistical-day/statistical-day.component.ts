import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { StatisticalService } from 'src/app/core/services/statistical.service';
import { IStatisticalDay, QueryParamsModel } from 'src/app/shared';

@Component({
  selector: 'app-statistical-day',
  templateUrl: './statistical-day.component.html',
})
export class StatisticalDayComponent implements OnInit, OnDestroy {
  startDate: NgbDateStruct;
  endDate: NgbDateStruct;
  chartView = [1200, 600];
  private subscriptions: Subscription[] = [];
  statisticalData: IStatisticalDay;
  constructor(
    private calendar: NgbCalendar,
    private statisticalService: StatisticalService,
    private toastr: ToastrService,
  ) {}

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.initDatePicker();
    this.search();
  }

  initDatePicker(): void {
    this.endDate = this.calendar.getToday();
    this.startDate = this.calendar.getNext(this.calendar.getToday(), 'd', -10);
  }

  search(): void {
    let startDate = '';
    if (this.startDate) {
      startDate = `${this.startDate.month}/${this.startDate.day}/${this.startDate.year}`;
    }

    let endDate = '';
    if (this.endDate) {
      endDate = `${this.endDate.month}/${this.endDate.day}/${this.endDate.year}`;
    }

    const filter = {
      startDate,
      endDate,
    };
    const queryParams = new QueryParamsModel(
      filter,
    );

    const subScription = this.statisticalService.byDay(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        this.statisticalData = response.result;
      } else {
        this.toastr.error(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }
}
