import { IDepartmentModel } from './department.model';

export interface IUserForListModel {
  id: number;
  userName: string;
  password: string;
  email: string;
  fullName: string;
  lastLogin: Date;
  phoneNumber: string;
  officerPosition: string;
  roles: string[];
  department?: IDepartmentModel;
}
