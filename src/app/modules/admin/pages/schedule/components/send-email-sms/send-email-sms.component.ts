import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { ScheduleService } from 'src/app/core';
import { ScheduleModel } from 'src/app/shared';

@Component({
  selector: 'app-send-email-sms',
  templateUrl: './send-email-sms.component.html',
})
export class SendEmailSmsComponent implements OnInit {
  @Input() schedule: ScheduleModel;
  messageContent = '';

  constructor(
    private scheduleService: ScheduleService,
    public activeModal: NgbActiveModal,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadMessageContent();
  }

  loadMessageContent(): void {
    this.scheduleService
      .getMessageContent(this.schedule.scheduleId)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.messageContent = response.result;
        }
      });
  }

  closeModal(isSubmit: boolean): void {
    this.activeModal.close({
      submit: isSubmit,
    });
  }

  onSubmit(): void {
    if (this.messageContent === null) {
      this.toastr.warning('Bạn cần nhập nội dung tin nhắn!', 'Thông báo');
      return;
    }

    if (!this.schedule.isActive) {
      this.toastr.warning('Lịch họp này chưa được phát hành!', 'Thông báo');
      return;
    }

    const payload = {
      scheduleId: this.schedule.scheduleId,
      messageContent: this.messageContent
    };

    this.scheduleService.updateMessageContent(payload).subscribe((response) => {
      if (response.isSuccess) {
        this.toastr.success('Bạn đã lưu và gửi thư mời thành công!', 'Thông báo');
        this.closeModal(true);
      } else {
        this.toastr.error('Gửi tin nhắn thất bại, vui lòng thử lại!');
      }
    });
  }
}
