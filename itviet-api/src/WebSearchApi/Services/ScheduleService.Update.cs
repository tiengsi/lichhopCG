using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Helpers.Domains;
using WebApi.Helpers.Exceptions;
using WebApi.Helpers.Extensions;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Enums;

namespace WebApi.Services
{
  public partial interface IScheduleService
  {
    Task<FunctionResult> UpdateScheduleByIdAsync(ScheduleForAddDto model);
    Task<int> UpdateStatusOfScheduleByIdAsync(int scheduleId);
    Task UpdateMessageContent(MessageContentPayload payload);
    Task<int> UpdatePersonalNotesAsync(PersonalNotesDto model);
    Task<int> UpdateScheduledResultDocumentAsync(ScheduledResultDocumentDto model);

    Task<int> UpdateScheduledResultReportAsync(ScheduledResultReportDto model);
    Task<int> UpdateScheduleTemplateAsync(ScheduleTemplateForAddDto model);
    Task<int> UpdatePersonalScheduleAsync(PersonalScheduleDto model);

    Task<int> UpdateIsShareFieldScheduleFileAttachmentListAsync(List<ScheduleFileAttachmentShareDto> modelList);
  }

  public partial class ScheduleService : IScheduleService
  {
    public async Task<FunctionResult> UpdateScheduleByIdAsync(ScheduleForAddDto model)
    {
      var result = new FunctionResult();
      result= CheckRequireFieldWhenUpdateSchedule(model);
      if (result.IsSuccess==false) return result;

      var foundItem = await _scheduleRepository.GetSingleById(model.ScheduleId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Not found schedule {model.ScheduleId}");
        result.AddError($"Không tìm thấy lịch họp {model.ScheduleId}");
        return result;
      }

      var auditSchedules = new List<AuditScheduleModel>();

      if (foundItem.ScheduleDate.Date != model.ScheduleDate.Date)
      {
        var auditSchedule = new AuditScheduleModel()
        {
          ChangeFrom = string.Empty,
          ChangeTo = string.Format("Ngày họp đã thay đổi từ {0} sang {1}", foundItem.ScheduleDate.ToString("dd/MM/yyyy"), model.ScheduleDate.ToString("dd/MM/yyyy")),
          ChangeDate = DateTime.Now,
          ScheduleId = foundItem.ScheduleId
        };
        auditSchedules.Add(auditSchedule);
      }

      if (foundItem.ScheduleTime != model.ScheduleTime)
      {
        var auditSchedule = new AuditScheduleModel()
        {
          ChangeFrom = string.Empty,
          ChangeTo = string.Format("Giờ họp đã thay đổi từ {0} sang {1}", foundItem.ScheduleTime, model.ScheduleTime),
          ChangeDate = DateTime.Now,
          ScheduleId = foundItem.ScheduleId
        };
        auditSchedules.Add(auditSchedule);
      }


      // delete old participants  and old other participants
      await DeleteParticipantsAndOtherParticipantsWhenUpdateScheduleAsync(model);
      foundItem.ConvertToScheduleModel(model);
      foundItem=AddParticipantsAndOtherParticipantsWhenUpdateSchedule(model, foundItem);


      try
      {
        await AddOrUpdateFilesAttachment(foundItem, model);
      }
      catch (Exception ex)
      {
        _logger.LogError("Update file attachment: ", ex);
      }

      _scheduleRepository.Update(foundItem);
      _unitOfWork.Commit();

      // gửi thư mời sau khi update
      if (model.SendSMSFlagForJob)
      {
        // send sms / email
        var sendemailSuccess = await _jobScheduleService.CreateManualJob(foundItem.ScheduleId, EJobScheduleType.EMAIL, EScheduleStatus.Approve);
        var sendSmsSuccess = await _jobScheduleService.CreateManualJob(foundItem.ScheduleId, EJobScheduleType.SMS, EScheduleStatus.Approve);


        //if (sendemailSuccess > 0)
        //{
        //  var auditSchedule = new AuditScheduleModel()
        //  {
        //    ChangeFrom = string.Empty,
        //    ChangeTo = "Đã gửi thư mời",
        //    ChangeDate = DateTime.Now,
        //    ScheduleId = foundItem.ScheduleId
        //  };
        //  auditSchedules.Add(auditSchedule);
        //}
        //if (sendSmsSuccess > 0)
        //{
        //  var auditSchedule = new AuditScheduleModel()
        //  {
        //    ChangeFrom = string.Empty,
        //    ChangeTo = "Đã gửi tin nhắn",
        //    ChangeDate = DateTime.Now,
        //    ScheduleId = foundItem.ScheduleId
        //  };
        //  auditSchedules.Add(auditSchedule);
        //}
      }
      else
      {
        if (model.IsAutoSendAtScheduledTime==true)
        {
          var scheduleTime = DateTime.Now;
          if (model.ScheduleTimeForScheduleJob!=null) scheduleTime= (DateTime)model.ScheduleTimeForScheduleJob;

          var sendemailSuccess = await _jobScheduleService.CreateAutoJob(foundItem.ScheduleId, EJobScheduleType.EMAIL, EScheduleStatus.Approve, scheduleTime);
          var sendSmsSuccess = await _jobScheduleService.CreateAutoJob(foundItem.ScheduleId, EJobScheduleType.SMS, EScheduleStatus.Approve, scheduleTime);

        }
      }
      var auditScheduleUpdate = new AuditScheduleModel()
      {
        ChangeFrom = string.Empty,
        ChangeTo = "Lịch đã được sửa",
        ChangeDate = DateTime.Now,
        ScheduleId = foundItem.ScheduleId
      };
      auditSchedules.Add(auditScheduleUpdate);

      _auditScheduleRepository.AddMulti(auditSchedules);
      _unitOfWork.Commit();

      result.SetData(foundItem.ScheduleId);
      return result;
    }

    public async Task<int> UpdateStatusOfScheduleByIdAsync(int scheduleId)
    {
      var foundItem = await _scheduleRepository.GetSingleById(scheduleId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Not found schedule {scheduleId}");
        throw new BusinessException($"Không tìm thấy lịch họp {scheduleId}", StatusCodes.Status404NotFound);
      }


      foundItem.IsActive = !foundItem.IsActive;
      var auditScheduleUpdate = new AuditScheduleModel()
      {
        ChangeFrom = string.Empty,
        ChangeTo = "Đã được phát hành",
        ChangeDate = DateTime.Now,
        ScheduleId = foundItem.ScheduleId
      };

      _auditScheduleRepository.Add(auditScheduleUpdate);
      _scheduleRepository.Update(foundItem);
      _unitOfWork.Commit();

      return foundItem.ScheduleId;
    }

    public async Task UpdateMessageContent(MessageContentPayload payload)
    {
      var foundItem = await _scheduleRepository.GetSingleById(payload.ScheduleId);
      if (foundItem == null)
      {
        _logger.LogWarning($"scheduleId {payload.ScheduleId} is not found");
        throw new BusinessException($"scheduleId {payload.ScheduleId} không tìm thấy", StatusCodes.Status404NotFound);
      }

      foundItem.MessageContent = payload.MessageContent;
      _scheduleRepository.Update(foundItem);
      _unitOfWork.Commit();

      // send sms / email
      await _jobScheduleService.CreateManualJob(payload.ScheduleId, EJobScheduleType.EMAIL, EScheduleStatus.Approve);
      await _jobScheduleService.CreateManualJob(payload.ScheduleId, EJobScheduleType.SMS, EScheduleStatus.Approve);
    }

    public async Task<int> UpdateScheduleTemplateAsync(ScheduleTemplateForAddDto model)
    {
      var foundItem = await _scheduleTemplateRepository.GetSingleById(model.ScheduleId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Not found schedule {model.ScheduleId}");
        throw new BusinessException($"Không tìm thấy lịch họp {model.ScheduleId}", StatusCodes.Status404NotFound);
      }

      // delete old participants
      var oldParticipants = await _participantTemplateRepository.GetMulti(x => x.ScheduleId == model.ScheduleId);
      foreach (var item in oldParticipants)
      {
        _participantTemplateRepository.Delete(item);
      }

      // delete old other participants
      var oldOtherParticipants = await _otherParticipantTemplateRepository.GetMulti(x => x.ScheduleId == model.ScheduleId);
      foreach (var item in oldOtherParticipants)
      {
        _otherParticipantTemplateRepository.Delete(item);
      }

      foundItem.UpdateScheduleTemplate(model);
      foreach (var id in model.UserIds)
      {
        foundItem.ParticipantsModels.Add(new ParticipantsTemplateModel()
        {
          UserId = id,
          ScheduleId = foundItem.ScheduleId
        });
      }

      // add new other participant
      foundItem.OtherParticipantModels = new List<OtherParticipantTemplateModel>();
      if (model.OtherParticipants.Any())
      {
        foreach (var item in model.OtherParticipants)
        {
          if (!string.IsNullOrEmpty(item.Name))
          {
            var otherParticipant = new OtherParticipantTemplateModel();
            otherParticipant.UpdateOtherParticipantTemplate(item);
            foundItem.OtherParticipantModels.Add(otherParticipant);
          }
        }
      }

      _scheduleTemplateRepository.Update(foundItem);
      _unitOfWork.Commit();

      return foundItem.ScheduleId;
    }

    public async Task<int> UpdatePersonalNotesAsync(PersonalNotesDto model)
    {
      var map = model.ToModel();
      map.PersonalNotesId = model.PersonalNotesId;
      var isValid = CheckBRPersonalNotes(map);
      if (isValid.IsSuccess==false) return -1;
      var result = await _scheduleRepository.UpdatePersonalNotesAsync(map);
      if (result == true)
        return map.PersonalNotesId;
      else
        return -1;
    }


    public async Task<int> UpdateScheduledResultDocumentAsync(ScheduledResultDocumentDto model)
    {

      var map = model.ToModel();
      map.ScheduledResultDocumentId = model.ScheduledResultDocumentId;
      var isValid = CheckBRScheduledResultDocument(map);
      if (isValid.IsSuccess==false) return -1;
      var result = await _scheduleRepository.UpdateScheduledResultDocumentAsync(map);
      if (result == true)
        return map.ScheduledResultDocumentId;
      else
        return -1;
    }

    public async Task<int> UpdateScheduledResultReportAsync(ScheduledResultReportDto model)
    {
      var map = model.ToModel();
      map.ScheduledResultReportId = model.ScheduledResultReportId;
      var isValid = CheckBRScheduledResultReport(map);
      var result = await _scheduleRepository.UpdateScheduledResultReportAsync(map);
      if (result == true)
        return map.ScheduledResultReportId;
      else
        return -1;
    }

    public async Task<int> UpdatePersonalScheduleAsync(PersonalScheduleDto model)
    {

      var map = model.ToModel();
      map.PersonalScheduleId = model.PersonalScheduleId;
      var isValid = CheckBRPersonalSchedule(map);
      var result = await _scheduleRepository.UpdatePersonalScheduleAsync(map);
      if (result == true)
        return map.PersonalScheduleId;
      else
        return -1;
    }


    public async Task<int> UpdateIsShareFieldScheduleFileAttachmentListAsync(List<ScheduleFileAttachmentShareDto> modelList)
    {
      foreach (var model in modelList)
      {
        var result = await _scheduleRepository.UpdateIsShareFieldOfScheduleFileAttachmentByIdAsync(model.Id, model.IsShare);
      }
      return 1;
    }
  }
}
