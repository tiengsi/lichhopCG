export class SelfScheduleModel{
  title: string;
  content: string;
  start: Date;
  end: Date;    
  constructor(t, c, s, e) {
    this.title = t;
    this.content = c;
    this.start = s;
    this.end = e;
  }
}