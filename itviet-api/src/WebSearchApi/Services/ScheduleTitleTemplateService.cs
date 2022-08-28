using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers.Domains;
using WebApi.Helpers.Exceptions;
using WebApi.Models;

namespace WebApi.Services
{
  public interface IScheduleTitleTemplateService
  {
    Task<int> CreateScheduleTitleTemplateAsync(ScheduleTitleTemplateDto model);

    Task<PaginationSet<ScheduleTitleTemplateModel>> GetAllScheduleTitleTemplateAsync(int index = 1, int pageSize = 10);
    Task<PaginationSet<ScheduleTitleTemplateDto>> GetAllScheduleTitleTemplateByOrganizeIdAsync(int index = 1, int pageSize = 10, int organizeId = 0);

    Task<FunctionResult> DeleteScheduleTitleTemplateAsync(int postId);

    Task<ScheduleTitleTemplateModel> GetScheduleTitleTemplateByIdAsync(int postId);

    Task<int> UpdateScheduleTitleTemplateAsync(ScheduleTitleTemplateDto model);
    Task<bool> DeleteListScheduleTitleTemplateByOrganizeIdsAsync(int organizeId);
  }
  public class ScheduleTitleTemplateService : IScheduleTitleTemplateService
  {
    private readonly IScheduleTitleTemplateRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ScheduleTitleTemplateService> _logger;
    private readonly IMapper _mapper;
    private readonly IScheduleTemplateRepository _scheduleTemplateRepository;
    private readonly IScheduleRepository _scheduleRepository;
    public ScheduleTitleTemplateService(
        IScheduleTitleTemplateRepository _repository,
        IUnitOfWork _unitOfWork,
        ILogger<ScheduleTitleTemplateService> _logger,
          IMapper mapper,
          IScheduleTemplateRepository scheduleTemplateRepository,
          IScheduleRepository scheduleRepository)
    {
      this._repository = _repository;
      this._unitOfWork = _unitOfWork;
      this._logger = _logger;
      this._mapper = mapper;
      this._scheduleTemplateRepository = scheduleTemplateRepository;
      this._scheduleRepository = scheduleRepository;
    }

    public async Task<int> CreateScheduleTitleTemplateAsync(ScheduleTitleTemplateDto model)
    {
      var isExist = await _repository.CheckContains(x => x.OrganizeId == model.OrganizeId && x.Template.ToLower().Equals(model.Template.ToLower()));
      if (isExist)
      {
        _logger.LogWarning($"{model.Template} is exist");
        throw new BusinessException($"{model.Template} đã tồn tại", StatusCodes.Status409Conflict);
      }
      var mappedModel = _mapper.Map<ScheduleTitleTemplateModel>(model);
      _repository.Add(mappedModel);
      _unitOfWork.Commit();

      return model.Id;
    }

    public async Task<FunctionResult> DeleteScheduleTitleTemplateAsync(int postId)
    {
      var result = new FunctionResult();
      var isExist = await _repository.CheckContains(x => x.Id == postId);
      if (!isExist)
      {
        result.AddError("Không tìm thấy bản này");
        return result;
      }
      await _scheduleTemplateRepository.AssignValueScheduleTitileTemplateIdInScheduleTemplate(postId);
      await _scheduleRepository.AssignValueScheduleTitileTemplateIdInSchedule(postId);
      //await _scheduleTemplateRepository.DeleteScheduleTemplateByScheduleTitleTemplateId(postId);
      //await _scheduleRepository.DeleteScheduleByScheduleTitleTemplateId(postId);
      _repository.DeleteMulti(x => x.Id == postId);
      _unitOfWork.Commit();
      result.SetData(postId);
      return result;
    }

    public async Task<bool> DeleteListScheduleTitleTemplateByOrganizeIdsAsync(int organizeId)
    {
      var listScheduleTitleTemplateByOrganizeId = await _repository.GetMulti(m => m.OrganizeId == organizeId);
      if (listScheduleTitleTemplateByOrganizeId.Count() == 0) return true;
      foreach (var item in listScheduleTitleTemplateByOrganizeId)
      {
        await _scheduleRepository.AssignValueScheduleTitileTemplateIdInSchedule(item.Id);
        await _scheduleTemplateRepository.AssignValueScheduleTitileTemplateIdInScheduleTemplate(item.Id);
      }
      var isSuccess = await _repository.DeleteListScheduleTitleTemplateByOrganizeIdAsync(organizeId);
      if (isSuccess == true) return true;
      return false;
    }

    public async Task<PaginationSet<ScheduleTitleTemplateModel>> GetAllScheduleTitleTemplateAsync(int index = 1, int pageSize = 10)
    {
      var total = await _repository.Count(x => true);
      var result = await _repository.GetMultiPaging(x => true, "Id", true, index, pageSize);

      return new PaginationSet<ScheduleTitleTemplateModel>()
      {
        Items = result,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<PaginationSet<ScheduleTitleTemplateDto>> GetAllScheduleTitleTemplateByOrganizeIdAsync(int index = 1, int pageSize = 10, int organizeId = 0)
    {
      var total = await _repository.Count(x => x.OrganizeId == organizeId);
      var result = await _repository.GetMultiPaging(x => x.OrganizeId == organizeId, "Id", true, index, pageSize);
      var finalResult = _mapper.Map<List<ScheduleTitleTemplateDto>>(result);
      return new PaginationSet<ScheduleTitleTemplateDto>()
      {
        Items = finalResult,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<ScheduleTitleTemplateModel> GetScheduleTitleTemplateByIdAsync(int postId)
    {
      var foundItem = await _repository.GetSingleByCondition(x => x.Id == postId);
      if (foundItem == null)
      {
        _logger.LogWarning($"Id {postId} is not found");
        throw new BusinessException($"TempalteId {postId} không tìm thấy", StatusCodes.Status404NotFound);
      }

      return foundItem;
    }

    public async Task<int> UpdateScheduleTitleTemplateAsync(ScheduleTitleTemplateDto model)
    {
      var foundItem = await _repository.GetSingleById(model.Id);
      if (foundItem == null)
      {
        _logger.LogWarning($"Not found post {model.Id}");
        throw new BusinessException($"Không tìm thấy địa điểm có id = {model.Id}", StatusCodes.Status404NotFound);
      }

      foundItem.Template = model.Template;
      _repository.Update(foundItem);
      _unitOfWork.Commit();

      return foundItem.Id;
    }
  }
}
