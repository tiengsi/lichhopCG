using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{

  public class PersonalNotesDto
  {

    public int PersonalNotesId { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string ContentNote { get; set; }
    public int? ScheduleId { get; set; }
    public PersonalNotesModel ToModel()
    {
      var model = new PersonalNotesModel()
      {
        Title = Title,
        UserId = UserId,
        ScheduleId = ScheduleId,
        ContentNote = ContentNote,        
        CreatedBy = UserId.ToString(),      
        UpdatedBy = UserId.ToString(),
        UpdatedDate = DateTime.Now
      };
      return model;
    }
  }
}
