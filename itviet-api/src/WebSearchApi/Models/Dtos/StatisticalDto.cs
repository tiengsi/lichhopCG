using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
    public class StatisticalDayDto
    {
        public int TotalSchedule { get; set; }
        public int TotalScheduleHasLetter { get; set; }
        public int TotalScheduleNoLetter { get; set; }
        public List<StatisticalChartRow> ChartData { get; set; }
    }

    public class StatisticalChartRow
    {
        public string Name { get; set; }

        public List<StatisticalChartSeries> Series { get; set; }
    }

    public class StatisticalChartSeries
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
