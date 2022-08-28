using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("ScheduleTypes")]
  public class ScheduleTypeModel : CommonModel
  {

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int ScheduleTypeId { get; set; }



    [Display(Name = "Tên loại phiên họp")]
    public string Name { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }

    [Display(Name = "Trạng thái")]
    public bool IsActive { get; set; }


    [Display(Name = "Thứ tự")]
    public int Order { get; set; }

    public int? OrganizeId { get; set; }

    [ForeignKey("OrganizeId ")]
    public virtual OrganizeModel OrganizeModel { get; set; }


  }
}
