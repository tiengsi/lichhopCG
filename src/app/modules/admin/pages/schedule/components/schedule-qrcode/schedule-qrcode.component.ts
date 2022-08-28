import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { ScheduleService } from 'src/app/core';
import { ScheduleFilesAttachment } from 'src/app/shared/models/schedule.model';

@Component({
  selector: 'app-schedule-qrcode',
  templateUrl: './schedule-qrcode.component.html',
  styleUrls: ['./schedule-qrcode.component.scss']
})
export class ScheduleQrcodeComponent implements OnInit {

  @Input() scheduleId;
  constructor(
    public activeModal: NgbActiveModal,
    private scheduleService: ScheduleService,
    private toastService: ToastrService,) { }
  
  

  qrString = new String();
  src = '';
  scheduleFilesAttachment: any;
  closeModal(isSubmit: boolean): void {   
    this.activeModal.close({
      submit: isSubmit
    });
  }
  onSubmit(){    
    var checkBoxes = document.querySelectorAll('.checkOne');
    var checkIds = [];
    checkBoxes.forEach(e => {
      if((<HTMLInputElement>e).checked)
      {
        checkIds.push((<HTMLInputElement>e).value)
      }
    });
    //console.log(checkIds);    
    this.scheduleFilesAttachment.forEach(x => {
      if(checkIds.includes(x.id.toString())){
        x.isShare = true;
      }
      else{
        x.isShare = false;
      }
    });
    this.scheduleService.updateSharedFiles(this.scheduleFilesAttachment).subscribe(res => {
      if(res.isSuccess) {
        this.toastService.success('Lưu thành công!');    
      }
      else this.toastService.error(
        'Lưu thất bại'
      );
    });
    this.activeModal.close();
  }

  getData(schedId: number): void {    
    const attach = this.scheduleService.getAllFilesAttachments(schedId).toPromise();
    const qrCode = this.scheduleService.getQRCodeByScheduleId(schedId).toPromise();           
    Promise.all([attach, qrCode]).then((res) => {
      if(res[0].isSuccess){
        this.scheduleFilesAttachment = [];
        this.scheduleFilesAttachment = res[0].result as ScheduleFilesAttachment[];                
        console.log(this.scheduleFilesAttachment);        
      }
      if (res[1].isSuccess) {
        this.qrString = '';
        this.qrString = res[1].result as String;
        this.src = 'data:image/png;base64,' + this.qrString;
        //console.log(this.qrString);
      }
    });
  }

  
  downloadQRImage(){
    window.location.href = 'data:application/octet-stream;base64,' + this.qrString;
  }

  onSelectAll(){
    var checkBoxAll = <HTMLInputElement>document.getElementById('checkAll');
    var checkBoxes = document.querySelectorAll('.checkOne');
    checkBoxes.forEach(e => {
      (<HTMLInputElement>e).checked = checkBoxAll.checked;
    })
  }

  ngOnInit(): void {
    this.getData(this.scheduleId);    
  }
  
  // ngOnChanges(changes: SimpleChanges): void {
  //   this.getData(this.scheduleId);    
  // }

}
