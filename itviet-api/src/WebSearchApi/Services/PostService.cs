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
    public interface IPostService
    {
        Task<int> Create(PostForCreateDto model);

        Task<PaginationSet<PostDto>> GetAll(string filter, string categoryCode, string sortOrder, string sortField, int index = 1, int pageSize = 10);

        Task<int> Delete(int postId);

        Task<PostModel> GetById(int postId);

        Task<int> Update(PostDto model);

        Task<UploadResultDto> UpdateImage(int postId, string imagePath, string publicId);
    }

    public class PostService : IPostService
    {

        IPostRepository _repository;
        IUnitOfWork _unitOfWork;
        ILogger<PostService> _logger;
        IUploaderService _uploaderService;
        private readonly IMapper _mapper;

        public PostService(IPostRepository repository, IUnitOfWork unitOfWork, ILogger<PostService> logger, IUploaderService uploaderService, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _uploaderService = uploaderService;
            _mapper = mapper;
        }

        public async Task<int> Create(PostForCreateDto model)
        {
            var isExist = await _repository.CheckContains(x => x.Title.ToLower().Equals(model.Title.ToLower()));
            if (isExist)
            {
                _logger.LogWarning($"{model.Title} is exist");
                throw new BusinessException($"{model.Title} đã tồn tại", StatusCodes.Status409Conflict);
            }

            var createModel = new PostModel();
            createModel.UpdatePost(model);
            //createModel.ImagePath = uploadResult.Url.ToString();
            //createModel.CloudinaryPublicId = uploadResult.PublicId;

            _repository.Add(createModel);
            _unitOfWork.Commit();

            return createModel.PostId;
        }

        public async Task<int> Delete(int postId)
        {
            var isExist = await _repository.CheckContains(x => x.PostId == postId);
            if (!isExist)
            {
                _logger.LogError($"Not found post");
                throw new BusinessException("Không tìm thấy tin tức này", StatusCodes.Status404NotFound);
            }

            var foundItem = await _repository.GetSingleById(postId);
            _repository.Delete(foundItem);
            _unitOfWork.Commit();

            // delete old image from Cloudinary
            if (!string.IsNullOrEmpty(foundItem.CloudinaryPublicId))
            {
                await _uploaderService.DeleteImage(foundItem.CloudinaryPublicId);
            }

            return foundItem.PostId;
        }

        public async Task<PaginationSet<PostDto>> GetAll(
            string filter, 
            string categoryCode, 
            string sortOrder, 
            string sortField, 
            int index = 1, 
            int pageSize = 10)
        {
            Expression<Func<PostModel, bool>> baseFilter = post => true;
            var isDesc = sortOrder == "desc" ? true : false;
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                baseFilter = baseFilter.And(x => x.Title.ToLower().Contains(filter) || x.Title.ToLower().Contains(filter));
            }

            if (!string.IsNullOrEmpty(categoryCode))
            {
                baseFilter = baseFilter.And(x => x.CategoryModel.CategoryCode.Contains(categoryCode));
            }

            var total = await _repository.Count(baseFilter);
            var result = await _repository.GetMultiPaging(baseFilter, sortField, isDesc, index, pageSize, new string[] { "CategoryModel" });

            var mappResult = _mapper.Map<List<PostDto>>(result);

            return new PaginationSet<PostDto>()
            {
                Items = mappResult,
                TotalCount = total,
                Page = index
            };
        }

        public async Task<PostModel> GetById(int postId)
        {
            var foundItem = await _repository.GetSingleByCondition(x => x.PostId == postId, new string[] { "CategoryModel" });
            if (foundItem == null)
            {
                _logger.LogWarning($"postId {postId} is not found");
                throw new BusinessException($"postId {postId} không tìm thấy", StatusCodes.Status404NotFound);
            }

            return foundItem;
        }

        public async Task<int> Update(PostDto model)
        {
            var foundItem = await _repository.GetSingleById(model.PostId);
            if (foundItem == null)
            {
                _logger.LogWarning($"Not found post {model.PostId}");
                throw new BusinessException($"Không tìm thấy tin tức {model.PostId}", StatusCodes.Status404NotFound);
            }

            foundItem.UpdatePost(model);
            _repository.Update(foundItem);
            _unitOfWork.Commit();

            return foundItem.PostId;
        }

        public async Task<UploadResultDto> UpdateImage(int postId, string imagePath, string publicId)
        {
            var foundItem = await _repository.GetSingleById(postId);
            if (foundItem == null)
            {
                _logger.LogWarning($"Not found post {postId}");
                throw new BusinessException($"Không tìm thấy tin tức {postId}", StatusCodes.Status404NotFound);
            }

            foundItem.CloudinaryPublicId = publicId;
            foundItem.ImagePath = imagePath;
            _repository.Update(foundItem);
            _unitOfWork.Commit();

            // delete old image from Cloudinary
            if (!string.IsNullOrEmpty(foundItem.CloudinaryPublicId))
            {
                await _uploaderService.DeleteImage(foundItem.CloudinaryPublicId);
            }

            return new UploadResultDto()
            {
                PublicId = publicId,
                Url = imagePath,
            };
        }
    }
}
