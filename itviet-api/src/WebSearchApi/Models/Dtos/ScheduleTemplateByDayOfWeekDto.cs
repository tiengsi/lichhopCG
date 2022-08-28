using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
    public class ScheduleTemplateByDayOfWeekDto
    {
        public string DayOfWeek { get; set; }

        public List<ScheduleTemplateListDto> Morning { get; set; }

        public List<ScheduleTemplateListDto> Afternoon { get; set; }

        public List<ScheduleTemplateListDto> Evening { get; set; }

        public ScheduleTemplateByDayOfWeekDto()
        {
            Morning = new List<ScheduleTemplateListDto>();
            Afternoon = new List<ScheduleTemplateListDto>();
            Evening = new List<ScheduleTemplateListDto>();
        }
    }    
}
