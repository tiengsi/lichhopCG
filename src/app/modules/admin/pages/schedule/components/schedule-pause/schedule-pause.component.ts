import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import * as _ from 'lodash';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import {
  ParticipantService,
  ScheduleService,
  ToastService,
} from 'src/app/core';
import { BrandNameService } from 'src/app/core/services/brandname.service';
import {
  EParticipantType,
  EScheduleStatus,
  IOtherParticipantSelected,
  IParticipant,
  IParticipantIsSelected,
  ScheduleModel,
} from 'src/app/shared';
import { BrandNameModel } from 'src/app/shared/models/brand-name.model';

@Component({
  selector: 'app-schedule-pause',
  templateUrl: './schedule-pause.component.html',
})
export class SchedulePauseComponent implements OnInit, OnDestroy {
  @Input() scheduleId: number;
  schedule: ScheduleModel = new ScheduleModel();
  participantIsSelected: IParticipantIsSelected[] = [];
  otherParticipantSelected: IOtherParticipantSelected[] = [
    {
      name: null,
      email: null,
      phoneNumber: null,
    },
  ];
  scheduleDate: string;
  listParticipant: IParticipant[];
  brandNames: BrandNameModel[] = [];

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    public activeModal: NgbActiveModal,
    private scheduleService: ScheduleService,
    private participantService: ParticipantService,
    private toastr: ToastrService,
    private brandNameService: BrandNameService
  ) {}

  ngOnDestroy(): void {
    this.subscriptions.forEach((el) => el.unsubscribe());
  }

  ngOnInit(): void {
    this.loadBrandNames();
    this.loadSchedule();
    this.loadParticipant();
  }

  get participantType(): typeof EParticipantType {
    return EParticipantType;
  }

  loadSchedule(): void {
    const scheduleSub = this.scheduleService
      .getScheduleById(this.scheduleId)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.schedule = response.result;
          if(this.schedule.brandNameId == 0){
            try{
              this.schedule.brandNameId = this.brandNames[0].brandNameId
            }catch{}
          }
          if (this.schedule.otherParticipants !== null) {
            this.otherParticipantSelected = this.schedule.otherParticipants;
            if (this.otherParticipantSelected.length > 0) {
              this.schedule.isOtherParticipant = true;
            }

            this.participantIsSelected = this.schedule.participantIsSelected;
          }

          const date = new Date(this.schedule.scheduleDate);
          this.scheduleDate = `${date.getDate()}/${
            date.getMonth() + 1
          }/${date.getFullYear()}`;
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

  addOtherParticipant(): void {
    this.otherParticipantSelected.push({
      name: null,
      email: null,
      phoneNumber: null,
    });
  }

  loadParticipant(): void {
    this.participantService.getAllForSelect().subscribe((response) => {
      if (response.isSuccess) {
        this.listParticipant = response.result;
      } else {
        this.toastr.error(
          'L???y danh s??ch nho??m ca??n b???? th????t ba??i, vui l??ng th??? l???i!'
        );
      }
    });
  }

  onChange(participantId: string): void {
    const subChange = this.participantService
      .chooseParticipant(participantId)
      .subscribe((response) => {
        if (response.isSuccess) {
          // kh??ng add item ???? t???n t???i trong participantIsSelected
          const participants = response.result;
          participants.forEach((item) => {
            const filter = _.filter(this.participantIsSelected, {
              participantId: item.participantId,
              receiverName: item.receiverName,
            });

            if (filter.length === 0) {
              this.participantIsSelected.push(item);
            }
          });
        }
      });

    this.subscriptions.push(subChange);
  }

  deleteParticipant(item: IParticipantIsSelected): void {
    const index = this.participantIsSelected.indexOf(item);
    this.participantIsSelected.splice(index, 1);
  }

  deleteOtherParticipant(item: IOtherParticipantSelected): void {
    const index = this.otherParticipantSelected.indexOf(item);
    this.otherParticipantSelected.splice(index, 1);
  }

  onSubmit(): void {
    if (this.schedule.scheduleStatus === EScheduleStatus.Pending) {
      this.toastr.warning(
        'L???ch n??y ch??a ???????c duy???t, l???ch ???????c duy???t m???i c?? th??? ho??n l???ch!',
        'Ho??n l???ch'
      );
      return;
    }

    if (this.schedule.scheduleStatus === EScheduleStatus.Pause) {
      this.toastr.warning('L???ch n??y ???? b??? ho??n tr?????c ???? r???i!', 'Ho??n l???ch');
      return;
    }

    const participantSelected = [];
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0; i < this.participantIsSelected.length; i++) {
      if (this.participantIsSelected[i].participantId !== -99) {
        participantSelected.push(this.participantIsSelected[i].participantId);
      }
    }

    this.schedule.userIds = participantSelected;

    this.schedule.otherParticipants = this.otherParticipantSelected;

    // pause
    const createSub = this.scheduleService
      .pause(this.schedule)
      .subscribe((response) => {
        if (response.isSuccess) {
          this.toastr.success('B???n ???? HO??N l???ch h???p th??nh c??ng!', 'Ho??n l???ch');
          this.closeModal(true);
        } else {
          this.toastr.error('Ho??n l???ch h???p th???t b???i, vui l??ng th??? l???i!');
        }
      });

    this.subscriptions.push(createSub);
  }

  closeModal(isSubmit: boolean): void {
    this.activeModal.close({
      submit: isSubmit,
    });
  }
}
