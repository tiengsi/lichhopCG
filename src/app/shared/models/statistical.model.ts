export interface IStatisticalDay {
  totalSchedule: number;
  totalScheduleHasLetter: number;
  totalScheduleNoLetter: number;
  chartData: IStatisticalChartRow[];
}

export interface IStatisticalChartRow{
  name: string;
  series: IStatisticalChartSeries[];
}

export interface IStatisticalChartSeries{
  name: string;
  value: number;
}
