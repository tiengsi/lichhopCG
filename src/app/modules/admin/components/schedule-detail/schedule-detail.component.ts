import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IOtherParticipantSelected, IParticipantIsSelected, ScheduleModel } from 'src/app/shared';

@Component({
  selector: 'app-schedule-detail',
  templateUrl: './schedule-detail.component.html',
})
export class ScheduleDetailComponent implements OnInit, OnDestroy {
  @Input() schedule: ScheduleModel;
  participantIsSelected: IParticipantIsSelected[] = [];
  otherParticipantSelected: IOtherParticipantSelected[] = [];
  private subscriptions: Subscription[] = [];

  constructor() { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit() {
    this.participantIsSelected = this.schedule.participantIsSelected;
    this.otherParticipantSelected = this.schedule.otherParticipants;
  }

}
