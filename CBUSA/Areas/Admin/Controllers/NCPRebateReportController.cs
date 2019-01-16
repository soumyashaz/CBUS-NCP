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
    [Authorize(Roles = "SuperAdmin")]
    public class NCPRebateReportController : Controller
    {
        readonly IContractServices _ObjContractService;
        readonly ISurveyService _ObjSurveyService;
        readonly IQuestionService _ObjQuestionService;
        readonly IContractComplianceService _ObjContractComplianceService;
        readonly ISurveyBuilderService _ObjSurveyBuilderService;
        readonly IQuaterService _ObjQuaterService;
        readonly IMarketService _ObjMarketService;
        readonly IConstructFormulaService _ConstructFormulaService;
        // GET: Admin/NCPRebateReport
        public NCPRebateReportController(IContractServices ObjContractService, ISurveyService ObjSurveyService,
            IQuestionService ObjQuestionService, IContractComplianceService ObjContractComplianceService, ISurveyBuilderService ObjSurveyBuilderService,
            IQuaterService ObjQuaterService, IMarketService ObjMarketService, IConstructFormulaService ConstructFormulaService)
        {
            _ObjContractService = ObjContractService;
            _ObjSurveyService = ObjSurveyService;
            _ObjQuestionService = ObjQuestionService;
            _ObjContractComplianceService = ObjContractComplianceService;
            _ObjSurveyBuilderService = ObjSurveyBuilderService;
            _ObjQuaterService = ObjQuaterService;
            _ObjMarketService = ObjMarketService;
            _ConstructFormulaService = ConstructFormulaService;
        }

        //Start Manage ncp rebate reports
        #region Manage Compliance report
        public ActionResult ManageReport()
        {
            return View();
        }

        //public ActionResult NcpRebateReportList([DataSourceRequest] DataSourceRequest request, string Type, string Flag, int? PageValue)
        //{
        //    int RowNo = 2;
        //    if (Flag == null && Type == null)
        //    {
        //        var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderByDescending(x => x.SurveyId).
        //                Select(x => new SurveyViewModel
        //                {
        //                    SurveyId = x.SurveyId,
        //                    Year=x.Year,
        //                    Quater=x.Quater,
        //                    ResponseCom = response(x.IsPublished),
        //                    ResponsePend = responsepend(x.SurveyId.ToString()),
        //                    SurveyName = x.SurveyName,
        //                    Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                    LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                    PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                    ContractName = x.Contract.ContractName,
        //                    IsPublished = x.IsPublished,
        //                    ContractId = x.ContractId,
        //                    SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""
        //                });
        //        return Json(SurveyList.ToDataSourceResult(request));
        //    }
        //    else
        //    {
        //        if (Flag == "")
        //        {
        //            if (Type == "asccon")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderBy(x => x.Contract.ContractName).
        //               Select(x => new SurveyViewModel
        //               {
        //                   SurveyId = x.SurveyId,
        //                   Year = x.Year,
        //                   Quater = x.Quater,
        //                   ResponseCom = response(x.IsPublished),
        //                   ResponsePend = responsepend(x.SurveyId.ToString()),
        //                   SurveyName = x.SurveyName,
        //                   Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                   LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                   PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                   ContractName = x.Contract.ContractName,
        //                   IsPublished = x.IsPublished,
        //                   ContractId = x.ContractId,
        //                   SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""
        //               });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else if (Type == "desccon")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderByDescending(x => x.Contract.ContractName).
        //               Select(x => new SurveyViewModel
        //               {
        //                   SurveyId = x.SurveyId,
        //                   Year = x.Year,
        //                   Quater = x.Quater,
        //                   SurveyName = x.SurveyName,
        //                   ResponseCom = response(x.IsPublished),
        //                   ResponsePend = responsepend(x.SurveyId.ToString()),
        //                   Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                   LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                   PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                   ContractName = x.Contract.ContractName,
        //                   IsPublished = x.IsPublished,
        //                   ContractId = x.ContractId,
        //                   SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""
        //               });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else if (Type == "ascncp")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderBy(x => x.SurveyName).
        //               Select(x => new SurveyViewModel
        //               {
        //                   SurveyId = x.SurveyId,
        //                   Year = x.Year,
        //                   Quater = x.Quater,
        //                   ResponseCom = response(x.IsPublished),
        //                   ResponsePend = responsepend(x.SurveyId.ToString()),
        //                   SurveyName = x.SurveyName,

        //                   Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                   LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                   PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                   ContractName = x.Contract.ContractName,
        //                   IsPublished = x.IsPublished,
        //                   ContractId = x.ContractId,
        //                   SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


        //               });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else if (Type == "descncp")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderByDescending(x => x.SurveyName).
        //              Select(x => new SurveyViewModel
        //              {
        //                  SurveyId = x.SurveyId,
        //                  Year = x.Year,
        //                  Quater = x.Quater,
        //                  ResponseCom = response(x.IsPublished),
        //                  ResponsePend = responsepend(x.SurveyId.ToString()),
        //                  SurveyName = x.SurveyName,

        //                  Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                  LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                  PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                  ContractName = x.Contract.ContractName,
        //                  IsPublished = x.IsPublished,
        //                  ContractId = x.ContractId,
        //                  SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""



        //              });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else if (Type == "ascyr")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderBy(x => x.Year).
        //               Select(x => new SurveyViewModel
        //               {
        //                   SurveyId = x.SurveyId,
        //                   Year = x.Year,
        //                   Quater = x.Quater,
        //                   ResponseCom = response(x.IsPublished),
        //                   ResponsePend = responsepend(x.SurveyId.ToString()),
        //                   SurveyName = x.SurveyName,

        //                   Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                   LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                   PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                   ContractName = x.Contract.ContractName,
        //                   IsPublished = x.IsPublished,
        //                   ContractId = x.ContractId,
        //                   SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


        //               });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else if (Type == "descyr")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderByDescending(x => x.Year).
        //              Select(x => new SurveyViewModel
        //              {
        //                  SurveyId = x.SurveyId,
        //                  Year = x.Year,
        //                  Quater = x.Quater,
        //                  ResponseCom = response(x.IsPublished),
        //                  ResponsePend = responsepend(x.SurveyId.ToString()),
        //                  SurveyName = x.SurveyName,

        //                  Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                  LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                  PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                  ContractName = x.Contract.ContractName,
        //                  IsPublished = x.IsPublished,
        //                  ContractId = x.ContractId,
        //                  SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""



        //              });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else if (Type == "ascdue")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderBy(x => x.EndDate).
        //               Select(x => new SurveyViewModel
        //               {
        //                   SurveyId = x.SurveyId,
        //                   Year = x.Year,
        //                   Quater = x.Quater,
        //                   ResponseCom = response(x.IsPublished),
        //                   ResponsePend = responsepend(x.SurveyId.ToString()),
        //                   SurveyName = x.SurveyName,

        //                   Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                   LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                   PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                   ContractName = x.Contract.ContractName,
        //                   IsPublished = x.IsPublished,
        //                   ContractId = x.ContractId,
        //                   SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


        //               });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else if (Type == "descdue")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderByDescending(x => x.EndDate).
        //              Select(x => new SurveyViewModel
        //              {
        //                  SurveyId = x.SurveyId,
        //                  Year = x.Year,
        //                  Quater = x.Quater,
        //                  ResponseCom = response(x.IsPublished),
        //                  ResponsePend = responsepend(x.SurveyId.ToString()),
        //                  SurveyName = x.SurveyName,

        //                  Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                  LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                  PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                  ContractName = x.Contract.ContractName,
        //                  IsPublished = x.IsPublished,
        //                  ContractId = x.ContractId,
        //                  SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""



        //              });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else if (Type == "archser")
        //            {
        //                DateTime archdate = DateTime.Now.AddDays(-30);
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderBy(x => x.EndDate).
        //               Select(x => new SurveyViewModel
        //               {
        //                   SurveyId = x.SurveyId,
        //                   Year = x.Year,
        //                   Quater = x.Quater,
        //                   ResponseCom = response(x.IsPublished),
        //                   ResponsePend = responsepend(x.SurveyId.ToString()),
        //                   SurveyName = x.SurveyName,

        //                   Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                   LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                   PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                   ContractName = x.Contract.ContractName,
        //                   IsPublished = x.IsPublished,
        //                   ContractId = x.ContractId,
        //                   SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


        //               });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }

        //            else if (Type == null)
        //            {

        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderByDescending(x => x.SurveyId).
        //                Select(x => new SurveyViewModel
        //                {
        //                    SurveyId = x.SurveyId,
        //                    Year = x.Year,
        //                    Quater = x.Quater,
        //                    ResponseCom = response(x.IsPublished),
        //                    ResponsePend = responsepend(x.SurveyId.ToString()),
        //                    SurveyName = x.SurveyName,

        //                    Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                    LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                    PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                    ContractName = x.Contract.ContractName,
        //                    IsPublished = x.IsPublished,
        //                    ContractId = x.ContractId,
        //                    SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""



        //                });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //        }
        //        else
        //        {
        //            if (Flag == "0")
        //            {
        //                var SurveyList = _ObjSurveyService.GetNcpSurveyAll().OrderByDescending(x => x.SurveyId).
        //               Select(x => new SurveyViewModel
        //               {
        //                   SurveyId = x.SurveyId,
        //                   Year = x.Year,
        //                   Quater = x.Quater,
        //                   ResponseCom = response(x.IsPublished),
        //                   ResponsePend = responsepend(x.SurveyId.ToString()),
        //                   SurveyName = x.SurveyName,

        //                   Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                   LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                   PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                   ContractName = x.Contract.ContractName,
        //                   IsPublished = x.IsPublished,
        //                   ContractId = x.ContractId,
        //                   SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""



        //               });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }
        //            else
        //            {
        //                var SurveyList = _ObjSurveyService.FindContractNcpSurveysAll(Convert.ToInt32(Flag)).OrderByDescending(x => x.SurveyId).
        //              Select(x => new SurveyViewModel
        //              {
        //                  SurveyId = x.SurveyId,
        //                  Year = x.Year,
        //                  Quater = x.Quater,
        //                  ResponseCom = response(x.IsPublished),
        //                  ResponsePend = responsepend(x.SurveyId.ToString()),
        //                  SurveyName = x.SurveyName,

        //                  Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
        //                  LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
        //                  PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
        //                  ContractName = x.Contract.ContractName,
        //                  IsPublished = x.IsPublished,
        //                  ContractId = x.ContractId,
        //                  SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""



        //              });
        //                return Json(SurveyList.ToDataSourceResult(request));
        //            }


        //        }

        //    }

        //    var value = "";
        //    return Json(value.ToDataSourceResult(request));
        //}

        public ActionResult NcpRebateReportList([DataSourceRequest] DataSourceRequest request, string Type, string Flag, int? PageValue)
        {

            int RowNo = 2;
            IEnumerable<Survey> SurveyList = null;

            IEnumerable<SurveyViewModel> list;
            if (Flag == null && Type == null)
            {
                SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderByDescending(x => x.SurveyId);

            }

            else
            {

                if (Flag == "")
                {

                    if (Type == "asccon")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderBy(x => x.Contract.ContractName);


                    }

                    else if (Type == "desccon")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderByDescending(x => x.Contract.ContractName);

                    }

                    else if (Type == "ascncp")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderBy(x => x.SurveyName);

                    }
                    else if (Type == "descncp")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderByDescending(x => x.SurveyName);

                    }
                    else if (Type == "ascyr")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderBy(x => x.Year);

                    }
                    else if (Type == "descyr")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderByDescending(x => x.Year);

                    }
                    else if (Type == "ascdue")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderBy(x => x.EndDate);

                    }
                    else if (Type == "descdue")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderByDescending(x => x.EndDate);

                    }
                    else if (Type == "archser")
                    {
                        DateTime archdate = DateTime.Now.AddDays(-30);
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderBy(x => x.EndDate);

                    }

                    else if (Type == null)
                    {

                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderByDescending(x => x.SurveyId);

                    }
                }
                else
                {
                    if (Flag == "0")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllActiveAndClose().OrderByDescending(x => x.SurveyId);

                    }
                    else
                    {
                        SurveyList = _ObjSurveyService.FindContractNcpSurveysAll(Convert.ToInt32(Flag)).OrderByDescending(x => x.SurveyId);

                    }


                }

            }
            list = SurveyList.Select(x => new SurveyViewModel
            {
                SurveyId = x.SurveyId,
                Year = x.Year != null ? x.Year : "",
                Quater = x.Quater != null ? x.Quater : "",
                ResponseCom = responsecomplete(x.SurveyId),
                ResponseInCom = responseincomplete(x.SurveyId),
                //ResponseCom = response(x.IsPublished),
                ResponsePend = responsepend(x.SurveyId.ToString()),
                SurveyName = x.SurveyName,

                Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                ContractName = x.Contract.ContractName,
                IsPublished = x.IsPublished,
                ContractId = x.ContractId,
                // SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""
                SurveyStatus = Status(x.RowStatusId)
            });

            int SurveyCounter = 1;
            var newList = list.ToList()
                    .Select(x => new SurveyViewModel
                    {
                        SurveyId = x.SurveyId,
                        Year = x.Year,
                        Quater = x.Quater,
                        ResponseCom = x.ResponseCom,
                        ResponseInCom = x.ResponseInCom,
                        ResponsePend = x.ResponsePend,
                        SurveyName = x.SurveyName,
                        Archive = x.Archive,
                        LastDate = x.LastDate,
                        PublishDate = x.PublishDate,
                        ContractName = x.ContractName,
                        IsPublished = x.IsPublished,
                        ContractId = x.ContractId,
                        SurveyStatus = x.SurveyStatus,
                        rowcount = SurveyCounter++
                    }).ToList();

            return Json(newList.ToDataSourceResult(request));
            
        }

        public ActionResult ArchivedReport()
        {
            return View();
        }


        public ActionResult ArchivedReportList([DataSourceRequest] DataSourceRequest request, string Type, string Flag, int? PageValue)
        {

            int RowNo = 2;
            int SurveyCounter = 1;
            IEnumerable<Survey> SurveyList = null;

            IEnumerable<SurveyViewModel> list;

            if (Flag == null && Type == null)
            {
                SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderByDescending(x => x.SurveyId);

            }

            else
            {

                if (Flag == "")
                {

                    if (Type == "asccon")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderBy(x => x.Contract.ContractName);

                    }

                    else if (Type == "desccon")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderByDescending(x => x.Contract.ContractName);

                    }

                    else if (Type == "ascncp")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderBy(x => x.SurveyName);

                    }
                    else if (Type == "descncp")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderByDescending(x => x.SurveyName);

                    }
                    else if (Type == "ascyr")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderBy(x => x.Year);

                    }
                    else if (Type == "descyr")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderByDescending(x => x.Year);

                    }
                    else if (Type == "ascdue")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderBy(x => x.EndDate);

                    }
                    else if (Type == "descdue")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderByDescending(x => x.EndDate);

                    }
                    else if (Type == "archser")
                    {
                        DateTime archdate = DateTime.Now.AddDays(-30);
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderBy(x => x.EndDate);

                    }

                    else if (Type == null)
                    {

                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderByDescending(x => x.SurveyId);

                    }
                }
                else
                {
                    if (Flag == "0")
                    {
                        SurveyList = _ObjSurveyService.GetNcpSurveyAllArchive().OrderByDescending(x => x.SurveyId);

                    }
                    else
                    {
                        SurveyList = _ObjSurveyService.FindContractNcpSurveysAllArchive(Convert.ToInt32(Flag)).OrderByDescending(x => x.SurveyId);

                    }


                }

            }

            list = SurveyList.Select(x => new SurveyViewModel
            {
                SurveyId = x.SurveyId,
                Year = x.Year,
                Quater = x.Quater,
                //ResponseCom = response(x.IsPublished),
                ResponseCom = responsecomplete(x.SurveyId),
                ResponseInCom = responseincomplete(x.SurveyId),
                ResponsePend = responsepend(x.SurveyId.ToString()),
                SurveyName = x.SurveyName,

                Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                ContractName = x.Contract.ContractName,
                IsPublished = x.IsPublished,
                ContractId = x.ContractId,
                SurveyStatus = Status(x.RowStatusId)



            });

            var newList = list.ToList()
                    .Select(x => new SurveyViewModel
                    {
                        SurveyId = x.SurveyId,
                        Year = x.Year,
                        Quater = x.Quater,
                        ResponseCom = x.ResponseCom,
                        ResponseInCom = x.ResponseInCom,
                        ResponsePend = x.ResponsePend,
                        SurveyName = x.SurveyName,
                        Archive = x.Archive,
                        LastDate = x.LastDate,
                        PublishDate = x.PublishDate,
                        ContractName = x.ContractName,
                        IsPublished = x.IsPublished,
                        ContractId = x.ContractId,
                        SurveyStatus = x.SurveyStatus,
                        rowcount = SurveyCounter++
                    }).ToList();

            return Json(newList.ToDataSourceResult(request));

        }
        public JsonResult GetContracts()
        {

            Int64 conid = 0;
            var ContractList = _ObjContractService.GetContract().OrderBy(x => x.ContractName)
                .Select(x => new { ContractId = x.ContractId, ContractName = x.ContractName }).ToList();

            ContractList.Insert(0, new { ContractId = conid, ContractName = "Select All" });



            return Json(ContractList, JsonRequestBehavior.AllowGet);
        }
        public string FromDate(string date)
        {
            string dat = "";
            if (date != null)
            {
                DateTime fromdate = Convert.ToDateTime(date);

                dat = fromdate.ToString("MM/dd/yy");
                return dat;
            }
            return dat;
        }
        public string response(bool data)
        {
            //string SurveyList = _ObjSurveyService.GetSurveyBuilder(Convert.ToInt32(data)).Where(x => x.IsSurveyCompleted == true).Count().ToString();
            string flag = "";
            if (data == true)
            {
                flag = "0|0";
            }
            else
            {
                flag = "Not published";
            }

            return flag;
        }
        public string responsecomplete(Int64 SurveyId)
        {
            //string SurveyList = _ObjSurveyService.GetSurveyBuilder(Convert.ToInt32(data)).Where(x => x.IsSurveyCompleted == true).Count().ToString();
            string flag = "";
            int Complete = _ObjSurveyService.GetCompleteedBuilderNCP(SurveyId);

            //int Complete = _ObjSurveyBuilderService.FindCompleteBuilderSurvey(SurveyId).Count();
            //int InComplete = _ObjSurveyBuilderService.FindInCompleteBuilderSurvey(SurveyId).Count();
            flag = Complete.ToString();


            return flag;
        }
        public string responseincomplete(Int64 SurveyId)
        {
            //string SurveyList = _ObjSurveyService.GetSurveyBuilder(Convert.ToInt32(data)).Where(x => x.IsSurveyCompleted == true).Count().ToString();
            string flag = "";
            int InComplete = _ObjSurveyService.GetInCompleteedBuilderNCP(SurveyId);

            //int Complete = _ObjSurveyBuilderService.FindCompleteBuilderSurvey(SurveyId).Count();
            //int InComplete = _ObjSurveyBuilderService.FindInCompleteBuilderSurvey(SurveyId).Count();
            flag = InComplete.ToString();


            return flag;
        }
        public string responsepend(string data)
        {
            string SurveyList = _ObjSurveyService.GetSurveyBuilder(Convert.ToInt32(data)).Where(x => x.IsSurveyCompleted == false).Count().ToString();


            return SurveyList;
        }
        public string Status(int statusid)
        {
            string dat = "";


            if (statusid == 1)
            {
                dat = "Live";
            }
            else if (statusid == 3)
            {
                dat = "Closed";
            }
            else
            {
                dat = "Archived";
            }

            return dat;
        }
        //public string Status(string date)
        //{
        //    string dat = "";
        //    if (date != null)
        //    {
        //        DateTime enddate = Convert.ToDateTime(date);

        //        if (enddate > DateTime.Now)
        //        {
        //            dat = "Live";
        //        }
        //        else
        //        {
        //            dat = "Closed";
        //        }
        //    }
        //    return dat;
        //}
        public string Archive(string date, bool pub)
        {
            DateTime enddate = Convert.ToDateTime(date);
            string dat = "";
            if (date != null)
            {
                if (enddate < DateTime.Now && pub == true)
                {
                    dat = "y";
                }

                else if (enddate < DateTime.Now && pub == false)
                {
                    dat = "n";
                }

            }
            return dat;

        }
        public string Publish(string date, string modified, bool pub)
        {
            DateTime enddate = Convert.ToDateTime(date);
            DateTime modify = Convert.ToDateTime(modified);
            string dat = "";
            if (date != null)
            {
                if (enddate > DateTime.Now && pub == true)
                {
                    dat = "Published on:" + modify.ToString("MM/dd/yy");
                }

                else if (enddate > DateTime.Now && pub == false)
                {
                    dat = "Last edited:" + modify.ToString("MM/dd/yy");
                }
                else if (enddate < DateTime.Now)
                {
                    dat = "Closed on:" + enddate.ToString("MM/dd/yy");
                }
            }
            return dat;
        }
        //end manage ncp rebate report
        #endregion
        public ActionResult Index()
        {
            return View();
        }


        #region compliance


        public ActionResult Compliance()
        {

            return View();
        }



        public ActionResult ActiveContract_Read([DataSourceRequest] DataSourceRequest request)
        {
            var ActiveContractList = _ObjContractService.GetActiveContract().OrderByDescending(x => x.ContractId).
                Select(x => new ActiveContractViewModel
                {
                    ConractId = x.ContractId,
                    ConractName = x.ContractName,
                });
            return Json(ActiveContractList.ToDataSourceResult(request));
        }

        #endregion

        #region configure compliance factor

        public ActionResult LoadWndConfigureCompliance(Int64 ContractId)
        {
            NcpComplianceViewModel ObVm = new NcpComplianceViewModel();
            ObVm.ContractId = ContractId;
            var ObjEstimatedCopliance = _ObjContractComplianceService.ContractEstimateCompliance(ContractId);
            if (ObjEstimatedCopliance != null)
            {

                ObVm.EstimatedSurveyId = ObjEstimatedCopliance.SurveyId;
                ObVm.EstimatedQuestionId = ObjEstimatedCopliance.QuestionId.GetValueOrDefault();
                ObVm.EstimatedComposeFormula = ObjEstimatedCopliance.ComplianceFormula;
                ObVm.IsEstimateDirectQuestion = ObjEstimatedCopliance.IsDirectQuestion;

            }
            else
            {
                ObVm.IsEstimateDirectQuestion = true;
            }
            var ObjActualCopliance = _ObjContractComplianceService.ContractActualCompliance(ContractId);
            if (ObjActualCopliance != null)
            {

                var Quater = _ObjQuaterService.GetQuaterById(ObjActualCopliance.QuaterId.GetValueOrDefault());

                if (Quater != null)
                {
                    ObVm.Year = Quater.Year;
                    ObVm.Quater = Quater.QuaterName;
                }


                ObVm.ActualsSurveyId = ObjActualCopliance.SurveyId;
                ObVm.ActualQuestionId = ObjActualCopliance.QuestionId.GetValueOrDefault();
                ObVm.ActualComposeFormula = ObjActualCopliance.ComplianceFormula;
                ObVm.IsActualDirectQuestion = ObjActualCopliance.IsDirectQuestion;
                //ObVm.Year= ObjActualCopliance.yea
            }
            else
            {
                ObVm.IsActualDirectQuestion = true;
            }

            ObVm.QuaterList = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
            {
                Value = x,
                Text = x
            }).ToList();
            ObVm.QuaterList.Insert(0, new SelectListItem { Text = "--Select Quarter--", Value = "0" });
            ObVm.YearList = new List<SelectListItem> {
                      new SelectListItem{Text="--Select Year--", Value="0"},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year-1), Value=Convert.ToString(DateTime.Now.Year-1)},
                     new SelectListItem{Text=Convert.ToString(DateTime.Now.Year), Value=Convert.ToString(DateTime.Now.Year)},
                      new SelectListItem{Text=Convert.ToString(DateTime.Now.Year+1), Value=Convert.ToString(DateTime.Now.Year+1)},

                    };


            return PartialView("_WndConfigureCompliance", ObVm);
        }


        public JsonResult SurveyList(Int64 ContractId)
        {

            var SurveyList = _ObjSurveyService.FindContractSurveysAll(ContractId).Select(x => new { SurveyName = x.SurveyName, SurveyId = x.SurveyId });
            return Json(SurveyList, JsonRequestBehavior.AllowGet);

        }


        public JsonResult NCPSurveyList(Int64 ContractId, int Year, string Quater)
        {
            if (Year == 0 && Quater == "0")
            {
                var SurveyList = _ObjSurveyService.FindContractNcpSurveysAll(0).Select(x => new { SurveyName = x.SurveyName, SurveyId = x.SurveyId });
                return Json(SurveyList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var SurveyList = _ObjSurveyService.FindContractNcpSurveysAll(ContractId).Where(x => x.Quater == Quater && x.Year == Year.ToString())
                    .Select(x => new { SurveyName = x.SurveyName, SurveyId = x.SurveyId });
                return Json(SurveyList, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult QuestionList(Int64? SurveyId)
        {

            var QuestionListList = _ObjQuestionService.GetSurveyNumericQuestionAll(SurveyId.GetValueOrDefault()).Select(x => new
            {
                QuestionValue = x.QuestionValue,
                QuestionId = x.QuestionId
            });
            return Json(QuestionListList, JsonRequestBehavior.AllowGet);

        }


        public JsonResult SaveContrcatCompliance(NcpComplianceViewModel ObjVm)
        {

            if (ObjVm.IsEstimated)
            {

                if (!ObjVm.EstimatedSurveyId.HasValue)
                {
                    ModelState.AddModelError("", "Please select appropiate survey");
                }

                if (ObjVm.IsEstimateDirectQuestion)
                {
                    if (!ObjVm.EstimatedQuestionId.HasValue)
                    {
                        ModelState.AddModelError("", "Please select appropiate question");
                    }
                }

            }
            else
            {

                if (!ObjVm.ActualQuestionId.HasValue)
                {
                    ModelState.AddModelError("", "Please select appropiate survey");
                }

                if (ObjVm.IsActualDirectQuestion)
                {
                    if (!ObjVm.ActualQuestionId.HasValue)
                    {
                        ModelState.AddModelError("", "Please select appropiate question");
                    }

                }

                if (ObjVm.Year == 0)
                {
                    ModelState.AddModelError("", "Please select appropiate year year");
                }
                if (ObjVm.Quater == "0")
                {
                    ModelState.AddModelError("", "Please select appropiate quater");
                }

            }
            if (!ModelState.IsValid)
            {
                string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                .SelectMany(E => E.Errors)
                .Select(E => E.ErrorMessage)
                .ToArray();

                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);
            }

            if (ObjVm.IsEstimated)
            {
                var ObjEstimatedCopliance = _ObjContractComplianceService.ContractEstimateCompliance(ObjVm.ContractId);

                if (ObjEstimatedCopliance == null)//add
                {
                    ContractCompliance _Obj = new ContractCompliance();
                    _Obj.ContractId = ObjVm.ContractId;
                    _Obj.SurveyId = ObjVm.EstimatedSurveyId.GetValueOrDefault();
                    if (ObjVm.IsEstimateDirectQuestion)
                    {
                        _Obj.QuestionId = ObjVm.EstimatedQuestionId.GetValueOrDefault();
                        _Obj.IsDirectQuestion = true;

                    }
                    else
                    {
                        //enter 
                        //  _Obj.ComplianceFormula = "";  //create  xml and insert it
                        _Obj.IsDirectQuestion = false;  // we can avoid this line of code but to better undrstand we write this line
                    }



                    _Obj.EstimatedValue = true;
                    _Obj.RowStatusId = (int)RowActiveStatus.Active;
                    _Obj.CreatedBy = 1;
                    _Obj.ModifiedBy = 1;
                    _Obj.CreatedOn = DateTime.Now;
                    _Obj.ModifiedOn = DateTime.Now;
                    _Obj.RowGUID = Guid.NewGuid();
                    _ObjContractComplianceService.SaveContractCompliance(_Obj);
                }
                else  //edit
                {
                    ObjEstimatedCopliance.SurveyId = ObjVm.EstimatedSurveyId.GetValueOrDefault();
                    if (ObjVm.IsEstimateDirectQuestion)
                    {
                        ObjEstimatedCopliance.QuestionId = ObjVm.EstimatedQuestionId.GetValueOrDefault();
                        ObjEstimatedCopliance.IsDirectQuestion = true;
                    }
                    else
                    {
                        // ObjEstimatedCopliance.ComplianceFormula = "";
                        ObjEstimatedCopliance.IsDirectQuestion = true;
                    }

                    ObjEstimatedCopliance.ModifiedBy = 1;
                    ObjEstimatedCopliance.ModifiedOn = DateTime.Now;
                    _ObjContractComplianceService.UpdateContractCompliance(ObjEstimatedCopliance);
                }

            }
            else
            {
                var Quater = _ObjQuaterService.GetQuaterWithYearQuaterName(ObjVm.Year, ObjVm.Quater);

                if (Quater == null)
                {
                    return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Please create appropiate Quater" }) }, JsonRequestBehavior.AllowGet);
                }

                var ObjActualCopliance = _ObjContractComplianceService.ContractActualComplianceWithQuater(ObjVm.ContractId, Quater.QuaterId);
                if (ObjActualCopliance == null)//add
                {
                    ContractCompliance _Obj = new ContractCompliance();
                    _Obj.ContractId = ObjVm.ContractId;
                    _Obj.SurveyId = ObjVm.ActualsSurveyId.GetValueOrDefault();
                    if (ObjVm.IsActualDirectQuestion)
                    {
                        _Obj.QuestionId = ObjVm.ActualQuestionId.GetValueOrDefault();
                        _Obj.IsDirectQuestion = true;
                    }
                    else
                    {
                        // _Obj.ComplianceFormula = "";
                        _Obj.IsDirectQuestion = false;
                    }
                    _Obj.QuaterId = Quater.QuaterId;
                    _Obj.ActualValue = true;
                    _Obj.RowStatusId = (int)RowActiveStatus.Active;
                    _Obj.CreatedBy = 1;
                    _Obj.ModifiedBy = 1;
                    _Obj.CreatedOn = DateTime.Now;
                    _Obj.ModifiedOn = DateTime.Now;
                    _Obj.RowGUID = Guid.NewGuid();
                    _ObjContractComplianceService.SaveContractCompliance(_Obj);
                }
                else  //edit
                {
                    ObjActualCopliance.SurveyId = ObjVm.ActualsSurveyId.GetValueOrDefault();
                    if (ObjVm.IsEstimateDirectQuestion)
                    {
                        ObjActualCopliance.QuestionId = ObjVm.ActualQuestionId.GetValueOrDefault();
                        ObjActualCopliance.IsDirectQuestion = true;
                    }
                    else
                    {
                        //  ObjActualCopliance.ComplianceFormula = "";
                        ObjActualCopliance.IsDirectQuestion = false;
                    }
                    ObjActualCopliance.ModifiedBy = 1;
                    ObjActualCopliance.ModifiedOn = DateTime.Now;

                    _ObjContractComplianceService.UpdateContractCompliance(ObjActualCopliance);
                }
            }


            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region overidde compliance
        public ActionResult LoadWndOvverideConfigureCompliance(Int64 ContractId, int Flag)
        {
            var BuilderList = _ObjContractService.GetAssociateBuilderWithContract(ContractId);
            OverrideComplianceViewModel ObjVm = new OverrideComplianceViewModel();

            if (Flag == 0)
            {
                ObjVm.ContractId = ContractId;
                ObjVm.Flag = 0;
                var BuilderComplianceList = _ObjContractComplianceService.GetContractBuilderComplianceAll(ContractId);
                ObjVm.ContractBuilderComplianceList = new List<BuilderComplianceFactorViewModel>();
                foreach (var Item in BuilderComplianceList)
                {
                    decimal[] OrginalValue = _ObjContractComplianceService.GetBuilderComplianceFactor(ContractId, Item.BuilderId, false);

                    ObjVm.ContractBuilderComplianceList.Add(new BuilderComplianceFactorViewModel
                    {
                        BuilderId = Item.BuilderId,
                        NewActualValue = Item.ActualValue,
                        NewlEstilamteValue = Item.EstimatedValue,
                        OrginalEstilamteValue = OrginalValue[0],
                        OrginalActualValue = OrginalValue[1]
                    });
                }
                // ObjVm.ContractBuilderComplianceList = BuilderComplianceList.Select(x =>
                // new BuilderComplianceFactorViewModel { BuilderId = x.BuilderId, NewActualValue = x.ActualValue, NewlEstilamteValue = x.EstimatedValue,
                // OrginalEstilamteValue=_ObjContractComplianceService.GetBuilderComplianceFactor(,OrginalActualValue

                // })
                //.ToList();
            }
            else
            {
                ObjVm.Flag = 1;
            }
            ObjVm.BuilderList = BuilderList.Select(x => new SelectListItem { Text = x.BuilderName, Value = Convert.ToString(x.BuilderId) }).ToList();
            ObjVm.BuilderList.Insert(0, new SelectListItem { Text = "--Select Builder--", Value = "0" });
            return PartialView("_WndOvverideConfigureCompliance", ObjVm);
        }
        public JsonResult AddNewRow(Int64 ContractId, int Count)
        {
            var BuilderList = _ObjContractService.GetAssociateBuilderWithContract(ContractId);
            OverrideComplianceViewModel ObjVm = new OverrideComplianceViewModel();
            ObjVm.Flag = 1;
            ObjVm.Count = Count;

            ObjVm.BuilderList = BuilderList.Select(x => new SelectListItem { Text = x.BuilderName, Value = Convert.ToString(x.BuilderId) }).ToList();
            ObjVm.BuilderList.Insert(0, new SelectListItem { Text = "--Select Builder--", Value = "0" });
            string PrtialViewString = PartialView("_WndOvverideConfigureCompliance", ObjVm).RenderToString();
            return Json(new { IsSuccess = true, PartialView = PrtialViewString }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveOverrideCompliance(OverrideComplianceViewModel ObjVm)
        {
            int Count = ObjVm.ContractBuilderComplianceList.Where(x => x.NewActualValue.ToString() == "" || x.NewlEstilamteValue.ToString() == "").Count();
            if (Count > 0)
            {
                ModelState.AddModelError("", "Please give proper input");
            }
            var GroupCount = ObjVm.ContractBuilderComplianceList.GroupBy(x => x.BuilderId).Select(y => new { BuilderCount = y.Count() });
            foreach (var Item in GroupCount)
            {
                if (Item.BuilderCount > 1)
                {
                    ModelState.AddModelError("", "Can't configure diffarent complince for same builder");
                    break;
                }
            }
            if (!ModelState.IsValid)
            {
                string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                .SelectMany(E => E.Errors)
                .Select(E => E.ErrorMessage)
                .ToArray();
                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);
            }

            List<ContractComplianceBuilder> InsertList = new List<ContractComplianceBuilder>();
            List<ContractComplianceBuilder> UpdateList = new List<ContractComplianceBuilder>();

            foreach (var Item in ObjVm.ContractBuilderComplianceList)
            {
                ContractComplianceBuilder ObjContractCompliance = _ObjContractComplianceService.GetContractBuilderCompliance(ObjVm.ContractId, Item.BuilderId);
                if (ObjContractCompliance == null)
                {
                    InsertList.Add(new ContractComplianceBuilder
                    {
                        EstimatedValue = Item.NewlEstilamteValue,
                        ActualValue = Item.NewActualValue,
                        BuilderId = Item.BuilderId,
                        ContractId = ObjVm.ContractId

                    });
                }
                else
                {
                    ObjContractCompliance.EstimatedValue = Item.NewlEstilamteValue;
                    ObjContractCompliance.ActualValue = Item.NewActualValue;
                    ObjContractCompliance.BuilderId = Item.BuilderId;
                    ObjContractCompliance.ContractId = ObjVm.ContractId;
                }
            }
            _ObjContractComplianceService.SaveContractComplianceBuilder(InsertList, UpdateList);

            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetBuilderActualComplianceFactor(Int64 ContractId, Int64 BuilderId)
        {
            decimal[] OrginalValue = _ObjContractComplianceService.GetBuilderComplianceFactor(ContractId, BuilderId, false);
            return Json(new
            {
                IsSuccess = true,
                OrginalEstilamteValue = OrginalValue[0],
                OrginalActualValue = OrginalValue[1]
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region create compliance report
        public ActionResult CreateRebateReport(Int64? SurveyId)
        {
            TempData["IsNcpSuvey"] = true;
            if (SurveyId.HasValue)
            {
                return RedirectToAction("SurveyDetails", "Survey", new { Area = "Admin", SurveyId = SurveyId.GetValueOrDefault(), IsNcpId = 1 });
            }
            else
            {
                return RedirectToAction("SurveyDetails", "Survey", new { Area = "Admin", SurveyId = 0, IsNcpId = 1 });
            }

        }

        #endregion


        #region CBUSAVolumeFess & Construct Formula        

        public ActionResult ConstructFormula()
        {
            bool IsNcp = true;
            ViewBag.IsNcpSurvey = true;
            ConstructFormulaViewModel ObjConstructFormula = new ConstructFormulaViewModel();
            ObjConstructFormula.IsNcpSurvey = IsNcp;
            ObjConstructFormula.QuestionId = 0;
            ObjConstructFormula.QuestionColumn = "";
            ObjConstructFormula.QuestionColumnValue = "";
            ObjConstructFormula.Quarter = "";
            ObjConstructFormula.Year = "";
            if (IsNcp)
            {
                var YearListItems = new List<SelectListItem> {
                    new SelectListItem{Text="--Select Year--", Value="0"},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year-1), Value=Convert.ToString(DateTime.Now.Year-1)},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year), Value=Convert.ToString(DateTime.Now.Year)},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year+1), Value=Convert.ToString(DateTime.Now.Year+1)},
                    };
                List<dynamic> QuarterListItems = new List<dynamic>();
                ObjConstructFormula.QuarterList = new List<SelectListItem>();
                ObjConstructFormula.QuarterList.Add(new SelectListItem { Text = "--Select Quarter--", Value = "0" });
                foreach (var Item in YearListItems)
                {
                    if (Convert.ToInt16(Item.Value) > 0)
                    {
                        var TmpData = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
                        {
                            Value = x + " - " + Item.Value,
                            Text = x + " - " + Item.Text
                        }).ToList();
                        ObjConstructFormula.QuarterList.AddRange(TmpData);
                        //QuarterListItems.Add(TmpData);
                    }
                }
                ObjConstructFormula.YearList = YearListItems;

                //ObjConstructFormula.QuarterList = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
                //{
                //    Value = x,
                //    Text = x
                //}).ToList();

                //ObjConstructFormula.QuarterList.Insert(0, new SelectListItem { Text = "--Select Quarter--", Value = "0" });


                ObjConstructFormula.QuestionList = _ObjQuestionService.GetSurveyQuestionAll(0).Select(x => new SelectListItem
                {
                    Value = x.QuestionId.ToString(),
                    Text = x.QuestionValue
                }).ToList();
                ObjConstructFormula.QuestionList.Insert(0, new SelectListItem { Text = "--Select Question--", Value = "0" });
                //ObjConstructFormula.QuestionList = new List<SelectListItem>();
                ObjConstructFormula.QuestionColumnList = new List<SelectListItem>();
                ObjConstructFormula.QuestionColumnValueList = new List<SelectListItem>();
                ObjConstructFormula.FormulaList = new List<SelectListItem>();
            }
            else
            {
                ObjConstructFormula.QuarterList = new List<SelectListItem>();
                ObjConstructFormula.YearList = new List<SelectListItem>();
                ObjConstructFormula.QuestionList = new List<SelectListItem>();
                ObjConstructFormula.QuestionColumnList = new List<SelectListItem>();
                ObjConstructFormula.QuestionColumnValueList = new List<SelectListItem>();
                ObjConstructFormula.FormulaList = new List<SelectListItem>();
            }

            ViewBag.IsValidationSummaryMsgAvail = 0;

            return View(ObjConstructFormula);
        }
        public JsonResult GetQuestionListForFormula(Int64? SurveyId, string QuarterName)
        {
            var QuestionList = _ObjQuestionService.GetSurveyQuestionAll(SurveyId.GetValueOrDefault()).Select(x => new
            {
                QuestionValue = x.QuestionValue,
                QuestionId = x.QuestionId
            }).Where(y => y.QuestionValue != null).OrderBy(z => z.QuestionId);

            return Json(QuestionList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuestionTypeId(Int64? SurveyId, Int64 QuestionId)
        {
            var QuestionType = _ObjQuestionService.GetSurveyQuestionAll(SurveyId.HasValue ? SurveyId.Value : 0).Where(a => a.QuestionId == QuestionId).Select(x => x.QuestionTypeId);
            return Json(QuestionType, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ConstructFormulaEdit(Int64 ConstructFormulaId)
        {
            bool IsNcp = true;
            ViewBag.IsNcpSurvey = true;
            ConstructFormulaViewModel FormulaVm = new ConstructFormulaViewModel();
            FormulaVm.IsNcpSurvey = IsNcp;
            ViewBag.IsEdit = true;
            if (ConstructFormulaId > 0)
            {


                ConstructFormula ObjModel = _ConstructFormulaService.GetConstructFormulaById(ConstructFormulaId);

                if (ObjModel != null)
                {
                    //ConstructFormulaViewModel FormulaVm = new ConstructFormulaViewModel();
                    var ObjMarketList = _ConstructFormulaService.GetConstructFormulaMarketByFormulaId(ObjModel.ConstructFormulaId);
                    FormulaVm.MarketList = "";
                    foreach (var ItemMarket in ObjMarketList)
                    {
                        FormulaVm.MarketList = FormulaVm.MarketList.Length > 0 ? FormulaVm.MarketList + "," + ItemMarket.MarketId.ToString() : ItemMarket.MarketId.ToString();
                    }

                    FormulaVm.ConstructFormulaId = ObjModel.ConstructFormulaId;
                    FormulaVm.SurveyId = ObjModel.SurveyId;
                    FormulaVm.SurveyName = ObjModel.Survey.SurveyName;
                    FormulaVm.ContractId = ObjModel.ContractId;
                    FormulaVm.ContractName = ObjModel.Contract.ContractName;
                    FormulaVm.Quarter = ObjModel.Quarter; //  + "-" + ObjModel.Year;
                    FormulaVm.Year = ObjModel.Year;
                    //  FormulaVm.QuestionId = ObjModel.QuestionId;
                    FormulaVm.QuestionColumn = ObjModel.QuestionColumnId.ToString();
                    FormulaVm.QuestionColumnValue = ObjModel.QuestionColumnValueId.ToString();
                    //  FormulaVm.MarketId = ObjModel.MarketId;
                    // FormulaVm.MarketList = ObjModel.MarketId.ToString();
                    //FormulaVm.MarketName = ObjModel.Market.MarketName;
                    FormulaVm.FormulaBuild = ObjModel.FormulaBuild;
                    FormulaVm.Formula = ObjModel.FormulaDescription;

                    ViewBag.SurveyId = FormulaVm.SurveyId;
                    ViewBag.Quarter = FormulaVm.Quarter;
                    ViewBag.Year = FormulaVm.Year;
                    ViewBag.QuestionId = FormulaVm.QuestionId;
                    ViewBag.ContractId = FormulaVm.ContractId;
                    ViewBag.FormulaDesc = FormulaVm.Formula;
                    ViewBag.FormulaBuild = FormulaVm.FormulaBuild;
                    ViewBag.MarketList = FormulaVm.MarketList;
                }

                var YearListItems = new List<SelectListItem> {
                    new SelectListItem{Text="--Select Year--", Value = "0"},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year-1), Value=Convert.ToString(DateTime.Now.Year-1)},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year), Value=Convert.ToString(DateTime.Now.Year)},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year+1), Value=Convert.ToString(DateTime.Now.Year+1)},
                    };
                List<dynamic> QuarterListItems = new List<dynamic>();
                FormulaVm.QuarterList = new List<SelectListItem>();
                FormulaVm.QuarterList.Add(new SelectListItem { Text = "--Select Quarter--", Value = "0" });
                foreach (var Item in YearListItems)
                {
                    if (Convert.ToInt16(Item.Value) > 0)
                    {
                        var TmpData = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
                        {
                            Value = x + "-" + Item.Value,
                            Text = x + "-" + Item.Text,
                            Selected = (x + "-" + Item.Value == FormulaVm.Quarter + "-" + FormulaVm.Year ? true : false)
                        }).ToList();

                        FormulaVm.QuarterList.AddRange(TmpData);
                    }
                }

                FormulaVm.YearList = YearListItems;

                FormulaVm.QuestionList = _ObjQuestionService.GetSurveyQuestionAll(0).Select(x => new SelectListItem
                {
                    Value = x.QuestionId.ToString(),
                    Text = x.QuestionValue
                }).ToList();
                FormulaVm.QuestionList.Insert(0, new SelectListItem { Text = "--Select Question--", Value = "0" });

                FormulaVm.QuestionColumnList = new List<SelectListItem>();
                FormulaVm.QuestionColumnValueList = new List<SelectListItem>();
                FormulaVm.FormulaList = new List<SelectListItem>();
            }
            else
            {
                FormulaVm.QuarterList = new List<SelectListItem>();
                FormulaVm.YearList = new List<SelectListItem>();
                FormulaVm.QuestionList = new List<SelectListItem>();
                FormulaVm.QuestionColumnList = new List<SelectListItem>();
                FormulaVm.QuestionColumnValueList = new List<SelectListItem>();
                FormulaVm.FormulaList = new List<SelectListItem>();
            }

            return View("ConstructFormula", FormulaVm);
        }
        public JsonResult GetSurveyByContract(Int64? ContractId, string Quarter, string Year)
        {
            var Survey = _ObjSurveyService.FindContractNcpSurveysAllByQuarter(ContractId.GetValueOrDefault(), Quarter, Year).Select(x => new
            {
                SurveyName = x.SurveyName,
                SurveyId = x.SurveyId
            });
            string SurveyId = Survey.Select(a => a.SurveyId).FirstOrDefault().ToString();
            return Json(SurveyId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSurveyByConstructFormula(Int64? ContractId, string Quarter, string Year)
        {
            var Survey = _ConstructFormulaService.GetAllFormula().Where(x => x.ContractId == ContractId.GetValueOrDefault() && x.Quarter == Quarter && x.Year == Year).Select(y => y.SurveyId).Distinct().FirstOrDefault();
            //.FindContractNcpSurveysAllByQuarter(ContractId.GetValueOrDefault(), Quarter, Year).Select(x => new

            string SurveyId = Survey.ToString();
            return Json(SurveyId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuestionList(Int64? SurveyId, string QuarterName)
        {
            var Question = _ObjQuestionService.GetSurveyQuestionAll(SurveyId.GetValueOrDefault()).Select(x => new
            {
                QuestionValue = x.QuestionValue,
                QuestionId = x.QuestionId
            }).Where(y => y.QuestionValue != null).OrderBy(z => z.QuestionId);

            return Json(Question, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuestionColumnList(Int64? SurveyId, Int64? QuestionId)
        {
            // to check if the selected question is for a text box or dropdown or grid. and to return value acordingly
            if (QuestionId.GetValueOrDefault() > 0 && SurveyId.GetValueOrDefault() > 0)
            {
                var QuestionType = _ObjQuestionService.GetQuestion(QuestionId.GetValueOrDefault()).QuestionTypeId;

                if (QuestionType == 3)  // grid type question
                {
                    var QuestionColumn = _ObjQuestionService.GetQuestionByQuestionType(QuestionId.GetValueOrDefault(), QuestionType, null, true).Select(x => new
                    {
                        QuestionColumnText = x.ColoumnHeaderValue,
                        QuestionColumnId = x.QuestionGridSettingHeaderId + "_" + x.IndexNumber,
                        ColumnIndexNumber = x.IndexNumber
                    }).Where(y => y.QuestionColumnText != null).OrderBy(z => z.QuestionColumnId);
                    return Json(QuestionColumn, JsonRequestBehavior.AllowGet);
                }
                else if (QuestionType == 2)  // text box type does not require column list
                {
                    List<ConstructFormulaViewModel> StaticQuestionColumn = new List<ConstructFormulaViewModel>();

                    StaticQuestionColumn.Add(new ConstructFormulaViewModel { QuestionColumnText = "N/A", QuestionColumnId = "0_0" });

                    return Json(StaticQuestionColumn, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //List<ConstructFormulaViewModel> StaticQuestionColumn = new List<ConstructFormulaViewModel>();

                //StaticQuestionColumn.Add(new ConstructFormulaViewModel { QuestionColumnText = "Column 1", QuestionColumnId = "0_0" });

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetQuestionColumnValueList(Int64 SurveyId, Int64? QuestionId, Int64? QuestionSettingId)
        {
            var QuestionType = _ObjQuestionService.GetQuestion(QuestionId.GetValueOrDefault()).QuestionTypeId;
            if (QuestionType == 2)
            {
                var QuestionColumnValue = _ObjQuestionService.GetQuestionByQuestionType(QuestionId.GetValueOrDefault(), QuestionType, null, false).Select((x, Index) => new
                {
                    QuestionColumnValueText = x.Value == null ? "Value " + Index : x.Value == " " ? "Value " + Index : x.Value,
                    QuestionColumnValueId = x.QuestionDropdownSettingId + "_" + Index,
                    QuestionColumnHeaverValue = x.Value,
                    RowIndexNumber = Index
                }).OrderBy(z => z.QuestionColumnValueId);
                return Json(QuestionColumnValue, JsonRequestBehavior.AllowGet);
            }
            else if (QuestionType == 3)
            {
                var GridDropListOptions = _ObjQuestionService.GetQuestionByQuestionType(QuestionId.GetValueOrDefault(), QuestionType, QuestionSettingId.GetValueOrDefault(), true).Select(x => x.DropdownTypeOptionValue);
                List<ConstructFormulaViewModel> QuestionColumnValue = new List<ConstructFormulaViewModel>();
                int Ctr = 1;
                string GridDropListItems;
                foreach (var Item in GridDropListOptions)
                {
                    if (Item != null && Item != "")
                    {
                        GridDropListItems = Item;
                        string[] SplitedOptionList = GridDropListItems.Split(',');
                        for (Ctr = 0; Ctr < SplitedOptionList.Length; Ctr++)
                        {
                            QuestionColumnValue.Add(new ConstructFormulaViewModel
                            {
                                QuestionColumnValueText = SplitedOptionList[Ctr],
                                QuestionColumnValueId = Ctr.ToString(),
                                QuestionColumnHeaverValue = SplitedOptionList[Ctr],
                                RowIndexNumber = Ctr
                            });
                        }
                    }
                }
                return Json(QuestionColumnValue, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetFormulaByQuarter(Int64? ContractId, string Quarter, string Year, string CopyToQuarter, string CopyToYear)
        {
            var Formula = _ConstructFormulaService.GetFormulaByFilters(ContractId.GetValueOrDefault(), Quarter, Year, null).Select(x => new
            {
                FormulaDescription = x.FormulaDescription,
                FormulaBuild = x.FormulaBuild
            });
            string FormulaDesc = Formula.Select(a => a.FormulaDescription).FirstOrDefault().ToString();
            string FormulaActual = Formula.Select(a => a.FormulaBuild).FirstOrDefault().ToString();
            // - have to take care by spliting each question and its properties like questiontype, question column alue, question column value to match exact with current quater questions.
            //var TargetQuarterYearQuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId.GetValueOrDefault(), CopyToQuarter, CopyToYear).Where(x=> x.QuestionValue ;
            //foreach(var Item in TargetQuarterYearQuestionList)
            //{
            //    if(FormulaDesc)
            //}

            return Json(FormulaDesc + "~" + FormulaActual, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ParseMarketControl(List<Int64> MarketHistory, int Flag)
        {
            var RemoveList = MarketHistory;

            if (MarketHistory.Count <= 1)
            {
                var ObjMarket = _ObjMarketService.GetMarket();
                MarketControlVM ObjVm = new MarketControlVM();
                ObjVm.ObjMarket = ObjMarket.OrderBy(x => x.MarketName).ToList();
                ObjVm.Flag = Flag;
                return Json(new { IsAppendRequired = true, MarketCustomControl = PartialView("_MarketCustomControlPopup", ObjVm).RenderToString(), RemoveList = RemoveList }
                    , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { IsAppendRequired = false, RemoveList = RemoveList }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ShowMarketPopup(string ControlValue)
        {
            return PartialView("_MarketCustomControlPopup", ControlValue);
        }
        public JsonResult BuildConstructFormula(string FormulaStringToBuild, int SelectedQuestionTypeId, Int64 SelectedQuestionId, Int64 SelectedQuestionColumnId, Int64 SelectedQuestionColumnValueId, int SelectedRowIndex, int SelectedColumnIndex)
        {
            string CreatedFormula = "";
            //if (SelectedQuestionTypeId == 3)
            //{

            //}
            //if (SelectedQuestionTypeId == 2)
            //{

            //}
            //if (SelectedQuestionTypeId == 1)
            //{

            //}

            return Json(new { ReturnedFormula = CreatedFormula }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveConstructFormula(ConstructFormulaViewModel ObjVm)
        {

            // return;

            bool Success = false;
            bool IsModelValid = true;
            int QuestionType = 0;
            if (ObjVm.IsNcpSurvey)
            {
                if (ObjVm.ContractId == 0)
                {
                    ModelState.AddModelError("", "Please select appropriate contract");
                    IsModelValid = false;
                }
                if (ObjVm.SurveyId == 0)
                {
                    ModelState.AddModelError("", "Please select appropriate contract");
                    IsModelValid = false;
                }
                if (ObjVm.QuestionId == 0)
                {
                    ModelState.AddModelError("", "Please select appropriate question");
                    IsModelValid = false;
                }


                if (!ObjVm.Quarter.HasValue())
                {
                    ModelState.AddModelError("", "Please select appropriate quarter");
                    IsModelValid = false;
                }

                if (ObjVm.Quarter.HasValue())
                {
                    if (ObjVm.Quarter == "0")
                    {
                        ModelState.AddModelError("", "Please select appropriate quarter");
                        IsModelValid = false;
                    }

                }
                //if (!ObjVm.QuestionColumnValue.HasValue())
                //{
                //    ModelState.AddModelError("", "Please select appropriate question column value");
                //    IsModelValid = false;
                //}
                if (ObjVm.MarketList == null)
                {
                    ModelState.AddModelError("", "Please select appropriate market(s)");
                    IsModelValid = false;
                }
                if (ObjVm.MarketList != null)
                {
                    string[] MarketId = ObjVm.MarketList.Split(',');
                    if (MarketId.Length == 0)
                    {
                        ModelState.AddModelError("", "Please select appropriate market(s)");
                        IsModelValid = false;
                    }
                    else
                    {
                        if (ObjVm.ConstructFormulaId == 0) //for Add
                        {
                            List<Market> AllreadyCreatedMarket = _ConstructFormulaService.GetAllreadyBuildFormulaMarket(ObjVm.ContractId, ObjVm.Year, ObjVm.Quarter);
                            List<Int64> MarketNeedToCreate = null;
                            if (ObjVm.MarketList.Substring(ObjVm.MarketList.Length - 1, 1) == ",")
                            {
                                MarketNeedToCreate = ObjVm.MarketList.Substring(0, ObjVm.MarketList.Length - 1).Split(',').Select(x => Int64.Parse(x)).ToList();
                            }
                            else
                            {
                                MarketNeedToCreate = ObjVm.MarketList.Substring(0, ObjVm.MarketList.Length).Trim().Split(',').Select(x => Int64.Parse(x)).ToList();
                            }

                            List<Market> CommonMarketId = AllreadyCreatedMarket.Where(x => MarketNeedToCreate.Contains(x.MarketId)).ToList();

                            if (CommonMarketId.Count > 0)
                            {
                                string MarketName = string.Join(",", CommonMarketId.Select(x => x.MarketName).ToList());
                                ModelState.AddModelError("", "You have already created formula for " + MarketName);
                                IsModelValid = false;
                            }
                        }
                        else
                        {
                            List<Market> AllreadyCreatedMarketForFormula = _ConstructFormulaService.GetAllreadyBuildFormulaMarket(ObjVm.ContractId, ObjVm.Year, ObjVm.Quarter, ObjVm.ConstructFormulaId);
                            List<Int64> MarketNeedToUpdate = null;
                            if (ObjVm.MarketList.Substring(ObjVm.MarketList.Length - 1, 1) == ",")
                            {
                                MarketNeedToUpdate = ObjVm.MarketList.Substring(0, ObjVm.MarketList.Length - 1).Split(',').Select(x => Int64.Parse(x)).ToList();
                            }
                            else
                            {
                                MarketNeedToUpdate = ObjVm.MarketList.Substring(0, ObjVm.MarketList.Length).Trim().Split(',').Select(x => Int64.Parse(x)).ToList();
                            }

                            var UnCommonMarketIdList = MarketNeedToUpdate.Where(x => !AllreadyCreatedMarketForFormula.Select(y => y.MarketId).Contains(x)).ToList();

                            if (UnCommonMarketIdList.Count > 0)
                            {

                                List<Market> AllMarketofContract = _ConstructFormulaService.GetAllreadyBuildFormulaMarket(ObjVm.ContractId, ObjVm.Year, ObjVm.Quarter);
                                List<Market> CommonMarketIdList = AllMarketofContract.Where(x => UnCommonMarketIdList.Contains(x.MarketId)).ToList();
                                if (CommonMarketIdList.Count > 0)
                                {
                                    string MarketName = string.Join(",", CommonMarketIdList.Select(x => x.MarketName).ToList());
                                    ModelState.AddModelError("", "You have already created formula for " + MarketName);
                                    IsModelValid = false;
                                }

                            }

                        }
                    }
                }
            }

            if (!IsModelValid)
            {
                string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                .SelectMany(E => E.Errors)
                .Select(E => E.ErrorMessage)
                .ToArray();
                Success = false;
                return Json(new { IsSuccess = Success, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    Int64[] MarketId;
                    if (ObjVm.MarketList.Substring(ObjVm.MarketList.Length - 1, 1) == ",")
                    {
                        MarketId = ObjVm.MarketList.Substring(0, ObjVm.MarketList.Length - 1).Split(',').Select(x => Int64.Parse(x)).ToArray();
                    }
                    else
                    {
                        MarketId = ObjVm.MarketList.Substring(0, ObjVm.MarketList.Length).Trim().Split(',').Select(x => Int64.Parse(x)).ToArray();
                    }
                    /*  for (int Ctr = 0; Ctr < MarketId.Length; Ctr++)  //close by Rabi
                      {
                          if (MarketId[Ctr].Length > 0)
                          {
                              if (Convert.ToInt64(MarketId[Ctr]) > 0)
                              {*/
                    string CreatedFormula = ObjVm.Formula.Replace('[', '(').Replace(']', ')').Replace('{', '(').Replace('}', ')').Replace("~~~~", "[").Replace("~~##", "]").Replace("~@$", "_").Replace("( ", "(").Replace(" )", ")"); // "~This is a text question created to test the enhanced module of CBUSA NCP. Please treat it as a test question#.#Row 1~"; // 
                    CreatedFormula = CreatedFormula.Replace("~~~~", "[");
                    string[] SelectedGuestionList = CreatedFormula.Split('~');
                    //string [] SelectedColumnValue = "";
                    string FormulaActual = CreatedFormula;
                    var QuestionAll = _ObjQuestionService.GetSurveyQuestionAll(ObjVm.SurveyId).Select(x => new
                    {
                        QuestionValue = x.QuestionValue,
                        QuestionId = x.QuestionId
                    }).Where(y => y.QuestionValue != null).OrderBy(z => z.QuestionId);
                    string QuestionText = "";
                    string QuestionId = "";
                    string QuestionColumnText = "";
                    string QuestionColumnId = "";
                    string QuestionColumnValueText = "";
                    string QuestionColumnValueId = "";
                    bool GridDropDownReplaced = false;
                    foreach (var Item in QuestionAll)
                    {
                        QuestionText = Item.QuestionValue;
                        QuestionId = Item.QuestionId.ToString();

                        QuestionType = _ObjQuestionService.GetQuestion(Item.QuestionId).QuestionTypeId;
                        ObjVm.QuestionTypeId = QuestionType;

                        if (QuestionType == 3)  // grid type question
                        {
                            // get column value and replace text to question header setting id
                            var QuestionColumn = _ObjQuestionService.GetQuestionByQuestionType(Item.QuestionId, QuestionType, null, true).Select(x => new
                            {
                                QuestionColumnText = QuestionText + "_" + x.ColoumnHeaderValue,
                                QuestionColumnId = QuestionId + "_" + x.QuestionGridSettingHeaderId + "_" + (x.IndexNumber - 1),
                                ColumnIndexNumber = x.IndexNumber,
                                QuestionSettingId = x.QuestionGridSettingHeaderId,
                                GridControlType = x.ControlType,
                                CheckRowOrColumn = x.ColoumnHeaderValue
                            }).Where(y => y.CheckRowOrColumn != null).OrderBy(z => z.QuestionColumnId);
                            foreach (var GridQuestion in QuestionColumn)
                            {
                                QuestionColumnText = GridQuestion.QuestionColumnText;
                                QuestionColumnId = GridQuestion.QuestionColumnId;
                                Int64 QuestionGridHeaderId = 0; // GridQuestion.QuestionSettingId;
                                if (GridQuestion.QuestionSettingId > 0)
                                {
                                    QuestionGridHeaderId = GridQuestion.QuestionSettingId;
                                }
                                // get drop down list of grid and replace text to id
                                if (GridQuestion.GridControlType == "2")
                                {
                                    var GridDropListOptions = _ObjQuestionService.GetQuestionByQuestionType(Convert.ToInt64(QuestionId), QuestionType, QuestionGridHeaderId, true).Select(x => x.DropdownTypeOptionValue);
                                    List<ConstructFormulaViewModel> QuestionColumnValue = new List<ConstructFormulaViewModel>();
                                    string GridDropListItems;
                                    foreach (var OptionItem in GridDropListOptions)
                                    {
                                        if (Item != null && OptionItem != "")
                                        {
                                            GridDropListItems = OptionItem;
                                            if (GridDropListItems != null)
                                            {
                                                string[] SplitedOptionList = GridDropListItems.Split(',');
                                                if (SplitedOptionList != null)
                                                {
                                                    for (int Ctrl = 0; Ctrl < SplitedOptionList.Length; Ctrl++)
                                                    {
                                                        QuestionColumnValueText = QuestionColumnText + "_" + SplitedOptionList[Ctrl];
                                                        //QuestionColumnValueId = QuestionColumnId + "_" + SplitedOptionList[Ctrl].Trim();
                                                        QuestionColumnValueId = QuestionColumnId + "=#" + SplitedOptionList[Ctrl].Trim() + "~#";
                                                        FormulaActual = FormulaActual.Replace(QuestionColumnValueText, QuestionColumnValueId);
                                                        //FormulaActual = FormulaActual.Replace(" ", "");
                                                        GridDropDownReplaced = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (GridQuestion.GridControlType == "1")
                                {
                                    FormulaActual = FormulaActual.Replace(QuestionColumnText, QuestionColumnId);
                                }
                            }
                        }
                        else if (QuestionType == 2)  // droplist type question
                        {
                            FormulaActual = FormulaActual.Replace(QuestionText, QuestionId);
                            var QuestionColumnValue = _ObjQuestionService.GetQuestionByQuestionType(Convert.ToInt64(QuestionId), QuestionType, null, false).Select((x, Index) => new
                            {
                                QuestionColumnValueText = "_" + x.Value,
                                QuestionColumnValueId = x.Value, // QuestionId + "_" + x.QuestionDropdownSettingId + "_" + Index,
                                QuestionColumnHeaverValue = x.Value,
                                RowIndexNumber = Index
                            }).OrderBy(z => z.QuestionColumnValueId);
                            foreach (var DropListItem in QuestionColumnValue)
                            {
                                QuestionColumnValueText = DropListItem.QuestionColumnValueText;
                                //QuestionColumnValueId = DropListItem.QuestionColumnValueId;
                                QuestionColumnValueId = "=#" + DropListItem.QuestionColumnValueId + "~#"; // DropListItem.QuestionColumnValueId;
                                FormulaActual = FormulaActual.Replace(QuestionColumnValueText, QuestionColumnValueId);
                                //FormulaActual = FormulaActual.Replace(QuestionText, "=#" + QuestionId + "~#");
                            }
                        }
                        else if (QuestionType == 1)  // Text type question
                        {
                            FormulaActual = FormulaActual.Replace(QuestionText, QuestionId);
                        }
                    }
                    int StringFoundInFormula = Regex.Matches(FormulaActual, @"[a-zA-Z]").Count;
                    if (StringFoundInFormula > 0)
                    {
                        StringFoundInFormula = Regex.Matches(FormulaActual, @"^").Count;
                        if (StringFoundInFormula == 0)
                        {
                            ModelState.AddModelError("", "Error in Formula Construction. Please check Formula if there are any conditional value(s) present then Conditiional break character '^' is missing.");
                            IsModelValid = false;
                        }
                    }
                    if (CreatedFormula == FormulaActual)
                    {
                        ModelState.AddModelError("", "Error in Formula Construction. In case of Copy formula from Previous Quarter, Please review the Questions and its properties.");
                        IsModelValid = false;
                    }
                    if (!IsFormulaSyntacticallyCorrect(FormulaActual))
                    {
                        ModelState.AddModelError("", "Error in Formula Construction.");
                        IsModelValid = false;
                    }
                    else
                    {
                        if (!IsFormulaEvalutionCorrect(FormulaActual, ObjVm.SurveyId))
                        {
                            ModelState.AddModelError("", "Error in Formula Construction.");
                            IsModelValid = false;
                        }
                    }
                    //if (ObjVm.ConstructFormulaId >0)
                    //{
                    //   var ExistingTranId = _ConstructFormulaService.GetFormulaByFilters(ObjVm.ContractId, ObjVm.Quarter, ObjVm.Year, Convert.ToInt64(MarketId[Ctr])).Select(x => x.ConstructFormulaId).FirstOrDefault();
                    if (IsModelValid)
                    {
                        if (ObjVm.ConstructFormulaId > 0)
                        {
                            ConstructFormula _Obj = new ConstructFormula();
                            _Obj.ConstructFormulaId = ObjVm.ConstructFormulaId;
                            _Obj.ContractId = ObjVm.ContractId;
                            _Obj.SurveyId = ObjVm.SurveyId;
                            _Obj.Quarter = ObjVm.Quarter;
                            _Obj.Year = ObjVm.Year;
                            _Obj.QuestionColumnId = Convert.ToInt64(ObjVm.QuestionColumn);
                            _Obj.QuestionColumnValueId = Convert.ToInt64(ObjVm.QuestionColumnValue);
                            _Obj.FormulaDescription = ObjVm.Formula;
                            _Obj.FormulaBuild = FormulaActual;
                            _Obj.RowStatusId = (int)RowActiveStatus.Active;
                            _Obj.CreatedBy = 1;
                            _Obj.ModifiedBy = 1;
                            _Obj.CreatedOn = DateTime.Now;
                            _Obj.ModifiedOn = DateTime.Now;
                            _Obj.RowGUID = Guid.NewGuid();
                            _ConstructFormulaService.SaveConstructFormula(_Obj, MarketId);
                        }
                        else
                        {
                            ConstructFormula _Obj = new ConstructFormula();
                            _Obj.ConstructFormulaId = 0;
                            _Obj.ContractId = ObjVm.ContractId;
                            _Obj.SurveyId = ObjVm.SurveyId;
                            _Obj.Quarter = ObjVm.Quarter;
                            _Obj.Year = ObjVm.Year;
                            _Obj.QuestionColumnId = Convert.ToInt64(ObjVm.QuestionColumn);
                            _Obj.QuestionColumnValueId = Convert.ToInt64(ObjVm.QuestionColumnValue);
                            _Obj.FormulaDescription = ObjVm.Formula;
                            _Obj.FormulaBuild = FormulaActual;
                            _Obj.RowStatusId = (int)RowActiveStatus.Active;
                            _Obj.CreatedBy = 1;
                            _Obj.ModifiedBy = 1;
                            _Obj.CreatedOn = DateTime.Now;
                            _Obj.ModifiedOn = DateTime.Now;
                            _Obj.RowGUID = Guid.NewGuid();
                            _ConstructFormulaService.SaveConstructFormula(_Obj, MarketId);
                        }
                    }
                    else
                    {
                        string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                       .SelectMany(E => E.Errors)
                       .Select(E => E.ErrorMessage)
                       .ToArray();
                        Success = false;
                        return Json(new { IsSuccess = Success, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);
                    }
                    /*   }   //close by Rabi
                               }
               }*/
                    Success = true;
                }
                catch (Exception ee)
                {
                    Success = false;
                }
                finally
                {
                    //  _ConstructFormulaService.SaveConstructFormula(new ConstructFormula(), MarketId);
                }
            }
            return Json(new { IsSuccess = Success }, JsonRequestBehavior.AllowGet);
        }

        public bool IsFormulaSyntacticallyCorrect(string Formula)
        {
            try
            {

                // string dd= "Formula"
                string replaced = "";
                Formula = Formula.Replace(" ", "").Replace("/", "").Replace("-", "");
                var pattern = @"\[[A-Za-z0-9_#~=()\\s@$&]+]";
                replaced = Regex.Replace(Formula, pattern, "2");
                replaced = replaced.Replace("2^", "").Replace("%", "");
                double result = Convert.ToDouble(new DataTable().Compute(replaced, null));
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool IsFormulaEvalutionCorrect(string Formula, Int64 SurveyId)
        {
            string DBConString = ConfigurationManager.ConnectionStrings["CBUSA"].ToString();
            SqlConnection DBcon = new SqlConnection(DBConString);
            try
            {


                //DBcon = new SqlConnection(DBConString);

                SqlCommand ComFormulaEvalute = new SqlCommand("Proc_CheckConstructFormulaCreation", DBcon);
                ComFormulaEvalute.Parameters.AddWithValue("@FormulaToCheck", Formula);
                ComFormulaEvalute.Parameters.AddWithValue("@SurveyId", SurveyId);
                ComFormulaEvalute.CommandType = System.Data.CommandType.StoredProcedure;
                ComFormulaEvalute.CommandType = System.Data.CommandType.StoredProcedure;
                if (DBcon.State == ConnectionState.Closed)
                {
                    DBcon.Open();
                }

                int Result = ComFormulaEvalute.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
            finally
            {


                DBcon.Close();
                DBcon.Dispose();
            }




        }

        public ActionResult VolumeFee()
        {
            ConstructFormulaViewModel ObjConstructFormula = new ConstructFormulaViewModel();

            var YearListItems = new List<SelectListItem> {
                new SelectListItem{Text="--Select Year--", Value="0"},
                new SelectListItem{Text=Convert.ToString(DateTime.Now.Year-1), Value=Convert.ToString(DateTime.Now.Year-1)},
                new SelectListItem{Text=Convert.ToString(DateTime.Now.Year), Value=Convert.ToString(DateTime.Now.Year)},
                new SelectListItem{Text=Convert.ToString(DateTime.Now.Year+1), Value=Convert.ToString(DateTime.Now.Year+1)},
                };
            List<dynamic> QuarterListItems = new List<dynamic>();
            ObjConstructFormula.QuarterList = new List<SelectListItem>();
            ObjConstructFormula.QuarterList.Add(new SelectListItem { Text = "--Select Quarter--", Value = "0" });
            foreach (var Item in YearListItems)
            {
                if (Convert.ToInt16(Item.Value) > 0)
                {
                    var QuarterData = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
                    {
                        Value = x + "-" + Item.Value,
                        Text = x + "-" + Item.Text
                    }).ToList();
                    ObjConstructFormula.QuarterList.AddRange(QuarterData);
                    //QuarterListItems.Add(TmpData);
                }
            }
            ObjConstructFormula.YearList = YearListItems;

            //ObjConstructFormula.QuarterList = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
            //{
            //    Value = x,
            //    Text = x
            //}).ToList();

            //ObjConstructFormula.QuarterList.Insert(0, new SelectListItem { Text = "--Select Quarter--", Value = "0" });


            ObjConstructFormula.QuestionList = _ObjQuestionService.GetSurveyQuestionAll(0).Select(x => new SelectListItem
            {
                Value = x.QuestionId.ToString(),
                Text = x.QuestionValue
            }).ToList();
            ObjConstructFormula.QuestionList.Insert(0, new SelectListItem { Text = "--Select Question--", Value = "0" });
            //ObjConstructFormula.QuestionList = new List<SelectListItem>();
            ObjConstructFormula.QuestionColumnList = new List<SelectListItem>();
            ObjConstructFormula.QuestionColumnValueList = new List<SelectListItem>();
            ObjConstructFormula.FormulaList = new List<SelectListItem>();
            ObjConstructFormula.MarketListData = new List<SelectListItem>();
            ObjConstructFormula.MarketListData.Add(new SelectListItem { Text = "--Select Market--", Value = "0" });
            var MarketData = _ObjMarketService.GetMarket().Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new SelectListItem
            {
                Value = x.MarketId.ToString(),
                Text = x.MarketName
            }).OrderBy(x => x.Text).ToList();
            ObjConstructFormula.MarketListData.AddRange(MarketData);

            ViewBag.IsValidationSummaryMsgAvail = 0;

            return View(ObjConstructFormula);
            //return View();
        }


        public JsonResult GetActivePendingContract()
        {

            var ContractList = _ObjContractService.GetActivePendingContractList().Select(x => new
            {
                ContractName = x.ContractName,
                ContractId = x.ContractId
            }).OrderBy(x => x.ContractName);

            return Json(ContractList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConstructFormula(Int64? ContractId, string QuarterName, string Year, string MarketList)
        {

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetConstructFormulaList([DataSourceRequest] DataSourceRequest request, string Type, Int64? ContractId, string QuarterName, string Year, Int64? MarketId, string Flag)
        {
            int RowNo = 2;
            IEnumerable<ConstructFormula> FormulaList = null;
            int TotalSurveyMarketCount = _ObjMarketService.GetMarket().Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Count(); // _ObjSurveyService.GetSurveyMarket(SurveyId).Select(x=> x.MarketId).Distinct().Count();
            IEnumerable<ConstructFormulaViewModel> list;
            if (Flag == null && Type == null)
            {
                FormulaList = _ConstructFormulaService.GetAllFormula().OrderByDescending(x => x.SurveyId).OrderByDescending(y => y.ConstructFormulaId);

            }
            else
            {
                if (Flag == "" || Flag == null)
                {

                    if (Type == "asccon")
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderBy(x => x.Contract.ContractName);
                    }

                    else if (Type == "desccon")
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderByDescending(x => x.Contract.ContractName);
                    }

                    else if (Type == "ascqtr")
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderBy(x => x.Quarter);
                    }
                    else if (Type == "descqtr")
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderByDescending(x => x.Quarter);
                    }
                    else if (Type == "ascyr")
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderBy(x => x.Year);
                    }
                    else if (Type == "descyr")
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderByDescending(x => x.Year);
                    }
                    else if (Type == "ascmkt")
                    {
                        // FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderBy(x => x.Market.MarketName);
                    }
                    else if (Type == "descmkt")
                    {
                        // FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderByDescending(x => x.Market.MarketName);
                    }
                    else if (Type == null)
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderByDescending(x => x.ContractId);
                    }
                }
                else
                {
                    if (Flag == "0")
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderByDescending(x => x.ContractId);

                    }
                    else
                    {
                        FormulaList = _ConstructFormulaService.GetFormulaByFilters(ContractId, QuarterName, Year, MarketId).OrderByDescending(x => x.SurveyId);
                    }


                }

            }
            list = FormulaList.Select(x => new ConstructFormulaViewModel
            {
                ConstructFormulaId = x.ConstructFormulaId,
                SurveyId = x.SurveyId,
                ContractId = x.ContractId,
                ContractName = x.Contract.ContractName,
                Quarter = x.Quarter != null ? x.Quarter : "",
                Year = x.Year != null ? x.Year : "",
                // MarketId = x.MarketId,
                MarketName = x.ConstructFormulaMarket != null ? (x.ConstructFormulaMarket.Count == TotalSurveyMarketCount ? "All Market" : string.Join(",", x.ConstructFormulaMarket.OrderBy(y => y.Market.MarketName).Select(y => y.Market.MarketName))) : "", //VolumeFeeMarketNameList(x.SurveyId, x.ConstructFormulaId), //  x.Market.MarketName,
                SurveyName = x.Survey.SurveyName,
            });
            return Json(list.ToDataSourceResult(request));
        }

        private string VolumeFeeMarketNameList(Int64 SurveyId, Int64 ConstructFormulaId)
        {
            string MarketList = "";
            var MarketData = _ConstructFormulaService.GetConstructFormulaMarketByFormulaId(ConstructFormulaId).OrderBy(x => x.Market.MarketName);
            int SurveyMarketCount = _ObjMarketService.GetMarket().Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Count(); // _ObjSurveyService.GetSurveyMarket(SurveyId).Select(x=> x.MarketId).Distinct().Count();
            int FormulaMarketCount = MarketData.Count();
            foreach (var MktData in MarketData)
            {
                if (FormulaMarketCount >= SurveyMarketCount)
                {
                    MarketList = "All Markets";
                }
                else
                {
                    MarketList = MarketList.Length == 0 ? MktData.Market.MarketName : MarketList + "," + MktData.Market.MarketName;
                }
            }
            return MarketList;
        }
        public ActionResult GetNCPVolumeFeeResult()
        {
            ConstructFormulaViewModel ObjConstructFormula = new ConstructFormulaViewModel();

            var YearListItems = new List<SelectListItem> {
                new SelectListItem{Text="--Select Year--", Value="0"},
                new SelectListItem{Text=Convert.ToString(DateTime.Now.Year-1), Value=Convert.ToString(DateTime.Now.Year-1)},
                new SelectListItem{Text=Convert.ToString(DateTime.Now.Year), Value=Convert.ToString(DateTime.Now.Year)},
                new SelectListItem{Text=Convert.ToString(DateTime.Now.Year+1), Value=Convert.ToString(DateTime.Now.Year+1)},
                };
            List<dynamic> QuarterListItems = new List<dynamic>();
            ObjConstructFormula.QuarterList = new List<SelectListItem>();
            ObjConstructFormula.QuarterList.Add(new SelectListItem { Text = "--Select Quarter--", Value = "0" });
            foreach (var Item in YearListItems)
            {
                if (Convert.ToInt16(Item.Value) > 0)
                {
                    var QuarterData = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
                    {
                        Value = x + "-" + Item.Value,
                        Text = x + "-" + Item.Text
                    }).OrderBy(y => y.Value).ToList();
                    ObjConstructFormula.QuarterList.AddRange(QuarterData);
                    //QuarterListItems.Add(TmpData);
                }
            }
            ObjConstructFormula.YearList = YearListItems;

            //ObjConstructFormula.QuarterList = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
            //{
            //    Value = x,
            //    Text = x
            //}).ToList();

            //ObjConstructFormula.QuarterList.Insert(0, new SelectListItem { Text = "--Select Quarter--", Value = "0" });


            ObjConstructFormula.QuestionList = _ObjQuestionService.GetSurveyQuestionAll(0).Select(x => new SelectListItem
            {
                Value = x.QuestionId.ToString(),
                Text = x.QuestionValue
            }).OrderBy(y => y.Value).ToList();
            ObjConstructFormula.QuestionList.Insert(0, new SelectListItem { Text = "--Select Question--", Value = "0" });
            //ObjConstructFormula.QuestionList = new List<SelectListItem>();
            ObjConstructFormula.QuestionColumnList = new List<SelectListItem>();
            ObjConstructFormula.QuestionColumnValueList = new List<SelectListItem>();
            ObjConstructFormula.FormulaList = new List<SelectListItem>();
            ObjConstructFormula.MarketListData = new List<SelectListItem>();
            ObjConstructFormula.MarketListData.Add(new SelectListItem { Text = "--Select Market--", Value = "0" });
            var MarketData = _ObjMarketService.GetMarket().Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new SelectListItem
            {
                Value = x.MarketId.ToString(),
                Text = x.MarketName
            }).OrderBy(y => y.Text).ToList();
            ObjConstructFormula.MarketListData.AddRange(MarketData);

            ViewBag.IsValidationSummaryMsgAvail = 0;

            return View(ObjConstructFormula);

        }
        public ActionResult DownloadNCPVolumeFeeResult(Int64 SurveyId, Int64? MarketId)
        {
            if (SurveyId <= 0)
                return View();

            var SurveyDetails = _ObjSurveyService.GetSurvey(SurveyId);
            Int64 ContractId = SurveyDetails.ContractId;
            string QuarterName = SurveyDetails.Quater;
            string Year = SurveyDetails.Year;
            Int64 SurvetMarketId = SurveyDetails.SurveyMarket.Select(x => x.MarketId).FirstOrDefault();
            string SelectedMarketId = MarketId.ToString();

            if (!SurveyDetails.IsNcpSurvey)
            {
                return View();
            }
            SqlConnection DBcon;
            string DBConString = ConfigurationManager.ConnectionStrings["CBUSA"].ToString();
            DBcon = new SqlConnection(DBConString);

            SqlCommand ComFormulaEvalute = new SqlCommand("Proc_NcpConstructFormulaEvalute_Cols", DBcon);
            ComFormulaEvalute.Parameters.AddWithValue("@SurveyId", SurveyId);
            ComFormulaEvalute.CommandType = System.Data.CommandType.StoredProcedure;

            SqlCommand ComResult = new SqlCommand("Proc_GetNCPRebateFormulaValueReport", DBcon);
            ComResult.Parameters.AddWithValue("@ContractId", ContractId);
            ComResult.Parameters.AddWithValue("@QUarterName", QuarterName.Length > 0 ? QuarterName : null);
            ComResult.Parameters.AddWithValue("@Year", Year.Length > 0 ? Year : null);
            ComResult.Parameters.AddWithValue("@MarketId", SelectedMarketId == null ? null : SelectedMarketId);
            ComResult.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter Adapter = new SqlDataAdapter(ComResult);
            DataTable DTValue = new DataTable();

            try
            {
                DBcon.Open();
                ComFormulaEvalute.Connection = DBcon;
                int Result = ComFormulaEvalute.ExecuteNonQuery();
                if (Result <= 0)
                {
                    Adapter.Fill(DTValue);

                    ViewBag.ExcelHeader = "NCP Volume Fee Calculation";

                    string attachment = "attachment; filename=NCPVolumeFeeCalculation.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in DTValue.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in DTValue.Rows)
                    {
                        tab = "";
                        for (i = 0; i < DTValue.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();
                }
            }
            catch (Exception ee)
            {
                Response.Write("Error in Formula ..." + ee.Message.ToString());
                //Response.End();
            }
            finally
            {
                ComFormulaEvalute.Dispose();
                ComResult.Dispose();
                Adapter.Dispose();
                DTValue.Clear();
                DBcon.Close();
                DBcon.Dispose();
            }
            return View();
        }
        public JsonResult CalculateVolumeFeeRebate(Int64? ContractId)
        {
            string ReturnMessage = "";
            SqlConnection DBcon;
            string DBConString = ConfigurationManager.ConnectionStrings["CBUSA"].ToString();
            DBcon = new SqlConnection(DBConString);

            SqlCommand ComResult = new SqlCommand("Proc_NCPVolumeFeeRebateCalculation", DBcon);
            ComResult.Parameters.AddWithValue("@InputContractId", ContractId.GetValueOrDefault().ToString());
            ComResult.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter Adapter = new SqlDataAdapter(ComResult);
            DataTable DTValue = new DataTable();

            try
            {
                DBcon.Open();
                
                Adapter.Fill(DTValue);
                if (DTValue.Rows.Count > 0)
                {
                    int i;
                    foreach (DataRow dr in DTValue.Rows)
                    {
                        for (i = 0; i < DTValue.Columns.Count; i++)
                        {
                            ReturnMessage = dr[i].ToString();
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                ReturnMessage = "Error in calculating volume fee ..." + ee.Message.ToString();
                //Response.End();
            }
            finally
            {
                ComResult.Dispose();
                Adapter.Dispose();
                DTValue.Clear();
                DBcon.Close();
                DBcon.Dispose();
            }
            return Json(ReturnMessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReopenBuilderNCPSurveyResponse()
        {
            ReOpenBuilderNCPSurveyResponseViewModel ObjModel = new ReOpenBuilderNCPSurveyResponseViewModel();
            ObjModel.BuilderId = 0;
            ObjModel.BuilderName = "";
            ObjModel.ContractId = 0;
            ObjModel.ContractName = "";
            return View(ObjModel);
        }
        public JsonResult GetBuilders(Int64? ContractId)
        {
            if(ContractId.GetValueOrDefault() >0)
            {
                var BuilderList = _ObjContractService.GetAssociateBuilderWithContract(ContractId.GetValueOrDefault()).Select(x => new
                {
                    BuilderName = x.BuilderName,
                    BuilderId = x.BuilderId
                }).OrderBy(x => x.BuilderName);

                return Json(BuilderList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            
        }
        //public JsonResult GetBuildersNCPSurveyEditStatus(Int64? BuilderId, Int64? ContractId)
        //{
        //    if (ContractId.GetValueOrDefault() > 0 && BuilderId.GetValueOrDefault() >0)
        //    {
        //        var CurrentQuarterId = _ObjQuaterService.GetQuaterByDate(DateTime.Now).Select(x => x.QuaterId).SingleOrDefault();
        //        var CurrentQuarterYear = _ObjQuaterService.GetQuaterByDate(DateTime.Now).Select(x => x.Year).SingleOrDefault();
        //        var PreviousQuarter = _ObjQuaterService.GetNCPReportingQuarterByCurrentQuarter(CurrentQuarterId).Select(x=> x.QuaterId).SingleOrDefault();
        //        var BuilderNCPSurveyREsponseEditStatus = _ObjSurveyService.GetNCPSurveyResponseEditPermission(BuilderId.GetValueOrDefault(), ContractId.GetValueOrDefault(), PreviousQuarter.QuaterId);
        //        var EditStatus = new List<SelectListItem> {                    
        //            new SelectListItem{Text=BuilderNCPSurveyREsponseEditStatus.ToString(), Value = "1"},
        //            };

        //        return Json(EditStatus, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public JsonResult SaveBuilderNCPSurveyResponseStatus(Int64 BuilderId, Int64 ContractId)
        //{
        //    // angshuman to block this surveyresponsestatus save
        //    SurveyResponseEditStatus _Obj = new SurveyResponseEditStatus();
        //    var CurrentQuarterId = _ObjQuaterService.GetQuaterByDate(DateTime.Now).Select(x=> x.QuaterId).SingleOrDefault();
        //    var CurrentQuarterYear = _ObjQuaterService.GetQuaterByDate(DateTime.Now).Select(x => x.Year).SingleOrDefault();
        //    var PreviousQuarter = _ObjQuaterService.GetNCPReportingQuarterByCurrentQuarter(CurrentQuarterId).Select(x=> x.QuaterId).SingleOrDefault();
        //    _Obj.BuilderId = BuilderId;
        //    _Obj.ContractId = ContractId;
        //    _Obj.QuaterId = PreviousQuarter;
        //    _Obj.IsEditable = true;
        //    _Obj.RowStatusId = (int) RowActiveStatus.Active;            
        //    string SaveMessage = _ObjSurveyService.SaveNCPSurveyResponseEditPermission(_Obj);
        //    return Json(SaveMessage, JsonRequestBehavior.AllowGet);
        //}
        #endregion

    }
}