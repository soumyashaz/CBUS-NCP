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
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ProjectService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public void SaveProject(Project ObjProject, bool disposeDbContext = true)
        {
            _ObjUnitWork.Project.Add(ObjProject);
            _ObjUnitWork.Complete();
            if (disposeDbContext)
            {
                _ObjUnitWork.Dispose();
            }
        }

        public void SaveBuilderProject(List<Project> ObjProjectVM)
        {
            foreach (var Item in ObjProjectVM)
            {
                if (Item.ProjectId != 0)
                {
                    var ObjProject = _ObjUnitWork.Project.Search(x => x.ProjectId == Item.ProjectId).FirstOrDefault();
                    if (ObjProject != null)
                    {
                        ObjProject.ProjectName = Item.ProjectName;
                        ObjProject.LotNo = Item.LotNo;
                        ObjProject.Address = Item.Address;
                        ObjProject.Zip = Item.Zip;
                        ObjProject.State = Item.State;
                        ObjProject.City = Item.City;

                        _ObjUnitWork.Project.Update(ObjProject);
                    }
                }
                else
                {
                    _ObjUnitWork.Project.Add(Item);
                }
            }
            //   _ObjUnitWork.Project.Add(ObjProject);
            _ObjUnitWork.Complete();
            // --------- Neyaz On 9-Oct-2017 ---- #7119------ Sart
            //_ObjUnitWork.Dispose();            
            // --------- Neyaz On 9-Oct-2017 ---- #7119------ End
        }

        public void Flag(bool flag)
        {
            _ObjUnitWork.Flag = flag;
        }
        public IEnumerable<Project> BuilderProjectList(Int64 BuilderId)
        {
            return _ObjUnitWork.Project.Search(x => x.BuilderId == BuilderId);
        }

        public IEnumerable<dynamic> BuilderProjectListUsingProc(Int64 BuilderId, int RowStatusId)
        {
            DataTable dt = new DataTable();
            using (CBUSADbContext db = new CBUSADbContext())
            {
                string connString = db.Database.Connection.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("proc_GetBuilderProjectList", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BuilderId", BuilderId);
                    cmd.Parameters.AddWithValue("@RowStatusId", RowStatusId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    da.Dispose();
                }
            }

            var DataRowList = dt.AsEnumerable().ToList();

            List<dynamic> ProjectList = new List<dynamic>();
            foreach (DataRow o in DataRowList)
            {
                CustomProjectModel Proj = new CustomProjectModel();

                Proj.BuilderId = Convert.ToInt64(o.ItemArray[0]);
                Proj.ProjectId = Convert.ToInt64(o.ItemArray[1]);
                Proj.ProjectName = o.ItemArray[2].ToString();
                Proj.LotNo = o.ItemArray[3].ToString();
                Proj.Address = o.ItemArray[4].ToString();
                Proj.Zip = o.ItemArray[5].ToString();
                Proj.RowStatusId = Convert.ToInt32(o.ItemArray[6]);
                Proj.StateId = Convert.ToInt32(o.ItemArray[7]);
                Proj.State = o.ItemArray[8].ToString();
                Proj.City = o.ItemArray[9].ToString();
                Proj.ReportedContractIds = o.ItemArray[10].ToString();

                ProjectList.Add(Proj);
            }

            return ProjectList.AsEnumerable();
        }

        public IEnumerable<Project> ProjectList()
        {
            return _ObjUnitWork.Project.Search(x => x.RowStatusId == (int)RowActiveStatus.Active);

            // return list;
        }
        public IEnumerable<Project> CopyProject(Int64 ProjectId)
        {
            return _ObjUnitWork.Project.Search(x => x.ProjectId == ProjectId);
        }
        public void UpdateProject(Project ObjProject, bool disposeDbContext = true)
        {
            _ObjUnitWork.Project.Update(ObjProject);
            _ObjUnitWork.Complete();
            if (disposeDbContext)
            {
                _ObjUnitWork.Dispose();
            }
        }

        public IEnumerable<Project> GetBuilderActiveProject(Int64 BuilderId)
        {
            return _ObjUnitWork.Project.
                    Search(x => x.BuilderId == BuilderId
                    && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        public IEnumerable<Project> GetBuilderSeletedProjectForQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            return _ObjUnitWork.Project.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId);

        }
        public IEnumerable<Project> GetBuilderSeletedProjectForQuaterHistory(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            return _ObjUnitWork.Project.GetBuilderSeletedProjectForQuaterHistory(BuilderId, QuaterId, ContractId);

        }

        #region Add ProjectStatus

        public IEnumerable<Project> GetBuilderProject(Int64 ContractId, Int64 BuilderId, List<Int64> PreviousQuater)
        {

            var ProjectNeverReportHistory = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
               && x.BuilderId == BuilderId && PreviousQuater.Contains(x.QuaterId) && x.ProjectStatusId == (int)EnumProjectStatus.NeverReport
               ).Select(x => x.ProjectId).ToList();

            var ReportUnitHistory = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
                && x.BuilderId == BuilderId && PreviousQuater.Contains(x.QuaterId) && x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit
                ).Select(x => x.ProjectId).ToList();


            return _ObjUnitWork.Project.Search(x => !ProjectNeverReportHistory.Contains(x.ProjectId) && !ReportUnitHistory.Contains(x.ProjectId) && x.BuilderId == BuilderId && x.RowStatusId ==
                (int)RowActiveStatus.Active, q => q.OrderBy(d => d.ProjectName));
        }


        public IEnumerable<Project> GetContractBuilderCurrentQuaterProject(Int64 ContractId, Int64 BuilderId, List<Int64> PreviousQuater, Int64 FilterProjectId)
        {

            var ProjectNeverReportHistory = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
                && x.BuilderId == BuilderId && PreviousQuater.Contains(x.QuaterId) && x.ProjectStatusId == (int)EnumProjectStatus.NeverReport
                ).Select(x => x.ProjectId).ToList();

            var ReportUnitHistory = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
                && x.BuilderId == BuilderId && PreviousQuater.Contains(x.QuaterId) && x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit
                ).Select(x => x.ProjectId).ToList();

            var ProjCurrentQuater = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
                    && x.BuilderId == BuilderId && !PreviousQuater.Contains(x.QuaterId)).Select(x => x.ProjectId).ToList();
            if (ProjCurrentQuater.Count > 0)
            {
                if (ProjectNeverReportHistory.Count > 0)
                {
                    for (int i = 0; i < ProjCurrentQuater.Count(); ++i)
                    {
                        ProjectNeverReportHistory.Remove(ProjCurrentQuater[i]);
                    }
                }

                if (ReportUnitHistory.Count > 0)
                {
                    for (int i = 0; i < ProjCurrentQuater.Count(); ++i)
                    {
                        ReportUnitHistory.Remove(ProjCurrentQuater[i]);
                    }
                }
            }

            if (FilterProjectId != 0)  //for any specific Project Id
            {
                var CurrentAvailableProject = _ObjUnitWork.Project.Search(x => !ProjectNeverReportHistory.Contains(x.ProjectId) && !ReportUnitHistory.Contains(x.ProjectId) && x.BuilderId == BuilderId &&
                      x.RowStatusId == (int)RowActiveStatus.Active && x.ProjectId == FilterProjectId
                    );
                return CurrentAvailableProject;
            }
            else //for all
            {
                var CurrentAvailableProject = _ObjUnitWork.Project.Search(x => !ProjectNeverReportHistory.Contains(x.ProjectId) && !ReportUnitHistory.Contains(x.ProjectId) && x.BuilderId == BuilderId &&
                    x.RowStatusId == (int)RowActiveStatus.Active
                    );
                return CurrentAvailableProject;
            }


        }

        public int GetBuilderProjectCount(Int64 BuilderId)
        {
            return _ObjUnitWork.Project.Search(x => x.BuilderId == BuilderId).Count();
        }

        public IEnumerable<Project> GetContractBuilderPriviousQuaterProject(Int64 ContractId, Int64 BuilderId, List<Int64> PreviousQuater)
        {
            var ProjectNeverReportHistory = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
                && x.BuilderId == BuilderId && PreviousQuater.Contains(x.QuaterId) && x.ProjectStatusId == (int)EnumProjectStatus.NeverReport
                ).Select(x => x.ProjectId).ToList();

            var ReportUnitHistory = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
                && x.BuilderId == BuilderId && PreviousQuater.Contains(x.QuaterId) && x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit
                ).Select(x => x.ProjectId).ToList();


            var ProjCurrentQuater = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
                    && x.BuilderId == BuilderId && !PreviousQuater.Contains(x.QuaterId)).Select(x => x.ProjectId).ToList();
            if (ProjCurrentQuater.Count > 0)
            {
                if (ProjectNeverReportHistory.Count > 0)
                {
                    for (int i = 0; i < ProjCurrentQuater.Count(); ++i)
                    {
                        ProjectNeverReportHistory.Remove(ProjCurrentQuater[i]);
                    }
                }

                if (ReportUnitHistory.Count > 0)
                {
                    for (int i = 0; i < ProjCurrentQuater.Count(); ++i)
                    {
                        ReportUnitHistory.Remove(ProjCurrentQuater[i]);
                    }
                }
            }

            var CurrentAvailableProject = _ObjUnitWork.Project.Search(x => !ProjectNeverReportHistory.Contains(x.ProjectId) && ReportUnitHistory.Contains(x.ProjectId) && x.BuilderId == BuilderId &&
                x.RowStatusId == (int)RowActiveStatus.Active
                );
            return CurrentAvailableProject;
        }
        //public IEnumerable<Project> GetContractBuilderPriviousQuaterProjectNew(Int64 ContractId, Int64 BuilderId, List<Int64> PreviousQuater)
        //{
        //    var BuilderQuaterContractStatusJoinedWithContractProjectReport = _ObjUnitWork.Project.GetBuilderContractStatusWithContractProjectReport(BuilderId, ContractId).Where(x=> PreviousQuater.Contains(x.QuaterId)).Select(s=>s.Project);

        //    //var ProjectNeverReportHistory = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
        //    //    && x.BuilderId == BuilderId && PreviousQuater.Contains(x.QuaterId) && x.ProjectStatusId == (int)EnumProjectStatus.NeverReport
        //    //    ).Select(x => x.ProjectId).ToList();

        //    //var ReportUnitHistory = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
        //    //    && x.BuilderId == BuilderId && PreviousQuater.Contains(x.QuaterId) && x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit
        //    //    ).Select(x => x.ProjectId).ToList();


        //    //var ProjCurrentQuater = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.ContractId == ContractId
        //    //        && x.BuilderId == BuilderId && !PreviousQuater.Contains(x.QuaterId)).Select(x => x.ProjectId).ToList();
        //    //if (ProjCurrentQuater.Count > 0)
        //    //{
        //    //    if (ProjectNeverReportHistory.Count > 0)
        //    //    {
        //    //        for (int i = 0; i < ProjCurrentQuater.Count(); ++i)
        //    //        {
        //    //            ProjectNeverReportHistory.Remove(ProjCurrentQuater[i]);
        //    //        }
        //    //    }

        //    //    if (ReportUnitHistory.Count > 0)
        //    //    {
        //    //        for (int i = 0; i < ProjCurrentQuater.Count(); ++i)
        //    //        {
        //    //            ReportUnitHistory.Remove(ProjCurrentQuater[i]);
        //    //        }
        //    //    }
        //    //}

        //    //var CurrentAvailableProject = _ObjUnitWork.Project.Search(x => !ProjectNeverReportHistory.Contains(x.ProjectId) && ReportUnitHistory.Contains(x.ProjectId) && x.BuilderId == BuilderId &&
        //    //    x.RowStatusId == (int)RowActiveStatus.Active
        //    //    );
        //    return BuilderQuaterContractStatusJoinedWithContractProjectReport;
        //}
        public IEnumerable<Project> GetSelectedProjectbyProjectList(List<Int64> ProjectList)
        {
            var CurrentAvailableProject = _ObjUnitWork.Project.Search(x => ProjectList.Contains(x.ProjectId));
            return CurrentAvailableProject;
        }
        //public IEnumerable<Project> GetSelectedProjectbyProjectList(Int64 BuilderId, Int64 ContractId)
        //{
        //    var CurrentAvailableProject = _ObjUnitWork.Project.GetBuilderContractStatusWithContractProjectReport(BuilderId, ContractId).Select(s=>s.Project);
        //    return CurrentAvailableProject;
        //}
        #endregion
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
                    Result = cmd.ExecuteNonQuery().ToString();
                }
            }
            return Result;
        }
        #region Created By Rajendar 3/14/2018
        public void CheckProjectReportStatus(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            
            var CurrentQuater = _ObjUnitWork.Quater.Get(QuaterId);
            // -- added on 17-april-2018 - angshuman as executing the LINQ within a loop is taking too much time.
            //IEnumerable<Quater> PreviousQuaterList = null;
            //if (CurrentQuater != null)
            //{
            //    PreviousQuaterList = _ObjUnitWork.Quater.Search(x => x.EndDate < CurrentQuater.StartDate);
            //}

            int ProjListCount = 0; // GetBuilderProject(ContractId, BuilderId, PreviousQuaterList.Select(x => x.QuaterId).ToList()).Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new { ProjectId = x.ProjectId, ProjectName = x.ProjectName }).Count();
            int QuaterContractProjectReportCount = 0; // _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsComplete == true).Count();

            string SQLQuery = "";
            SQLQuery = " Select Count(*) Records ";
            SQLQuery += " From Project ";
            SQLQuery += " Where Builderid = " + BuilderId;
            SQLQuery += " and RowStatusId = " + (int)RowActiveStatus.Active;
            SQLQuery += " and ProjectId not in (Select ProjectId ";
            SQLQuery += "       From BuilderQuaterContractProjectReport ";
            SQLQuery += "       Where BuilderId = " + BuilderId;
            SQLQuery += "       and ContractId = " + ContractId;
            SQLQuery += "       and RowStatusId = 1 ";
            SQLQuery += "       and ProjectStatusId  in ( " + (int)EnumProjectStatus.NeverReport + ", " + (int)EnumProjectStatus.ReportUnit + ") ";
            SQLQuery += "       and QuaterId in (Select QuaterId From Quater Where EndDate < '" + CurrentQuater.StartDate + "') ";
            SQLQuery += "      ) ";

            var ProjListCountData = GetDataIntoListQuery(SQLQuery);
            foreach (var item in ProjListCountData)
            {
                ProjListCount = Convert.ToInt16(item[0].ToString());
            }
            SQLQuery = " Select Count(*) Records ";
            SQLQuery += " From BuilderQuaterContractProjectReport ";
            SQLQuery += " Where Builderid = " + BuilderId;
            SQLQuery += " and RowStatusId = " + (int)RowActiveStatus.Active;
            SQLQuery += " and QuaterId = " + QuaterId;
            SQLQuery += " and ContractId = " + ContractId;
            SQLQuery += " and IsComplete = 'true' ";
            SQLQuery += " and RowStatusId = 1 ";

            var QuaterContractProjectReportCountData = GetDataIntoListQuery(SQLQuery);
            foreach (var item in QuaterContractProjectReportCountData)
            {
                QuaterContractProjectReportCount = Convert.ToInt16(item[0].ToString());
            }
            // -- added on 17-april-2018 - angshuman

            var BuilderQuaterContractStatus = _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId && x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();
            if (BuilderQuaterContractStatus != null)
            {
                if (BuilderQuaterContractStatus.QuarterContractStatusId == (Int64)QuarterContractStatusEnum.ReportForThisQuarter)
                {
                    if (QuaterContractProjectReportCount >= ProjListCount)  // ProjListCount == QuaterContractProjectReportCount - changed on 4/4/2018 - angshuman
                    {
                        var QuaterContractProjectDetails = _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(w => w.BuilderQuaterContractProjectReport.ContractId == ContractId && w.BuilderQuaterContractProjectReport.BuilderId == BuilderId && w.BuilderQuaterContractProjectReport.QuaterId == QuaterId);
                        if (QuaterContractProjectDetails != null && QuaterContractProjectDetails.Count() > 0)
                        {
                            int FileNeedtoUploadCount = QuaterContractProjectDetails.Where(w => w.Question.IsFileNeedtoUpload).Count();
                            int FileNameCount = QuaterContractProjectDetails.Where(w => !string.IsNullOrWhiteSpace(w.FileName)).Count();
                            int AnswerCount = QuaterContractProjectDetails.Where(w => !string.IsNullOrWhiteSpace(w.Answer)).Count();
                            int TotalCount = QuaterContractProjectDetails.Count();
                            if (TotalCount == AnswerCount)
                            {
                                if (FileNameCount == FileNeedtoUploadCount)
                                {
                                    BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed;
                                    BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
                                    _ObjUnitWork.BuilderQuarterContractStatus.Update(BuilderQuaterContractStatus);
                                }
                                else
                                {
                                    BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.InProgress;
                                    BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
                                    _ObjUnitWork.BuilderQuarterContractStatus.Update(BuilderQuaterContractStatus);
                                }
                            }
                            else
                            {
                                BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.InProgress;
                                BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
                                _ObjUnitWork.BuilderQuarterContractStatus.Update(BuilderQuaterContractStatus);
                            }
                        }
                        else
                        {
                            BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed;
                            BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
                            _ObjUnitWork.BuilderQuarterContractStatus.Update(BuilderQuaterContractStatus);
                        }
                    }
                    else
                    {
                        BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.InProgress;
                        BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
                        _ObjUnitWork.BuilderQuarterContractStatus.Update(BuilderQuaterContractStatus);
                    }
                }
                else
                {
                    BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed;
                    BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
                    _ObjUnitWork.BuilderQuarterContractStatus.Update(BuilderQuaterContractStatus);
                }
            }
            //InProgress:           
        }
        // return new EmptyResult();
        #endregion
    }

    public class CustomProjectModel
    {
        public Int64 ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string LotNo { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public Int32 StateId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public Int64 BuilderId { get; set; }
        public string BuilderName { get; set; }
        public Int32 RowStatusId { get; set; }
        public string ReportedContractIds { get; set; }
    }
}

