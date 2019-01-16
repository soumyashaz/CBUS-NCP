using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;

namespace CBUSA.Services.Model
{
    public class QuarterReminderService: IQuarterReminderService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public QuarterReminderService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<QuarterReminder> GetQuarterReminderDates(Int64 QuarterId)
        {
            return _ObjUnitWork.QuarterReminder.Search(x => x.QuarterID == QuarterId);
        }

        public void AddQuarterReminder(Int64 QuarterID, string ReminderName, DateTime ReminderDate)
        {
            QuarterReminder QR = new QuarterReminder();
            QR.QuarterID = QuarterID;
            QR.ReminderName = ReminderName;
            QR.ReminderDate = ReminderDate;

            _ObjUnitWork.QuarterReminder.Add(QR);
            _ObjUnitWork.Complete();
            //_ObjUnitWork.Dispose();
        }

        public void UpdateQuarterReminder(Int64 QuarterReminderID, string ReminderName, DateTime ReminderDate)
        {
            QuarterReminder QR = _ObjUnitWork.QuarterReminder.Get(QuarterReminderID);
            QR.ReminderDate = ReminderDate;

            _ObjUnitWork.QuarterReminder.Update(QR);
        }

        public void DeleteQuarterReminders(Int64 QuarterID)
        {
            IEnumerable<QuarterReminder> QRs = _ObjUnitWork.QuarterReminder.Search(x => x.QuarterID == QuarterID);

            foreach (QuarterReminder QR in QRs)
            {
                _ObjUnitWork.QuarterReminder.Remove(QR);
            }
        }
    }
}
