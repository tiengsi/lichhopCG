using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Repositories;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Enums;

namespace WebApi.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetByType(ECategoryType type);

        Task<List<CategoryModel>> GetByMenu(string menu);

        Task<string> GetByCategoryCode(string code);
    }

    public class CategoryService : ICategoryService
    {
        ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GetByCategoryCode(string code)
        {
            var cate = await _repository.GetSingleByCondition(x => x.CategoryCode.Equals(code));
            return cate?.CategoryName;
        }

        public async Task<List<CategoryModel>> GetByMenu(string menu)
        {
            return await _repository.GetAll();
        }

        public async Task<List<CategoryModel>> GetByType(ECategoryType type)
        {
            return await _repository.GetMulti(x => x.TypeCode == type);
        }
    }
}
