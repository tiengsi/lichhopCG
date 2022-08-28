using System;
using System.Collections.Generic;
using WebApi.Models.Enums;

namespace WebApi.Models.Dtos
{
  public class FEPermissionByRoleDto
  {
    public string DisplayName { get; set; }
    public string NamePermission { get; set; }
    public string PermissionLevel { get; set; }
    public List<KeyPairSB> RoleInfo { get; set; }    =new List<KeyPairSB>();
    public List<FEPermissionByRoleDto> SubPermissionList { get; set; }

  }
}
