import { Component, OnInit } from '@angular/core';
import { ScheduleService } from 'src/app/core';
import { PersonalScheduleModel } from '../../../models/personalSchedule.model';

@Component({
  selector: 'app-self-schedule',
  templateUrl: './self-schedule.component.html',
  styleUrls: ['./self-schedule.component.scss']
})
export class SelfScheduleComponent implements OnInit {

  selfSchedule=[
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('12/1/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/1/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/2/2022'), morning: false },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/3/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/1/2022'), morning: false },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/21/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/21/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/21/2022'), morning: false },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/28/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/28/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/22/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/30/2022'), morning: false },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('3/31/2022'), morning: true },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('4/1/2022'), morning: false },
    { id:1, title: 'Schedule No.1', content: 'Content No.1', time: new Date('4/2/2022'), morning: true },
  ];
  apiSchedList: PersonalScheduleModel[];
  constructor(private scheduleService: ScheduleService) { }

  getData(userId: number): void {
    const sched = this.scheduleService.getPersonalScheduleByUserId(userId).toPromise();           
    Promise.all([sched]).then((res) => {
      if (res[0].isSuccess) {
        console.log(res[0].result);        
        this.apiSchedList = res[0].result as PersonalScheduleModel[];
        //this.transformData();
        this.transFormData();        
        console.log(this.apiSchedList);
        console.log(this.selfSchedule);      
      }
    });
  }
  transFormData(){
    this.selfSchedule = [];
    this.apiSchedList.forEach(x => {
      this.selfSchedule.push({
        id: x.personalScheduleId,
        title: x.title,
        content: x.description,
        time: new Date(x.fromdate),
        morning: new Date(x.fromdate).getHours() < 12 ? true : false
      })
    })
  }
  addNewPersonalSched(event: PersonalScheduleModel){
    this.scheduleService.createPersonalSchedule(event).subscribe(res => {
      if(res.isSuccess) console.log(event);
      else console.log('Tạo thất bại');
    })
  }

  changeDate(event){
    this.selfSchedule = [];
    var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));
    this.getData(userInfo.userId);
  }
  ngOnInit(): void {
    this.apiSchedList = [];
    this.selfSchedule = [];
    var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));
    this.getData(userInfo.userId);
  }

}
