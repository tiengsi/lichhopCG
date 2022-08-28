import { Component, OnInit, Input } from '@angular/core';
import { ScheduleService } from 'src/app/core';
import { ScheduleFilesAttachment, ScheduleModel } from 'src/app/shared/models/schedule.model';

@Component({
  selector: 'app-meeting-info',
  templateUrl: './meeting-info.component.html',
  styleUrls: ['./meeting-info.component.scss']
})
export class MeetingInfoComponent implements OnInit {
  
  isLoaded = false;
  constructor(private scheduleService: ScheduleService) { }
  @Input() infoModel:any;
  // infoModel={
  //   title: 'HỘI NGHỊ BAN THƯỜNG VỤ TỈNH ỦY TRIỂN KHAI NHIỆM VỤ CHUYỂN ĐỔI SỐ TỈNH YÊN BÁI NĂM 2022',
  //   timeStart: '07:30 11/03/2022',
  //   timeEnd: '11:30 11/03/2022',
  //   venue: 'Đô thị thông minh',
  //   host: {
  //     name: 'Hoàng Minh Tiến',
  //     position: 'Giám đốc',
  //     unit: 'Ban giám đốc',
  //     department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái'
  //   },
  //   reportUnit: '',
  //   associatedUnits: [],
  //   meetingSummary: 'Họp về cái gì đó...',
  //   attendants: [
  //     {
  //       name: 'Hoàng Minh Tiến',
  //       position: 'Giám đốc',
  //       unit: 'Ban giám đốc',
  //       department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái',
  //       hadAttended: true
  //     },
  //     {
  //       name: 'Hoàng Minh Tiến',
  //       position: 'Giám đốc',
  //       unit: 'Ban giám đốc',
  //       department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái',
  //       hadAttended: true
  //     },
  //     {
  //       name: 'Hoàng Minh Tiến',
  //       position: 'Giám đốc',
  //       unit: 'Ban giám đốc',
  //       department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái',
  //       hadAttended: true
  //     },
  //     {
  //       name: 'Hoàng Minh Tiến',
  //       position: 'Giám đốc',
  //       unit: 'Ban giám đốc',
  //       department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái',
  //       hadAttended: true
  //     },
  //     {
  //       name: 'Hoàng Minh Tiến',
  //       position: 'Giám đốc',
  //       unit: 'Ban giám đốc',
  //       department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái',
  //       hadAttended: false
  //     },
  //     {
  //       name: 'Hoàng Minh Tiến',
  //       position: 'Giám đốc',
  //       unit: 'Ban giám đốc',
  //       department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái',
  //       hadAttended: false
  //     },
  //     {
  //       name: 'Hoàng Minh Tiến',
  //       position: 'Giám đốc',
  //       unit: 'Ban giám đốc',
  //       department: 'Sở Thông tin và Truyền thông tỉnh Yên Bái',
  //       hadAttended: false
  //     }      
  //   ],
  //   attended: 4,
  //   absented: 11,
  //   guests: [
  //     {
  //       name: 'Hoàng Minh Tiến',
  //       description: 'Sở Thông tin và Truyền thông tỉnh Yên Bái',
  //       hadAttended: true
  //     }
  //   ],
  //   guestsAttended: 1,
  //   guestsAbsented: 0
  // };
  

  hideAndShow(classString){
    var elem = document.querySelectorAll(classString)
    elem.forEach((e: any) => e.style.display = e.style.display != 'none' ? 'none' : 'table-row');
  }
  trueToFalse(a, b){
    if(a.hadAttended == true && b.hadAttended == false) return -1;
    if(a.hadAttended == false && b.hadAttended == true) return 1;
    return 0;
  };  
  sortByAttendace(z: boolean){
    if(z) this.infoModel.attendants.sort(this.trueToFalse);
    else this.infoModel.attendants = this.infoModel.attendants.sort(this.trueToFalse).reverse();
  }
  ngOnInit(): void {
    this.infoModel?.attendants.sort(this.trueToFalse);
  }

}
