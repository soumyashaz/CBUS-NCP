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
    public class QuarterEmailTemplateService: IQuarterEmailTemplateService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public QuarterEmailTemplateService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public QuarterEmailTemplate GetQuarterEmailTemplate(Int64 QuarterId)
        {
            return _ObjUnitWork.QuarterEmailTemplate.Search(x => x.QuaterId == QuarterId).FirstOrDefault();
        }

        public string GetQuarterInvitationEmailSubject(Int64 QuarterId)
        {
            return _ObjUnitWork.QuarterEmailTemplate.Search(x => x.QuaterId == QuarterId).FirstOrDefault().InvitationEmailSubject;
        }

        public string GetQuarterInvitationEmailTemplate(Int64 QuarterId)
        {
            return _ObjUnitWork.QuarterEmailTemplate.Search(x => x.QuaterId == QuarterId).FirstOrDefault().InvitationEmailTemplate;
        }

        public string GetQuarterReminderEmailSubject(Int64 QuarterId)
        {
            return _ObjUnitWork.QuarterEmailTemplate.Search(x => x.QuaterId == QuarterId).FirstOrDefault().ReminderEmailSubject;
        }

        public string GetQuarterReminderEmailTemplate(Int64 QuarterId)
        {
            return _ObjUnitWork.QuarterEmailTemplate.Search(x => x.QuaterId == QuarterId).FirstOrDefault().ReminderEmailTemplate;
        }

        public void AddQuarterEmailTemplates(Int64 QuarterID, string InvitationEmailSubject, string InvitationEmailTemplate, string ReminderEmailSubject, string ReminderEmailTemplate)
        {
            QuarterEmailTemplate QET = new QuarterEmailTemplate();

            QET.QuaterId = QuarterID;
            QET.InvitationEmailSubject = InvitationEmailSubject;
            QET.InvitationEmailTemplate = InvitationEmailTemplate;
            QET.ReminderEmailSubject = ReminderEmailSubject;
            QET.ReminderEmailTemplate = ReminderEmailTemplate;

            _ObjUnitWork.QuarterEmailTemplate.Add(QET);
            _ObjUnitWork.Complete();
        }

        public void UpdateQuarterEmailTemplates(Int64 QuarterID, string InvitationEmailSubject, string InvitationEmailTemplate, string ReminderEmailSubject, string ReminderEmailTemplate)
        {
            QuarterEmailTemplate QET = _ObjUnitWork.QuarterEmailTemplate.Search(x => x.QuaterId == QuarterID).FirstOrDefault();

            QET.InvitationEmailSubject = InvitationEmailSubject;
            QET.InvitationEmailTemplate = InvitationEmailTemplate;
            QET.ReminderEmailSubject = ReminderEmailSubject;
            QET.ReminderEmailTemplate = ReminderEmailTemplate;

            _ObjUnitWork.QuarterEmailTemplate.Update(QET);
        }
    }
}
