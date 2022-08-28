using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  public class BrandNameModel : CommonModel
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int BrandNameId { get; set; } 

 
    [Display(Name ="Loại contract")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
    public int ContractType { get; set; }
    [Display(Name ="Mã API User")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
    public string ApiUserName { get; set; } 
    [Display(Name ="Mật khẩu API")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
    public string ApiPassword { get; set; } 
[Display(Name ="Tên thành viên")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
    public string BrandName { get; set; }
    [Display(Name ="Liên kết api")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
    public string ApiLink { get; set; }      

    [Display(Name="Mã organizeId")]
    public int OrganizeId { get; set; }
    [ForeignKey("OrganizeId")]
    public virtual OrganizeModel OrganizeModel { get; set; }

  }
}
