export class CategoryModel {
  categoryId: number;
  categoryName: string;
  typeCode: ECategoryType;
  categoryCode: string;
  link: string;
  ParentId?: number;
  isActive?: boolean;
}

export class CategoryForSelectModel {
  categoryId: number;
  categoryName: string;
}

export enum ECategoryType {
  Post,
  Article,
  Link,
  Schedule,
}
