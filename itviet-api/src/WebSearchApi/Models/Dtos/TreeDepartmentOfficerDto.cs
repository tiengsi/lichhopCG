using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Dtos
{
    public class TreeDepartmentOfficerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Adress { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Fax { get; set; }

        public int? ParentId { get; set; }

        public virtual IEnumerable<TreeDepartmentOfficerDto> SubDepartments { get; set; }

        public virtual IEnumerable<OfficerDto> Officers { get; set; }
    }
}
