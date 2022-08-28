using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("UserRole")]
    public class UserRoleModel: IdentityUserRole<int>
    {
        public virtual RoleModel Role { get; set; }

        public virtual UserModel User { get; set; }
    }
}
