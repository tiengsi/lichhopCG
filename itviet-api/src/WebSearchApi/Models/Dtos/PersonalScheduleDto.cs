using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{

  public class PersonalScheduleDto
  {

    public int PersonalScheduleId { get; set; }


    public string Title { get; set; }


    public int UserId { get; set; }


    public string Description { get; set; }


    public DateTime Fromdate { get; set; }


    public DateTime ToDate { get; set; }


    public virtual UserModel User { get; set; }
    public PersonalScheduleModel ToModel()
    {
      var model = new PersonalScheduleModel()
      {

        Fromdate = Fromdate,
        UserId = UserId,
        ToDate = ToDate,
        Description = Description,
        CreatedBy = UserId.ToString(),      
        UpdatedBy = UserId.ToString(),
        UpdatedDate = DateTime.Now,
        Title = Title
      };
      return model;
    }
  }
}
