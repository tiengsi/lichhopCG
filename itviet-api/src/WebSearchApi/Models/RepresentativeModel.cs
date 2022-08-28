using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    // Người đại diện cho một phòng ban
    [Table("Representative")]
    public class RepresentativeModel
    {
        [Key, Column(Order = 0)]
        public int DepartmentId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual DepartmentModel Department { get; set; }                

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
    }

    [Table("RepresentativeTemplate")]
    public class RepresentativeTemplateModel
    {
        [Key, Column(Order = 0)]
        public int DepartmentId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual DepartmentTemplateModel Department { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
    }
}
