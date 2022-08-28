import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ToastService } from 'src/app/core';
import { EmailLogsService } from 'src/app/core/services/email-logs.service';
import { QueryParamsModel, ScheduleModel } from 'src/app/shared';
import { IEmailLogs } from 'src/app/shared/models/email-logs.mode';

@Component({
  selector: 'app-email-sms-log',
  templateUrl: './email-sms-log.component.html',
  styleUrls: ['./email-sms-log.component.scss']
})
export class EmailSmsLogComponent implements OnInit, OnDestroy {
  @Input() schedule: ScheduleModel;
  participants: IEmailLogs[] = [];
  otherParticipant: IEmailLogs[] = [];
  isCompleteSendSms: boolean;
  isCompleteSendEmail: boolean;

  private subscriptions: Subscription[] = [];

  constructor(
    private emailLogsService: EmailLogsService,
    private toastService: ToastService,) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit() {
    this.loadEmailLogs();
  }

  loadEmailLogs(): void {
    const filter = {
      scheduleId: this.schedule.scheduleId
    };
    const queryParams = new QueryParamsModel(
      filter
    );

    const subScription = this.emailLogsService.getAll(queryParams).subscribe((response) => {
      if (response.isSuccess) {
        for (let index = 0; index < response.result.emailSmsLogs.length; index++) {
          const element = response.result.emailSmsLogs[index];
          if (element.isOtherPariticipant) {
            this.otherParticipant.push(element);
          } else {
            this.participants.push(element);
          }
        }

        this.isCompleteSendSms =  response.result.isCompleteSendSms;
        this.isCompleteSendEmail = response.result.isCompleteSendEmail;
      } else {
        this.toastService.showError(response.message);
      }
    });

    this.subscriptions.push(subScription);
  }
}
