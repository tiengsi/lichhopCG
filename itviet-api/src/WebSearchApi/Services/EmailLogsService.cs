using AutoMapper;
using LinqKit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
  public interface IEmailLogsService
  {
    Task<EmailSmsStatusDto> GetAll(int scheduleId);

    Task<int> Create(EmailLogsModel model);

    Task<EmailLogsModel> GetByUserId(int userId);

    Task<int> Update(EmailLogsModel model);

    Task<EmailLogsModel> GetByOtherParticipantId(int otherParticitpantId);

    Task<IEnumerable<EmailLogsModel>> GetByScheduleId(int scheduleId);

  }
  public class EmailLogsService : IEmailLogsService
  {
    IEmailLogsRepository _repository;
    IUnitOfWork _unitOfWork;
    IMapper _mapper;

    public EmailLogsService(IEmailLogsRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
      _repository = repository;
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task<EmailSmsStatusDto> GetAll(int scheduleId)
    {
      var result = await _repository.GetMulti(x => x.ScheduleId == scheduleId, new string[] { "User", "User.Department", "OtherParticipant" });

      var mapResult = _mapper.Map<List<EmailSmsLogDto>>(result);

      var isNotCompleteSendEmail = mapResult.Exists(x => !x.SendEmailIsSuccess);
      var isNotCompleteSendSms = mapResult.Exists(x => !x.SendSmsIsSuccess);

      return new EmailSmsStatusDto()
      {
        EmailSmsLogs = mapResult,
        IsCompleteSendEmail = !isNotCompleteSendEmail,
        IsCompleteSendSms = !isNotCompleteSendSms
      };
    }

    public async Task<int> Create(EmailLogsModel model)
    {
      _repository.Add_Ex(model);
      //_unitOfWork.Commit();
      return await Task.FromResult(model.Id);
    }

    public async Task<EmailLogsModel> GetByUserId(int userId)
    {
      var result = await _repository.GetSingleByCondition(x => x.UserId == userId);

      return result;
    }

    public async Task<int> Update(EmailLogsModel model)
    {
      _repository.Update_Ex(model);
      //_unitOfWork.Commit();
      return await Task.FromResult(model.Id);
    }

    public async Task<EmailLogsModel> GetByOtherParticipantId(int otherParticitpantId)
    {
      var result = await _repository.GetSingleByCondition(x => x.OtherParticipantId == otherParticitpantId);

      return result;
    }

    public async Task<IEnumerable<EmailLogsModel>> GetByScheduleId(int scheduleId)
    {
      var result = await _repository.GetMulti_Ex(x => x.ScheduleId == scheduleId);

      return result;
    }
  }
}
