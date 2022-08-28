import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { EScheduleStatus, ScheduleModel } from 'src/app/shared';

@Component({
  selector: 'app-schedule-detail-tab',
  templateUrl: './schedule-detail-tab.component.html',
  styleUrls: ['./schedule-detail-tab.component.scss']
})
export class ScheduleDetailTabComponent implements OnInit {

  @Input() scheduleDetail: ScheduleModel;
  @Input() scheduleHistory: any[] = [];
  @Input() scheduleFilesAttachment: any[] =[];

  constructor(public activeModal: NgbActiveModal,  private router: Router,) { }
  get scheduleStatus(): typeof EScheduleStatus {
    return EScheduleStatus;
  }
  ngOnInit(): void {
  }
  closeTab(): void {
    this.router.navigateByUrl('/');
  }
  mapScheduleStatus(value): string {
    if (value === EScheduleStatus.Pending) {
      return 'Đang soạn thảo';
    } else if (value === EScheduleStatus.Approve) {
      return 'Đã duyệt';
    } else if (value === EScheduleStatus.Pause) {
      return 'Lịch bị hoãn';
    } else if (value === EScheduleStatus.Changed) {
      return 'Lịch bị dời';
    } else if (value === EScheduleStatus.Release) {
      return 'Đã phát hành';
    }
  }

}
