using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
  [Table("Viettel_BrandName")]
  public class Viettel_BrandNameModel : BrandNameModel
  {
    [Display(Name = "Mã khách hàng")]
    public string CPCode { get; set; }
    
  }
}
