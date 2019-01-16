using CBUSA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBUSA.Services.Interface;
using CBUSA.Areas.Admin.Models;
using CBUSA.Domain;
using CBUSA.Models;
using Newtonsoft.Json;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.Practices.Unity;
using System.Text;
using System.Security.Claims;
using System.IO;
//using MedullusSendGridEmailLib;
namespace CBUSA.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SurveyController : Controller
    {
        // GET: Admin/Survey

        readonly IContractServices _ObjContractService;
        readonly ISurveyService _ObjSurveyService;
        readonly IQuestionService _ObjQuestionService;
        readonly IBuilderService _ObjBuilderService;
        readonly ISurveyBuilderEmailSentService _ObjBuilderEmailSent;
        readonly IContractBuilderService _ObjContractBuilderService;
        readonly ISurveyBuilderService _ObjSurveyBuilderService;


        private const string contentFolderRoot = "~/ApplicationDocument/";
        private const string prettyName = "EditorImage/";
        private static readonly string[] foldersToCopy = new[] { "~/ApplicationDocument/EditorImage/" };
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";

        private const int ThumbnailHeight = 80;
        private const int ThumbnailWidth = 80;

        private readonly Models.DirectoryBrowser directoryBrowser;
        private readonly Models.ContentInitializer contentInitializer;
        private readonly Models.ThumbnailCreator thumbnailCreator;


        public SurveyController(IContractServices ObjContractService,
            ISurveyService ObjSurveyService, IQuestionService ObjQuestionService, IBuilderService ObjBuilderService,
            ISurveyBuilderEmailSentService ObjBuilderEmailSent, IContractBuilderService ObjContractBuilderService, ISurveyBuilderService ObjSurveyBuilderService
            )
        {
            _ObjContractService = ObjContractService;
            _ObjSurveyService = ObjSurveyService;
            _ObjQuestionService = ObjQuestionService;
            _ObjBuilderService = ObjBuilderService;

            _ObjBuilderEmailSent = ObjBuilderEmailSent;
            _ObjContractBuilderService = ObjContractBuilderService;
            _ObjSurveyBuilderService = ObjSurveyBuilderService;

            directoryBrowser = new Models.DirectoryBrowser();
            contentInitializer = new Models.ContentInitializer(contentFolderRoot, foldersToCopy, prettyName);
            thumbnailCreator = new Models.ThumbnailCreator();
        }

        public ActionResult Index()
        {
            return View();
        }

        #region survey details create

        public ActionResult SurveyDetails(Int64? SurveyId, Int16? IsNcpId)
        {
            bool IsNcp = false;


            if (IsNcpId.HasValue)
            {
                if (IsNcpId.GetValueOrDefault() == 1)
                {
                    IsNcp = true;
                    ViewBag.IsNcpSurvey = true;
                }
            }
            else
            {
                ViewBag.IsNcpSurvey = false;
            }

            if (SurveyId.GetValueOrDefault() == 0)
            {
                SurveyViewModel SurveyVm = new SurveyViewModel();
                SurveyVm.IsNcpSurvey = IsNcp;

                if (IsNcp)
                {
                    SurveyVm.QuaterList = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
                    {
                        Value = x,
                        Text = x
                    }).ToList();
                    SurveyVm.QuaterList.Insert(0, new SelectListItem { Text = "--Select Quarter--", Value = "0" });
                    //SurveyVm.YearList = _ObjSurveyService.GetYearAll().Select(x => new SelectListItem
                    //{
                    //    Value = x.ToString(),
                    //    Text = x.ToString()

                    //}).ToList();
                    SurveyVm.YearList = new List<SelectListItem> {
                      new SelectListItem{Text="--Select Year--", Value="0"},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year-1), Value=Convert.ToString(DateTime.Now.Year-1)},
                     new SelectListItem{Text=Convert.ToString(DateTime.Now.Year), Value=Convert.ToString(DateTime.Now.Year)},
                      new SelectListItem{Text=Convert.ToString(DateTime.Now.Year+1), Value=Convert.ToString(DateTime.Now.Year+1)},

                    };
                }
                else
                {
                    SurveyVm.QuaterList = new List<SelectListItem>();
                    SurveyVm.YearList = new List<SelectListItem>();
                }

                ViewBag.IsValidationSummaryMsgAvail = 0;

                return View(SurveyVm);
            }
            else
            {
                Survey ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault());

                if (ObjSurvey != null)
                {
                    SurveyViewModel SurveyVm = new SurveyViewModel();
                    SurveyVm.SurveyId = ObjSurvey.SurveyId;
                    SurveyVm.SurveyLabel = ObjSurvey.Label;
                    SurveyVm.SurveyName = ObjSurvey.SurveyName;
                    SurveyVm.StartDate = ObjSurvey.StartDate;
                    SurveyVm.EndDate = ObjSurvey.EndDate;
                    SurveyVm.ContractId = ObjSurvey.ContractId;
                    SurveyVm.IsEnrolment = ObjSurvey.IsEnrolment;
                    SurveyVm.IsNcpSurvey = IsNcp;

                    if (IsNcp)
                    {
                        SurveyVm.QuaterList = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
                        {
                            Value = x,
                            Text = x
                        }).ToList();
                        SurveyVm.QuaterList.Insert(0, new SelectListItem { Text = "--Select Quater--", Value = "0" });
                        //SurveyVm.YearList = _ObjSurveyService.GetYearAll().Select(x => new SelectListItem
                        //{
                        //    Value = x.ToString(),
                        //    Text = x.ToString()
                        //}).ToList();
                        SurveyVm.YearList = new List<SelectListItem> {
                             new SelectListItem{Text="--Select Year--", Value="0"},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year-1), Value=Convert.ToString(DateTime.Now.Year-1)},
                     new SelectListItem{Text=Convert.ToString(DateTime.Now.Year), Value=Convert.ToString(DateTime.Now.Year)},
                      new SelectListItem{Text=Convert.ToString(DateTime.Now.Year+1), Value=Convert.ToString(DateTime.Now.Year+1)},

                    };

                        SurveyVm.Year = ObjSurvey.Year;
                        SurveyVm.Quater = ObjSurvey.Quater;

                    }
                    else
                    {
                        SurveyVm.QuaterList = new List<SelectListItem>();
                        SurveyVm.YearList = new List<SelectListItem>();
                        SurveyVm.Year = "0";
                        SurveyVm.Quater = "";
                    }

                    Session["ContractName"] = _ObjContractService.GetContract(ObjSurvey.ContractId).ContractName;
                    Session["SurveyName"] = ObjSurvey.SurveyName;
                    ViewBag.IsValidationSummaryMsgAvail = 0;
                    ViewBag.SurveyId = SurveyId;
                    ViewBag.IsSurveyPublish = ObjSurvey.IsPublished;

                    return View(SurveyVm);
                }

                return RedirectToAction("SurveyDetails", "Survey", new { Area = "Admin" });
            }
        }
        public JsonResult GetActivePendingContract(bool? IsNcpSurvey)
        {
            if (IsNcpSurvey.HasValue)
            {
                var ContractList = _ObjContractService.GetActiveContract().Select(x => new
                {
                    ContractName = x.ContractName,
                    ContractId = x.ContractId
                });

                return Json(ContractList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var ContractList = _ObjContractService.GetActivePendingContractList().Select(x => new
                {
                    ContractName = x.ContractName,
                    ContractId = x.ContractId
                });

                return Json(ContractList, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult IsEnrollmentSurveyAvailable(Int64 ContractId)
        {
            bool Yes = _ObjSurveyService.IsEnrollmentAvailable(ContractId);
            return Json(new { IsSuccess = Yes }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult SaveSurvey(SurveyViewModel ObjSurveyVm)
        {
            if (ModelState.IsValid)
            {

                ObjSurveyVm.YearList = new List<SelectListItem> {
                      new SelectListItem{Text="--Select Year--", Value="0"},
                    new SelectListItem{Text=Convert.ToString(DateTime.Now.Year-1), Value=Convert.ToString(DateTime.Now.Year-1)},
                     new SelectListItem{Text=Convert.ToString(DateTime.Now.Year), Value=Convert.ToString(DateTime.Now.Year)},
                      new SelectListItem{Text=Convert.ToString(DateTime.Now.Year+1), Value=Convert.ToString(DateTime.Now.Year+1)},

                    };

                ObjSurveyVm.QuaterList = _ObjSurveyService.GetQuaterAll().Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                }).ToList();
                ObjSurveyVm.QuaterList.Insert(0, new SelectListItem { Text = "--Select Quater--", Value = "0" });


                if (ObjSurveyVm.SurveyId == 0)
                {
                    //Reassign properties 

                    if (ObjSurveyVm.IsEnrolment == true)
                    {
                        bool Yes = _ObjSurveyService.IsEnrollmentAvailable(ObjSurveyVm.ContractId);

                        if (!Yes)
                        {
                            ModelState.AddModelError("", "Contract already have Enrollment Survey");
                            ViewBag.IsValidationSummaryMsgAvail = 1;
                            string[] ModelError1 = ModelState.Values.Where(E => E.Errors.Count > 0)
                                .SelectMany(E => E.Errors)
                                .Select(E => E.ErrorMessage)
                                .ToArray();
                            ViewBag.IsValidationSummaryMsg = BuildModelError.GetModelError(ModelError1);
                            ViewBag.IsNcpSurvey = false;
                            return View("SurveyDetails", ObjSurveyVm);

                            // ModelState.AddModelError("", "Contract allready have Enrollment Survey");
                        }
                    }

                    if (ObjSurveyVm.IsNcpSurvey == true)
                    {
                        bool IsAvail = _ObjSurveyService.IsNcpSurveyAvailable(ObjSurveyVm.Quater, ObjSurveyVm.Year, ObjSurveyVm.ContractId);
                        if (!IsAvail)
                        {
                            ModelState.AddModelError("", "Cannot create Quarterly rebate report. A Quarterly rebate report for the selected contract and Year-Quarter already exists.");
                            ViewBag.IsValidationSummaryMsgAvail = 1;
                            string[] ModelErrorNcp = ModelState.Values.Where(E => E.Errors.Count > 0)
                                .SelectMany(E => E.Errors)
                                .Select(E => E.ErrorMessage)
                                .ToArray();
                            ViewBag.IsValidationSummaryMsg = BuildModelError.GetModelError(ModelErrorNcp);
                            ViewBag.IsNcpSurvey = true;
                            return View("SurveyDetails", ObjSurveyVm);
                        }
                    }

                    Int64 ContractCurrentStatus = _ObjContractService.GetContract(ObjSurveyVm.ContractId).ContractStatusId;
                    Survey ObjSurvey = new Survey();
                    //  ObjSurvey.SurveyId = ObjSurveyVm.SurveyId;
                    ObjSurvey.Label = ObjSurveyVm.SurveyLabel;
                    ObjSurvey.SurveyName = ObjSurveyVm.SurveyName;
                    ObjSurvey.StartDate = ObjSurveyVm.StartDate;
                    ObjSurvey.EndDate = ObjSurveyVm.EndDate;
                    ObjSurvey.ContractId = ObjSurveyVm.ContractId;

                    Session["ContractName"] = _ObjContractService.GetContract(ObjSurvey.ContractId).ContractName;
                    Session["SurveyName"] = ObjSurvey.SurveyName;

                    if (ObjSurveyVm.IsNcpSurvey)
                    {
                        ObjSurvey.IsNcpSurvey = true;

                        if (ObjSurveyVm.Year != "0")
                            ObjSurvey.Year = ObjSurveyVm.Year;
                        if (ObjSurveyVm.Quater != "0")
                            ObjSurvey.Quater = ObjSurveyVm.Quater;
                    }

                    else
                    {
                        ObjSurvey.IsEnrolment = ObjSurveyVm.IsEnrolment;
                    }



                    ObjSurvey.ContractCurrentStatus = ContractCurrentStatus;
                    ObjSurvey.CreatedOn = DateTime.Now;
                    ObjSurvey.CreatedBy = 1;
                    ObjSurvey.ModifiedOn = DateTime.Now;
                    ObjSurvey.ModifiedBy = 1;
                    ObjSurvey.RowStatusId = (int)RowActiveStatus.Active;
                    ObjSurvey.RowGUID = Guid.NewGuid();
                    _ObjSurveyService.SaveSurvey(ObjSurvey);

                    return RedirectToAction("AddQuestion", "Survey", new { Area = "Admin", SurveyId = ObjSurvey.SurveyId });
                    ///


                }
                else
                {
                    Survey ObjSurvey = _ObjSurveyService.GetSurvey(ObjSurveyVm.SurveyId);

                    if (ObjSurvey != null)
                    {

                        if (ObjSurveyVm.IsEnrollmentChange == 1)
                        {
                            if (ObjSurveyVm.IsEnrolment == true)
                            {
                                bool Yes = _ObjSurveyService.IsEnrollmentAvailable(ObjSurveyVm.ContractId);
                                if (!Yes)
                                {
                                    ModelState.AddModelError("", "Contract allready have Enrollment Survey");
                                    ViewBag.IsValidationSummaryMsgAvail = 1;
                                    string[] ModelError1 = ModelState.Values.Where(E => E.Errors.Count > 0)
                                        .SelectMany(E => E.Errors)
                                        .Select(E => E.ErrorMessage)
                                        .ToArray();
                                    ViewBag.IsValidationSummaryMsg = BuildModelError.GetModelError(ModelError1);
                                    return View("SurveyDetails", ObjSurveyVm);
                                }
                            }
                        }
                        if (ObjSurveyVm.IsNcpSurvey == true)
                        {

                            if (ObjSurveyVm.Quater == null || ObjSurveyVm.Year == null)
                            {
                                bool IsAvail = _ObjSurveyService.IsNcpSurveyAvailable(ObjSurveyVm.Quater, ObjSurveyVm.Year, ObjSurveyVm.ContractId);
                                if (!IsAvail)
                                {

                                    ModelState.AddModelError("", "Cannot create Quarterly rebate report. A Quarterly rebate report for the selected contract and Year-Quarter already exists.");
                                    ViewBag.IsValidationSummaryMsgAvail = 1;
                                    string[] ModelErrorNcp = ModelState.Values.Where(E => E.Errors.Count > 0)
                                        .SelectMany(E => E.Errors)
                                        .Select(E => E.ErrorMessage)
                                        .ToArray();
                                    ViewBag.IsValidationSummaryMsg = BuildModelError.GetModelError(ModelErrorNcp);
                                    ViewBag.IsNcpSurvey = true;
                                    return View("SurveyDetails", ObjSurveyVm);
                                }
                            }
                        }

                        ObjSurvey.Label = ObjSurveyVm.SurveyLabel;
                        ObjSurvey.SurveyName = ObjSurveyVm.SurveyName;
                        ObjSurvey.StartDate = ObjSurveyVm.StartDate;
                        ObjSurvey.EndDate = ObjSurveyVm.EndDate;
                        ObjSurvey.ContractId = ObjSurveyVm.ContractId;


                        if (ObjSurveyVm.IsNcpSurvey)
                        {
                            ObjSurvey.IsNcpSurvey = true;
                            if (ObjSurveyVm.Year != "0")
                                ObjSurvey.Year = ObjSurveyVm.Year;
                            if (ObjSurveyVm.Quater != "0")
                                ObjSurvey.Quater = ObjSurveyVm.Quater;
                        }

                        else
                        {
                            // ObjSurvey.IsEnrolment = ObjSurveyVm.IsEnrolment;



                            if (ObjSurveyVm.IsEnrollmentChange == 1)
                            {
                                ObjSurvey.IsEnrolment = ObjSurveyVm.IsEnrolment;
                                if (ObjSurveyVm.IsEnrolment == true)
                                {

                                    Int64 ContractCurrentStatus = _ObjContractService.GetContract(ObjSurveyVm.ContractId).ContractStatusId;
                                    ObjSurvey.ContractCurrentStatus = ContractCurrentStatus;
                                }
                                else
                                {
                                    Int64 ContractCurrentStatus = _ObjContractService.GetContract(ObjSurveyVm.ContractId).ContractStatusId;
                                    ObjSurvey.ContractCurrentStatus = 0;
                                }
                            }
                        }
                        // 
                        ObjSurvey.ModifiedOn = DateTime.Now;
                        ObjSurvey.ModifiedBy = 1;
                        _ObjSurveyService.UpdateSurvey(ObjSurvey);
                        return RedirectToAction("AddQuestion", "Survey", new { Area = "Admin", SurveyId = ObjSurvey.SurveyId });
                    }
                }

            }
            ViewBag.IsValidationSummaryMsgAvail = 1;
            string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                .SelectMany(E => E.Errors)
                .Select(E => E.ErrorMessage)
                .ToArray();
            ViewBag.IsValidationSummaryMsg = BuildModelError.GetModelError(ModelError);
            return View("SurveyDetails", ObjSurveyVm);
        }



        #endregion



        #region AddQuestion

        public ActionResult AddQuestion(Int64? SurveyId, Int64? QuestionId, Int32? IsCopy)
        {

            // if(SurveyId)


            ViewBag.IsCopy = 0;
            if (SurveyId.HasValue)
            {

                Survey ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault());



                if (ObjSurvey != null)
                {
                    if (ObjSurvey.IsNcpSurvey)
                    {
                        ViewBag.IsNcpSurvey = true;
                    }
                    else
                    {
                        ViewBag.IsNcpSurvey = false;
                    }
                    ViewBag.IsSurveyPublish = ObjSurvey.IsPublished;
                    if (QuestionId.HasValue)
                    {
                        Question _ObjQuestion = _ObjQuestionService.GetQuestion(QuestionId.GetValueOrDefault(), SurveyId.GetValueOrDefault());

                        AddQuestionViewModel ObjVM = new AddQuestionViewModel();
                        if (IsCopy.HasValue)
                        {
                            if (IsCopy.GetValueOrDefault() == 1)// for copy
                            {
                                ViewBag.IsCopy = 1;
                                ObjVM.ObjQuestion = new Question
                                {
                                    SurveyId = _ObjQuestion.SurveyId,
                                    //  QuestionId = _ObjQuestion.QuestionId,
                                    QuestionValue = _ObjQuestion.QuestionValue,
                                    IsMandatory = _ObjQuestion.IsMandatory,
                                    IsFileNeedtoUpload = _ObjQuestion.IsFileNeedtoUpload,
                                    QuestionTypeId = _ObjQuestion.QuestionTypeId
                                };
                            }
                            else
                            {
                                ObjVM.ObjQuestion = new Question
                                {
                                    SurveyId = _ObjQuestion.SurveyId,
                                    QuestionId = _ObjQuestion.QuestionId,
                                    QuestionValue = _ObjQuestion.QuestionValue,
                                    IsMandatory = _ObjQuestion.IsMandatory,
                                    IsFileNeedtoUpload = _ObjQuestion.IsFileNeedtoUpload,
                                    QuestionTypeId = _ObjQuestion.QuestionTypeId
                                };
                            }
                        }
                        else
                        {
                            ObjVM.ObjQuestion = new Question
                            {
                                SurveyId = _ObjQuestion.SurveyId,
                                QuestionId = _ObjQuestion.QuestionId,
                                QuestionValue = _ObjQuestion.QuestionValue,
                                IsMandatory = _ObjQuestion.IsMandatory,
                                IsFileNeedtoUpload = _ObjQuestion.IsFileNeedtoUpload,
                                QuestionTypeId = _ObjQuestion.QuestionTypeId
                            };
                        }
                        if (_ObjQuestion.QuestionTextBoxSetting.Count > 0)
                        {
                            ObjVM.ObjTextBoxSetting = new QuestionTextBoxSetting
                            {
                                QuestionTextBoxSettingId = _ObjQuestion.QuestionTextBoxSetting.FirstOrDefault().QuestionTextBoxSettingId,
                                IsAlphabets = _ObjQuestion.QuestionTextBoxSetting.FirstOrDefault().IsAlphabets,
                                IsNumber = _ObjQuestion.QuestionTextBoxSetting.FirstOrDefault().IsNumber,
                                IsSpecialCharecter = _ObjQuestion.QuestionTextBoxSetting.FirstOrDefault().IsSpecialCharecter,
                                LowerLimit = _ObjQuestion.QuestionTextBoxSetting.FirstOrDefault().LowerLimit,
                                UpperLimit = _ObjQuestion.QuestionTextBoxSetting.FirstOrDefault().UpperLimit,
                                TextBoxTypeId = _ObjQuestion.QuestionTextBoxSetting.FirstOrDefault().TextBoxTypeId,

                            };

                            ObjVM.ObjTextBoxType = new TextBoxType { TextBoxTypeId = _ObjQuestion.QuestionTextBoxSetting.FirstOrDefault().TextBoxTypeId };


                            ObjVM.ObjDropDownSetting = new QuestionDropdownSetting();
                            ObjVM.ObjGridSetting = new QuestionGridSetting();
                            ObjVM.ObjQuestionGridSettingHeader = new QuestionGridSettingHeaderViewModel();

                        }
                        else if (_ObjQuestion.QuestionDropdownSetting.Count > 0)
                        {
                            ObjVM.ObjDropDownSetting = new QuestionDropdownSetting
                            {
                                QuestionDropdownSettingId = _ObjQuestion.QuestionDropdownSetting.FirstOrDefault().QuestionDropdownSettingId,
                                Value = string.Join(",", _ObjQuestion.QuestionDropdownSetting.Select(x => x.Value))
                            };


                            ObjVM.ObjGridSetting = new QuestionGridSetting();
                            ObjVM.ObjTextBoxSetting = new QuestionTextBoxSetting();
                            ObjVM.ObjTextBoxType = new TextBoxType();
                            ObjVM.ObjQuestionGridSettingHeader = new QuestionGridSettingHeaderViewModel();
                        }
                        else if (_ObjQuestion.QuestionGridSetting.Count > 0)
                        {
                            ObjVM.ObjGridSetting = new QuestionGridSetting
                            {
                                Row = _ObjQuestion.QuestionGridSetting.FirstOrDefault().Row,
                                Column = _ObjQuestion.QuestionGridSetting.FirstOrDefault().Column,

                            };

                            ObjVM.ObjQuestionGridSettingHeader = new QuestionGridSettingHeaderViewModel
                            {
                                RowHeaderValue = string.Join("####RowValue####", _ObjQuestion.QuestionGridSetting.FirstOrDefault().
                                QuestionGridSettingHeader.Where(x => x.ColoumnHeaderValue == null).Select(y => y.RowHeaderValue)),
                                ColoumnHeaderValue = JsonConvert.SerializeObject(_ObjQuestion.QuestionGridSetting.FirstOrDefault().
                                QuestionGridSettingHeader.Where(x => x.RowHeaderValue == null).OrderBy(z => z.IndexNumber).Select(y => new
                                {
                                    ColVal = y.ColoumnHeaderValue,
                                    IndexNo = y.IndexNumber,
                                    CltrType = y.ControlType,
                                    DropDownValue = y.DropdownTypeOptionValue
                                }))

                            };


                            ObjVM.ObjDropDownSetting = new QuestionDropdownSetting();
                            ObjVM.ObjTextBoxSetting = new QuestionTextBoxSetting();
                            ObjVM.ObjTextBoxType = new TextBoxType();

                        }



                        return View(ObjVM);
                    }
                    else
                    {
                        AddQuestionViewModel ObjVM = new AddQuestionViewModel();
                        ObjVM.ObjQuestion = new Question { SurveyId = SurveyId.GetValueOrDefault() };
                        ObjVM.ObjTextBoxType = new TextBoxType();
                        ObjVM.ObjTextBoxSetting = new QuestionTextBoxSetting();
                        ObjVM.ObjDropDownSetting = new QuestionDropdownSetting();
                        ObjVM.ObjGridSetting = new QuestionGridSetting();
                        ObjVM.ObjTextBoxSetting = new QuestionTextBoxSetting();
                        ObjVM.ObjQuestionGridSettingHeader = new QuestionGridSettingHeaderViewModel();

                        return View(ObjVM);
                    }
                }
                return RedirectToAction("SurveyDetails", "Survey");
            }
            else
            {
                return RedirectToAction("SurveyDetails", "Survey");
            }


        }

        public JsonResult GetQuestionType()
        {
            var QuestionTypelist = _ObjQuestionService.GetQuestionTypeAll().Select(x => new
            {
                TypeName = x.TypeName,
                QuestionTypeId = x.QuestionTypeId
            });

            return Json(QuestionTypelist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTextBoxType()
        {
            var TextBoxTypeList = _ObjQuestionService.GetTextBoxTypeAll().Select(x => new
            {
                TextBoxTypeName = x.TextBoxTypeName,
                TextBoxTypeId = x.TextBoxTypeId
            });

            return Json(TextBoxTypeList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ShowAddGridColoumnDropDownvalue(string ControlValue)
        {
            return PartialView("_AddGridColoumnDropDownvalue", ControlValue);
        }


        public JsonResult SaveQuestion(AddQuestionViewModel ObjVm)
        {


            if (ModelState.IsValid)
            {
                if (ObjVm.ObjQuestion.QuestionTypeId == (int)EnumQuestionType.TextBox)
                {

                    if (ObjVm.ObjTextBoxSetting.TextBoxTypeId == 1)
                    {
                        if (!ObjVm.ObjTextBoxSetting.IsAlphabets && !ObjVm.ObjTextBoxSetting.IsNumber && !ObjVm.ObjTextBoxSetting.IsSpecialCharecter)
                        {
                            ModelState.AddModelError("", "Please select allow charecter");
                        }

                    }
                    if (ObjVm.ObjTextBoxSetting.TextBoxTypeId == 3)
                    {
                        if (ObjVm.ObjTextBoxSetting.LowerLimit > ObjVm.ObjTextBoxSetting.UpperLimit)
                        {
                            ModelState.AddModelError("", "Please add correct range ");
                        }
                    }
                }


                else if (ObjVm.ObjQuestion.QuestionTypeId == (int)EnumQuestionType.Grid)
                {
                    if (ObjVm.ObjGridSetting.Row == 0 || ObjVm.ObjGridSetting.Column == 0)
                    {
                        ModelState.AddModelError("", "Row and coloumn no must be greater than zero");
                    }
                    if (ObjVm.ObjQuestionGridSettingHeader.RowHeaderValue == null)
                    {
                        ModelState.AddModelError("", "You are not allowed to create question with out any row header");
                    }
                    if (ObjVm.ObjQuestionGridSettingHeader.ColoumnHeaderValue == null)
                    {
                        ModelState.AddModelError("", "You are not allowed to create question with out any coloumn header");
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

                if (ObjVm.ObjQuestion.QuestionId == 0)  //for inser data
                {

                    Question _ObjQuestion = new Question();
                    _ObjQuestion.QuestionTypeId = ObjVm.ObjQuestion.QuestionTypeId;
                    _ObjQuestion.IsFileNeedtoUpload = ObjVm.ObjQuestion.IsFileNeedtoUpload;
                    _ObjQuestion.IsMandatory = ObjVm.ObjQuestion.IsMandatory;
                    _ObjQuestion.QuestionValue = ObjVm.ObjQuestion.QuestionValue;
                    // _ObjQuestion.SurveyOrder = ObjVm.ObjQuestion.;

                    _ObjQuestion.SurveyId = ObjVm.ObjQuestion.SurveyId;
                    _ObjQuestion.RowStatusId = (int)RowActiveStatus.Active;
                    _ObjQuestion.CreatedBy = 1;
                    _ObjQuestion.ModifiedBy = 1;
                    _ObjQuestion.CreatedOn = DateTime.Now;
                    _ObjQuestion.ModifiedOn = DateTime.Now;
                    _ObjQuestion.RowGUID = Guid.NewGuid();

                    switch (ObjVm.ObjQuestion.QuestionTypeId)
                    {
                        case (int)EnumQuestionType.TextBox:

                            QuestionTextBoxSetting _ObjQuestionTextBoxSetting = new QuestionTextBoxSetting();
                            _ObjQuestionTextBoxSetting.IsAlphabets = ObjVm.ObjTextBoxSetting.IsAlphabets;
                            _ObjQuestionTextBoxSetting.IsNumber = ObjVm.ObjTextBoxSetting.IsNumber;
                            _ObjQuestionTextBoxSetting.IsSpecialCharecter = ObjVm.ObjTextBoxSetting.IsSpecialCharecter;
                            _ObjQuestionTextBoxSetting.LowerLimit = ObjVm.ObjTextBoxSetting.LowerLimit;
                            _ObjQuestionTextBoxSetting.UpperLimit = ObjVm.ObjTextBoxSetting.UpperLimit;
                            _ObjQuestionTextBoxSetting.TextBoxTypeId = ObjVm.ObjTextBoxSetting.TextBoxTypeId;
                            _ObjQuestionService.SaveTextBoxQuestion(_ObjQuestion, _ObjQuestionTextBoxSetting);

                            break;
                        case (int)EnumQuestionType.DropList:

                            List<QuestionDropdownSetting> List = new List<QuestionDropdownSetting>();
                            string[] DropdownListValue = ObjVm.ObjDropDownSetting.Value.Split(',');

                            foreach (string child in DropdownListValue)
                            {
                                QuestionDropdownSetting _ObjQuestionDropdownSetting = new QuestionDropdownSetting();
                                _ObjQuestionDropdownSetting.Value = child;
                                List.Add(_ObjQuestionDropdownSetting);
                            }


                            _ObjQuestionService.SaveDropListQuestion(_ObjQuestion, List);




                            break;
                        case (int)EnumQuestionType.Grid:


                            List<QuestionGridSettingHeader> _ObjQuestionGridHeaderSettingList = new List<QuestionGridSettingHeader>();
                            int Flag = 1;
                            int Row = 0;
                            int Coloumn = 0;
                            if (ObjVm.ObjQuestionGridSettingHeader.RowHeaderValue != null)
                            {
                                string[] HeaderValue = ObjVm.ObjQuestionGridSettingHeader.RowHeaderValue.Split(new string[] { "####RowValue####" }, StringSplitOptions.None);

                                foreach (var Child in HeaderValue)
                                {
                                    QuestionGridSettingHeader Obj = new QuestionGridSettingHeader();
                                    Obj.RowHeaderValue = Child;
                                    Obj.IndexNumber = Flag;

                                    Flag = Flag + 1;

                                    _ObjQuestionGridHeaderSettingList.Add(Obj);
                                    Row = Row + 1;
                                }
                            }

                            string[] ColoumnValue = ObjVm.ObjQuestionGridSettingHeader.ColoumnHeaderValue.Split(new string[] { "####ColValue####" }, StringSplitOptions.None);
                            List<GridColoumnControlViewModel> ObjColoumnCltr = JsonConvert.DeserializeObject<List<GridColoumnControlViewModel>>(ObjVm.ObjQuestionGridSettingHeader.ColoumnControlValue);

                            foreach (var Child in ObjColoumnCltr)
                            {
                                QuestionGridSettingHeader Obj = new QuestionGridSettingHeader();
                                Obj.ColoumnHeaderValue = ColoumnValue[Child.ControIndex - 1];
                                Obj.IndexNumber = Child.ControIndex;
                                Obj.ControlType = Child.ControlType;
                                Obj.DropdownTypeOptionValue = Child.ControlValue;
                                _ObjQuestionGridHeaderSettingList.Add(Obj);
                                Coloumn = Coloumn + 1;
                            }

                            QuestionGridSetting _ObjQuestionGridSetting = new QuestionGridSetting();
                            _ObjQuestionGridSetting.Row = Row;
                            _ObjQuestionGridSetting.Column = Coloumn;
                            _ObjQuestionService.SaveGridQuestion(_ObjQuestion, _ObjQuestionGridSetting, _ObjQuestionGridHeaderSettingList);
                            break;
                    }
                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }

                if (ObjVm.ObjQuestion.QuestionId != 0)  //for Edit data
                {

                    Question _ObjQuestion = _ObjQuestionService.GetQuestion(ObjVm.ObjQuestion.QuestionId, ObjVm.ObjQuestion.SurveyId);
                    _ObjQuestion.QuestionTypeId = ObjVm.ObjQuestion.QuestionTypeId;
                    _ObjQuestion.IsFileNeedtoUpload = ObjVm.ObjQuestion.IsFileNeedtoUpload;
                    _ObjQuestion.IsMandatory = ObjVm.ObjQuestion.IsMandatory;
                    _ObjQuestion.QuestionValue = ObjVm.ObjQuestion.QuestionValue;
                    _ObjQuestion.ModifiedBy = 1;
                    _ObjQuestion.ModifiedOn = DateTime.Now;

                    _ObjQuestionService.DeleteTextDropDownGridSettingData(_ObjQuestion);


                    switch (ObjVm.ObjQuestion.QuestionTypeId)
                    {
                        case (int)EnumQuestionType.TextBox:

                            QuestionTextBoxSetting _ObjQuestionTextBoxSetting = new QuestionTextBoxSetting();
                            _ObjQuestionTextBoxSetting.QuestionId = _ObjQuestion.QuestionId;
                            _ObjQuestionTextBoxSetting.IsAlphabets = ObjVm.ObjTextBoxSetting.IsAlphabets;
                            _ObjQuestionTextBoxSetting.IsNumber = ObjVm.ObjTextBoxSetting.IsNumber;
                            _ObjQuestionTextBoxSetting.IsSpecialCharecter = ObjVm.ObjTextBoxSetting.IsSpecialCharecter;
                            _ObjQuestionTextBoxSetting.LowerLimit = ObjVm.ObjTextBoxSetting.LowerLimit;
                            _ObjQuestionTextBoxSetting.UpperLimit = ObjVm.ObjTextBoxSetting.UpperLimit;
                            _ObjQuestionTextBoxSetting.TextBoxTypeId = ObjVm.ObjTextBoxSetting.TextBoxTypeId;
                            _ObjQuestionService.EditTextBoxQuestion(_ObjQuestion, _ObjQuestionTextBoxSetting);

                            break;
                        case (int)EnumQuestionType.DropList:

                            List<QuestionDropdownSetting> List = new List<QuestionDropdownSetting>();
                            string[] DropdownListValue = ObjVm.ObjDropDownSetting.Value.Split(',');

                            foreach (string child in DropdownListValue)
                            {
                                QuestionDropdownSetting _ObjQuestionDropdownSetting = new QuestionDropdownSetting();
                                _ObjQuestionDropdownSetting.Value = child;
                                _ObjQuestionDropdownSetting.QuestionId = _ObjQuestion.QuestionId;
                                List.Add(_ObjQuestionDropdownSetting);
                            }
                            _ObjQuestionService.EditDropListQuestion(_ObjQuestion, List);




                            break;
                        case (int)EnumQuestionType.Grid:

                            QuestionGridSetting _ObjQuestionGridSetting = new QuestionGridSetting();

                            _ObjQuestionGridSetting.QuestionId = _ObjQuestion.QuestionId;





                            string[] HeaderValue = ObjVm.ObjQuestionGridSettingHeader.RowHeaderValue.Split(new string[] { "####RowValue####" }, StringSplitOptions.None);
                            List<QuestionGridSettingHeader> _ObjQuestionGridHeaderSettingList = new List<QuestionGridSettingHeader>();

                            int Flag = 1;
                            int Row = 0;
                            int Coloumn = 0;
                            foreach (var Child in HeaderValue)
                            {
                                QuestionGridSettingHeader Obj = new QuestionGridSettingHeader();
                                Obj.RowHeaderValue = Child;
                                Obj.IndexNumber = Flag;

                                Flag = Flag + 1;

                                _ObjQuestionGridHeaderSettingList.Add(Obj);
                                Row = Row + 1;
                            }

                            string[] ColoumnValue = ObjVm.ObjQuestionGridSettingHeader.ColoumnHeaderValue.Split(new string[] { "####ColValue####" }, StringSplitOptions.None);
                            List<GridColoumnControlViewModel> ObjColoumnCltr = JsonConvert.DeserializeObject<List<GridColoumnControlViewModel>>(ObjVm.ObjQuestionGridSettingHeader.ColoumnControlValue);

                            foreach (var Child in ObjColoumnCltr)
                            {
                                QuestionGridSettingHeader Obj = new QuestionGridSettingHeader();
                                Obj.ColoumnHeaderValue = ColoumnValue[Child.ControIndex - 1];
                                Obj.IndexNumber = Child.ControIndex;
                                Obj.ControlType = Child.ControlType;
                                Obj.DropdownTypeOptionValue = Child.ControlValue;
                                _ObjQuestionGridHeaderSettingList.Add(Obj);
                                Coloumn = Coloumn + 1;
                            }

                            _ObjQuestionGridSetting.Row = Row;
                            _ObjQuestionGridSetting.Column = Coloumn;

                            _ObjQuestionService.EditGridQuestion(_ObjQuestion, _ObjQuestionGridSetting, _ObjQuestionGridHeaderSettingList);
                            break;
                    }
                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }



            }



            string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                 .SelectMany(E => E.Errors)
                 .Select(E => E.ErrorMessage)
                 .ToArray();


            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);


            //AddQuestionViewModel ObjVM = new AddQuestionViewModel();
            //ObjVM.ObjQuestion = new Question();
            //ObjVM.ObjTextBoxType = new TextBoxType();
            //ObjVM.ObjTextBoxSetting = new QuestionTextBoxSetting();
            //ObjVM.ObjDropDownSetting = new QuestionDropdownSetting();
            //ObjVM.ObjGridSetting = new QuestionGridSetting();
            //ObjVM.ObjTextBoxSetting = new QuestionTextBoxSetting();

            //ObjVM.ObjQuestion = new Question();


        }

        #endregion

        #region Configureinvitess
        public ActionResult ConfigureInvites(Int64? SurveyId)
        {
            if (SurveyId.HasValue)
            {
                Survey ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault());
                //  bool[] SurveyStatus=_ObjSurveyService.IsNcpSurvey(SurveyId.GetValueOrDefault());
                if (ObjSurvey != null)
                {
                    ViewBag.SurveyId = SurveyId;
                    ViewBag.IsSurveyPublish = ObjSurvey.IsPublished;
                    if (ObjSurvey.IsNcpSurvey)
                    {
                        ViewBag.IsNcpSurvey = true;
                    }
                    else
                    {
                        ViewBag.IsNcpSurvey = false;
                    }
                    return View();
                }
            }
            return RedirectToAction("SurveyDetails", "Survey", new { Area = "Admin" });
        }

        public ActionResult ShowSurveyAddInvitesPopup(string ControlValue)
        {
            return PartialView("_SurveyAddInvitesPopup", ControlValue);
        }

        public ActionResult SurveyMarketBuilderList_Read([DataSourceRequest] DataSourceRequest request, Int64 SurveyId)
        {
            Survey _ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId);
            bool IsSurveyPublish = _ObjSurvey.IsPublished;
            var SurveyMarketList = _ObjSurvey.SurveyMarket.Where(y => y.Market.RowStatusId == (Int32)RowActiveStatus.Active).OrderBy(x => x.Market.MarketName).Select(x => new
            {
                MarketId = x.MarketId,
                MarketName = x.Market.MarketName,
                IsSurveyPublished = IsSurveyPublish,
                BuilderCount = x.Market.Bulder.Where(y => y.RowStatusId == (Int32)RowActiveStatus.Active).Count()
            });




            return Json(SurveyMarketList.ToDataSourceResult(request));
        }


        public JsonResult SaveSurveyMarket(List<Int64> MarketList, Int64 SurveyId)
        {

            Survey _ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId);
            List<Int64> SavedMarketList = MarketList.Except(_ObjSurvey.SurveyMarket.Select(x => x.MarketId)).ToList();
            if (SavedMarketList.Count > 0)
            {
                _ObjSurveyService.SaveSurveyMarket(SavedMarketList, SurveyId);

                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                ModelState.AddModelError("", "These market allready in database..");
                string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                 .SelectMany(E => E.Errors)
                 .Select(E => E.ErrorMessage)
                 .ToArray();
                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult RemoveSurveyMarket(Int64 SurveyId, Int64 MarketId)
        {


            _ObjSurveyService.RemoveSurveyMarket(MarketId, SurveyId);

            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region SurveySettings
        public ActionResult SurveySettings(Int64? SurveyId)
        {
            if (SurveyId.HasValue)
            {
                bool[] SurveyStatus = _ObjSurveyService.IsNcpSurvey(SurveyId.GetValueOrDefault());
                //   Survey ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault());
                if (SurveyStatus[0] == true)
                {
                    if (SurveyStatus[1])
                    {
                        ViewBag.IsNcpSurvey = true;
                    }
                    else
                    {
                        ViewBag.IsNcpSurvey = false;
                    }

                    SurveyEmailSetting _ObjEmailSetting = _ObjSurveyService.GetSurveyEmailSetting((SurveyId.GetValueOrDefault()));
                    if (_ObjEmailSetting != null)
                    {
                        ViewBag.IsSurveyPublish = _ObjEmailSetting.Survey.IsPublished;
                        SurveySettingsViewModel ObjVm = new SurveySettingsViewModel
                        {
                            SurveyId = SurveyId.GetValueOrDefault(),
                            SenderEmail = _ObjEmailSetting.SenderEmail,
                            RemainderForContinueSurvey = _ObjEmailSetting.RemainderForContinueSurvey,
                            RemainderForTakeSurvey = _ObjEmailSetting.RemainderForTakeSurvey,
                            RemainderForTakeSurveySecond = _ObjEmailSetting.RemainderForTakeSurveySecond,
                            RemainderForTakeSurveyThird = _ObjEmailSetting.RemainderForTakeSurveyThird,
                            DayBeforeSurveyEnd = _ObjEmailSetting.DayBeforeSurveyEnd,
                            DayBeforeSurveyEndSecond = _ObjEmailSetting.DayBeforeSurveyEndSecond,
                            DayBeforeSurveyEndThird = _ObjEmailSetting.DayBeforeSurveyEndThird,
                            DayAfterSurveyEnd = _ObjEmailSetting.DayAfterSurveyEnd
                        };
                        return View(ObjVm);
                    }
                    else
                    {
                        ViewBag.IsSurveyPublish = false;
                        SurveySettingsViewModel ObjVm = new SurveySettingsViewModel
                        {
                            SurveyId = SurveyId.GetValueOrDefault()
                        };

                        return View(ObjVm);
                    }
                }
            }
            return RedirectToAction("SurveyDetails", "Survey", new { Area = "Admin" });
        }
        
        public JsonResult SaveSurveyEmailSettings(SurveySettingsViewModel ObjVm)
        {

            if (ModelState.IsValid)
            {
                if (ObjVm.RemainderForTakeSurvey)
                {
                    if (ObjVm.DayBeforeSurveyEnd == 0)
                    {
                        ModelState.AddModelError("", "Please add corrcet days..before the email goes out");
                    }
                }
                if (ObjVm.RemainderForTakeSurveySecond)
                {
                    if (ObjVm.DayBeforeSurveyEndSecond == 0)
                    {
                        ModelState.AddModelError("", "Please add corrcet days..after the email goes out");
                    }
                }
                if (ObjVm.RemainderForTakeSurveyThird)
                {
                    if (ObjVm.DayBeforeSurveyEndThird == 0)
                    {
                        ModelState.AddModelError("", "Please add corrcet days..after the email goes out");
                    }
                }

                if (ObjVm.RemainderForContinueSurvey)
                {
                    if (ObjVm.DayAfterSurveyEnd == 0)
                    {
                        ModelState.AddModelError("", "Please add corrcet days..after the email goes out");
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
                SurveyEmailSetting _ObjEmailSetting = _ObjSurveyService.GetSurveyEmailSetting(ObjVm.SurveyId);

                if (_ObjEmailSetting == null)
                {

                    SurveyEmailSetting _Obj = new SurveyEmailSetting
                    {

                        SurveyId = ObjVm.SurveyId,
                        SenderEmail = ObjVm.SenderEmail,
                        RemainderForContinueSurvey = ObjVm.RemainderForContinueSurvey,
                        RemainderForTakeSurvey = ObjVm.RemainderForTakeSurvey,
                        RemainderForTakeSurveySecond = ObjVm.RemainderForTakeSurveySecond,
                        RemainderForTakeSurveyThird = ObjVm.RemainderForTakeSurveyThird,


                        DayBeforeSurveyEnd = ObjVm.DayBeforeSurveyEnd,

                        DayBeforeSurveyEndSecond = ObjVm.DayBeforeSurveyEndSecond,
                        DayBeforeSurveyEndThird = ObjVm.DayBeforeSurveyEndThird,

                        DayAfterSurveyEnd = ObjVm.DayAfterSurveyEnd


                    };
                    SurveyInviteEmailSetting _ObjSurveyInviteEmailSetting = null;

                    if (ObjVm.InviteEmailDumpId != "0")
                    {
                        _ObjSurveyInviteEmailSetting = _ObjSurveyService.GetSurveyInviteEmailSetting(ObjVm.InviteEmailDumpId);
                        _ObjSurveyInviteEmailSetting.SurveyEmailSettingId = _Obj.SurveyEmailSettingId;

                    }

                    SurveyRemainderEmailSetting _ObjSurveyRemainderEmailSetting = null;
                    if (ObjVm.RemainderEmailDumpId != "0")
                    {
                        _ObjSurveyRemainderEmailSetting = _ObjSurveyService.GetSurveyRemainderEmailSetting(ObjVm.RemainderEmailDumpId);
                        _ObjSurveyRemainderEmailSetting.SurveyEmailSettingId = _Obj.SurveyEmailSettingId;

                    }

                    SurveySaveContinueEmailSetting _ObjSurveySaveContinueEmailSetting = null;
                    if (ObjVm.ContinueEmailDumpId != "0")
                    {
                        _ObjSurveySaveContinueEmailSetting = _ObjSurveyService.GetSurveySaveContinueEmailSetting(ObjVm.ContinueEmailDumpId);
                        _ObjSurveySaveContinueEmailSetting.SurveyEmailSettingId = _Obj.SurveyEmailSettingId;

                    }

                    _ObjSurveyService.SaveSurveyEmailSetting(_Obj, _ObjSurveyInviteEmailSetting, _ObjSurveyRemainderEmailSetting, _ObjSurveySaveContinueEmailSetting);

                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    SurveyEmailSetting _Obj = _ObjEmailSetting;

                    //SurveyId = ObjVm.SurveyId,
                    _Obj.SenderEmail = ObjVm.SenderEmail;
                    _Obj.RemainderForContinueSurvey = ObjVm.RemainderForContinueSurvey;
                    _Obj.RemainderForTakeSurvey = ObjVm.RemainderForTakeSurvey;
                    _Obj.DayBeforeSurveyEnd = ObjVm.DayBeforeSurveyEnd;
                    _Obj.DayAfterSurveyEnd = ObjVm.DayAfterSurveyEnd;

                    _Obj.RemainderForTakeSurveySecond = ObjVm.RemainderForTakeSurveySecond;
                    _Obj.RemainderForTakeSurveyThird = ObjVm.RemainderForTakeSurveyThird;

                    _Obj.DayBeforeSurveyEndSecond = ObjVm.DayBeforeSurveyEndSecond;
                    _Obj.DayBeforeSurveyEndThird = ObjVm.DayBeforeSurveyEndThird;

                    SurveyInviteEmailSetting _ObjSurveyInviteEmailSetting = null;

                    if (ObjVm.InviteEmailDumpId != "0")
                    {
                        _ObjSurveyInviteEmailSetting = _ObjSurveyService.GetSurveyInviteEmailSetting(ObjVm.InviteEmailDumpId);
                        _ObjSurveyInviteEmailSetting.SurveyEmailSettingId = _Obj.SurveyEmailSettingId;

                    }

                    SurveyRemainderEmailSetting _ObjSurveyRemainderEmailSetting = null;
                    if (ObjVm.RemainderEmailDumpId != "0")
                    {
                        _ObjSurveyRemainderEmailSetting = _ObjSurveyService.GetSurveyRemainderEmailSetting(ObjVm.RemainderEmailDumpId);
                        _ObjSurveyRemainderEmailSetting.SurveyEmailSettingId = _Obj.SurveyEmailSettingId;

                    }

                    SurveySaveContinueEmailSetting _ObjSurveySaveContinueEmailSetting = null;
                    if (ObjVm.ContinueEmailDumpId != "0")
                    {
                        _ObjSurveySaveContinueEmailSetting = _ObjSurveyService.GetSurveySaveContinueEmailSetting(ObjVm.ContinueEmailDumpId);
                        _ObjSurveySaveContinueEmailSetting.SurveyEmailSettingId = _Obj.SurveyEmailSettingId;

                    }

                    _ObjSurveyService.EditSurveyEmailSetting(_Obj, _ObjSurveyInviteEmailSetting, _ObjSurveyRemainderEmailSetting, _ObjSurveySaveContinueEmailSetting);


                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);

                }
            }



            string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
             .SelectMany(E => E.Errors)
             .Select(E => E.ErrorMessage)
             .ToArray();
            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);



        }
        
        public ActionResult LoadSurveyInviteEmail_Load(Int64 SurveyId, string DumpId)
        {

            SurveyEmailSetting _ObjEmailSetting = _ObjSurveyService.GetSurveyEmailSetting((SurveyId));

            if (_ObjEmailSetting != null)
            {
                if (_ObjEmailSetting.SurveyInviteEmailSetting.Count > 0)
                {
                    SurveyEmailSettingsViewModel ObjVm = new SurveyEmailSettingsViewModel
                    {
                        InviteEmailSubject = _ObjEmailSetting.SurveyInviteEmailSetting.FirstOrDefault().Subject,
                        InviteEmailBody = _ObjEmailSetting.SurveyInviteEmailSetting.FirstOrDefault().EmailContent
                    };
                    return PartialView("_SurveyInviteEmailPopup", ObjVm);
                }
            }
            if (DumpId != "0")
            {
                SurveyInviteEmailSetting _ObjInviteEmail = _ObjSurveyService.GetSurveyInviteEmailSetting(DumpId);


                SurveyEmailSettingsViewModel ObjVm = new SurveyEmailSettingsViewModel
                {
                    InviteEmailSubject = _ObjInviteEmail.Subject,
                    InviteEmailBody = _ObjInviteEmail.EmailContent
                };

                return PartialView("_SurveyInviteEmailPopup", ObjVm);
            }
            return PartialView("_SurveyInviteEmailPopup");
        }
        
        public ActionResult LoadSurveyRemainderEmailUrl_load(Int64 SurveyId, string DumpId)
        {
            SurveyEmailSetting _ObjEmailSetting = _ObjSurveyService.GetSurveyEmailSetting((SurveyId));

            if (_ObjEmailSetting != null)
            {

                if (_ObjEmailSetting.SurveyRemainderEmailSetting.Count > 0)
                {
                    SurveyEmailSettingsViewModel ObjVm = new SurveyEmailSettingsViewModel
                    {
                        RemainderEmailSubject = _ObjEmailSetting.SurveyRemainderEmailSetting.FirstOrDefault().Subject,
                        RemainderEmailBody = _ObjEmailSetting.SurveyRemainderEmailSetting.FirstOrDefault().EmailContent
                    };
                    return PartialView("_SurveyAddRemainderEmailPopup", ObjVm);
                }
            }
            if (DumpId != "0")
            {
                SurveyRemainderEmailSetting _ObjRemainderEmail = _ObjSurveyService.GetSurveyRemainderEmailSetting(DumpId);


                SurveyEmailSettingsViewModel ObjVm = new SurveyEmailSettingsViewModel
                {
                    RemainderEmailSubject = _ObjRemainderEmail.Subject,
                    RemainderEmailBody = _ObjRemainderEmail.EmailContent
                };

                return PartialView("_SurveyAddRemainderEmailPopup", ObjVm);
            }
            return PartialView("_SurveyAddRemainderEmailPopup");



            // return PartialView("_SurveyAddRemainderEmailPopup");
        }
        
        public ActionResult LoadSurveySaveContinueEmailUrl_Load(Int64 SurveyId, string DumpId)
        {

            SurveyEmailSetting _ObjEmailSetting = _ObjSurveyService.GetSurveyEmailSetting((SurveyId));

            if (_ObjEmailSetting != null)
            {
                if (_ObjEmailSetting.SurveySaveContinueEmailSetting.Count > 0)
                {
                    SurveyEmailSettingsViewModel ObjVm = new SurveyEmailSettingsViewModel
                    {
                        SaveContinueEmailSubject = _ObjEmailSetting.SurveySaveContinueEmailSetting.FirstOrDefault().Subject,
                        SaveContinueEmailBody = _ObjEmailSetting.SurveySaveContinueEmailSetting.FirstOrDefault().EmailContent
                    };
                    return PartialView("_SurveyAddSaveContinueEmailPopup", ObjVm);
                }
            }
            if (DumpId != "0")
            {
                SurveySaveContinueEmailSetting _ObjRemainderEmail = _ObjSurveyService.GetSurveySaveContinueEmailSetting(DumpId);
                SurveyEmailSettingsViewModel ObjVm = new SurveyEmailSettingsViewModel
                {
                    SaveContinueEmailSubject = _ObjRemainderEmail.Subject,
                    SaveContinueEmailBody = _ObjRemainderEmail.EmailContent
                };

                return PartialView("_SurveyAddSaveContinueEmailPopup", ObjVm);
            }
            return PartialView("_SurveyAddSaveContinueEmailPopup");
            //return PartialView("_SurveyAddSaveContinueEmailPopup");
        }


        [HttpPost]
        //[AllowHtml()]
        public JsonResult SaveInviteEmailSetting(SurveyEmailSettingsViewModel ObjVm)
        {

            SurveyEmailSetting _ObjEmailSetting = _ObjSurveyService.GetSurveyEmailSetting(ObjVm.SurveyId);

            if (_ObjEmailSetting == null)
            {

                if (ObjVm.DumpId == "0")
                {
                    var _container = CBUSA.App_Start.UnityConfig.GetConfiguredContainer();
                    var ObjRandom = _container.Resolve<IRandom>();
                    string DumpId = ObjRandom.StringRandom("Dump");
                    SurveyInviteEmailSetting _ObjSurveyInviteEmailSetting = new SurveyInviteEmailSetting
                    {
                        DumpId = DumpId,
                        EmailContent = ObjVm.InviteEmailBody,
                        Subject = ObjVm.InviteEmailSubject
                    };
                    _ObjSurveyService.SaveSurveyInviteEmailSetting(_ObjSurveyInviteEmailSetting);

                    return Json(new { IsSuccess = true, DumpId = DumpId }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    SurveyInviteEmailSetting _ObjSurveyInviteEmailSetting = _ObjSurveyService.GetSurveyInviteEmailSetting(ObjVm.DumpId);
                    _ObjSurveyInviteEmailSetting.EmailContent = ObjVm.InviteEmailBody;
                    _ObjSurveyInviteEmailSetting.Subject = ObjVm.InviteEmailSubject;
                    _ObjSurveyService.UpdateSurveyInviteEmailSetting(_ObjSurveyInviteEmailSetting);
                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                if (_ObjEmailSetting.SurveyInviteEmailSetting.Count == 0)
                {

                    SurveyInviteEmailSetting _ObjSurveyInviteEmailSetting = new SurveyInviteEmailSetting
                    {
                        SurveyEmailSettingId = _ObjEmailSetting.SurveyEmailSettingId,
                        EmailContent = ObjVm.InviteEmailBody,
                        Subject = ObjVm.InviteEmailSubject
                    };
                    _ObjSurveyService.SaveSurveyInviteEmailSetting(_ObjSurveyInviteEmailSetting);
                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    SurveyInviteEmailSetting _ObjSurveyInviteEmailSetting = _ObjEmailSetting.SurveyInviteEmailSetting.FirstOrDefault();
                    _ObjSurveyInviteEmailSetting.EmailContent = ObjVm.InviteEmailBody;
                    _ObjSurveyInviteEmailSetting.Subject = ObjVm.InviteEmailSubject;
                    _ObjSurveyService.UpdateSurveyInviteEmailSetting(_ObjSurveyInviteEmailSetting);
                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
            }




            //Survey _ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId);
            //List<Int64> SavedMarketList = MarketList.Except(_ObjSurvey.SurveyMarket.Select(x => x.MarketId)).ToList();
            //if (SavedMarketList.Count > 0)
            //{
            //    _ObjSurveyService.SaveSurveyMarket(SavedMarketList, SurveyId);



            //}
            //else
            //{

            //    ModelState.AddModelError("", "These market allready in database..");
            //    string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
            //     .SelectMany(E => E.Errors)
            //     .Select(E => E.ErrorMessage)
            //     .ToArray();
            //    return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);
            //}
        }

        public JsonResult SaveRemainderEmailSetting(SurveyEmailSettingsViewModel ObjVm)
        {

            SurveyEmailSetting _ObjEmailSetting = _ObjSurveyService.GetSurveyEmailSetting((ObjVm.SurveyId));

            if (_ObjEmailSetting == null)
            {
                if (ObjVm.DumpId == "0")
                {
                    var _container = CBUSA.App_Start.UnityConfig.GetConfiguredContainer();
                    var ObjRandom = _container.Resolve<IRandom>();
                    string DumpId = ObjRandom.StringRandom("Dump");
                    SurveyRemainderEmailSetting _ObjSurveyInviteEmailSetting = new SurveyRemainderEmailSetting
                    {
                        DumpId = DumpId,
                        EmailContent = ObjVm.RemainderEmailBody,
                        Subject = ObjVm.RemainderEmailSubject
                    };
                    _ObjSurveyService.SaveSurveyRemainderEmailSetting(_ObjSurveyInviteEmailSetting);

                    return Json(new { IsSuccess = true, DumpId = DumpId }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    SurveyRemainderEmailSetting _ObjSurveyInviteEmailSetting = _ObjSurveyService.GetSurveyRemainderEmailSetting(ObjVm.DumpId);
                    _ObjSurveyInviteEmailSetting.EmailContent = ObjVm.RemainderEmailBody;
                    _ObjSurveyInviteEmailSetting.Subject = ObjVm.RemainderEmailSubject;
                    _ObjSurveyService.UpdateSurveyRemainderEmailSetting(_ObjSurveyInviteEmailSetting);
                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (_ObjEmailSetting.SurveyRemainderEmailSetting.Count == 0)
                {
                    SurveyRemainderEmailSetting _ObjSurveyInviteEmailSetting = new SurveyRemainderEmailSetting
                    {
                        SurveyEmailSettingId = _ObjEmailSetting.SurveyEmailSettingId,
                        EmailContent = ObjVm.RemainderEmailBody,
                        Subject = ObjVm.RemainderEmailSubject
                    };
                    _ObjSurveyService.SaveSurveyRemainderEmailSetting(_ObjSurveyInviteEmailSetting);

                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    SurveyRemainderEmailSetting _ObjSurveyInviteEmailSetting = _ObjEmailSetting.SurveyRemainderEmailSetting.FirstOrDefault();
                    _ObjSurveyInviteEmailSetting.EmailContent = ObjVm.RemainderEmailBody;
                    _ObjSurveyInviteEmailSetting.Subject = ObjVm.RemainderEmailSubject;
                    _ObjSurveyService.UpdateSurveyRemainderEmailSetting(_ObjSurveyInviteEmailSetting);
                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
            }

        }
        public JsonResult SaveContinueEmailSetting(SurveyEmailSettingsViewModel ObjVm)
        {
            SurveyEmailSetting _ObjEmailSetting = _ObjSurveyService.GetSurveyEmailSetting((ObjVm.SurveyId));


            if (_ObjEmailSetting == null)
            {
                if (ObjVm.DumpId == "0")
                {
                    var _container = CBUSA.App_Start.UnityConfig.GetConfiguredContainer();
                    var ObjRandom = _container.Resolve<IRandom>();
                    string DumpId = ObjRandom.StringRandom("Dump");
                    SurveySaveContinueEmailSetting _ObjSurveyInviteEmailSetting = new SurveySaveContinueEmailSetting
                    {
                        DumpId = DumpId,
                        EmailContent = ObjVm.SaveContinueEmailBody,
                        Subject = ObjVm.SaveContinueEmailSubject
                    };
                    _ObjSurveyService.SaveSurveySaveContinueEmailSetting(_ObjSurveyInviteEmailSetting);

                    return Json(new { IsSuccess = true, DumpId = DumpId }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    SurveySaveContinueEmailSetting _ObjSurveyInviteEmailSetting = _ObjSurveyService.GetSurveySaveContinueEmailSetting(ObjVm.DumpId);
                    _ObjSurveyInviteEmailSetting.EmailContent = ObjVm.SaveContinueEmailBody;
                    _ObjSurveyInviteEmailSetting.Subject = ObjVm.SaveContinueEmailSubject;
                    _ObjSurveyService.UpdateSurveySaveContinueEmailSetting(_ObjSurveyInviteEmailSetting);
                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (_ObjEmailSetting.SurveySaveContinueEmailSetting.Count == 0)
                {
                    SurveySaveContinueEmailSetting _ObjSurveyInviteEmailSetting = new SurveySaveContinueEmailSetting
                    {
                        SurveyEmailSettingId = _ObjEmailSetting.SurveyEmailSettingId,
                        EmailContent = ObjVm.SaveContinueEmailBody,
                        Subject = ObjVm.SaveContinueEmailSubject
                    };
                    _ObjSurveyService.SaveSurveySaveContinueEmailSetting(_ObjSurveyInviteEmailSetting);

                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    SurveySaveContinueEmailSetting _ObjSurveyInviteEmailSetting = _ObjEmailSetting.SurveySaveContinueEmailSetting.FirstOrDefault();
                    _ObjSurveyInviteEmailSetting.EmailContent = ObjVm.SaveContinueEmailBody;
                    _ObjSurveyInviteEmailSetting.Subject = ObjVm.SaveContinueEmailSubject;
                    _ObjSurveyService.UpdateSurveySaveContinueEmailSetting(_ObjSurveyInviteEmailSetting);
                    return Json(new { IsSuccess = true, DumpId = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        #endregion

        //Started Manage Survey created by prasenjit  
        public ActionResult ManageSurvey()
        {
            return View();
        }
        public ActionResult ArchivedSurvey()
        {
            return View();
        }
        public ActionResult ArchivedSurveyList([DataSourceRequest] DataSourceRequest request, string Type, string Flag, int? PageValue)
        {

            int RowNo = 2;
            int SurveyCounter = 1;

            if (Flag == null && Type == null)
            {
                var SurveyList = _ObjSurveyService.GetSurveyAllArchive().
                        Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
                            ResponseCom = responsecomplete(x.SurveyId),
                            ResponseInCom = responseincomplete(x.SurveyId),
                            ResponsePend = responsepend(x.SurveyId.ToString()),
                            SurveyName = x.SurveyName,
                            // PublishDate= FromDate(x.StartDate.ToString()),
                            LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                            // PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                            PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                            ContractName = x.Contract.ContractName,
                            IsPublished = x.IsPublished,
                            ContractId = x.ContractId,
                            SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                            //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                        });
                
                var newList = SurveyList.ToList()
                    .Select(x => new SurveyViewModel
                    {
                        SurveyId = x.SurveyId,
                        ResponseCom = x.ResponseCom,
                        ResponseInCom = x.ResponseInCom,
                        ResponsePend = x.ResponsePend,
                        SurveyName = x.SurveyName,
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
            else
            {
                if (Flag == "")
                {

                    if (Type == "asccon")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderBy(x => x.Contract.ContractName).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newList = SurveyList.ToList()
                            .Select(x => new SurveyViewModel
                            {
                                SurveyId = x.SurveyId,
                                ResponseCom = x.ResponseCom,
                                ResponseInCom = x.ResponseInCom,
                                ResponsePend = x.ResponsePend,
                                SurveyName = x.SurveyName,
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

                    else if (Type == "desccon")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderByDescending(x => x.Contract.ContractName).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           SurveyName = x.SurveyName,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newList = SurveyList.ToList()
                           .Select(x => new SurveyViewModel
                           {
                               SurveyId = x.SurveyId,
                               ResponseCom = x.ResponseCom,
                               ResponseInCom = x.ResponseInCom,
                               ResponsePend = x.ResponsePend,
                               SurveyName = x.SurveyName,
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

                    else if (Type == "ascncp")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderBy(x => x.SurveyName).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""

                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newList = SurveyList.ToList()
                          .Select(x => new SurveyViewModel
                          {
                              SurveyId = x.SurveyId,
                              ResponseCom = x.ResponseCom,
                              ResponseInCom = x.ResponseInCom,
                              ResponsePend = x.ResponsePend,
                              SurveyName = x.SurveyName,
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
                    else if (Type == "descncp")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderByDescending(x => x.SurveyName).
                      Select(x => new SurveyViewModel
                      {
                          SurveyId = x.SurveyId,
                          ResponseCom = responsecomplete(x.SurveyId),
                          ResponseInCom = responseincomplete(x.SurveyId),
                          ResponsePend = responsepend(x.SurveyId.ToString()),
                          SurveyName = x.SurveyName,
                          // PublishDate= FromDate(x.StartDate.ToString()),
                          LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                          PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                          ContractName = x.Contract.ContractName,
                          IsPublished = x.IsPublished,
                          ContractId = x.ContractId,
                          SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                          //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                      });

                        var newList = SurveyList.ToList()
                          .Select(x => new SurveyViewModel
                          {
                              SurveyId = x.SurveyId,
                              ResponseCom = x.ResponseCom,
                              ResponseInCom = x.ResponseInCom,
                              ResponsePend = x.ResponsePend,
                              SurveyName = x.SurveyName,
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
                    else if (Type == "ascdue")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderBy(x => x.EndDate).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""

                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newList = SurveyList.ToList()
                          .Select(x => new SurveyViewModel
                          {
                              SurveyId = x.SurveyId,
                              ResponseCom = x.ResponseCom,
                              ResponseInCom = x.ResponseInCom,
                              ResponsePend = x.ResponsePend,
                              SurveyName = x.SurveyName,
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
                    else if (Type == "descdue")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderByDescending(x => x.EndDate).
                      Select(x => new SurveyViewModel
                      {
                          SurveyId = x.SurveyId,
                          ResponseCom = responsecomplete(x.SurveyId),
                          ResponseInCom = responseincomplete(x.SurveyId),
                          ResponsePend = responsepend(x.SurveyId.ToString()),
                          SurveyName = x.SurveyName,
                          // PublishDate= FromDate(x.StartDate.ToString()),
                          LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                          PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                          ContractName = x.Contract.ContractName,
                          IsPublished = x.IsPublished,
                          ContractId = x.ContractId,
                          SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                          //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                      });

                        var newList = SurveyList.ToList()
                          .Select(x => new SurveyViewModel
                          {
                              SurveyId = x.SurveyId,
                              ResponseCom = x.ResponseCom,
                              ResponseInCom = x.ResponseInCom,
                              ResponsePend = x.ResponsePend,
                              SurveyName = x.SurveyName,
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
                    else if (Type == "archser")
                    {
                        DateTime archdate = DateTime.Now.AddDays(-30);
                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderBy(x => x.EndDate).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""

                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newList = SurveyList.ToList()
                         .Select(x => new SurveyViewModel
                         {
                             SurveyId = x.SurveyId,
                             ResponseCom = x.ResponseCom,
                             ResponseInCom = x.ResponseInCom,
                             ResponsePend = x.ResponsePend,
                             SurveyName = x.SurveyName,
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

                    else if (Type == null)
                    {

                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderByDescending(x => x.SurveyId).
                        Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
                            ResponseCom = responsecomplete(x.SurveyId),
                            ResponseInCom = responseincomplete(x.SurveyId),
                            ResponsePend = responsepend(x.SurveyId.ToString()),
                            SurveyName = x.SurveyName,
                            // PublishDate= FromDate(x.StartDate.ToString()),
                            LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                            PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                            ContractName = x.Contract.ContractName,
                            IsPublished = x.IsPublished,
                            ContractId = x.ContractId,
                            SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                            //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                        });

                        var newList = SurveyList.ToList()
                         .Select(x => new SurveyViewModel
                         {
                             SurveyId = x.SurveyId,
                             ResponseCom = x.ResponseCom,
                             ResponseInCom = x.ResponseInCom,
                             ResponsePend = x.ResponsePend,
                             SurveyName = x.SurveyName,
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
                }
                else
                {
                    if (Flag == "0")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllArchive().OrderByDescending(x => x.SurveyId).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
                            ResponseCom = x.ResponseCom,
                            ResponseInCom = x.ResponseInCom,
                            ResponsePend = x.ResponsePend,
                            SurveyName = x.SurveyName,
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
                    else
                    {
                        var SurveyList = _ObjSurveyService.FindContractSurveysAllArchive(Convert.ToInt32(Flag)).OrderByDescending(x => x.SurveyId).
                      Select(x => new SurveyViewModel
                      {
                          SurveyId = x.SurveyId,
                          ResponseCom = responsecomplete(x.SurveyId),
                          ResponseInCom = responseincomplete(x.SurveyId),
                          ResponsePend = responsepend(x.SurveyId.ToString()),
                          SurveyName = x.SurveyName,
                          // PublishDate= FromDate(x.StartDate.ToString()),
                          LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                          PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                          ContractName = x.Contract.ContractName,
                          IsPublished = x.IsPublished,
                          ContractId = x.ContractId,
                          SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                          //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                      });

                        var newList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
                            ResponseCom = x.ResponseCom,
                            ResponseInCom = x.ResponseInCom,
                            ResponsePend = x.ResponsePend,
                            SurveyName = x.SurveyName,
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


                }

            }

            var value = "";
            return Json(value.ToDataSourceResult(request));
        }

        public ActionResult SurveyList([DataSourceRequest] DataSourceRequest request, string Type, string Flag, int? PageValue)
        {

            int RowNo = 2;
            int SurveyCounter = 1;
            if (Flag == null && Type == null)
            {
                var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderByDescending(x => x.SurveyId).
                        Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
                            // ResponseCom = response(x.IsPublished, x.SurveyId),
                            ResponseCom = responsecomplete(x.SurveyId),
                            ResponseInCom = responseincomplete(x.SurveyId),
                            ResponsePend = responsepend(x.SurveyId.ToString()),
                            SurveyName = x.SurveyName,
                            //PublishDate= FromDate(x.StartDate.ToString()),
                            Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                            LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                            //PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                            PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                            ContractName = x.Contract.ContractName,
                            IsPublished = x.IsPublished,
                            ContractId = x.ContractId,
                            SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                            //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                        });

                var newSurveyList = SurveyList.ToList()
                    .Select(x => new SurveyViewModel
                    {
                        SurveyId = x.SurveyId,
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

                return Json(newSurveyList.ToDataSourceResult(request));
            }
            else
            {
                if (Flag == "")
                {
                    if (Type == "asccon")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderBy(x => x.Contract.ContractName).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           //PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));

                    }

                    else if (Type == "desccon")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderByDescending(x => x.Contract.ContractName).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           SurveyName = x.SurveyName,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           // PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }

                    else if (Type == "ascncp")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderBy(x => x.SurveyName).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           // PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""

                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }
                    else if (Type == "descncp")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderByDescending(x => x.SurveyName).
                          Select(x => new SurveyViewModel
                          {
                              SurveyId = x.SurveyId,
                              ResponseCom = responsecomplete(x.SurveyId),
                              ResponseInCom = responseincomplete(x.SurveyId),
                              ResponsePend = responsepend(x.SurveyId.ToString()),
                              SurveyName = x.SurveyName,
                              // PublishDate= FromDate(x.StartDate.ToString()),
                              Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                              LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                              //  PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                              PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                              ContractName = x.Contract.ContractName,
                              IsPublished = x.IsPublished,
                              ContractId = x.ContractId,
                              SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                              //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                          });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }
                    else if (Type == "ascdue")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderBy(x => x.EndDate).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           // PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""

                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }
                    else if (Type == "descdue")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderByDescending(x => x.EndDate).
                          Select(x => new SurveyViewModel
                          {
                              SurveyId = x.SurveyId,
                              ResponseCom = responsecomplete(x.SurveyId),
                              ResponseInCom = responseincomplete(x.SurveyId),
                              ResponsePend = responsepend(x.SurveyId.ToString()),
                              SurveyName = x.SurveyName,
                              // PublishDate= FromDate(x.StartDate.ToString()),
                              Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                              LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                              //PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                              PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                              ContractName = x.Contract.ContractName,
                              IsPublished = x.IsPublished,
                              ContractId = x.ContractId,
                              SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                              //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                          });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }
                    else if (Type == "archser")
                    {
                        DateTime archdate = DateTime.Now.AddDays(-30);
                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderBy(x => x.EndDate).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           //PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""

                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }

                    else if (Type == null)
                    {

                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderByDescending(x => x.SurveyId).
                        Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
                            ResponseCom = responsecomplete(x.SurveyId),
                            ResponseInCom = responseincomplete(x.SurveyId),
                            ResponsePend = responsepend(x.SurveyId.ToString()),
                            SurveyName = x.SurveyName,
                            // PublishDate= FromDate(x.StartDate.ToString()),
                            Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                            LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                            // PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                            PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                            ContractName = x.Contract.ContractName,
                            IsPublished = x.IsPublished,
                            ContractId = x.ContractId,
                            SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                            //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                        });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }
                }
                else
                {
                    if (Flag == "0")
                    {
                        var SurveyList = _ObjSurveyService.GetSurveyAllActiveAndClose().OrderByDescending(x => x.SurveyId).
                       Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
                           ResponseCom = responsecomplete(x.SurveyId),
                           ResponseInCom = responseincomplete(x.SurveyId),
                           ResponsePend = responsepend(x.SurveyId.ToString()),
                           SurveyName = x.SurveyName,
                           // PublishDate= FromDate(x.StartDate.ToString()),
                           Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                           LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                           //PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                           PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                           ContractName = x.Contract.ContractName,
                           IsPublished = x.IsPublished,
                           ContractId = x.ContractId,
                           SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                           //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                       });

                        var newSurveyList = SurveyList.ToList()
                        .Select(x => new SurveyViewModel
                        {
                            SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }
                    else
                    {
                        var SurveyList = _ObjSurveyService.FindContractSurveysAll(Convert.ToInt32(Flag)).OrderByDescending(x => x.SurveyId).
                      Select(x => new SurveyViewModel
                      {
                          SurveyId = x.SurveyId,
                          ResponseCom = responsecomplete(x.SurveyId),
                          ResponseInCom = responseincomplete(x.SurveyId),
                          ResponsePend = responsepend(x.SurveyId.ToString()),
                          SurveyName = x.SurveyName,
                          // PublishDate= FromDate(x.StartDate.ToString()),
                          Archive = x.EndDate != null ? Archive(x.EndDate.ToString(), x.IsPublished) : "",
                          LastDate = x.EndDate != null ? FromDate(x.EndDate.ToString()) : "",
                          // PublishDate = x.EndDate != null && x.Publishdate != null ? Publish(x.EndDate.ToString(), x.Publishdate.ToString(), x.IsPublished) : "",
                          PublishDate = x.ModifiedOn != null ? Publish(x.ModifiedOn.ToString(), x.Publishdate.ToString(), x.IsPublished, x.RowStatusId) : "",
                          ContractName = x.Contract.ContractName,
                          IsPublished = x.IsPublished,
                          ContractId = x.ContractId,
                          SurveyStatus = x.EndDate != null ? Status(x.EndDate.ToString()) : ""


                          //ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                      });

                        var newSurveyList = SurveyList.ToList()
                       .Select(x => new SurveyViewModel
                       {
                           SurveyId = x.SurveyId,
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

                        return Json(newSurveyList.ToDataSourceResult(request));
                    }

                }

            }

            var value = "";
            return Json(value.ToDataSourceResult(request));
        }
        public JsonResult GetContracts()
        {

            Int64 conid = 0;
            var ContractList = _ObjContractService.GetContract().OrderBy(x => x.ContractName)
                .Select(x => new { ContractId = x.ContractId, ContractName = x.ContractName }).ToList();

            ContractList.Insert(0, new { ContractId = conid, ContractName = "Select All" });



            return Json(ContractList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetActiveContracts()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            Int64 conid = 0;
            var ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContractName)
                .Select(x => new { ContractId = x.Contract.ContractId, ContractName = x.Contract.ContractName }).ToList();

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
        //public string response(bool data, Int64 SurveyId)
        //{

        //    string flag = "";
        //    if (data == true)
        //    {
        //        int Complete = _ObjSurveyBuilderService.FindCompleteBuilderSurvey(SurveyId).Count();
        //        int InComplete = _ObjSurveyBuilderService.FindInCompleteBuilderSurvey(SurveyId).Count();
        //        flag = Complete + "|" + InComplete;
        //    }
        //    else
        //    {
        //        flag = "Not published";
        //    }

        //    return flag;
        //}
        public string responsecomplete(Int64 SurveyId)
        {
            //string SurveyList = _ObjSurveyService.GetSurveyBuilder(Convert.ToInt32(data)).Where(x => x.IsSurveyCompleted == true).Count().ToString();
            string flag = "";

            int Complete = _ObjSurveyBuilderService.FindCompleteBuilderSurvey(SurveyId).Count();
            int InComplete = _ObjSurveyBuilderService.FindInCompleteBuilderSurvey(SurveyId).Count();
            flag = Complete.ToString();


            return flag;
        }
        public string responseincomplete(Int64 SurveyId)
        {
            //string SurveyList = _ObjSurveyService.GetSurveyBuilder(Convert.ToInt32(data)).Where(x => x.IsSurveyCompleted == true).Count().ToString();
            string flag = "";

            int Complete = _ObjSurveyBuilderService.FindCompleteBuilderSurvey(SurveyId).Count();
            int InComplete = _ObjSurveyBuilderService.FindInCompleteBuilderSurvey(SurveyId).Count();
            flag = InComplete.ToString();


            return flag;
        }
        public string responsepend(string data)
        {
            string SurveyList = _ObjSurveyService.GetSurveyBuilder(Convert.ToInt32(data)).Where(x => x.IsSurveyCompleted == false).Count().ToString();


            return SurveyList;
        }
        public string Status(string date)
        {
            string dat = "";

            if (date != null)
            {
                DateTime enddate = Convert.ToDateTime(date);
                DateTime CurrentDate = DateTime.Now;

                if (enddate > CurrentDate)
                {
                    dat = "Live";
                }
                else if (CurrentDate < enddate && CurrentDate < enddate.AddDays(30))    //Survey has ended but less than 30 days ago
                {
                    dat = "Closed";
                }
                else
                {
                    dat = "Archived";
                }
            }
            return dat;
        }
        public string Archive(string date, bool pub)
        {
            DateTime enddate = Convert.ToDateTime(date);
            DateTime CurrentDate = DateTime.Now;

            string dat = "";
            if (date != null)
            {
                if (CurrentDate >= enddate.AddDays(30) && pub == true)   //End date was more than 30 days ago
                {
                    dat = "y";
                }
                else if (enddate <= CurrentDate || pub == false)
                {
                    dat = "n";
                }

            }
            return dat;

        }
        public string Publish(string modifieddate, string publishedate, bool pub, int RowStatusid)
        {
            DateTime modify = Convert.ToDateTime(modifieddate);

            string dat = "";
            if (RowStatusid == 2)
            {
                dat = "Closed on:" + modify.ToString("MM/dd/yy");
            }
            else if (pub == true && publishedate != "")
            {
                DateTime publish = Convert.ToDateTime(publishedate);
                dat = "Published on:" + publish.ToString("MM/dd/yy");
            }
            else
            {
                dat = "Last edited:" + modify.ToString("MM/dd/yy");
            }


            return dat;
        }

        //end manage survey 

        #region Preview survey question
        public ActionResult PreviewQuestion(Int64? SurveyId)
        {
            if (SurveyId.HasValue)
            {
                bool[] SurveyStatus = _ObjSurveyService.IsNcpSurvey(SurveyId.GetValueOrDefault());
                if (SurveyStatus[0])
                {
                    if (SurveyStatus[1])
                    {
                        ViewBag.IsNcpSurvey = true;
                    }
                    else
                    {
                        ViewBag.IsNcpSurvey = false;
                    }
                    ViewBag.SurveyId = SurveyId.GetValueOrDefault();
                    bool IsSurveyPublish = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault()).IsPublished;
                    ViewBag.IsSurveyPublish = IsSurveyPublish;
                    return View();

                }
            }
            return RedirectToAction("SurveyDetails", "Survey", new { Area = "Admin" });
        }

        public ActionResult PreviewQuestionList_Read([DataSourceRequest] DataSourceRequest request, Int64 SurveyId)
        {
            Survey _ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId);
            //  bool IsSurveyPublish = _ObjSurvey.IsPublished;
            var SurveyQuestionList = _ObjSurvey.Question.Where(y => y.RowStatusId == (Int32)RowActiveStatus.Active).OrderBy(x => x.SurveyOrder)
                .Select(x => new SurveyQuestionViewModel
                {
                    QuestionId = x.QuestionId,
                    Question = x.QuestionValue,
                    QuestionType = x.QuestionType.QuestionTypeId != 1 ? x.QuestionType.TypeName : x.QuestionType.TypeName + " - " + x.QuestionTextBoxSetting.FirstOrDefault().TextBoxType.TextBoxTypeName,
                    IsSurveyPublished = _ObjSurvey.IsPublished,
                    SurveyOrder = x.SurveyOrder,
                    OrderUp = !(x.SurveyOrder == 1),
                    OrderDown = !(x.SurveyOrder == _ObjSurvey.Question.Max(y => y.SurveyOrder))

                });

            return Json(SurveyQuestionList.ToDataSourceResult(request));
        }

        string QuestionType(Int64 TypeId, Int64 TexboxTypeId)
        {
            string QuestionType = string.Empty;

            switch (TypeId)
            {
                case 1:

                    switch (TexboxTypeId)
                    {
                        case 1:
                            QuestionType = "D";
                            break;
                        case 2:
                            QuestionType = "D";
                            break;
                        case 3:
                            QuestionType = "D";
                            break;
                        case 4:
                            QuestionType = "D";
                            break;
                    }
                    break;
                case 2:
                    QuestionType = "Drop list";
                    break;
                case 3:
                    QuestionType = "2 D Matrix";
                    break;

            }
            return QuestionType;
        }

        [HttpPost]
        public JsonResult DeleteSurveQuestion(Int64 SurveyId, Int64 QuestionId)
        {

            var ObjQuestion = _ObjQuestionService.GetQuestion(QuestionId, SurveyId);
            if (ObjQuestion != null)
            {
                ObjQuestion.RowStatusId = (int)RowActiveStatus.Deleted;
                ObjQuestion.ModifiedOn = DateTime.Now;
                ObjQuestion.ModifiedBy = 1;

                _ObjQuestionService.DeleteQuestion(ObjQuestion);
                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }


            ModelState.AddModelError("", "Please try one more time");

            string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                   .SelectMany(E => E.Errors)
                   .Select(E => E.ErrorMessage)
                   .ToArray();

            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);



        }

        [HttpPost]
        public JsonResult UpdateQuestionText(SurveyQuestionViewModel ObjSurveyQuestion)
        {
            var ObjQuestion = _ObjQuestionService.GetQuestion(ObjSurveyQuestion.QuestionId, ObjSurveyQuestion.SurveyId);
            if (ObjQuestion != null)
            {
                if (ObjSurveyQuestion.Question.Length > 0)
                {
                    ObjQuestion.QuestionValue = ObjSurveyQuestion.Question;
                    ObjQuestion.ModifiedOn = DateTime.Now;
                    ObjQuestion.ModifiedBy = 1;
                    _ObjQuestionService.EditQuestion(ObjQuestion);
                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }
            }

            ModelState.AddModelError("", "Please try one more time");

            string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                   .SelectMany(E => E.Errors)
                   .Select(E => E.ErrorMessage)
                   .ToArray();

            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult OrderingUpQuestion(SurveyQuestionViewModel ObjSurveyQuestion)
        {

            _ObjQuestionService.OrderingUpQuestion(ObjSurveyQuestion.SurveyId, ObjSurveyQuestion.QuestionId);
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult OrderingDownQuestion(SurveyQuestionViewModel ObjSurveyQuestion)
        {

            _ObjQuestionService.OrderingDownQuestion(ObjSurveyQuestion.SurveyId, ObjSurveyQuestion.QuestionId);
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult PreviewSurvey(Int64 id)
        {

            Survey ObjSurvey = _ObjSurveyService.GetSurvey(id);

            PreviewQuestionViewModel ObjVm = new PreviewQuestionViewModel { SurveyName = ObjSurvey.SurveyName, ObjQuestion = ObjSurvey.Question.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).OrderBy(x => x.SurveyOrder).ToList() };

            return View(ObjVm);
        }

        #endregion


        #region Publish Survey

        public ActionResult PublishSurvey(Int64? SurveyId)
        {
            if (SurveyId.HasValue)
            {
                if (_ObjSurveyService.IsSurveyExist(SurveyId.GetValueOrDefault()))
                {

                    var Survey = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault());

                    if (Survey.IsNcpSurvey)
                    {
                        ViewBag.IsNcpSurvey = true;
                    }
                    else
                    {
                        ViewBag.IsNcpSurvey = false;
                    }

                    bool IsSurveyPublish = Survey.IsPublished;
                    ViewBag.IsSurveyPublish = IsSurveyPublish;

                    ViewBag.SurveyId = SurveyId;
                    return View();
                }
            }
            return RedirectToAction("SurveyDetails", "Survey", new { Area = "Admin" });
        }

        //started mail sending during survey publishing by prasenjit

        public JsonResult SendSurveyToBuilders(Int64 SurveyId)
        {
            Survey ObjSurvey = _ObjSurveyService.GetSurvey(SurveyId);
            if (ObjSurvey != null)
            {
                bool Flag = false;
                List<Builder> Builders = null;
                List<Builder> BuildersTemp = null;

                if (ObjSurvey.SurveyEmailSetting.FirstOrDefault() == null)
                {
                    ModelState.AddModelError("", "Please complete email setting before publish");
                }

                if (ObjSurvey.SurveyEmailSetting.FirstOrDefault() != null)
                {
                    if (ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyInviteEmailSetting.FirstOrDefault() == null)
                    {
                        ModelState.AddModelError("", "Please complete invite email setting before publish");
                    }
                    else
                    {
                        if (ObjSurvey.SurveyEmailSetting.FirstOrDefault().SurveyInviteEmailSetting.FirstOrDefault().EmailContent == "")
                        {
                            ModelState.AddModelError("", "Please complete invite email setting before publish");
                        }
                    }
                }
                if (ObjSurvey.IsNcpSurvey)
                {
                    if (ObjSurvey.Quater == null || ObjSurvey.Year == null)
                    {
                        ModelState.AddModelError("", "Please complete quarter and year before publish");
                    }
                }

                if (!ModelState.IsValid)
                {

                    string[] ModelErrorQuaterYear = ModelState.Values.Where(E => E.Errors.Count > 0)
                           .SelectMany(E => E.Errors)
                           .Select(E => E.ErrorMessage)
                           .ToArray();

                    return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorQuaterYear) }, JsonRequestBehavior.AllowGet);
                }
                if (ObjSurvey.IsNcpSurvey)
                {
                    BuildersTemp = _ObjContractBuilderService.GetBuilderofContract(ObjSurvey.ContractId).Select(x => x.Builder).ToList();
                }
                else
                {
                    List<Market> MarketList = ObjSurvey.SurveyMarket.Select(x => x.Market).ToList();
                    BuildersTemp = GetBuilderId(MarketList);

                }

                List<Int64> AllreadyGetInviteEmailBuilderList = _ObjSurveyService.BuilderAllreadyGetInvitationForSurvey(SurveyId, ObjSurvey.IsEnrolment).Select(x => x.BuilderId).ToList(); // send enrollment as parameter - when enrollment we have to consider ContractBuilder

                Builders = BuildersTemp.Where(x => !AllreadyGetInviteEmailBuilderList.Contains(x.BuilderId)).ToList();


                if (Builders.Count != 0)
                {
                    string GroupId = Guid.NewGuid().ToString() + Convert.ToString(SurveyId);
                    Flag = false;
                    _ObjBuilderEmailSent.Flag(Flag);
                    foreach (var item in Builders)
                    {


                        string Email = item.Email;
                        Int64 BuilderId = item.BuilderId;
                        //Random rnd = new Random();
                        //StringBuilder builder = new StringBuilder();
                        //char ch;
                        //for (int i = 0; i < 20; i++)
                        //{
                        //    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rnd.NextDouble() + 65)));
                        //    builder.Append(ch);
                        //}

                        //string GroupId = builder.ToString();
                        //GroupId = GroupId + SurveyId;
                        //  Flag = true;
                        //   _ObjBuilderEmailSent.Flag(Flag);
                        try
                        {
                            Sendemail(Email, SurveyId, BuilderId, GroupId, ObjSurvey.SurveyEmailSetting.FirstOrDefault().SenderEmail, ObjSurvey.IsNcpSurvey);
                        }
                        catch
                        {
                        }

                        //if (res == "fromemail")
                        //{
                        //    ModelState.AddModelError("", "Please complete email setting before publish");

                        //    string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                        //           .SelectMany(E => E.Errors)
                        //           .Select(E => E.ErrorMessage)
                        //           .ToArray();

                        //    return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);
                        //}
                        //if (res == "invite")
                        //{
                        //    ModelState.AddModelError("", "Please complete invite email setting before publish");

                        //    string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                        //           .SelectMany(E => E.Errors)
                        //           .Select(E => E.ErrorMessage)
                        //           .ToArray();

                        //    return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);
                        //}

                    }



                    ObjSurvey.SurveyId = SurveyId;
                    ObjSurvey.IsPublished = true;
                    ObjSurvey.StartDate = DateTime.Now;
                    //if (ObjSurvey.StartDate == null)
                    //{
                    //    ObjSurvey.StartDate = DateTime.Now;
                    //}
                    ObjSurvey.ModifiedOn = DateTime.Now;
                    ObjSurvey.Publishdate = DateTime.Now;
                    Flag = true;
                    _ObjBuilderEmailSent.Flag(Flag);

                    _ObjSurveyService.UpdateSurvey(ObjSurvey);

                    // string Link = "Survey/ManageSurvey";
                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    if (BuildersTemp.Count == 0)
                    {
                        if (ObjSurvey.IsNcpSurvey)
                        {
                            ModelState.AddModelError("", "Currently there is no builders to send survey");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please configure builders");
                        }

                        string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                                   .SelectMany(E => E.Errors)
                                   .Select(E => E.ErrorMessage)
                                   .ToArray();

                        return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);


                    }
                    else
                    {
                        if (ObjSurvey.IsNcpSurvey)
                        {
                            ModelState.AddModelError("", "Please add new builder to send survey");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please add new builder to send survey");
                        }

                        string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
                                   .SelectMany(E => E.Errors)
                                   .Select(E => E.ErrorMessage)
                                   .ToArray();

                        return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorChild) }, JsonRequestBehavior.AllowGet);


                    }



                }

            }
            else
            {
                //  string Link = "Survey/ManageSurvey";
                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);

            }
            //return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public List<Builder> GetBuilderId(List<Market> MarketList)
        {
            int Count = 0;
            List<Builder> Builders = new List<Builder>();
            foreach (var Item in MarketList)
            {
                Builders.InsertRange(Count, Item.Bulder.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).ToList());
                Count = Item.Bulder.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Count();
            }
            return Builders;
        }

        public bool Sendemail(string Email, Int64 SurveyId, long BuilderId, string GroupId, string SenderEmail, bool IsNcp) 
        {

            SendEmail obj = new SendEmail();
            IEmailSendApi _ObjEmail = new EmailSendApi();
            Int64 EmailSetingId = _ObjSurveyService.GetSurveyEmailSetting(SurveyId).SurveyEmailSettingId;

            SurveyInviteEmailSetting ObjInviteEmailSetting = _ObjSurveyService.GetSurveyInviteEmailSettingBySurveySetting(EmailSetingId);
            string Body = ObjInviteEmailSetting.EmailContent;
            string Link = string.Empty;

            /*close by rabi on jan 03 2017

            if (!IsNcp)
            {
                // string TakeSurveyUrl = Url.Action("TakeSurvey", "Survey", new { Area = "AttendSurvey", SurveyId = SurveyId, BuilderId = BuilderId, UserId = "####UserId####" });
                string TakeSurveyUrl = Url.Action("TakeSurvey", "Survey", new { Area = "AttendSurvey", SurveyId = SurveyId, BuilderId = "####UserId####"});
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                Link = baseUrl + TakeSurveyUrl;
            }
            else
            {
                var bits = BitConverter.GetBytes(BuilderId);
                var EncryptedBuilderId = Convert.ToBase64String(bits);
                //  String EncryptedBuilderId= Convert.ToBase64String()
                string SubmitNcpreportUrl = Url.Action("Login", "Account", new { Area = "CbusaBuilder", UserId = "####UserId####", Flag = "SubmitReport" });
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                Link = baseUrl + SubmitNcpreportUrl;
            }


            Body = Body.Replace("####Link####", Link);  */

            string Subject = ObjInviteEmailSetting.Subject;
            // bool Status = _ObjEmail.Send(Subject, Body, Email, emailfrom);

            //Get BuilderUser to send survey email

            List<BuilderUser> ListBuilderUser = _ObjBuilderService.GetBuliderAllUser(BuilderId).ToList();
                        
            // string[] UserEmailList = ListBuilderUser.Select(x => x.Email).ToArray();
            ///
            bool Status = false;
            // bool Status = _ObjEmail.Send(Subject, Body, Email, SenderEmail);
            if (ListBuilderUser.Count > 0)
            {
                foreach (var ObjBuilderUser in ListBuilderUser)
                {

                    if (!IsNcp)
                    {

                        string TakeSurveyUrl = Url.Action("TakeSurvey", "Survey", new { Area = "AttendSurvey", SurveyId = SurveyId, BuilderId = BuilderId });
                        string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                        Link = baseUrl + TakeSurveyUrl;
                        Body = Body.Replace("####Link####", Link);
                        Status = _ObjEmail.Send(Subject, Body, ObjBuilderUser.Email, SenderEmail);
                    }
                    else
                    {
                        //********** CODE COMMENTED OUT TO STOP SENDING OUT NCP INVITATION EMAILS FROM SITE **********
                        //********** By Apala on 29.12.2017 for VSO#
                        //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ObjBuilderUser.BuilderUserId.ToString());
                        //string EncryptedBuilderId = System.Convert.ToBase64String(plainTextBytes);

                        //string SubmitNcpreportUrl = Url.Action("Login", "Account", new { Area = "CbusaBuilder", UserId = EncryptedBuilderId, Flag = "SubmitReport" });
                        //string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                        //Link = baseUrl + SubmitNcpreportUrl;

                        //Body = Body.Replace("####Link####", Link);

                        //Status = _ObjEmail.Send(Subject, Body, ObjBuilderUser.Email, SenderEmail);
                        //*****************************************************************************************
                    }


                    BuilderUserSurveyEmailSent objSent = new BuilderUserSurveyEmailSent();
                    objSent.SurveyId = SurveyId;
                    objSent.BuilderId = Convert.ToInt32(BuilderId);
                    objSent.SendDate = DateTime.Now;
                    objSent.SendBy = 1;
                    objSent.GroupId = GroupId;
                    objSent.IsMailSent = Status;
                    objSent.BuilderUserId = ObjBuilderUser.BuilderUserId;
                    _ObjBuilderEmailSent.SaveSurveyEmailBuilderUser(objSent);
                }
            }

            BuilderSurveyEmailSent objEmailSent = new BuilderSurveyEmailSent();
            objEmailSent.SurveyId = SurveyId;
            objEmailSent.BuilderId = Convert.ToInt32(BuilderId);
            objEmailSent.SendDate = DateTime.Now;
            objEmailSent.SendBy = 1;
            objEmailSent.GroupId = GroupId;
            objEmailSent.IsMailSent = false;

            if (Status == true)
            {
                objEmailSent.IsMailSent = true;
                //  objEmailSent.BuilderId = BuilderId;
                // _ObjBuilderEmailSent.UpdateSurveyEmailBuilder(objEmailSent);
            }
            _ObjBuilderEmailSent.SaveSurveyEmailBuilder(objEmailSent);

            return Status;
        }


        public ActionResult ArchievedStatus(Int64 surveyid)
        {

            Survey ObjSurvey = _ObjSurveyService.GetSurvey(surveyid);
            ObjSurvey.SurveyId = surveyid;
            ObjSurvey.ModifiedBy = 1;
            ObjSurvey.ModifiedOn = DateTime.Now;
            ObjSurvey.ArchivedDate = DateTime.Now;
            ObjSurvey.RowStatusId = (int)RowActiveStatus.Archived;

            _ObjSurveyService.UpdateSurvey(ObjSurvey);
            // RowActiveStatus objActive = new RowActiveStatus();


            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ImagaeBrowser

        public string ContentPath
        {
            get
            {
                return contentInitializer.CreateUserFolder(Server);
            }
        }

        private string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        private string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        protected virtual bool CanAccess(string path)
        {
            return path.StartsWith(ToAbsolute(ContentPath), StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return ToAbsolute(ContentPath);
            }

            return CombinePaths(ToAbsolute(ContentPath), path);
        }
        public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }
        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');

            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }
        public virtual bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create(string path, Models.FileBrowserEntry entry)
        {
            path = NormalizePath(path);
            var name = entry.Name;

            if (!string.IsNullOrEmpty(name) && AuthorizeCreateDirectory(path, name))
            {
                var physicalPath = Path.Combine(Server.MapPath(path), name);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                return Json(new
                {
                    name = entry.Name,
                    type = "d",
                    size = entry.Size
                });
            }

            throw new HttpException(403, "Forbidden");
        }
        public JsonResult Read(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeRead(path))
            {
                try
                {
                    directoryBrowser.Server = Server;

                    var result = directoryBrowser
                        .GetContent(path, DefaultFilter)
                        .Select(f => new
                        {
                            name = f.Name,
                            type = f.Type == EntryType.File ? "f" : "d",
                            size = f.Size
                        });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new HttpException(404, "File Not Found");
                }
            }

            throw new HttpException(403, "Forbidden");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(string path, HttpPostedFileBase file)
        {
            path = NormalizePath(path);
            var fileName = Path.GetFileName(file.FileName);

            if (AuthorizeUpload(path, file))
            {
                file.SaveAs(Path.Combine(Server.MapPath(path), fileName));

                return Json(new
                {
                    size = file.ContentLength,
                    name = fileName,
                    type = "f"
                }, "text/plain");
            }

            throw new HttpException(403, "Forbidden");
        }

        [OutputCache(Duration = 360, VaryByParam = "path")]
        public ActionResult Image(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeImage(path))
            {
                var physicalPath = Server.MapPath(path);

                if (System.IO.File.Exists(physicalPath))
                {
                    const string contentType = "image/png";
                    return File(System.IO.File.OpenRead(physicalPath), contentType);
                }
            }

            throw new HttpException(403, "Forbidden");
        }

        public virtual bool AuthorizeImage(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }
        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }
        [OutputCache(Duration = 3600, VaryByParam = "path")]
        public virtual ActionResult Thumbnail(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeThumbnail(path))
            {
                var physicalPath = Server.MapPath(path);

                if (System.IO.File.Exists(physicalPath))
                {
                    Response.AddFileDependency(physicalPath);

                    return CreateThumbnail(physicalPath);
                }
                else
                {
                    throw new HttpException(404, "File Not Found");
                }
            }
            else
            {
                throw new HttpException(403, "Forbidden");
            }
        }

        private FileContentResult CreateThumbnail(string physicalPath)
        {
            using (var fileStream = System.IO.File.OpenRead(physicalPath))
            {
                var desiredSize = new Models.ImageSize
                {
                    Width = ThumbnailWidth,
                    Height = ThumbnailHeight
                };

                const string contentType = "image/png";

                return File(thumbnailCreator.Create(fileStream, desiredSize, contentType), contentType);
            }
        }




        #endregion
    }


}