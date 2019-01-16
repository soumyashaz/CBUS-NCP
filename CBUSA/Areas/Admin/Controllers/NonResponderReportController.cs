using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using CBUSA.Areas.Admin.Models;
using CBUSA.Services;
using CBUSA.Services.Interface;
using CBUSA.Domain;

using System.IO;
using Microsoft.Practices.Unity;
using CBUSA.Models;
using CBUSA.Repository;
using System.Text;
using System.Web.UI;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CBUSA.Areas.Admin.Controllers
{
    public class NonResponderReportController : Controller
    {
        readonly IQuaterService _ObjQuaterService;
        readonly INonResponderReportService _ObjNonResponderReportService;

        public NonResponderReportController(IQuaterService ObjQuaterService, INonResponderReportService ObjNonResponderReportService)
        {
            _ObjQuaterService = ObjQuaterService;
            _ObjNonResponderReportService = ObjNonResponderReportService;
        }

        // GET: Admin/NonResponderReport
        public ActionResult Index()
        {
            var CurrentQuarter = _ObjQuaterService.GetQuaterByDate(DateTime.Now);
            Int64 CurrQtrId = CurrentQuarter.Select(qtr => qtr.QuaterId).First();

            ViewBag.CurrQuarter = CurrQtrId;

            return View();
        }

        //Method for fetching Quarter list for populating Quarter dropdown - includes all past quarters & current quarter
        public ActionResult GetQuarterList()
        {
            var CurrentQuarter = _ObjQuaterService.GetQuaterByDate(DateTime.Now);
            Int64 CurrQtrId = CurrentQuarter.Select(qtr => qtr.QuaterId).First();

            var PreviousQuarters = _ObjQuaterService.GetAllPreviousQuater(CurrQtrId).Select(x => 
                                                    new { QuarterId = x.QuaterId, QuarterYear = x.QuaterName + " - " + x.Year })
                                                    .Union(_ObjQuaterService.GetQuaterByDate(DateTime.Now).Select(y => 
                                                    new { QuarterId = y.QuaterId, QuarterYear = y.QuaterName + " - " + y.Year }));

            return Json(PreviousQuarters, JsonRequestBehavior.AllowGet);
        }

        //Method for fetching Non-Responders list data
        public ActionResult GetNonResponderList([DataSourceRequest] DataSourceRequest request, Int64 QuarterId)
        {
            var NonRespondersList = _ObjNonResponderReportService.GetNonResponderBuilderList(QuarterId).Select(x => new 
                                    NonResponderViewModel
                                    {
                                        BuilderId = x.BuilderId,
                                        BuilderName = x.BuilderName,
                                        MarketId = x.MarketId,
                                        MarketName = x.MarketName,
                                        ContractList = x.ContractList,
                                        CountOfParticipatingContracts = x.CountOfParticipatingContracts,
                                        NumberOfReportingQuarters = x.NumberOfReportingQuarters
                                    });

            return Json(NonRespondersList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        //Method for downloading Non-Responders report in CSV format (for a given quarter)
        public FileResult DownloadNonResponderReport(Int64 QuarterId)
        {
            var NonRespondersList = _ObjNonResponderReportService.GetNonResponderBuilderList(QuarterId).Select(x => new
                                    NonResponderViewModel
            {
                BuilderId = x.BuilderId,
                BuilderName = x.BuilderName,
                MarketId = x.MarketId,
                MarketName = x.MarketName,
                ContractList = x.ContractList,
                CountOfParticipatingContracts = x.CountOfParticipatingContracts,
                NumberOfReportingQuarters = x.NumberOfReportingQuarters
            });

            string csv = "";

            try
            {
                if (NonRespondersList.Count() > 0)
                {
                    string newline = "\n";

                    csv = String.Concat(csv + "Market" + ",");
                    csv = String.Concat(csv + "Builder" + ",");
                    csv = String.Concat(csv + "Contracts Enrolled" + ",");
                    csv = String.Concat(csv + "No. Of Enrolled Contracts" + ",");
                    csv = String.Concat(csv + "No. Of Quarters Reported" + newline);

                    foreach (NonResponderViewModel nr in NonRespondersList)
                    {
                        csv = String.Concat(csv + nr.MarketName.Replace(",", "") + ",");
                        csv = String.Concat(csv + nr.BuilderName.Replace(",", "") + ",");
                        csv = String.Concat(csv + nr.ContractList.Replace("," , ";") + ",");
                        csv = String.Concat(csv + nr.CountOfParticipatingContracts + ",");
                        csv = String.Concat(csv + nr.NumberOfReportingQuarters + newline);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
            }

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "NonResponderReport.csv");
        }
    }
}