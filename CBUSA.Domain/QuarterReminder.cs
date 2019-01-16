using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class QuarterReminder
    {
        public Int64 QuarterReminderID { get; set; }
        public Int64 QuarterID { get; set; }
        public string ReminderName { get; set; }
        public DateTime ReminderDate { get; set; }        
    }
}
