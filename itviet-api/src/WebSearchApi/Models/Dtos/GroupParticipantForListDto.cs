using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Dtos
{
    public class GroupParticipantForListDto
    {
        public int GroupParticipantId { get; set; }

        public string GroupParticipantName { set; get; }

        public DateTime CreatedDate { get; set; }

        public List<DepartmentDto> Departments { get; set; }

        public List<UserForCreateDto> Users { get; set; }

        public List<OtherParticipantDto> OtherParticipants { get; set; }
    }
}
