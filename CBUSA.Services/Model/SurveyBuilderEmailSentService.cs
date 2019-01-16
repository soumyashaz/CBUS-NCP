using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Model
{
    public class SurveyBuilderEmailSentService : ISurveyBuilderEmailSentService
    {
        private readonly IUnitOfWork _ObjUnitWork;


        public SurveyBuilderEmailSentService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public void Flag(bool flag)
        {
            _ObjUnitWork.Flag = flag;
        }
        public void SaveSurveyEmailBuilder(BuilderSurveyEmailSent ObjBuilder)
        {
            _ObjUnitWork.BuilderEmailSent.Add(ObjBuilder);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }
        public void UpdateSurveyEmailBuilder(BuilderSurveyEmailSent ObjBuilder)
        {
            _ObjUnitWork.BuilderEmailSent.Update(ObjBuilder);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public void SaveSurveyEmailBuilderUser(BuilderUserSurveyEmailSent ObjBuilder)
        {
            _ObjUnitWork.BuilderUserEmailSent.Add(ObjBuilder);
        }

    }
}
