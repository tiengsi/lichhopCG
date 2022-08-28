import { Component, Input, OnInit, OnChanges } from '@angular/core';

@Component({
  selector: 'app-modal-detail',
  templateUrl: './modal-detail.component.html',
  styleUrls: ['./modal-detail.component.scss']
})
export class ModalDetailComponent implements OnChanges {

  @Input('self') isSelf = false;
  @Input() model = null;
  timeString = '';
  days = ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy']
  constructor() { }
  
  toggleForm(flag){
    if(flag == 1) document.getElementById('detailModal').style.display = 'block';
    else if(flag == 2) document.getElementById('detailModal').style.display = 'none';    
  }
  // ngOnInit(): void {
  //   console.log(this.model);
  // }
  goToDetail(sid: number){
    window.open(`/scheduler/schedule-detail/?sid=${sid}`, '_blank');
  }
  ngOnChanges(): void {
    if(this.model!=null){
      console.log(this.model?.time);      
      var date = new Date(this.model?.time);
      this.timeString = `${this.days[date.getDay()]} ngày ${date.getDate()}/${date.getMonth()}/${date.getFullYear()}`;
      console.log(this.timeString); 
    }
    console.log(this.model);
  }

}
