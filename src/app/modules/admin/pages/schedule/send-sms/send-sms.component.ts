import { Component, OnDestroy, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ParticipantService, ScheduleService } from 'src/app/core';
import {
  EParticipantType,
  IParticipant,
  IParticipantIsSelected,
  IReceiver,
  ISms,
} from 'src/app/shared';

@Component({
  selector: 'app-send-sms',
  templateUrl: './send-sms.component.html',
})
export class SendSmsComponent implements OnInit, OnDestroy {
  smsForm: ISms = {
    content: null,
    phoneNumber: [],
    organizeId: null
  };
  
  listParticipant: IParticipant[];
  receivers: IReceiver[] = [];

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private participantService: ParticipantService,
    private toastr: ToastrService,
    private scheduleService: ScheduleService
  ) {}

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadParticipant();
  }

  get participantType(): typeof EParticipantType {
    return EParticipantType;
  }

  loadParticipant(): void {
    this.participantService.getAllForSelect().subscribe((response) => {
      if (response.isSuccess) {
        this.listParticipant = response.result;
      } else {
        this.toastr.error(
          'Lấy danh sách nhóm cán bộ thất bại, vui lòng thử lại!'
        );
      }
    });
  }

  onChange(participantId: string): void {
    const subChange = this.participantService
      .chooseReceiver(participantId)
      .subscribe((response) => {
        if (response.isSuccess) {
          // không add item đã tồn tại trong participantIsSelected
          const receivers = response.result;
          receivers.forEach((item) => {
            const filter = _.filter(this.receivers, {
              phoneNumber: item.phoneNumber,
              receiverName: item.receiverName,
            });

            if (filter.length === 0) {
              this.receivers.push(item);
            }
          });
        }
      });

    this.subscriptions.push(subChange);
  }

  addReceiver(): void {
    this.receivers.push({
      receiverName: null,
      phoneNumber: null,
    });
  }

  deleteReceiver(item: IReceiver): void {
    const index = this.receivers.indexOf(item);
    this.receivers.splice(index, 1);
  }

  onSubmit(): void {
    const listPhoneNumber = [];
    // tslint:disable-next-line:prefer-for-of
    for (let index = 0; index < this.receivers.length; index++) {
      const element = this.receivers[index];
      listPhoneNumber.push(element.phoneNumber);
    }
    var orgnaizeId = JSON.parse(localStorage.getItem('app-schedule-token'))?.organizeId;
    if(orgnaizeId == undefined || orgnaizeId == null) return; 
    const body: ISms = {
      phoneNumber: listPhoneNumber,
      content: this.smsForm.content,
      organizeId: orgnaizeId
    };

    const submit = this.scheduleService.sendSms(body).subscribe((response) => {
      if (response.isSuccess) {
        this.toastr.success('Bạn đã gửi tin nhắn thành công!');
      } else {
        this.toastr.error(response.message);
      }
    });

    this.subscriptions.push(submit);
  }
}
