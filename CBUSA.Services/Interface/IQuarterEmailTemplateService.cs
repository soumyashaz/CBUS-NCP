using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;

namespace CBUSA.Services.Interface
{
    public interface IQuarterEmailTemplateService
    {
        QuarterEmailTemplate GetQuarterEmailTemplate(Int64 QuarterId);
        string GetQuarterInvitationEmailSubject(Int64 QuarterId);
        string GetQuarterInvitationEmailTemplate(Int64 QuarterId);
        string GetQuarterReminderEmailSubject(Int64 QuarterId);
        string GetQuarterReminderEmailTemplate(Int64 QuarterId);
        void AddQuarterEmailTemplates(Int64 QuarterID, string InvitationEmailSubject, string InvitationEmailTemplate, string ReminderEmailSubject, string ReminderEmailTemplate);
        void UpdateQuarterEmailTemplates(Int64 QuarterID, string InvitationEmailSubject, string InvitationEmailTemplate, string ReminderEmailSubject, string ReminderEmailTemplate);
    }
}
