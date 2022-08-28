using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("Role")]
    public class RoleModel : IdentityRole<int>
    {
        public RoleModel() : base()
        {
            UserRoles = new HashSet<UserRoleModel>();
        }

        public string Description { get; set; }

        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)]
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        [MaxLength(256)]
        public string UpdatedBy { set; get; }

        public bool IsActive { set; get; }

        public virtual ICollection<UserRoleModel> UserRoles { get; set; }

    }
}
