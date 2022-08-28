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
    Task<int> DeleteScheduleByIdAsync(int scheduleId);
    Task<FunctionResult> DeleteScheduleByOrganizeIdAsync(int organizeId);
    Task<FunctionResult> DeleteScheduleTemplateByOrganizeIdAsync(int organizeId);
    Task<int> DeletePersonalNotesAsync(int personalNoteId);
    Task<int> DeletePersonalScheduleByIdAsync(int personalNoteId);
    Task<int> DeleteScheduleTemplateAsync(int scheduleId);

  }

  public partial class ScheduleService : IScheduleService
  {
    public async Task<int> DeleteScheduleByIdAsync(int scheduleId)
    {
      var isExist = await _scheduleRepository.CheckContains(x => x.ScheduleId == scheduleId);
      if (!isExist)
      {
        _logger.LogError($"Not Schedule post");
        throw new BusinessException($"Không tìm thấy lịch họp này", StatusCodes.Status404NotFound);
      }

      var foundItem = await _scheduleRepository.GetSingleById(scheduleId);

      _emailLogsRepository.DeleteMulti(x => x.ScheduleId == scheduleId);
      _otherParticipantRepository.DeleteMulti(x => x.ScheduleId == scheduleId);
      _auditScheduleRepository.DeleteMulti(x => x.ScheduleId == scheduleId);
      _participantRepository.DeleteMulti(x => x.ScheduleId == scheduleId);
      _scheduleHistoryRepository.DeleteMulti(x=>x.ScheduleId==scheduleId);   
      await _scheduleRepository.DeletePersonalNotesByScheduleIdAsync(scheduleId);
      await _scheduleRepository.DeleteScheduledResultDocumentByScheduleIdAsync(scheduleId);
      await _scheduleRepository.DeleteScheduledResultReportByScheduleIdAsync(scheduleId);

      _scheduleRepository.Delete(foundItem);
      _unitOfWork.Commit();

      return foundItem.ScheduleId;
    }

    public async Task<FunctionResult> DeleteScheduleByOrganizeIdAsync(int organizeId)
    {
      var result = new FunctionResult();
      var scheduleListByOrganize = await _scheduleRepository.GetScheduleByOrganizeIdAsync(organizeId);
      foreach (var schedule in scheduleListByOrganize)
      {
        await DeleteScheduleByIdAsync(schedule.ScheduleId);
      }
      return result;
    }

    public async Task<int> DeleteScheduleTemplateAsync(int scheduleId)
    {
      try
      {
        var isExist = await _scheduleTemplateRepository.CheckContains(x => x.ScheduleId == scheduleId);
        if (!isExist)
        {
          _logger.LogError($"Not Schedule post");
          throw new BusinessException($"Không tìm thấy lịch họp này", StatusCodes.Status404NotFound);
        }

        var foundItem = await _scheduleTemplateRepository.GetSingleById(scheduleId);

        _otherParticipantTemplateRepository.DeleteMulti(x => x.ScheduleId == scheduleId);
        _participantTemplateRepository.DeleteMulti(x => x.ScheduleId == scheduleId);
        _scheduleTemplateRepository.Delete(foundItem);
        _unitOfWork.Commit();

        return foundItem.ScheduleId;
      }
      catch (Exception ex)
      {
        _logger.LogError("DeleteTemplate: ", ex);
        return 0;
      }
    }

    public async Task<FunctionResult> DeleteScheduleTemplateByOrganizeIdAsync(int organizeId)
    {
      var result = new FunctionResult();
      var scheduleListByOrganize = await _scheduleTemplateRepository.GetScheduleTemplateByOrganizeIdAsync(organizeId);
      foreach (var scheduleTemp in scheduleListByOrganize)
      {
        await DeleteScheduleTemplateAsync(scheduleTemp.ScheduleId);
      }
      return result;
    }

    public async Task<int> DeletePersonalNotesAsync(int personalNoteId)
    {
      var result = await _scheduleRepository.DeletePersonalNotesByIdAsync(personalNoteId);
      if (result == true)
        return personalNoteId;
      else
        return -1;
    }

    public async Task<int> DeletePersonalScheduleByIdAsync(int personalNoteId)
    {
      var result = await _scheduleRepository.DeletePersonalScheduleAsync(personalNoteId);
      if (result == true)
        return personalNoteId;
      else
        return -1;
    }
  }
}
