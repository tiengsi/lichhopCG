import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ScheduleService, UserService } from 'src/app/core';
import { OrganizationService } from 'src/app/core/services/organization.service';
import { ScheduleFilesAttachment, ScheduleModel } from 'src/app/shared/models/schedule.model';

@Component({
  selector: 'app-qr-test',
  templateUrl: './qr-test.component.html',
  styleUrls: ['./qr-test.component.scss']
})
export class QrTestComponent implements OnInit {

  constructor(private scheduleService: ScheduleService, private route: ActivatedRoute, private userService: UserService, private organizeService: OrganizationService) { }

  src = '';
  qrString = new String();
  scheduleId;
  scheduleFilesAttachment;
  scheduleInfo;
  hostName = '';
  organizeName = '';
  timeAndDate = '';
  timeAndDateEnd = '';
  getData(schedId: number): void {   
    const schedInfo = this.scheduleService.getScheduleById(schedId).toPromise();     
    const attach = this.scheduleService.getSharedFiles(schedId).toPromise();
    const qrCode = this.scheduleService.getQRCodeByScheduleId(schedId).toPromise();           
    Promise.all([qrCode, attach, schedInfo]).then((res) => {
      if (res[0].isSuccess) {
        this.qrString = '';
        this.qrString = res[0].result as String;
        this.src = 'data:image/png;base64,' + this.qrString;
        //console.log(this.qrString);
      }
      if(res[1].isSuccess){
        this.scheduleFilesAttachment = [];
        this.scheduleFilesAttachment = res[1].result as ScheduleFilesAttachment[];     
        // this.scheduleFilesAttachment.forEach(e => {
        //   if(e.filePath.slice(-3) == 'pdf'){

        //   }else{
        //     e.filePath = 'https://docs.google.com/gview?url='+e.filePath;
        //   }
        // })           
        console.log(this.scheduleFilesAttachment);        
      }
      if(res[2].isSuccess){        
        this.scheduleInfo = res[2].result as ScheduleModel;                
        console.log(this.scheduleInfo);    
        var schedDate = new Date(this.scheduleInfo?.scheduleDate);
        this.timeAndDate = this.scheduleInfo?.scheduleTime + ` - Ngày ${schedDate.getDate()} tháng ${schedDate.getMonth() + 1} năm ${schedDate.getFullYear()}`;
        var forEndDate = this.scheduleInfo?.scheduleEndDate == null || this.scheduleInfo?.scheduleEndDate == undefined ? new Date(this.scheduleInfo?.scheduleDate) : new Date(this.scheduleInfo?.scheduleEndDate);
        var forEndTime = this.scheduleInfo?.scheduleEndTime == null || this.scheduleInfo?.scheduleEndTime == undefined ? this.scheduleInfo?.scheduleTime : this.scheduleInfo?.scheduleEndTime;
        this.timeAndDateEnd = forEndTime + ` - Ngày ${forEndDate.getDate()} tháng ${forEndDate.getMonth() + 1} năm ${forEndDate.getFullYear()}`;
        if(this.scheduleInfo.id == null || this.scheduleInfo.id == 0){
          this.hostName = this.scheduleInfo.officerName;
        }
        else{
          const hostSub = this.userService.getUserById(this.scheduleInfo.id).subscribe(res => {
            if(res.isSuccess){
              this.hostName = res.result.fullName;
            }
          });
        }
        const organizeSub = this.organizeService.GetById(this.scheduleInfo.organizeId).subscribe(res => {
          if(res.isSuccess){
            this.organizeName= res.result.name;
          }
        })    
      }
    });
  }

  hideAndShow(classString){
    var elem = document.querySelectorAll(classString)
    elem.forEach((e: any) => e.style.display = e.style.display != 'none' ? 'none' : 'flex');
  }
  ngOnInit(): void {
    var z = <HTMLDivElement>document.querySelector('.user-block');
    console.log(z);
    z.style.visibility = 'hidden';    
    this.route.queryParams.subscribe(params => {
      this.scheduleId = params['sid'];
      console.log(this.scheduleId);
    });

    this.getData(this.scheduleId);    
  }

}
