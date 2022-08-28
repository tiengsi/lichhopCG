namespace WebApi.Models
{


  public class ScheduleTitleTemplateDto
  {
    public ScheduleTitleTemplateDto()
    {     
    }
   
    public int Id { get; set; }
 
    public string Template { get; set; }

    public bool IsShow { get; set; } = true;
   
    public int OrganizeId { get; set; }
    
  }
}
