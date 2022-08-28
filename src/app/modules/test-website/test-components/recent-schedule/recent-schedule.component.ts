import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, ParamMap } from '@angular/router';
import { ScheduleService, UserService } from 'src/app/core';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { UserForSelectModel } from 'src/app/shared';
import { Organization } from 'src/app/shared/models/organization.model';
import { ScheduleFilesAttachment, ScheduleModel } from 'src/app/shared/models/schedule.model';


@Component({
  selector: 'app-recent-schedule',
  templateUrl: './recent-schedule.component.html',
  styleUrls: ['./recent-schedule.component.scss']
})
export class RecentScheduleComponent implements OnInit {

  tabLabels = ['CHƯƠNG TRÌNH HỌP', 'TÀI LIỆU HỌP', 'GHI CHÚ CÁ NHÂN', 'KẾT LUẬN CUỘC HỌP'];

  constructor(private scheduleService: ScheduleService, private route: ActivatedRoute, private userService: UserService, private organizeService: OrganizationService) { }

  scheduleId = -1;
  infoLoaded = false;
  docsLoaded = false;
  docsLoaded2 = false;
  noteLoaded = false;
  resultLoaded = false;
  scheduleDetail: any;
  scheduleHistory: any;
  scheduleFilesAttachment: any;
  infoModel: any;
  meetingInfo: any;  
  hasData = false;  
  hostName = '';
  organizeName = '';

  userInfo = JSON.parse(localStorage.getItem('app-schedule-token')); 
  userId = this.userInfo?.userId != undefined ? this.userInfo.userId : 0;
  userName = this.userInfo?.userName != undefined ? this.userInfo.userName : 0;

  
  getData(scheduleId: number): void {
    var detailz: any;
    if(scheduleId == -1){      
      var currentUserId = -1;
      var currentUserEmail = 'a';
      if(this.userId != 0){
        currentUserId = this.userId;
      }
      else{
        currentUserEmail = this.userName;
      }      
      var currentDate = new Date();
      var curDate = currentDate.toDateString();
      curDate = curDate.substring(4);
      console.log(curDate);
      detailz = this.scheduleService.getLastestScheduleOfLoggedInUser(currentUserId, currentUserEmail, curDate).toPromise();      
    }
    else{
      detailz = this.scheduleService.getScheduleById(scheduleId).toPromise();
    }
    const detail = detailz;      
    Promise.all([detail]).then((res) => {
      if (res[0].isSuccess) {
        this.scheduleDetail = null;
        this.scheduleDetail = res[0].result as ScheduleModel;        
        
        if(this.scheduleDetail != null && this.scheduleDetail?.scheduleId != undefined){
          this.hasData = true;
          this.scheduleId = this.scheduleDetail.scheduleId;              

          if(this.scheduleDetail.id == 0 || this.scheduleDetail.id == null){
            this.scheduleDetail.hostName = this.scheduleDetail.officerName;
            const organizeSub = this.organizeService.GetById(this.scheduleDetail.organizeId).subscribe(res => {
                  if(res.isSuccess){
                    var organize = res.result as Organization;
                    this.organizeName = organize.name;
                    this.scheduleDetail.associatedUnits = [this.organizeName];
                    this.scheduleDetail.reportUnit = this.organizeName;
                    console.log('Organize: ' + this.organizeName);
  
                    this.transformData();
                    this.meetingInfo = {
                      id: this.scheduleId,
                      title: this.infoModel?.title,
                      timeStart: this.infoModel?.timeStart,
                      timeEnd: this.infoModel?.timeEnd,
                    }
                    this.infoLoaded=true;
                    this.noteLoaded=true;
                    this.resultLoaded = true;
                    this.docsLoaded = true;
                    console.log(this.scheduleDetail);  
                  }
                })  
          }else{
            const hostSub = this.userService.getUserById(this.scheduleDetail.id).subscribe(res => {
              if(res.isSuccess){
                var host = res.result as UserForSelectModel;
                this.hostName = host.fullName;
                this.scheduleDetail.hostName = this.hostName;
                console.log('HostName: ' + this.hostName);
                const organizeSub = this.organizeService.GetById(this.scheduleDetail.organizeId).subscribe(res => {
                  if(res.isSuccess){
                    var organize = res.result as Organization;
                    this.organizeName = organize.name;
                    this.scheduleDetail.associatedUnits = [this.organizeName];
                    this.scheduleDetail.reportUnit = this.organizeName;
                    console.log('Organize: ' + this.organizeName);
  
                    this.transformData();
                    this.meetingInfo = {
                      id: this.scheduleId,
                      title: this.infoModel?.title,
                      timeStart: this.infoModel?.timeStart,
                      timeEnd: this.infoModel?.timeEnd,
                    }
                    this.infoLoaded=true;
                    this.noteLoaded=true;
                    this.resultLoaded = true;
                    this.docsLoaded = true;
                    console.log(this.scheduleDetail);  
                  }
                })              
              }
            })

          }
          
          const attachments = this.scheduleService.getAllFilesAttachments(this.scheduleDetail.scheduleId).toPromise();
          Promise.all([attachments]).then((res) =>{
            if (res[0].isSuccess) {              
              this.scheduleFilesAttachment = [];
              this.scheduleFilesAttachment = res[0].result as ScheduleFilesAttachment[];
              this.docsLoaded2 = true;
              //console.log(this.scheduleFilesAttachment);
            }
          });          
        }
      }
      else{
        console.log(
          res[0]
        )
      }      
    });        
  }
  transformData():void {
    var schedDate = new Date(this.scheduleDetail?.scheduleDate);
    var schedEndDate = this.scheduleDetail?.scheduleEndDate == null || this.scheduleDetail?.scheduleEndDate == undefined ? schedDate : new Date(this.scheduleDetail?.scheduleEndDate);
    this.infoModel={
      title: this.scheduleDetail?.scheduleTitle,
      timeStart: this.scheduleDetail?.scheduleTime + ` - Ngày ${schedDate.getDate()} tháng ${schedDate.getMonth() + 1} năm ${schedDate.getFullYear()}` ,
      timeEnd: (this.scheduleDetail?.scheduleEndTime == null || this.scheduleDetail?.scheduleEndTime == undefined ? this.scheduleDetail?.scheduleTime : this.scheduleDetail?.scheduleEndTime) + ` - Ngày ${schedEndDate.getDate()} tháng ${schedEndDate.getMonth() + 1} năm ${schedEndDate.getFullYear()}`,
      venue: this.scheduleDetail?.scheduleLocation || this.scheduleDetail?.otherLocation,
      // host: {
      //   name: this.scheduleDetail?.officerName,
      //   position: 'Giám đốc',
      //   unit: 'Ban giám đốc',
      //   department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái'
      // },
      hostName: this.scheduleDetail.hostName,
      meetingLink: this.scheduleDetail.meetingLink,
      reportUnit: this.scheduleDetail.reportUnit,
      associatedUnits: this.scheduleDetail.associatedUnits,
      meetingSummary: this.scheduleDetail?.scheduleContent?.toString(),
      attendants: [
        
      ],
      attended: 0,
      absented: 0,
      guests: [
        
      ],
      guestsAttended: 0,
      guestsAbsented: 0
    };
    
    this.scheduleDetail?.participantIsSelected.forEach( x =>{
      this.infoModel.attendants.push(
        {
          name: x.receiverName,
          position: '',
          unit: '',
          department: x.departmentName,
          hadAttended: true
        })
    });  
    this.scheduleDetail?.otherParticipants.forEach(x => {
      this.infoModel.guests.push({
        name: x.name,
        description: x.email + ' - ' + x.phoneNumber,
        hadAttended: true
      })
    })      
  }
  ngOnInit(): void {    
    this.route.queryParams.subscribe(params => {
      this.scheduleId = params['sid'];
    });
    if(this.scheduleId == undefined || this.scheduleId <= 0 || this.scheduleId == null){
      this.scheduleId = -1;
    }
    this.getData(this.scheduleId);    
  }

}
