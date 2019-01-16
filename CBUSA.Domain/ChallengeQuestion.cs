using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ChallengeQuestion : BaseColumnField
    {
        [Key]
        public int ChallengeQuestionId { get; set; }
        [Required]
        [Display(Name = "Challenge Question")]
        public string ChallengeQuestionDescription { get; set; }


    }
}
