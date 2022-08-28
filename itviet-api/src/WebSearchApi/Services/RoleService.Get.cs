using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Data.Repositories;
using WebApi.Helpers.Domains;
using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Models.Settings;

namespace WebApi.Services
{
    public partial interface IRoleService
    {
        Task<PaginationSet<RoleModel>> GetAllRoleAsync(int index, int pageSize);
    }

    public partial class RoleService : IRoleService
    {   

        public async Task<PaginationSet<RoleModel>> GetAllRoleAsync(int index, int pageSize)
        {
            var total = await _roleRepository.Count(x => !x.Name.Equals(Constants.SuperAdmin));
            var listRole = await _roleRepository.GetMultiPaging(x => !x.Name.Equals(Constants.SuperAdmin), "CreatedDate", true, index, pageSize);

            return new PaginationSet<RoleModel>()
            {
                Items = listRole,
                TotalCount = total,
                Page = index
            };
        }

    }
}
