using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CommonModel
    {
        [MaxLength(256)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(256)]
        public string UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
