using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using WebApi.Models.Enums;

namespace WebApi.Models
{

  [Table("ScheduleTitleTemplate")]
  public class ScheduleTitleTemplateModel
  {
    public ScheduleTitleTemplateModel()
    {
      ScheduleModels = new HashSet<ScheduleModel>();
    }

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Template { get; set; }

    public bool IsShow { get; set; } = true;


    [Display(Name = "OrganizeId")]
    public int OrganizeId { get; set; }

    [ForeignKey("OrganizeId")]
    public virtual OrganizeModel OrganizeModel { get; set; }

    public virtual ICollection<ScheduleModel> ScheduleModels { get; set; }
    public virtual ICollection<ScheduleTemplateModel> ScheduleTemplateModels { get; set; }
  }
}
