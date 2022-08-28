using System;
using System.Collections.Generic;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Helpers.Extensions
{
  public static class EntityExtensions
  {
    public static void UpdatePost(this PostModel model, PostDto modelVM)
    {
      model.PostId = modelVM.PostId;
      model.Title = modelVM.Title;
      model.Description = modelVM.Description;
      model.FilterTitle = Ultil.FilterChar(modelVM.Title);
      model.IsActive = modelVM.IsActive;
      model.ImagePath = modelVM.ImagePath;
      model.Body = modelVM.Body;
      model.CategoryId = modelVM.CategoryId;
      model.CreatedDate = DateTime.Now;
      model.UpdatedDate = DateTime.Now;
      model.CloudinaryPublicId = modelVM.PublicId;
    }

    public static void UpdateDepartment(this DepartmentModel model, DepartmentDto modelVM)
    {
      model.Name = modelVM.Name;
      model.Adress = modelVM.Adress;
      model.Email = modelVM.Email;
      model.IsActive = modelVM.IsActive;
      model.Fax = modelVM.Fax;
      model.PhoneNumber = modelVM.PhoneNumber;
      model.ParentId = modelVM.ParentId == 0 ? null : modelVM.ParentId;
      model.UpdatedDate = DateTime.Now;
      model.SortOrder = modelVM.SortOrder;
      model.ShortName = modelVM.ShortName;
      model.OrganizeId = modelVM.OrganizeId;
      model.RepresentativeModels = new List<RepresentativeModel>();
      if (modelVM.UserRepresentative != null && modelVM.UserRepresentative.Count > 0)
      {
        foreach (var item in modelVM.UserRepresentative)
        {
          model.RepresentativeModels.Add(new RepresentativeModel()
          {
            UserId = item,
            DepartmentId = modelVM.Id.Value
          });
        }
      }
    }

    public static void UpdateSetting(this SettingModel model, SettingDto modelVM)
    {
      model.SettingComment = modelVM.SettingComment;
      model.SettingKey = modelVM.SettingKey;
      model.SettingName = modelVM.SettingName;
      model.CreatedDate = DateTime.Now;
      model.SettingTypeControl = modelVM.SettingTypeControl;
      model.SettingValue = modelVM.SettingValue;
    }

    public static void ConvertToScheduleModel(this ScheduleModel model, ScheduleDto modelVM)
    {
      model.ScheduleContent = modelVM.ScheduleContent;
      model.ScheduleDate = modelVM.ScheduleDate;
      model.ScheduleId = modelVM.ScheduleId;
      model.CreatedDate = DateTime.Now;
      model.UpdatedDate = DateTime.Now;
      model.ScheduleTime = modelVM.ScheduleTime;
      model.Id = modelVM.Id;
      model.Participants = modelVM.Participants;
      model.OtherLocation = modelVM.OtherLocation;
      model.LocationId = modelVM.LocationId;
      model.OtherHost = modelVM.OtherHost;
      model.IsActive = modelVM.IsActive;
      model.ISendSMS = modelVM.ISendSMS;
      model.IsSendEmail = modelVM.IsSendEmail;
      model.ParticipantDisplay = modelVM.ParticipantDisplay;
      model.ReasonChangeSchedule = modelVM.ReasonChangeSchedule;
      model.ScheduleStatus = modelVM.ScheduleStatus;
      model.ScheduleTitleTemplateId = modelVM.ScheduleTitleTemplateId;
      model.ScheduleTitle = modelVM.ScheduleTitle;
      model.MessageContent = modelVM.MessageContent;
      model.DepartmentPrepare = modelVM.DepartmentPrepare;
      model.IsSendSMSInvite = modelVM.IsSendSMSInvite;
      model.FilePath = modelVM.FilePath;
      model.CloudinaryPublicId = modelVM.CloudinaryPublicId;
      model.MeetingLink = modelVM.MeetingLink;
      model.IsAutoSendAtScheduledTime = modelVM.IsAutoSendAtScheduledTime;
      model.BrandNameId = modelVM.BrandNameId;

      model.ScheduleEndDate = modelVM.ScheduleEndDate;
      model.ScheduleEndTime = modelVM.ScheduleEndTime;

      model.IncludedOfficer = modelVM.IncludedOfficer;
    }

    public static void UpdateScheduleTemplate(this ScheduleTemplateModel model, ScheduleTemplateDto modelVM)
    {
      model.ScheduleContent = modelVM.ScheduleContent;
      //            model.ScheduleDate = modelVM.ScheduleDate;
      model.ScheduleId = modelVM.ScheduleId;
      model.CreatedDate = DateTime.Now;
      model.UpdatedDate = DateTime.Now;
      model.ScheduleTime = modelVM.ScheduleTime;
      model.Id = modelVM.Id;
      model.Participants = modelVM.Participants;
      model.OtherLocation = modelVM.OtherLocation;
      model.LocationId = modelVM.LocationId;
      model.OtherHost = modelVM.OtherHost;
      model.IsActive = modelVM.IsActive;
      model.ISendSMS = modelVM.ISendSMS;
      model.IsSendEmail = modelVM.IsSendEmail;
      model.ParticipantDisplay = modelVM.ParticipantDisplay;
      model.ReasonChangeSchedule = modelVM.ReasonChangeSchedule;
      model.ScheduleStatus = modelVM.ScheduleStatus;
      model.ScheduleTitleTemplateId = modelVM.ScheduleTitleTemplateId;
      model.ScheduleTitle = modelVM.ScheduleTitle;
      model.MessageContent = modelVM.MessageContent;
      model.DepartmentPrepare = modelVM.DepartmentPrepare;
      model.IsSendSMSInvite = modelVM.IsSendSMSInvite;
      model.FilePath = modelVM.FilePath;
      model.CloudinaryPublicId = modelVM.CloudinaryPublicId;
      model.OrganizeId = modelVM.OrganizeId;
    }

    public static void UpdateLocation(this LocationModel model, LocationDto modelVM)
    {
      model.IsActive = modelVM.IsActive;
      model.Title = modelVM.Title;
      model.UpdatedDate = DateTime.Now;
      model.Id = modelVM.Id;
    }

    public static void UpdateGroupParticipant(this GroupParticipantModel model, GroupParticipantForCreateDto modelVM)
    {
      model.GroupParticipantName = modelVM.Name;
      model.OrganizeId = modelVM.OrganizeId;
      var listGroupDepartment = new List<GroupDepartmentModel>();

      if (modelVM.DepartmentIds != null)
      {
        foreach (var item in modelVM.DepartmentIds)
        {
          listGroupDepartment.Add(new GroupDepartmentModel()
          {
            DepartmentId = item,
          });
        }
      }

      model.GroupDepartmentModels = listGroupDepartment;

      var listUserPariticipant = new List<UserParticipantModel>();

      if (modelVM.UserIds != null)
      {
        foreach (var item in modelVM.UserIds)
        {
          listUserPariticipant.Add(new UserParticipantModel()
          {
            UserId = item,
          });
        }
      }

      model.UserParticipantModels = listUserPariticipant;

      var listOrtherParticipant = new List<OtherParticipantModel>();

      if (modelVM.OtherParticipants != null)
      {
        foreach (var item in modelVM.OtherParticipants)
        {
          listOrtherParticipant.Add(new OtherParticipantModel()
          {
            Name = item.Name,
            Email = item.Email,
            PhoneNumber = item.PhoneNumber,
            ScheduleId=item.ScheduleId
          });
        }

        model.OtherParticipantModels = listOrtherParticipant;
      }
    }

    public static void UpdateOtherParticipant(this OtherParticipantModel model, OtherParticipantDto modelVM)
    {
      model.OtherParticipantId = modelVM.OtherParticipantId;
      model.ScheduleId = modelVM.ScheduleId;
      model.PhoneNumber = modelVM.PhoneNumber;
      model.Email = modelVM.Email;
      model.Name = modelVM.Name;
    }

    public static void UpdateOtherParticipantTemplate(this OtherParticipantTemplateModel model, OtherParticipantDto modelVM)
    {
      model.OtherParticipantId = modelVM.OtherParticipantId;
      model.ScheduleId = modelVM.ScheduleId;
      model.PhoneNumber = modelVM.PhoneNumber;
      model.Email = modelVM.Email;
      model.Name = modelVM.Name;
    }
  }
}
