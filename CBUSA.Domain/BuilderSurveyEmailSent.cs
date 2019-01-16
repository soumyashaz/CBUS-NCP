using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class BuilderSurveyEmailSent
    {
        public Int64 BuilderSurveyEmailSentId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 BuilderId { get; set; }
        [MaxLength(250)]
        public string GroupId { get; set; }
        public DateTime SendDate { get; set; }
        public Int64 SendBy { get; set; }
        public bool IsMailSent { get; set; }
        public Survey Survey { get; set; }
        public Builder Builder { get; set; }

    }
}
