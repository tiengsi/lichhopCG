using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("PermissionsMasterData")]
  public class PermissionsMasterDataModel : CommonModel
  {
    
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int PermissionsMasterDataId { get; set; }

    [Key]
    [Display(Name = "Permission Code Name")]
    [Required]
    public string NamePermission { get; set; }

    [Display(Name = "Display Name")]
    public string DisplayName { get; set; }

    [Display(Name = "Parent ermission Code Name")]
    [Required]
    public string NamePermissionParent { get; set; }

    [Display(Name = "Permission Level")]
    public string PermissionLevel { get; set; }

    [Display(Name = "Route Temple")]
    public string RouteTemple { get; set; }

    [Display(Name = "Method API")]
    public string Method { get; set; }


  }
}
