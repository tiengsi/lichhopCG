export class PageResponseModel<T> {
  items: T;
  totalCount: number;
  page: number;
  count: number;

  constructor(items: T, totalCount: number = 0, page: number = 0, count: number = 0) {
    this.items = items;
    this.totalCount = totalCount;
    this.page = page;
    this.count = count;
  }
}
