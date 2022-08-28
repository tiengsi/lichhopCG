using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers;
using WebApi.Helpers.Domains;
using WebApi.Helpers.Exceptions;
using WebApi.Helpers.Extensions;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
  public interface ILocationService
  {
    Task<FunctionResult> CreateLocationAsync(LocationDto model);

    Task<PaginationSet<LocationModel>> GetAll(string filter, string sortOrder, string sortField, int organizeId, int index = 1, int pageSize = 10);

    Task<IEnumerable<LocationModel>> GetAllForSelect(int organizeId);

    Task<FunctionResult> DeleteLocationByIdAsync(int postId);
    Task<bool> DeleteListLoacationByOrganizeIdAsync(int organizeId);

    Task<LocationModel> GetLocationByIdAsync(int postId);

    Task<FunctionResult> UpdateLocationAsync(LocationDto model);
    Task<IEnumerable<LocationDto>> GetListLocationByOrganizeIdAsync(int organizeId);
  }

  public class LocationService : ILocationService
  {

    private readonly ILocationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LocationService> _logger;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IMapper _mapper;
    private readonly IScheduleHistoryRepository _scheduleHistoryRepository;
    private readonly IScheduleTemplateRepository _scheduleTemplateRepository;
    private readonly IOrganizeRepository _organizeRepository;

    public LocationService(
        ILocationRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<LocationService> logger,
        IScheduleRepository scheduleRepository,
        IScheduleTemplateRepository scheduleTemplateRepository,
        IScheduleHistoryRepository scheduleHistoryRepository,
        IOrganizeRepository organizeRepository,
       IMapper mapper)
    {
      _repository = repository;
      _unitOfWork = unitOfWork;
      _logger = logger;
      _scheduleRepository = scheduleRepository;
      _mapper = mapper;
      _scheduleHistoryRepository = scheduleHistoryRepository;
      _scheduleTemplateRepository = scheduleTemplateRepository;
      _organizeRepository = organizeRepository;
    }
    private FunctionResult ValidateRequiredFieldsLocation(LocationDto model)
    {
      var result = new FunctionResult();
      if (string.IsNullOrEmpty(model.Title)) result.AddError("Yêu cầu nhập title!");
      if (model.OrganizeId <=0) result.AddError("OrganizeId phải là số nguyên dương và lớn hơn 0");
      return result;
    }
    public async Task<bool> CheckContainsOrganizeId(LocationDto model)
    {
      var isExist =await _organizeRepository.CheckContains(m => m.OrganizeId == model.OrganizeId);
      if (isExist == false) return false;
      return true;
    }



    public async Task<FunctionResult>CreateLocationAsync(LocationDto model)
    {
      var response = ValidateRequiredFieldsLocation(model);
      if (response.IsSuccess == false) return response;
      var isExist = await _repository.CheckContains(x => x.Title.ToLower().Equals(model.Title.ToLower())&&x.OrganizeId==model.OrganizeId);
      if (isExist)
      {
        response.AddError($"{model.Title} đã tồn tại!");
        return response;
      }
      var isExistOrganizeId = await CheckContainsOrganizeId(model);
      if(isExistOrganizeId==false)
      {
        response.AddError("Mã đơn vị không hợp lệ");
        return response;
      }  
      var map = model.ToModel();
      var isSuccess = await _repository.InsertLocationAsync(map);
      if (isSuccess == false)
      {
        response.AddError("Thêm location không thành công ");
        return response;
      }
      response.SetData(map.Id);
      return response;
    }
    public async Task<FunctionResult> DeleteLocationByIdAsync(int id)
    {
      var response = new FunctionResult();
      var isExist = await _repository.CheckContains(x => x.Id == id);
      if (!isExist)
      {
        response.AddError("Không tìm thấy mã đơn vị này");
        return response;
      }
      await _scheduleRepository.AssignValueLocationIdInSchedule(id);
      await _scheduleTemplateRepository.AssignValueLocationIdInScheduleTemplate(id);
      await _scheduleHistoryRepository.AssignValueLocationIdInScheduleHistory(id);
      var isSuccess = await _repository.DeleteLocationByIdAsync(id);
      if (isSuccess == false) return response;
      response.SetData(id);
      return response;
    }
    public async Task<bool> DeleteListLoacationByOrganizeIdAsync(int organizeId)
    {
      var listLocationByOrganizeId = await _repository.GetMulti(m => m.OrganizeId == organizeId);
      if (listLocationByOrganizeId.Count() == 0) return true;
      foreach(var item in listLocationByOrganizeId)
      {
        await _scheduleRepository.AssignValueLocationIdInSchedule(item.Id);
        await _scheduleTemplateRepository.AssignValueLocationIdInScheduleTemplate(item.Id);
        await _scheduleHistoryRepository.AssignValueLocationIdInScheduleHistory(item.Id);
      }

      var isSuccess = await _repository.DeleteLocationByOrganizeIdAsync(organizeId);
      if (isSuccess == true) return true;
      return false;
    }

    public async Task<PaginationSet<LocationModel>> GetAll(string filter, string sortOrder, string sortField, int organizeId, int index = 1, int pageSize = 10)
    {
      Expression<Func<LocationModel, bool>> baseFilter = post => true;
      var isDesc = sortOrder == "desc" ? true : false;
      if (!string.IsNullOrEmpty(filter))
      {
        filter = filter.ToLower();
        baseFilter = baseFilter.And(x => x.Title.ToLower().Contains(filter));
      }

      if(organizeId != 0)
      {
        baseFilter = baseFilter.And(x => x.OrganizeId == organizeId);
      }

      var total = await _repository.Count(baseFilter);
      var result = await _repository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize);

      return new PaginationSet<LocationModel>()
      {
        Items = result,
        TotalCount = total,
        Page = index
      };
    }

    public async Task<IEnumerable<LocationModel>> GetAllForSelect(int organizeId)
    {
      return await _repository.GetMulti(x => x.IsActive && x.OrganizeId == organizeId);
    }

    public async Task<LocationModel> GetLocationByIdAsync(int id)
    {
      var foundItem = await _repository.GetSingleByCondition(x => x.Id == id);
      if (foundItem == null)
      {
        return null;
      }
      return foundItem;
    }
    public async Task<FunctionResult> UpdateLocationAsync(LocationDto model)
    {
      var result = ValidateRequiredFieldsLocation(model);
      var map = model.ToModel();
      map.Id = model.Id;
      if (result.IsSuccess == false) return result;
      var foundItem = await _repository.GetSingleById(model.Id);
      if (foundItem == null)
      {
        result.AddError("Không tìm thấy mã địa điểm này");
        return result;
      }
      var isExistOrganizeId = await CheckContainsOrganizeId(model);
      if (isExistOrganizeId == false)
      {
        result.AddError("Mã đơn vị không hợp lệ");
        return result;
      }
      var isExist = await _repository.CheckContains(x => x.Title.ToLower().Equals(model.Title.ToLower())&&x.Id!=model.Id&&x.OrganizeId==model.OrganizeId);
      if (isExist)
      {
        result.AddError($"{model.Title} đã tồn tại!");
        return result;
      }
      var isSuccess = await _repository.UpdateLocationAsync(map);
      if(isSuccess==false)
      {
        result.AddError("Update location không thành công");
        return result;
      }
      result.SetData(map.Id);
      return result;
    }
    public async Task<IEnumerable<LocationDto>> GetListLocationByOrganizeIdAsync(int organizeId)
    {
      var locationModels = await _repository.GetListLocationByOrganizeIdAsync(organizeId);
      if (locationModels.Count() == 0) return new List<LocationDto>();
      var map = _mapper.Map<IEnumerable<LocationDto>>(locationModels);
      return map;
    }
  }
}
