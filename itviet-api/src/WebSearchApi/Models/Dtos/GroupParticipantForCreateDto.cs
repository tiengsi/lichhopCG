using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
  public class GroupParticipantForCreateDto
  {
    public int Id { get; set; }

    public string Name { set; get; }

    public List<int> DepartmentIds { get; set; }

    public List<int> UserIds { get; set; }
    public int OrganizeId { get; set; }

    public List<OtherParticipantDto> OtherParticipants { get; set; }
  }
}
