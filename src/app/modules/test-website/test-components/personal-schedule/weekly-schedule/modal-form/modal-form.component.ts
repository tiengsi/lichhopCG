import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { SelfScheduleModel } from 'src/app/modules/test-website/models/selfSchedule.model';

@Component({
  selector: 'app-modal-form',
  templateUrl: './modal-form.component.html',
  styleUrls: ['./modal-form.component.scss']
})
export class ModalFormComponent implements OnInit {

  public Editor = ClassicEditor;
  @Output() formInfo = new EventEmitter<SelfScheduleModel>();
  constructor() { } 

  toggleForm(flag){
    if(flag == 1) document.getElementById('myModal').style.display = 'block';
    else if(flag == 2) document.getElementById('myModal').style.display = 'none';    
  }
  
  onSubmit(f: NgForm){
    var formObj = f.form.controls;    
    var model = new SelfScheduleModel(formObj.title.value, formObj.content.value, new Date(formObj.startDate.value), new Date(formObj.endDate.value));
    f.resetForm();
    this.toggleForm(2);
    this.formInfo.emit(model);
    //console.log(model);
  }
  ngOnInit(): void {
  }

}
