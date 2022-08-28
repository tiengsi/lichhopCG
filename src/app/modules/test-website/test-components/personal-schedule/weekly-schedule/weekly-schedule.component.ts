import { Component, EventEmitter, Input, OnInit, Output, OnChanges } from '@angular/core';
import { PersonalScheduleModel } from '../../../models/personalSchedule.model';
import { SelfScheduleModel } from '../../../models/selfSchedule.model';

@Component({
  selector: 'app-weekly-schedule',
  templateUrl: './weekly-schedule.component.html',
  styleUrls: ['./weekly-schedule.component.scss']
})
export class WeeklyScheduleComponent implements OnInit, OnChanges {
  
  @Input() isSelf = true;
  @Input() scheduleList = [];  

  @Output() startDate = new EventEmitter<string>();
  @Output() submitForm = new EventEmitter<PersonalScheduleModel>();

  sDate = 'Mon Apr 4 2022';
  detailModal = null;
  dayList = [];
  filtedList=[];
  yearValues = [];
  weekDayValues = [];  
  generateYearValues(){
    var curYear = new Date().getFullYear();
    var yearMaxVal = curYear + 1;
    var startYear = 2019;
    for(let i = startYear; i<=yearMaxVal; i++) {
      this.yearValues.push({
          val: i.toString(),
          isSelected: i == curYear
        }
      );
    }
  } 
  addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
  }
  minusDate(date, days){
    var result = new Date(date);
    result.setDate(result.getDate() - days);
    return result;
  }
  generateWeekDayValues(year) {
    var curYear = year;    
    var startDateString = '1/1/'+curYear;
    var endDateString = '12/31/'+curYear;
    var endDate = new Date(endDateString);
    var startDate = new Date(startDateString);
    var laterDate;
    if(startDate.getDay() == 1){
      laterDate = this.addDays(startDateString, 6);
    }
    else{
      var i = 1;
      while(true){
        if(this.addDays(startDateString, i).getDay() == 0){
          laterDate = this.addDays(startDateString, i);
          break;
        }
        i++;
      }
    }
    while(laterDate <= endDate){
      var zz = new Date();
      this.weekDayValues.push(
        {
          startValue: startDate.toDateString(),
          start: this.formatDate(startDate.toDateString()),
          end: this.formatDate(laterDate.toDateString()),
          isSelected: (startDate <= zz && zz <= endDate)
        }
      );
      startDate = this.addDays(laterDate.toDateString(), 1);
      laterDate = this.addDays(startDate.toDateString(), 6);
    }
    if(laterDate != endDate){
      startDate = this.addDays(laterDate.toDateString(), 1);
      var zz = new Date();
      this.weekDayValues.push(
        {
          startValue: startDate.toDateString(),
          start: this.formatDate(startDate.toDateString()),
          end: this.formatDate(endDate.toDateString()),
          isSelected: (startDate <= zz && zz <= endDate)
        }
      );
    }
  }
  yearChange(value){
    this.weekDayValues=[];
    this.generateWeekDayValues(value);
  }
  loadDataByWeek(value){
    var dateSString = value;
    var dateS = new Date(dateSString);
    var dateE = this.addDays(dateS, 7);
    this.filtedList = [];
    this.filtedList = this.scheduleList.filter(x => x.time >= dateS && x.time <= dateE);  
    this.dayList = [];  
    for(let i = 0; i<7;i++){
      var targetDate = this.addDays(dateS, i);
      var dayScheds = this.filtedList.filter(x => x.time.toDateString() == targetDate.toDateString());
      this.dayList.push({
        date: this.formatDate(targetDate.toDateString()),
        morningScheds: dayScheds.filter(x => x.morning == true),
        afternoonScheds: dayScheds.filter(x => x.morning == false),
      })
    }      
  }
  weekChange(value){
    this.loadDataByWeek(value);
  }
  formatDate(dateString){
    var stringArr = dateString.split(' ');
    var temp = stringArr[1];
    stringArr[1] = stringArr[2];
    stringArr[2] = temp;
    stringArr[0] = this.changeString(stringArr[0], 1);
    stringArr[2] = this.changeString(stringArr[2], 2);
    return `${stringArr[0]}, ${stringArr[1]}/${stringArr[2]}/${stringArr[3]}`;
  }
  changeString(text, type){
    if(type == 1){
      var getFrom = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
      var setFrom = ['Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7', 'Chủ nhật',]
      for(let i = 0; i< getFrom.length; i++){
        if(getFrom[i] == text) return text = setFrom[i];
      }
    }else if(type=2){
      var getFrom = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
      var setFrom = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12',]
      for(let i = 0; i< getFrom.length; i++){
        if(getFrom[i] == text) return text = setFrom[i];
      }
    }
  }
  showForm(){
    document.getElementById('myModal').style.display = 'block';    
  }
  onSubmitForm(model: SelfScheduleModel){
    console.log(model);
    this.scheduleList.push(
      {
        title: model.title, 
        content: model.content, 
        time: model.start, 
        morning: model.start.getHours() < 12
      }
    );
    var personalSchedModel = new PersonalScheduleModel();
    personalSchedModel.title = model.title;
    personalSchedModel.description = model.content;
    var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));   
    personalSchedModel.userId = userInfo.userId;
    personalSchedModel.fromdate = model.start;
    personalSchedModel.toDate = model.end;
    this.submitForm.emit(personalSchedModel);
    this.loadDataByWeek(this.sDate); 
  }

  showDetails(model){
    document.getElementById('detailModal').style.display = 'block';
    this.detailModal = model;
  }
   
  changeDate(event){
    this.startDate.emit(event);
    //this.weekChange(event);
    this.sDate = event;
  }
  constructor() { }

  ngOnInit(): void {
    var prevMonday = new Date();    
    prevMonday.setDate(prevMonday.getDate() - (prevMonday.getDay() + 6) % 7);    
    this.sDate = prevMonday.toDateString();
    this.loadDataByWeek(this.sDate);  
    this.generateYearValues();
    this.generateWeekDayValues(new Date().getFullYear());
    
  }
  
  ngOnChanges(): void{        
    this.loadDataByWeek(this.sDate);      
  }

}
