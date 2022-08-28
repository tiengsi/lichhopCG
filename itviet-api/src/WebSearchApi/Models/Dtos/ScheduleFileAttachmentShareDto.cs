using System;

namespace WebApi.Models.Dtos
{

  public class ScheduleFileAttachmentShareDto
  {    
    public int Id { get; set; }   
    public int ScheduleId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string CloudinaryPublicId { get; set; }
    public string NotationNumber { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Quote { get; set; }    
    public bool IsShare { get; set; }    
  }
  
}
