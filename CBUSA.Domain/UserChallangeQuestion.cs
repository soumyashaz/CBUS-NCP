using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class UserChallangeQuestion : BaseColumnField
    {
        [Key]
        public Int64 UserChallangeQuestionId { get; set; }

        [Required]
        public string Answer { get; set; }
        [ForeignKey("User")]
        public int Id { get; set; }
        public User User { get; set; }

        [ForeignKey("ChallengeQuestion")]
        public int ChallengeQuestionId { get; set; }
        public ChallengeQuestion ChallengeQuestion { get; set; }
    }
}
