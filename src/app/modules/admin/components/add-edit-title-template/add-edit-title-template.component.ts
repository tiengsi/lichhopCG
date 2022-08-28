import { ITitleTemplate } from './../../../../shared/models/title-template.model';
import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { TitleTemplateService } from 'src/app/core/services/title-template.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-edit-title-template',
  templateUrl: './add-edit-title-template.component.html',
})
export class AddEditTitleTemplateComponent implements OnInit {
  @Input() titleTemplate: ITitleTemplate;
  isModify = false;
  form: FormGroup;
  title = 'Thêm mới mẫu tiêu đề lịch';
  userInfo = JSON.parse(localStorage.getItem('app-schedule-token'));
  organizeId = this.userInfo?.organizeId != undefined ? this.userInfo.organizeId : 0;
  constructor(
    public activeModal: NgbActiveModal,
    private toastr: ToastrService,
    private titleTemplateService: TitleTemplateService,
    private postFB: FormBuilder,
  ) { }

  ngOnInit(): void {
    this.createForm();
    this.isModify = !!this.titleTemplate;
    if (this.isModify) {
      this.title = 'Chỉnh sửa mẫu tiêu đề lịch';
      this.pathValue();
    }
  }

  createForm(): void {
    this.form = this.postFB.group({
      template: [null, Validators.compose([Validators.required])],
    });
  }

  pathValue(): void {
    Object.keys(this.titleTemplate).forEach((key: string) => {
      if (this.form.contains(key)) {
        const formControl = this.form.get(key);
        if (this.titleTemplate[key] && formControl) {
          formControl.patchValue(this.titleTemplate[key]);
        }
      }
    });
  }

  isControlHasError(controlName: string, validationType: string): boolean {
    const control = this.form.controls[controlName];
    if (!control) {
      return false;
    }

    const result =
      control.hasError(validationType) && (control.dirty || control.touched);
    return result;
  }

  isCheckValidForm(): boolean {
    const controls = this.form.controls;
    if (this.form.invalid) {
      Object.keys(controls).forEach((controlName) =>
        controls[controlName].markAsTouched()
      );

      return false;
    }

    return true;
  }

  onCreate(): void {
    if (!this.isCheckValidForm()) { return; }

    const payLoad = this.preparePost();
    this.titleTemplateService.create(payLoad).subscribe((res) => {
      if (res.isSuccess) {
        this.toastr.success('Thêm mới thành công!');
        this.closeModal(true);
      }
      else {
        this.toastr.error(
          res.message
        );
      }
    });
  }

  onSave(): void {
    if (!this.isCheckValidForm()) { return; }

    const payLoad = this.preparePost();
    payLoad.id = this.titleTemplate.id;
    this.titleTemplateService.update(payLoad)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastr.success('Bạn đã cập nhật thành công!');
          this.closeModal(true);
        }
        else {
          this.toastr.error(
            response.message
          );
        }
      });
  }

  preparePost(): ITitleTemplate {
    const controls = this.form.controls;
    const location: ITitleTemplate = {
      template: controls.template.value,
      organizeId: this.organizeId
    };

    return location;
  }

  closeModal(isSubmit: boolean): void {
    this.activeModal.close({
      submit: isSubmit,
    });
  }

}
