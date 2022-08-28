// Angular
import { Pipe, PipeTransform } from '@angular/core';
import { EScheduleStatus } from '../models/schedule.model';

/**
 * Returns only first letter of string
 */
@Pipe({
  name: 'scheduleStatus'
})
export class ScheduleStatusPipe implements PipeTransform {

  /**
   * Transform
   *
   * @param value: any
   * @param args: any
   */
  transform(value: EScheduleStatus): string {
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
