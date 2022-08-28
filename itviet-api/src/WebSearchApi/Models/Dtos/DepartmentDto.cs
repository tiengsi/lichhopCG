using System.Collections.Generic;

namespace WebApi.Models.Dtos
{
    public class DepartmentDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string ShortName { set; get; }

        public string Adress { get; set; } = "N/A";

        public string Email { get; set; } = "N/A";

        public string PhoneNumber { get; set; } = "N/A"; 

        public string Fax { get; set; } = "N/A"; 

        public int? ParentId { get; set; }

        public List<int> UserRepresentative { get; set; }

        public bool IsActive { get; set; }

        public int SortOrder { get; set; }

        // Tên người đại diện
        public string Representative { get; set; }

        // Id người đại diện
        public string RepresentativeId { get; set; }
        public int OrganizeId { get; set; }
    }

    public class DepartmentRepresentativePayload
    {
        public int DepartmentId { get; set; }
        public int RepresentativeId { get; set; }
    }
}
