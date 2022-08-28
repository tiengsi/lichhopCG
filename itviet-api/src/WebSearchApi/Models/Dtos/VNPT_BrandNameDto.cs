
namespace WebApi.Models
{
  public class VNPT_BrandNameDto : BrandNameDto
  {
    public string PhoneNumber { get; set; }
    public VNPT_BrandNameModel ToModel()
    {
      var newOjb = new VNPT_BrandNameModel()
      {
        OrganizeId = OrganizeId,

        ContractType = ContractType,
        PhoneNumber=PhoneNumber,
        IsActive = IsActive,
        ApiLink = ApiLink,
        ApiPassword = ApiPass,
        ApiUserName = ApiUser,
        BrandName = BranchName,
        CreatedBy = "SuperAdmin",
        UpdatedBy = "SuperAdmin",
        UpdatedDate = System.DateTime.Now,

      };
      return newOjb;
    }
  }
}
