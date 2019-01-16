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
    public class SurveyResponseService : ISurveyResponseService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public SurveyResponseService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<SurveyResult> GetSurveyResultId(Int64 SurveyId, Int64 BuilderId)
        {
            return _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId);
        }
        public IEnumerable<SurveyBuilder> GetSurveyBuilderId(Int64 SurveyId, Int64 BuilderId)
        {
            return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.BuilderId == BuilderId && x.RowStatusId==(int)RowActiveStatus.Active);
        }
        public SurveyResult GetSurveyResult(Int64 SurveyResultId)
        {
            return _ObjUnitWork.SurveyResult.Get(SurveyResultId);            
        }
        public void UpdateSurveyResult(SurveyResult ObjSurveyResult, bool DisposeConn = false)
        {
            if (DisposeConn == true)
            {
                _ObjUnitWork.Dispose();
            }
            else
            {
                _ObjUnitWork.SurveyResult.Update(ObjSurveyResult);
                _ObjUnitWork.Complete();
            }
        }
        public SurveyBuilder GetSurveyBuilder(Int64 SurveyBuilderId)
        {
            return _ObjUnitWork.SurveyBuilder.Get(SurveyBuilderId);
        }
        public void UpdateSurveyBuilder(SurveyBuilder ObjSurveyBuilder, bool DisposeConn = false)
        {
            if (DisposeConn == true)
            {
                _ObjUnitWork.Dispose();
            }
            else
            {
                _ObjUnitWork.SurveyBuilder.Update(ObjSurveyBuilder);
                _ObjUnitWork.Complete();
            }
        }
        public BuilderQuaterContractProjectDetails GetBuilderQuaterProjectDetails(Int64 ObjBuilderQuaterContractProjectDetailsId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectDetails.Get(ObjBuilderQuaterContractProjectDetailsId);
        }
        public void UpdateBuilderQuaterContractProjectDetails(BuilderQuaterContractProjectDetails ObjBuilderProjects, bool DisposeConn = false)
        {
            if (DisposeConn == true)
            {
                _ObjUnitWork.Dispose();
            }
            else
            {
                _ObjUnitWork.BuilderQuaterContractProjectDetails.Update(ObjBuilderProjects);
                _ObjUnitWork.Complete();
            }
        }
        public IEnumerable<BuilderQuaterContractProjectReport>GetBuilderQuaterContractProjectReport(Int64 ProjectId, Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.ContractId == ContractId && x.QuaterId == QuaterId);
        }
        public BuilderQuaterAdminReport GetBuilderQuaterAdminReportId(Int32 BuilderQuaterAdminReportId)
        {
            return _ObjUnitWork.BuilderQuaterAdminReport.Get(BuilderQuaterAdminReportId);
        }
        public void UpdateBuilderQuaterAdminReport(BuilderQuaterAdminReport ObjBuilderProjects, bool DisposeConn = false)
        {
            if (DisposeConn == true)
            {
                _ObjUnitWork.Dispose();
            }
            else
            {
                _ObjUnitWork.BuilderQuaterAdminReport.Update(ObjBuilderProjects);
                _ObjUnitWork.Complete();
            }
        }
        public IEnumerable<SurveyBuilder> GetParticipatedBuilder(Int64 SurveyId, int SurveyCompleteFilter)
        {
            switch (SurveyCompleteFilter)
            {
                case 1:  //All
                    return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active);
                    ;
                case 2://Complete
                    return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSurveyCompleted == true);
                    ;
                case 3: //Incomplete
                    return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSurveyCompleted == false);
                    ;
            }
            return null;

        }
        public IEnumerable<dynamic> GetParticipatedBuilderActive(Int64 SurveyId, int SurveyCompleteFilter)
        {
            //switch (SurveyCompleteFilter)
            //{
            //    case 1:  //All
            //        return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active);
            //        ;
            //    case 2://Complete
            //        return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSurveyCompleted == true);
            //        ;
            //    case 3: //Incomplete
            //        return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSurveyCompleted == false);
            //        ;
            //}
            //return null;
            var data = _ObjUnitWork.SurveyResult.GetParticipatedBuilderActive(SurveyId, SurveyCompleteFilter);
            return data;
        }
        public IEnumerable<Builder> GetBuilderDetails(Int64 BuilderId)
        {
            return _ObjUnitWork.Builder.Search(x => x.BuilderId == BuilderId);
        }
        public IEnumerable<SurveyResponse> GetBuilderSurveyResult(Int64 SurveyId, Int64 BuilderId)
        {
            return _ObjUnitWork.SurveyResult.GetBuilderSurveyResponse(SurveyId, BuilderId);
            // return null;
        }
        public IEnumerable<SurveyResponse> GetBuilderSurveyResponseFiltered(Int64 SurveyId, Int64 BuilderId, List<Int64> QuestionFilter)
        {
            return _ObjUnitWork.SurveyResult.GetBuilderSurveyResponseFiltered(SurveyId, BuilderId, QuestionFilter);
        }
        // added newly - angshuman on 17/09/2016
        public bool CheckNCPSurvey(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyResult.CheckNCPSurvey(SurveyId);
        }
        public IEnumerable<dynamic> GetParticipatedBuilderNCP(Int64 SurveyId, int SurveyCompleteFilter)
        {
            var data = _ObjUnitWork.SurveyResult.GetParticipatedBuilderNCP(SurveyId, SurveyCompleteFilter);
            //switch (SurveyCompleteFilter)
            //{
            //    case 1:  //All
            //        return data.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active);
            //        _ObjUnitWork.d
            //        ;
            //    case 2://Complete
            //        return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsComplete == true);
            //        ;
            //    case 3: //Incomplete
            //        return _ObjUnitWork.SurveyBuilder.Search(x => x.SurveyId == SurveyId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSurveyCompleted == false);
            //        ;
            //}
            return data;
        }
        public IEnumerable<SurveyResponseNCP> GetBuilderSurveyResponseNCP(Int64 SurveyId, Int64 BuilderId)
        {
            return _ObjUnitWork.SurveyResult.GetBuilderSurveyResponseNCP(SurveyId, BuilderId);
            // return null;
        }
        public string GetContractQuater(Int64 SurveyId)
        {
            return _ObjUnitWork.SurveyResult.GetContractQuater(SurveyId);
            // return null;
        }
        public IEnumerable<SurveyResponseNCP> GetBuilderSurveyResponseNCPFiltered(Int64 SurveyId, Int64 BuilderId, List<Int64> QuestionFilter)
        {
            return _ObjUnitWork.SurveyResult.GetBuilderSurveyResponseNCPFiltered(SurveyId, BuilderId, QuestionFilter);
            // return null;
        }
        public IEnumerable<string> GetBuilderMrketName(Int64 MarketId)
        {
            return _ObjUnitWork.Market.Find(a => a.MarketId == MarketId).Select(b => b.MarketName);
        }
    }
}
