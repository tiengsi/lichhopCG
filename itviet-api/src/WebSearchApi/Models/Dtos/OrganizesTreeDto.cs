using System.Collections.Generic;

namespace WebApi.Models
{

  public class OrganizesTreeDto
  {    
    public int OrganizeId { get; set; }
    public string Name { get; set; }
    public IEnumerable<OrganizesTreeDto> SubOrganizeList { get; set; }
    public int? OrganizeParentId { get; set; }
    public string CodeName { get; set; }
    public string OtherName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int Order { get; set; } 
    public bool IsActive { get; set; }

  }
}
