export interface IDepartmentModel {
  id?: number;
  name: string;
  shortName?: string;
  adress?: string;
  email?: string;
  phoneNumber?: string;
  fax?: string;
  parentId?: number;
  isActive?: boolean;
  userRepresentative?: number[];
  sortOrder?: number;
  representative?: string;
  representativeId?: number;
  organizeId?: number;
}
