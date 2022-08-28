using System;
using System.Collections.Generic;
using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
    public class ScheduleListDto
    {
        public int ScheduleId { get; set; }

        public DateTime ScheduleDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }

        public string ScheduleTime { get; set; }
        public string ScheduleEndTime { get; set; }

        public string ScheduleContent { get; set; }

        public string ScheduleTitle { get; set; }

        public string OtherLocation { get; set; }

        public int Id { get; set; }
    public string IncludedOfficer { get; set; }

    public LocationDto LocationModel { get; set; }

        public string OfficerName { get; set; }

        public string OtherHost { get; set; }

        public string Participants { get; set; }

        public string ParticipantDisplay { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public EScheduleStatus ScheduleStatus { get; set; }

        public string ReasonChangeSchedule { get; set; }

        public string DepartmentPrepare { get; set; }

        public bool IsSendSMSInvite { get; set; }

        public bool IsChangeLocation { get; set; }
        public bool IsHasFilesAttachment { get; set; }
    }

    public class ScheduleTemplateListDto
    {
        public int ScheduleId { get; set; }
        
        public string ScheduleTime { get; set; }

        public string ScheduleContent { get; set; }

        public string ScheduleTitle { get; set; }

        public string OtherLocation { get; set; }

        public int Id { get; set; }

        public LocationDto LocationModel { get; set; }

        public string OfficerName { get; set; }

        public string OtherHost { get; set; }

        public string Participants { get; set; }

        public string ParticipantDisplay { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public EScheduleStatus ScheduleStatus { get; set; }

        public string ReasonChangeSchedule { get; set; }

        public string DepartmentPrepare { get; set; }

        public bool IsSendSMSInvite { get; set; }

        public bool IsChangeLocation { get; set; }
    }

}
