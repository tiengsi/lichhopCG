using System;
using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
  public class UserForCreateDto
  {
    public int Id { get; set; }

    public string UserName { set; get; }

    public string ShortName { set; get; }

    public string Password { set; get; }

    public string Email { set; get; }

    public int? DptId { get; set; }

    public string FullName { set; get; }

    public bool IsHost { set; get; }

    public bool IsShow { set; get; }

    public string PhoneNumber { set; get; }

    public string UpdatedBy { set; get; }

    public string OfficerPosition { set; get; }

    public DateTime LastLogin { get; set; }
    public int? OrganizeId { get; set; }

    // list of role name
    public List<string> Roles { get; set; }

    public UserModel ToModel()
    {
      return new UserModel()
      {
        UserName = UserName,
        Email = Email,
        EmailConfirmed = true,
        FullName = FullName,
        DptId = DptId == 0 ? null : DptId,
        LockoutEnabled = false,
        LastLogin = DateTime.Now,
        PhoneNumber = PhoneNumber,
        OfficerPosition = OfficerPosition,
        IsHost = IsHost,
        IsShow = IsShow,
        OrganizeId = OrganizeId == 0 ? null : OrganizeId,
        ShortName = ShortName,
        UpdatedBy = "SuperAdmin",
        UpdatedDate = System.DateTime.Now,
      };

    }
  }


  public class UserForListDto : UserForCreateDto
  {
    public DepartmentDto Department { get; set; }
  }
}
