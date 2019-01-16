using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;
using System.Data;
using System.Data.SqlClient;

namespace CBUSA.Services.Model
{
    public class BuilderQuaterContractProjectDetailsService : IBuilderQuaterContractProjectDetailsService
    {
         private readonly IUnitOfWork _ObjUnitWork;
         public BuilderQuaterContractProjectDetailsService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }        // public IEnumerable<BuilderQuaterContractProjectDetails> GetAllProjectDetailsOfContract(Int64 QuestionId, Int64 BuilderQuaterAdminReportId)
        //{
        //   return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x=>x.QuestionId==QuestionId && x.BuilderQuaterContractProjectReportId==BuilderQuaterAdminReportId)
        //    return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x => x.QuestionId ==
        //        QuestionId && x.BuilderQuaterContractProjectReportId == BuilderQuaterAdminReportId && x.RowStatusId==(int)RowActiveStatus.Active);
        //}


         IEnumerable<Domain.BuilderQuaterContractProjectDetails> IBuilderQuaterContractProjectDetailsService.GetAllProjectDetailsOfContract(long QuestionId, long BuilderQuaterAdminReportId)
         {
             return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x => x.QuestionId ==
                QuestionId && x.BuilderQuaterContractProjectReportId == BuilderQuaterAdminReportId && x.RowStatusId == (int)RowActiveStatus.Active);
         }

        IEnumerable<Domain.BuilderQuaterContractProjectDetails> IBuilderQuaterContractProjectDetailsService.GetProjectDetailsForBuilderQuaterContractProjectReport(long BuilderQuaterContractProjectReportId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x => x.BuilderQuaterContractProjectReportId == BuilderQuaterContractProjectReportId && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        public IEnumerable<dynamic> GetProjectDetailsForBuilderQuaterContractProjectReportByQuater(Int64 QuaterID)
        {
            CBUSADbContext db = new CBUSADbContext();
            string connString = db.Database.Connection.ConnectionString;
            string query = String.Concat("select Report.BuilderId, B.BuilderName, Details.QuestionId, S.SurveyName, Details.FileName ",
                                                "from BuilderQuaterContractProjectReport Report, BuilderQuaterContractProjectDetails Details, Builder B, Question Q, Survey S ",
                                                "where Report.BuilderQuaterContractProjectReportId = Details.BuilderQuaterContractProjectReportId ",
                                                "and Report.BuilderId = B.BuilderId ",
                                                "and Details.QuestionId = Q.QuestionId ",
                                                "and Q.SurveyId = S.SurveyId ",
                                                "and Report.QuaterId = "+QuaterID+" ",
                                                "and Details.filename is not null ",
                                                "group by Report.BuilderId, B.BuilderName, Details.QuestionId, S.SurveyName, Details.FileName"); ;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                da.Dispose();
            }

            return dt.AsEnumerable();

        }

        public void AddProjectReportDetails(BuilderQuaterContractProjectDetails objProjectDetails, bool disposeDbContext = true)
        {
            _ObjUnitWork.BuilderQuaterContractProjectDetails.Add(objProjectDetails);
            _ObjUnitWork.Complete();

            if (disposeDbContext)
            {
                _ObjUnitWork.Dispose();
            }
        }

        public void UpdateProjectReportDetails(BuilderQuaterContractProjectDetails objProjectDetails, bool disposeDbContext = true)
        {
            _ObjUnitWork.BuilderQuaterContractProjectDetails.Update(objProjectDetails);
            _ObjUnitWork.Complete();

            if (disposeDbContext)
            {
                _ObjUnitWork.Dispose();
            }
        }
    }
}
