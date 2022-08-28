using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using WebApi.Models.Settings;

namespace WebApi.Services
{
  public partial interface IScheduleService
  {
    Task<PaginationSet<ScheduleListDto>> GetAllScheduleAsync(
        int host,
        string scheduleDate,
        int? locationId,
        int active,
        int status,
        string sortOrder,
        string sortField,
        int index = 1,
        int pageSize = 10);

    Task<List<ScheduleDto>> GetAllScheduleByDateAsync(DateTime scheduleDate);

    Task<PaginationSet<ScheduleListDto>> GetAllScheduleByOrganizeIdAsync(
                                                                        int organizeId,
                                                                        int host,
                                                                        string scheduleDate,
                                                                        int? locationId,
                                                                        int active,
                                                                        int status,
                                                                        string sortOrder,
                                                                        string sortField,
                                                                        int index = 1,
                                                                        int pageSize = 10);



    Task<PaginationSet<ScheduleListDto>> GetAll(
        int host,
        string startDate,
        string endDate,
        int? locationId,
        int active,
        int status,
        string sortOrder,
        string sortField,
        int index = 1,
        int pageSize = 10);

    Task<PaginationSet<ScheduleListDto>> GetAllScheduleByOrganizeIdAndWeekAsync(
                                                                                int organizeId,
                                                                                int host,
                                                                                string startDate,
                                                                                string endDate,
                                                                                int? locationId,
                                                                                int active,
                                                                                int status,
                                                                                string sortOrder,
                                                                                string sortField,
                                                                                int index = 1,
                                                                                int pageSize = 10);



    Task<List<ScheduleByDayOfWeekDto>> GetAllByWeek(
        int host,
        string startDate,
        string endDate,
        int? locationId,
        int active,
        int status,
        bool selectAllWeek,
        string sortOrder,
        string sortField);

    Task<List<ScheduleByDayOfWeekDto>> GetAllScheduleByOrganizeIdAndWeek_NewVerAsync(
                                                                                 int organizeId,
                                                                                int host,
                                                                                string startDate,
                                                                                string endDate,
                                                                                int? locationId,
                                                                                int active,
                                                                                int status,
                                                                                bool selectAllWeek,
                                                                                string sortOrder,
                                                                                string sortField);


    Task<object> GetAllGroupByHost(int host, string scheduleDate, int? locationId, string endDate);



    Task<ScheduleForDetailDto> GetScheduleById(int scheduleId);

    Task<IEnumerable<ScheduleByDayOfWeekDto>> GetSchedulesByUserInPeriodDateAsync(int userId,
                                                                       string userEmail,
                                                                       string startDate,
                                                                       string endDate,
                                                                       bool selectAllWeek);
    Task<ScheduleForDetailDto> GetLatestScheduleByUser(int userId, string userEmail, string currentDate);

    Task<string> GetMessageContent(int scheduleId);
    Task<string> GetQRCodeByScheduleIdAsync(int scheduleId);


    Task<List<AuditScheduleDto>> getAllHistoryByScheduleId(int scheduleId);

    Task<IEnumerable<PersonalNotesModel>> GetPersonalNotesByScheduleIdAndUserIdAsync(int scheduleId, int userId);


    Task<IEnumerable<PersonalScheduleModel>> GetPersonalSchedulesByUserIdAsync(int userId);

    Task<IEnumerable<PersonalScheduleModel>> GetPersonalSchedulesByUserIdInPeriodDateAsync(int userId, string startDate, string endDate);

    Task<IEnumerable<ScheduledResultDocumentModel>> GetScheduledResultDocumentByScheduleIdAsync(int scheduleId);
    #region Template
    Task<List<ScheduleTemplateByDayOfWeekDto>> GetAllTemplateByWeek(
      int host,
      int? locationId,
      int active,
      int status,
      bool selectAllWeek,
      string sortOrder,
      string sortField);

    Task<List<ScheduleTemplateByDayOfWeekDto>> GetAllScheduleTemplateByWeekAndOrganizeIdAsync(
                                                                                                int organizeId,
                                                                                            int host,
                                                                                            int? locationId,
                                                                                            int active,
                                                                                            int status,
                                                                                            bool selectAllWeek,
                                                                                            string sortOrder,
                                                                                            string sortField);

    Task<ScheduleTemplateForDetailDto> GetTemplateById(int scheduleId);
    Task<IEnumerable<ScheduledResultReportModel>> GetScheduledResultReportByScheduleIdAsync(int scheduleId);

    #endregion Template

  }

  public partial class ScheduleService : IScheduleService
  {


    public async Task<PaginationSet<ScheduleListDto>> GetAll(
        int host,
        string startDate,
        string endDate,
        int? locationId,
        int active,
        int status,
        string sortOrder,
        string sortField,
        int index = 1,
        int pageSize = 10)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;
      var isDesc = sortOrder == "desc" ? true : false;
      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }

      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }

      if (active != -1)
      {
        baseFilter = active == 0 ? baseFilter.And(x => !x.IsActive) : baseFilter.And(x => x.IsActive);
      }

      if (status != -1)
      {
        baseFilter = baseFilter.And(x => x.ScheduleStatus == (EScheduleStatus)status);
      }

      if (!string.IsNullOrEmpty(startDate))
      {
        DateTime date = DateTime.Parse(startDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate >= date);
      }

      if (!string.IsNullOrEmpty(endDate))
      {
        DateTime pEndDate = DateTime.Parse(endDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate <= pEndDate);
      }

      var total = await _scheduleRepository.Count(baseFilter);
      var result = new List<ScheduleModel>();
      if (!string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("asc"))
      {
        result = await _scheduleRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      }
      else
      {
        result = await _scheduleRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      }

      var mappResult = _mapper.Map<List<ScheduleListDto>>(result);

      return new PaginationSet<ScheduleListDto>()
      {
        Items = mappResult,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<PaginationSet<ScheduleListDto>> GetAllScheduleByOrganizeIdAndWeekAsync(
                                                                                            int organizeId,
                                                                                             int host,
                                                                                             string startDate,
                                                                                             string endDate,
                                                                                             int? locationId,
                                                                                             int active,
                                                                                             int status,
                                                                                             string sortOrder,
                                                                                             string sortField,
                                                                                             int index = 1,
                                                                                             int pageSize = 10)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;
      var isDesc = sortOrder == "desc" ? true : false;
      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }

      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }
      if (organizeId >= 0)
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }

      if (active != -1)
      {
        baseFilter = active == 0 ? baseFilter.And(x => !x.IsActive) : baseFilter.And(x => x.IsActive);
      }

      if (status != -1)
      {
        baseFilter = baseFilter.And(x => x.ScheduleStatus == (EScheduleStatus)status);
      }

      if (!string.IsNullOrEmpty(startDate))
      {
        DateTime date = DateTime.Parse(startDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate >= date);
      }

      if (!string.IsNullOrEmpty(endDate))
      {
        DateTime pEndDate = DateTime.Parse(endDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate <= pEndDate);
      }

      var total = await _scheduleRepository.Count(baseFilter);
      var result = new List<ScheduleModel>();
      if (!string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("asc"))
      {
        result = await _scheduleRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      }
      else
      {
        result = await _scheduleRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      }

      var mappResult = _mapper.Map<List<ScheduleListDto>>(result);

      return new PaginationSet<ScheduleListDto>()
      {
        Items = mappResult,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<PaginationSet<ScheduleListDto>> GetAllScheduleAsync(
      int host,
      string scheduleDate,
      int? locationId,
      int active,
      int status,
      string sortOrder,
      string sortField,
      int index = 1,
      int pageSize = 10)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;
      var isDesc = sortOrder == "desc" ? true : false;
      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }

      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }

      if (active != -1)
      {
        baseFilter = active == 0 ? baseFilter.And(x => !x.IsActive) : baseFilter.And(x => x.IsActive);
      }

      if (status != -1)
      {
        baseFilter = baseFilter.And(x => x.ScheduleStatus == (EScheduleStatus)status);
      }

      if (!string.IsNullOrEmpty(scheduleDate))
      {
        var date = DateTime.Parse(scheduleDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate.Year == date.Year && x.ScheduleDate.Month == date.Month && x.ScheduleDate.Day == date.Day);
      }

      var total = await _scheduleRepository.Count(baseFilter);
      var result = new List<ScheduleModel>();
      if (!string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("asc"))
      {
        result = await _scheduleRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      }
      else
      {
        result = await _scheduleRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      }

      var mappResult = _mapper.Map<List<ScheduleListDto>>(result);

      return new PaginationSet<ScheduleListDto>()
      {
        Items = mappResult,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<List<ScheduleDto>> GetAllScheduleByDateAsync(DateTime scheduleDate)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;

      baseFilter = baseFilter.And(x => x.ScheduleDate.Date.Equals(scheduleDate.Date));

      var result = await _scheduleRepository.GetMulti(baseFilter);
      var mappResult = _mapper.Map<List<ScheduleDto>>(result);
      return mappResult;
    }

    public async Task<PaginationSet<ScheduleListDto>> GetAllScheduleByOrganizeIdAsync(
                                                                                     int organizeId,
                                                                                     int host,
                                                                                     string scheduleDate,
                                                                                     int? locationId,
                                                                                     int active,
                                                                                     int status,
                                                                                     string sortOrder,
                                                                                     string sortField,
                                                                                     int index = 1,
                                                                                     int pageSize = 10)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;
      var isDesc = sortOrder == "desc" ? true : false;
      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }

      if (organizeId >= 0)
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }

      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }

      if (active != -1)
      {
        baseFilter = active == 0 ? baseFilter.And(x => !x.IsActive) : baseFilter.And(x => x.IsActive);
      }

      if (status != -1)
      {
        baseFilter = baseFilter.And(x => x.ScheduleStatus == (EScheduleStatus)status);
      }

      if (!string.IsNullOrEmpty(scheduleDate))
      {
        var date = DateTime.Parse(scheduleDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate.Year == date.Year && x.ScheduleDate.Month == date.Month && x.ScheduleDate.Day == date.Day);
      }

      var total = await _scheduleRepository.Count(baseFilter);
      var result = new List<ScheduleModel>();
      if (!string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("asc"))
      {
        result = await _scheduleRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      }
      else
      {
        result = await _scheduleRepository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      }

      var mappResult = _mapper.Map<List<ScheduleListDto>>(result);

      return new PaginationSet<ScheduleListDto>()
      {
        Items = mappResult,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<List<ScheduleByDayOfWeekDto>> GetAllByWeek(
        int host,
        string startDate,
        string endDate,
        int? locationId,
        int active,
        int status,
        bool selectAllWeek,
        string sortOrder,
        string sortField)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;
      var isDesc = sortOrder == "desc" ? true : false;

      DateTime date = DateTime.Parse(startDate);
      DateTime pEndDate = DateTime.Parse(endDate);

      DateTime dayOfWeek = DateTime.Now;

      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }

      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }

      if (active != -1)
      {
        baseFilter = active == 0 ? baseFilter.And(x => !x.IsActive) : baseFilter.And(x => x.IsActive);
      }

      if (status != -1)
      {
        baseFilter = baseFilter.And(x => x.ScheduleStatus == (EScheduleStatus)status);
      }

      // Tìm kiếm theo cả tuần
      if (selectAllWeek)
      {
        if (!string.IsNullOrEmpty(startDate))
        {
          baseFilter = baseFilter.And(x => x.ScheduleDate >= date);
        }

        if (!string.IsNullOrEmpty(endDate))
        {
          baseFilter = baseFilter.And(x => x.ScheduleDate <= pEndDate);
        }
      }
      else // Tìm kiếm theo từng ngày trong tuần
      {
        dayOfWeek = date;
        baseFilter = baseFilter.And(x => x.ScheduleDate.Year == date.Year && x.ScheduleDate.Month == date.Month && x.ScheduleDate.Day == date.Day);
      }

      var schedules = await _scheduleRepository.GetMulti(baseFilter, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel", "ScheduleFilesAttachment" });
      var mappResult = _mapper.Map<List<ScheduleListDto>>(schedules);

      var dayOffWeek = new List<AllDayOfWeekDto>();

      int temp = 2;
      do
      {
        switch (temp)
        {
          case 2:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ hai {date.ToString("dd/MM")}"
            });
            break;
          case 3:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ ba {date.ToString("dd/MM")}"
            });
            break;
          case 4:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ tư {date.ToString("dd/MM")}"
            });
            break;
          case 5:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ năm {date.ToString("dd/MM")}"
            });
            break;
          case 6:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ sáu {date.ToString("dd/MM")}"
            });
            break;
          case 7:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ bảy {date.ToString("dd/MM")}"
            });
            break;
          case 8:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Chủ nhật {date.ToString("dd/MM")}"
            });
            break;
        }
        temp += 1;
        date = date.AddDays(1);
      } while (temp < 9);

      var finalResult = new List<ScheduleByDayOfWeekDto>();

      if (selectAllWeek)
      {
        foreach (var itemDay in dayOffWeek)
        {
          var scheduleByDayOfWeek = new ScheduleByDayOfWeekDto();
          scheduleByDayOfWeek.DayOfWeek = itemDay.DayName;
          foreach (var itemSchedule in mappResult)
          {
            if (itemDay.Date.Day == itemSchedule.ScheduleDate.Day)
            {
              var timeOfSchedule = itemSchedule.ScheduleTime.Split(':');
              if (timeOfSchedule.Length > 1)
              {
                // check xem lịch họp thuộc buổi sáng hay buổi chiều
                var timeOfDayOfSchedule = int.Parse(timeOfSchedule[0]);
                if (timeOfDayOfSchedule >= 1 && timeOfDayOfSchedule < 12)
                {
                  scheduleByDayOfWeek.Morning.Add(itemSchedule);
                }
                else if (timeOfDayOfSchedule >= 12 && timeOfDayOfSchedule < 18)
                {
                  scheduleByDayOfWeek.Afternoon.Add(itemSchedule);
                }
                else if (timeOfDayOfSchedule >= 18 && timeOfDayOfSchedule < 24)
                {
                  scheduleByDayOfWeek.Evening.Add(itemSchedule);
                }
              }
            }
          }
          finalResult.Add(scheduleByDayOfWeek);
        }
      }
      else // chỉ lọc theo một ngày trong tuần
      {
        //var findDay = dayOffWeek.FirstOrDefault(x => x.Date.Day == dayOfWeek.Day);
        var scheduleByDayOfWeek = new ScheduleByDayOfWeekDto
        {
          DayOfWeek = dayOfWeek.ToString("dd/MM/yyyy")
        };
        var scheduleDayOfWeek = mappResult.Where(x => x.ScheduleDate.Day == dayOfWeek.Day);
        foreach (var item in scheduleDayOfWeek)
        {
          var timeOfSchedule = item.ScheduleTime.Split(':');
          if (timeOfSchedule.Length > 1)
          {
            // check xem lịch họp thuộc buổi sáng hay buổi chiều
            var timeOfDayOfSchedule = int.Parse(timeOfSchedule[0]);
            if (timeOfDayOfSchedule >= 1 && timeOfDayOfSchedule < 12)
            {
              scheduleByDayOfWeek.Morning.Add(item);
            }
            else if (timeOfDayOfSchedule >= 12 && timeOfDayOfSchedule < 18)
            {
              scheduleByDayOfWeek.Afternoon.Add(item);
            }
            else if (timeOfDayOfSchedule >= 18 && timeOfDayOfSchedule < 24)
            {
              scheduleByDayOfWeek.Evening.Add(item);
            }
          }
        }
        finalResult.Add(scheduleByDayOfWeek);
      }

      return finalResult;
    }




    public async Task<List<ScheduleByDayOfWeekDto>> GetAllScheduleByOrganizeIdAndWeek_NewVerAsync(
                                                                                                  int organizeId,
                                                                                                int host,
                                                                                                string startDate,
                                                                                                string endDate,
                                                                                                int? locationId,
                                                                                                int active,
                                                                                                int status,
                                                                                                bool selectAllWeek,
                                                                                                string sortOrder,
                                                                                                string sortField)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;
      var isDesc = sortOrder == "desc" ? true : false;

      DateTime date = DateTime.Parse(startDate);
      DateTime pEndDate = DateTime.Parse(endDate);

      DateTime dayOfWeek = DateTime.Now;

      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }
      if (organizeId >0)
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }
      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }

      if (active != -1)
      {
        baseFilter = active == 0 ? baseFilter.And(x => !x.IsActive) : baseFilter.And(x => x.IsActive);
      }

      if (status != -1)
      {
        baseFilter = baseFilter.And(x => x.ScheduleStatus == (EScheduleStatus)status);
      }

      // Tìm kiếm theo cả tuần
      if (selectAllWeek)
      {
        if (!string.IsNullOrEmpty(startDate))
        {
          baseFilter = baseFilter.And(x => x.ScheduleDate >= date);
        }

        if (!string.IsNullOrEmpty(endDate))
        {
          baseFilter = baseFilter.And(x => x.ScheduleDate <= pEndDate);
        }
      }
      else // Tìm kiếm theo từng ngày trong tuần
      {
        dayOfWeek = date;
        baseFilter = baseFilter.And(x => x.ScheduleDate.Year == date.Year && x.ScheduleDate.Month == date.Month && x.ScheduleDate.Day == date.Day);
      }

      var schedules = await _scheduleRepository.GetMulti(baseFilter, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel", "ScheduleFilesAttachment" });
      var mappResult = _mapper.Map<List<ScheduleListDto>>(schedules);

      var dayOffWeek = new List<AllDayOfWeekDto>();

      int temp = 2;
      do
      {
        switch (temp)
        {
          case 2:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ hai {date.ToString("dd/MM")}"
            });
            break;
          case 3:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ ba {date.ToString("dd/MM")}"
            });
            break;
          case 4:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ tư {date.ToString("dd/MM")}"
            });
            break;
          case 5:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ năm {date.ToString("dd/MM")}"
            });
            break;
          case 6:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ sáu {date.ToString("dd/MM")}"
            });
            break;
          case 7:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ bảy {date.ToString("dd/MM")}"
            });
            break;
          case 8:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Chủ nhật {date.ToString("dd/MM")}"
            });
            break;
        }
        temp += 1;
        date = date.AddDays(1);
      } while (temp < 9);

      var finalResult = new List<ScheduleByDayOfWeekDto>();

      if (selectAllWeek)
      {
        foreach (var itemDay in dayOffWeek)
        {
          var scheduleByDayOfWeek = new ScheduleByDayOfWeekDto();
          scheduleByDayOfWeek.DayOfWeek = itemDay.DayName;
          foreach (var itemSchedule in mappResult)
          {
            if (itemDay.Date.Day == itemSchedule.ScheduleDate.Day)
            {
              var timeOfSchedule = itemSchedule.ScheduleTime.Split(':');
              if (timeOfSchedule.Length > 1)
              {
                // check xem lịch họp thuộc buổi sáng hay buổi chiều
                var timeOfDayOfSchedule = int.Parse(timeOfSchedule[0]);
                if (timeOfDayOfSchedule >= 1 && timeOfDayOfSchedule < 12)
                {
                  scheduleByDayOfWeek.Morning.Add(itemSchedule);
                }
                else if (timeOfDayOfSchedule >= 12 && timeOfDayOfSchedule < 18)
                {
                  scheduleByDayOfWeek.Afternoon.Add(itemSchedule);
                }
                else if (timeOfDayOfSchedule >= 18 && timeOfDayOfSchedule < 24)
                {
                  scheduleByDayOfWeek.Evening.Add(itemSchedule);
                }
              }
            }
          }
          finalResult.Add(scheduleByDayOfWeek);
        }
      }
      else // chỉ lọc theo một ngày trong tuần
      {
        //var findDay = dayOffWeek.FirstOrDefault(x => x.Date.Day == dayOfWeek.Day);
        var scheduleByDayOfWeek = new ScheduleByDayOfWeekDto
        {
          DayOfWeek = dayOfWeek.ToString("dd/MM/yyyy")
        };
        var scheduleDayOfWeek = mappResult.Where(x => x.ScheduleDate.Day == dayOfWeek.Day);
        foreach (var item in scheduleDayOfWeek)
        {
          var timeOfSchedule = item.ScheduleTime.Split(':');
          if (timeOfSchedule.Length > 1)
          {
            // check xem lịch họp thuộc buổi sáng hay buổi chiều
            var timeOfDayOfSchedule = int.Parse(timeOfSchedule[0]);
            if (timeOfDayOfSchedule >= 1 && timeOfDayOfSchedule < 12)
            {
              scheduleByDayOfWeek.Morning.Add(item);
            }
            else if (timeOfDayOfSchedule >= 12 && timeOfDayOfSchedule < 18)
            {
              scheduleByDayOfWeek.Afternoon.Add(item);
            }
            else if (timeOfDayOfSchedule >= 18 && timeOfDayOfSchedule < 24)
            {
              scheduleByDayOfWeek.Evening.Add(item);
            }
          }
        }
        finalResult.Add(scheduleByDayOfWeek);
      }

      return finalResult;
    }

    public async Task<object> GetAllGroupByHost(int host, string scheduleDate, int? locationId, string endDate)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;

      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }

      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }

      // lấy lịch theo ngày
      if (string.IsNullOrEmpty(endDate))
      {
        if (!string.IsNullOrEmpty(scheduleDate))
        {
          var date = DateTime.Parse(scheduleDate);
          baseFilter = baseFilter.And(x => x.ScheduleDate.Year == date.Year && x.ScheduleDate.Month == date.Month && x.ScheduleDate.Day == date.Day);
        }
      }
      else // lấy lịch theo tuần
      {

        if (!string.IsNullOrEmpty(scheduleDate))
        {
          var date = DateTime.Parse(scheduleDate);
          baseFilter = baseFilter.And(x => x.ScheduleDate >= date);
        }

        var pEndDate = DateTime.Parse(endDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate <= pEndDate);
      }


      baseFilter = baseFilter.And(x => x.IsActive);

      //baseFilter = baseFilter.And(x => x.ScheduleStatus == EScheduleStatus.Approve || x.ScheduleStatus == EScheduleStatus.Changed);

      var result = await _scheduleRepository.GetMulti(baseFilter, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });

      result = result.OrderBy(x => x.ScheduleTime).ToList();

      var mappResult = _mapper.Map<List<ScheduleDto>>(result);
      var group = mappResult.GroupBy(gr => new { gr.OfficerName, gr.OfficerPosition })
          .Select(m => new
          {
            m.Key.OfficerName,
            Schedules = m.Where(x => x.OfficerName.Equals(m.Key.OfficerName)).ToList(),
            m.Key.OfficerPosition
          }).ToList();

      return group;
    }

    public async Task<ScheduleForDetailDto> GetScheduleById(int scheduleId)
    {
      var foundItem = await _scheduleRepository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
          new string[] {
                    "ParticipantsModels.User.Department",
                    "OtherParticipantModels",
                    "UserModel",
                    "LocationModel",
                    "ScheduleFilesAttachment"
          });
      if (foundItem == null)
      {
        _logger.LogWarning($"scheduleId {scheduleId} is not found");
        throw new BusinessException($"scheduleId {scheduleId} không tìm thấy", StatusCodes.Status404NotFound);
      }

      var mapResult = _mapper.Map<ScheduleForDetailDto>(foundItem);

      mapResult.ScheduleLocation = foundItem.LocationModel != null && string.IsNullOrEmpty(foundItem.OtherLocation) ? foundItem.LocationModel.Title : string.Empty;

      if (foundItem.OtherParticipantModels.Any())
      {
        var mapOther = _mapper.Map<List<OtherParticipantDto>>(foundItem.OtherParticipantModels);
        mapResult.OtherParticipants = mapOther;
      }

      if (foundItem.ParticipantsModels.Any())
      {
        mapResult.ParticipantIsSelected = _mapper.Map<List<ParticipantIsSelectedDto>>(foundItem.ParticipantsModels);
      }

      if (foundItem.ScheduleFilesAttachment.Any())
      {
        mapResult.ScheduleFilesAttachment = _mapper.Map<List<ScheduleFilesAttachmentDto>>(foundItem.ScheduleFilesAttachment);
        var hostURL = _appSettings.HostURL;
        foreach (var scheduleFileAttachment in mapResult.ScheduleFilesAttachment)
        {
          scheduleFileAttachment.FilePath=$"{hostURL}\\{scheduleFileAttachment.FilePath}";
        }
      }

      return mapResult;
    }

    public async Task<IEnumerable<ScheduleByDayOfWeekDto>> GetSchedulesByUserInPeriodDateAsync(int userId,
                                                                                    string userEmail,
                                                                                    string startDate,
                                                                                    string endDate,
                                                                                    bool selectAllWeek
                                                                                   )
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;


      DateTime date = DateTime.Parse(startDate);
      DateTime pEndDate = DateTime.Parse(endDate);

      DateTime dayOfWeek = DateTime.Now;



      if (userId != -1)
        baseFilter = baseFilter.And(x => x.ParticipantsModels.Any(x => x.UserId == userId));
      else
        if (userEmail != string.Empty)
        baseFilter = baseFilter.And(x => x.OtherParticipantModels.Any(x => x.Email == userEmail));
      else
        return new List<ScheduleByDayOfWeekDto>();

      baseFilter = baseFilter.And(x => x.IsActive == true);
      // Tìm kiếm theo cả tuần
      if (selectAllWeek)
      {
        if (!string.IsNullOrEmpty(startDate))
        {
          baseFilter = baseFilter.And(x => x.ScheduleDate >= date);
        }

        if (!string.IsNullOrEmpty(endDate))
        {
          baseFilter = baseFilter.And(x => x.ScheduleDate <= pEndDate);
        }
      }
      else // Tìm kiếm theo từng ngày trong tuần
      {
        dayOfWeek = date;
        baseFilter = baseFilter.And(x => x.ScheduleDate.Year == date.Year && x.ScheduleDate.Month == date.Month && x.ScheduleDate.Day == date.Day);
      }

      var schedules = await _scheduleRepository.GetMulti(baseFilter, new string[] { "UserModel", "ParticipantsModels", "OtherParticipantModels", "ScheduleFilesAttachment" });
      var mappResult = _mapper.Map<List<ScheduleListDto>>(schedules);

      var dayOffWeek = new List<AllDayOfWeekDto>();

      int temp = 2;
      do
      {
        switch (temp)
        {
          case 2:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ hai {date.ToString("dd/MM")}"
            });
            break;
          case 3:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ ba {date.ToString("dd/MM")}"
            });
            break;
          case 4:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ tư {date.ToString("dd/MM")}"
            });
            break;
          case 5:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ năm {date.ToString("dd/MM")}"
            });
            break;
          case 6:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ sáu {date.ToString("dd/MM")}"
            });
            break;
          case 7:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Thứ bảy {date.ToString("dd/MM")}"
            });
            break;
          case 8:
            dayOffWeek.Add(new AllDayOfWeekDto()
            {
              Date = date,
              DayName = $"Chủ nhật {date.ToString("dd/MM")}"
            });
            break;
        }
        temp += 1;
        date = date.AddDays(1);
      } while (temp < 9);

      var finalResult = new List<ScheduleByDayOfWeekDto>();

      if (selectAllWeek)
      {
        foreach (var itemDay in dayOffWeek)
        {
          var scheduleByDayOfWeek = new ScheduleByDayOfWeekDto();
          scheduleByDayOfWeek.DayOfWeek = itemDay.DayName;
          foreach (var itemSchedule in mappResult)
          {
            if (itemDay.Date.Day == itemSchedule.ScheduleDate.Day)
            {
              var timeOfSchedule = itemSchedule.ScheduleTime.Split(':');
              if (timeOfSchedule.Length > 1)
              {
                // check xem lịch họp thuộc buổi sáng hay buổi chiều
                var timeOfDayOfSchedule = int.Parse(timeOfSchedule[0]);
                if (timeOfDayOfSchedule >= 1 && timeOfDayOfSchedule < 12)
                {
                  scheduleByDayOfWeek.Morning.Add(itemSchedule);
                }
                else if (timeOfDayOfSchedule >= 12 && timeOfDayOfSchedule < 18)
                {
                  scheduleByDayOfWeek.Afternoon.Add(itemSchedule);
                }
                else if (timeOfDayOfSchedule >= 18 && timeOfDayOfSchedule < 24)
                {
                  scheduleByDayOfWeek.Evening.Add(itemSchedule);
                }
              }
            }
          }
          finalResult.Add(scheduleByDayOfWeek);
        }
      }
      else // chỉ lọc theo một ngày trong tuần
      {
        //var findDay = dayOffWeek.FirstOrDefault(x => x.Date.Day == dayOfWeek.Day);
        var scheduleByDayOfWeek = new ScheduleByDayOfWeekDto
        {
          DayOfWeek = dayOfWeek.ToString("dd/MM/yyyy")
        };
        var scheduleDayOfWeek = mappResult.Where(x => x.ScheduleDate.Day == dayOfWeek.Day);
        foreach (var item in scheduleDayOfWeek)
        {
          var timeOfSchedule = item.ScheduleTime.Split(':');
          if (timeOfSchedule.Length > 1)
          {
            // check xem lịch họp thuộc buổi sáng hay buổi chiều
            var timeOfDayOfSchedule = int.Parse(timeOfSchedule[0]);
            if (timeOfDayOfSchedule >= 1 && timeOfDayOfSchedule < 12)
            {
              scheduleByDayOfWeek.Morning.Add(item);
            }
            else if (timeOfDayOfSchedule >= 12 && timeOfDayOfSchedule < 18)
            {
              scheduleByDayOfWeek.Afternoon.Add(item);
            }
            else if (timeOfDayOfSchedule >= 18 && timeOfDayOfSchedule < 24)
            {
              scheduleByDayOfWeek.Evening.Add(item);
            }
          }
        }
        finalResult.Add(scheduleByDayOfWeek);
      }

      return finalResult;
    }





    public async Task<string> GetMessageContent(int scheduleId)
    {
      var foundItem = await _scheduleRepository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
          new string[] {
                    "ScheduleTitleTemplateModel",
                    "UserModel",
                    "LocationModel"
          });
      if (foundItem == null)
      {
        _logger.LogWarning($"scheduleId {scheduleId} is not found");
        throw new BusinessException($"scheduleId {scheduleId} không tìm thấy", StatusCodes.Status404NotFound);
      }

      string template = foundItem.ScheduleTitleTemplateModel == null ? "" : foundItem.ScheduleTitleTemplateModel.Template;
      string location = foundItem.LocationModel == null ? foundItem.OtherLocation : foundItem.LocationModel.Title;
      string host = foundItem.UserModel == null ? foundItem.OtherHost : foundItem.UserModel.FullName;

      string messageContent = $"{template} {foundItem.ScheduleTitle} vào lúc {foundItem.ScheduleTime}, ngày {foundItem.ScheduleDate.ToString("dd/MM/yyyy")} " +
          $"tại {location} do đồng chí {host} chủ trì";
      return messageContent;
    }



    public async Task<List<AuditScheduleDto>> getAllHistoryByScheduleId(int scheduleId)
    {
      try
      {
        var audits = await _auditScheduleRepository.GetMulti(r => r.ScheduleId == scheduleId);
        audits = audits.OrderByDescending(r => r.ChangeDate).ToList();

        return _mapper.Map<List<AuditScheduleDto>>(audits);
      }
      catch
      {
        return new List<AuditScheduleDto>();
      }
    }



    private string GetSchuduleStatus(EScheduleStatus status)
    {
      switch (status)
      {
        case EScheduleStatus.Created:
          return "Tạo mới";
        case EScheduleStatus.Pending:
          return "Đang soạn thảo";
        case EScheduleStatus.Approve:
          return "Đã duyệt";
        case EScheduleStatus.Pause:
          return "Lịch bị hoãn";
        case EScheduleStatus.Release:
          return "Đã phát hành";
        case EScheduleStatus.Changed:
          return "Đã dời lịch họp";
        default:
          return string.Empty;
      }
    }


    public async Task<List<ScheduleTemplateByDayOfWeekDto>> GetAllTemplateByWeek(
     int host,
     int? locationId,
     int active,
     int status,
     bool selectAllWeek,
     string sortOrder,
     string sortField)
    {
      Expression<Func<ScheduleTemplateModel, bool>> baseFilter = schedule => true;
      var isDesc = sortOrder == "desc" ? true : false;

      DateTime dayOfWeek = DateTime.Now;

      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }

      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }

      if (active != -1)
      {
        baseFilter = active == 0 ? baseFilter.And(x => !x.IsActive) : baseFilter.And(x => x.IsActive);
      }

      if (status != -1)
      {
        baseFilter = baseFilter.And(x => x.ScheduleStatus == (EScheduleStatus)status);
      }

      var schedules = await _scheduleTemplateRepository.GetMulti(baseFilter, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      var mappResult = _mapper.Map<List<ScheduleTemplateListDto>>(schedules);

      var finalResult = new List<ScheduleTemplateByDayOfWeekDto>();

      var scheduleByDayOfWeek = new ScheduleTemplateByDayOfWeekDto();
      foreach (var itemSchedule in mappResult)
      {
        scheduleByDayOfWeek.DayOfWeek = string.Empty;
        var timeOfSchedule = itemSchedule.ScheduleTime.Split(':');
        if (timeOfSchedule.Length > 1)
        {
          // check xem lịch họp thuộc buổi sáng hay buổi chiều
          var timeOfDayOfSchedule = int.Parse(timeOfSchedule[0]);
          if (timeOfDayOfSchedule >= 1 && timeOfDayOfSchedule < 12)
          {
            scheduleByDayOfWeek.Morning.Add(itemSchedule);
          }
          else if (timeOfDayOfSchedule >= 12 && timeOfDayOfSchedule < 18)
          {
            scheduleByDayOfWeek.Afternoon.Add(itemSchedule);
          }
          else if (timeOfDayOfSchedule >= 18 && timeOfDayOfSchedule < 24)
          {
            scheduleByDayOfWeek.Evening.Add(itemSchedule);
          }
        }
      }

      finalResult.Add(scheduleByDayOfWeek);

      return finalResult;
    }

    public async Task<List<ScheduleTemplateByDayOfWeekDto>> GetAllScheduleTemplateByWeekAndOrganizeIdAsync(
                                                                                               int organizeId,
                                                                                           int host,
                                                                                           int? locationId,
                                                                                           int active,
                                                                                           int status,
                                                                                           bool selectAllWeek,
                                                                                           string sortOrder,
                                                                                           string sortField)

    {
      Expression<Func<ScheduleTemplateModel, bool>> baseFilter = schedule => true;
      var isDesc = sortOrder == "desc" ? true : false;

      DateTime dayOfWeek = DateTime.Now;

      if (organizeId >0)
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }

      if (locationId != -1)
      {
        baseFilter = baseFilter.And(x => x.LocationId == locationId);
      }
      if (host != 0)
      {
        baseFilter = baseFilter.And(x => x.Id == host);
      }

      if (active != -1)
      {
        baseFilter = active == 0 ? baseFilter.And(x => !x.IsActive) : baseFilter.And(x => x.IsActive);
      }

      if (status != -1)
      {
        baseFilter = baseFilter.And(x => x.ScheduleStatus == (EScheduleStatus)status);
      }

      var schedules = await _scheduleTemplateRepository.GetMulti(baseFilter, new string[] { "UserModel", "ParticipantsModels.User", "LocationModel" });
      var mappResult = _mapper.Map<List<ScheduleTemplateListDto>>(schedules);

      var finalResult = new List<ScheduleTemplateByDayOfWeekDto>();

      var scheduleByDayOfWeek = new ScheduleTemplateByDayOfWeekDto();
      foreach (var itemSchedule in mappResult)
      {
        scheduleByDayOfWeek.DayOfWeek = string.Empty;
        var timeOfSchedule = itemSchedule.ScheduleTime.Split(':');
        if (timeOfSchedule.Length > 1)
        {
          // check xem lịch họp thuộc buổi sáng hay buổi chiều
          var timeOfDayOfSchedule = int.Parse(timeOfSchedule[0]);
          if (timeOfDayOfSchedule >= 1 && timeOfDayOfSchedule < 12)
          {
            scheduleByDayOfWeek.Morning.Add(itemSchedule);
          }
          else if (timeOfDayOfSchedule >= 12 && timeOfDayOfSchedule < 18)
          {
            scheduleByDayOfWeek.Afternoon.Add(itemSchedule);
          }
          else if (timeOfDayOfSchedule >= 18 && timeOfDayOfSchedule < 24)
          {
            scheduleByDayOfWeek.Evening.Add(itemSchedule);
          }
        }
      }

      finalResult.Add(scheduleByDayOfWeek);

      return finalResult;
    }


    public async Task<ScheduleTemplateForDetailDto> GetTemplateById(int scheduleId)
    {
      var foundItem = await _scheduleTemplateRepository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
          new string[] {
                    "ParticipantsModels.User.Department",
                    "OtherParticipantModels",
                    "UserModel",
                    "LocationModel"
          });
      if (foundItem == null)
      {
        _logger.LogWarning($"scheduleId {scheduleId} is not found");
        throw new BusinessException($"scheduleId {scheduleId} không tìm thấy", StatusCodes.Status404NotFound);
      }

      var mapResult = _mapper.Map<ScheduleTemplateForDetailDto>(foundItem);

      if (foundItem.OtherParticipantModels.Any())
      {
        var mapOther = _mapper.Map<List<OtherParticipantDto>>(foundItem.OtherParticipantModels);
        mapResult.OtherParticipants = mapOther;
      }

      if (foundItem.ParticipantsModels.Any())
      {
        mapResult.ParticipantIsSelected = _mapper.Map<List<ParticipantIsSelectedDto>>(foundItem.ParticipantsModels);
      }

      return mapResult;
    }




    public async Task<IEnumerable<PersonalNotesModel>> GetPersonalNotesByScheduleIdAndUserIdAsync(int scheduleId, int userId)
    {

      var foundItem = await _scheduleRepository.GetPersonalNotesByScheduleIdAndUserIdAsync(scheduleId, userId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Personal Notes List is empty");
        return new List<PersonalNotesModel>();
      }

      return foundItem;

    }

    public async Task<IEnumerable<ScheduledResultDocumentModel>> GetScheduledResultDocumentByScheduleIdAsync(int scheduleId)
    {
      var foundItem = await _scheduleRepository.GetScheduledResultDocumentByScheduleIdAsync(scheduleId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Scheduled Result Document List is empty");
        return new List<ScheduledResultDocumentModel>();
      }

      var hostURL = _appSettings.HostURL;
      var finalList = foundItem.ToList();
      foreach (var item in finalList)
      {
        item.Path=$"{hostURL}\\{item.Path}";
      }
      return finalList;
    }

    public async Task<IEnumerable<ScheduledResultReportModel>> GetScheduledResultReportByScheduleIdAsync(int scheduleId)
    {
      var foundItemList = await _scheduleRepository.GetScheduledResultReportByScheduleIdAsync(scheduleId);
      if (foundItemList == null)
      {
        _logger.LogWarning($"Scheduled Result Report List is empty");
        return new List<ScheduledResultReportModel>();
      }
      var hostURL = _appSettings.HostURL;
      var finalList = foundItemList.ToList();
      foreach (var item in finalList)
      {
        item.Path=$"{hostURL}\\{item.Path}";
      }    
      return finalList;
    }

    public async Task<IEnumerable<PersonalScheduleModel>> GetPersonalSchedulesByUserIdAsync(int userId)
    {
      var foundItem = await _scheduleRepository.GetPersonalScheduleByUserIdAsync(userId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Personal Schedule List is empty");
        return new List<PersonalScheduleModel>();
      }

      return foundItem;
    }

    public async Task<IEnumerable<PersonalScheduleModel>> GetPersonalSchedulesByUserIdInPeriodDateAsync(int userId, string startDate, string endDate)
    {
      var foundItem = await _scheduleRepository.GetPersonalScheduleByUserIdAsync(userId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Personal Schedule List is empty");
        return new List<PersonalScheduleModel>();
      }
      if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
      {
        DateTime fromDate = DateTime.Parse(startDate);
        DateTime toDate = DateTime.Parse(endDate);
        foundItem = foundItem.Where(x => x.Fromdate >= fromDate && x.ToDate <= toDate);
      }
      return foundItem;
    }

    public async Task<string> GetQRCodeByScheduleIdAsync(int scheduleId)
    {
      QRCodeGenerator qrGenerator = new QRCodeGenerator();
      QRCodeData qrCodeData = qrGenerator.CreateQrCode(Constants.QR_CODE_URL + "?sid=" + scheduleId, QRCodeGenerator.ECCLevel.Q);
      QRCode qrCode = new QRCode(qrCodeData);
      Bitmap qrCodeImage = qrCode.GetGraphic(20);
      MemoryStream ms = new MemoryStream();
      qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
      var result = Convert.ToBase64String(ms.ToArray());
      return await Task.Run(() => result);
    }

    public async Task<ScheduleForDetailDto> GetLatestScheduleByUser(int userId, string? userEmail, string currentDate)
    {
      var allItemFound = await _scheduleRepository.GetAll(
        new string[] {
                    "ParticipantsModels.User.Department",
                    "OtherParticipantModels",
                    "UserModel",
                    "LocationModel",
                    "ScheduleFilesAttachment"
        });
      //var foundItem = await _repository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
      //  new string[] {
      //              "ParticipantsModels.User.Department",
      //              "OtherParticipantModels",
      //              "UserModel",
      //              "LocationModel",
      //              "ScheduleFilesAttachment"
      //  });
      DateTime curDate = Convert.ToDateTime(currentDate);
      allItemFound = allItemFound.Where(x => Convert.ToDateTime(x.ScheduleTime) >= curDate).ToList();
      var foundItem = new ScheduleModel();
      if (userId == -1 && userEmail != null)
      {
        foundItem = allItemFound.FirstOrDefault(x => x.OtherParticipantModels.FirstOrDefault(y => y.Email.ToLower() == userEmail.ToLower()) != null);
      }
      else if (userId != -1)
      {
        foundItem = allItemFound.FirstOrDefault(x => x.ParticipantsModels.FirstOrDefault(y => y.UserId == userId) != null);
      }
      else
      {
        _logger.LogWarning($"Schedule is not found");
        throw new BusinessException($"Schedule không tìm thấy", StatusCodes.Status404NotFound);
      }
      if (foundItem == null)
      {
        _logger.LogWarning($"schedule is not found");
        throw new BusinessException($"schedule không tìm thấy", StatusCodes.Status404NotFound);
      }

      var mapResult = _mapper.Map<ScheduleForDetailDto>(foundItem);

      mapResult.ScheduleLocation = foundItem.LocationModel != null && string.IsNullOrEmpty(foundItem.OtherLocation) ? foundItem.LocationModel.Title : string.Empty;

      if (foundItem.OtherParticipantModels.Any())
      {
        var mapOther = _mapper.Map<List<OtherParticipantDto>>(foundItem.OtherParticipantModels);
        mapResult.OtherParticipants = mapOther;
      }

      if (foundItem.ParticipantsModels.Any())
      {
        mapResult.ParticipantIsSelected = _mapper.Map<List<ParticipantIsSelectedDto>>(foundItem.ParticipantsModels);
      }

      if (foundItem.ScheduleFilesAttachment.Any())
      {
        mapResult.ScheduleFilesAttachment = _mapper.Map<List<ScheduleFilesAttachmentDto>>(foundItem.ScheduleFilesAttachment);
        var hostURL = _appSettings.HostURL;
        foreach (var scheduleFileAttachment in mapResult.ScheduleFilesAttachment)
        {
          scheduleFileAttachment.FilePath=$"{hostURL}\\{scheduleFileAttachment.FilePath}";
        }
      }

      return mapResult;
    }


  }
}
