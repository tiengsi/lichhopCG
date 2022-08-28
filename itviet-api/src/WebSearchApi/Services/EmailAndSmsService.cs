using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Enums;
using WebApi.Models.Settings;

namespace WebApi.Services
{
  public interface IEmailAndSmsService
  {
    Task<bool> SendSmsForApprove(int scheduleId);
    Task SendSmsForPause(int scheduleId);
    Task SendSmsForMoved(int scheduleId);
    Task<string> SendSMS(SendSmsDto model);
    Task<bool> SendEmailForApprove(int scheduleId);
    Task SendEmailForChange(int scheduleId);
    Task SendEmailForPause(ScheduleForDetailDto scheduleDto, ScheduleModel schedule);
  }

  public class EmailAndSmsService : IEmailAndSmsService
  {
    private readonly IScheduleRepository _repository;
    private readonly IParticipantsRepository _repositoryParticipants;
    private readonly IEmailLogsService _emailLogsService;
    private readonly IHelperService _helperService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmailAndSmsService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ISettingRepository _repositorySetting;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly Helpers.UploadSettings _uploadSettings;
    private readonly AppSettings _appSettings;

    public EmailAndSmsService(IScheduleRepository repository,
        IUnitOfWork unitOfWork,
         IMapper mapper,
        ILogger<EmailAndSmsService> logger,
        IEmailLogsService emailLogsService,
        IHelperService helperService,
        IUserRepository userRepository,
        ISettingRepository repositorySetting,
        IEmailTemplateRepository emailTemplateRepository,
         IOptions<AppSettings> appSettings,

        IParticipantsRepository repositoryParticipants,
         IOptions<Helpers.UploadSettings> uploadSettings)
    {
      _repository = repository;
      _unitOfWork = unitOfWork;
      _logger = logger;
      _emailLogsService = emailLogsService;
      _helperService = helperService;
      _userRepository = userRepository;
      _repositoryParticipants = repositoryParticipants;
      _mapper = mapper; ;
      _repositorySetting = repositorySetting;
      _appSettings = appSettings.Value;
      _emailTemplateRepository = emailTemplateRepository;
      _uploadSettings = uploadSettings.Value;
    }
    //const string urlHost = _appSettings.HostURL;
    #region Send SMS

    public async Task<bool> SendSmsForApprove(int scheduleId)
    {
      var isSendSMSAllSuccess = true;
      //var foundItem = await _repository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
      //    new string[] { "ParticipantsModels.User", "LocationModel", "OtherParticipantModels", "UserModel", "ScheduleTitleTemplateModel" });

      //var titleTemplate = foundItem.ScheduleTitleTemplateModel.Template;
      //var smsContent = $"{titleTemplate} {foundItem.ScheduleTitle}, vào lúc {foundItem.ScheduleTime} ngày {foundItem.ScheduleDate.ToString("dd/MM/yyyy")}, đồng chí ";

      //smsContent = foundItem.Id == null ? smsContent + foundItem.OtherHost : smsContent + foundItem.UserModel.FullName;
      //smsContent += " chủ trì, tại ";
      //smsContent = foundItem.LocationId == null ? smsContent + foundItem.OtherLocation : smsContent + foundItem.LocationModel.Title;
      //smsContent += " (Xem chi tiết trên ứng dụng Lịch họp trực tuyến)";

      //var foundItem = await _repository.GetSingleById(scheduleId);
      var foundItem = await _repository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
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
        // throw new BusinessException($"scheduleId {scheduleId} không tìm thấy", StatusCodes.Status404NotFound);
      }


      var smsContent = Ultil.ChuyenCoDauThanhKhongDau(foundItem.MessageContent);

      var userSent = new List<int>();
      var isSendAgainToAll = false;
      //Participant
      var isSendSuccess = await SendSMSForParticipantsModelsAsync(foundItem.ParticipantsModels.ToList(), userSent, smsContent, isSendAgainToAll, scheduleId, foundItem.OrganizeId);
      //Other Participant
      isSendSuccess = await SendSMSForOtherParticipantsModelsAsync(foundItem.OtherParticipantModels.ToList(), userSent, smsContent, isSendAgainToAll, scheduleId, foundItem.OrganizeId);

      // người chủ trì
      isSendSuccess = await SendSMSForHostAsync(userSent, smsContent, foundItem, isSendAgainToAll, scheduleId, foundItem.OrganizeId);

      var setting = await _repositorySetting.GetSingleByCondition(p => p.SettingKey.Equals("Mobiles"));
      if (setting != null && string.IsNullOrEmpty(setting.SettingValue) == false)
      {
        var mobilearr = setting.SettingValue.Split(";");
        if (mobilearr.Any())
        {
          foreach (var phone in mobilearr)
          {
            await SendSmsAndCreateLog(phone, smsContent, foundItem.Id, scheduleId, null, foundItem.OrganizeId, false);
          }
        }
      }
      _unitOfWork.Commit();
      return isSendSMSAllSuccess;
    }

    // thông báo hoãn lịch
    public async Task SendSmsForPause(int scheduleId)
    {
      var foundItem = await _repository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
          new string[] { "ParticipantsModels.User", "LocationModel", "OtherParticipantModels", "UserModel" });

      var smsContent = $"Thong bao hoan họp: {foundItem.ScheduleTitle}, lúc {foundItem.ScheduleTime} ngày {foundItem.ScheduleDate.ToString("dd/MM/yyyy")} ";

      smsContent = foundItem.Id == null ? (String.IsNullOrEmpty(foundItem.OtherHost) ? smsContent : smsContent + " do dong chi " + foundItem.OtherHost + " chủ trì,") : (String.IsNullOrEmpty(foundItem.UserModel.FullName) ? smsContent : smsContent + " do dong chi " + foundItem.UserModel.FullName + " chủ trì,");
      smsContent += "";
      smsContent = foundItem.LocationId == null ? smsContent + "" + foundItem.OtherLocation : smsContent + " tại " + foundItem.LocationModel.Title;
      smsContent += "Cac don vi xem chi tiet noi dung tai dia chi " + _appSettings.HostURL + "/scheduler/schedule-detail/?sid=" + scheduleId;

      smsContent = Ultil.ChuyenCoDauThanhKhongDau(smsContent);

      var userSent = new List<int>();

      var isSendSuccess = await SendSMSForParticipantsModelsAsync(foundItem.ParticipantsModels.ToList(), userSent, smsContent, true, scheduleId, foundItem.OrganizeId);

      isSendSuccess = await SendSMSForOtherParticipantsModelsAsync(foundItem.OtherParticipantModels.ToList(), userSent, smsContent, true, scheduleId, foundItem.OrganizeId);

      // người chủ trì
      isSendSuccess = await SendSMSForHostAsync(userSent, smsContent, foundItem, true, scheduleId, foundItem.OrganizeId);


      var setting = await _repositorySetting.GetSingleByCondition(p => p.SettingKey.Equals("Mobiles"));
      if (setting != null && string.IsNullOrEmpty(setting.SettingValue) == false)
      {
        var mobilearr = setting.SettingValue.Split(";");
        if (mobilearr.Any())
        {
          foreach (var phone in mobilearr)
          {
            await SendSmsAndCreateLog(phone, smsContent, foundItem.Id, scheduleId, null, foundItem.OrganizeId, true);
          }
        }
      }


      _unitOfWork.Commit();
    }

    // dời lịch họp
    public async Task SendSmsForMoved(int scheduleId)
    {
      var foundItem = await _repository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
          new string[] { "ParticipantsModels.User", "LocationModel", "OtherParticipantModels", "UserModel" });

      var smsContent = $"Thong bao thay đổi lịch họp: {foundItem.ScheduleTitle}, sẽ diễn ra lúc {foundItem.ScheduleTime} ngày {foundItem.ScheduleDate.ToString("dd/MM/yyyy")} ";

      smsContent = foundItem.Id == null ? smsContent + foundItem.OtherHost : smsContent + " do dong chi " + foundItem.UserModel.FullName + " chủ trì,";
      smsContent += " tại ";
      smsContent = foundItem.LocationId == null ? smsContent + foundItem.OtherLocation : smsContent + foundItem.LocationModel.Title;
      smsContent += " (Cac don vi xem chi tiet noi dung tai dia chi " + _appSettings.HostURL + "/scheduler/schedule-detail/?sid=" + scheduleId + ")";

      smsContent = Ultil.ChuyenCoDauThanhKhongDau(smsContent);

      var userSent = new List<int>();

      var isSendSuccess = await SendSMSForParticipantsModelsAsync(foundItem.ParticipantsModels.ToList(), userSent, smsContent, true, scheduleId, foundItem.OrganizeId);

      isSendSuccess = await SendSMSForOtherParticipantsModelsAsync(foundItem.OtherParticipantModels.ToList(), userSent, smsContent, true, scheduleId, foundItem.OrganizeId);

      // người chủ trì
      isSendSuccess = await SendSMSForHostAsync(userSent, smsContent, foundItem, true, scheduleId, foundItem.OrganizeId);



      var setting = await _repositorySetting.GetSingleByCondition(p => p.SettingKey.Equals("Mobiles"));
      if (setting != null && string.IsNullOrEmpty(setting.SettingValue) == false)
      {
        var mobilearr = setting.SettingValue.Split(";");
        if (mobilearr.Any())
        {
          foreach (var phone in mobilearr)
          {
            await SendSmsAndCreateLog(phone, smsContent, foundItem.Id, scheduleId, null, foundItem.OrganizeId, true);
          }
        }
      }

      _unitOfWork.Commit();
    }

    public async Task<string> SendSMS(SendSmsDto model)
    {
      var smsContent = Ultil.ChuyenCoDauThanhKhongDau(model.Content);

      foreach (var phone in model.PhoneNumber)
      {
        await SendSmsAndCreateLog(phone, smsContent, null, null, null, model.OrganizeId, false);
      }

      _unitOfWork.Commit();
      return "Thành công";
    }

    private async Task<bool> SendSmsAndCreateLog(
                                                string phone,
                                                string smsContent,
                                                int? userId,
                                                int? scheduleId,
                                                int? otherPariticpant,
                                                int organizeId,
                                                bool isSendAgain)
    {
      EmailLogsModel foundLog = null;
      var isSend = true;
      var result = 0;
      var emailSmSLogList = new List<EmailLogsModel>();
      if (scheduleId != null) emailSmSLogList = await GetEmailLogByScheduleIdAsync(scheduleId.GetValueOrDefault());
      if (userId != null)
      {
        foundLog = emailSmSLogList.Where(x => x.UserId == userId.GetValueOrDefault() && x.IsSmsLog == true).FirstOrDefault();
      }
      else if (otherPariticpant != null)
      {
        foundLog = emailSmSLogList.Where(x => x.OtherParticipantId == otherPariticpant.GetValueOrDefault() && x.IsSmsLog == true).FirstOrDefault();

      }

      if (foundLog != null)//đã từng gửi sms
      {
        if (isSendAgain == true)//yêu cầu gửi lại ko cần ràng buộc
        {
          isSend = await _helperService.SendSms(phone, smsContent, organizeId);
          foundLog.SendSmsIsSuccess = isSend;
          result = await _emailLogsService.Update(foundLog);
        }
        else//kiểm tra xem lần gửi trước đã thành công chưa, nếu chưa thì gửi lại
        {
          if (foundLog.IsSmsLog == true && foundLog.SendSmsIsSuccess == false)
          {
            isSend = await _helperService.SendSms(phone, smsContent, organizeId);
            foundLog.SendSmsIsSuccess = isSend;
            result = await _emailLogsService.Update(foundLog);
          }
        }

        return isSend;
      }
      else//lần đầu gửi sms
      {
        isSend = await _helperService.SendSms(phone, smsContent, organizeId);
        result = await _emailLogsService.Create(new EmailLogsModel
        {
          ScheduleId = scheduleId,
          SendDate = DateTime.Now,
          SendSmsIsSuccess = isSend,
          UserId = userId,
          OtherParticipantId = otherPariticpant,
          IsSmsLog = true
        });
        return isSend;
      }

    }

    private async Task<bool> SendSMSForParticipantsModelsAsync(List<ParticipantsModel> participantList, List<int> userSent, string smsContent, bool isSendAgainToAll, int scheduleId, int organizeId)
    {
      var isSendAllUserSuccess = true;
      if (participantList.Any())
      {
        foreach (var ptc in participantList)
        {
          if (!string.IsNullOrEmpty(ptc.User.PhoneNumber))
          {
            try
            {
              if (userSent.Any(x => x == ptc.User.Id) == false)
              {
                _logger.LogInformation("ptc.User.PhoneNumber.." + ptc.User.PhoneNumber);
                var isSendSuccess = await SendSmsAndCreateLog(ptc.User.PhoneNumber, smsContent, ptc.User.Id, scheduleId, null, organizeId, isSendAgainToAll);
                if (isSendSuccess == false) isSendAllUserSuccess = false;
                userSent.Add(ptc.User.Id);
              }
            }
            catch (Exception ex)
            {
              isSendAllUserSuccess = false;
            }
          }
        }
      }
      return isSendAllUserSuccess;
    }

    private async Task<bool> SendSMSForOtherParticipantsModelsAsync(List<OtherParticipantModel> otherParticipantList, List<int> userSent, string smsContent, bool isSendAgainToAll, int scheduleId, int organizeId)
    {
      var isSendAllUserSuccess = true;
      if (otherParticipantList.Any())
      {
        _logger.LogInformation("otherParticipantList.." + otherParticipantList.Count);
        foreach (var grMT in otherParticipantList)
        {
          if (!string.IsNullOrEmpty(grMT.PhoneNumber))
          {
            try
            {
              _logger.LogInformation("foundItem.grMT.PhoneNumber.." + grMT.PhoneNumber);
              var isSendSuccess = await SendSmsAndCreateLog(grMT.PhoneNumber, smsContent, null, scheduleId, grMT.OtherParticipantId, organizeId, isSendAgainToAll);
              if (isSendSuccess == false)
              {
                isSendAllUserSuccess = false;
                _logger.LogError("Send SMS Failed: " + grMT.PhoneNumber);
              }
              else
              {
                _logger.LogError("Send SMS Success " + grMT.PhoneNumber);
              }
            }
            catch (Exception)
            {
              isSendAllUserSuccess = false;
              _logger.LogError("Send SMS Failed isSendAllUserSuccess" + grMT.PhoneNumber);
            }

          }
        }
      }
      return isSendAllUserSuccess;
    }

    private async Task<bool> SendSMSForHostAsync(List<int> userSent, string smsContent, ScheduleModel foundItem, bool isSendAgainToAll, int scheduleId, int organizeId)
    {
      var isSendAllUserSuccess = true;

      if (foundItem.UserModel != null)
      {
        if (!string.IsNullOrEmpty(foundItem.UserModel.PhoneNumber))
        {
          try
          {
            if (userSent.Any(x => x==foundItem.Id.GetValueOrDefault())==false)
            {
              var isSendSuccess = await SendSmsAndCreateLog(foundItem.UserModel.PhoneNumber, smsContent, foundItem.Id, scheduleId, null, organizeId, isSendAgainToAll);
              if (isSendSuccess==false) isSendAllUserSuccess = false;
              userSent.Add(foundItem.Id.GetValueOrDefault());
            }
          }
          catch (Exception)
          {
            isSendAllUserSuccess=false;
          }
        }
      }

      return isSendAllUserSuccess;
    }

    private async Task<List<EmailLogsModel>> GetEmailLogByScheduleIdAsync(int scheduleId)
    {
      var foundLogList = await _emailLogsService.GetByScheduleId(scheduleId);
      return foundLogList.ToList();
    }
    #endregion

    #region "Send Email"

    //Duyệt lịch
    public async Task<bool> SendEmailForApprove(int scheduleId)
    {
      var isSendAllUserSuccess = true;
      var foundItem = await _repository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
          new string[] { "ParticipantsModels.User", "LocationModel", "OtherParticipantModels", "UserModel" });

      var emailTitle = "Thư Mời Họp";
      var htmlContent = string.Empty;

      var emailTemplateByOrganizeList = await _emailTemplateRepository.GetListEmailTemplateByOrganizeIdAsync(foundItem.OrganizeId);
      var approvelEmail = emailTemplateByOrganizeList.Where(x => x.TypeEmail == Constants.Approve).FirstOrDefault();
      if (approvelEmail == null)
        htmlContent = DefaultHtmlSampleEmailApprove(scheduleId, foundItem);
      else
      {
        htmlContent = GetHtmlSampleFromDatabaseEmailApprove(scheduleId, foundItem, approvelEmail);
      }
      var isSendAgainToAll = false;
      var userSent = new List<int>();

      var isSendSuccess = await SendEmailForParticipantsModelsAsync(foundItem.ParticipantsModels.ToList(), userSent, emailTitle, htmlContent, foundItem, isSendAgainToAll, scheduleId);


      isSendSuccess = await SendEmailForOtherParticipantsModelsAsync(foundItem.OtherParticipantModels.ToList(), userSent, emailTitle, htmlContent, foundItem, isSendAgainToAll, scheduleId);


      // người chủ trì
      isSendSuccess = await SendEmailForHostAsync(userSent, emailTitle, htmlContent, foundItem, isSendAgainToAll, scheduleId);

      var setting = await _repositorySetting.GetSingleByCondition(p => p.SettingKey.Equals("Emails"));
      if (setting != null && string.IsNullOrEmpty(setting.SettingValue) == false)
      {
        var mobilearr = setting.SettingValue.Split(";");
        if (mobilearr.Any())
        {
          foreach (var email in mobilearr)
          {

            await SendMailAndCreateLogs(email, emailTitle, htmlContent, foundItem.Id, null, foundItem, false, scheduleId);
          }
        }
      }

      _unitOfWork.Commit();
      return isSendAllUserSuccess;
    }

    private string DefaultHtmlSampleEmailApprove(int scheduleId, ScheduleModel foundItem)
    {

      var htmlContent = new StringBuilder();
      var host = foundItem.UserModel == null ? foundItem.OtherHost : $"{foundItem.UserModel.OfficerPosition}-{foundItem.UserModel.FullName}";
      htmlContent.Append($"<p>Mời họp: {foundItem.ScheduleTitle}</p>");
      htmlContent.Append($"<p>Chủ trì: đồng chí {host}</p>");
      htmlContent.Append($"Thời gian: {foundItem.ScheduleTime} - {foundItem.ScheduleDate.ToString("dd/MM/yyyy")}");

      var location = foundItem.LocationModel == null ? foundItem.OtherLocation : foundItem.LocationModel.Title;
      htmlContent.Append($"<p>Địa điểm: {location}</p>");
      htmlContent.Append($"<p>Thành phần: {foundItem.ParticipantDisplay}</p>");
      htmlContent.Append($"{foundItem.ScheduleContent}");

      //if (!string.IsNullOrEmpty(foundItem.FilePath))
      //{
      //  htmlContent.Append($"<p>Vui lòng xem chi tiết <b><a href='{foundItem.FilePath}' target='_blank'>tại đây</a><b></p>");
      //}
      string goToPath = Constants.QR_CODE_URL + "?sid=" + foundItem.ScheduleId;
      htmlContent.Append($"<p>Vui lòng xem chi tiết <b><a href='{goToPath}' target='_blank'>tại đây</a><b></p>");
      return htmlContent.ToString();
    }

    private string GetHtmlSampleFromDatabaseEmailApprove(int scheduleId, ScheduleModel foundItem, EmailTemplateModel emailTemplate)
    {

      var htmlContent = string.Empty;
      var HTMLPath = string.Empty;
      try
      {
        var fileDocumenttPath = $"{Directory.GetCurrentDirectory()}\\Upload\\{emailTemplate.FilePath.Replace(_uploadSettings.EmailTemplate, nameof(_uploadSettings.EmailTemplate))}";
        HTMLPath = fileDocumenttPath.Replace(".docx", ".html");


        using (StreamReader reader = new StreamReader(HTMLPath))
        {
          htmlContent = reader.ReadToEnd();
        }
        htmlContent = RemoveSpace(htmlContent, "TieuDeLich");
        htmlContent = RemoveSpace(htmlContent, "ChuTri");
        htmlContent = RemoveSpace(htmlContent, "GioBatDau");
        htmlContent = RemoveSpace(htmlContent, "NgayBatDau");
        htmlContent = RemoveSpace(htmlContent, "DiaDiem");
        htmlContent = RemoveSpace(htmlContent, "ThanhPhanThamDu");
        htmlContent = RemoveSpace(htmlContent, "NoiDung");
        htmlContent = RemoveSpace(htmlContent, "Link");

        var host = foundItem.UserModel == null ? foundItem.OtherHost : $"{foundItem.UserModel.OfficerPosition}-{foundItem.UserModel.FullName}";
        var location = foundItem.LocationModel == null ? foundItem.OtherLocation : foundItem.LocationModel.Title;


        htmlContent = htmlContent.Replace("{TieuDeLich}", foundItem.ScheduleTitle);
        htmlContent = htmlContent.Replace("{ChuTri}", host);
        htmlContent = htmlContent.Replace("{GioBatDau}", foundItem.ScheduleTime);
        htmlContent = htmlContent.Replace("{NgayBatDau}", foundItem.ScheduleDate.ToString("dd/MM/yyyy"));
        htmlContent = htmlContent.Replace("{DiaDiem}", location);
        htmlContent = htmlContent.Replace("{ThanhPhanThamDu}", foundItem.ParticipantDisplay);
        htmlContent = htmlContent.Replace("{NoiDung}", foundItem.ScheduleContent);
        //htmlContent = htmlContent.Replace("{Link}", foundItem.MeetingLink);
        htmlContent = htmlContent.Replace("{Link}", _appSettings.HostURL + "/scheduler/shared-documents/?sid=" + foundItem.ScheduleId);
        

        return htmlContent;

      }
      catch (Exception ex)
      {
        _logger.LogError("Không thể đổ dữ liệu vào file html: " + HTMLPath);
      }
      return htmlContent;
    }

    private string RemoveSpace(string input, string varname)
    {
      var result = input;

      result = result.Replace($"{{ {varname} }}", $"{{{varname}}}");
      result = result.Replace($"{{{varname} }}", $"{{{varname}}}");
      result = result.Replace($"{{ {varname}}}", $"{{{varname}}}");

      return result;
    }

    //Dời lịch
    public async Task SendEmailForChange(int scheduleId)
    {
      var foundItem = await _repository.GetSingleByCondition(x => x.ScheduleId == scheduleId,
          new string[] { "ParticipantsModels.User", "LocationModel", "OtherParticipantModels" });

      var emailTitle = "Thông báo dời lịch họp";
      var htmlContent = string.Empty;

      var emailTemplateByOrganizeList = await _emailTemplateRepository.GetListEmailTemplateByOrganizeIdAsync(foundItem.OrganizeId);
      var changeEmail = emailTemplateByOrganizeList.Where(x => x.TypeEmail == Constants.Changed).FirstOrDefault();
      if (changeEmail == null)
        htmlContent = DefaultHtmlSampleEmailChange(foundItem);
      else
      {
        htmlContent = GetHtmlSampleFromDatabaseEmailChange(foundItem, changeEmail);
      }

      var userSent = new List<int>();

      var isSendSuccess = await SendEmailForParticipantsModelsAsync(foundItem.ParticipantsModels.ToList(), userSent, emailTitle, htmlContent, foundItem, true, scheduleId);

      isSendSuccess = await SendEmailForOtherParticipantsModelsAsync(foundItem.OtherParticipantModels.ToList(), userSent, emailTitle, htmlContent, foundItem, true, scheduleId);

      // người chủ trì
      isSendSuccess = await SendEmailForHostAsync(userSent, emailTitle, htmlContent, foundItem, true, scheduleId);


      var setting = await _repositorySetting.GetSingleByCondition(p => p.SettingKey.Equals("Emails"));
      if (setting != null && string.IsNullOrEmpty(setting.SettingValue) == false)
      {
        var mobilearr = setting.SettingValue.Split(";");
        if (mobilearr.Any())
        {
          foreach (var email in mobilearr)
          {
            await SendMailAndCreateLogs(email, emailTitle, htmlContent.ToString(), foundItem.Id, null, foundItem, true, scheduleId);
          }
        }
      }

      _unitOfWork.Commit();
    }

    private string DefaultHtmlSampleEmailChange(ScheduleModel foundItem)
    {
      var htmlContent = new StringBuilder();
      var host = foundItem.UserModel == null ? foundItem.OtherHost : $"{foundItem.UserModel.OfficerPosition}-{foundItem.UserModel.FullName}";
      htmlContent.Append($"<p>Lịch họp: {foundItem.ScheduleTitle}</p>");
      htmlContent.Append($"<p>Chủ trì: {host}</p>");
      htmlContent.Append($"Thời gian bắt đầu: {foundItem.ScheduleTime} - {foundItem.ScheduleDate.ToString("dd/MM/yyyy")}");

      var location = foundItem.LocationModel == null ? foundItem.OtherLocation : foundItem.LocationModel.Title;
      htmlContent.Append($"<p>Địa điểm: {location}</p>");
      htmlContent.Append($"<p>Thành phần: {foundItem.ParticipantDisplay}</p>");
      htmlContent.Append($"<p>{foundItem.ScheduleContent}</p>");
      htmlContent.Append($"<p>Lý do dời lịch: {foundItem.ReasonChangeSchedule}</p>");
      return htmlContent.ToString();
    }

    private string GetHtmlSampleFromDatabaseEmailChange(ScheduleModel foundItem, EmailTemplateModel emailTemplate)
    {

      var htmlContent = string.Empty;
      var HTMLPath = string.Empty;
      try
      {
        var fileDocumenttPath = $"{Directory.GetCurrentDirectory()}\\Upload\\{emailTemplate.FilePath.Replace(_uploadSettings.EmailTemplate, nameof(_uploadSettings.EmailTemplate))}";
        HTMLPath = fileDocumenttPath.Replace(".docx", ".html");


        using (StreamReader reader = new StreamReader(HTMLPath))
        {
          htmlContent = reader.ReadToEnd();
        }
        htmlContent = RemoveSpace(htmlContent, "TieuDeLich");
        htmlContent = RemoveSpace(htmlContent, "ChuTri");
        htmlContent = RemoveSpace(htmlContent, "GioBatDau");
        htmlContent = RemoveSpace(htmlContent, "NgayBatDau");
        htmlContent = RemoveSpace(htmlContent, "DiaDiem");
        htmlContent = RemoveSpace(htmlContent, "ThanhPhanThamDu");
        htmlContent = RemoveSpace(htmlContent, "NoiDung");
        htmlContent = RemoveSpace(htmlContent, "LyDoDoiLich");

        var host = foundItem.UserModel == null ? foundItem.OtherHost : $"{foundItem.UserModel.OfficerPosition}-{foundItem.UserModel.FullName}";
        var location = foundItem.LocationModel == null ? foundItem.OtherLocation : foundItem.LocationModel.Title;


        htmlContent = htmlContent.Replace("{TieuDeLich}", foundItem.ScheduleTitle);
        htmlContent = htmlContent.Replace("{ChuTri}", host);
        htmlContent = htmlContent.Replace("{GioBatDau}", foundItem.ScheduleTime);
        htmlContent = htmlContent.Replace("{NgayBatDau}", foundItem.ScheduleDate.ToString("dd/MM/yyyy"));
        htmlContent = htmlContent.Replace("{DiaDiem}", location);
        htmlContent = htmlContent.Replace("{ThanhPhanThamDu}", foundItem.ParticipantDisplay);
        htmlContent = htmlContent.Replace("{NoiDung}", foundItem.ScheduleContent);
        htmlContent = htmlContent.Replace("{LyDoDoiLich}", foundItem.ReasonChangeSchedule);
        return htmlContent;

      }
      catch (Exception ex)
      {
        _logger.LogError("Không thể đổ dữ liệu vào file html: " + HTMLPath);
      }
      return htmlContent;
    }


    //Hoãn Lịch
    public async Task SendEmailForPause(ScheduleForDetailDto scheduleDto, ScheduleModel schedule)
    {
      var emailTitle = "Thông báo HOÃN lịch họp";
      var htmlContent = string.Empty;

      var foundItem = await _repository.GetSingleByCondition(x => x.ScheduleId == schedule.ScheduleId,
          new string[] { "ParticipantsModels.User", "LocationModel", "OtherParticipantModels" });

      var emailTemplateByOrganizeList = await _emailTemplateRepository.GetListEmailTemplateByOrganizeIdAsync(foundItem.OrganizeId);
      var changeEmail = emailTemplateByOrganizeList.Where(x => x.TypeEmail == Constants.Pause).FirstOrDefault();
      if (changeEmail == null)
        htmlContent = DefaultHtmlSampleEmailPause(foundItem);
      else
      {
        htmlContent = GetHtmlSampleFromDatabaseEmailPause(foundItem, changeEmail);
      }

      var userSent = new List<int>();

      if (scheduleDto.ParticipantIsSelected.Any())
      {
        foreach (var ptc in scheduleDto.ParticipantIsSelected)
        {
          var user = await _userRepository.GetSingleById(ptc.ParticipantId);
          if (user != null)
          {
            if (!string.IsNullOrEmpty(user.Email))
            {
              try
              {
                if (userSent.Any(x => x == user.Id) == false)
                {
                  await SendMailAndCreateLogs(user.Email, emailTitle, htmlContent.ToString(), user.Id, null, schedule, true, schedule.ScheduleId);
                  userSent.Add(user.Id);
                }
              }
              catch (Exception)
              {

              }

            }
          }
        }
      }

      if (scheduleDto.OtherParticipants.Any())
      {
        foreach (var grMT in scheduleDto.OtherParticipants)
        {
          if (!string.IsNullOrEmpty(grMT.Email))
          {
            try
            {
              await SendMailAndCreateLogs(grMT.Email, emailTitle, htmlContent.ToString(), null, grMT.OtherParticipantId, schedule, true, schedule.ScheduleId);
            }
            catch (Exception)
            {
            }

          }
        }
      }

      // người chủ trì
      if (schedule.UserModel != null)
      {
        try
        {
          if (userSent.Any(x => x == schedule.Id.GetValueOrDefault()) == false)
          {
            await SendMailAndCreateLogs(schedule.UserModel.Email, emailTitle, htmlContent.ToString(), schedule.Id, null, schedule, true, schedule.ScheduleId);
            userSent.Add(schedule.Id.GetValueOrDefault());
          }
        }
        catch (Exception)
        {

        }

      }

      var setting = await _repositorySetting.GetSingleByCondition(p => p.SettingKey.Equals("Emails"));
      if (setting != null && string.IsNullOrEmpty(setting.SettingValue) == false)
      {
        var mobilearr = setting.SettingValue.Split(";");
        if (mobilearr.Any())
        {
          foreach (var email in mobilearr)
          {
            await SendMailAndCreateLogs(email, emailTitle, htmlContent.ToString(), schedule.Id, null, schedule, false, schedule.ScheduleId);
          }
        }
      }
      _unitOfWork.Commit();
    }

    private string DefaultHtmlSampleEmailPause(ScheduleModel schedule)
    {
      var htmlContent = new StringBuilder();
      var host = schedule.UserModel == null ? schedule.OtherHost : $"{schedule.UserModel.OfficerPosition}-{schedule.UserModel.FullName}";
      htmlContent.Append($"<p>Lịch họp: {schedule.ScheduleTitle}</p>");
      htmlContent.Append($"<p>Chủ trì: {host}</p>");
      htmlContent.Append($"Thời gian: {schedule.ScheduleTime} - {schedule.ScheduleDate.ToString("dd/MM/yyyy")}");

      var location = schedule.LocationModel == null ? schedule.OtherLocation : schedule.LocationModel.Title;
      htmlContent.Append($"<p>Địa điểm: {location}</p>");
      htmlContent.Append($"<p>Thành phần: {schedule.ParticipantDisplay}</p>");
      htmlContent.Append($"<p>{schedule.ScheduleContent}</p>");
      return htmlContent.ToString();
    }

    private string GetHtmlSampleFromDatabaseEmailPause(ScheduleModel foundItem, EmailTemplateModel emailTemplate)
    {

      var htmlContent = string.Empty;
      var HTMLPath = string.Empty;
      try
      {
        var fileDocumenttPath = $"{Directory.GetCurrentDirectory()}\\Upload\\{emailTemplate.FilePath.Replace(_uploadSettings.EmailTemplate, nameof(_uploadSettings.EmailTemplate))}";
        HTMLPath = fileDocumenttPath.Replace(".docx", ".html");


        using (StreamReader reader = new StreamReader(HTMLPath))
        {
          htmlContent = reader.ReadToEnd();
        }
        htmlContent = RemoveSpace(htmlContent, "TieuDeLich");
        htmlContent = RemoveSpace(htmlContent, "ChuTri");
        htmlContent = RemoveSpace(htmlContent, "GioBatDau");
        htmlContent = RemoveSpace(htmlContent, "NgayBatDau");
        htmlContent = RemoveSpace(htmlContent, "DiaDiem");
        htmlContent = RemoveSpace(htmlContent, "ThanhPhanThamDu");
        htmlContent = RemoveSpace(htmlContent, "NoiDung");

        var host = foundItem.UserModel == null ? foundItem.OtherHost : $"{foundItem.UserModel.OfficerPosition}-{foundItem.UserModel.FullName}";
        var location = foundItem.LocationModel == null ? foundItem.OtherLocation : foundItem.LocationModel.Title;


        htmlContent = htmlContent.Replace("{TieuDeLich}", foundItem.ScheduleTitle);
        htmlContent = htmlContent.Replace("{ChuTri}", host);
        htmlContent = htmlContent.Replace("{GioBatDau}", foundItem.ScheduleTime);
        htmlContent = htmlContent.Replace("{NgayBatDau}", foundItem.ScheduleDate.ToString("dd/MM/yyyy"));
        htmlContent = htmlContent.Replace("{DiaDiem}", location);
        htmlContent = htmlContent.Replace("{ThanhPhanThamDu}", foundItem.ParticipantDisplay);
        htmlContent = htmlContent.Replace("{NoiDung}", foundItem.ScheduleContent);

        return htmlContent;

      }
      catch (Exception ex)
      {
        _logger.LogError("Không thể đổ dữ liệu vào file html: " + HTMLPath);
      }
      return htmlContent;
    }

    private async Task<bool> SendMailAndCreateLogs(
        string email,
        string emailTitle,
        string emailContent,
        int? userId,
        int? otherPariticpant,
        ScheduleModel schedule,
        bool isSendAgain,
        int scheduleId)
    {
      var result = 0;
      EmailLogsModel foundLog = null;
      var isSend = true;
      var emailSmSLogList = await GetEmailLogByScheduleIdAsync(scheduleId);
      if (userId != null)
      {
        foundLog = emailSmSLogList.Where(x => x.UserId == userId.GetValueOrDefault() && x.IsSmsLog == false).FirstOrDefault();
      }
      else if (otherPariticpant != null)
      {
        foundLog = emailSmSLogList.Where(x => x.OtherParticipantId == otherPariticpant.GetValueOrDefault() && x.IsSmsLog == false).FirstOrDefault();
      }

      if (foundLog != null)//Người này đã từng gửi email
      {
        if (isSendAgain == true)//yêu cầu gửi lại ko cần ràng buộc
        {
          isSend = await _helperService.SendEmail(email, emailContent, emailTitle);
          foundLog.SendEmailIsSuccess = isSend;
          result = await _emailLogsService.Update(foundLog);
        }
        else//kiểm tra xem lần gửi trước đã thành công chưa, nếu chưa thì gửi lại
        {
          if (foundLog.IsSmsLog == false && foundLog.SendEmailIsSuccess == false)
          {
            isSend = await _helperService.SendEmail(email, emailContent, emailTitle);
            foundLog.SendEmailIsSuccess = isSend;
            result = await _emailLogsService.Update(foundLog);
          };
        }

        return isSend;
      }
      else//lần đầu gửi email
      {
        isSend = await _helperService.SendEmail(email, emailContent, emailTitle);
        result = await _emailLogsService.Create(new EmailLogsModel
        {
          ScheduleId = schedule.ScheduleId,
          SendDate = DateTime.Now,
          SendEmailIsSuccess = isSend,
          UserId = userId,
          IsSmsLog = false,
          OtherParticipantId = otherPariticpant
        });
        return isSend;
      }

    }


    private async Task<bool> SendEmailForParticipantsModelsAsync(List<ParticipantsModel> participantList, List<int> userSent, string emailTitle, string htmlContent, ScheduleModel foundItem, bool isSendAgainToAll, int scheduleId)
    {
      var isSendAllUserSuccess = true;
      if (participantList != null && participantList.Any())
      {
        foreach (var ptc in participantList)
        {
          if (!string.IsNullOrEmpty(ptc.User.Email))
          {
            try
            {
              if (userSent.Any(x => x == ptc.User.Id) == false)
              {
                var isSendSuccess = await SendMailAndCreateLogs(ptc.User.Email, emailTitle, htmlContent, ptc.User.Id, null, foundItem, isSendAgainToAll, scheduleId);
                if (isSendSuccess == false) isSendAllUserSuccess = false;
                userSent.Add(ptc.User.Id);
              }
            }
            catch (Exception)
            {
              isSendAllUserSuccess = false;
            }
          }
        }
      }
      return isSendAllUserSuccess;
    }

    private async Task<bool> SendEmailForOtherParticipantsModelsAsync(List<OtherParticipantModel> otherParticipantList, List<int> userSent, string emailTitle, string htmlContent, ScheduleModel foundItem, bool isSendAgainToAll, int scheduleId)
    {
      var isSendAllUserSuccess = true;
      if (otherParticipantList.Any())
      {
        foreach (var grMT in otherParticipantList)
        {
          if (!string.IsNullOrEmpty(grMT.Email))
          {
            try
            {
              var isSendSuccess = await SendMailAndCreateLogs(grMT.Email, emailTitle, htmlContent, null, grMT.OtherParticipantId, foundItem, isSendAgainToAll, scheduleId);
              if (isSendSuccess == false) isSendAllUserSuccess = false;
            }
            catch (Exception)
            {
              isSendAllUserSuccess = false;
            }

          }
        }
      }
      return isSendAllUserSuccess;
    }

    private async Task<bool> SendEmailForHostAsync(List<int> userSent, string emailTitle, string htmlContent, ScheduleModel foundItem, bool isSendAgainToAll, int scheduleId)
    {
      var isSendAllUserSuccess = true;
      if (foundItem.UserModel != null)
      {
        if (!string.IsNullOrEmpty(foundItem.UserModel.Email))
        {
          try
          {
            if (userSent.Any(x => x==foundItem.Id)==false)
            {
              var isSendSuccess = await SendMailAndCreateLogs(foundItem.UserModel.Email, emailTitle, htmlContent, foundItem.Id, null, foundItem, isSendAgainToAll, scheduleId);
              if (isSendSuccess==false) isSendAllUserSuccess=false;
              userSent.Add(foundItem.Id.GetValueOrDefault());
            }
          }
          catch (Exception)
          {
            isSendAllUserSuccess=false;
          }
        }
      }
      return isSendAllUserSuccess;
    }

    #endregion



  }
}
