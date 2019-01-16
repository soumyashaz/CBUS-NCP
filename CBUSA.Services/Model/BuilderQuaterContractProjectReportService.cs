using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;
using CBUSA.Repository.Model;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Entity;

namespace CBUSA.Services.Model
{
    public class BuilderQuaterContractProjectReportService : IBuilderQuaterContractProjectReportService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public BuilderQuaterContractProjectReportService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public void SaveProjectReport(BuilderQuaterContractProjectReport objProject, bool disposeDbContext = true)
        {
            _ObjUnitWork.BuilderQuaterContractProjectReport.Add(objProject);
            _ObjUnitWork.Complete();
            // --------- Neyaz On 9-Oct-2017 ---- #7119------ Sart
            //_ObjUnitWork.Dispose();
            if (disposeDbContext)
            {
                _ObjUnitWork.Dispose();
            }
            // --------- Neyaz On 9-Oct-2017 ---- #7119------ End
        }

        public void ForceDisposeDbContext()
        {
            _ObjUnitWork.Dispose();
        }
        public void Flag(bool flag)
        {
            _ObjUnitWork.Flag = flag;
        }
        public IEnumerable<BuilderQuaterContractProjectReport> CheckExistProjectAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ProjectId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ProjectId == ProjectId && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public IEnumerable<BuilderQuaterContractProjectReport> CheckExistProjectAgainstBuilderQuaterContract(Int64 BuilderId, Int64 QuaterId, Int64 ContractId, Int64 ProjectId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ContractId == ContractId && x.ProjectId == ProjectId && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        public void UpdateProjectReport(BuilderQuaterContractProjectReport objProject)
        {
            _ObjUnitWork.BuilderQuaterContractProjectReport.Update(objProject);
            _ObjUnitWork.Complete();
           // _ObjUnitWork.Dispose();            
        }
        public void SaveProjectReportInBulk(List<BuilderQuaterContractProjectReport> objProject)
        {
            _ObjUnitWork.BuilderQuaterContractProjectReport.AddRange(objProject);
            _ObjUnitWork.Complete();
            // _ObjUnitWork.Dispose();            
        }
        public void GetDispose()
        {
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }
        public void UpdateMultipleProjectReport(BuilderQuaterContractProjectReport objProject)
        {
            _ObjUnitWork.BuilderQuaterContractProjectReport.Update(objProject);

        }
        public IEnumerable<BuilderQuaterContractProjectReport> CheckExistContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        public IEnumerable<BuilderQuaterContractProjectReport> CheckAllContractForBuilderQuarter(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        public IEnumerable<BuilderQuaterContractProjectReport> CheckAllContractReportSubmit(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ProjectStatusId == (int)RowActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        public IEnumerable<BuilderQuaterContractProjectReport> CheckCompleteContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ContractId == ContractId && x.ProjectStatusId == (int)RowActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active && x.IsComplete == false);
        }
        public IEnumerable<BuilderQuaterContractProjectReport> CheckCompleteBuilderQuaterContractProjectReport(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ContractId == ContractId  && x.RowStatusId == (int)RowActiveStatus.Active && x.IsComplete == true);
        }

        public IEnumerable<BuilderQuaterContractProjectReport> GetSelectedProjectForQuater(Int64 ContractId, Int64 BuilderId, Int64 QuaterId)
        {

            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId &&
                 x.ContractId == ContractId && x.QuaterId == QuaterId
                 && x.RowStatusId == (int)RowActiveStatus.Active
                 && x.IsComplete == false
                 && x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit
               );
        }

        //public void SaveBuilderProjectResult(Int64 ContractId, Int64 BuilderId, Int64 QuaterId,
        //    List<Int64> ProjectList, List<BuilderQuaterContractProjectDetails> ObjReport, string ServerFilePath, bool AddToSurveyResponseHistory = false)
        public void SaveBuilderProjectResult(Int64 ContractId, Int64 BuilderId, Int64 QuaterId,
            List<Int64> ProjectList, List<BuilderQuaterContractProjectDetails> ObjReport, string ServerFilePath)
        {

            var BuilderQuaterReport = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId
                && x.ContractId == ContractId && x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active
                 && x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit
                 && ProjectList.Contains(x.ProjectId)
                 );
            if (BuilderQuaterReport != null)
            {
                if (BuilderQuaterReport.Count() > 0)
                {
                    foreach (var ItemChild in ObjReport)
                    {
                        Int64 BuilderQuaterReportId = BuilderQuaterReport.Where(x => x.ProjectId == ItemChild.BuilderQuaterContractProjectReport.ProjectId).FirstOrDefault().BuilderQuaterContractProjectReportId;

                        BuilderQuaterContractProjectDetails AdminReport = null;

                        if(ItemChild.Question.QuestionTypeId!=(int)EnumQuestionType.Grid)
                        {
                            AdminReport = _ObjUnitWork.BuilderQuaterContractProjectDetails.
                           Search(x => x.BuilderQuaterContractProjectReportId ==
                               BuilderQuaterReportId && x.QuestionId == ItemChild.QuestionId
                               && x.RowStatusId == (int)RowActiveStatus.Active
                               ).FirstOrDefault();
                        }
                        else
                        {
                            AdminReport = _ObjUnitWork.BuilderQuaterContractProjectDetails.
                           Search(x => x.BuilderQuaterContractProjectReportId ==
                               BuilderQuaterReportId && x.QuestionId == ItemChild.QuestionId
                               && x.RowNumber==ItemChild.RowNumber && x.ColumnNumber==ItemChild.ColumnNumber
                               && x.RowStatusId == (int)RowActiveStatus.Active
                               ).FirstOrDefault();
                        }
                        
                        if (AdminReport != null)
                        {
                            AdminReport.Answer = ItemChild.Answer;
                            AdminReport.ModifiedBy = 1;
                            AdminReport.ModifiedOn = DateTime.Now;

                            if (ItemChild.FileName != null)
                            {
                                if (AdminReport.FileName != null)
                                {
                                    var physicalPath = Path.Combine(ServerFilePath, AdminReport.FileName);
                                    if (System.IO.File.Exists(physicalPath))
                                    {
                                        System.IO.File.Delete(physicalPath);
                                    }
                                }
                                AdminReport.FileName = ItemChild.FileName;
                            }
                            //if (AddToSurveyResponseHistory)
                            //{
                            //    // have to add ncp response in history before updating details for dynamics difference value calculation.
                            //    SurveyResponseHistory ResponseHistory = new SurveyResponseHistory();
                            //    ResponseHistory.Answer = AdminReport.Answer;
                            //    ResponseHistory.RowNumber = AdminReport.RowNumber;
                            //    ResponseHistory.ColumnNumber = AdminReport.ColumnNumber;
                            //    ResponseHistory.QuestionId = AdminReport.QuestionId;
                            //    ResponseHistory.BuilderQuaterContractProjectReportId = BuilderQuaterReportId;
                            //    ResponseHistory.FileName = AdminReport.FileName != null ? AdminReport.FileName : null;
                            //    ResponseHistory.RowStatusId = (int) RowActiveStatus.Active;
                            //    ResponseHistory.CreatedBy = 1;
                            //    ResponseHistory.ModifiedBy = 1;
                            //    ResponseHistory.CreatedOn = DateTime.Now;
                            //    ResponseHistory.ModifiedOn = DateTime.Now;
                            //    ResponseHistory.RowGUID = Guid.NewGuid();
                            //    _ObjUnitWork.SurveyResponseHistory.Add(ResponseHistory);
                            //}
                            
                            _ObjUnitWork.BuilderQuaterContractProjectDetails.Update(AdminReport);
                        }
                        else
                        {
                            BuilderQuaterContractProjectDetails ObjReportNew = new BuilderQuaterContractProjectDetails
                            {
                                Answer = ItemChild.Answer,
                                RowNumber = ItemChild.RowNumber,
                                ColumnNumber = ItemChild.ColumnNumber,
                                QuestionId = ItemChild.QuestionId,
                                BuilderQuaterContractProjectReportId = BuilderQuaterReportId,
                                FileName = ItemChild.FileName != null ? ItemChild.FileName : null,
                                RowStatusId = (int)RowActiveStatus.Active,
                                CreatedBy = 1,
                                ModifiedBy = 1,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                                RowGUID = Guid.NewGuid(),
                            };
                            _ObjUnitWork.BuilderQuaterContractProjectDetails.Add(ObjReportNew);
                        }
                       

                    }

                    foreach (var Item in BuilderQuaterReport)
                    {
                        Item.IsComplete = true;
                        Item.CompleteDate = DateTime.Now;
                    }
                    _ObjUnitWork.Complete();
                    //_ObjUnitWork.Dispose();
                    // call stored proc to calculate volume fee difference                        
                    //if (AddToSurveyResponseHistory)
                    //{
                    //    SqlConnection con;
                    //    string constr = ConfigurationManager.ConnectionStrings["CBUSA"].ToString();
                    //    con = new SqlConnection(constr);
                    //    SqlCommand com = new SqlCommand("Proc_ReValidateNCPVolumeFee", con);
                    //    com.Parameters.AddWithValue("@InputBuilderId", BuilderId.ToString());
                    //    com.Parameters.AddWithValue("@InputQuarterId", QuaterId.ToString());
                    //    com.Parameters.AddWithValue("@InputContractId", ContractId.ToString());
                    //    com.CommandType = CommandType.StoredProcedure;
                    //    //SqlDataAdapter da = new SqlDataAdapter(com);
                    //    //DataTable dt = new DataTable();                 
                    //    try
                    //    {
                    //        con.Open();
                    //        com.ExecuteNonQuery();                            
                    //    }
                    //    catch(Exception ee)
                    //    {
                    //        //throw new Exception(ee.Message.ToString());
                    //    }
                    //    finally
                    //    {
                    //        com.Dispose();
                    //        //da.Fill(dt);
                    //        con.Close();
                    //        //dt.Dispose();
                    //        //da.Dispose();
                    //        con.Dispose();
                    //    }                        
                    //}
                }
            }
        }

        public IEnumerable<BuilderQuaterContractProjectReport> GetBuilderSeletedProjectReportForQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.GetBuilderSeletedProjectReportForQuater(BuilderId, QuaterId, ContractId);
        }

        public IEnumerable<BuilderQuaterContractProjectReport> GetAllProjectCountForQuater(Int64 ContractId, Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.ContractId == ContractId && x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public IEnumerable<BuilderQuaterContractProjectReport> GetRepotDetails(Int64 BuilderId, Int64 ContractId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active);

            // return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x=>BuilderId==BuilderId && x=>x.);
        }
        public IEnumerable<BuilderQuarterContractStatus> GetReportDetails(Int64 BuilderId, Int64 ContractId)
        {
            return _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.BuilderId == BuilderId && x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active);

            // return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x=>BuilderId==BuilderId && x=>x.);
        }
        public IEnumerable<BuilderQuaterContractProjectReport> GetActiveRepotDetails(Int64 BuilderId, Int64 ContractId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.ContractId == ContractId && x.ProjectStatusId == (int)RowActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active);

            // return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x=>BuilderId==BuilderId && x=>x.);
        }
        public IEnumerable<BuilderQuaterContractProjectReport> GetActiveQuaterRepotDetails(Int64 BuilderId, Int64 ContractId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.ContractId == ContractId && x.QuaterId == QuaterId && x.ProjectStatusId == (int)RowActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active);

            // return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x=>BuilderId==BuilderId && x=>x.);
        }
        public IEnumerable<BuilderQuaterContractProjectReport> GetRepotDetailsofAllContract(Int64 BuilderId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        #region Add ProjectStatus

        public IEnumerable<BuilderQuaterContractProjectReport> GetBuilderCurrentQuaterSelectedProject(Int64 BuilderId, Int64 ContractId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId &&
                x.ContractId == ContractId && x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active);

        }


        public BuilderQuaterContractProjectReport GetBuilderQuaterContractProjectReport(Int64 BuilderId, Int64 ContractId, Int64 QuaterId, Int64 ProjectId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId &&
                x.ContractId == ContractId && x.QuaterId == QuaterId
                && x.ProjectId == ProjectId
                && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

        }
        public IEnumerable<dynamic> GetDataIntoListQuery(string query)
        {
            DataTable dt = new DataTable();
            using (CBUSADbContext db = new CBUSADbContext())
            {
                string connString = db.Database.Connection.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    da.Dispose();
                }
            }
            return dt.AsEnumerable().ToList();
        }
        public string DeleteDataUsingADONET(string query)
        {
            string Result = "";
            using (CBUSADbContext db = new CBUSADbContext())
            {
                string connString = db.Database.Connection.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if(conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    Result = cmd.ExecuteNonQuery().ToString();
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return Result;
        }
        public void SaveBuilderProjectStatus(List<BuilderQuaterContractProjectReport> objProject)
        {
            // -- added by angshuman on 12-april-2018
            var BuilderId = objProject.Select(x => x.BuilderId).FirstOrDefault();
            var QuarterId = objProject.Select(x => x.QuaterId).FirstOrDefault();
            var ContractId = objProject.Select(x => x.ContractId).FirstOrDefault();
            
            string SQLQuery = "";
            SQLQuery = " select a.* ";
            SQLQuery += " from BuilderQuaterContractProjectDetails a inner join BuilderQuaterContractProjectReport b ";
            SQLQuery += " on a.BuilderQuaterContractProjectReportid = b.BuilderQuaterContractProjectReportid ";
            SQLQuery += " where b.builderid = " + BuilderId;
            SQLQuery += "  and b.quaterid = " + QuarterId;
            SQLQuery += "  and b.contractid =" + ContractId;
            SQLQuery += "  and b.RowStatusId = 1 ";
            SQLQuery += "  and a.RowStatusId = 1 ";

            var ProjectDetailsList = GetDataIntoListQuery(SQLQuery); // _ObjUnitWork.BuilderQuaterContractProjectDetails.GetAll();

            SQLQuery = " select b.* ";
            SQLQuery += " from BuilderQuaterContractProjectReport b ";
            SQLQuery += " where b.builderid = " + BuilderId;
            SQLQuery += "  and b.quaterid = " + QuarterId;
            SQLQuery += "  and b.contractid =" + ContractId;
            SQLQuery += "  and b.RowStatusId = 1 ";
            var BuilderQtrContractProjReport = GetDataIntoListQuery(SQLQuery); // _ObjUnitWork.BuilderQuaterContractProjectReport.Find(x => x.BuilderId == BuilderId);

            List<BuilderQuaterContractProjectReport> ObjBuilderQuaterContractProjectReport = new List<BuilderQuaterContractProjectReport>();
            foreach (var item in BuilderQtrContractProjReport)
            {
                ObjBuilderQuaterContractProjectReport.Add(new BuilderQuaterContractProjectReport
                {
                    BuilderQuaterContractProjectReportId = Convert.ToInt64(item[0]),
                    BuilderId = Convert.ToInt64(item[1]),
                    QuaterId = Convert.ToInt64(item[2]),
                    ContractId = Convert.ToInt64(item[3]),
                    ProjectId = Convert.ToInt64(item[4]),
                    ProjectStatusId = Convert.ToInt64(item[5]),
                    BuilderQuaterAdminReportId = Convert.ToInt64(item[6]),
                    IsComplete = Convert.ToBoolean(item[7]),
                    CompleteDate = Convert.ToDateTime(item[8]),
                    RowStatusId = Convert.ToInt16(item[9]),
                    CreatedOn = Convert.ToDateTime(item[10]),
                    CreatedBy = Convert.ToInt16(item[11]),
                    ModifiedOn = Convert.ToDateTime(item[12]),
                    ModifiedBy = Convert.ToInt16(item[13]),
                    RowGUID = item[14],
                    BuilderQuarterContractStatusId = Convert.ToInt64(item[15])
                });
            }

            List<BuilderQuaterContractProjectDetails> ObjBuilderQuaterContractProjectDetails = new List<BuilderQuaterContractProjectDetails>();
            foreach (var item in ProjectDetailsList)
            {
                ObjBuilderQuaterContractProjectDetails.Add(new BuilderQuaterContractProjectDetails
                {
                    BuilderQuaterContractProjectDetailsId = Convert.ToInt64(item[0]),
                    Answer = Convert.ToString(item[1]) ,
                    RowNumber = Convert.ToInt16(item[2]),
                    ColumnNumber = Convert.ToInt16(item[3]),
                    QuestionId = Convert.ToInt64(item[4]),
                    BuilderQuaterContractProjectReportId = Convert.ToInt64(item[5]),
                    FileName = Convert.ToString(item[12]),
                    RowStatusId = Convert.ToInt16(item[6]),
                    RowGUID = item[11],
                    CreatedBy = Convert.ToInt16(item[8]),
                    CreatedOn = Convert.ToDateTime(item[7]),
                    ModifiedBy = Convert.ToInt16(item[10]),
                    ModifiedOn = Convert.ToDateTime(item[9])
                });
            }
            // -- added by angshuman on 12-april-2018 for optimization of project status save time

            foreach (var Item in objProject)
            {
                //var QuaterProjectStatus = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == Item.BuilderId
                //   && x.ContractId == Item.ContractId
                //   && x.QuaterId == Item.QuaterId
                //    && x.ProjectId == Item.ProjectId
                //    ).FirstOrDefault();
             
                if (ObjBuilderQuaterContractProjectReport != null && ObjBuilderQuaterContractProjectReport.Count() >0)
                {
                    var QuaterProjectStatus = ObjBuilderQuaterContractProjectReport.Where(x => x.ContractId == Item.ContractId
                                                                               && x.QuaterId == Item.QuaterId
                                                                                && x.ProjectId == Item.ProjectId
                                                                                ).FirstOrDefault();
                    if (QuaterProjectStatus == null )
                    {
                        _ObjUnitWork.BuilderQuaterContractProjectReport.Add(Item);
                    }
                    else
                    {
                        QuaterProjectStatus.ProjectStatusId = Item.ProjectStatusId;
                        QuaterProjectStatus.IsComplete = Item.ProjectStatusId == 1 ? false : true;
                        QuaterProjectStatus.ModifiedOn = DateTime.Now;
                        QuaterProjectStatus.BuilderQuarterContractStatusId = Item.BuilderQuarterContractStatusId; // added newly on 20-April-2018 - angshuman.
                        _ObjUnitWork.BuilderQuaterContractProjectReport.UpdateAsync(QuaterProjectStatus);
                        //========== Neyaz === 27-Oct-2017 ==== VSO#8627===== Start
                        if (Item.ProjectStatusId != 1)
                        {
                            //var ProjectDetails = _ObjUnitWork.BuilderQuaterContractProjectDetails.Find(x => x.BuilderQuaterContractProjectReportId == QuaterProjectStatus.BuilderQuaterContractProjectReportId);
                            var ProjectDetails = ObjBuilderQuaterContractProjectDetails.Where(x => x.BuilderQuaterContractProjectReportId == QuaterProjectStatus.BuilderQuaterContractProjectReportId);
                            if (ProjectDetails != null)
                            {
                                if (ProjectDetails.Count() > 0)
                                {
                                    //foreach (var DetItem in ProjectDetails)
                                    //{
                                    //    _ObjUnitWork.BuilderQuaterContractProjectDetails.Remove (DetItem);
                                    //}
                                    
                                    //_ObjUnitWork.BuilderQuaterContractProjectDetails.RemoveRange(ProjectDetails);

                                    SQLQuery = "";
                                    SQLQuery = " delete ";
                                    SQLQuery += " from BuilderQuaterContractProjectDetails ";
                                    SQLQuery += " where BuilderQuaterContractProjectReportId = " + QuaterProjectStatus.BuilderQuaterContractProjectReportId;
                                    var Result = DeleteDataUsingADONET(SQLQuery);
                                }
                            }
                        }
                        //========== Neyaz === 27-Oct-2017 ==== VSO#8627===== End
                    }
                }  
                else
                {
                    if (ObjBuilderQuaterContractProjectReport.Count() ==0)
                    {
                        _ObjUnitWork.BuilderQuaterContractProjectReport.Add(Item);
                    }
                }
            }
            _ObjUnitWork.Complete();
           // _ObjUnitWork.Dispose();
        }

        public void UpdateBuilderContractProjectStatus(List<BuilderQuaterContractProjectReport> objProject, bool disposeDbContext = true)
        {
            string ErrMsg="";
            try
            {                
                foreach (var Item in objProject)
                {
                    _ObjUnitWork.BuilderQuaterContractProjectReport.Update(Item);
                    
                    var ProjectDetails = _ObjUnitWork.BuilderQuaterContractProjectDetails.
                                         Find(x => x.BuilderQuaterContractProjectReportId == Item.BuilderQuaterContractProjectReportId);

                    if (ProjectDetails != null)
                    {
                        if (ProjectDetails.Count() > 0)
                        {
                            foreach (var DetItem in ProjectDetails)
                            {
                                _ObjUnitWork.BuilderQuaterContractProjectDetails.Remove(DetItem);
                            }
                        }
                    }
                }
                _ObjUnitWork.Complete();                
                _ObjUnitWork.Dispose();                
            }
            catch ( Exception e)
            {
                ErrMsg = e.Message.ToString();
            }
        }

        #endregion
        public IEnumerable<BuilderQuaterContractProjectReport> GetRepotDetailsofSpecificAdminContractId(Int64 BuilderAdminContractId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderQuaterAdminReportId == BuilderAdminContractId && x.ProjectStatusId == (int)RowActiveStatus.Active && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        public void EditBuilderProjectResultByAdmin(Int64 ContractId, Int64 BuilderId, Int64 QuaterId,
         Int64 ProjectId, List<BuilderQuaterContractProjectDetails> ObjReport, string ServerFilePath)
        {

            var BuilderQuaterReport = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId
                && x.ContractId == ContractId && x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active
                 && x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit
                 && x.ProjectId == ProjectId
                 ).FirstOrDefault();
            if (BuilderQuaterReport != null)
            {
                //if (BuilderQuaterReport.Count() > 0)
                //{
                foreach (var ItemChild in ObjReport)
                {

                    // Int64 BuilderQuaterReportId = BuilderQuaterReport.Where(x => x.ProjectId == ItemChild.BuilderQuaterContractProjectReport.ProjectId).FirstOrDefault().BuilderQuaterContractProjectReportId;
                    var AdminReport = _ObjUnitWork.BuilderQuaterContractProjectDetails.
                        Search(x => x.BuilderQuaterContractProjectReportId == BuilderQuaterReport.BuilderQuaterContractProjectReportId
                             && x.QuestionId == ItemChild.QuestionId
                            && x.RowStatusId == (int)RowActiveStatus.Active
                            ).FirstOrDefault();

                    if (AdminReport != null)
                    {
                        AdminReport.Answer = ItemChild.Answer;
                        AdminReport.ModifiedBy = 1;
                        AdminReport.ModifiedOn = DateTime.Now;

                        if (ItemChild.FileName != null)
                        {
                            if (AdminReport.FileName != null)
                            {
                                var physicalPath = Path.Combine(ServerFilePath, AdminReport.FileName);
                                if (System.IO.File.Exists(physicalPath))
                                {
                                    System.IO.File.Delete(physicalPath);
                                }
                            }
                            AdminReport.FileName = ItemChild.FileName;
                        }
                        
                        _ObjUnitWork.BuilderQuaterContractProjectDetails.Update(AdminReport);
                    }

                }
                
                _ObjUnitWork.Complete();
                _ObjUnitWork.Dispose();
                //}
            }

        }
        public void UpdateBuilderProjectStatus(Int64 BuilderId, Int64 ContractId, Int64 QuaterId, Int64 ProjectId)
        {
            var QuaterProjectStatus = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId
                && x.ContractId == ContractId
                && x.QuaterId == QuaterId
                 && x.ProjectId == ProjectId
                 ).FirstOrDefault();

            if (QuaterProjectStatus != null)
            {
                QuaterProjectStatus.ProjectStatusId = 1;
                QuaterProjectStatus.IsComplete = false;
                QuaterProjectStatus.ModifiedOn = DateTime.Now;
                _ObjUnitWork.BuilderQuaterContractProjectReport.Update(QuaterProjectStatus);
            }
            else
            {
                var BuilderQuaterReport = _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId).FirstOrDefault();
                var QuarterContractStatus = _ObjUnitWork.BuilderQuarterContractStatus.Search(z => z.BuilderId == BuilderId && z.QuaterId == QuaterId && z.ContractId == ContractId && z.RowStatusId == (int)RowActiveStatus.Active).Select(y => y.BuilderQuarterContractStatusId).FirstOrDefault();
                if (BuilderQuaterReport != null && QuarterContractStatus >0)
                {

                    BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
                    objProjectReport.ProjectId = ProjectId;
                    objProjectReport.BuilderId = BuilderId;
                    objProjectReport.QuaterId = QuaterId;
                    objProjectReport.ContractId = ContractId;
                    objProjectReport.ProjectStatusId = (int)EnumProjectStatus.ReportUnit;
                    objProjectReport.IsComplete = false;
                    objProjectReport.CreatedOn = DateTime.Now;
                    objProjectReport.ModifiedOn = DateTime.Now;
                    objProjectReport.CreatedBy = 1;
                    objProjectReport.ModifiedBy = 1;
                    objProjectReport.CompleteDate = DateTime.Now;
                    objProjectReport.RowGUID = Guid.NewGuid();
                    objProjectReport.BuilderQuarterContractStatusId = QuarterContractStatus;

                    objProjectReport.BuilderQuaterAdminReportId = BuilderQuaterReport.BuilderQuaterAdminReportId;
                    _ObjUnitWork.BuilderQuaterContractProjectReport.Add(objProjectReport);

                }

            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void UpdateBuilderMultipleProjectStatus(Int64 BuilderId, Int64 ContractId, Int64 QuaterId, List<Int64> ListProjectId)
        {

            foreach (var Item in ListProjectId)
            {

                var QuaterProjectStatus = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId
                && x.ContractId == ContractId
                && x.QuaterId == QuaterId
                 && x.ProjectId == Item
                 ).FirstOrDefault();
                if (QuaterProjectStatus != null)
                {
                    QuaterProjectStatus.ProjectStatusId = 1;
                    QuaterProjectStatus.IsComplete = false;
                    QuaterProjectStatus.ModifiedOn = DateTime.Now;
                    _ObjUnitWork.BuilderQuaterContractProjectReport.Update(QuaterProjectStatus);
                }
                else
                {
                    var BuilderQuaterReport = _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId).FirstOrDefault();
                    if (BuilderQuaterReport != null)
                    {

                        BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
                        objProjectReport.ProjectId = Item;
                        objProjectReport.BuilderId = BuilderId;
                        objProjectReport.QuaterId = QuaterId;
                        objProjectReport.ContractId = ContractId;
                        objProjectReport.ProjectStatusId = (int)EnumProjectStatus.ReportUnit;
                        objProjectReport.IsComplete = false;
                        objProjectReport.CreatedOn = DateTime.Now;
                        objProjectReport.ModifiedOn = DateTime.Now;
                        objProjectReport.CreatedBy = 1;
                        objProjectReport.ModifiedBy = 1;
                        objProjectReport.CompleteDate = DateTime.Now;
                        objProjectReport.RowGUID = Guid.NewGuid();
                        objProjectReport.BuilderQuaterAdminReportId = BuilderQuaterReport.BuilderQuaterAdminReportId;
                        _ObjUnitWork.BuilderQuaterContractProjectReport.Add(objProjectReport);
                    }
                }

            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
            
        }
        
        public IEnumerable<BuilderQuaterContractProjectReport> CheckExistAllContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId, Int64 ProjectId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ContractId == ContractId && x.ProjectId == ProjectId);
        }
        public IEnumerable<BuilderQuaterContractProjectReport> CheckExistAllContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId);
        }
        public IEnumerable<BuilderQuaterContractProjectReport> GetLatestContractAgainstBuilderProject(Int64 BuilderId, Int64 ProjectId)
        {
            //long BuilderQuaterContractProjectReportId = 0;
            var BuilderQuaterReport = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.ProjectId == ProjectId);
            //if (BuilderQuaterReport.Count() > 0)
            //{
            //    var latest = BuilderQuaterReport.OrderByDescending(m => m.ModifiedOn).FirstOrDefault();
            //    //BuilderQuaterContractProjectReportId = BuilderQuaterReport.Max(y => y.BuilderQuaterContractProjectReportId);
            //    BuilderQuaterContractProjectReportId = latest.BuilderQuaterContractProjectReportId;
            //}
            //else
            //    BuilderQuaterContractProjectReportId = 0;

            //return _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderQuaterContractProjectReportId == BuilderQuaterContractProjectReportId);

            return BuilderQuaterReport;
        }
    }
}
