
namespace WebApi.Models
{
  public class Viettel_BrandNameDto : BrandNameDto
  {
    public string CPCode { get; set; }
    public Viettel_BrandNameModel ToModel()
    {
      var newOjb = new Viettel_BrandNameModel()
      {
        OrganizeId = OrganizeId,

        ContractType = ContractType,
        CPCode=CPCode,
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
