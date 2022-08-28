import { IOtherParticipantSelected, UserModel } from '..';
import { IDepartmentModel } from './department.model';

export class GroupParticipantForCreateModel {
  id?: number;
  name: string;
  departmentIds?: number[];
  userIds?: number;
  otherParticipants?: IOtherParticipantSelected[];
  organizeId?: number;
}

export class GroupParticipantForListModel {
  groupParticipantId: number;
  groupParticipantName: string;
  departments: IDepartmentModel[];
  users: UserModel[];
  otherParticipants?: IOtherParticipantSelected[];
  createdDate: Date;
  organizeId?: number;
}

export interface IGroupParticipantForSelectScheduleModel {
  id: number;
  name: string;
}

