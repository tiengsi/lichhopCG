using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
  [Table("Location")]
  public class LocationModel : CommonModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Display(Name = "Tên")]
    [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
    public string Title { get; set; }
    [Display(Name = "Mã organizeId")]
    public int OrganizeId { get; set; }
    [ForeignKey("OrganizeId")]
    public virtual OrganizeModel OrganizeModel { get; set; }
  }
}
