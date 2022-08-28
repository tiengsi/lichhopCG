using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("Organizes")]
  public class OrganizeModel : CommonModel
  {

    public OrganizeModel()
    {
      BrandModels = new HashSet<VNPT_BrandNameModel>();
      EmailTemplateModels = new HashSet<EmailTemplateModel>();
      DepartmentModels = new HashSet<DepartmentModel>();
      UserModels = new HashSet<UserModel>();
      LocationModels = new HashSet<LocationModel>();
      GroupParticipantModels = new HashSet<GroupParticipantModel>();
    }
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int OrganizeId { get; set; }


    [Display(Name = "Tên đơn vị")]
    public string Name { get; set; }

    [Display(Name = "Đơn vị cha")]
    public int? OrganizeParentId { get; set; }

    [Display(Name = "Mã đơn vị")]
    [MaxLength(256)]
    public string CodeName { get; set; }

    [Display(Name = "Tên viết tắt")]
    public string OtherName { get; set; }

    [Display(Name = "Địa chỉ")]
    public string Address { get; set; }

    [Display(Name = "Điện thoại")]
    [StringLength(40, ErrorMessage = "Bạn chỉ được nhập tối đa 40 ký tự")]
    public string Phone { get; set; }

    [Display(Name = "Thứ tự")]
    public int Order { get; set; }

    [Display(Name = "Trạng thái")]
    public bool IsActive { get; set; }
    public virtual ICollection<VNPT_BrandNameModel> BrandModels { get; set; }
    public virtual ICollection<EmailTemplateModel> EmailTemplateModels { get; set; }
    public virtual ICollection<DepartmentModel> DepartmentModels { get; set; }
    public virtual ICollection<UserModel> UserModels { get; set; }
    public virtual ICollection<LocationModel> LocationModels { get; set; }
    public virtual ICollection<GroupParticipantModel> GroupParticipantModels { get; set; }


  }
}
