using System;
using System.Collections.Generic;
using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
  public class ScheduleDto
  {
    public int ScheduleId { get; set; }

    public DateTime ScheduleDate { get; set; }

    public string ScheduleTime { get; set; }
    public DateTime? ScheduleEndDate { get; set; }
    public string ScheduleEndTime { get; set; }

    public string ScheduleContent { get; set; }

    public string ScheduleTitle { get; set; }

    public string OtherLocation { get; set; }

    public string ScheduleLocation { get; set; }

    public int? Id { get; set; }
    public string IncludedOfficer { get; set; }

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
    public bool SendSMSFlagForJob { get; set; }

    public bool IsChangeLocation { get; set; }

    public string FilePath { get; set; }

    public string CloudinaryPublicId { get; set; }
    public int OrganizeId { get; set; }
    public string MeetingLink { get; set; }
    public bool IsAutoSendAtScheduledTime { get; set; }
    public int? BrandNameId { get; set; }
  }

  public class ScheduleForAddDto : ScheduleDto
  {
    public List<OtherParticipantDto> OtherParticipants { get; set; }
    public List<AuditScheduleDto> AuditSchedules { get; set; }
    public List<ScheduleFilesAttachmentDto> ScheduleFilesAttachment { get; set; }
    public List<int> UserIds { get; set; }

    public DateTime? ScheduleTimeForScheduleJob { get; set; }

  }
  public class ScheduleFilesAttachmentDto
  {    
    public int Id { get; set; }   
    public int ScheduleId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string CloudinaryPublicId { get; set; }
    public string NotationNumber { get; set; }
    public bool IsShare { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Quote { get; set; }    
  }
  public class ScheduleForDetailDto : ScheduleDto
  {
    public List<OtherParticipantDto> OtherParticipants { get; set; } = new List<OtherParticipantDto>();
    public List<AuditScheduleDto> AuditSchedules { get; set; } = new List<AuditScheduleDto>();

    public List<ParticipantIsSelectedDto> ParticipantIsSelected { get; set; } = new List<ParticipantIsSelectedDto>();
    public List<ScheduleFilesAttachmentDto> ScheduleFilesAttachment { get; set; } = new List<ScheduleFilesAttachmentDto>();
  }

  public class ScheduleGroupByHost
  {
    public string OfficerName { get; set; }

    public List<ScheduleDto> Schedules { get; set; }
  }

  public class ScheduleReleasePayload
  {
    public string StartDate { get; set; }

    public string EndDate { get; set; }
  }

  public class MessageContentPayload
  {
    public string MessageContent { get; set; }

    public int ScheduleId { get; set; }
  }
}
