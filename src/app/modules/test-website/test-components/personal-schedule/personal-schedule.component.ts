import { Component, OnInit } from '@angular/core';
import { ScheduleService } from 'src/app/core';
import { QueryParamsModel } from 'src/app/shared';

@Component({
  selector: 'app-personal-schedule',
  templateUrl: './personal-schedule.component.html',
  styleUrls: ['./personal-schedule.component.scss']
})
export class PersonalScheduleComponent implements OnInit {

  ////new-version-by-week-fe?sortOrder=asc&sortField=&index=0&pageSize=10&host=0&locationId=-1&startDate=3/28/2022&endDate=4/3/2022&selectAllWeek=true&active=1&status=-1
  tabLabels = [
    'Lịch họp',
    'Lịch cá nhân'
  ]
  constructor(private scheduleService: ScheduleService) { }

  // getData(){
  //   const filter = {
  //     host: 0,
  //     locationId: -1,
  //     startDate: '3/28/2022',
  //     endDate: `4/3/2022`,
  //     selectAllWeek: true,
  //     active: 1,
  //     status: -1,
  //   };
  //   const params = new QueryParamsModel(filter);
  //   var getSchedule = this.scheduleService.getAllByWeekForFE(params).subscribe((response) => {
  //       if (response.isSuccess) {
  //         var zz = response.result;
  //         console.log(zz);
  //       }
  //     });
  // }
  ngOnInit(): void {
    // this.getData();
  }

}
