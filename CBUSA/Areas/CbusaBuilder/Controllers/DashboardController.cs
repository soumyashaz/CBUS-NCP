using CBUSA.Domain;
using CBUSA.Models;
using CBUSA.Services;
using CBUSA.Services.Interface;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace CBUSA.Areas.CbusaBuilder.Controllers
{
    public class DashboardController : Controller
    {
        readonly IQuaterService _ObjQuaterService;
        readonly IContractServices _ObjContractService;
        readonly IBuilderService _ObjBuilderService;
        readonly IContractBuilderService _ObjContractBuilderService;
        readonly IProjectService _ObjProjectService;
        readonly IBuilderQuaterAdminReportService _ObjQuaterContractAdminReport;
        readonly IBuilderQuaterContractProjectReportService _ObjQuaterContractProjectReport;
        readonly IBuilderQuaterContractProjectDetailsService _ObjBuilderQuaterContractProjectDetails;
        readonly IBuilderQuarterContractStatusService _ObjBuilderQuarterContractStatus;

        public DashboardController(IQuaterService ObjQuaterService, IContractServices ObjContractService, 
                                   IBuilderService ObjBuilderService, IContractBuilderService ObjContractBuilderService,
                                   IProjectService ObjProjectService,
                                   IBuilderQuaterAdminReportService ObjQuaterContractAdminReport,
                                   IBuilderQuaterContractProjectReportService ObjQuaterContractProjectReport, 
                                   IBuilderQuaterContractProjectDetailsService ObjBuilderQuaterContractProjectDetails,
                                   IBuilderQuarterContractStatusService ObjBuilderQuarterContractStatus)
        {
            _ObjQuaterService = ObjQuaterService;
            _ObjContractService = ObjContractService;
            _ObjBuilderService = ObjBuilderService;
            _ObjContractBuilderService = ObjContractBuilderService;
            _ObjProjectService = ObjProjectService;
            _ObjQuaterContractAdminReport = ObjQuaterContractAdminReport;
            _ObjBuilderQuaterContractProjectDetails = ObjBuilderQuaterContractProjectDetails;
            _ObjQuaterContractProjectReport = ObjQuaterContractProjectReport;
            _ObjBuilderQuarterContractStatus = ObjBuilderQuarterContractStatus;
        }

        // GET: CbusaBuilder/Dashboard
        public ActionResult Index(long bldruserid)
        {
            bool IsBuilderAuthenticated = false;
            long BuilderId = 0;

            //-------------------- IDENTITY ------------------------
            var User = _ObjBuilderService.IsUserAuthenticate(bldruserid);

            if (User != null)
            {
                var Builder = User.Builder;

                if (Builder.RowStatusId == (int)RowActiveStatus.Active)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Sid, User.BuilderId.ToString()));
                    claims.Add(new Claim(ClaimTypes.PrimarySid, User.BuilderUserId.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, User.FirstName + " " + User.LastName));
                    claims.Add(new Claim(ClaimTypes.Email, User.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Builder"));

                    var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                    var ctx = Request.GetOwinContext();
                    var authenticationManager = ctx.Authentication;
                    authenticationManager.SignIn(id);

                    IsBuilderAuthenticated = true;

                    BuilderId = Builder.BuilderId;

                    ViewBag.BuilderId = BuilderId;
                }
            }
            //---------------------------------------------------

            if (IsBuilderAuthenticated)
            {
                ViewBag.BuilderId = BuilderId;

                PopulateContractLogos(BuilderId);

                int CountOfActiveContractsCanJoin = _ObjContractService.GetNonAssociateContractWithBuilder(BuilderId, "act").Count();
                int CountOfPendingContractsCanJoin = _ObjContractService.GetNonAssociateContractWithBuilder(BuilderId, "pen").Count();

                ViewBag.CountOfContractsCanJoin = (CountOfActiveContractsCanJoin + CountOfPendingContractsCanJoin);

                DateTime currentdate = DateTime.Now.Date;
                bool ReportingPeriodOn = false;
                ReportingPeriodOn = _ObjQuaterService.IsReportingPeriodOn(currentdate);
                ViewBag.IsReportingPeriodOn = ReportingPeriodOn;

                if (ReportingPeriodOn)
                {
                    //---------------------- Reporting End Date ----------------------
                    Quater CurrentQuarter = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
                    ViewBag.QuaterId = CurrentQuarter.QuaterId;
                    ViewBag.ReportingQuarter = CurrentQuarter.QuaterName;
                    ViewBag.ReportingYear = CurrentQuarter.Year;

                    System.Globalization.DateTimeFormatInfo objDateTimeFormat = new System.Globalization.DateTimeFormatInfo();
                    ViewBag.ReportingEndMonth = objDateTimeFormat.GetAbbreviatedMonthName(CurrentQuarter.BuilderReportingEndDate.Month);
                    ViewBag.ReportingEndDay = CurrentQuarter.BuilderReportingEndDate.Day;

                    //---------------------- Contract Reporting Statuses ----------------------
                    bool IsReportSubmitted = false;
                    IsReportSubmitted = _ObjQuaterContractAdminReport.IsReportAllreadySubmited(BuilderId, CurrentQuarter.QuaterId);
                    ViewBag.IsReportSubmitted = IsReportSubmitted.ToString();

                    IEnumerable<BuilderQuarterContractStatus> BQCSList = _ObjBuilderQuarterContractStatus.CheckExistingBuilderRecord(BuilderId).Where(bqcs => bqcs.QuaterId == CurrentQuarter.QuaterId);
                    int ContractReportCompleted = 0;
                    int ContractReportInProgress = 0;
                    int ContractReportNotStarted = 0;

                    foreach (BuilderQuarterContractStatus objBQCS in BQCSList)
                    {
                        if (objBQCS.QuarterContractStatusId == (int)QuarterContractStatusEnum.ReportForThisQuarter)
                        {
                            if (objBQCS.ProjectReportStatusId == (int)ProjectReportStatusEnum.Completed)
                            {
                                ContractReportCompleted++;
                            }
                            else if (objBQCS.ProjectReportStatusId == (int)ProjectReportStatusEnum.InProgress)
                            {
                                ContractReportInProgress++;
                            }
                        }
                        else if (objBQCS.QuarterContractStatusId == (int)QuarterContractStatusEnum.NothingToReportThisQuarter || objBQCS.QuarterContractStatusId == (int)QuarterContractStatusEnum.NeverReportForThisContract)
                        {
                            ContractReportCompleted++;
                        }
                    }

                    int AllActiveContractsCount = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, CurrentQuarter.QuaterId).Count();
                    ContractReportNotStarted = AllActiveContractsCount - (ContractReportCompleted + ContractReportInProgress);

                    ViewBag.ContractReportCompleted = ContractReportCompleted;
                    ViewBag.ContractReportInProgress = ContractReportInProgress;
                    ViewBag.ContractReportNotStarted = ContractReportNotStarted;

                    //---------------------- Project Archiving Possibilities ----------------------
                    var AllActiveProjects = _ObjProjectService.GetBuilderActiveProject(BuilderId);
                    var ReportedProjectIdList = _ObjQuaterContractProjectReport.GetRepotDetailsofAllContract(BuilderId)
                                                .Where(bqcpr => bqcpr.QuaterId != CurrentQuarter.QuaterId
                                                && bqcpr.Project.RowStatusId == (int)RowActiveStatus.Active
                                                && (bqcpr.ProjectStatusId == (int)EnumProjectStatus.ReportUnit || bqcpr.ProjectStatusId == (int)EnumProjectStatus.NeverReport))
                                                .Select(bqcpr => bqcpr.ProjectId).Distinct();

                    var AllReportableContracts = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, CurrentQuarter.QuaterId)
                                                 .OrderBy(con => con.ContractId)
                                                 .Select(con => con.ContractId).Distinct().ToList();

                    int ProjectsNotEligibleForReporting = 0;
                    foreach (int ProjectId in ReportedProjectIdList)
                    {
                        var AllContractsReportedAgainst = _ObjQuaterContractProjectReport.GetLatestContractAgainstBuilderProject(BuilderId, ProjectId)
                                                          .Where(bqcpr => bqcpr.QuaterId != CurrentQuarter.QuaterId
                                                          && (bqcpr.ProjectStatusId == (int)EnumProjectStatus.ReportUnit || bqcpr.ProjectStatusId == (int)EnumProjectStatus.NeverReport))
                                                          .OrderBy(bqcpr => bqcpr.ContractId)
                                                          .Select(bqcpr => bqcpr.ContractId).Distinct().ToList();

                        bool ProjectReportedAgainstAllContracts = false;
                        ProjectReportedAgainstAllContracts = AllReportableContracts.SequenceEqual(AllContractsReportedAgainst);

                        if (ProjectReportedAgainstAllContracts)
                            ProjectsNotEligibleForReporting++;
                    }

                    int ProjectsCreated9MonthsAgo = _ObjProjectService.GetBuilderActiveProject(BuilderId).Where(p => (currentdate.Subtract(p.CreatedOn)).Days >= 270).Count();

                    long LastReportingQuarterId = _ObjQuaterService.GetAllPreviousQuater(CurrentQuarter.QuaterId).Reverse().FirstOrDefault().QuaterId;
                    long SecondLastReportingQuarterId = _ObjQuaterService.GetAllPreviousQuater(LastReportingQuarterId).Reverse().FirstOrDefault().QuaterId;

                    var ProjectsReportedInLastQuarter = _ObjQuaterContractProjectReport.CheckExistAllContractAgainstBuilderQuater(BuilderId, LastReportingQuarterId).Select(p => p.ProjectId);
                    var ProjectsReportedInSecondLastQuarter = _ObjQuaterContractProjectReport.CheckExistAllContractAgainstBuilderQuater(BuilderId, SecondLastReportingQuarterId).Select(p => p.ProjectId);

                    int ProjectsNotReportedIn6Months = _ObjProjectService.GetBuilderActiveProject(BuilderId).Where(p => (currentdate.Subtract(p.CreatedOn)).Days >= 180
                                                       && !ProjectsReportedInLastQuarter.Contains(p.ProjectId) && !ProjectsReportedInSecondLastQuarter.Contains(p.ProjectId)).Count();

                    ViewBag.ProjectsNotEligibleForReporting = ProjectsNotEligibleForReporting;
                    ViewBag.ProjectsCreated9MonthsAgo = ProjectsCreated9MonthsAgo;
                    ViewBag.ProjectsNotReportedIn6Months = ProjectsNotReportedIn6Months;
                }
            }
            else
            {
                //-------------------- FIX For NOT NULL Error -----------------
                ViewBag.BuilderId = 0;
                ViewBag.IsReportingPeriodOn = false;
                ViewBag.TotalContracts = 0;
                ViewBag.TotalPages = 0;
                //--------------------------------------------------------------
            }

            return View();
        }

        public List<string> PopulateContractLogos(long BuilderId)
        {
            List<string> strLogoImage = new List<string>();
            byte[] ContractIcon = null;

            var BuilderContracts = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(c => c.Contract.RowStatusId == (int)RowActiveStatus.Active);

            int TotalContracts = BuilderContracts.Count();
            int RemainderPage = 0;

            if (TotalContracts % 4 > 0)
                RemainderPage = 1;

            int TotalPages = (TotalContracts / 4) + RemainderPage;
            ViewBag.TotalContracts = TotalContracts;
            ViewBag.TotalPages = TotalPages;

            foreach (ContractBuilder objConBuilder in BuilderContracts)
            {
                ContractIcon = objConBuilder.Contract.ContractIcon;

                string LogoImageBase64 = "";
                if (ContractIcon != null)
                {
                    LogoImageBase64 = Convert.ToBase64String(ContractIcon);
                    LogoImageBase64 = string.Format("data:image/png;base64,{0}", LogoImageBase64);
                    strLogoImage.Add(LogoImageBase64);
                }
                else
                {
                    strLogoImage.Add("");
                }
            }

            for (int i=1; i<=(TotalPages * 4); i++)
            {
                if (strLogoImage.Count() < i)
                {
                    strLogoImage.Add("");
                }
            }

            ViewBag.ContractLogos = strLogoImage;
            return strLogoImage;
        }

        public FileResult DownloadPreReportingWorksheet(Int64 BuilderId)
        {
            string BuilderPRWFileName = String.Concat(BuilderId.ToString(), ".pdf");
            string BuilderPRWFilePath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), "PreReportingWorksheet", BuilderPRWFileName);

            if (System.IO.File.Exists(BuilderPRWFilePath))
            {
                return File(BuilderPRWFilePath, MimeMapping.GetMimeMapping(BuilderPRWFileName), BuilderPRWFileName);
            }
            else
            {
                BuilderPRWFileName = "NCPReportingRequirementsAndInstructions.pdf";
                BuilderPRWFilePath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), "PreReportingWorksheet", BuilderPRWFileName);

                return File(BuilderPRWFilePath, MimeMapping.GetMimeMapping("NCPReportingRequirementsAndInstructions.pdf"), "NCPReportingRequirementsAndInstructions.pdf");
            }
        }
    }
}