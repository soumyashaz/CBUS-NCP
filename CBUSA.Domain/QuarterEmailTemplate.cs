using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class QuarterEmailTemplate
    {
        public Int64 QuarterEmailTemplateId { get; set; }
        public Int64 QuaterId { get; set; }
        public string InvitationEmailSubject { get; set; }
        public string InvitationEmailTemplate { get; set; }
        public string ReminderEmailSubject { get; set; }
        public string ReminderEmailTemplate { get; set; }
    }
}
