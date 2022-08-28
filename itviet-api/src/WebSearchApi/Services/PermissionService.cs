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
using WebApi.Models.Settings;

namespace WebApi.Services
{
  public interface IPermissionService
  {
    Task<IEnumerable<PermissionListOfUIDto>> GetPermissionListOfUIByUserIdAsync(int userId);
    Task<bool> CheckPermisisonAccessAPIByUserId(int userId,string apiRouteTemp, string method);

    Task<IEnumerable<FEPermissionByRoleDto>> GetFEPermissionsByRolesAsync ();
    Task<FunctionResult> UpdateFEPermissionsByRolesAsync(IEnumerable<FEPermissionByRoleDto> model);
  }
  public class PermissionService : IPermissionService
  {

    IUnitOfWork _unitOfWork;
    ILogger<PermissionService> _logger;
    IUploaderService _uploaderService;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;


    public PermissionService(IUnitOfWork unitOfWork, ILogger<PermissionService> logger, IUploaderService uploaderService, IRoleRepository roleRepository, IPermissionRepository permissionRepository)
    {

      _unitOfWork = unitOfWork;
      _logger = logger;
      _uploaderService = uploaderService;
      _roleRepository=roleRepository;
      _permissionRepository=permissionRepository;
    }
    public async Task<IEnumerable<PermissionListOfUIDto>> GetPermissionListOfUIByUserIdAsync(int userId)
    {
      //get Role list of User
      var result = new List<PermissionListOfUIDto>();
      var roleListOfUser = await _roleRepository.GetRoleListByUserIdAsync(userId);
      var roleInfoList = await _roleRepository.GetRoleInfoByRoleIdAsync(roleListOfUser.Select(x => x.RoleId));
      var roleNameCodeList = roleInfoList.Select(x => x.Name);
      var permissionRoleMappingList = await _permissionRepository.GetPermissionMasterAndRoleMappingByRoleAsync(roleNameCodeList);
      foreach (var item in permissionRoleMappingList)
      {
        var newObj = new PermissionListOfUIDto()
        {
          NamePermission=item.NamePermission,
          IsAllow=item.IsAllow
        };
        result.Add(newObj);
      }
      return result;
    }

    public async Task<bool> CheckPermisisonAccessAPIByUserId(int userId, string apiRouteTemp,string method)
    {
   
      var roleListOfUser = await _roleRepository.GetRoleListByUserIdAsync(userId);
      var roleInfoList = await _roleRepository.GetRoleInfoByRoleIdAsync(roleListOfUser.Select(x => x.RoleId));
      var roleNameOfUserList = roleInfoList.Select(x => x.Name);
      var permissionMasterInfo=await _permissionRepository.GetPermissionsMasterDataByRouteTempleAsync(apiRouteTemp,method);
      if (permissionMasterInfo==null) return false;
      var permissionRoleMappingOfRouteTempList = await _permissionRepository.GetPermissionMasterAndRoleMappingByPermissionNameCodeAsync(permissionMasterInfo.NamePermission);

     var  resultFinal =permissionRoleMappingOfRouteTempList.Where(x => roleNameOfUserList.Any(y => y==x.RoleName));
      if (resultFinal.Count()>0 && resultFinal.Any(x => x.IsAllow==true)) return true;
      return false;
    }

    public async Task<IEnumerable<FEPermissionByRoleDto>> GetFEPermissionsByRolesAsync()
    {
      var result=new List<FEPermissionByRoleDto>();
      var condition = new List<string>() { Constants.MasterPermission_Function, Constants.MasterPermission_Action };
      var FEPermissionsMasterList = await _permissionRepository.GetPermissionsMasterDataByPermissionLevelAsync(condition);
      var FEP_FunctionsList = FEPermissionsMasterList.Where(x => x.PermissionLevel==Constants.MasterPermission_Function);
      var FEPermissionMappingList = await _permissionRepository.GetPermissionMasterAndRoleMappingByPermissionNameCodeAsync(FEPermissionsMasterList.Select(x => x.NamePermission));
      foreach (var permissionFunc in FEP_FunctionsList)
      {
        var newPermissionObj = new FEPermissionByRoleDto();
        newPermissionObj.NamePermission=permissionFunc.NamePermission;
        newPermissionObj.DisplayName=permissionFunc.DisplayName;
        newPermissionObj.PermissionLevel=permissionFunc.PermissionLevel;
        newPermissionObj.SubPermissionList=new List<FEPermissionByRoleDto>();
        //get SubPermissionMaster
        var subPermissionMasterList = FEPermissionsMasterList.Where(x => x.NamePermissionParent==permissionFunc.NamePermission);

        var pemissionRoleMappingCurrent = FEPermissionMappingList.Where(x => x.NamePermission==permissionFunc.NamePermission);
        //Get Role Info of current PemissionAndRole
        foreach (var map in pemissionRoleMappingCurrent)
        {
          var newRoleInfo = new KeyPairSB(map.RoleName, map.IsAllow);
          newPermissionObj.RoleInfo.Add(newRoleInfo);
        }

        //Get Sub Permission Info
        foreach (var subPMaster in subPermissionMasterList)
        {
          var newSubObj = new FEPermissionByRoleDto();
          newSubObj.NamePermission=subPMaster.NamePermission;
          newSubObj.PermissionLevel=subPMaster.PermissionLevel;

          var subPemissionRoleMappingCurrent = FEPermissionMappingList.Where(x => x.NamePermission==subPMaster.NamePermission);
          //Get Role Info of current SubPemissionAndRole
          foreach (var map in subPemissionRoleMappingCurrent)
          {
            var newRoleInfo = new KeyPairSB(map.RoleName, map.IsAllow);
            newSubObj.RoleInfo.Add(newRoleInfo);
          }
          newPermissionObj.SubPermissionList.Add(newSubObj);
        }
        result.Add(newPermissionObj);

      }
      return result;
    }
    public async Task<FunctionResult> UpdateFEPermissionsByRolesAsync(IEnumerable<FEPermissionByRoleDto> model)
    {
      var result = new FunctionResult();
      if (model.Any()==false) return result;
      var FE_perNeedUpdate = new List<PermissionMasterAndRoleMappingModel>();
      foreach (var perFunc_Cur in model)
      {
        //add data of Permission Funcs
        var condition = new List<string>() { Constants.MasterPermission_Api };
        var api_PermissionMasterByPermissionFunctionList = await _permissionRepository.GetSubPermissionsMasterDataByPermissionLevelAndParentPermissionAsync(perFunc_Cur.NamePermission, condition);
        foreach (var item in perFunc_Cur.RoleInfo)
        {
          //add NamePermission of Function to list
          FE_perNeedUpdate.Add(InitDataUpdate(perFunc_Cur.NamePermission, item.Key, item.Value));
          //add NamePermission API List relate to NamePermission of Func follow each Role
          foreach (var api in api_PermissionMasterByPermissionFunctionList)
          {
            FE_perNeedUpdate.Add(InitDataUpdate(api.NamePermission, item.Key, item.Value));
          }
        }

        //add data of Permission Actions List that is subPermission of Permission Function
        foreach (var subPerAction_Cur in perFunc_Cur.SubPermissionList)
        {
          var api_PermissionMasterFollowPermissionActionList = await _permissionRepository.GetSubPermissionsMasterDataByPermissionLevelAndParentPermissionAsync(subPerAction_Cur.NamePermission, condition);
          foreach (var item in subPerAction_Cur.RoleInfo)
          {
            //add NamePermission of Action to list
            FE_perNeedUpdate.Add(InitDataUpdate(subPerAction_Cur.NamePermission, item.Key, item.Value));
            //add API NamePermission  List relate to NamePermission of Action follow each Role
            foreach (var api in api_PermissionMasterFollowPermissionActionList)
            {
              FE_perNeedUpdate.Add(InitDataUpdate(api.NamePermission, item.Key, item.Value));
            }
          }
        }      
      }
      var updateResult = await _permissionRepository.UpdatePermissionMasterAndRoleMappingByNamePermissionAndRoleAsync(FE_perNeedUpdate);


      return result;
    }

    private PermissionMasterAndRoleMappingModel InitDataUpdate(string namePer, string roleName, bool isAlow)
    {
      var newOjb = new PermissionMasterAndRoleMappingModel();
      newOjb.NamePermission=namePer;
      newOjb.IsAllow=isAlow;
      newOjb.RoleName=roleName;
      return newOjb;
    }
  }
}
