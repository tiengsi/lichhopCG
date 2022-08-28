export interface IEmailLogs {
  sendSmsIsSuccess: boolean;
  sendEmailIsSuccess: boolean;
  fullName: string;
  departmentName: string;
  isOtherPariticipant: boolean;
}

export interface IEmailSmsStatus {
  emailSmsLogs: IEmailLogs[];
  isCompleteSendEmail: boolean;
  isCompleteSendSms: boolean;
}

