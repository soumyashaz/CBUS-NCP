using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IQuarterReminderService
    {
        IEnumerable<QuarterReminder> GetQuarterReminderDates(Int64 QuarterId);
        void AddQuarterReminder(Int64 QuarterID, string ReminderName, DateTime ReminderDate);
        void UpdateQuarterReminder(Int64 QuarterReminderID, string ReminderName, DateTime ReminderDate);
        void DeleteQuarterReminders(Int64 QuarterID);
    }
}
