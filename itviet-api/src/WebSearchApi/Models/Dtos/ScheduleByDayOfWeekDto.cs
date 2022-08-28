using System;
using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
    public class ScheduleByDayOfWeekDto
    {
        public string DayOfWeek { get; set; }

        public List<ScheduleListDto> Morning { get; set; }

        public List<ScheduleListDto> Afternoon { get; set; }

        public List<ScheduleListDto> Evening { get; set; }

        public ScheduleByDayOfWeekDto()
        {
            Morning = new List<ScheduleListDto>();
            Afternoon = new List<ScheduleListDto>();
            Evening = new List<ScheduleListDto>();
        }
    }

    public class AllDayOfWeekDto
    {
        public string DayName { get; set; }

        public DateTime Date { get; set; }
    }
}
