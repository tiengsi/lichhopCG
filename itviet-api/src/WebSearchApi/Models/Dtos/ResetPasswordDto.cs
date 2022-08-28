using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Dtos
{
  public class ResetPasswordDto
  {
    public int UserId { get; set; }
    public string NewPassword { get; set; }
  }
}
