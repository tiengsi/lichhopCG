using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Dtos
{
  public class LocationDto
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public bool IsActive { get; set; }
    public int OrganizeId { get; set; }
    public LocationModel ToModel()
    {
      var newOjb = new LocationModel()
      {
        OrganizeId = OrganizeId,
        Title=Title,
        IsActive=IsActive,
        CreatedBy = "SuperAdmin",
        UpdatedBy = "SuperAdmin",
        UpdatedDate = System.DateTime.Now,
      };
      return newOjb;
    }
  }

  public class LocationForListDto : LocationDto
  {
    public DateTime CreatedDate { get; set; }
  }
}
