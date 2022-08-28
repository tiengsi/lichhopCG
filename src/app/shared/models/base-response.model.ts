export class BaseResponseModel<T> {
  isSuccess: string;
  statusCode: number;
  message: string;
  result: T;
}
