using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBUSA.Services.Interface;
using CBUSA.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using CBUSA.Domain;
using CBUSA.Areas.Admin.Models;
using System.Text;
using System.IO;
using Microsoft.Practices.Unity;
using CBUSA.Areas.CbusaBuilder.Models;
using CBUSA.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using Newtonsoft.Json;

namespace CBUSA.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SurveyResponseController : Controller
    {

        readonly ISurveyService _ObjSurveyService;
        readonly IQuestionService _ObjQuestionService;
        readonly ISurveyResponseService _ObjSurveyResponseService;

        readonly IBuilderQuaterContractProjectReportService _ObjBuilderQuaterContractProjectReportService;
        readonly IQuaterService _ObjQuaterService;
        readonly IContractServices _ObjContractService;
        readonly IBuilderService _ObjBuilderService;
        readonly IMarketService _ObjMarketService;

        public SurveyResponseController(ISurveyService ObjSurveyService, IQuestionService ObjQuestionService, ISurveyResponseService ObjSurveyResponseService,
            IBuilderQuaterContractProjectReportService ObjBuilderQuaterContractProjectReportService, IQuaterService ObjQuaterService, IContractServices ObjContractService,
            IBuilderService ObjBuilderService, IMarketService ObjMarketService
            )
        {
            _ObjSurveyService = ObjSurveyService;
            _ObjQuestionService = ObjQuestionService;
            _ObjSurveyResponseService = ObjSurveyResponseService;
            _ObjBuilderQuaterContractProjectReportService = ObjBuilderQuaterContractProjectReportService;
            _ObjQuaterService = ObjQuaterService;
            _ObjContractService = ObjContractService;
            _ObjBuilderService = ObjBuilderService;
            _ObjMarketService = ObjMarketService;
        }
        // GET: Admin/SurveyResponse
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowResponse(Int64 SurveyId, Int32? IsCompleted)
        {
            ViewBag.SurveyId = SurveyId;
           
            ViewBag.ShowStatus = true;
            ViewBag.ShowFullname = true;
            ViewBag.ShowEmail = true;
            ViewBag.IsNcpSurvey = false;
            var Survey = _ObjSurveyService.GetSurvey(SurveyId);
            if (Survey != null)
            {
                ViewBag.SurveyName = Survey.SurveyName;

                var CheckNCPSurvey = _ObjSurveyResponseService.CheckNCPSurvey(SurveyId);
                if (CheckNCPSurvey == true)
                {
                    ViewBag.IsNcpSurvey = true;
                }

                var FilterList = new List<SelectListItem> {
                      new SelectListItem{Text="Show all", Value="1"},
                             new SelectListItem{Text="Show only completed", Value="2"},
                              new SelectListItem{Text="Show only incomplete", Value="3"},
                        };
                ViewBag.FilterList = FilterList;
                ViewBag.ResponseFilterSet = 1;
                ViewBag.FilterIndex = 0;
                if (IsCompleted != null)
                {
                    if (IsCompleted == 2)
                    {
                        ViewBag.ResponseFilterSet = 2;
                        ViewBag.FilterIndex = 1;
                    }
                    if (IsCompleted == 3)
                    {
                        ViewBag.ResponseFilterSet = 3;
                        ViewBag.FilterIndex = 2;
                        
                    }
                }

                if (CheckNCPSurvey == true)
                {
                    var ParticipatedBuilder = _ObjSurveyResponseService.GetParticipatedBuilderNCP(SurveyId, 1);
                    
                    List<dynamic> MarketFilterList = new List<dynamic>();
                    List<dynamic> MarketFilterList1 = new List<dynamic>();
                    List<dynamic> BuilderFilterList = new List<dynamic>();
                    List<dynamic> BuilderFilterList1 = new List<dynamic>();

                    foreach (var builder in ParticipatedBuilder)
                    {
                        var MarketId = _ObjBuilderService.GetSpecificBuilder(builder.BuilderId).Market.MarketId;
                        bool has = MarketFilterList.Any(mar => mar.Value == MarketId.ToString());

                        if (!has)
                        {
                            MarketFilterList.Add(new
                            {
                                Value = MarketId.ToString(),
                                Text = _ObjBuilderService.GetSpecificBuilder(builder.BuilderId).Market.MarketName
                            });
                        }

                        BuilderFilterList.Add(new
                        {
                            Value = builder.BuilderId.ToString(),
                            Text = _ObjBuilderService.GetSpecificBuilder(builder.BuilderId).BuilderName
                        });
                    }

                    MarketFilterList1 = MarketFilterList.Union(new List<SelectListItem> { new SelectListItem { Text = "0--Select Market--", Value = "0" } }).ToList().OrderBy(y => y.Text).ToList();
                    MarketFilterList1.First(d => d.Value == "0").Text = "--Select Market--";
                    ViewBag.MarketFilterList = MarketFilterList1;

                    BuilderFilterList1 = BuilderFilterList.Union(new List<SelectListItem> { new SelectListItem { Text = "0--Select Builder--", Value = "0" } }).ToList().OrderBy(y => y.Text).ToList();
                    BuilderFilterList1.First(d => d.Value == "0").Text = "--Select Builder--";
                    ViewBag.BuilderFilterList = BuilderFilterList1;
                }
                else
                {
                    var MarketFilterList = _ObjSurveyService.GetSurveyMarket(SurveyId).Select(x => new SelectListItem
                    {
                        Value = x.MarketId.ToString(),
                        Text = x.Market.MarketName
                    }).ToList().Union(new List<SelectListItem> { new SelectListItem { Text = "0--Select Market--", Value = "0" } }).ToList().OrderBy(y => y.Text);

                    var MarketFilterList1 = MarketFilterList.ToList();
                    MarketFilterList1.First(d => d.Value == "0").Text = "--Select Market--";
                    ViewBag.MarketFilterList = MarketFilterList1;

                    var BuilderFilterList = _ObjSurveyService.GetSurveyBuilder(SurveyId).Select(x => new SelectListItem
                    {
                        Value = x.BuilderId.ToString(),
                        Text = x.Builder.BuilderName
                    }).ToList().Union(new List<SelectListItem> { new SelectListItem { Text = "0--Select Builder--", Value = "0" } }).ToList().OrderBy(y => y.Text);

                    var BuilderFilterList1 = BuilderFilterList.ToList();
                    BuilderFilterList1.First(d => d.Value == "0").Text = "--Select Builder--";
                    ViewBag.BuilderFilterList = BuilderFilterList1;
                }
            }
            return View();
        }
        public ActionResult DeleteSurveyResult(Int64 surveyid, string[] BuilderIdList)
        {
            var CheckNCPSurvey = _ObjSurveyResponseService.CheckNCPSurvey(surveyid);
            try
            {
                if (CheckNCPSurvey == false)
                {
                    //var ParticipatedBuilder = _ObjSurveyResponseService.GetParticipatedBuilder(surveyid, 1);
                    for (int x = 0; x < BuilderIdList.Length; x++)
                    {
                        var ObjSurveyList = _ObjSurveyResponseService.GetSurveyResultId(surveyid, Int64.Parse(BuilderIdList[x]));
                        foreach (var ItemId in ObjSurveyList)
                        {
                            SurveyResult ObjSurvey = _ObjSurveyResponseService.GetSurveyResult(ItemId.SurveyResultId);
                            ObjSurvey.SurveyId = surveyid;
                            ObjSurvey.ModifiedBy = 1;
                            ObjSurvey.ModifiedOn = DateTime.Now;
                            ObjSurvey.RowStatusId = (int)RowActiveStatus.Deleted;

                            _ObjSurveyResponseService.UpdateSurveyResult(ObjSurvey);
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < BuilderIdList.Length; x++)
                    {
                        var BuilderSurveyResponse = _ObjSurveyResponseService.GetBuilderSurveyResponseNCP(surveyid, Int64.Parse(BuilderIdList[x]));
                        if (BuilderSurveyResponse != null)
                        {
                            foreach (var Items in BuilderSurveyResponse)
                            {
                                BuilderQuaterContractProjectDetails ObjNCPResponse = _ObjSurveyResponseService.GetBuilderQuaterProjectDetails(Items.BuilderQuaterContractProjectDetailsId);
                                ObjNCPResponse.BuilderQuaterContractProjectReportId = surveyid;
                                ObjNCPResponse.ModifiedBy = 1;
                                ObjNCPResponse.ModifiedOn = DateTime.Now;
                                ObjNCPResponse.RowStatusId = (int)RowActiveStatus.Deleted;

                                _ObjSurveyResponseService.UpdateBuilderQuaterContractProjectDetails(ObjNCPResponse);
                            }
                        }
                    }
                }
                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                SurveyResult Obj = new SurveyResult();
                _ObjSurveyResponseService.UpdateSurveyResult(Obj, true);
                BuilderQuaterContractProjectDetails Obj1 = new BuilderQuaterContractProjectDetails();
                _ObjSurveyResponseService.UpdateBuilderQuaterContractProjectDetails(Obj1, true);
            }

            //return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeStatusSurveyResult(Int64 surveyid, string[] BuilderIdList, string QuarterId)
        {
            var CheckNCPSurvey = _ObjSurveyResponseService.CheckNCPSurvey(surveyid);
            string[] SelectedBuilder;
            bool SurveyStatus = false;
            try
            {
                if (CheckNCPSurvey == false)
                {

                    for (int x = 0; x < BuilderIdList.Length; x++)
                    {
                        SelectedBuilder = BuilderIdList[x].Split('~');
                        if (SelectedBuilder[1] == "1" || SelectedBuilder[1] == "true")
                        {
                            SurveyStatus = false;
                        }
                        else if (SelectedBuilder[1] == "2" || SelectedBuilder[1] == "false")
                        {
                            SurveyStatus = true;
                        }
                        var ObjSurveyList = _ObjSurveyResponseService.GetSurveyBuilderId(surveyid, Int64.Parse(SelectedBuilder[0]));
                        foreach (var ItemId in ObjSurveyList)
                        {
                            SurveyBuilder ObjSurveyBuilder = _ObjSurveyResponseService.GetSurveyBuilder(ItemId.SurveyBuilderId);
                            ObjSurveyBuilder.SurveyId = surveyid;
                            ObjSurveyBuilder.ModifiedBy = 1;
                            ObjSurveyBuilder.ModifiedOn = DateTime.Now;
                            ObjSurveyBuilder.IsSurveyCompleted = SurveyStatus;

                            _ObjSurveyResponseService.UpdateSurveyBuilder(ObjSurveyBuilder);
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < BuilderIdList.Length; x++)
                    {
                        SelectedBuilder = BuilderIdList[x].Split('~');
                        SelectedBuilder = BuilderIdList[x].Split('~');
                        if (SelectedBuilder[1] == "1" || SelectedBuilder[1] == "true")
                        {
                            SurveyStatus = false;
                        }
                        else if (SelectedBuilder[1] == "2" || SelectedBuilder[1] == "false")
                        {
                            SurveyStatus = true;
                        }
                        var BuilderSurveyDetails = _ObjSurveyResponseService.GetBuilderSurveyResponseNCP(surveyid, Int64.Parse(SelectedBuilder[0]));
                        foreach (var data in BuilderSurveyDetails)
                        {
                            var BuilderSurveyResponse = _ObjSurveyResponseService.GetBuilderQuaterContractProjectReport(data.ProjectId, data.BuilderId, Int32.Parse(QuarterId), data.ContractId);

                            if (BuilderSurveyResponse != null)
                            {
                                foreach (var Items in BuilderSurveyResponse)
                                {
                                    BuilderQuaterAdminReport ObjNCPResponse = _ObjSurveyResponseService.GetBuilderQuaterAdminReportId(Int32.Parse(Items.BuilderQuaterAdminReportId.ToString()));
                                    ObjNCPResponse.BuilderQuaterAdminReportId = Int32.Parse(Items.BuilderQuaterAdminReportId.ToString());
                                    ObjNCPResponse.ModifiedBy = 1;
                                    ObjNCPResponse.ModifiedOn = DateTime.Now;
                                    ObjNCPResponse.IsSubmit = SurveyStatus;

                                    _ObjSurveyResponseService.UpdateBuilderQuaterAdminReport(ObjNCPResponse);
                                }
                            }
                        }
                    }
                }
                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                SurveyBuilder Obj = new SurveyBuilder();
                _ObjSurveyResponseService.UpdateSurveyBuilder(Obj, true);
                BuilderQuaterAdminReport Obj1 = new BuilderQuaterAdminReport();
                _ObjSurveyResponseService.UpdateBuilderQuaterAdminReport(Obj1, true);
            }

            //return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ArchieveSurveyResult(Int64 surveyid)
        {
            var CheckNCPSurvey = _ObjSurveyResponseService.CheckNCPSurvey(surveyid);
            if (CheckNCPSurvey == false)
            {
                var ParticipatedBuilder = _ObjSurveyResponseService.GetParticipatedBuilder(surveyid, 1);
                foreach (var Item in ParticipatedBuilder)
                {
                    var ObjSurveyList = _ObjSurveyResponseService.GetSurveyResultId(surveyid, Item.BuilderId);
                    foreach (var ItemId in ObjSurveyList)
                    {
                        SurveyResult ObjSurvey = _ObjSurveyResponseService.GetSurveyResult(ItemId.SurveyResultId);
                        ObjSurvey.SurveyId = surveyid;
                        ObjSurvey.ModifiedBy = 1;
                        ObjSurvey.ModifiedOn = DateTime.Now;
                        ObjSurvey.RowStatusId = (int)RowActiveStatus.Archived;

                        _ObjSurveyResponseService.UpdateSurveyResult(ObjSurvey);
                    }
                }
                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult RenderFilterQuestionDropdown(Int64 SurveyId)
        {
            var QuestionList = _ObjQuestionService.GetSurveyQuestionAll(SurveyId).
                Select(x => new CustomQestionControlVM { QuestionId = x.QuestionId, QuestionValue = x.QuestionValue }).ToList();
            var CheckNCPSurvey = _ObjSurveyResponseService.CheckNCPSurvey(SurveyId);
            if (CheckNCPSurvey == false)
            {
                List<CustomQestionControlVM> StaticQuestion = new List<CustomQestionControlVM>();

                StaticQuestion.Add(new CustomQestionControlVM { QuestionId = 0, QuestionValue = "Status" });
                StaticQuestion.Add(new CustomQestionControlVM { QuestionId = 1, QuestionValue = "Company name" }); // rabi on 5 jan 
                StaticQuestion.Add(new CustomQestionControlVM { QuestionId = 2, QuestionValue = "Builder Id" });
                StaticQuestion.Add(new CustomQestionControlVM { QuestionId = 3, QuestionValue = "Market" });
                List<CustomQestionControlVM> QuestionListAct = new List<CustomQestionControlVM>();
                QuestionListAct.AddRange(StaticQuestion);
                QuestionListAct.AddRange(QuestionList);
                string PrtialViewString = PartialView("_SurveyQuestionCustomControl", QuestionListAct).RenderToString();
                return Json(new { IsSuccess = true, QuestionFilterCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string PrtialViewString = PartialView("_SurveyQuestionCustomControl", QuestionList).RenderToString();
                return Json(new { IsSuccess = true, QuestionFilterCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult QuestionList_Read([DataSourceRequest] DataSourceRequest request, Int64 SurveyId, string QuestionIdListFilter)
        {
           var QuestionList = _ObjQuestionService.GetQuestionValueWithGrid(SurveyId, QuestionIdListFilter).ToList();
           var DynamicViewModel = QuestionList.Select((x, index) => new
            {
                ProjectId = x.ProjectId,
                QuestionId = x.QuestionId,
                QuestionValue = x.QuestionValue.ToString().Length > 50 ? x.QuestionValue.ToString().Substring(0, 50) : x.QuestionValue,
                RowIndex = x.RowIndex,
                ColIndex = x.ColIndex,
                IsColoumnSortingAvailable = x.QuestionTypeId != 3 ? true : false,
                ColoumnOrder = index
            });
            var jsonResult = Json(DynamicViewModel.ToDataSourceResult(request));
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
            //return Json(DynamicViewModel.ToDataSourceResult(request));
        }
        public ActionResult QuestionAnsware_Read([DataSourceRequest] DataSourceRequest request, Int64 SurveyId, string ResponseFilter, string MarketFilter, string BuilderFilter, string QuestionIdListFilter)
        {
            // string Filter = Request.Params["Filter"].ToString();
            string QuestionListFilterCopy = QuestionIdListFilter;

            var ProjectQuestionList = _ObjQuestionService.GetQuestionValueWithGrid(SurveyId, QuestionIdListFilter).ToList();
            int QuestionList = ProjectQuestionList.Count();
            var SelectedQuestionArray = QuestionIdListFilter.Split(',');
            
            if (QuestionIdListFilter != null && QuestionIdListFilter != "" && QuestionIdListFilter.Length > 0)
            {
                ViewBag.ShowStatus = false;
                ViewBag.ShowFullname = false;
                ViewBag.ShowEmail = false;
                ViewBag.ShowCity = false;
                ViewBag.ShowState = false;
                ViewBag.ShowLot = false;
                for (int ctr = 0; ctr < SelectedQuestionArray.Length; ctr++)
                {
                    if (Int64.Parse(SelectedQuestionArray[ctr]) == 0) //status
                    {
                        ViewBag.ShowStatus = true;
                        SelectedQuestionArray[ctr] = "-1";
                    }
                    if (Int64.Parse(SelectedQuestionArray[ctr]) == 1) //Company Name
                    {
                        ViewBag.ShowFullname = true;
                        SelectedQuestionArray[ctr] = "-1";
                    }
                    if (Int64.Parse(SelectedQuestionArray[ctr]) == 3) //Market
                    {
                        ViewBag.ShowEmail = true;
                        SelectedQuestionArray[ctr] = "-1";
                    }
                    if (Int64.Parse(SelectedQuestionArray[ctr]) == 2) //BuilderId
                    {
                        ViewBag.ShowCity = true;
                        SelectedQuestionArray[ctr] = "-1";
                    }
                    if (Int64.Parse(SelectedQuestionArray[ctr]) == 4)
                    {
                        ViewBag.ShowState = true;
                        SelectedQuestionArray[ctr] = "-1";
                    }
                    if (Int64.Parse(SelectedQuestionArray[ctr]) == 5)
                    {
                        ViewBag.ShowLot = true;
                        SelectedQuestionArray[ctr] = "-1";
                    }
                }
            }
            else
            {
                ViewBag.ShowStatus = true;
                ViewBag.ShowFullname = true;
                ViewBag.ShowEmail = true;
                ViewBag.ShowCity = true;
                ViewBag.ShowState = true;
                ViewBag.ShowLot = true;
            }

            string SelectedQuestion = string.Join(",", SelectedQuestionArray.ToList<string>());
            SelectedQuestion = SelectedQuestion.Replace("-1,", "").Replace("-1", "");

            int Flag = 0;
            List<SurveyResponseViewModel> ObjList = new List<SurveyResponseViewModel>();
            List<Int64> SelectedQuestionList = new List<Int64>();
            int SurveyCompleteFilter = 0;
            switch (Convert.ToInt32(ResponseFilter))
            {
                case 1:
                    SurveyCompleteFilter = 1;
                    break;
                case 2:
                    SurveyCompleteFilter = 2;
                    break;
                case 3:
                    SurveyCompleteFilter = 3;
                    break;
            }
            var CheckNCPSurvey = _ObjSurveyResponseService.CheckNCPSurvey(SurveyId);
            ViewBag.ExcelHeader = "Survey Response";
            string ErrorMessage = "";
            if (CheckNCPSurvey == false)
            {
                try
                {

                //Flag = 0;
                //var ParticipatedBuilder = _ObjSurveyResponseService.GetParticipatedBuilderActive(SurveyId, SurveyCompleteFilter);
                //var FilteredParticipatedBuilder = ParticipatedBuilder;
                
                //if (BuilderFilter != "0")
                //{
                //    var FilteredBuilderId = Convert.ToInt64(BuilderFilter);
                //    FilteredParticipatedBuilder = FilteredParticipatedBuilder.ToList().Where(x => x.BuilderId == FilteredBuilderId);
                //}

                //foreach (var Item in FilteredParticipatedBuilder)
                //{
                //    Builder Bldr = _ObjBuilderService.GetSpecificBuilder((long)Item.BuilderId);
                   
                //    //List<Builder> builder = _ObjSurveyResponseService.GetBuilderDetails(Item.BuilderId);
                //    Int64 MarketId = Bldr.MarketId;
                //    string BuilderMarketName = "";

                //    bool FlagMarketFilter = false;

                //    if (MarketFilter != "0")
                //    {
                //        var FilteredMarketId = Convert.ToInt64(MarketFilter);
                //        if (MarketId == FilteredMarketId)
                //        {
                //            FlagMarketFilter = true;
                //        }
                //    }
                //    else
                //    {
                //        FlagMarketFilter = true;
                //    }

                //    if (FlagMarketFilter == true)
                //    {
                //        BuilderMarketName = _ObjSurveyResponseService.GetBuilderMrketName(MarketId).FirstOrDefault();
                //        //_ObjBuilderService.BuilderDetails(Item.BuilderId).FirstOrDefault().Market.MarketName;
                //        // normal survey Details without question filter to take care for show answers
                //        if (QuestionIdListFilter.Length > 0)
                //        {
                //            for (int x = 0; x < SelectedQuestionArray.Length; x++)
                //            {
                //                SelectedQuestionList.Add(Int64.Parse(SelectedQuestionArray[x]));
                //            }
                //           List<SurveyResponse> BuilderSurveyResponse = _ObjSurveyResponseService.GetBuilderSurveyResponseFiltered(SurveyId, Item.BuilderId, SelectedQuestionList);

                //            if (BuilderSurveyResponse != null)
                //            {
                //                ObjList.Add(new SurveyResponseViewModel
                //                {
                //                    ColoumnOrder = Flag,
                //                    ShowNCP = 1,
                //                    ShowStatus = ViewBag.ShowStatus,
                //                    ShowFullname = ViewBag.ShowFullname,
                //                    ShowEmail = ViewBag.ShowEmail,
                //                    ExcelReportHeader = "Survey Response",
                //                    ShowCity = ViewBag.ShowCity,
                //                    //ShowState = ViewBag.ShowState,
                //                    //ShowLot = ViewBag.ShowLot,
                //                    SurveyId = SurveyId,
                //                    BuilderId = Item.BuilderId,
                //                    //InviteFullName = builder.Select(a => a.FirstName + " " + a.LastName).FirstOrDefault().ToString(),
                //                    InviteFullName = Bldr.BuilderName,
                //                    //InviteEmail = builder.Select(a => a.Email).FirstOrDefault().ToString(),
                //                    MarketName = BuilderMarketName,
                //                    IsSurveyCompleted = Item.IsSurveyCompleted,
                //                    QuestionCount = QuestionList,
                //                    //ProjectQuestionList = ProjectQuestionList,
                //                    Response = BuilderSurveyResponse.Select(x => new
                //                    {
                //                        QuestionId = x.QuestionId,
                //                        flag = 1,
                //                        QuestionTypeId = x.QuestionTypeId,
                //                        ProjectId = 0,
                //                        RowIndex = x.RowIndex,
                //                        ColIndex = x.ColIndex,
                //                        ResultSet = x.Result.Select(y => new { RowNo = y != null ? y.RowNumber : 0, ColNo = y != null ? y.ColumnNumber : 0, Answer = y != null ? y.Answer != null ? y.Answer : "" : "" }),
                //                    }).ToList<dynamic>()
                //                });
                //            }
                //        }
                //        else
                //        {
                //            // normal survey Details question filter to take care for show answers
                //            List<SurveyResponse> BuilderSurveyResponse = _ObjSurveyResponseService.GetBuilderSurveyResult(SurveyId, Item.BuilderId);

                //            if (BuilderSurveyResponse != null)
                //            {
                //                ObjList.Add(new SurveyResponseViewModel
                //                {
                //                    ColoumnOrder = Flag,
                //                    ShowNCP = 1,
                //                    ShowStatus = ViewBag.ShowStatus,
                //                    ShowFullname = ViewBag.ShowFullname,
                //                    ShowEmail = ViewBag.ShowEmail,
                //                    ExcelReportHeader = "Survey Response",
                //                    ShowCity = ViewBag.ShowCity,
                //                    //ShowState = ViewBag.ShowState,
                //                    //ShowLot = ViewBag.ShowLot,
                //                    SurveyId = SurveyId,
                //                    BuilderId = Item.BuilderId,
                //                    //InviteFullName = builder.Select(a => a.FirstName + " " + a.LastName).FirstOrDefault().ToString(),
                //                    InviteFullName = Bldr.BuilderName,
                //                    MarketName = BuilderMarketName == null? "" : BuilderMarketName,
                //                    //InviteEmail = builder.Select(a => a.Email).FirstOrDefault().ToString(),
                //                    IsSurveyCompleted = Item.IsSurveyCompleted,
                //                    QuestionCount = QuestionList,
                //                    //ProjectQuestionList = ProjectQuestionList,
                //                    Response = BuilderSurveyResponse.Select(x => new
                //                    {
                //                        QuestionId = x.QuestionId,
                //                        flag = 1,
                //                        QuestionTypeId = x.QuestionTypeId,
                //                        ProjectId = 0,
                //                        RowIndex = x.RowIndex,
                //                        ColIndex = x.ColIndex,
                //                        ResultSet = x.Result.Select(y => new { RowNo = y != null ? y.RowNumber : 0, ColNo = y != null ? y.ColumnNumber : 0, Answer = y != null ? y.Answer != null ? y.Answer : "" : "" }),
                //                    }).ToList<dynamic>()
                //                });
                //            }
                //        }                        
                //        }
                //    }

                //*****************************************************************************************************
                SqlConnection con;
                string constr = ConfigurationManager.ConnectionStrings["CBUSA"].ToString();
                con = new SqlConnection(constr);
                SqlCommand com = new SqlCommand("sp_GetEnrolmentSurveyResult", con);
                com.Parameters.AddWithValue("@SurveyId", SurveyId);
                com.Parameters.AddWithValue("@ShowQuestionIdList", QuestionIdListFilter.Length > 0 ? QuestionIdListFilter : null);
                com.Parameters.AddWithValue("@IsCompleted", SurveyCompleteFilter);
                com.Parameters.AddWithValue("@MarketId", MarketFilter != "0" ? Convert.ToInt64(MarketFilter) : 0);
                com.Parameters.AddWithValue("@BuilderId", BuilderFilter != "0" ? Convert.ToInt64(BuilderFilter) : 0);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandTimeout = 600;
                    
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();

                List<dynamic> List1 = dt.AsEnumerable().Select(x => new
                {
                    ColoumnOrder = Flag,
                    ShowNCP = 1,
                    ShowStatus = ViewBag.ShowStatus,
                    ShowFullname = ViewBag.ShowFullname,
                    ShowEmail = ViewBag.ShowEmail,
                    ShowCity = ViewBag.ShowCity,
                    SurveyId = SurveyId,
                    ExcelReportHeader = "",
                    BuilderId = x.Field<Int64>("BuilderId"),
                    InviteFullName = x.Field<string>("BuilderName"),
                    InviteEmail = "",
                    IsSurveyCompleted = x.Field<bool>("IsSurveyCompleted"),
                    RowNumber = x.Field<Int64>("ItemNo"),
                    ContractId = 0,
                    ProjectId = 0,
                    QuaterId = 0,
                    ContractName = "",
                    FileUpload = 0,
                    MarketName = x.Field<string>("MarketName"),
                    BuilderCompany = x.Field<string>("BuilderName"),
                    BuilderName = x.Field<string>("BuilderName"),
                    ProjectName = "",
                    ProjectAddress = "",
                    Response = GetSurveyAnswers(dt, x.Field<Int64>("BuilderId"))
                }).ToList<dynamic>();
                //**************************************************************************************

                int ResponseCounter = 1;

                var newList = List1.ToList()
                    .Select(x => new 
                    {
                        ColoumnOrder = x.ColoumnOrder,
                        ShowNCP = x.ShowNCP,
                        ShowStatus = x.ShowStatus,
                        ShowFullname = x.ShowFullname,
                        ShowEmail = x.ShowEmail,
                        ShowCity = x.ShowCity,
                        SurveyId = x.SurveyId,
                        ExcelReportHeader = x.ExcelReportHeader,
                        BuilderId = x.BuilderId,
                        InviteFullName = x.InviteFullName,
                        InviteEmail = x.InviteEmail,
                        IsSurveyCompleted = x.IsSurveyCompleted,
                        RowNumber = x.RowNumber,
                        ContractId = x.ContractId,
                        ProjectId = x.ProjectId,
                        QuaterId = x.QuaterId,
                        ContractName = x.ContractName,
                        FileUpload = x.FileUpload,
                        MarketName = x.MarketName,
                        BuilderCompany = x.BuilderCompany,
                        BuilderName = x.BuilderName,
                        ProjectName = x.ProjectName,
                        ProjectAddress = x.ProjectAddress,
                        Response = x.Response,
                        rowcount = ResponseCounter++
                    }).ToList();

                var jsonResult = Json(newList.ToDataSourceResult(request));
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;

                }
                catch (Exception ee)
                {
                    ErrorMessage = ee.Message.ToString();
                    return Json("");
                }
            }
            else
            {
                // NCP Details to show

                //var ParticipatedBuilder = _ObjSurveyResponseService.GetParticipatedBuilderNCP(SurveyId, SurveyCompleteFilter);
                //foreach (var Item in ParticipatedBuilder)
                //{
                //    List<Builder> builder = _ObjSurveyResponseService.GetBuilderDetails(Item.BuilderId);
                //    // NCP Details question filter to take care for show answers
                //    if (QuestionIdListFilter.Length > 0)
                //    {
                //        for (int x = 0; x < SelectedQuestionArray.Length; x++)
                //        {
                //            SelectedQuestionList.Add(Int64.Parse(SelectedQuestionArray[x]));
                //        }
                //        List<SurveyResponseNCP> BuilderSurveyResponse = _ObjSurveyResponseService.GetBuilderSurveyResponseNCPFiltered(SurveyId, Item.BuilderId, SelectedQuestionList);
                //        if (BuilderSurveyResponse != null)
                //        {
                //            ObjList.Add(new SurveyResponseViewModel
                //            {
                //                ColoumnOrder = Flag,
                //                ShowNCP = 2,
                //                ShowStatus = ViewBag.ShowStatus,
                //                ShowFullname = ViewBag.ShowFullname,
                //                ShowEmail = ViewBag.ShowEmail,
                //                //ShowCity = ViewBag.ShowCity,
                //                //ShowState = ViewBag.ShowState,
                //                //ShowLot = ViewBag.ShowLot,
                //                SurveyId = SurveyId,
                //                BuilderId = Item.BuilderId,
                //                InviteFullName = builder.Select(a => a.FirstName + " " + a.LastName).FirstOrDefault().ToString(),
                //                //InviteFullName = builder.Select(a => a.BuilderName).FirstOrDefault().ToString(),
                //                InviteEmail = builder.Select(a => a.Email).FirstOrDefault().ToString(),
                //                IsSurveyCompleted = Item.IsSurveyCompleted,
                //                ContractId = BuilderSurveyResponse.Select(a => a.ContractId).FirstOrDefault(),
                //                ProjectId = BuilderSurveyResponse.Select(a => a.ProjectId).FirstOrDefault(),
                //                QuaterId = BuilderSurveyResponse.Select(a => a.QuaterId).FirstOrDefault(),
                //                QuestionCount = QuestionList,
                //                //ProjectQuestionList = ProjectQuestionList,
                //                Response = BuilderSurveyResponse.Select(x => new
                //                {
                //                    QuestionId = x.QuestionId,
                //                    flag = 1,
                //                    QuestionTypeId = x.QuestionTypeId,
                //                    ProjectId = x.Result.Select(y => y.BuilderQuaterContractProjectReport.ProjectId).FirstOrDefault(),
                //                    RowIndex = x.RowIndex,
                //                    ColIndex = x.ColIndex,
                //                    ResultSet = x.Result.Select(y => new { RowNo = y != null ? y.RowNumber : 0, ColNo = y != null ? y.ColumnNumber : 0, QuestionId = y.QuestionId, Answer = y != null ? y.Answer != null ? y.Answer : "-" : "-" }),
                //                }).ToList<dynamic>()
                //            });
                //        }
                //    }
                //    else
                //    {
                //        // NCP Details without question filter to take care for show answers
                //        List<SurveyResponseNCP> BuilderSurveyResponse = _ObjSurveyResponseService.GetBuilderSurveyResponseNCP(SurveyId, Item.BuilderId);
                //        if (BuilderSurveyResponse != null)
                //        {
                //            ObjList.Add(new SurveyResponseViewModel
                //            {
                //                ColoumnOrder = Flag,
                //                ShowNCP = 2,
                //                ShowStatus = ViewBag.ShowStatus,
                //                ShowFullname = ViewBag.ShowFullname,
                //                ShowEmail = ViewBag.ShowEmail,
                //                //ShowCity = ViewBag.ShowCity,
                //                //ShowState = ViewBag.ShowState,
                //                //ShowLot = ViewBag.ShowLot,
                //                SurveyId = SurveyId,
                //                BuilderId = Item.BuilderId,
                //                InviteFullName = builder.Select(a => a.FirstName + " " + a.LastName).FirstOrDefault().ToString(),
                //                //InviteFullName = builder.Select(a => a.BuilderName).FirstOrDefault().ToString(),
                //                InviteEmail = builder.Select(a => a.Email).FirstOrDefault().ToString(),
                //                IsSurveyCompleted = Item.IsSurveyCompleted,
                //                ContractId = BuilderSurveyResponse.Select(a => a.ContractId).FirstOrDefault(),
                //                ProjectId = BuilderSurveyResponse.Select(a => a.ProjectId).FirstOrDefault(),
                //                QuaterId = BuilderSurveyResponse.Select(a => a.QuaterId).FirstOrDefault(),
                //                QuestionCount = QuestionList,
                //                //ProjectQuestionList = ProjectQuestionList,
                //                Response = BuilderSurveyResponse.Select(x => new
                //                {
                //                    QuestionId = x.QuestionId,
                //                    flag=1,
                //                    QuestionTypeId = x.QuestionTypeId,
                //                    ProjectQuestionList = ProjectQuestionList,
                //                    ProjectId = x.Result.Select(y=> y.BuilderQuaterContractProjectReport.ProjectId).FirstOrDefault(),
                //                    RowIndex = x.RowIndex,
                //                    ColIndex = x.ColIndex,
                //                    ResultSet = x.Result.Select(y => new { RowNo = y != null ? y.RowNumber : 0, ColNo = y != null ? y.ColumnNumber : 0, Answer = y != null ? y.Answer != null ? y.Answer : "-" : "-" }),
                //                }).ToList<dynamic>()
                //            });
                //        }
                //    }
                //}
                //List<Builder> builder = _ObjSurveyResponseService.GetBuilderDetails(Item.BuilderId);


                //string QuestionList = SelectedQuestionList.ToString();

                SqlConnection con;
                string constr = ConfigurationManager.ConnectionStrings["CBUSA"].ToString();
                con = new SqlConnection(constr);
                SqlCommand com = new SqlCommand("Proc_GetNcpResult", con);
                com.Parameters.AddWithValue("@SurveyId", SurveyId);
                com.Parameters.AddWithValue("@QuestionIdList", QuestionIdListFilter.Length > 0 ? QuestionIdListFilter : null);
                com.Parameters.AddWithValue("@RenderType", SurveyCompleteFilter);
                com.Parameters.AddWithValue("@MarketId", MarketFilter.Length > 0 ? MarketFilter : null);
                com.Parameters.AddWithValue("@BuilderId", BuilderFilter.Length > 0 ? BuilderFilter : null);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandTimeout = 600;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();

                ViewBag.ExcelHeader = _ObjSurveyResponseService.GetContractQuater(SurveyId);

                List<dynamic> List1 = dt.AsEnumerable().Select(x => new
                {
                    ColoumnOrder = Flag,
                    ShowNCP = 2,
                    ShowStatus = false,
                    ShowFullname = false,
                    ShowEmail = false,
                    ShowCity=false,
                    SurveyId = SurveyId,
                    ExcelReportHeader = ViewBag.ExcelHeader,
                    BuilderId = x.Field<Int64>("BuilderId"),
                    InviteFullName = x.Field<string>("BuilderName"),
                    InviteEmail = "",
                    IsSurveyCompleted = "",
                    RowNumber = x.Field<Int64>("RowNumber"),
                    ContractId = x.Field<Int64>("ContractId"),
                    ProjectId = x.Field<Int64>("ProjectId"),
                    QuaterId = x.Field<Int64>("QuaterId"),
                    ContractName = "",
                    FileUpload = x.Field<Int32>("FileUpload"),
                    MarketName = x.Field<string>("MarketName"),
                    BuilderCompany = x.Field<string>("BuilderCompany"),
                    BuilderName = x.Field<string>("BuilderName"),
                    ProjectName = x.Field<string>("ProjectName"),
                    ProjectAddress = x.Field<string>("ProjectAddress"),

                    Response = GetNCPAnswers(dt, x.Field<Int64>("ProjectId"), x.Field<Int64>("BuilderId"), x.Field<Int64>("ContractId"))
                }).ToList<dynamic>();

                int ResponseCounter = 1;
                var newList = List1.ToList()
                    .Select(x => new
                    {
                        ColoumnOrder = x.ColoumnOrder,
                        ShowNCP = x.ShowNCP,
                        ShowStatus = x.ShowStatus,
                        ShowFullname = x.ShowFullname,
                        ShowEmail = x.ShowEmail,
                        ShowCity = x.ShowCity,
                        SurveyId = x.SurveyId,
                        ExcelReportHeader = x.ExcelReportHeader,
                        BuilderId = x.BuilderId,
                        InviteFullName = x.InviteFullName,
                        InviteEmail = x.InviteEmail,
                        IsSurveyCompleted = x.IsSurveyCompleted,
                        RowNumber = x.RowNumber,
                        ContractId = x.ContractId,
                        ProjectId = x.ProjectId,
                        QuaterId = x.QuaterId,
                        ContractName = x.ContractName,
                        FileUpload = x.FileUpload,
                        MarketName = x.MarketName,
                        BuilderCompany = x.BuilderCompany,
                        BuilderName = x.BuilderName,
                        ProjectName = x.ProjectName,
                        ProjectAddress = x.ProjectAddress,
                        Response = x.Response,
                        rowcount = ResponseCounter++
                    }).ToList();

                var jsonResult = Json(newList.ToDataSourceResult(request));
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;

                // -- approach -I
                //var dynamicDt = new List<dynamic>();
                //foreach (DataRow row in dt.Rows)
                //{
                //    dynamic dyn = new ExpandoObject();
                //    dynamicDt.Add(dyn);
                //    foreach (DataColumn column in dt.Columns)
                //    {
                //        var dic = (IDictionary<string, object>)dyn;
                //        dic[column.ColumnName] = row[column];
                //    }
                //}

                //string JSONresult;
                //JSONresult = JsonConvert.SerializeObject(dt);
                // -- approach -I

                //----- new approach II
                //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                //Dictionary<string, object> rowNew;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    rowNew = new Dictionary<string, object>();
                //    foreach (DataColumn col in dt.Columns)
                //    {
                //        rowNew.Add(col.ColumnName, dr[col]);
                //    }
                //    rows.Add(rowNew);
                //}
                ////return serializer.Serialize(rows);
                //string RowsData = serializer.Serialize(rows);
                //----- new approach II


                //----- new approach III


                //foreach (DataRow dr in dt.Rows)
                //{                    
                //    ObjList.Add(new SurveyResponseViewModel
                //    {
                //        ColoumnOrder = Flag,
                //        ShowNCP=2,
                //        ShowStatus = ViewBag.ShowStatus,
                //        ShowFullname = ViewBag.ShowFullname,
                //        ShowEmail = ViewBag.ShowEmail,
                //        SurveyId = SurveyId,
                //        InviteFullName = "",
                //        InviteEmail = "",
                //        Response = GetNCPAnswers(dt, dr)
                //    }                        
                //    );
                //}


                //----- new approach III
                //return Json(List1.ToDataSourceResult(request));
                //var jsonResult = Json(dt.ToDataSourceResult(request));
                //jsonResult.MaxJsonLength = Int32.MaxValue;
                //return jsonResult;
            }

            //return Json(ObjList.ToDataSourceResult(request));
        }

        private List<dynamic> GetSurveyAnswers(DataTable dt, Int64 BuilderId)
        {
            DataRow FilterRow = dt.AsEnumerable().Where(x => x.Field<Int64>("BuilderId") == BuilderId).FirstOrDefault();

            List<dynamic> tmpList = new List<dynamic>();

            if (FilterRow != null)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    int x;
                    if (Int32.TryParse(col.ColumnName.ToString(), out x))
                    {
                        tmpList.Add(new
                        {
                            Answer = FilterRow[col].ToString()
                        });
                    }
                }
            }
            return tmpList.ToList();
        }

        private List<dynamic> GetNCPAnswers(DataTable dt, Int64 ProjectId, Int64 BuilderId, Int64 ContractId)
        {
            DataRow FilterRow = dt.AsEnumerable().Where(x => x.Field<Int64>("BuilderId") == BuilderId && x.Field<Int64>("ProjectId") == ProjectId &&
                x.Field<Int64>("ContractId") == ContractId
                ).FirstOrDefault();

            List<dynamic> tmpList = new List<dynamic>();

            if (FilterRow != null)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    int x;
                    if (Int32.TryParse(col.ColumnName.ToString(), out x))
                    {
                        tmpList.Add(new
                        {
                            Answer = FilterRow[col].ToString()
                        });
                    }


                }

            }
            return tmpList.ToList();
        }
        private List<dynamic> GetNCPAnswers(DataTable dt, DataRow dr)
        {
            List<dynamic> tmpList = new List<dynamic>();
            bool print = true;
            foreach (DataColumn col in dt.Columns)
            {
                //rowNew.Add(col.ColumnName, dr[col]);
                if (col.ColumnName.Contains("RowNumber"))
                {
                    if (dr[col].ToString() != "1")
                    {
                        print = false;
                    }
                    tmpList.Add(new
                    {
                        RowNumber = dr[col].ToString()
                    });
                }
                else if (col.ColumnName.Contains("MarketName"))
                {
                    if (print)
                    {
                        tmpList.Add(new
                        {
                            BuilderMarket = dr[col].ToString()
                        });
                    }
                    else
                    {
                        tmpList.Add(new
                        {
                            BuilderMarket = ""
                        });
                    }

                }
                else if (col.ColumnName.Contains("BuilderCompany"))
                {
                    if (print)
                    {
                        tmpList.Add(new
                        {
                            Company = dr[col].ToString()
                        });
                    }
                    else
                    {
                        tmpList.Add(new
                        {
                            Company = ""
                        });
                    }
                }
                else if (col.ColumnName.Contains("BuilderName"))
                {
                    tmpList.Add(new
                    {
                        InviteFullInviteFullName = dr[col].ToString()
                    });
                }
                else if (col.ColumnName.Contains("ProjectName"))
                {
                    tmpList.Add(new
                    {
                        ProjectName = dr[col].ToString()
                    });
                }
                else if (col.ColumnName.Contains("ProjectAddress"))
                {
                    tmpList.Add(new
                    {
                        ProjectAddress = dr[col].ToString()
                    });
                }
                else if (col.ColumnName.Contains("ProjectId"))
                {
                    tmpList.Add(new
                    {
                        ProjectId = dr[col].ToString()
                    });
                }
                else if (col.ColumnName.Contains("BuilderId"))
                {
                    tmpList.Add(new
                    {
                        BuilderId = dr[col].ToString()
                    });
                }
                else if (col.ColumnName.Contains("ContractId"))
                {
                    tmpList.Add(new
                    {
                        ContractId = dr[col].ToString()
                    });
                }
                else
                {
                    int x;
                    if (Int32.TryParse(col.ColumnName.ToString(), out x))
                    {
                        tmpList.Add(new
                        {
                            Answer = dr[col].ToString()
                        });
                    }
                }

            }



            return tmpList.ToList();
        }

        #region Edit SurveyResponse  Rabi

        public ActionResult EditBuilderSurveyResponse(Int64? SurveyId, Int64? BuilderId)
        {

            if (SurveyId.HasValue && BuilderId.HasValue)
            {
                var Survey = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault());
                if (Survey != null)
                {
                    var QuestionList = _ObjQuestionService.GetSurveyQuestionAll(SurveyId.GetValueOrDefault());
                    var SurveyResult = _ObjSurveyService.GetBuilderSurveyResult(SurveyId.GetValueOrDefault(), BuilderId.GetValueOrDefault());
                    if (Survey != null)
                    {
                        var ObjVm = new EditSurveyResponseViewModel
                        {
                            SurveyId = SurveyId.GetValueOrDefault(),
                            BuilderId = BuilderId.GetValueOrDefault(),
                            ObjSurvey = Survey,
                            QuestionList = QuestionList != null ? QuestionList.ToList() : new List<Question> { },
                            ObjSurveyResult = SurveyResult != null ? SurveyResult.ToList() : new List<SurveyResult> { }

                        };
                        return View(ObjVm);
                    }
                }
            }
            else
            {
                return View(new EditSurveyResponseViewModel { ObjSurvey = new Survey() });
            }
            return View();
        }

        [HttpPost]
        public JsonResult EditSurveyResultByAdmin(EditSurveyResponseViewModel ObjVm)
        {
            string ServerFilePath = Server.MapPath("~/ApplicationDocument/SurveyDocument");
            _ObjSurveyService.EditSurveyResultByAdmin(ObjVm.SurveyId, ObjVm.BuilderId, ObjVm.ObjSurveyResult, ServerFilePath);
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Save([DataSourceRequest] DataSourceRequest request, IEnumerable<HttpPostedFileBase> files, Int16 uploadID)
        {
            var savedFilePaths = new List<string>();
            string FileNameToSave = "";
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.

                    var fileName = Path.GetFileName(file.FileName);
                    var _container = CBUSA.App_Start.UnityConfig.GetConfiguredContainer();
                    var ObjRandom = _container.Resolve<IRandom>();
                    string FileNameRandom = ObjRandom.StringRandom("File");
                    string extension = System.IO.Path.GetExtension(fileName);
                    FileNameToSave = FileNameRandom + extension;
                    string physicalPath = Path.Combine(Server.MapPath("~/ApplicationDocument/SurveyDocument"), FileNameToSave);
                    file.SaveAs(physicalPath);
                    savedFilePaths.Add(FileNameToSave);



                }
            }

            return Json(new { IsUpload = true, UploadFileName = FileNameToSave });
            // Return an empty string to signify success
            // return Json(new[] { savedFilePaths }.ToDataSourceResult(request));
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            // Return an empty string to signify success
            return Json(new { IsUpload = false });
        }

        public FileResult DownloadResourceFile(string FileName)
        {
            string ServerFilePath = Server.MapPath("~/ApplicationDocument/SurveyDocument");
            var physicalPath = Path.Combine(ServerFilePath, FileName);
            return File(physicalPath, MimeMapping.GetMimeMapping(FileName), FileName);
        }
        #endregion


        #region Edit NCP Survey Response Rabi


        public ActionResult EditBuilderReport(Int64 ContractId, Int64 BuilderId, Int64 QuaterId)
        {
            ViewBag.Contract = ContractId;
            ViewBag.IsNcpSurvey = true;
            var Contract = _ObjContractService.GetContract(ContractId);
            if (Contract != null)
            {
                DateTime currentdate = DateTime.Now.Date;
                Quater ObjQuater = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();

                Survey ObjSurvey = _ObjSurveyService.GetContractSurvey(ContractId).Where(x => x.IsNcpSurvey == true).FirstOrDefault();

                BuilderReportViewModel ObjVM = new BuilderReportViewModel
                {
                    BuilderId = BuilderId,
                    ContractId = ContractId,
                    ContractName = Contract.ContractName,
                    ContractIcon = Contract.ContractIcon,
                    QuaterId = QuaterId,
                    SurveyId = ObjSurvey != null ? ObjSurvey.SurveyId : 0
                };
                return View(ObjVM);
            }
            return View(new BuilderReportViewModel { });
        }
        public ActionResult EditBuilderProjectReport(Int64 ContractId, Int64 BuilderId, Int64 QuaterId, Int64 ProjectId)
        {
            Quater ObjQuater = _ObjQuaterService.GetQuaterById(QuaterId);
            var QuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());

            var AdminQuaterProjectReport = _ObjBuilderQuaterContractProjectReportService.GetBuilderSeletedProjectReportForQuater(BuilderId, QuaterId, ContractId).ToList().Where(x => x.ProjectId == ProjectId);
            List<BuilderReportSubmitViewModel> AdminReportResult = new List<BuilderReportSubmitViewModel>();
            foreach (var Item in AdminQuaterProjectReport)
            {
                AdminReportResult.AddRange(Item.BuilderQuaterContractProjectDetails.Select(x => new BuilderReportSubmitViewModel
                {
                    ProjectId = Item.ProjectId,
                    FileName = x.FileName,
                    RowNumber = x.RowNumber,
                    ColumnNumber = x.ColumnNumber,
                    QuestionId = x.QuestionId,
                    Answer = x.Answer

                }).ToList());
            };

            if (AdminReportResult.Count > 0)
            {
                EditProjectResponseViewModel ObjVm = new EditProjectResponseViewModel
                {
                    QuestionList = QuestionList.ToList(),
                    SurveyId = QuestionList.FirstOrDefault() != null ? QuestionList.FirstOrDefault().SurveyId : 0,
                    ObjSubmitReportResult = AdminReportResult,
                    ContractId = ContractId,
                    ProjectId = ProjectId,
                    QuaterId = QuaterId,
                    BuilderId = BuilderId


                };
                return View(ObjVm);
            }
            else
            {
                return View(new EditProjectResponseViewModel { QuestionList = new List<Question> { } });
            }

        }
        public ActionResult SaveReportFile([DataSourceRequest] DataSourceRequest request, IEnumerable<HttpPostedFileBase> files, Int16 uploadID)
        {
            var savedFilePaths = new List<string>();
            string FileNameToSave = "";
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.

                    var fileName = Path.GetFileName(file.FileName);
                    var _container = CBUSA.App_Start.UnityConfig.GetConfiguredContainer();
                    var ObjRandom = _container.Resolve<IRandom>();
                    string FileNameRandom = ObjRandom.StringRandom("File");
                    string extension = System.IO.Path.GetExtension(fileName);
                    FileNameToSave = FileNameRandom + extension;
                    string physicalPath = Path.Combine(Server.MapPath("~/ApplicationDocument/SurveyDocument"), FileNameToSave);
                    file.SaveAs(physicalPath);
                    savedFilePaths.Add(FileNameToSave);



                }
            }

            return Json(new { IsUpload = true, UploadFileName = FileNameToSave });
            // Return an empty string to signify success
            // return Json(new[] { savedFilePaths }.ToDataSourceResult(request));
        }

        public ActionResult RemoveReportFile(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            // Return an empty string to signify success
            return Json(new { IsUpload = false });
        }

        [HttpPost]
        public JsonResult EditAdminReport(EditProjectResponseViewModel ObjVm)
        {
            try
            {
                if (ObjVm.ObjSubmitReportResult.Count > 0)
                {
                    //  var ProjectLis = ObjVm.SubmitReport.Select(x => x.ProjectId).Distinct().ToList();
                    var BuilderReport = ObjVm.ObjSubmitReportResult.Select(x => new BuilderQuaterContractProjectDetails
                    {
                        Answer = x.Answer,
                        RowNumber = x.RowNumber,
                        ColumnNumber = x.ColumnNumber,
                        QuestionId = x.QuestionId,
                        FileName = x.FileName,
                        BuilderQuaterContractProjectReport = new BuilderQuaterContractProjectReport { ProjectId = x.ProjectId },
                        Question = new Question { QuestionTypeId = x.QuestionTypeId }
                    }).ToList();

                    string ServerFilePath = Server.MapPath("~/ApplicationDocument/NCPRebateReportDocument");

                    _ObjBuilderQuaterContractProjectReportService.EditBuilderProjectResultByAdmin(ObjVm.ContractId, ObjVm.BuilderId, ObjVm.QuaterId, ObjVm.ProjectId,
                        BuilderReport, ServerFilePath);

                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Internal server error" }) }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}