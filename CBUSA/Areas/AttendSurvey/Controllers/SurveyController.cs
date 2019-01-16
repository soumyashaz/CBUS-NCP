using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBUSA.Services.Interface;
using CBUSA.Areas.AttendSurvey.Models;
using CBUSA.Domain;
using CBUSA.Models;
using CBUSA.Services;
using System.IO;
using Microsoft.Practices.Unity;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
namespace CBUSA.Areas.AttendSurvey.Controllers
{
    public class SurveyController : Controller
    {
        readonly ISurveyService _ObjSurveyService;
        readonly IQuestionService _ObjQuestionService;
        readonly IBuilderService _ObjBuilderService;

        public SurveyController(ISurveyService ObjSurveyService, IQuestionService ObjQuestionService,
            IBuilderService ObjBuilderService)
        {
            _ObjSurveyService = ObjSurveyService;
            _ObjQuestionService = ObjQuestionService;
            _ObjBuilderService = ObjBuilderService;
        }

        // GET: AttendSurvey/Survey
        public ActionResult Index()
        {
            return View();
        }




        public ActionResult ThankYou(Int64 SurveyId, Int64 BuilderId, bool IsSurveyCompleted)
        {
            var Survey = _ObjSurveyService.GetSurvey(SurveyId);
            if (Survey != null)
            {
                ViewBag.SurveyName = Survey.SurveyName;
                ViewBag.IsTakeSurveyCompleted = IsSurveyCompleted;
                ViewBag.SurveyId = Survey.SurveyId;
                ViewBag.BuilderId = BuilderId;
            }
            return View();
        }
        public ActionResult TakeSurvey(Int64? SurveyId, Int64? BuilderId)
        {

            if (SurveyId.HasValue && BuilderId.HasValue)
            {
                var Builder = _ObjBuilderService.GetSpecificBuilder(BuilderId.GetValueOrDefault());

                if (Builder == null)
                {
                    return RedirectToAction("Message", "Survey", new { Area = "AttendSurvey", id = 3 });
                }


                var Survey = _ObjSurveyService.GetSurvey(SurveyId.GetValueOrDefault());
                if (Survey != null)
                {


                    var IsAuthorizedToTakeSurvey = Survey.SurveyMarket.Where(x => x.MarketId == Builder.MarketId).Any();
                    if (!IsAuthorizedToTakeSurvey)
                    {
                        return RedirectToAction("Message", "Survey", new { Area = "AttendSurvey", id = 3 });
                    }
                    //  var IsAuthorize = _ObjSurveyService.IsBuilderAuthorizedToAcessSurvey(SurveyId.GetValueOrDefault(), BuilderId.GetValueOrDefault());
                    if (Survey.RowStatusId != (int)RowActiveStatus.Active)
                    {
                        return RedirectToAction("Message", "Survey", new { Area = "AttendSurvey", id = 1 });
                    }

                    var IsComplete = _ObjSurveyService.IsBuilderAllraedyCompleteSurvey(SurveyId.GetValueOrDefault(), BuilderId.GetValueOrDefault());
                    if (IsComplete)
                    {
                        return RedirectToAction("Message", "Survey", new { Area = "AttendSurvey", id = 2 });
                    }

                    var Question = _ObjQuestionService.GetQuestionWithIndexNo(SurveyId.GetValueOrDefault(), 1); //firs time default 1
                    var SurveyResult = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(SurveyId.GetValueOrDefault(), BuilderId.GetValueOrDefault(), Question.QuestionId);

                    if (Survey != null)
                    {

                        byte[] ContractImage = Survey.Contract.ContractIcon;
                        var ObjVm = new TakeSurveyViewModel
                        {

                            SurveyId = SurveyId.GetValueOrDefault(),
                            BuilderId = BuilderId.GetValueOrDefault(),
                            ObjSurvey = Survey,
                            ContractImage= ContractImage,
                            //ManufacturerName = Survey.Contract.Manufacturer.ManufacturerName,
                            ManufacturerName = GetManufacturer(Survey),
                            ObjTakeSurveyQuestion = new TakeSurveyQuestionViewModel
                            {
                                CurrentQuestionIndex = 1,
                                ObjSurveyResult = SurveyResult != null ? SurveyResult.ToList() : new List<SurveyResult> { },
                                Question = Question,
                                TotalQuestion = Survey.Question.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Count()
                            }


                        };
                        return View(ObjVm);
                    }
                }
            }
            else
            {
                return View(new TakeSurveyViewModel { });
            }

            return View(new TakeSurveyViewModel { });
        }


        [HttpPost]
        public JsonResult LoadSurveyQuestion(Int64 SurveyId, Int64 BuilderId, int QuestionIndex, int TotalQuestion)
        {

            var Question = _ObjQuestionService.GetQuestionWithIndexNo(SurveyId, QuestionIndex); //firs time default 1
            var SurveyResult = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(SurveyId, BuilderId, Question.QuestionId);

            var ObjTakeSurveyQuestion = new TakeSurveyQuestionViewModel
                {
                    CurrentQuestionIndex = QuestionIndex,
                    ObjSurveyResult = SurveyResult != null ? SurveyResult.ToList() : new List<SurveyResult> { },
                    Question = Question,
                    TotalQuestion = TotalQuestion
                };
            string PrtialViewString = PartialView("_SurveyQuestion", ObjTakeSurveyQuestion).RenderToString();
            return Json(new { IsSuccess = true, PartialView = PrtialViewString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSurveyResult(TakeSurveyViewModel ObjVm)
        {
            string ServerFilePath = Server.MapPath("~/ApplicationDocument/SurveyDocument");
            _ObjSurveyService.SaveSurveyResult(ObjVm.SurveyId, ObjVm.BuilderId, ObjVm.IsSurveyComplete, ObjVm.ObjTakeSurveyQuestion.ObjSurveyResult, ServerFilePath);
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


        public ActionResult Message(int id)
        {
            if (id == 1)
            {
                ViewBag.Authorized = "Survey already completed";
            }
            else if (id == 2)
            {
                ViewBag.Complete = "You have already completed this survey";
            }
            else
            {
                ViewBag.Complete = "You are not authorized to take the survey";
            }
            return View();
        }

        private string GetManufacturer(Survey objSurvey)
        {
            string strManufacturer = "";
            if (objSurvey.Contract.Manufacturer != null)
            {
                strManufacturer = objSurvey.Contract.Manufacturer.ManufacturerName;
            }
            else
            {
                strManufacturer = objSurvey.Contract.PrimaryManufacturer;
            }                
            return strManufacturer;
        }
    }
    
}