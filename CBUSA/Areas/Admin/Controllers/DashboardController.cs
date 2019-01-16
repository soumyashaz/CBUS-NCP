using CBUSA.Areas.Admin.Models;
using CBUSA.Areas.CbusaBuilder.Models;
using CBUSA.Domain;
using CBUSA.Services;
using CBUSA.Services.Interface;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Security.Claims;
using System.IO;
using System.Dynamic;
using CBUSA.Models;
using System.Text.RegularExpressions;
using System.IO.Compression;


namespace CBUSA.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        readonly IQuaterService _ObjQuaterService;
        readonly IHolidayService _ObjHolidayService;
        readonly IQuarterReminderService _ObjQuarterReminderService;
        readonly IQuarterEmailTemplateService _ObjQuarterEmailTemplateService;
        readonly IAdminDashboardService _ObjDashboardService;
        readonly IBuilderQuaterContractProjectReportService _ObjQuaterContractProjectReport;
        readonly IBuilderQuaterContractProjectDetailsService _ObjBuilderQuaterContractProjectDetails;
        public DashboardController(IQuaterService ObjQuaterService, IQuarterReminderService ObjQuarterReminderService, IQuarterEmailTemplateService ObjQuarterEmailTemplateService, IAdminDashboardService ObjDashboardService, IHolidayService ObjHolidayService
            , IBuilderQuaterContractProjectReportService ObjQuaterContractProjectReport, IBuilderQuaterContractProjectDetailsService ObjBuilderQuaterContractProjectDetails)
        {
            _ObjQuaterService = ObjQuaterService;
            _ObjHolidayService = ObjHolidayService;
            _ObjDashboardService = ObjDashboardService;
            _ObjQuarterReminderService = ObjQuarterReminderService;
            _ObjQuarterEmailTemplateService = ObjQuarterEmailTemplateService;
            _ObjBuilderQuaterContractProjectDetails = ObjBuilderQuaterContractProjectDetails;
            _ObjQuaterContractProjectReport = ObjQuaterContractProjectReport;
        }

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            GetQuarterEmailTemplates();
            return View();
        }

        public string GetReportingDaysRemaining()
        {
             string ReportingDays = "";

            DateTime currentdate = DateTime.Now.Date;
            
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            
            Quater Q = _ObjQuaterService.GetQuaterById(QuaterId);
            var TotalDays = (Q.ReportingEndDate - Q.ReportingStartDate).TotalDays;
            var RemainingDays = (Q.ReportingEndDate - DateTime.Now.Date).TotalDays;

            ReportingDays = String.Concat(TotalDays.ToString(), "/", RemainingDays.ToString());

            return ReportingDays;
        }

        public string GetReportingStatistics()
        {
            string ReportingStatistics = "";

            DateTime currentdate = DateTime.Now.Date;
         
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            
            int CompletedReporting = _ObjDashboardService.GetCountOfCompletedReportForQuarter(QuaterId);
            int InProgressReporting = _ObjDashboardService.GetCountOfInProgressReportForQuarter(QuaterId);
            int NotStartedReporting = _ObjDashboardService.GetCountOfNotStartedReportForQuarter(QuaterId);

            ReportingStatistics = String.Concat(CompletedReporting.ToString(), "/", InProgressReporting.ToString(), "/", NotStartedReporting.ToString());

            return ReportingStatistics;
        }

        public ActionResult GetHolidayList([DataSourceRequest] DataSourceRequest request)
        {
            var HolidayList = _ObjHolidayService.GetEntireHolidayList();

            return Json(HolidayList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportingWindow()
        {
            DateTime currentdate = DateTime.Now.Date;
            // changed on 6-feb-2018
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            //Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            // changed on 6-feb-2018

            Quater Q = _ObjQuaterService.GetQuaterById(QuaterId);

            return Json(Q, JsonRequestBehavior.AllowGet);
        }

        public void SetReportingWindow(String ReportingFromDate, String ReportingToDate)
        {
            DateTime FromDate = Convert.ToDateTime(ReportingFromDate);
            DateTime ToDate = Convert.ToDateTime(ReportingToDate);

            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;

            bool ReportingPeriodOn = _ObjQuaterService.IsReportingPeriodOn(currentdate);

            if (ReportingPeriodOn)
            {
                Int64 PrevQuarterId = _ObjQuaterService.GetAllPreviousQuater(QuaterId).OrderByDescending(q => q.EndDate).FirstOrDefault().QuaterId;
                _ObjQuaterService.UpdateQuarterReportingWindow(PrevQuarterId, FromDate, ToDate);
            }
            else
            {
                _ObjQuaterService.UpdateQuarterReportingWindow(QuaterId, FromDate, ToDate);
            }
        }

        public ActionResult GetQuarterReminderDates([DataSourceRequest] DataSourceRequest request)
        {
            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;

            var QuarterReminder = _ObjQuarterReminderService.GetQuarterReminderDates(QuaterId);

            return Json(QuarterReminder.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public void SetQuarterReminderDates(String[] ReminderDates)
        {
            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            Int64 ReminderQuarterId;

            bool ReportingPeriodOn = _ObjQuaterService.IsReportingPeriodOn(currentdate);
            if (!ReportingPeriodOn)
            {
                ReminderQuarterId = QuaterId;
            }
            else
            {
                ReminderQuarterId = _ObjQuaterService.GetAllPreviousQuater(QuaterId).OrderByDescending(q => q.EndDate).FirstOrDefault().QuaterId; 
            }

            IEnumerable<QuarterReminder> QRDates = _ObjQuarterReminderService.GetQuarterReminderDates(QuaterId);

            if (QRDates.Count() > 0)
            {
                _ObjQuarterReminderService.DeleteQuarterReminders(ReminderQuarterId);
            }

            for (int i = 0; i < ReminderDates.Count(); i++)
            {
                DateTime qReminderDate = Convert.ToDateTime(ReminderDates[i]);
                String ReminderName = String.Concat("Reminder Date#", (i + 1).ToString());

                _ObjQuarterReminderService.AddQuarterReminder(ReminderQuarterId, ReminderName, qReminderDate);
            }
        }

        public void GetQuarterEmailTemplates()
        {
            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;

            QuarterEmailTemplate QET =_ObjQuarterEmailTemplateService.GetQuarterEmailTemplate(QuaterId);

            if (QET != null)
            {
                ViewBag.QuarterInvitationEmailSubject = QET.InvitationEmailSubject;
                ViewBag.QuarterInvitationEmailTemplate = QET.InvitationEmailTemplate;
                ViewBag.QuarterReminderEmailSubject = QET.ReminderEmailSubject;
                ViewBag.QuarterReminderEmailTemplate = QET.ReminderEmailTemplate;
            }
            else
            {
                ViewBag.QuarterInvitationEmailSubject = "";
                ViewBag.QuarterInvitationEmailTemplate = "";
                ViewBag.QuarterReminderEmailSubject = "";
                ViewBag.QuarterReminderEmailTemplate = "";
            }
        }

        public string SetQuarterEmailTemplates(String[] EmailTemplate)
        {
            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;

            QuarterEmailTemplate QET = _ObjQuarterEmailTemplateService.GetQuarterEmailTemplate(QuaterId);

            if (QET != null)
            {
                _ObjQuarterEmailTemplateService.UpdateQuarterEmailTemplates(QuaterId, EmailTemplate[0], EmailTemplate[1], EmailTemplate[2], EmailTemplate[3]);
            }
            else
            {
                _ObjQuarterEmailTemplateService.AddQuarterEmailTemplates(QuaterId, EmailTemplate[0], EmailTemplate[1], EmailTemplate[2], EmailTemplate[3]);
            }

            return "Success!";
        }

        public JsonResult FetchBurndownStatistics()
        {
            DateTime currentdate = DateTime.Now.Date;
            // changed on 6-feb-2018
            //Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            // changed on 6-feb-2018

            Quater Q = _ObjQuaterService.GetQuaterById(QuaterId);

            int DaysPassed = (DateTime.Now.Date - Q.ReportingStartDate).Days;
            int DaysRemaining = (Q.ReportingEndDate - DateTime.Now.Date).Days;

            var dynBurndownList = new List<dynamic>();

            DateTime dt = Q.ReportingStartDate;
            while (dt <= currentdate)
            {
                int PendingBuildersCount = _ObjDashboardService.GetCountOfReportsSubmittedForQuarterForDay(QuaterId, dt);

                dynamic DailyBurndownData = new ExpandoObject();
                DailyBurndownData.Date = dt.Date.ToShortDateString();
                DailyBurndownData.Value = PendingBuildersCount;

                dynBurndownList.Add(DailyBurndownData);

                dt = dt.AddDays(1);
            }

            if (DaysPassed > 14)
            {


            }
            else if (DaysPassed > 7)
            {


            }
            else
            {


            }

            return Json(dynBurndownList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailsOfNotStartedBuilders([DataSourceRequest] DataSourceRequest request)//[DataSourceRequest] DataSourceRequest request
        {
            DateTime currentdate = DateTime.Now.Date;
            // changed on 6-feb-2018
            //Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            // changed on 6-feb-2018

            var BuilderDetailsList = _ObjDashboardService.GetBuilderDetailsOfNotStartedBuilders(QuaterId).ToList();

            var Result = BuilderDetailsList.Select(bd => new DashboardBuilderListViewModel
            {
                BuilderId = bd.BuilderId,
                BuilderName = bd.BuilderName,
                MarketId = bd.MarketId,
                MarketName = bd.MarketName,
                ContractStatus = bd.ContractStatus,
                LastActivityDate = bd.LastActivityDate
            }).OrderBy(bd => bd.BuilderName);

            var jsonResult = Json(Result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);//.ToDataSourceResult(request)
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //return Json(Result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailsOfInProgressBuilders([DataSourceRequest] DataSourceRequest request)//[DataSourceRequest] DataSourceRequest request
        {
            DateTime currentdate = DateTime.Now.Date;
            // changed on 6-feb-2018
            //Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            // changed on 6-feb-2018

            var BuilderDetailsList = _ObjDashboardService.GetBuilderDetailsOfInProgressBuilders(QuaterId).ToList();

            var Result = BuilderDetailsList.Select(bd => new DashboardBuilderListViewModel
            {
                BuilderId = bd.BuilderId,
                BuilderName = bd.BuilderName,
                MarketId = bd.MarketId,
                MarketName = bd.MarketName,
                ContractStatus = bd.ContractStatus,
                LastActivityDate = bd.LastActivityDate
            }).OrderBy(bd => bd.BuilderName);

            var jsonResult = Json(Result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);//.ToDataSourceResult(request)
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //return Json(Result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailsOfReportingCompletedBuilders([DataSourceRequest] DataSourceRequest request)//[DataSourceRequest] DataSourceRequest request
        {
            DateTime currentdate = DateTime.Now.Date;
            // changed on 6-feb-2018
            //Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            // changed on 6-feb-2018

            var BuilderDetailsList = _ObjDashboardService.GetBuilderDetailsOfReportSubmittedBuilders(QuaterId).ToList();

            var Result = BuilderDetailsList.Select(bd => new DashboardBuilderListViewModel
            {
                BuilderId = bd.BuilderId,
                BuilderName = bd.BuilderName,
                MarketId = bd.MarketId,
                MarketName = bd.MarketName,
                ContractStatus = bd.ContractStatus,
                LastActivityDate = bd.LastActivityDate
            }).OrderBy(bd => bd.BuilderName);

            var jsonResult = Json(Result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);//.ToDataSourceResult(request)
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            //return Json(Result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailsOfAllParticipatingBuilders([DataSourceRequest] DataSourceRequest request)
        {
            DateTime currentdate = DateTime.Now.Date;
            // changed on 6-feb-2018
            //Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            // changed on 6-feb-2018

            var AllBuilderDetailsList = _ObjDashboardService.GetBuilderDetailsOfAllParticipatingBuilders(QuaterId).ToList();

            var Result = AllBuilderDetailsList.Select(bd => new DashboardBuilderContractProjectListViewModel
            {
                BuilderId = bd.BuilderId,
                BuilderName = bd.BuilderName,
                MarketId = bd.MarketId,
                MarketName = bd.MarketName,
                ContractId = bd.ContractId,
                ContractName = bd.ContractName,
                VendorName = bd.VendorName,
                TotalProjects = bd.TotalProjects,
                ReportedProjects = bd.ReportedProjects,
                ContractEnrolledDate = bd.ContractEnrolledDate,
                ProjectId = bd.ProjectId,
                ProjectName = bd.ProjectName,
                LotNo = bd.LotNo,
                Address = bd.Address,
                City = bd.City,
                State = bd.State,
                Zip = bd.Zip,
                ProjectCreatedOn = bd.ProjectCreatedOn,
                ProjectStatus = bd.ProjectStatus
            }).OrderBy(bd => bd.BuilderName);

            var jsonResult = Json(Result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }



        #region Hierarchy Grid By Rajendar 2018-02-28
        public JsonResult GetDetailsOfAllBuilderContractsForCurrentQuarter()
        {

            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            GridRequestParameters KendoParameter = GridRequestParameters.Current;
            IEnumerable<dynamic> AllBuilderContractDetailsList = _ObjDashboardService.GetDetailsOfAllBuilderContractsForCurrentQuarter(QuaterId);
            //string group = string.Empty;
            //if (KendoParameter.Groups.Count() > 0)
            //{
            //    foreach (var item in KendoParameter.Groups)
            //    {
            //        AllBuilderContractDetailsList.GroupBy(g => (g.GetType().GetProperty(item.GroupBy).GetValue(g)));
            //    }
            //}
            if (KendoParameter.Filters.Count() > 0)
            {
                // start of filter list
                string OtherFilter = KendoParameter.FilterLogic.ToString();
                string OtherFilterValue = KendoParameter.Filters.Select(x => x.Value).FirstOrDefault().ToString().ToLower();
                if (OtherFilter.ToUpper() == "OR")
                {
                    AllBuilderContractDetailsList = AllBuilderContractDetailsList.Where(c => c.BuilderName.ToString().ToLower().Contains(OtherFilterValue)
                                                    || c.MarketName.ToString().ToLower().Contains(OtherFilterValue)
                                                    || c.ContractName.ToString().ToLower().Contains(OtherFilterValue)
                                                    || c.VendorName.ToString().ToLower().Contains(OtherFilterValue)

                                                  );
                }
                else
                {
                    foreach (KendoFilterInfo Filter in KendoParameter.Filters)
                    {
                        string FilterDataType = string.Empty;
                        switch (Filter.Field)
                        {


                            case "BuilderName":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "BuilderName", ref AllBuilderContractDetailsList, FilterDataType);
                                break;
                            case "MarketName":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "MarketName", ref AllBuilderContractDetailsList, FilterDataType);
                                break;
                            case "ContractName":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "ContractName", ref AllBuilderContractDetailsList, FilterDataType);
                                break;
                            case "VendorName":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "VendorName", ref AllBuilderContractDetailsList, FilterDataType);
                                break;
                            case "ContractEnrolledDate":
                                FilterDataType = "date";
                                KendoParameter.KendoGridFilterGeneric(Filter, "ContractEnrolledDate", ref AllBuilderContractDetailsList, FilterDataType);
                                break;
                            case "ListReport":
                                FilterDataType = "integer";
                                KendoParameter.KendoGridFilterGeneric(Filter, "ListReport", ref AllBuilderContractDetailsList, FilterDataType);

                                break;


                        }
                    }
                }
                // end of for each loop - filter list
            }

            if (KendoParameter.Sortings.Count() > 0)
            {
                List<SortingInfo> ListSort = KendoParameter.Sortings.ToList();

                AllBuilderContractDetailsList = GridRequestParameters.MultipleSort(AllBuilderContractDetailsList, ListSort);


            }
            int count = 0;

            if (AllBuilderContractDetailsList != null)
            {
                count = AllBuilderContractDetailsList.Count();
                var data = AllBuilderContractDetailsList.Skip(KendoParameter.Skip).Take(KendoParameter.Take);
                var JsonData = new { total = count, data };//,group=KendoParameter.Groups
                var jsonResult = Json(JsonData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            //var data = AllBuilderContractDetailsList.Skip(KendoParameter.Skip).Take(KendoParameter.Take);

            return Json("", JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetDetailsOfAllProjectByBuilderContractsForCurrentQuarter(Int64 BuilderId, Int64 ContractId)
        {

            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
            GridRequestParameters KendoParameter = GridRequestParameters.Current;
            IEnumerable<dynamic> AllProjectByBuilderContractDetailsList = _ObjDashboardService.GetDetailsOfAllProjectByBuilderContractsForCurrentQuarter(QuaterId, BuilderId, ContractId);
            if (KendoParameter.Filters.Count() > 0)
            {
                // start of filter list
                string OtherFilter = KendoParameter.FilterLogic.ToString();
                string OtherFilterValue = KendoParameter.Filters.Select(x => x.Value).FirstOrDefault().ToString().ToLower();
                if (OtherFilter.ToUpper() == "OR")
                {
                    AllProjectByBuilderContractDetailsList = AllProjectByBuilderContractDetailsList.Where(c => c.ProjectName.ToLower().Contains(OtherFilterValue)
                                                    || c.LotNo.ToLower().Contains(OtherFilterValue)
                                                    || c.Address.ToLower().Contains(OtherFilterValue)
                                                    || c.City.ToLower().Contains(OtherFilterValue)
                                                    || c.State.ToLower().Contains(OtherFilterValue)
                                                    || c.Zip.ToLower().Contains(OtherFilterValue)

                                                  );
                }
                else
                {
                    foreach (KendoFilterInfo Filter in KendoParameter.Filters)
                    {
                        string FilterDataType = string.Empty;
                        switch (Filter.Field)
                        {


                            case "ProjectName":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "ProjectName", ref AllProjectByBuilderContractDetailsList, FilterDataType);
                                break;
                            case "LotNo":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "LotNo", ref AllProjectByBuilderContractDetailsList, FilterDataType);
                                break;
                            case "Address":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "Address", ref AllProjectByBuilderContractDetailsList, FilterDataType);
                                break;
                            case "City":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "City", ref AllProjectByBuilderContractDetailsList, FilterDataType);
                                break;
                            case "State":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "State", ref AllProjectByBuilderContractDetailsList, FilterDataType);
                                break;
                            case "Zip":
                                FilterDataType = "string";
                                KendoParameter.KendoGridFilterGeneric(Filter, "Zip", ref AllProjectByBuilderContractDetailsList, FilterDataType);
                                break;
                            case "ProjectCreatedOn":
                                FilterDataType = "date";
                                KendoParameter.KendoGridFilterGeneric(Filter, "ProjectCreatedOn", ref AllProjectByBuilderContractDetailsList, FilterDataType);
                                break;


                        }
                    }
                }
                // end of for each loop - filter list
            }

            if (KendoParameter.Sortings.Count() > 0)
            {
                List<SortingInfo> ListSort = KendoParameter.Sortings.ToList();

                AllProjectByBuilderContractDetailsList = GridRequestParameters.MultipleSort(AllProjectByBuilderContractDetailsList, ListSort);


            }
            int count = 0;
            if (AllProjectByBuilderContractDetailsList.Count() > 0)
            {
                count = AllProjectByBuilderContractDetailsList.Count();
            }
            var data = AllProjectByBuilderContractDetailsList.Skip(KendoParameter.Skip).Take(KendoParameter.Take);
            var JsonData = new { total = count, data };
            var jsonResult = Json(JsonData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetDetailsOfAllProjectByBuilderContractsForCurrentQuarterByQuater()
        {

            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetLastReportingQuarter().QuaterId;
           
            IEnumerable<dynamic> AllProjectByBuilderContractDetailsList = _ObjDashboardService.GetDetailsOfAllProjectByBuilderContractsForCurrentQuarter(QuaterId);
           
            var data = AllProjectByBuilderContractDetailsList;
           
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
        #region Covert to Zip & Download By Rajendar On 3/16/2018
        public ActionResult GetReportedDocuments()
        {

            return View();
        }
        public JsonResult GetQuaterlist()
        {
            var Q = _ObjQuaterService.GetQuater().Select(s => new { QuaterId = s.QuaterId, QuaterName = s.QuaterName + "-" + s.Year }).ToList();
            return Json(Q, JsonRequestBehavior.AllowGet);
        }
        public FileResult GetDownloadableReportedDocument(Int64 QuaterId)
        {
            if (QuaterId != 0)
            {
                var ObjList = _ObjBuilderQuaterContractProjectDetails.GetProjectDetailsForBuilderQuaterContractProjectReportByQuater(QuaterId);
                var ObjQuater = _ObjQuaterService.GetQuaterById(QuaterId);
                if (ObjList != null && ObjList.Count() > 0)
                {
                    string FileRepoDestinationPath = Server.MapPath("~/ApplicationDocument/NCPRebateReportDocumentDownloadable");
                    if (!Directory.Exists(FileRepoDestinationPath))
                    {

                        System.IO.Directory.CreateDirectory(FileRepoDestinationPath);
                    }
                    else
                    {
                        System.IO.Directory.Delete(FileRepoDestinationPath, true);
                        if (!Directory.Exists(FileRepoDestinationPath))
                        {

                            System.IO.Directory.CreateDirectory(FileRepoDestinationPath);
                        }
                    }
                    foreach (var item in ObjList)
                    {
                        string FileName = item.ItemArray[4];

                        //******** CONDITION ADDED TO SUPPORT MULTIPLE FILE UPLOAD (APALA - 18th Dec 2018) ********
                        if (FileName.Contains(";"))
                        {
                            string[] arrFiles = FileName.Split(';');

                            foreach (string strFileName in arrFiles)
                            {
                                CopyFile(item.ItemArray[1], item.ItemArray[3], strFileName);
                            }
                        }
                        //******************************************************************************************
                        else
                        {
                            CopyFile(item.ItemArray[1], item.ItemArray[3], item.ItemArray[4]);
                        }
                    }
                    string ZipLoaction = Server.MapPath("~/ApplicationDocument/NCPRebateReportDocumentDownloadableZip/" + ObjQuater.QuaterName+"-"+ObjQuater.Year+".zip");
                   
                    if (System.IO.File.Exists(ZipLoaction))
                    {
                        System.IO.File.Delete(ZipLoaction);
                    }

                    ZipFile.CreateFromDirectory(FileRepoDestinationPath, ZipLoaction, CompressionLevel.Fastest,true);
                    if (System.IO.File.Exists(ZipLoaction))
                    {
                        return File(ZipLoaction, MimeMapping.GetMimeMapping(ZipLoaction), ObjQuater.QuaterName + "-" + ObjQuater.Year + ".zip");
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            return null;
        }
        private void CopyFile(string BuilderName, string SurveyName, string FileName)
        {
            string FileRepoSourcePath = Server.MapPath("~/ApplicationDocument/NCPRebateReportDocument");
            string FileRepoDestinationPath = Server.MapPath("~/ApplicationDocument/NCPRebateReportDocumentDownloadable");
            string FileCopyFromPath = Path.Combine(FileRepoSourcePath, FileName);
            string StrBuilderName = Regex.Replace(BuilderName, @"[^a-zA-Z0-9_]", "");
            string StrSurveyName = Regex.Replace(SurveyName, @"[^a-zA-Z0-9_]", "");
            string ConcatPath = string.Concat(StrBuilderName, @"\", StrSurveyName);
            string FileCopyToPath = string.Empty;
            if (ConcatPath.Length < 50)
            {
                FileCopyToPath = Path.Combine(FileRepoDestinationPath, Path.Combine(StrBuilderName, StrSurveyName));
            }
            else
            {
                StrSurveyName = StrSurveyName.Substring(0, (StrSurveyName.Length - (ConcatPath.Length - 50)));
                FileCopyToPath = Path.Combine(FileRepoDestinationPath, Path.Combine(StrBuilderName, StrSurveyName));
            }
            if (System.IO.File.Exists(FileCopyFromPath) && !string.IsNullOrWhiteSpace(FileCopyToPath))
            {
                if (!Directory.Exists(FileCopyToPath))
                {
                    Directory.CreateDirectory(FileCopyToPath);
                }
                if (!System.IO.File.Exists(Path.Combine(FileCopyToPath, FileName)))
                {
                    System.IO.File.Copy(FileCopyFromPath, Path.Combine(FileCopyToPath, FileName));
                }

            }
        }
        #endregion
    }
}