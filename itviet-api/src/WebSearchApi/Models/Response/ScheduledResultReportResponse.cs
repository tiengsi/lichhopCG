using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{

  public class ScheduledResultReportResponse 
  {


    public int ScheduledResultReportId { get; set; } 
    public int UserId { get; set; }
    public int UserName { get; set; }

    public string Path { get; set; }
  
    public string ReportContent { get; set; }

   
    public DateTime ReportTime { get; set; }

    public int? ScheduleId { get; set; }

    
    public  UserModel User { get; set; }


  }
}
