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
    Task<FunctionResult> CreateScheduleAsync(ScheduleForAddDto model);
    Task<int> CreatePersonalNotesAsync(PersonalNotesDto model);
    Task<int> CreateScheduledResultDocumentAsync(ScheduledResultDocumentDto model);
    Task<FunctionResult> CreateScheduledResultReportAsync(ScheduledResultReportDto model);
    Task<int> CreatePersonalScheduleAsync(PersonalScheduleDto model);
    Task<int> CreateTemplate(ScheduleTemplateForAddDto model);

  }

  public partial class ScheduleService : IScheduleService
  {
    public async Task<FunctionResult> CreateScheduleAsync(ScheduleForAddDto model)
    {
      var result = new FunctionResult();

      result= CheckRequireField(model);
      if (result.IsSuccess==false) return result;

       result = await CheckBusinessRulesOfSchedule(model);
      if (result.IsSuccess==false) return result;
    
      var createModel = new ScheduleModel();
      createModel.ConvertToScheduleModel(model);
      createModel.OrganizeId = model.OrganizeId;
      createModel.IsAutoSendAtScheduledTime = false;

      createModel= AddParticipantsAndOtherParticipantsWhenUpdateSchedule(model, createModel); 

      try
      {
        _scheduleRepository.Add(createModel);
        _unitOfWork.Commit();

        //init FileAttachment
        _filesAttachmentService.CreateFileAttachment(model.ScheduleFilesAttachment, createModel.ScheduleId);
        var auditSchedule = CreateAuditSchedule(EScheduleStatus.Created, EScheduleStatus.Pending, createModel.ScheduleId);
        if (auditSchedule != null) _auditScheduleRepository.Add(auditSchedule);
        _unitOfWork.Commit();

      }
      catch (Exception ex)
      {
        _logger.LogError($"Create Schedule: ", ex);
      }
      // send email/ sms
      if (model.ScheduleStatus == EScheduleStatus.Approve && model.IsSendEmail)
      {
        await _jobScheduleService.CreateManualJob(createModel.ScheduleId, EJobScheduleType.EMAIL, EScheduleStatus.Approve);
      }

      // send email/ sms
      if (model.ScheduleStatus == EScheduleStatus.Approve && model.ISendSMS)
      {
        await _jobScheduleService.CreateManualJob(createModel.ScheduleId, EJobScheduleType.SMS, EScheduleStatus.Approve);
      }

      result.SetData(createModel.ScheduleId);
      return result;
    }

    public async Task<int> CreatePersonalNotesAsync(PersonalNotesDto model)
    {
      var map = model.ToModel();
      map.CreatedDate = DateTime.Now;
      await _scheduleRepository.InsertPersonalNotesAsync(map);
      return map.PersonalNotesId;
    }

    public async Task<FunctionResult> CreateScheduledResultReportAsync(ScheduledResultReportDto model)
    {
      var isValid = new FunctionResult();
      var map = model.ToModel();
      map.CreatedDate = DateTime.Now;
      isValid = CheckBRScheduledResultReport(map);
      if (isValid.IsSuccess==false) return isValid;
      await _scheduleRepository.InsertScheduledResultReportAsync(map);
      isValid.SetData(map.ScheduledResultReportId);
      return isValid;
    }


    public async Task<int> CreateScheduledResultDocumentAsync(ScheduledResultDocumentDto model)
    {

      var map = model.ToModel();
      map.CreatedDate = DateTime.Now;
      await _scheduleRepository.InsertScheduledResultDocumentAsync(map);
      return map.ScheduledResultDocumentId;
    }

    public async Task<int> CreatePersonalScheduleAsync(PersonalScheduleDto model)
    {
      var isValid = new FunctionResult();
      var map = model.ToModel();
      map.CreatedDate = DateTime.Now;
      isValid=CheckBRPersonalSchedule(map);
      if (isValid.IsSuccess==false) return 0;
      await _scheduleRepository.InsertPersonalScheduleAsync(map);
      return map.PersonalScheduleId;
    } 

    public async Task<int> CreateTemplate(ScheduleTemplateForAddDto model)
    {
      var isExistSchedule = await CheckExistScheduleTemplate(model);

      if (!isExistSchedule)
      {
        var createModel = new ScheduleTemplateModel();
        createModel.UpdateScheduleTemplate(model);

        foreach (var id in model.UserIds)
        {
          createModel.ParticipantsModels.Add(new ParticipantsTemplateModel()
          {
            UserId = id,
            ScheduleId = createModel.ScheduleId
          });
        }

        createModel.OtherParticipantModels = new List<OtherParticipantTemplateModel>();
        if (model.OtherParticipants.Any())
        {
          foreach (var item in model.OtherParticipants)
          {
            if (!string.IsNullOrEmpty(item.Name))
            {
              var otherParticipant = new OtherParticipantTemplateModel();
              otherParticipant.UpdateOtherParticipantTemplate(item);
              createModel.OtherParticipantModels.Add(otherParticipant);
            }
          }
        }

        _scheduleTemplateRepository.Add(createModel);
        _unitOfWork.Commit();

        return createModel.ScheduleId;
      }

      return 0;
    }



  }
}
