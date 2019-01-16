using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Interface
{
    public interface ISurveyBuilderEmailSentService
    {
        void SaveSurveyEmailBuilder(BuilderSurveyEmailSent ObjBuilder);
        void UpdateSurveyEmailBuilder(BuilderSurveyEmailSent ObjBuilder);

        void SaveSurveyEmailBuilderUser(BuilderUserSurveyEmailSent ObjBuilder);
        void Flag(bool flag);

    }
}
