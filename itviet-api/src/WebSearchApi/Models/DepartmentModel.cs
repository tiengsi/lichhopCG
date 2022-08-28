using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("Department")]
  public class DepartmentModel : CommonModel
  {
    public DepartmentModel()
    {
      Officers = new HashSet<UserModel>();
      RepresentativeModels = new HashSet<RepresentativeModel>();
      GroupDepartmentModels = new HashSet<GroupDepartmentModel>();
    }

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Display(Name = "Tên")]
    [Required(ErrorMessage = "Yêu cầu nhập tên !")]
    [StringLength(500, ErrorMessage = "Bạn chỉ được nhập tối đa 500 ký tự !")]
    public string Name { get; set; }

    [MaxLength(256)]
    public string ShortName { set; get; }

    [Display(Name = "Địa chỉ")]
    [StringLength(1000, ErrorMessage = "Bạn chỉ được nhập tối đa 1000 ký tự !")]
    public string Adress { get; set; }

    [Display(Name = "Email")]
    [StringLength(500, ErrorMessage = "Bạn chỉ được nhập tối đa 500 ký tự")]
    public string Email { get; set; }

    [Display(Name = "Điện thoại")]
    [StringLength(40, ErrorMessage = "Bạn chỉ được nhập tối đa 40 ký tự")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Fax")]
    [StringLength(20, ErrorMessage = "Bạn chỉ được nhập tối đa 20 ký tự")]
    public string Fax { get; set; }

    [Display(Name = "Cấp cha")]
    public int? ParentId { get; set; }

    [Display(Name = "Sắp xếp")]
    public int SortOrder { get; set; }
    [Display(Name = "Mã organizeId")]
    public int OrganizeId { get; set; }
    [ForeignKey("OrganizeId")]
    public virtual OrganizeModel OrganizeModel { get; set; }

    [ForeignKey("ParentId")]
    public virtual ICollection<DepartmentModel> SubDepartments { get; set; }

    public virtual ICollection<UserModel> Officers { get; set; }

    public virtual ICollection<RepresentativeModel> RepresentativeModels { get; set; }

    public virtual ICollection<GroupDepartmentModel> GroupDepartmentModels { get; set; }

   
  }

  [Table("DepartmentTemplate")]
  public class DepartmentTemplateModel : CommonModel
  {
    public DepartmentTemplateModel()
    {
      Officers = new HashSet<UserModel>();
      RepresentativeModels = new HashSet<RepresentativeTemplateModel>();
      GroupDepartmentModels = new HashSet<GroupDepartmentTemplateModel>();
    }

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Display(Name = "Tên")]
    [Required(ErrorMessage = "Yêu cầu nhập tên !")]
    [StringLength(500, ErrorMessage = "Bạn chỉ được nhập tối đa 500 ký tự !")]
    public string Name { get; set; }

    [MaxLength(256)]
    public string ShortName { set; get; }

    [Display(Name = "Địa chỉ")]
    [StringLength(1000, ErrorMessage = "Bạn chỉ được nhập tối đa 1000 ký tự !")]
    public string Adress { get; set; }

    [Display(Name = "Email")]
    [StringLength(500, ErrorMessage = "Bạn chỉ được nhập tối đa 500 ký tự")]
    public string Email { get; set; }

    [Display(Name = "Điện thoại")]
    [StringLength(40, ErrorMessage = "Bạn chỉ được nhập tối đa 40 ký tự")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Fax")]
    [StringLength(20, ErrorMessage = "Bạn chỉ được nhập tối đa 20 ký tự")]
    public string Fax { get; set; }

    [Display(Name = "Cấp cha")]
    public int? ParentId { get; set; }

    [Display(Name = "Sắp xếp")]
    public int SortOrder { get; set; }

    [ForeignKey("ParentId")]
    public virtual ICollection<DepartmentTemplateModel> SubDepartments { get; set; }

    public virtual ICollection<UserModel> Officers { get; set; }

    public virtual ICollection<RepresentativeTemplateModel> RepresentativeModels { get; set; }

    public virtual ICollection<GroupDepartmentTemplateModel> GroupDepartmentModels { get; set; }

  }
}
