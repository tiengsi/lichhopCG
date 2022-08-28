import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import {
  IOtherParticipantSelected,
  IParticipantIsSelected,
} from './participant.model';
import { ScheduleTemplateModel } from './schedule-template.model';

export class ScheduleModel {
  scheduleId: number;
  scheduleDate: Date;
  scheduleTime: string;
  scheduleEndDate: Date;
  scheduleEndTime: string;
  scheduleContent: string;
  locationId?: number;
  id: number;
  includedOfficer: string;
  otherLocation: string;
  scheduleLocation: string;
  createdDate: Date;
  isActive: boolean;
  officerName: string;
  userIds: number[];
  otherHost: string;
  groupMeetingIds: number[];
  iSendSMS: boolean;
  isSendEmail: boolean;
  participantDisplay: string;
  isOtherParticipant: boolean;
  otherParticipants: IOtherParticipantSelected[];
  participantIsSelected?: IParticipantIsSelected[];
  scheduleStatus: EScheduleStatus;
  reasonChangeSchedule: string;
  scheduleTitle: string;
  scheduleTitleTemplateId: number;
  isSendSMSInvite?: boolean;
  messageContent: string;
  departmentPrepare: string;
  isChangeLocation: boolean;
  filePath: string;
  cloudinaryPublicId: string;
  scheduleFilesAttachment: ScheduleFilesAttachment[] = [];
  organizeId?: number;
  meetingLink: string;
  isAutoSendAtScheduledTime: boolean;
  brandNameId?: number;
  scheduleTimeForScheduleJob?: Date;
  sendSMSFlagForJob?: boolean;

  constructor() {
    this.scheduleDate = null;
    this.scheduleTime = null;
    this.scheduleEndDate = null;
    this.scheduleEndTime = null;
    this.scheduleContent = null;
    this.scheduleTitle = null;
    this.otherLocation = '';
    this.scheduleLocation = '';
    this.otherHost = '';
    this.locationId = 0;
    this.id = 0;
    this.includedOfficer = '';
    this.isActive = false;
    this.createdDate = new Date();
    this.userIds = [];
    this.groupMeetingIds = [];
    this.iSendSMS = false;
    this.isSendEmail = false;
    this.participantDisplay = null;
    this.isOtherParticipant = false;
    this.otherParticipants = [];
    this.reasonChangeSchedule = null;
    this.scheduleTitleTemplateId = 0;
    this.departmentPrepare = null;
    this.isChangeLocation = false;
    this.filePath = '';
    this.cloudinaryPublicId = '';
    this.scheduleFilesAttachment = [];
    this.isAutoSendAtScheduledTime = false;
    this.scheduleTimeForScheduleJob = null;
    this.sendSMSFlagForJob = false;
  }
}

export class ScheduleGroupByHostModel {
  officerName: string;
  officerPosition: string;
  schedules: ScheduleModel[];
}

export enum EScheduleStatus {
  Pending,
  Approve,
  Pause, // Hoãn lịch
  Changed, // Dời lịch
  Release, // Phát hành
}

export enum EScheduleAddType {
  Morning = 1,
  Afternoon = 2,
  Evening = 3,
}

export class ScheduleByWeekModel {
  dayOfWeek: string;
  morning: ScheduleModel[];
  afternoon: ScheduleModel[];
  evening: ScheduleModel[];

  constructor() {
    this.morning = [];
    this.evening = [];
    this.afternoon = [];
  }
}

export class ScheduleTemplateByWeekModel {
  dayOfWeek: string;
  morning: ScheduleTemplateModel[];
  afternoon: ScheduleTemplateModel[];
  evening: ScheduleTemplateModel[];

  constructor() {
    this.morning = [];
    this.evening = [];
    this.afternoon = [];
  }
}

export class AuditScheduleModel {
  id: number;
  changeFrom: string;
  changeTo: string;
  changeDate: Date;
  scheduleId: number;

  constructor() {
    this.id = 0;
    this.changeFrom = '';
    this.changeTo = '';
    this.changeDate = new Date();
    this.scheduleId = 0;
  }
}

export class ScheduleFilesAttachment {
  id?: number;
  scheduleId: number;
  fileName: string;
  filePath: string;
  cloudinaryPublicId: string;
  notationNumber: string;
  releaseDate: Date;
  quote: string;
  releaseDateStr?: NgbDateStruct;
  isShare: boolean;
  constructor() {
    this.id = 0;
    this.filePath = '';
    this.scheduleId = 0;
    this.fileName = '';
    this.cloudinaryPublicId = '';
    this.notationNumber = '';
    this.releaseDate = new Date();
    this.quote = '';
    this.isShare = true;
    this.releaseDateStr = {
      year: this.releaseDate.getFullYear(),
      month: this.releaseDate.getMonth() + 1,
      day: this.releaseDate.getDate(),
    };
  }
}
