import { UserForSelectModel } from "src/app/shared";

export class PersonalNoteModel {
    personalNotesId: number | undefined;
    title: string;
    userId: number;
    user: UserForSelectModel | undefined;
    contentNote: string;
    scheduleId: number | undefined;
    updatedDate: Date | undefined;
}