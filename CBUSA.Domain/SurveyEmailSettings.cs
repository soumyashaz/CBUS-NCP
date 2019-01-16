using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class SurveyEmailSetting
    {
        public Int64 SurveyEmailSettingId { get; set; }
        [StringLength(100)]
        public string SenderEmail { get; set; }
        public bool RemainderForTakeSurvey { get; set; }
        public Int32 DayBeforeSurveyEnd { get; set; }

        public bool RemainderForTakeSurveySecond { get; set; }
        public Int32 DayBeforeSurveyEndSecond { get; set; }


        public bool RemainderForTakeSurveyThird { get; set; }
        public Int32 DayBeforeSurveyEndThird { get; set; }

        public bool RemainderForContinueSurvey { get; set; }
        public Int32 DayAfterSurveyEnd { get; set; }
        public Int64 SurveyId { get; set; }
        public virtual Survey Survey { get; set; }

        public virtual ICollection<SurveyInviteEmailSetting> SurveyInviteEmailSetting { get; set; }
        public virtual ICollection<SurveyRemainderEmailSetting> SurveyRemainderEmailSetting { get; set; }
        public virtual ICollection<SurveySaveContinueEmailSetting> SurveySaveContinueEmailSetting { get; set; }
    }

    public class SurveyInviteEmailSetting
    {
        public Int64 SurveyInviteEmailSettingId { get; set; }
        [StringLength(250)]
        public string Subject { get; set; }
        public string EmailContent { get; set; }
         [StringLength(100)]
        public string DumpId { get; set; }
        public Int64? SurveyEmailSettingId { get; set; }
        public virtual SurveyEmailSetting SurveyEmailSettings { get; set; }

    }

    public class SurveyRemainderEmailSetting
    {
        public Int64 SurveyRemainderEmailSettingId { get; set; }
        [StringLength(250)]
        public string Subject { get; set; }
        public string EmailContent { get; set; }
          [StringLength(100)]
        public string DumpId { get; set; }
        public Int64? SurveyEmailSettingId { get; set; }
        public virtual SurveyEmailSetting SurveyEmailSettings { get; set; }

    }

    public class SurveySaveContinueEmailSetting
    {
        public Int64 SurveySaveContinueEmailSettingId { get; set; }
        [StringLength(250)]
        public string Subject { get; set; }
        public string EmailContent { get; set; }
          [StringLength(100)]
        public string DumpId { get; set; }
        public Int64? SurveyEmailSettingId { get; set; }
        public virtual SurveyEmailSetting SurveyEmailSettings { get; set; }

    }

}
