
namespace WebApi.Models
{
  public class BrandNameDto
  {
    public int BrandNameId { get; set; } 
  
   
    public int ContractType { get; set; }
    
    public string ApiUser { get; set; }
   
    public string ApiPass { get; set; }
   
    public string BranchName { get; set; }
   
    public string ApiLink { get; set; }      
    public bool IsActive { get; set; }
   
    public int OrganizeId { get; set; }  
   
  }
}
