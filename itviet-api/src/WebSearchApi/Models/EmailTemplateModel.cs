using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
  [Table("EmailTemplate")]
  public class EmailTemplateModel:CommonModel
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int EmailTemplateId { get; set; }
    [Display(Name ="Tiêu đề")]
    [Required(ErrorMessage ="Yêu cầu nhập trường này")]
    public string Title { get; set; }
    [Display(Name ="Tên tài liệu")]
    [Required(ErrorMessage ="Yêu cầu nhập trường này!")]
    public string FileName { get; set; }
    [Display(Name = "Tài liệu đính kèm")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này!")]
    public string FilePath { get; set; }
    [Display(Name = "Cloudinary publicId")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này!")]
    public string TypeEmail { get; set; }
    [Display(Name = "loại tài liệu")]   
    public string CloudinaryPublicId { get; set; }
    [Display(Name ="Mã đơn vị")]
    public int OrganizeId { get; set; }
    [ForeignKey("OrganizeId")]
    public virtual OrganizeModel OrganizeModel { get; set; }
  }
}
