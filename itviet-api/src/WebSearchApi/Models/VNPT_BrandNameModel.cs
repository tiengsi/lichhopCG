using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("VNPT_BrandName")]
  public class VNPT_BrandNameModel : BrandNameModel
  {
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    
  }
}
