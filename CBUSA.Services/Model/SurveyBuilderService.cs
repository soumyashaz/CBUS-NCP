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
   public class SurveyBuilderService : ISurveyBuilderService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public SurveyBuilderService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<SurveyBuilder> FindInCompleteSurveyBuilderBySurveyId(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.IsSurveyCompleted == false && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public IEnumerable<SurveyBuilder> FindInCompleteSurveyBySurveyIdBuilderId(Int64 SurveyId,Int64 BuilderId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.IsSurveyCompleted == false && x.RowStatusId == (int)RowActiveStatus.Active && x.BuilderId==BuilderId);

        }
        public IEnumerable<SurveyBuilder> FindCompleteSurveyBuilderBySurveyId(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.IsSurveyCompleted == true && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public IEnumerable<SurveyBuilder> FindExistSurveyBuilderBySurveyId(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.RowStatusId==(int)RowActiveStatus.Active);

        }

        public IEnumerable<SurveyBuilder> FindSurveyOfBuilder(Int64 SurveyId,Int64 BuilderId)
        {
            return _ObjUnitWork.SurveyBuilder.Find(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public IEnumerable<SurveyBuilder> FindCompleteBuilderSurvey(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.IsSurveyCompleted==true && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public IEnumerable<SurveyBuilder> FindInCompleteBuilderSurvey(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.IsSurveyCompleted == false && x.RowStatusId == (int)RowActiveStatus.Active);

        }
    }
}
