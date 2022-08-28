export class ScheduledResultDocumentModel {
    scheduledResultDocumentId: number;
    title: string;
    status: boolean;
    path: string;
    updatedDate: Date;
    scheduleId: number | undefined;
}