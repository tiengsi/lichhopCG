namespace WebApi.Models
{

  public class OrganizeDto
  {


    public int OrganizeId { get; set; }
    public string Name { get; set; }
    public int? OrganizeParentId { get; set; }
    public string CodeName { get; set; }
    public string OtherName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }


    public OrganizeModel ToModel()
    {
      var newOjb = new OrganizeModel()
      {
        OrganizeParentId = OrganizeParentId,
        Address = Address,
        CodeName = CodeName,
        CreatedBy = "SuperAdmin",
        UpdatedBy = "SuperAdmin",
        UpdatedDate = System.DateTime.Now,
        IsActive = IsActive,
        Name = Name,
        Order = Order,
        OtherName = OtherName,
        Phone = Phone
      };
      return newOjb;
    }

  }
}
