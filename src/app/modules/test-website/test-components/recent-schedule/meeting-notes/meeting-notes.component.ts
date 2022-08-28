import { Component, Input, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ScheduleService } from 'src/app/core';
import { PersonalNoteModel } from '../../../models/personalNote.model';

@Component({
  selector: 'app-meeting-notes',
  templateUrl: './meeting-notes.component.html',
  styleUrls: ['./meeting-notes.component.scss']
})
export class MeetingNotesComponent implements OnInit {

  public Editor = ClassicEditor;
  model: any;

  @Input() meetingInfo: any;
  // meetingInfo = {
  //   title: 'HỘI NGHỊ BAN THƯỜNG VỤ TỈNH ỦY TRIỂN KHAI NHIỆM VỤ CHUYỂN ĐỔI SỐ TỈNH YÊN BÁI NĂM 2022',
  //   timeStart: '07:30 11/03/2022',
  //   timeEnd: '11:30 11/03/2022',
  // }  
  noteList = [
    {
      id: 1,
      title: 'Note no.1',
      createdBy: {
        name: 'Hoàng Minh Tiến'
      },
      content: 'zz',
      modified: new Date().toString()     
    }
  ]
  constructor(private scheduleService: ScheduleService) { }
  
  getData(){
    var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));  
    var getPersonalNote = this.scheduleService.getPersonalNotesByScheduleIdAndUserId(this.meetingInfo.id, userInfo.userId).toPromise();
    Promise.all([getPersonalNote]).then((res) =>{
      if(res[0].isSuccess){        
        res[0].result.forEach(x => {
          this.noteList.push({
            id: x.personalNotesId,
            title: x.title,
            createdBy: {
              name: x.user?.fullName
            },
            content: x.contentNote,
            modified: x.updatedDate.toString()
          })
        })
      }
      else console.log(res[0].message)
    })
  }
  onSubmit(f: NgForm){    
    var userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));  
    this.noteList.push({
      id: 0,
      title: f.form.controls.title.value,
      createdBy: {
        name: userInfo.displayName
      },
      content: f.form.controls.content.value,
      modified: new Date().toString()
    });
    this.scheduleService.createPersonalNote({
      personalNotesId: 0,
      title: f.form.controls.title.value,
      userId: userInfo.userId,
      contentNote: f.form.controls.content.value,
      scheduleId: this.meetingInfo.id,      
    } as PersonalNoteModel).subscribe(res => {
      if(res.isSuccess) console.log('Tạo lịch thành công');
      else console.log(res.message);
    })
    f.resetForm();
  }
 
  hideAndShow(classString){
    var elem = document.querySelectorAll(classString)
    elem.forEach((e: any) => e.style.display = e.style.display != 'none' ? 'none' : 'flex');
  }

  viewNote(i: number){
    this.model = this.noteList[i];
    this.toggleForm(1);
  }
  toggleForm(flag){
    if(flag == 1) document.getElementById('detailModal').style.display = 'block';
    else if(flag == 2) document.getElementById('detailModal').style.display = 'none';    
  }
  ngOnInit(): void {
    this.noteList = [];
    this.getData();
  }

}
