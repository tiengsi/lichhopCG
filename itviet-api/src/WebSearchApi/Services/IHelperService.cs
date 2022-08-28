using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ViettelSMSServiceRef;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Settings;
using Exception = System.Exception;

namespace WebApi.Services
{
  public interface IHelperService
  {
    Task<bool> SendEmail(string toEmail, string content, string subject);

    Task<bool> SendSms(string toPhonenumber, string message, int organizeId);
  }

  public class HelperService : IHelperService
  {
    private readonly EmailSettings _emailSettings;
    private readonly SmsSettings _smsSettings;
    ILogger<HelperService> _logger;
    private readonly ISettingRepository _settingRepository;
    private readonly IBrandNameRepository _brandNameRepository;
    private HttpClient _client;
    private HttpRequestMessage _request;

    public HelperService(IOptions<EmailSettings> emailSettings, IOptions<SmsSettings> smsSettings, ILogger<HelperService> logger, ISettingRepository settingRepository, IBrandNameRepository brandNameRepository)
    {
      _emailSettings = emailSettings.Value;
      _smsSettings = smsSettings.Value;
      _logger = logger;
      _settingRepository = settingRepository;
      _brandNameRepository=brandNameRepository;
      //_emailSettings.Email = _settingRepository.GetSingleByCondition(r => r.SettingKey.Equals("SettingEmailSender")).Result.SettingValue;
      //_emailSettings.Password = _settingRepository.GetSingleByCondition(r => r.SettingKey.Equals("SettingEmailPassword")).Result.SettingValue;
      //_smsSettings.Username = _settingRepository.GetSingleByCondition(r => r.SettingKey.Equals("SmsSettingsUsername")).Result.SettingValue;
      //_smsSettings.Password = _settingRepository.GetSingleByCondition(r => r.SettingKey.Equals("SmsSettingsPassword")).Result.SettingValue;
      //_smsSettings.Phonenumber = _settingRepository.GetSingleByCondition(r => r.SettingKey.Equals("SmsSettingsPhoneNumber")).Result.SettingValue;
      //_smsSettings.Provider = _settingRepository.GetSingleByCondition(r => r.SettingKey.Equals("SmsSettingsProvider")).Result.SettingValue;
    }

    public async Task<bool> SendEmail(string toEmail, string content, string subject)
    {
      bool rs;
      try
      {
        _logger.LogInformation($"Starting to send email to {toEmail}");
        MailMessage message = new MailMessage();
        var smtp = new SmtpClient();
        {
          smtp.Host = _emailSettings.Host;
          smtp.Port = _emailSettings.Port;
          smtp.EnableSsl = true;
          smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
          smtp.UseDefaultCredentials = false;
          smtp.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
          smtp.Timeout = 20000;
        }
        MailAddress fromAddress = new MailAddress(_emailSettings.Email, "Hệ thống lịch họp");
        message.From = fromAddress;
        message.To.Add(toEmail);
        message.Subject = subject;
        message.IsBodyHtml = true;
        message.Body = content;
        await smtp.SendMailAsync(message);
        rs = true;
      }
      catch (Exception ex)
      {
        _logger.LogError($"Send email to {toEmail} get error: {ex}");
        rs = false;
      }

      return rs;
    }

    public async Task<bool> SendSms(string toPhonenumber, string message, int organizeId)
    {
      var linkURL = string.Empty;
      var CPCode = string.Empty;
      bool rs = true;
      var smsConfig_Viettel = new Viettel_BrandNameModel();
      var smsConfig_vnpt = new VNPT_BrandNameModel();
      var smsConfigList_Viettel = await _brandNameRepository.GetViettel_BrandNameListByOrganizeIdAsync(organizeId);
      if (smsConfigList_Viettel.Any()==true)
      {
        _smsSettings.Provider="Viettel";
        smsConfig_Viettel=smsConfigList_Viettel.FirstOrDefault();
      }
      else
      {
        var smsConfigList_vnpt = await _brandNameRepository.GetVNPT_BrandNameListByOrganizeIdAsync(organizeId);
        if (smsConfigList_vnpt.Any()==true)
        {
          _smsSettings.Provider="VNPT";
          smsConfig_vnpt=smsConfigList_vnpt.FirstOrDefault();
        }
        else
        {
          _logger.LogError($"No have Viettel or VNPT Config in DB");
          return false;
        }
      }

      switch (_smsSettings.Provider)
      {
        case "Viettel":
          #region Viettel

          _smsSettings.Username=smsConfig_Viettel.ApiUserName;
          _smsSettings.Password=smsConfig_Viettel.ApiPassword;
          _smsSettings.BrandName=smsConfig_Viettel.BrandName;
          _smsSettings.ContentType=smsConfig_Viettel.ContractType.ToString();
          _smsSettings.CPCode=smsConfig_Viettel.CPCode;
          linkURL=smsConfig_Viettel.ApiLink;

          try
          {
            rs= await SendSMS_ViettelAsync(linkURL, message, toPhonenumber);
            _logger.LogInformation($"Send sms to {toPhonenumber} get status: {rs.ToString()}");
          }
          catch (Exception ex)
          {
            _logger.LogError($"Send sms to {toPhonenumber} get error: {ex}");
            rs = false;
          }
          return rs;
        #endregion
        case "VNPT":
          #region VNPT

          _smsSettings.Username=smsConfig_vnpt.ApiUserName;
          _smsSettings.Password=smsConfig_vnpt.ApiPassword;
          _smsSettings.BrandName=smsConfig_vnpt.BrandName;
          _smsSettings.ContentType=smsConfig_vnpt.ContractType.ToString();
          _smsSettings.Phonenumber=smsConfig_vnpt.PhoneNumber.ToString();
          _smsSettings.LinkURL=smsConfig_vnpt.ApiLink;
          
          try
          {
            rs= await SendSMS_VNPTAsync(_smsSettings.LinkURL, message, toPhonenumber);
            _logger.LogInformation($"Send sms to {toPhonenumber} get status: {rs.ToString()}");
          }
          catch (Exception ex)
          {
            _logger.LogError($"Send sms to {toPhonenumber} get error: {ex}");
            rs = false;
          }
          return rs;
        #endregion

        default:
          return false;
      }

    }

    private async Task<bool> SendSMS_VNPTAsync(string linkURL, string message, string toPhonenumber)
    {
     
      bool rs;
      try
      {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _request = new HttpRequestMessage
        {
          Method = HttpMethod.Post,
          RequestUri = new Uri(linkURL),
        };
        var content = new
        {
          username = _smsSettings.Username,
          password = _smsSettings.Password,
          type = _smsSettings.ContentType,
          brandname = _smsSettings.BrandName,
          message = message,
          phonenumber = toPhonenumber
        };
        var json = JsonConvert.SerializeObject(content);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        _request.Content = stringContent;
        var response = await _client.SendAsync(_request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        var responseObj = JsonConvert.DeserializeObject<SmsResponseDto>(result);

        _logger.LogInformation($"responseObj.MessageId = {responseObj.MessageId}");
        _logger.LogInformation($"responseObj.Id = {responseObj.Id}");
        _logger.LogInformation($"responseObj.Description = {responseObj.Description}");

        if (responseObj.Id == 1)
        {
          _logger.LogInformation("Gửi Sms thành công");
          rs = true;
        }
        else if (responseObj.Id == 2)
        {
          _logger.LogError("Số điện thoại sai format");
          rs = false;
        }
        else if (responseObj.Id == 4)
        {
          _logger.LogError("Brandname chưa active");
          rs = false;
        }
        else if (responseObj.Id == 15)
        {
          _logger.LogError("Kết nối Gateway lỗi");
          rs = false;
        }
        else if (responseObj.Id == 17)
        {
          _logger.LogError("Template không hợp lệ");
          rs = false;
        }
        else if (responseObj.Id == 25)
        {
          _logger.LogError("sai mạng, hoặc lable không hợp lệ");
          rs = false;
        }
        else if (responseObj.Id == 100)
        {
          _logger.LogError("database error");
          rs = false;
        }
        else
        {
          rs = false;
        }
      }
      catch (System.Exception ex)
      {
        _logger.LogError($"Send sms to {toPhonenumber} get error: {ex}");
        rs = false;
      }
      return rs;
    }

    private async Task<bool> SendSMS_ViettelAsync(string linkURL, string message, string toPhonenumber)
    {
      bool rs = true;
      string requestId = "1";
      string cmdCode = "bulksms";
      try
      {

        var test = new CcApiClient();
        // var outcome =await test.wsCpMtAsync("smsbrand_sottttyenbai", "123456a@", "SOTTTTYB", requestId, toPhonenumber, toPhonenumber, "SOTTTT_YB", "bulksms", "UBNNYenBaiMoiHop", "0");
        var outcome = await test.wsCpMtAsync(_smsSettings.Username, _smsSettings.Password, _smsSettings.CPCode, requestId, toPhonenumber, toPhonenumber, _smsSettings.BrandName, cmdCode, message, _smsSettings.ContentType);
        _logger.LogError($"Username {_smsSettings.Username},Password {_smsSettings.Password} ,CPCode {_smsSettings.CPCode} ,BrandName {_smsSettings.BrandName}, {_smsSettings.ToString()}");
        if (outcome.@return.message.Contains("Insert MT_QUEUE: OK")==false)
        {
          var messageTV = Constants.Viettel_ErrorList.Where(x => x.Key.Contains(outcome.@return.message)==true).FirstOrDefault();
          if (messageTV!=null) _logger.LogError($"{messageTV.Value}");
          else
            _logger.LogError($"wsCpMtAsync outcome Viettel {outcome.@return.message}");
          rs = false;
        }
        _logger.LogInformation($"Info wsCpMtAsync outcome Viettel {outcome.@return.message}");
      }
      catch (System.Exception ex)
      {
        _logger.LogError($"Send sms to {toPhonenumber} get error: {ex}");
        rs = false;
      }
      return rs;
    }
  }
}
