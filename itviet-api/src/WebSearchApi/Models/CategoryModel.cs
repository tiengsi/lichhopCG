using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Enums;

namespace WebApi.Models
{
    [Table("Category")]
    public class CategoryModel: CommonModel
    {
        public CategoryModel()
        {
            PostModels = new HashSet<PostModel>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { set; get; }

        [MaxLength(500)]
        [Required(ErrorMessage = "Yêu cầu nhập trường này!")]
        public string CategoryName { set; get; }

        [Display(Name = "Kiểu danh mục")]
        [Required(ErrorMessage = "Yêu cầu nhập kiểu danh mục !")]
        public ECategoryType TypeCode { get; set; }

        [Display(Name = "Danh mục cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Mã danh mục")]
        [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
        [StringLength(400, ErrorMessage = "Chỉ được nhập tối đa 400 ký tự !")]
        public string CategoryCode { get; set; }

        [Display(Name = "Liên kết")]
        [StringLength(200, ErrorMessage = "Chỉ được nhập tối đa 200 ký tự !")]
        public string Link { get; set; }

        [MaxLength(500)]
        public string Description { set; get; }

        [Display(Name = "Ảnh đại diện")]
        [StringLength(200, ErrorMessage = "Chỉ được nhập tối đa 200 ký tự !")]
        public string Icon { get; set; }

        [ForeignKey("ParentId")]
        public virtual ICollection<CategoryModel> SubCategories { get; set; }
        public virtual ICollection<PostModel> PostModels { get; set; }
    }
}
