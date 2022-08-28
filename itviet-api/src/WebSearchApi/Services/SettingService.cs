using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers.Exceptions;
using WebApi.Helpers.Extensions;
using WebApi.Models;
using WebApi.Models.Dtos;

namespace WebApi.Services
{
    public interface ISettingService
    {
        Task<SettingModel> GetByKey(string key);

        Task<List<SettingModel>> GetAll();

        Task<string> Update(List<SettingDto> model);

        Task<string> Update(string settingKey, string imagePath, string cloudinaryPublicId);
    }
    public class SettingService : ISettingService
    {
        ISettingRepository _repository;
        IUnitOfWork _unitOfWork;
        ILogger<SettingService> _logger;
        IUploaderService _uploaderService;

        public SettingService(ISettingRepository repository, IUnitOfWork unitOfWork, ILogger<SettingService> logger, IUploaderService uploaderService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _uploaderService = uploaderService;
        }

        public async Task<List<SettingModel>> GetAll()
        {
            var foundItem = await _repository.GetAll();
            return foundItem.OrderBy(x => x.SortOrder).ToList();
        }

        public async Task<SettingModel> GetByKey(string key)
        {
            var foundItem = await _repository.GetSingleById(key);
            if (foundItem == null)
            {
                _logger.LogWarning($"key {key} is not found");
                throw new BusinessException($"key {key} không tìm thấy", StatusCodes.Status404NotFound);
            }

            return foundItem;
        }

        public async Task<string> Update(List<SettingDto> model)
        {
            foreach (var item in model)
            {
                var foundItem = await _repository.GetSingleById(item.SettingKey);
                if (foundItem == null)
                {
                    _logger.LogWarning($"Not found SettingKey {item.SettingKey}");
                    throw new BusinessException($"Không tìm thấy bản ghi {item.SettingKey}", StatusCodes.Status404NotFound);
                }
                foundItem.UpdateSetting(item);
                _repository.Update(foundItem);
            }

            _unitOfWork.Commit();

            return "Cập nhật thành công";
        }

        public async Task<string> Update(string settingKey, string imagePath, string cloudinaryPublicId)
        {
            var foundItem = await _repository.GetSingleById(settingKey);
            if (foundItem == null)
            {
                _logger.LogWarning($"Not found SettingKey {settingKey}");
                throw new BusinessException($"Không tìm thấy bản ghi {settingKey}", StatusCodes.Status404NotFound);
            }

            // delete old image from Cloudinary
            if (!string.IsNullOrEmpty(foundItem.SettingValue))
            {
                await _uploaderService.DeleteImage(foundItem.SettingComment);
            }

            foundItem.SettingValue = imagePath;
            foundItem.SettingComment = cloudinaryPublicId;
            _repository.Update(foundItem);

            _unitOfWork.Commit();

            return "Cập nhật thành công";
        }
    }
}
