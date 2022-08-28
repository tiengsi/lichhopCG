using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Dtos
{
    public class OfficerDto
    {
        public string FullName { set; get; }

        public string Email { set; get; }

        public string PhoneNumber { set; get; }

        public string OfficerPosition { get; set; }
    }
}
