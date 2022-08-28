using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApi.Models.Dtos;

namespace WebApi.Models
{
    [Table("Post")]
    public class PostModel: CommonModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Yêu cầu nhập tiêu đề !")]
        [StringLength(300, ErrorMessage = "Bạn chỉ được nhập tối đa 300 ký tự !")]
        public string Title { get; set; }

        [Display(Name = "Filter title")]
        [Required(ErrorMessage = "Yêu cầu nhập trường này !")]
        public string FilterTitle { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(1000, ErrorMessage = "Bạn chỉ được nhập tối đa 1000 ký tự")]
        public string Description { get; set; }

        [Display(Name = "Chi tiết bài viết")]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "Ảnh")]
        public string ImagePath { get; set; }

        public string CloudinaryPublicId { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryModel CategoryModel { get; set; }

    }
}
