using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("PermissionMasterAndRoleMapping")]
  public class PermissionMasterAndRoleMappingModel : CommonModel
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int PermissionMasterAndRoleMappingId { get; set; }

    [Display(Name = "Permission Code Name")]
    [Required]
    public string NamePermission { get; set; }

    [Display(Name = "Role Name")]
    [Required]
    public string RoleName { get; set; }

    [Required]
    public bool IsAllow { get; set; }
 

    [ForeignKey("NamePermission")]
    public virtual PermissionsMasterDataModel PermissionsMasterDataInfo { get; set; } 

  }
}
