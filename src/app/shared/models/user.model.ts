export class UserModel {
  id: number;
  userName: string;
  password: string;
  email: string;
  dptId?: number;
  fullName: string;
  shortName?: string;
  lastLogin: Date;
  phoneNumber: string;
  officerPosition: string;
  roles: string[];
  isHost: boolean;
  isShow: boolean;
  organizeId?: number;
}

export class UserForSelectModel {
  id: number;
  fullName: string;
  officerPosition?: string;
  isSelected?: boolean = false;
  positionName?: string;
}

export interface GetOfficerRequest {
  filter: string;
  sortField: string;
}
