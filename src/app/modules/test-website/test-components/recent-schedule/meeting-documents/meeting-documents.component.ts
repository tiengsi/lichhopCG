import { Component, Input, OnInit } from '@angular/core';
import { ScheduleService } from 'src/app/core';

@Component({
  selector: 'app-meeting-documents',
  templateUrl: './meeting-documents.component.html',
  styleUrls: ['./meeting-documents.component.scss']
})
export class MeetingDocumentsComponent implements OnInit {

  @Input() docsData: any[];
  @Input('info') meetingInfo : any;
  // meetingInfo = {
  //   title: 'HỘI NGHỊ BAN THƯỜNG VỤ TỈNH ỦY TRIỂN KHAI NHIỆM VỤ CHUYỂN ĐỔI SỐ TỈNH YÊN BÁI NĂM 2022',
  //   timeStart: '07:30 11/03/2022',
  //   timeEnd: '11:30 11/03/2022',
  // } 
  
  initMeetingDocs = [
    {
      name: 'Chương trình và Báo cáo Hội nghị chuyển đổi số T3.2022',
      docs: [
        {
          name: 'Chương trình hội Hội nghỉ chuyển đổi số T3.2022.doc',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },
        {
          name: 'Dự thảo Báo cáo kết quả thực hiện NQ 51 về chuyển đổi số.pdf',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        }
      ]
    },
    {
      name: 'Tổng hợp tham luận tại Hội nghị',
      docs: [
        {
          name: 'Tham luận - Huyện ủy Văn Yên.docx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },
        {
          name: 'Tham luận - Công an tỉnh.docx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },
        {
          name: 'Tham luận - Bưu điện.docx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },
        {
          name: 'Tham luận - Sở giáo dục.docx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },{
          name: 'Tham luận - Bệnh viện tỉnh.docx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },
        {
          name: 'Tham luận - Đảng ủy khối DN.docx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },
      ]
    },
    {
      name: 'Bài tham luận của Giám đốc Sở Thông tin và Truyền thông',
      docs: [
        {
          name: 'Bài tham luận của Giám đốc Sở Thông tin và Truyền thông.docx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },
        {
          name: 'Trình chiếu bài tham luận của Giám đốc Sở Thông tin và Truyền thông.pptx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        }
      ]
    },
    {
      name: 'Tham luận của GĐ Khiêm Trung tâm chuyển đổi số',
      docs: [
        {
          name: 'Tham luan Dc Khiem GD Trung tam CDS.docx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        },
        {
          name: 'Tham luan Dc Khiem GD Trung tam CDS.pptx',
          modified: new Date(),
          isPublic: true,
          status: 4,          
        }
      ]
    }
  ]  
  meetingDocs = this.initMeetingDocs;
  constructor(private scheduleService: ScheduleService) { }

  qrString = new String();
  src = '';
  getData(schedId: number): void {    
    const qrCode = this.scheduleService.getQRCodeByScheduleId(schedId).toPromise();           
    Promise.all([qrCode]).then((res) => {
      if (res[0].isSuccess) {
        this.qrString = '';
        this.qrString = res[0].result as String;
        this.src = 'data:image/png;base64,' + this.qrString;
        //console.log(this.qrString);
      }
    });
  }

  checkAll(i: number){
    var checkBoxAll = document.getElementById('checkBoxAll-' + i);
    var allCheckBoxes = document.querySelectorAll('.checkBox-'+i);
    allCheckBoxes.forEach((x: any) => (<HTMLInputElement>x).checked = (<HTMLInputElement>checkBoxAll).checked);
  }
  filtingResult(){
    var searchText = (<HTMLInputElement>document.getElementById('input-search')).value;
    this.meetingDocs = [];
    this.initMeetingDocs.forEach(x => {
      var z = x.docs.filter(z => z.name.toLowerCase().includes(searchText.toLowerCase()));
      if(z.length != 0) this.meetingDocs.push({
        name: x.name,
        docs: z
      });
    })    
  }
  clearSearch(){
    (<HTMLInputElement>document.getElementById('input-search')).value = '';
    this.meetingDocs = this.initMeetingDocs;
  }
  hideAndShow(classString){
    var elem = document.querySelectorAll(classString)
    elem.forEach((e: any) => e.style.display = e.style.display != 'none' ? 'none' : 'flex');
  }
  showDoc(doc: any){
    window.alert('path to file: ' + doc);
  }
  downDoc(){
    window.alert('downloading file');
  }
  shareDoc(){
    window.open('https://www.google.com/', '_blank').focus();
  }


  ngOnInit(): void {
    this.getData(this.meetingInfo.id);
  }

}
