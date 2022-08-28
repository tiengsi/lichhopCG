import { UserModel } from "src/app/shared";

export class PersonalScheduleModel{
    personalScheduleId: number | undefined;
    title: string;
    userId: number;
    user: UserModel;
    description: string;
    fromdate: Date;
    toDate: Date;
}