export enum ESettingTypeControl {
  TextBox,
  Select,
  CheckBox,
  Editor,
  File,
}

export class SettingModel {
  settingKey: string;
  settingValue: string;
  settingName: string;
  settingComment: string;
  settingTypeControl: ESettingTypeControl;
  sortOrder: number;
}
