

namespace WebApi.Models
{
  public class EmailTemplateDto
  {
    public int EmailTemplateId { get; set; }
    public string Title { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string CloudinaryPublicId { get; set; }
    public string TypeEmail { get; set; }
    public int OrganizeId { get; set; }
    public bool IsActvie { get; set; }
    public EmailTemplateModel ToModel()
    {
      var newOjb = new EmailTemplateModel()
      {
        OrganizeId = OrganizeId,
        Title=Title,
        FileName=FileName,
        FilePath=FilePath,
        CloudinaryPublicId=CloudinaryPublicId,
        CreatedBy = "SuperAdmin",
        UpdatedBy = "SuperAdmin",
        UpdatedDate = System.DateTime.Now,
        IsActive=true,
        TypeEmail=TypeEmail
      };
      return newOjb;
    }
  }
}
