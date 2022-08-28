using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
  public class ScheduleTemplateDto
  {
    public int ScheduleId { get; set; }
    public string ScheduleTime { get; set; }

    public string ScheduleContent { get; set; }

    public string ScheduleTitle { get; set; }

    public string OtherLocation { get; set; }

    public string ScheduleLocation { get; set; }

    public int? Id { get; set; }

    public int? LocationId { get; set; }

    public string OfficerName { get; set; }

    public string OfficerPosition { get; set; }

    public string Participants { get; set; }

    public string ParticipantDisplay { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public string OtherHost { get; set; }

    public bool IsOtherParticipant { get; set; }

    public bool IsSendEmail { get; set; }

    public bool ISendSMS { get; set; }

    public string ReasonChangeSchedule { get; set; }

    public EScheduleStatus ScheduleStatus { get; set; }

    public int? ScheduleTitleTemplateId { get; set; }

    public string MessageContent { get; set; }

    public string DepartmentPrepare { get; set; }

    public bool IsSendSMSInvite { get; set; }

    public bool IsChangeLocation { get; set; }

    public string FilePath { get; set; }

    public string CloudinaryPublicId { get; set; }

    public int OrganizeId { get; set; }
  }

  public class ScheduleTemplateForAddDto : ScheduleTemplateDto
  {
    public List<OtherParticipantDto> OtherParticipants { get; set; }
    public List<int> UserIds { get; set; }

  }

  public class ScheduleTemplateForDetailDto : ScheduleTemplateDto
  {
    public List<OtherParticipantDto> OtherParticipants { get; set; } = new List<OtherParticipantDto>();
    public List<ParticipantIsSelectedDto> ParticipantIsSelected { get; set; } = new List<ParticipantIsSelectedDto>();
  }

}
