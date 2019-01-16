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
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public AdminDashboardService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public int GetCountOfNotStartedReportForQuarter(Int64 QuaterId)
        {
            //Quater Q = _ObjUnitWork.Quater.Search(q => q.QuaterId == QuaterId).SingleOrDefault();

            //var S = _ObjUnitWork.Survey.Search(s => s.Quater == Q.QuaterName && s.Year == Q.Year.ToString() && s.IsNcpSurvey == true && s.RowStatusId == (int)RowActiveStatus.Active);
            //var B = _ObjUnitWork.Builder.Search(b => b.RowStatusId == (int)RowActiveStatus.Active);

            ////var allBuilders = _ObjUnitWork.BuilderEmailSent.Search(be => be.IsMailSent == true)
            ////                    .Join(S, x => x.SurveyId, y => y.SurveyId, (x, y) => new { x.BuilderId })
            ////                    .Join(B, m => m.BuilderId, n => n.BuilderId, (m, n) => new { m.BuilderId }).Distinct().Select(bb => bb.BuilderId).ToList();

            //int TotalBuilders = _ObjUnitWork.BuilderEmailSent.Search(be => be.IsMailSent == true)
            //                    .Join(S, x => x.SurveyId, y => y.SurveyId, (x, y) => new { x.BuilderId })
            //                    .Join(B, m => m.BuilderId, n => n.BuilderId, (m, n) => new { m.BuilderId }).Select(cb => cb.BuilderId).Distinct().Count();


            ////int TotalBuilders = _ObjUnitWork.ContractBuilder.Search(cb => cb.RowStatusId == (int)RowActiveStatus.Active).Join(S, x=> x.ContractId, y=> y.ContractId, (x,y) => new { x.BuilderId }).Select(cb => cb.BuilderId).Distinct().Count();
            ////int BuildersWhoReported = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.BuilderId).Distinct().Count();
            //int BuildersWhoReported = _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.BuilderId).Distinct().Count();
            //if (TotalBuilders > 0)
            //{
            //    return (TotalBuilders - BuildersWhoReported);
            //}
            //else
            //{
            //    return (TotalBuilders);
            //}
            return GetCountQuery("Select count(*) from GetBuilderDetailsOfNotStartedBuilders(" + QuaterId + ")");
        }

        public int GetCountOfInProgressReportForQuarter(Int64 QuaterId)
        {
            //int CountOfBuildersWhoSubmittedReport = _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSubmit == true).Count();
            ////int TotalBuildersWhoReported = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.BuilderId).Distinct().Count();
            //int TotalBuildersWhoReported = _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.BuilderId).Distinct().Count();

            //return (TotalBuildersWhoReported - CountOfBuildersWhoSubmittedReport);
            return GetCountQuery("Select count(*) from GetBuilderDetailsOfInProgressBuilders(" + QuaterId + ")");
        }

        public int GetCountOfCompletedReportForQuarter(Int64 QuaterId)
        {
            return GetCountQuery("Select count(*) from GetDetailsOfReportingCompletedBuilders(" + QuaterId + ")");
            //return _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSubmit == true).Count();
        }

        public int GetCountOfReportsSubmittedForQuarterForDay(Int64 QuaterId, DateTime ReportDate)
        {
            Quater Q = _ObjUnitWork.Quater.Search(q => q.QuaterId == QuaterId).SingleOrDefault();

            var S = _ObjUnitWork.Survey.Search(s => s.Quater == Q.QuaterName && s.Year == Q.Year.ToString() && s.IsNcpSurvey == true && s.RowStatusId == (int)RowActiveStatus.Active);
            var B = _ObjUnitWork.Builder.Search(b => b.RowStatusId == (int)RowActiveStatus.Active);

            int TotalBuilders = _ObjUnitWork.BuilderEmailSent.Search(be => be.IsMailSent == true)
                                .Join(S, x => x.SurveyId, y => y.SurveyId, (x, y) => new { x.BuilderId })
                                .Join(B, m => m.BuilderId, n => n.BuilderId, (m, n) => new { m.BuilderId }).Select(cb => cb.BuilderId).Distinct().Count();

            int CountOfBuildersWhoSubmittedReport = _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.QuaterId == QuaterId
                                                    && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSubmit == true
                                                    && DbFunctions.DiffDays(x.SubmitDate, ReportDate) >= 0).Count();

            return (TotalBuilders - CountOfBuildersWhoSubmittedReport);
        }

        public IEnumerable<dynamic> GetBuilderDetailsOfNotStartedBuilders(Int64 QuaterId)
        {
            //Quater Q = _ObjUnitWork.Quater.Search(q => q.QuaterId == QuaterId).SingleOrDefault();

            //var S = _ObjUnitWork.Survey.Search(s => s.Quater == Q.QuaterName && s.Year == Q.Year.ToString() && s.IsNcpSurvey == true && s.RowStatusId == (int)RowActiveStatus.Active);
            //var B = _ObjUnitWork.Builder.Search(b => b.RowStatusId == (int)RowActiveStatus.Active);

            //var AllBuilders = _ObjUnitWork.BuilderEmailSent.Search(be => be.IsMailSent == true)
            //                    .Join(S, x => x.SurveyId, y => y.SurveyId, (x, y) => new { x.BuilderId,x.Builder })
            //                    .Join(B, m => m.BuilderId, n => n.BuilderId, (m, n) => new { m.BuilderId,m.Builder });

            //var BuildersWhoReported = _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active).Select(x =>  x.BuilderId ).Distinct();
            var BuilderNotStarted = GetDataIntoListQuery("Select * from GetBuilderDetailsOfNotStartedBuilders(" + QuaterId + ")");
            //var BuilderNotStarted = AllBuilders.Where(w => !BuildersWhoReported.Contains(w.BuilderId));
            //var BuilderQuaterAdminReport = _ObjUnitWork.BuilderQuaterAdminReport.GetAll().Where(w=> BuilderNotStarted.Select(s => s.BuilderId).Contains(w.BuilderId)).Select(s => new { s.BuilderId,s.SubmitDate, s.Quater }).OrderByDescending(o => o.SubmitDate);
            var Result = BuilderNotStarted.Select(row => new
            {
                BuilderId = Convert.ToInt64(row["BuilderID"]),
                BuilderName = Convert.ToString(row["BuilderName"]),
                QuaterId = Convert.ToInt64(row["QuaterId"]),
                MarketId = Convert.ToInt64(row["MarketId"]),
                MarketName = Convert.ToString(row["MarketName"]),
                LastActivityDate = Convert.ToString(row["LastActivityDate"]),
                TotalContract = Convert.ToString(row["TotalContract"]),
                ActualContract = Convert.ToString(row["ActualContract"]),

            }).Select(ab => new DashboardBuilderListRepository
            {
                BuilderId = ab.BuilderId,
                BuilderName = ab.BuilderName,
                MarketId = ab.MarketId,
                MarketName = ab.MarketName,

                ContractStatus = String.Concat(ab.ActualContract.ToString(), " of ", ab.TotalContract.ToString()),
                LastActivityDate = ab.LastActivityDate
            });
            return (Result);
        }

        public IEnumerable<dynamic> GetBuilderDetailsOfInProgressBuilders(Int64 QuaterId)
        {
            //var BuildersWhoSubmittedReport = _ObjUnitWork.BuilderQuaterAdminReport
            //                                .Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSubmit == true)
            //                                .Select(bws => bws.BuilderId).Distinct();

            //var BuildersWhoReported = _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active).Select(s=> new { s.BuilderId,s.Builder,s.ModifiedOn}).OrderByDescending(o=>o.ModifiedOn).Distinct();
            var BuilderInProgress = GetDataIntoListQuery("Select * from GetBuilderDetailsOfInProgressBuilders(" + QuaterId + ")");
            //var BuilderInProgress = BuildersWhoReported.Where(w => !BuildersWhoSubmittedReport.Contains(w.BuilderId));

            var Result = BuilderInProgress.Select(row => new
            {
                BuilderId = Convert.ToInt64(row["BuilderID"]),
                BuilderName = Convert.ToString(row["BuilderName"]),
                QuaterId = Convert.ToInt64(row["QuaterId"]),
                MarketId = Convert.ToInt64(row["MarketId"]),
                MarketName = Convert.ToString(row["MarketName"]),
                LastActivityDate = Convert.ToString(row["LastActivityDate"]),
                TotalContract = Convert.ToString(row["TotalContract"]),
                ActualContract = Convert.ToString(row["ActualContract"]),

            }).Select(ab => new DashboardBuilderListRepository
            {
                BuilderId = ab.BuilderId,
                BuilderName = ab.BuilderName,
                MarketId = ab.MarketId,
                MarketName = ab.MarketName,

                ContractStatus = String.Concat(ab.ActualContract.ToString(), " of ", ab.TotalContract.ToString()),
                LastActivityDate = ab.LastActivityDate
            });

            return (Result);
        }

        public IEnumerable<dynamic> GetBuilderDetailsOfReportSubmittedBuilders(Int64 QuaterId)
        {

            //var BuildersWhoSubmittedReport = _ObjUnitWork.BuilderQuaterAdminReport
            //                                .Search(x => x.QuaterId == QuaterId && x.RowStatusId == (int)RowActiveStatus.Active && x.IsSubmit == true)
            //                               .Select(s=>new { s.BuilderId,s.Builder,s.QuaterId,s.SubmitDate}).Distinct();
            var BuildersWhoSubmittedReport = GetDataIntoListQuery("Select * from GetDetailsOfReportingCompletedBuilders(" + QuaterId + ")");
            var Result = BuildersWhoSubmittedReport.Select(row => new
            {
                BuilderId = Convert.ToInt64(row["BuilderID"]),
                BuilderName = Convert.ToString(row["BuilderName"]),
                QuaterId = Convert.ToInt64(row["QuaterId"]),
                MarketId = Convert.ToInt64(row["MarketId"]),
                MarketName = Convert.ToString(row["MarketName"]),
                LastActivityDate = Convert.ToString(row["LastActivityDate"]),
                TotalContract = Convert.ToString(row["TotalContract"]),
                ActualContract = Convert.ToString(row["ActualContract"]),

            }).Select(ab =>
                                                            new DashboardBuilderListRepository
                                                            {
                                                                BuilderId = ab.BuilderId,
                                                                BuilderName = ab.BuilderName,
                                                                QuaterId = ab.QuaterId,
                                                                MarketId = ab.MarketId,
                                                                MarketName = ab.MarketName,
                                                                //ContractStatus = GetContractStatusForBuilder(ab.BuilderId, QuaterId, "Report Submitted"),
                                                                //LastActivityDate = ab.SubmitDate.ToShortDateString()
                                                                ContractStatus = String.Concat(ab.ActualContract.ToString(), " of ", ab.TotalContract.ToString()),
                                                                LastActivityDate = ab.LastActivityDate
                                                            });


            return (Result);
        }

        private string GetContractStatusForBuilder(Int64 BuilderId, Int64 QuarterId, string ReportingStatus)
        {
            string ContractStatus = "";
            int total = GetTotalContractCountForBuilder(BuilderId, QuarterId);
            switch (ReportingStatus)
            {
                case "Not Started":
                    ContractStatus = String.Concat("0 of ", total.ToString());
                    break;
                case "In Progress":
                    var BuildersWhoReported = _ObjUnitWork.BuilderQuarterContractStatus.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuarterId && x.RowStatusId == (int)RowActiveStatus.Active).Count();
                    ContractStatus = String.Concat(BuildersWhoReported.ToString(), " of ", total.ToString());
                    break;
                case "Report Submitted":
                    ContractStatus = String.Concat(total.ToString(), " of ", total.ToString());
                    break;
            }


            return ContractStatus;
        }

        private int GetTotalContractCountForBuilder(Int64 BuilderId, Int64 QuarterId)
        {
            Quater Q = _ObjUnitWork.Quater.Get(QuarterId);
            int TotalContractCount = _ObjUnitWork.ContractBuilder.Search(cb => cb.BuilderId == BuilderId && cb.RowStatusId == (int)RowActiveStatus.Active)
                                    .Where(cb => cb.JoiningDate < Q.ReportingStartDate).Count();

            return TotalContractCount;
        }

        private int GetReportedContractCountForBuilder(Int64 BuilderId, Int64 QuarterId)
        {
            int TotalProjectsOfBuilder = _ObjUnitWork.Project.Search(pr => pr.BuilderId == BuilderId && pr.RowStatusId == (int)RowActiveStatus.Active).Count();

            Quater Q = _ObjUnitWork.Quater.Get(QuarterId);

            IEnumerable<ContractBuilder> ActiveContractsOfBuilder = _ObjUnitWork.ContractBuilder
                                                                    .Search(cb => cb.BuilderId == BuilderId && cb.RowStatusId == (int)RowActiveStatus.Active)
                                                                    .Where(cb => cb.JoiningDate < Q.ReportingStartDate);

            IEnumerable<Project> ActiveProjectsOfBuilder = _ObjUnitWork.Project.Search(pr => pr.BuilderId == BuilderId && pr.RowStatusId == (int)RowActiveStatus.Active);

            int ReportedContractCount = 0;

            if (ActiveContractsOfBuilder.Count() > 0)
            {
                foreach (var BuilderContract in ActiveContractsOfBuilder)
                {
                    int NeverReportProjectCount = _ObjUnitWork.BuilderQuaterContractProjectReport
                                                  .Search(bqcpr => bqcpr.BuilderId == BuilderId && bqcpr.ContractId == BuilderContract.ContractId && bqcpr.ProjectStatusId == (int)EnumProjectStatus.NeverReport)
                                                  .Join(ActiveProjectsOfBuilder, x => x.ProjectId, y => y.ProjectId, (x, y) => new { x.BuilderQuaterAdminReportId }).Count();

                    int PreviouslyReportedProjectCount = _ObjUnitWork.BuilderQuaterContractProjectReport
                                                        .Search(bqcpr => bqcpr.BuilderId == BuilderId && bqcpr.ContractId == BuilderContract.ContractId && bqcpr.QuaterId != QuarterId && bqcpr.ProjectStatusId == (int)EnumProjectStatus.ReportUnit)
                                                        .Join(ActiveProjectsOfBuilder, x => x.ProjectId, y => y.ProjectId, (x, y) => new { x.BuilderQuaterAdminReportId }).Count();

                    int NoReportCurrQuarterProjectCount = _ObjUnitWork.BuilderQuaterContractProjectReport
                                                         .Search(bqcpr => bqcpr.BuilderId == BuilderId && bqcpr.ContractId == BuilderContract.ContractId && bqcpr.QuaterId == QuarterId && bqcpr.ProjectStatusId == (int)EnumProjectStatus.NothingtoReport)
                                                         .Join(ActiveProjectsOfBuilder, x => x.ProjectId, y => y.ProjectId, (x, y) => new { x.BuilderQuaterAdminReportId }).Count();

                    int ReportUnitCurrQuarterProjectCount = _ObjUnitWork.BuilderQuaterContractProjectReport
                                                           .Search(bqcpr => bqcpr.BuilderId == BuilderId && bqcpr.ContractId == BuilderContract.ContractId && bqcpr.QuaterId == QuarterId && bqcpr.ProjectStatusId == (int)EnumProjectStatus.ReportUnit && bqcpr.IsComplete == true)
                                                           .Join(ActiveProjectsOfBuilder, x => x.ProjectId, y => y.ProjectId, (x, y) => new { x.BuilderQuaterAdminReportId }).Count();

                    int Total = NeverReportProjectCount + PreviouslyReportedProjectCount + NoReportCurrQuarterProjectCount + ReportUnitCurrQuarterProjectCount;

                    if ((Total == TotalProjectsOfBuilder) && (TotalProjectsOfBuilder != PreviouslyReportedProjectCount))
                    {
                        ReportedContractCount++;
                    }
                }
            }

            return ReportedContractCount;
        }

        private string Stringify(object Value)
        {
            if (Value == null)
            {
                return "-";
            }
            else
            {
                return Value.ToString();
            }
        }

        public IEnumerable<dynamic> GetBuilderDetailsOfAllParticipatingBuilders(Int64 QuaterId)
        {
            Quater Q = _ObjUnitWork.Quater.Search(q => q.QuaterId == QuaterId).SingleOrDefault();

            var S = _ObjUnitWork.Survey.Search(s => s.Quater == Q.QuaterName && s.Year == Q.Year.ToString() && s.IsNcpSurvey == true && s.RowStatusId == (int)RowActiveStatus.Active);
            var B = _ObjUnitWork.Builder.Search(b => b.RowStatusId == (int)RowActiveStatus.Active);

            var AllBuilders = _ObjUnitWork.BuilderEmailSent.Search(be => be.IsMailSent == true)
                                .Join(S, x => x.SurveyId, y => y.SurveyId, (x, y) => new { x.BuilderId })
                                .Join(B, m => m.BuilderId, n => n.BuilderId, (m, n) => new { m.BuilderId }).Distinct();

            var BQCPR = _ObjUnitWork.BuilderQuaterContractProjectReport
                        .Search(bqcpr => bqcpr.QuaterId == QuaterId && bqcpr.RowStatusId == (int)RowActiveStatus.Active)
                        .Join(AllBuilders, bqcpr => bqcpr.BuilderId, ab => ab.BuilderId,
                                (bqcpr, ab) => new DashboardBuilderContractProjectListRepository
                                {
                                    BuilderId = ab.BuilderId,
                                    BuilderName = bqcpr.Builder.BuilderName,
                                    MarketId = bqcpr.Builder.MarketId,
                                    MarketName = bqcpr.Builder.Market.MarketName,
                                    ContractId = bqcpr.ContractId,
                                    ContractName = bqcpr.Contract.ContractName,
                                    VendorName = bqcpr.Contract.Manufacturer.ManufacturerName,
                                    ContractEnrolledDate = _ObjUnitWork.ContractBuilder.Search(cb => cb.ContractId == bqcpr.ContractId && cb.BuilderId == bqcpr.BuilderId && cb.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault().JoiningDate.ToShortDateString(),
                                    TotalProjects = _ObjUnitWork.Project.Search(pr => pr.BuilderId == bqcpr.BuilderId && pr.RowStatusId == (int)RowActiveStatus.Active).Count(),
                                    ReportedProjects = _ObjUnitWork.BuilderQuaterContractProjectReport.Search(bqcpr1 => bqcpr1.BuilderId == bqcpr.BuilderId && bqcpr1.QuaterId == QuaterId && bqcpr1.ContractId == bqcpr.ContractId && bqcpr1.IsComplete == true).Select(bqcpr1 => bqcpr1.ProjectId).Distinct().Count(),
                                    ProjectId = bqcpr.ProjectId,
                                    ProjectName = bqcpr.Project.ProjectName,
                                    LotNo = bqcpr.Project.LotNo,
                                    Address = bqcpr.Project.Address,
                                    City = bqcpr.Project.City,
                                    State = bqcpr.Project.State,
                                    Zip = bqcpr.Project.Zip,
                                    ProjectCreatedOn = bqcpr.Project.CreatedOn.ToShortDateString(),
                                    ProjectStatus = GetProjectStatus(QuaterId, bqcpr.ContractId, bqcpr.ProjectId)
                                });

            return (BQCPR);
        }

        private int GetProjectStatus(Int64 QuarterId, Int64 ContractId, Int64 ProjectId)
        {
            BuilderQuaterContractProjectReport ProjectStatus = _ObjUnitWork.BuilderQuaterContractProjectReport
                                                                           .Search(bqcpr => bqcpr.ProjectId == ProjectId && bqcpr.ContractId == ContractId
                                                                           && bqcpr.QuaterId == QuarterId && bqcpr.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

            if (ProjectStatus == null)
            {
                return 0;
            }
            else if (ProjectStatus.ProjectStatusId == (int)EnumProjectStatus.NeverReport || ProjectStatus.ProjectStatusId == (int)EnumProjectStatus.NothingtoReport)
            {
                return 3;
            }
            else if (ProjectStatus.ProjectStatusId == (int)EnumProjectStatus.ReportUnit && ProjectStatus.IsComplete == false)
            {
                return 2;
            }
            else if (ProjectStatus.ProjectStatusId == (int)EnumProjectStatus.ReportUnit && ProjectStatus.IsComplete == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public IEnumerable<dynamic> GetDetailsOfAllBuilderContractsForCurrentQuarter(Int64 QuaterId)
        {
            var BuildersReport = GetDataIntoListQuery("Select * from GetBuilderDetailsOfAllParticipatingBuilders(" + QuaterId + ")");
            var Result = BuildersReport.Select(row => new
            {
                BuilderId = Convert.ToInt64(row["BuilderID"]),
                BuilderName = Convert.ToString(row["BuilderName"]),
                MarketId = Convert.ToInt64(row["MarketId"]),
                MarketName = Convert.ToString(row["MarketName"]),
                ManufacturerName = Convert.ToString(row["ManufacturerName"]),
                ContractId = Convert.ToInt64(row["ContractId"]),
                ContractName = Convert.ToString(row["ContractName"]),
                Status = Convert.ToInt32(row["Status"]),
                TotalProject = Convert.ToInt64(row["TotalProject"]),
                ReportProject = Convert.ToInt64(row["ReportProject"]),
                ContractEnrolledDate = Convert.ToString(row["ContractEnrolledDate"]),

            }).Select(m => new DashboardBuilderContractProjectListRepository
            {

                BuilderId = m.BuilderId,
                ContractId = m.ContractId,

                BuilderName = m.BuilderName,
                ContractName = m.ContractName,
                VendorName = m.ManufacturerName,
                MarketId = m.MarketId,
                MarketName = m.MarketName,
                ReportedProjects = m.ReportProject,
                TotalProjects = m.TotalProject,
                ContractEnrolledDate = m.ContractEnrolledDate,
                ListReport = m.Status,
            }).Distinct();

            //Quater Q = _ObjUnitWork.Quater.Search(q => q.QuaterId == QuaterId).SingleOrDefault();

            //var S = _ObjUnitWork.Survey.Search(s => s.Quater == Q.QuaterName && s.Year == Q.Year.ToString() && s.IsNcpSurvey == true && s.RowStatusId == (int)RowActiveStatus.Active);
            //var B = _ObjUnitWork.Builder.Search(b => b.RowStatusId == (int)RowActiveStatus.Active);

            //var AllBuilders = _ObjUnitWork.BuilderEmailSent.Search(be => be.IsMailSent == true)
            //                    .Join(S, x => x.SurveyId, y => y.SurveyId, (x, y) => new { x.BuilderId })
            //                    .Join(B, m => m.BuilderId, n => n.BuilderId, (m, n) => new { m.BuilderId }).Distinct();
            //var GroupedProject = _ObjUnitWork.Project.Search(pr => pr.RowStatusId == (int)RowActiveStatus.Active).GroupBy(info => info.BuilderId).Select(group => new { BuilderId = group.Key, ReportedTotalProjectCount = group.Count() }).OrderBy(x => x.BuilderId);
            //var ContractEnrolledDate = _ObjUnitWork.ContractBuilder.Search(cb => cb.RowStatusId == (int)RowActiveStatus.Active).Select(s => new { BuilderId = s.BuilderId, ContractId = s.ContractId,Contract=s.Contract, JoiningDate = s.JoiningDate.ToShortDateString() });
            //var BQCPR = _ObjUnitWork.BuilderQuarterContractStatus
            //            .Search(bqcpr => bqcpr.QuaterId == QuaterId && bqcpr.ContractId > 0 && bqcpr.RowStatusId == (int)RowActiveStatus.Active)
            //            .Join(AllBuilders, bqcpr => bqcpr.BuilderId, ab => ab.BuilderId,
            //                   (bqcpr, ab) => new

            //                   {
            //                       BuilderId = bqcpr.BuilderId,
            //                       BuilderName = bqcpr.Builder != null ? bqcpr.Builder.BuilderName : "",
            //                       MarketId = bqcpr.Builder != null ? bqcpr.Builder.MarketId : 0,
            //                       MarketName = bqcpr.Builder != null ? (bqcpr.Builder.Market != null ? bqcpr.Builder.Market.MarketName : "") : "",
            //                       ContractId = bqcpr.ContractId,
            //                       ContractName = bqcpr.Contract != null ? bqcpr.Contract.ContractName : "",
            //                       QuaterId = bqcpr.QuaterId,
            //                       IsCom = bqcpr.ProjectReportStatusId,
            //                       VendorName = bqcpr.Contract != null ? (bqcpr.Contract.Manufacturer != null ? bqcpr.Contract.Manufacturer.ManufacturerName : "") : "",

            //                   }).Distinct();



            //var GroupBQCPR = BQCPR.Where(w => w.IsCom == (Int64)ProjectReportStatusEnum.Completed).GroupBy(info => new { info.BuilderId, info.ContractId, info.QuaterId })
            //            .Select(group => new
            //            {
            //                BuilderId = group.Key.BuilderId,
            //                ContractId = group.Key.ContractId,
            //                QuaterId = group.Key.QuaterId,
            //                ReportedProjectCount = group.Count()
            //            })
            //            .OrderBy(x => x.BuilderId);
            //var JoinedBQCPR = BQCPR
            //                 .Join(GroupBQCPR, bqcpr1 => new { bqcpr1.BuilderId, bqcpr1.ContractId, bqcpr1.QuaterId }, gbqcpr => new { gbqcpr.BuilderId, gbqcpr.ContractId, gbqcpr.QuaterId }, (bqcpr1, gbqcpr) => new
            //                 {
            //                     bqcpr1.BuilderId,
            //                     bqcpr1.ContractId,
            //                     bqcpr1.QuaterId,
            //                     bqcpr1.BuilderName,
            //                     bqcpr1.ContractName,
            //                     bqcpr1.VendorName,
            //                     bqcpr1.MarketId,
            //                     bqcpr1.MarketName,
            //                     gbqcpr.ReportedProjectCount,

            //                 })
            //                 .Join(GroupedProject, bqcpr => bqcpr.BuilderId, gp => gp.BuilderId, (bqcpr, gp) =>
            //                 new
            //                 {
            //                     bqcpr.BuilderId,
            //                     bqcpr.ContractId,
            //                     bqcpr.QuaterId,
            //                     bqcpr.BuilderName,
            //                     bqcpr.ContractName,
            //                     bqcpr.VendorName,
            //                     bqcpr.MarketId,
            //                     bqcpr.MarketName,
            //                     bqcpr.ReportedProjectCount,
            //                     gp.ReportedTotalProjectCount
            //                 }
            //                 ).Join(ContractEnrolledDate, m => new { m.BuilderId, m.ContractId }, n => new { n.BuilderId, n.ContractId }, (m, n) => new DashboardBuilderContractProjectListRepository
            //                 {

            //                     BuilderId = m.BuilderId,
            //                     ContractId = m.ContractId,
            //                     QuaterId = m.QuaterId,
            //                     BuilderName = m.BuilderName,
            //                     ContractName = m.ContractName,
            //                     VendorName = m.VendorName,
            //                     MarketId = m.MarketId,
            //                     MarketName = m.MarketName,
            //                     ReportedProjects = m.ReportedProjectCount,
            //                     TotalProjects = m.ReportedTotalProjectCount,
            //                     ContractEnrolledDate = n.JoiningDate,
            //                     ListReport = Convert.ToDouble(m.ReportedProjectCount) / Convert.ToDouble(m.ReportedTotalProjectCount),
            //                 }).Distinct();



            return (Result);
        }
        public IEnumerable<dynamic> GetDetailsOfAllProjectByBuilderContractsForCurrentQuarter(Int64 QuaterId, Int64 BuilderId, Int64 ContractId)
        {


            var Result = (from ProjectList in _ObjUnitWork.Project.Search(s => s.BuilderId == BuilderId && s.RowStatusId == (int)RowActiveStatus.Active)
                         join BQCPR in _ObjUnitWork.BuilderQuaterContractProjectReport
                                 .Search(bqcpr => bqcpr.QuaterId == QuaterId && bqcpr.BuilderId == BuilderId && bqcpr.ProjectId > 0 && bqcpr.ContractId == ContractId && bqcpr.RowStatusId == (int)RowActiveStatus.Active)
                                 on
                                 new { ProjectList.BuilderId, ProjectList.ProjectId } equals new { BQCPR.BuilderId, BQCPR.ProjectId } into BQCPRList
                                 from ab in BQCPRList.DefaultIfEmpty(new BuilderQuaterContractProjectReport())
                                 select new DashboardBuilderContractProjectListRepository
                                 {
                                     BuilderId = ProjectList.BuilderId,

                                     ContractId = ContractId,

                                     ProjectId = ProjectList.ProjectId,
                                     ProjectName = ProjectList.ProjectName,
                                     LotNo = ProjectList.LotNo,
                                     Address = ProjectList.Address,
                                     City = ProjectList.City,
                                     State = ProjectList.State,
                                     Zip = ProjectList.Zip,
                                     ProjectCreatedOn = ProjectList.CreatedOn.ToShortDateString(),
                                     ProjectStatus = GetProjectStatus(QuaterId, ContractId, ProjectList.ProjectId)
                                 }).Distinct();

            return (Result);
        }
        public IEnumerable<dynamic> GetDetailsOfAllProjectByBuilderContractsForCurrentQuarter(Int64 QuaterId)
        {


            var BQCPR = _ObjUnitWork.BuilderQuaterContractProjectReport
                        .Search(bqcpr => bqcpr.QuaterId == QuaterId && bqcpr.RowStatusId == (int)RowActiveStatus.Active)
                        .Select(bqcpr => new DashboardBuilderContractProjectListRepository
                        {
                            BuilderId = bqcpr.BuilderId,
                            ContractId = bqcpr.ContractId,
                            ProjectId = bqcpr.ProjectId,
                            ProjectName = bqcpr.Project != null ? bqcpr.Project.ProjectName : "",
                            LotNo = bqcpr.Project != null ? bqcpr.Project.LotNo : "",
                            Address = bqcpr.Project != null ? bqcpr.Project.Address : "",
                            City = bqcpr.Project != null ? bqcpr.Project.City : "",
                            State = bqcpr.Project != null ? bqcpr.Project.State : "",
                            Zip = bqcpr.Project != null ? bqcpr.Project.State : "",
                            ProjectCreatedOn = bqcpr.Project.CreatedOn.ToShortDateString(),
                            ProjectStatus = GetProjectStatus(QuaterId, bqcpr.ContractId, bqcpr.ProjectId)
                        }).Distinct();

            return (BQCPR);
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
            return dt.AsEnumerable();
            //    .Select(row => new 
            //{
            //    BuilderId = Convert.ToInt64(row["BuilderID"]),
            //    BuilderName = Convert.ToString(row["BuilderName"]),
            //    QuaterId = Convert.ToInt64(row["QuaterId"]),
            //    MarketId = Convert.ToInt64(row["MarketId"]),
            //    MarketName = Convert.ToString(row["MarketName"]),
            //    LastActivityDate = Convert.ToString(row["LastActivityDate"]),
            //    TotalContract = Convert.ToString(row["TotalContract"]),
            //    ActualContract = Convert.ToString(row["ActualContract"]),

            //}); 
        }
        public int GetCountQuery(string query)
        {
            //DataTable dt = new DataTable();
            int count = 0;
            using (CBUSADbContext db = new CBUSADbContext())
            {
                string connString = db.Database.Connection.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();
                    }
                    else
                    {
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();
                    }

                }
            }
            return count;
        }
    }
}