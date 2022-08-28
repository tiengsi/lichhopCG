import { Component, OnInit } from '@angular/core';
import { ScheduleService } from 'src/app/core';
import { QueryParamsModel } from 'src/app/shared/models/query-params.model';

@Component({
  selector: 'app-assigned-schedule',
  templateUrl: './assigned-schedule.component.html',
  styleUrls: ['./assigned-schedule.component.scss']
})
export class AssignedScheduleComponent implements OnInit {

  assignedSchedule=[
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('12/1/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/1/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/2/2022'), morning: false },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/3/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/1/2022'), morning: false },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/21/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/21/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/21/2022'), morning: false },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/28/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/28/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/22/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/30/2022'), morning: false },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('3/31/2022'), morning: true },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('4/1/2022'), morning: false },
    { id: 1, title: 'Schedule No.2', content: 'Content No.2', time: new Date('4/2/2022'), morning: true },
  ];
  constructor(private scheduleService: ScheduleService) { }
  
  startDateOfWeek = new Date('Mon Apr 4 2022');
  endDateOfWeek = this.addDays(this.startDateOfWeek, 7);
  schedules: any;
  getData(){
    var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));   
    var currentUserId = -1;
    var currentUserEmail = '';
    if(userInfo != null && userInfo?.userId != null){
      currentUserId = userInfo?.userId;
    }
    else{
      currentUserEmail = userInfo?.userName;
    }
    const filter = {
      // host: 0,
      // locationId: -1,
      // startDate: `${this.startDateOfWeek.getMonth() + 1
      //   }/${this.startDateOfWeek.getDate()}/${this.startDateOfWeek.getFullYear()}`,
      // endDate: `${this.endDateOfWeek.getMonth() + 1
      //   }/${this.endDateOfWeek.getDate()}/${this.endDateOfWeek.getFullYear()}`,
      // selectAllWeek: true,
      // active: -1, //Change 1 to -1
      // status: -1,
      userId: currentUserId,
      userEmail: currentUserEmail,
      startDate: `${this.startDateOfWeek.getMonth() + 1
        }/${this.startDateOfWeek.getDate()}/${this.startDateOfWeek.getFullYear()}`,
      endDate: `${this.endDateOfWeek.getMonth() + 1
        }/${this.endDateOfWeek.getDate()}/${this.endDateOfWeek.getFullYear()}`,
      selectAllWeek: true
    };
    const queryParams = new QueryParamsModel(filter);
    const loadSub = this.scheduleService
      .getScheduleOfLoginUser(queryParams)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.schedules = response.result;
          console.log(response.result);
          this.transformData();
          console.log(this.schedules);
          console.log(this.assignedSchedule);
        }
      });
  }
  transformData(){
    this.assignedSchedule = [];
    this.schedules.forEach(x => {
      x.morning.forEach(y => {
        this.assignedSchedule.push(
          {
            id: y.scheduleId,
            title: y.scheduleTitle,
            content: y.scheduleContent,
            time: new Date(y.scheduleDate),
            morning: true
          }
        )
      });
      x.afternoon.forEach(y=>{
        this.assignedSchedule.push(
          {
            id: y.scheduleId,
            title: y.scheduleTitle,
            content: y.scheduleContent,
            time: new Date(y.scheduleDate),
            morning: false
          }
        )
      });
      x.evening.forEach(y=>{
        this.assignedSchedule.push(
          {
            id: y.scheduleId,
            title: y.scheduleTitle,
            content: y.scheduleContent,
            time: new Date(y.scheduleDate),
            morning: false
          }
        )
      })
    })
  }
  addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }
  changeDate(startDate: string){
    this.startDateOfWeek = new Date(startDate);
    this.endDateOfWeek = this.addDays(this.startDateOfWeek, 7);
    this.getData();
  }
  ngOnInit(): void {
    this.assignedSchedule = [];
    var prevMonday = new Date();    
    prevMonday.setDate(prevMonday.getDate() - (prevMonday.getDay() + 6) % 7);    
    this.startDateOfWeek = new Date(prevMonday.toDateString());
    this.endDateOfWeek = this.addDays(this.startDateOfWeek, 7);
    this.getData();
  }

}
//api/schedules/new-version-by-week-fe?sortOrder=asc&sortField=&index=0&pageSize=10&host=0&locationId=-1&startDate=4/4/2022&endDate=4/10/2022&selectAllWeek=true&active=-1&status=-1