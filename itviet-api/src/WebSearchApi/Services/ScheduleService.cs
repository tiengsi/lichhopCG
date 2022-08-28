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
using WebApi.Data;
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
    Task ReleaseSchedule(ScheduleReleasePayload payload);
    Task ReleaseScheduleById(int id);

    Task<int> ApproveScheduleByIdAsync(int scheduleId);
    // hoãn lịch
    Task<int> PauseScheduleByIdAsync(ScheduleForDetailDto schedule);
    // dời lịch
    Task<int> ChangeScheduleAsync(ScheduleForDetailDto schedule);

  }

  public partial class ScheduleService : IScheduleService
  {
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IParticipantsRepository _participantRepository;
    private readonly IParticipantsTemplateRepository _participantTemplateRepository;
    private readonly IJobScheduleService _jobScheduleService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ScheduleService> _logger;
    private readonly IMapper _mapper;
    private readonly IOtherParticipantRepository _otherParticipantRepository;
    private readonly IOtherParticipantTemplateRepository _otherParticipantTemplateRepository;
    private readonly IEmailAndSmsService _emailAndSmsService;
    private readonly IEmailLogsRepository _emailLogsRepository;
    private readonly IAuditScheduleRepository _auditScheduleRepository;
    private readonly IScheduleTemplateRepository _scheduleTemplateRepository;
    private readonly IScheduleFilesAttachmentRepository _attachmentRepository;
    private readonly IScheduleFilesAttachmentService _filesAttachmentService;
    private readonly IScheduleHistoryRepository _scheduleHistoryRepository;
    private readonly AppSettings _appSettings;



    public ScheduleService(IScheduleRepository repository, IUnitOfWork unitOfWork, ILogger<ScheduleService> logger,
        IMapper mapper,
        IParticipantsRepository participantRepository,
        IParticipantsTemplateRepository participantTemplateRepository,
        IJobScheduleService jobScheduleService,
        IOtherParticipantRepository otherParticipantRepository,
        IOtherParticipantTemplateRepository otherParticipantTemplateRepository,
        IEmailAndSmsService emailAndSmsService,
        IEmailLogsRepository emailLogsRepository,
        IAuditScheduleRepository auditScheduleRepository,
        IScheduleTemplateRepository scheduleTemplateRepository,
        IScheduleFilesAttachmentRepository attachmentRepository,
        IScheduleFilesAttachmentService filesAttachmentService,
        IScheduleHistoryRepository scheduleHistoryRepository,
         IOptions<AppSettings> appSettings)
    {
      _scheduleRepository = repository;
      _unitOfWork = unitOfWork;
      _logger = logger;
      _mapper = mapper;
      _participantRepository = participantRepository;
      _participantTemplateRepository = participantTemplateRepository;
      _jobScheduleService = jobScheduleService;
      _otherParticipantRepository = otherParticipantRepository;
      _otherParticipantTemplateRepository = otherParticipantTemplateRepository;
      _emailAndSmsService = emailAndSmsService;
      _emailLogsRepository = emailLogsRepository;
      _auditScheduleRepository = auditScheduleRepository;
      _scheduleTemplateRepository = scheduleTemplateRepository;
      _attachmentRepository = attachmentRepository;
      _filesAttachmentService = filesAttachmentService;
      _scheduleHistoryRepository=scheduleHistoryRepository;
      _appSettings=appSettings.Value;

    }

    private async Task<FunctionResult> CheckBusinessRulesOfSchedule(ScheduleForAddDto model)
    {
      var result = new FunctionResult();
      if (model.OtherLocation == null) model.OtherLocation = "";
      if (model.OtherHost == null) model.OtherHost = "";

      // 1. check trung người chủ trì và địa điểm trong hệ thống và cùng thời gian
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule =>
          schedule.ScheduleDate.Date == model.ScheduleDate.Date &&
          schedule.ScheduleDate.Month == model.ScheduleDate.Month &&
          schedule.ScheduleDate.Year == model.ScheduleDate.Year &&
          schedule.ScheduleTime.Equals(model.ScheduleTime) &&
          schedule.Id == model.Id &&
          schedule.LocationId == model.LocationId &&
          schedule.OrganizeId == model.OrganizeId &&
          schedule.Id != null && schedule.LocationId != null;

      var isExist = await _scheduleRepository.CheckContains(baseFilter);
      if (isExist)
      {
        result.AddError("Địa điểm, người chủ trì của lịch họp trùng với thời gian của lịch họp khác, vui lòng chọn địa điểm hoặc người chủ trì hoặc thời gian khác");
      }

      // 2. check trùng người chủ trì và địa điểm ngoài hệ thống và cùng thời gian
      baseFilter = schedule =>
          schedule.ScheduleDate.Date == model.ScheduleDate.Date &&
          schedule.ScheduleDate.Month == model.ScheduleDate.Month &&
          schedule.ScheduleDate.Year == model.ScheduleDate.Year &&
          schedule.ScheduleTime.Equals(model.ScheduleTime) &&
          schedule.OtherHost.ToLower().Equals(model.OtherHost.ToLower()) &&
          schedule.OtherLocation.ToLower().Equals(model.OtherLocation.ToLower()) &&
          schedule.OrganizeId ==model.OrganizeId &&
          model.Id == null && model.LocationId == null;

      isExist = await _scheduleRepository.CheckContains(baseFilter);
      if (isExist)
      {
        result.AddError("Địa điểm, người chủ trì của lịch họp trùng với thời gian của lịch họp khác, vui lòng chọn địa điểm hoặc người chủ trì hoặc thời gian khác");
      }

      // 3. check trùng người chủ trì trong hệ thống và địa điểm ngoài thệ thống và cùng thời gian
      baseFilter = schedule =>
          schedule.ScheduleDate.Date == model.ScheduleDate.Date &&
          schedule.ScheduleDate.Month == model.ScheduleDate.Month &&
          schedule.ScheduleDate.Year == model.ScheduleDate.Year &&
          schedule.ScheduleTime.Equals(model.ScheduleTime) &&
          schedule.Id == model.Id &&
          schedule.OrganizeId ==model.OrganizeId &&
          schedule.OtherLocation.ToLower().Equals(model.OtherLocation.ToLower()) &&
          model.Id != null && model.LocationId == null;

      isExist = await _scheduleRepository.CheckContains(baseFilter);
      if (isExist)
      {
        result.AddError("Địa điểm, người chủ trì của lịch trùng với thời gian của lịch họp khác, vui lòng chọn địa điểm hoặc người chủ trì hoặc thời gian khác");
      }

      // 4. check trùng người chủ trì ngoài hệ thống và địa điểm trong thệ thống và cùng thời gian
      baseFilter = schedule =>
          schedule.ScheduleDate.Date == model.ScheduleDate.Date &&
          schedule.ScheduleDate.Month == model.ScheduleDate.Month &&
          schedule.ScheduleDate.Year == model.ScheduleDate.Year &&
          schedule.ScheduleTime.Equals(model.ScheduleTime) &&
          schedule.LocationId == model.LocationId &&
          schedule.OrganizeId ==model.OrganizeId &&
          schedule.OtherHost.ToLower().Equals(model.OtherHost.ToLower()) &&
          model.Id == null && model.LocationId != null;

      isExist = await _scheduleRepository.CheckContains(baseFilter);
      if (isExist)
      {
        result.AddError("Địa điểm, người chủ trì của lịch họp trùng với thời gian của lịch họp khác, vui lòng chọn địa điểm hoặc người chủ trì hoặc thời gian khác");
      }

      return result;
    }

    private FunctionResult CheckRequireField(ScheduleForAddDto model)
    {
      var result = new FunctionResult();

      if (string.IsNullOrEmpty(model.ScheduleTitle))
      {
        result.AddError("ScheduleTitle is required");
      }

      if (string.IsNullOrEmpty(model.ScheduleDate.ToString()))
      {
        result.AddError("ScheduleDate is required");
      }

      if (string.IsNullOrEmpty(model.ScheduleTime))
      {
        result.AddError("ScheduleTime is required");
      }
      if (model.OrganizeId<=0)
      {
        result.AddError("OrganizeId is required");
      }
      return result;
    }

    private FunctionResult CheckRequireFieldWhenUpdateSchedule(ScheduleForAddDto model)
    {
      var result = new FunctionResult();

      if (model.ScheduleId == 0)
      {
        result.AddError("ScheduleId is required");
      }
      if (model.OrganizeId <= 0)
      {
        result.AddError("OrganizeId is required");
      }

      if (string.IsNullOrEmpty(model.ScheduleDate.ToString()))
      {
        result.AddError("ScheduleDate is required");
      }

      if (string.IsNullOrEmpty(model.ScheduleTime))
      {
        result.AddError("ScheduleTime is required");
      }
      if(model.SendSMSFlagForJob==false && model.IsAutoSendAtScheduledTime==true && (model.BrandNameId<=0)) result.AddError("BrandName rỗng khi chế độ hẹn giờ gửi SMS và email đang được thiết lập");
      if(model.SendSMSFlagForJob==true && (model.BrandNameId<=0)) result.AddError("BrandName rỗng không thể gửi SMS, vui lòng thử lại");
      return result;
    }


    private async Task<bool> CheckExistScheduleTemplate(ScheduleTemplateForAddDto model)
    {
      if (model.OtherLocation == null) model.OtherLocation = "";
      if (model.OtherHost == null) model.OtherHost = "";

      // 1. check trung người chủ trì và địa điểm trong hệ thống và cùng thời gian
      Expression<Func<ScheduleTemplateModel, bool>> baseFilter = schedule =>
          schedule.ScheduleTime.Equals(model.ScheduleTime) &&
          schedule.Id == model.Id && schedule.LocationId == model.LocationId &&
          schedule.Id != null && schedule.LocationId != null;

      var isExist = await _scheduleTemplateRepository.CheckContains(baseFilter);
      if (isExist)
      {
        throw new BusinessException($"Địa điểm, người chủ trì của lịch họp trùng với thời gian của lịch họp khác, vui lòng chọn địa điểm hoặc người chủ trì hoặc thời gian khác", StatusCodes.Status409Conflict);
      }

      // 2. check trùng người chủ trì và địa điểm ngoài hệ thống và cùng thời gian
      baseFilter = schedule =>
          schedule.ScheduleTime.Equals(model.ScheduleTime) &&
          schedule.OtherHost.ToLower().Equals(model.OtherHost.ToLower()) &&
          schedule.OtherLocation.ToLower().Equals(model.OtherLocation.ToLower()) &&
          model.Id == null && model.LocationId == null;

      isExist = await _scheduleTemplateRepository.CheckContains(baseFilter);
      if (isExist)
      {
        throw new BusinessException($"Địa điểm, người chủ trì của lịch họp trùng với thời gian của lịch họp khác, vui lòng chọn địa điểm hoặc người chủ trì hoặc thời gian khác", StatusCodes.Status409Conflict);
      }

      // 3. check trùng người chủ trì trong hệ thống và địa điểm ngoài thệ thống và cùng thời gian
      baseFilter = schedule =>
          schedule.ScheduleTime.Equals(model.ScheduleTime) &&
          schedule.Id == model.Id &&
          schedule.OtherLocation.ToLower().Equals(model.OtherLocation.ToLower()) &&
          model.Id != null && model.LocationId == null;

      isExist = await _scheduleTemplateRepository.CheckContains(baseFilter);
      if (isExist)
      {
        throw new BusinessException($"Địa điểm, người chủ trì của lịch trùng với thời gian của lịch họp khác, vui lòng chọn địa điểm hoặc người chủ trì hoặc thời gian khác", StatusCodes.Status409Conflict);
      }

      // 4. check trùng người chủ trì ngoài hệ thống và địa điểm trong thệ thống và cùng thời gian
      baseFilter = schedule =>
          schedule.ScheduleTime.Equals(model.ScheduleTime) &&
          schedule.LocationId == model.LocationId &&
          schedule.OtherHost.ToLower().Equals(model.OtherHost.ToLower()) &&
          model.Id == null && model.LocationId != null;

      isExist = await _scheduleTemplateRepository.CheckContains(baseFilter);
      if (isExist)
      {
        throw new BusinessException($"Địa điểm, người chủ trì của lịch họp trùng với thời gian của lịch họp khác, vui lòng chọn địa điểm hoặc người chủ trì hoặc thời gian khác", StatusCodes.Status409Conflict);
      }

      return false;
    }


    private async Task AddOrUpdateFilesAttachment(ScheduleModel foundItem, ScheduleForAddDto model)
    {
      var filesAttachment = model.ScheduleFilesAttachment;
      var listFilesExist = await _attachmentRepository.GetMulti(r => r.ScheduleId == model.ScheduleId);
      var toAdd = filesAttachment.Where(r => !listFilesExist.Exists(f => f.Id == r.Id)).ToList();
      var toUpdateBefore = listFilesExist.Where(r => filesAttachment.Exists(f => f.Id == r.Id)).ToList();
      var toUpdate = filesAttachment.Where(r => listFilesExist.Exists(f => f.Id == r.Id)).ToList();
      var toDelete = listFilesExist.Where(r => !filesAttachment.Exists(f => f.Id == r.Id)).ToList();

      if (foundItem.ScheduleFilesAttachment == null && toAdd.Count > 0)
      {
        foundItem.ScheduleFilesAttachment = new List<ScheduleFilesAttachmentModel>();
      }

      foreach (var item in toAdd)
      {
        var addItem = _mapper.Map<ScheduleFilesAttachmentModel>(item);
        addItem.ScheduleId = model.ScheduleId;
        foundItem.ScheduleFilesAttachment.Add(addItem);
      }

      foreach (var item in toUpdateBefore)
      {
        var updateItem = _mapper.Map<ScheduleFilesAttachmentModel>(item);
        foundItem.ScheduleFilesAttachment.Remove(updateItem);
      }

      foreach (var item in toUpdate)
      {
        var itemUpdate = listFilesExist.FirstOrDefault(r => r.Id == item.Id);
        itemUpdate.NotationNumber = item.NotationNumber;
        itemUpdate.ReleaseDate = item.ReleaseDate;
        itemUpdate.Quote = item.Quote;
        foundItem.ScheduleFilesAttachment.Add(itemUpdate);
      }

      foreach (var item in toDelete)
      {
        var deleteItem = _mapper.Map<ScheduleFilesAttachmentModel>(item);
        foundItem.ScheduleFilesAttachment.Remove(deleteItem);
      }

    }


    public async Task<int> ApproveScheduleByIdAsync(int scheduleId)
    {
      var foundItem = await _scheduleRepository.GetSingleById(scheduleId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Not found schedule {scheduleId}");
        throw new BusinessException($"Không tìm thấy lịch họp {scheduleId}", StatusCodes.Status404NotFound);
      }

      if (foundItem.ScheduleStatus == EScheduleStatus.Approve)
      {
        _logger.LogWarning($"Lịch này đã được duyệt trước đó rồi");
        throw new BusinessException($"Lịch này đã được duyệt trước đó rồi", StatusCodes.Status208AlreadyReported);
      }

      var auditSchedule = CreateAuditSchedule(foundItem.ScheduleStatus, EScheduleStatus.Approve, foundItem.ScheduleId);
      if (auditSchedule != null) _auditScheduleRepository.Add(auditSchedule);

      foundItem.ScheduleStatus = EScheduleStatus.Approve;
      _scheduleRepository.Update(foundItem);
      _unitOfWork.Commit();

      if (foundItem.IsSendEmail)
      {
        await _jobScheduleService.CreateManualJob(scheduleId, EJobScheduleType.EMAIL, EScheduleStatus.Approve);
      }

      if (foundItem.ISendSMS)
      {
        await _jobScheduleService.CreateManualJob(scheduleId, EJobScheduleType.SMS, EScheduleStatus.Approve);
      }

      return foundItem.ScheduleId;
    }

    public async Task<int> PauseScheduleByIdAsync(ScheduleForDetailDto schedule)
    {
      var foundItem = await _scheduleRepository.GetSingleById(schedule.ScheduleId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Not found schedule {schedule.ScheduleId}");
        throw new BusinessException($"Không tìm thấy lịch họp {schedule.ScheduleId}", StatusCodes.Status404NotFound);
      }

      if (foundItem.ScheduleStatus == EScheduleStatus.Pending)
      {
        _logger.LogWarning($"Lịch này chưa được duyệt, nên không thể sử dụng chức năng hoãn lịch");
        throw new BusinessException($"Lịch này chưa được duyệt, nên không thể sử dụng chức năng hoãn lịch", StatusCodes.Status208AlreadyReported);
      }

      if (foundItem.ScheduleStatus == EScheduleStatus.Pause)
      {
        _logger.LogWarning($"Lịch này đã được hủy trước đó rồi");
        throw new BusinessException($"Lịch này đã được hủy trước đó rồi", StatusCodes.Status208AlreadyReported);
      }

      var auditSchedule = CreateAuditSchedule(schedule.ScheduleStatus, EScheduleStatus.Pause, foundItem.ScheduleId);
      if (auditSchedule != null) _auditScheduleRepository.Add(auditSchedule);

      foundItem.ScheduleStatus = EScheduleStatus.Pause;
      foundItem.IsActive = true;

      _scheduleRepository.Update(foundItem);
      _unitOfWork.Commit();
      try
      {

     
      if (schedule.IsSendEmail)
      {
        await _emailAndSmsService.SendEmailForPause(schedule, foundItem);
      }

      if (schedule.ISendSMS)
      {
        await _emailAndSmsService.SendSmsForPause(schedule.ScheduleId);
      }
      }
      catch (Exception ex)
      {

        throw ex;
      } 
      return foundItem.ScheduleId;
    }

    public async Task<int> ChangeScheduleAsync(ScheduleForDetailDto schedule)
    {
      var foundItem = await _scheduleRepository.GetSingleById(schedule.ScheduleId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Not found schedule {schedule.ScheduleId}");
        throw new BusinessException($"Không tìm thấy lịch họp {schedule.ScheduleId}", StatusCodes.Status404NotFound);
      }

      if (foundItem.ScheduleStatus == EScheduleStatus.Pending)
      {
        _logger.LogWarning($"Lịch này chưa được duyệt, nên không thể sử dụng chức năng dời lịch");
        throw new BusinessException($"Lịch này chưa được duyệt, nên không thể sử dụng chức năng dời lịch", StatusCodes.Status208AlreadyReported);
      }

      bool isChangeLocation = false;
      if (foundItem.LocationId == null)
      {
        if (!string.IsNullOrEmpty(foundItem.OtherLocation) && schedule.OtherLocation != foundItem.OtherLocation)
        {
          isChangeLocation = true;
        }

        if (!string.IsNullOrEmpty(foundItem.OtherLocation) && string.IsNullOrEmpty(schedule.OtherLocation) && schedule.LocationId != null)
        {
          isChangeLocation = true;
        }
      }
      else
      {
        if (!string.IsNullOrEmpty(schedule.OtherLocation) && schedule.LocationId == null)
        {
          isChangeLocation = true;
        }

        if (foundItem.LocationId != schedule.LocationId)
        {
          isChangeLocation = true;
        }
      }
      var auditSchedule = CreateAuditSchedule(foundItem.ScheduleStatus, EScheduleStatus.Changed, foundItem.ScheduleId);
      if (auditSchedule != null) _auditScheduleRepository.Add(auditSchedule);

      foundItem.ConvertToScheduleModel(schedule);
      foundItem.IsChangeLocation = isChangeLocation;
      foundItem.ScheduleStatus = EScheduleStatus.Changed;

      if (foundItem.IsChangeLocation)
      {
        var auditScheduleUpdate = new AuditScheduleModel()
        {
          ChangeFrom = string.Empty,
          ChangeTo = "Đã đổi địa điểm",
          ChangeDate = DateTime.Now,
          ScheduleId = foundItem.ScheduleId
        };

        _auditScheduleRepository.Add(auditScheduleUpdate);
      }

      _scheduleRepository.Update(foundItem);
      _unitOfWork.Commit();
      if (schedule.IsSendEmail)
      {
        await _emailAndSmsService.SendEmailForChange(foundItem.ScheduleId);
      }
      if (schedule.ISendSMS)
      {
        await _emailAndSmsService.SendSmsForMoved(schedule.ScheduleId);
      }
      return foundItem.ScheduleId;
    }

    public async Task ReleaseScheduleById(int id)
    {
      var entity = await _scheduleRepository.GetSingleById(id);
      var auditSchedule = CreateAuditSchedule(entity.ScheduleStatus, EScheduleStatus.Release, entity.ScheduleId);
      if (auditSchedule != null) _auditScheduleRepository.Add(auditSchedule);
      entity.IsActive = true;
      entity.ScheduleStatus = EScheduleStatus.Release;
      _scheduleRepository.Update(entity);
      _unitOfWork.Commit();
    }
    public async Task ReleaseSchedule(ScheduleReleasePayload payload)
    {
      Expression<Func<ScheduleModel, bool>> baseFilter = schedule => true;
      if (!string.IsNullOrEmpty(payload.StartDate))
      {
        DateTime date = DateTime.Parse(payload.StartDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate >= date);
      }

      if (!string.IsNullOrEmpty(payload.EndDate))
      {
        DateTime pEndDate = DateTime.Parse(payload.EndDate);
        baseFilter = baseFilter.And(x => x.ScheduleDate <= pEndDate);
      }

      var result = await _scheduleRepository.GetMulti(baseFilter);

      foreach (var item in result)
      {
        var auditSchedule = CreateAuditSchedule(item.ScheduleStatus, EScheduleStatus.Release, item.ScheduleId);
        if (auditSchedule != null) _auditScheduleRepository.Add(auditSchedule);

        item.IsActive = true;
        item.ScheduleStatus = EScheduleStatus.Release;
        _scheduleRepository.Update(item);
      }

      _unitOfWork.Commit();
    }

    private AuditScheduleModel CreateAuditSchedule(EScheduleStatus changeForm, EScheduleStatus changeTo, int scheduleId)
    {
      if (changeForm == changeTo)
      {
        return null;
      }

      var changeFormStr = GetSchuduleStatus(changeForm);
      var changeToStr = GetSchuduleStatus(changeTo);
      var model = new AuditScheduleModel()
      {
        ChangeFrom = changeFormStr,
        ChangeTo = changeToStr,
        ChangeDate = DateTime.Now,
        ScheduleId = scheduleId
      };

      return model;
    }

    private FunctionResult CheckBRScheduledResultReport(ScheduledResultReportModel model)
    {
      var result = new FunctionResult();
      if (string.IsNullOrEmpty(model.Path)==true) result.AddError("Không tìm thấy đường dẫn của file");
      if (model.UserId<=0) result.AddError("UserId phải có giá trị dương");
      return result;
    }
    private FunctionResult CheckBRPersonalSchedule(PersonalScheduleModel model)
    {
      var result = new FunctionResult();
      if (model.UserId<=0) result.AddError("UserId không hợp lệ");
      if (string.IsNullOrEmpty(model.Title)) result.AddError("Title không được rỗng");
      //if (model.Fromdate.Date>=model.ToDate.Date) result.AddError("Ngày đến bé hơn ngày bắt đầu");
      return result;
    }

    private FunctionResult CheckBRPersonalNotes(PersonalNotesModel model)
    {
      var result = new FunctionResult();
      if (string.IsNullOrEmpty(model.Title)) result.AddError("Title rỗng");
      if (model.UserId<=0) result.AddError("UserId phải có giá trị dương");
      if (model.PersonalNotesId<=0) result.AddError("PersonalNotesId phải có giá trị dương");
      return result;
    }

    private FunctionResult CheckBRScheduledResultDocument(ScheduledResultDocumentModel model)
    {
      var result = new FunctionResult();
      if (string.IsNullOrEmpty(model.Title)) result.AddError("Title rỗng");
      if (string.IsNullOrEmpty(model.Path)) result.AddError("Giá trị Path không hợp lệ");
      if (model.ScheduledResultDocumentId<=0) result.AddError("ScheduledResultDocumentId phải có giá trị dương");
      return result;
    }

    private async Task<FunctionResult> DeleteParticipantsAndOtherParticipantsWhenUpdateScheduleAsync(ScheduleForAddDto model)
    {
      var result = new FunctionResult();
      var oldParticipants = await _participantRepository.GetMulti(x => x.ScheduleId == model.ScheduleId);
      foreach (var item in oldParticipants)
      {
        _participantRepository.Delete(item);
      }

      var oldOtherParticipants = await _otherParticipantRepository.GetMulti(x => x.ScheduleId == model.ScheduleId);
      foreach (var item in oldOtherParticipants)
      {
        _otherParticipantRepository.Delete(item);
      }

      return result;
    }

    private ScheduleModel AddParticipantsAndOtherParticipantsWhenUpdateSchedule(ScheduleForAddDto model,ScheduleModel foundItem)
    {     
    
      foreach (var id in model.UserIds)
      {
        foundItem.ParticipantsModels.Add(new ParticipantsModel()
        {
          UserId = id,
          ScheduleId = foundItem.ScheduleId
        });
      }

      // add new other participant
      foundItem.OtherParticipantModels = new List<OtherParticipantModel>();
      if (model.OtherParticipants.Any())
      {
        foreach (var item in model.OtherParticipants)
        {
          if (!string.IsNullOrEmpty(item.Name))
          {
            var otherParticipant = new OtherParticipantModel();
            otherParticipant.UpdateOtherParticipant(item);
            foundItem.OtherParticipantModels.Add(otherParticipant);
          }
        }
      }
      return foundItem;
    }


  }
}
