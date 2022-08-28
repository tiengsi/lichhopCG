import {
  IOtherParticipantSelected,
  IParticipantIsSelected,
} from './participant.model';
import { EScheduleStatus } from './schedule.model';

export class ScheduleTemplateModel {
  scheduleId: number;
  scheduleTime: string;
  scheduleContent: string;
  locationId?: number;
  id: number;
  otherLocation: string;
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
  organizeId?: number;

  constructor() {
    this.scheduleTime = null;
    this.scheduleContent = null;
    this.scheduleTitle = null;
    this.otherLocation = '';
    this.otherHost = '';
    this.locationId = 0;
    this.id = 0;
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
    this.participantIsSelected = [];
    this.scheduleStatus = EScheduleStatus.Pending;
  }
}
