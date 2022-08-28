import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgbActiveModal, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ScheduleService } from 'src/app/core';
import { BrandNameService } from 'src/app/core/services/brandname.service';
import { LocationService } from 'src/app/core/services/location.service';
import { EScheduleStatus, ScheduleModel } from 'src/app/shared';
import { BrandNameModel } from 'src/app/shared/models/brand-name.model';
import { ILocationModel } from 'src/app/shared/models/location.model';
import { EditorConfig } from 'src/app/shared/models/post.model';

@Component({
  selector: 'app-schedule-change',
  templateUrl: './schedule-change.component.html',
})
export class ScheduleChangeComponent implements OnInit, OnDestroy {
  @Input() scheduleId: number;
  schedule: ScheduleModel = new ScheduleModel();
  lableScheduleDate: string;
  scheduleDate: NgbDateStruct;
  scheduleTime = { hour: 13, minute: 30 };
  locations: ILocationModel[] = [
    {
      id: 0,
      title: '--- Chọn địa điểm ---',
    },
  ];

  config: AngularEditorConfig = EditorConfig;
  minuteStep = 15;

  brandNames: BrandNameModel[] = [];

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    public activeModal: NgbActiveModal,
    private scheduleService: ScheduleService,
    private toastr: ToastrService,
    private locationService: LocationService,
    private brandNameService: BrandNameService
  ) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadLocation();
    this.loadBrandNames();
    this.loadSchedule();
  }

  loadSchedule(): void {
    const scheduleSub = this.scheduleService
      .getScheduleById(this.scheduleId)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.schedule = response.result;
          const date = new Date(this.schedule.scheduleDate);
          this.lableScheduleDate = `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;

          this.scheduleDate = {
            year: date.getFullYear(),
            month: date.getMonth() + 1,
            day: date.getDate(),
          };
          this.splitTime();

          if(this.schedule.brandNameId == 0){
            try{
              this.schedule.brandNameId = this.brandNames[0].brandNameId;
            }catch{}
          }          
        }
      });

    this.subscriptions.push(scheduleSub);
  }

  loadBrandNames(): void {
    this.brandNameService.GetVNPTBrandName().subscribe((res) => {
      if(res.isSuccess){
        var vnpts = res.result as BrandNameModel[];
        vnpts.forEach(x => {
          x.branchName = 'VNPT - ' + x.branchName;
          this.brandNames.push(x);
        })
      }
    });
    this.brandNameService.GetViettelBrandName().subscribe((res) => {
      if(res.isSuccess){
        var viettels = res.result as BrandNameModel[];
        viettels.forEach(x => {
          x.branchName = 'Viettel - ' + x.branchName;
          this.brandNames.push(x);
        })
      }
    });
  }

  loadLocation(): void {
    const locationSub = this.locationService
      .getAllActive()
      .subscribe((response) => {
        if (response.isSuccess) {
          this.locations = this.locations.concat(response.result);
        } else {
          this.toastr.error(
            'Lấy danh sách địa điểm thất bại, vui lòng thử lại!'
          );
        }
      });

    this.subscriptions.push(locationSub);
  }

  closeModal(isSubmit: boolean): void {
    this.activeModal.close({
      submit: isSubmit
    });
  }

  onSubmit(): void {
    if (this.schedule.scheduleStatus === EScheduleStatus.Pending) {
      this.toastr.warning('Lịch này chưa được duyệt, lịch được duyệt mới có thể đời lịch!', 'Dời lịch');
      return;
    }

    this.schedule.scheduleDate = new Date(
      `${this.scheduleDate.month}/${this.scheduleDate.day + 1}/${this.scheduleDate.year}`
    );
    let scheduleTimeHour = this.scheduleTime.hour.toString();
    if (this.scheduleTime.hour < 10) {
      scheduleTimeHour = `0${this.scheduleTime.hour}`;
    }

    let scheduleTimeMinute = this.scheduleTime.minute.toString();
    if (this.scheduleTime.minute < 10) {
      scheduleTimeMinute = `0${this.scheduleTime.minute}`;
    }
    this.schedule.scheduleTime = `${scheduleTimeHour}:${scheduleTimeMinute}`;

    if (this.schedule.id === 0) {
      this.schedule.id = null;
    }

    if(this.schedule.locationId ===0) {
      this.schedule.locationId = null;
    }

    if (this.schedule.scheduleTitleTemplateId === 0) {
      this.schedule.scheduleTitleTemplateId = null;
    }    

    console.log(this.schedule);
    // Change
    const createSub = this.scheduleService
    .change(this.schedule)
    .subscribe((response) => {
      if (response.isSuccess) {
        this.toastr.success('Bạn đã DỜI lịch họp thành công!', 'Dời lịch');
        this.closeModal(true);
      } else {
        this.toastr.error(
          'DỜI lịch họp thất bại, vui lòng thử lại!'
        );
      }
    });

    this.subscriptions.push(createSub);
  }

  private splitTime(): void {
    if (this.schedule) {
      const timeIsSplit = this.schedule.scheduleTime.split(':');
      this.scheduleTime = {
        // tslint:disable-next-line:radix
        hour: Number.parseInt(timeIsSplit[0].toString()),
        // tslint:disable-next-line:radix
        minute: Number.parseInt(timeIsSplit[1].toString()),
      };
    }
  }

}
