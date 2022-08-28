using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    [Table("UserParticipant")]
    public class UserParticipantModel
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int GroupParticipantId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }

        [ForeignKey("GroupParticipantId")]
        public virtual GroupParticipantModel GroupParticipant { get; set; }
    }
}
