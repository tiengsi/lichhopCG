export interface IParticipant {
  name: string;
  shortName: string;
  id: string;
  participantType: EParticipantType;
}

export enum EParticipantType {
  Group,
  Department,
  User,
}

export interface IParticipantIsSelected {
    departmentName: string;
    participantId: number;
    receiverName: string;
}

export interface IOtherParticipantSelected {
  otherParticipantId?: number;
  name: string;
  email: number;
  phoneNumber: string;
}
