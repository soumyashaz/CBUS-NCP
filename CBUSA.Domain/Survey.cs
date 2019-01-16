using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Domain
{
    public class Survey : BaseColumnField
    {
        public Int64 SurveyId { get; set; }
        [Required]
        public string SurveyName { get; set; }
        [Required]
        public string Label { get; set; }
        [Required]
        public Int64 ContractId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEnrolment { get; set; }
        public Int64 ContractCurrentStatus { get; set; }
        public bool IsPublished { get; set; }
        public bool IsNcpSurvey { get; set; }
        public DateTime? Publishdate { get; set; }
        public DateTime? ArchivedDate { get; set; }
        [MaxLength(250)]
        public string Year { get; set; }
         [MaxLength(250)]
        public string Quater { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ICollection<SurveyMarket> SurveyMarket { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual ICollection<SurveyEmailSetting> SurveyEmailSetting { get; set; }
    }
}
