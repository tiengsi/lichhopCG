using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data.Infrastructure;
using WebApi.Models;

namespace WebApi.Data.Repositories
{
  public interface IPermissionRepository : IRepository<PermissionsMasterDataModel>
  {
    Task<IEnumerable<PermissionsMasterDataModel>> GetPermissionsMasterDataAsync();
    Task<IEnumerable<PermissionsMasterDataModel>> GetPermissionsMasterDataByPermissionLevelAsync(IEnumerable<string> condition);
    Task<IEnumerable<PermissionsMasterDataModel>> GetSubPermissionsMasterDataByPermissionLevelAndParentPermissionAsync(string parentPermissionName, IEnumerable<string> condition);
    Task<PermissionsMasterDataModel> GetPermissionsMasterDataByRouteTempleAsync(string routeTemp, string method);
    Task<IEnumerable<PermissionMasterAndRoleMappingModel>> GetPermissionMasterAndRoleMappingByRoleAsync(string roleNameCode);
    Task<IEnumerable<PermissionMasterAndRoleMappingModel>> GetPermissionMasterAndRoleMappingByRoleAsync(IEnumerable<string> roleNameCode);
    Task<IEnumerable<PermissionMasterAndRoleMappingModel>> GetPermissionMasterAndRoleMappingByPermissionNameCodeAsync(string permissionNameCode);
    Task<IEnumerable<PermissionMasterAndRoleMappingModel>> GetPermissionMasterAndRoleMappingByPermissionNameCodeAsync(IEnumerable<string> permissionNameCodeList);
    Task<bool> UpdatePermissionMasterAndRoleMappingByNamePermissionAndRoleAsync(IEnumerable<PermissionMasterAndRoleMappingModel> updateList);
    Task<bool> InsertPermissionMasterAndRoleMappingByNewRoleAsync(IEnumerable<PermissionMasterAndRoleMappingModel> createList);
    Task<bool> DeletePermissionMasterAndRoleMappingByNewRoleAsync(string roleName);
  }

  public class PermissionRepository : RepositoryBase<PermissionsMasterDataModel>, IPermissionRepository
  {
    private WebApiDbContext _dataContext;
    private AutoMapper.IMapper _mapper;
    public PermissionRepository(WebApiDbContext context, AutoMapper.IMapper mapper) : base(context)
    {
      _dataContext = context;
      _mapper = mapper;
    }

    public async Task<IEnumerable<PermissionsMasterDataModel>> GetPermissionsMasterDataAsync()
    {
      var result = await _dataContext.PermissionsMasterDataModel.ToListAsync();
      return result;
    }
    public async Task<IEnumerable<PermissionsMasterDataModel>> GetPermissionsMasterDataByPermissionLevelAsync(IEnumerable<string> condition)
    {
      var result = await _dataContext.PermissionsMasterDataModel.Where(x => condition.Any(y => y==x.PermissionLevel)).ToListAsync();
      return result;
    }

    public async Task<PermissionsMasterDataModel> GetPermissionsMasterDataByRouteTempleAsync(string routeTemp, string method)
    {
      var result = await _dataContext.PermissionsMasterDataModel.Where(x => x.RouteTemple.Equals(routeTemp) && x.Method.Equals(method)).FirstOrDefaultAsync();
      return result;

      //var newClacc = new WebApiContextDesignFactory();
      //using (WebApiDbContext newdb = newClacc.CreateDbContext(null))
      //{
      //  var result = await newdb.PermissionsMasterDataModel.Where(x => x.RouteTemple.Equals(routeTemp) && x.Method.Equals(method)).FirstOrDefaultAsync();
      //  return result;
      //}
    }

    public async Task<IEnumerable<PermissionMasterAndRoleMappingModel>> GetPermissionMasterAndRoleMappingByRoleAsync(string roleNameCode)
    {
      var result = await _dataContext.PermissionMasterAndRoleMappingModel.Where(x => x.RoleName==roleNameCode).ToListAsync();

      return result;
    }

    public async Task<IEnumerable<PermissionMasterAndRoleMappingModel>> GetPermissionMasterAndRoleMappingByRoleAsync(IEnumerable<string> roleNameCode)
    {
      var result = await _dataContext.PermissionMasterAndRoleMappingModel.Where(x => roleNameCode.Any(y => y==x.RoleName)).ToListAsync();

      return result;
    }

    public async Task<IEnumerable<PermissionMasterAndRoleMappingModel>> GetPermissionMasterAndRoleMappingByPermissionNameCodeAsync(string permissionNameCode)
    {

      var result = await _dataContext.PermissionMasterAndRoleMappingModel.Where(x => x.NamePermission.Equals(permissionNameCode)).ToListAsync();
      return result;
      //var newClacc = new WebApiContextDesignFactory();
      //using (WebApiDbContext newdb = newClacc.CreateDbContext(null))
      //{
      //  var result = await newdb.PermissionMasterAndRoleMappingModel.Where(x => x.NamePermission.Equals(permissionNameCode)).ToListAsync();
      //  return result;
      //}
    }

    public async Task<IEnumerable<PermissionMasterAndRoleMappingModel>> GetPermissionMasterAndRoleMappingByPermissionNameCodeAsync(IEnumerable<string> permissionNameCodeList)
    {
      var result = await _dataContext.PermissionMasterAndRoleMappingModel.Where(x => permissionNameCodeList.Any(y => y.Equals(x.NamePermission))).ToListAsync();
      return result;
    }

    public async Task<bool> UpdatePermissionMasterAndRoleMappingByNamePermissionAndRoleAsync(IEnumerable<PermissionMasterAndRoleMappingModel> updateList)
    {
      try
      {
        foreach (var item in updateList)
        {
          var entityUpdate = await _dataContext.PermissionMasterAndRoleMappingModel.FirstOrDefaultAsync(x => x.NamePermission == item.NamePermission && x.RoleName==item.RoleName);
          if (entityUpdate != null)
          {
            entityUpdate.IsAllow = item.IsAllow;
            await _dataContext.SaveChangesAsync();
          }
        }
        return true;
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }

    public async Task<bool> InsertPermissionMasterAndRoleMappingByNewRoleAsync(IEnumerable<PermissionMasterAndRoleMappingModel> createList)
    {
      try
      {
        await _dataContext.PermissionMasterAndRoleMappingModel.AddRangeAsync(createList);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {
        throw ex;
      }
    }

    public async Task<IEnumerable<PermissionsMasterDataModel>> GetSubPermissionsMasterDataByPermissionLevelAndParentPermissionAsync(string parentPermissionName, IEnumerable<string> condition)
    {
      var result = await _dataContext.PermissionsMasterDataModel.Where(x => condition.Any(y => y==x.PermissionLevel) && x.NamePermissionParent==parentPermissionName).ToListAsync();
      return result;
    }

    public async Task<bool> DeletePermissionMasterAndRoleMappingByNewRoleAsync(string roleName)
    {

      try
      {
        var result = await _dataContext.PermissionMasterAndRoleMappingModel.Where(x => x.RoleName==roleName).ToListAsync();
        if (result == null) return true;

        _dataContext.PermissionMasterAndRoleMappingModel.RemoveRange(result);
        await _dataContext.SaveChangesAsync();
        return true;
      }
      catch (System.Exception ex)
      {

        throw ex;
      }
    }
  }
}
