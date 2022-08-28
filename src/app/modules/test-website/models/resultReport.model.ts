import { UserModel } from "src/app/shared";

export class ScheduledResultReportModel {
    scheduledResultReportId: number;
    userId: number;
    user: UserModel
    path: string;
    reportContent: string;
    reportTime: Date;
    scheduleId: number | undefined
}