using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class UserInRole
    {
        [Key]
        public Int64 UserRoleId { get; set; }
        public int UserId { get; set; }        
        public int RoleId { get; set; }

        public virtual User User { get;set;}

        public virtual Role Role { get; set; }

    }
}
