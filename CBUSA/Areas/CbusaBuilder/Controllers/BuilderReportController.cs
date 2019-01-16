using CBUSA.Areas.CbusaBuilder.Models;
using CBUSA.Domain;
using CBUSA.Services;
using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBUSA.Models;
using System.IO;
using Kendo.Mvc.UI;
using Microsoft.Practices.Unity;
using System.Text;
using System.Security.Claims;
namespace CBUSA.Areas.CbusaBuilder.Controllers
{
    public class BuilderReportController : Controller
    {

        readonly IContractServices _ObjContractService;
        readonly IQuaterService _ObjQuaterService;
        readonly IBuilderQuaterContractProjectReportService _ObjBuilderQuaterContractProjectReportService;
        readonly IProjectService _ObjProjectService;
        readonly IQuestionService _ObjQuestionService;
        readonly IBuilderQuaterAdminReportService _ObjBuilderQuaterAdminReportService;
        readonly ISurveyService _ObjSurveyService;
        readonly IBuilderQuaterContractProjectReportService _ObjQuaterContractProjectReport;
        readonly IBuilderQuarterContractStatusService _ObjBuilderQuaterContractStatusService;
        public BuilderReportController(IContractServices ObjContractService, IQuaterService ObjQuaterService,
            IBuilderQuaterContractProjectReportService ObjBuilderQuaterContractProjectReportService,
            IProjectService ObjProjectService, IQuestionService ObjQuestionService, IBuilderQuaterAdminReportService ObjBuilderQuaterAdminReportService, ISurveyService ObjSurveyService
            , IBuilderQuarterContractStatusService ObjBuilderQuaterContractStatusService, IBuilderQuaterContractProjectReportService ObjQuaterContractProjectReport)
        {
            _ObjContractService = ObjContractService;
            _ObjQuaterService = ObjQuaterService;
            _ObjBuilderQuaterContractProjectReportService = ObjBuilderQuaterContractProjectReportService;
            _ObjProjectService = ObjProjectService;
            _ObjQuestionService = ObjQuestionService;
            _ObjBuilderQuaterAdminReportService = ObjBuilderQuaterAdminReportService;
            _ObjSurveyService = ObjSurveyService;
            _ObjQuaterContractProjectReport = ObjQuaterContractProjectReport;
            _ObjBuilderQuaterContractStatusService = ObjBuilderQuaterContractStatusService;
        }

        // GET: CbusaBuilder/BuilderReport

        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Builder")]
        public ActionResult SubmitReport(Int64 ContractId)
        {
            ViewBag.Contract = ContractId;
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string id = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(id);
            var Contract = _ObjContractService.GetContract(ContractId);
            if (Contract != null)
            {
                DateTime currentdate = DateTime.Now.Date;
                Quater ObjQuater = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();

                var IsReportInitiated = _ObjBuilderQuaterAdminReportService.IsReportInitiated(BuilderId, ObjQuater.QuaterId);

                if (!IsReportInitiated)
                {
                    return RedirectToAction("RegularReporting", "Builder", new { Area = "CbusaBuilder" });
                }
                // here have to open regular reporting if admin has re-opened the ncp survey - angshuman on 10-july-2017
                var IsReportSubmit = _ObjBuilderQuaterAdminReportService.IsReportAllreadySubmited(BuilderId, ObjQuater.QuaterId);
                
                //if (IsReportSubmit)
                //{
                //    var CheckAdminEdit = _ObjSurveyService.GetNCPSurveyResponseEditStatus(BuilderId, ObjQuater.QuaterId, ContractId).Select(x => x.IsEditable).SingleOrDefault();
                //    if (CheckAdminEdit != true )
                //    {
                //        return RedirectToAction("ReportThankYou", "BuilderReport", new { Area = "CbusaBuilder" });
                //    }
                //}
                
                BuilderReportViewModel ObjVM = new BuilderReportViewModel
                {
                    BuilderId = BuilderId,
                    ContractId = ContractId,
                    ContractName = Contract.ContractName,
                    ContractIcon = Contract.ContractIcon,
                    QuaterName = ObjQuater.QuaterName + "  " + ObjQuater.Year,
                    QuaterId = ObjQuater.QuaterId
                };
                return View(ObjVM);
            }
            return View(new BuilderReportViewModel { });
        }

        [HttpPost]
        public JsonResult RenderProjectDropdown(Int64 ContractId, Int64 BuilderId, Int64 QuaterId)
        {
            CustomProjectControlVm ObjVm = null;
            //----------------------- Neyaz on 05/10/2017 #8172 ----- Start
            //var ActiveProject = _ObjProjectService.GetBuilderActiveProject(BuilderId);
            var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();
            var ActiveProject = _ObjProjectService.GetBuilderProject(ContractId, BuilderId, PreviousQuaterList);
            //----------------------- Neyaz on 05/10/2017 #8172 ----- End
            var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId).Select(x => x.ProjectId).ToList<Int64>();
            
            if (ActiveProject != null)
            {
                ObjVm = new CustomProjectControlVm { ProjectList = ActiveProject.ToList(), SelectedProject = SelectedProjectForQuater };
            }
            else
            {
                ObjVm = new CustomProjectControlVm { };
            }
            string PrtialViewString = PartialView("_ProjectCustomControl", ObjVm).RenderToString();
            return Json(new { IsSuccess = true, ProjectCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
        }

        // start -- commented authorization as admin role is not available as of 09-jun-2017 and admin would like to edit the ncp enrollment response 
        //[Authorize(Roles = "Builder")]
        // end -- commented authorization as admin role is not available as of 09-jun-2017 and admin would like to edit the ncp enrollment response 
        public JsonResult LoadAdminReport(Int64 ContractId, Int64 BuilderId, Int64 QuaterId)
        {
            var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId);
            if (SelectedProjectForQuater != null)
            {
                if (SelectedProjectForQuater.Count() > 0)
                {
                    Quater ObjQuater = _ObjQuaterService.GetQuaterById(QuaterId);

                    var QuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());
                    if (QuestionList == null)
                    {
                        QuestionList = _ObjQuestionService.GetBuilderReportQuestionHistory(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());
                        if (QuestionList == null)
                        {
                            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Ncp survey not yet configure " }) }, JsonRequestBehavior.AllowGet);
                        }                            
                    }

                    var AdminQuaterProjectReport = _ObjBuilderQuaterContractProjectReportService.GetBuilderSeletedProjectReportForQuater(BuilderId, QuaterId, ContractId);


                    if (AdminQuaterProjectReport == null)
                    {
                        return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Please select Project that you want to report for this month " }) }, JsonRequestBehavior.AllowGet);
                    }

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

                    // AdminReportResult = 

                    //Fetch Result
                    BuilderReportViewModel ObjVm = new BuilderReportViewModel
                    {
                        QuestionList = QuestionList.ToList(),
                        ProjectList = SelectedProjectForQuater.ToList(),
                        SurveyId = QuestionList.FirstOrDefault() != null ? QuestionList.FirstOrDefault().SurveyId : 0,
                        SubmitReport = AdminReportResult

                    };
                    string PrtialViewString = PartialView("_GenarateBuilderReport", ObjVm).RenderToString();
                    return Json(new { IsSuccess = true, ProjectCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Please make project before submit report" }) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddProjectAdminReport(Int64 ContractId, Int64 BuilderId, Int64 QuaterId, Int64 ProjectId)
        {
            //var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId);

            if (ProjectId > 0)
            {
                _ObjBuilderQuaterContractProjectReportService.UpdateBuilderProjectStatus(BuilderId, ContractId, QuaterId, ProjectId);

                var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId).Where(x => x.ProjectId == ProjectId);

                Quater ObjQuater = _ObjQuaterService.GetQuaterById(QuaterId);
                var QuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());
                if(QuestionList != null)
                {
                    BuilderReportViewModel ObjVm = new BuilderReportViewModel
                    {
                        QuestionList = QuestionList != null ? QuestionList.ToList() : null,
                        ProjectList = SelectedProjectForQuater.FirstOrDefault() != null ? SelectedProjectForQuater.ToList() : null,
                        SurveyId = QuestionList.FirstOrDefault() != null ? QuestionList.FirstOrDefault().SurveyId : 0,
                        SubmitReport = new List<BuilderReportSubmitViewModel>()
                    };
                    string PrtialViewString = PartialView("_AddProjectToReport", ObjVm).RenderToString();
                    return Json(new { IsSuccess = true, ProjectCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    BuilderReportViewModel ObjVm = new BuilderReportViewModel();
                    string PrtialViewString = PartialView("_AddProjectToReport", ObjVm).RenderToString();
                    return Json(new { IsSuccess = true, ProjectCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult AddMultipleProjectAdminReport(Int64 ContractId, Int64 BuilderId, Int64 QuaterId, List<Int64> ListProjectId)
        {
            if (ListProjectId != null)
            {
                if (ListProjectId.Count > 0)
                {
                    _ObjBuilderQuaterContractProjectReportService.UpdateBuilderMultipleProjectStatus(BuilderId, ContractId, QuaterId, ListProjectId);
                    var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId).Where(x => ListProjectId.Contains(x.ProjectId));
                    Quater ObjQuater = _ObjQuaterService.GetQuaterById(QuaterId);
                    var QuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());
                    if(QuestionList != null)
                    {
                        BuilderReportViewModel ObjVm = new BuilderReportViewModel
                        {
                            //QuestionList = QuestionList.ToList(),
                            //ProjectList = SelectedProjectForQuater.ToList()
                            //,
                            //SurveyId = QuestionList.FirstOrDefault() != null ? QuestionList.FirstOrDefault().SurveyId : 0,
                            //SubmitReport = new List<BuilderReportSubmitViewModel>()

                            QuestionList = QuestionList != null ? QuestionList.ToList() : null,
                            ProjectList = SelectedProjectForQuater.FirstOrDefault() != null ? SelectedProjectForQuater.ToList() : null,
                            SurveyId = QuestionList.FirstOrDefault() != null ? QuestionList.FirstOrDefault().SurveyId : 0,
                            SubmitReport = new List<BuilderReportSubmitViewModel>()
                        };
                        string PrtialViewString = PartialView("_AddProjectToReport", ObjVm).RenderToString();
                        return Json(new { IsSuccess = true, ProjectCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        BuilderReportViewModel ObjVm = new BuilderReportViewModel();
                        string PrtialViewString = PartialView("_AddProjectToReport", ObjVm).RenderToString();
                        return Json(new { IsSuccess = true, ProjectCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
        }
        //[Authorize(Roles = "Builder")]
        public JsonResult SaveAdminReport(BuilderReportViewModel ObjVm)
        {
            try
            {
                if (ObjVm.SubmitReport.Count > 0)
                {
                    var ProjectLis = ObjVm.SubmitReport.Select(x => x.ProjectId).Distinct().ToList();
                    var BuilderReport = ObjVm.SubmitReport.Select(x => new BuilderQuaterContractProjectDetails
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
                    //bool CheckSurveyResponseEditStatus = _ObjSurveyService.GetNCPSurveyResponseEditStatus(ObjVm.BuilderId, ObjVm.QuaterId, ObjVm.ContractId).Select(x => x.IsEditable).SingleOrDefault();

                    //_ObjBuilderQuaterContractProjectReportService.SaveBuilderProjectResult(ObjVm.ContractId, ObjVm.BuilderId, ObjVm.QuaterId, ProjectLis, BuilderReport, ServerFilePath, CheckSurveyResponseEditStatus);
                    _ObjBuilderQuaterContractProjectReportService.SaveBuilderProjectResult(ObjVm.ContractId, ObjVm.BuilderId, ObjVm.QuaterId, ProjectLis, BuilderReport, ServerFilePath);
                    _ObjProjectService.CheckProjectReportStatus(ObjVm.BuilderId, ObjVm.QuaterId, ObjVm.ContractId);
                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Internal server error" }) }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Save([DataSourceRequest] DataSourceRequest request, IEnumerable<HttpPostedFileBase> files, Int16 uploadID, string PreviousUploadedFiles)
        {
            var savedFilePaths = new List<string>();
            string FileNameToSave = "";
            string ConcatenatedFileNames = "";
            string ActualFileNames = "";
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
                    string physicalPath = Path.Combine(Server.MapPath("~/ApplicationDocument/NCPRebateReportDocument"), FileNameToSave);
                    file.SaveAs(physicalPath);
                    savedFilePaths.Add(FileNameToSave);

                    ConcatenatedFileNames = String.Concat(ConcatenatedFileNames, FileNameToSave, ";");
                    ActualFileNames = String.Concat(ActualFileNames, fileName, ";");
                }

                ConcatenatedFileNames = ConcatenatedFileNames.Substring(0, (ConcatenatedFileNames.Length - 1));
                ActualFileNames = ActualFileNames.Substring(0, (ActualFileNames.Length - 1));
            }

            //Delete previously uploaded files
            if (PreviousUploadedFiles != null && PreviousUploadedFiles != "")
            {
                string[] arrPrevFileNames = PreviousUploadedFiles.Split(';');

                foreach (string strPrevFileName in arrPrevFileNames)
                {
                    var physicalPath = Path.Combine(Server.MapPath("~/ApplicationDocument/NCPRebateReportDocument"), strPrevFileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            return Json(new { IsUpload = true, UploadFileName = ConcatenatedFileNames, ActualFileNames  = ActualFileNames });
            // Return an empty string to signify success
            // return Json(new[] { savedFilePaths }.ToDataSourceResult(request));
        }

        public ActionResult Remove(string[] fileNames, string ActualFileNames, string UploadedFileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    string[] arrActualFileNames = ActualFileNames.Split(';');
                    string[] arrUploadedFileNames = UploadedFileNames.Split(';');

                    int Counter = 0;
                    string UploadedFileName = "";

                    foreach (string ActualFileName in arrActualFileNames)
                    {
                        if (ActualFileName == fileName)         //match found
                        {
                            UploadedFileName = arrUploadedFileNames[Counter];

                            ActualFileNames = ActualFileNames.Replace(ActualFileName, "").Replace(";;", ";");
                            UploadedFileNames = UploadedFileNames.Replace(UploadedFileName, "").Replace(";;", ";");

                            break;
                        }

                        Counter++;
                    }

                    var physicalPath = Path.Combine(Server.MapPath("~/ApplicationDocument/NCPRebateReportDocument"), UploadedFileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            // Return an empty string to signify success
            return Json(new { IsUpload = false, UploadFileName = UploadedFileNames, ActualFileNames = ActualFileNames });
        }

        public FileResult DownloadResourceFile(string FileName)
        {
            string ServerFilePath = Server.MapPath("~/ApplicationDocument/NCPRebateReportDocument");
            var physicalPath = Path.Combine(ServerFilePath, FileName);
            return File(physicalPath, MimeMapping.GetMimeMapping(FileName), FileName);
        }

        //public JsonResult LoadAdminReportView(Int64 ContractId, Int64 BuilderId, Int64 QuaterId)
        //{
        //    //  var AdminQuaterProjectReport = _ObjBuilderQuaterContractProjectReportService.GetBuilderSeletedProjectReportForQuater(BuilderId, QuaterId, ContractId);
        //    var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId);
        //    if (SelectedProjectForQuater != null)
        //    {
        //        if (SelectedProjectForQuater.Count() > 0)
        //        {
        //            Quater ObjQuater = _ObjQuaterService.GetQuaterById(QuaterId);
        //            var QuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());

        //            var AdminQuaterProjectReport = _ObjBuilderQuaterContractProjectReportService.GetBuilderSeletedProjectReportForQuater(BuilderId, QuaterId, ContractId);
        //            List<BuilderReportSubmitViewModel> AdminReportResult = new List<BuilderReportSubmitViewModel>();
        //            foreach (var Item in AdminQuaterProjectReport)
        //            {
        //                AdminReportResult.AddRange(Item.BuilderQuaterContractProjectDetails.Select(x => new BuilderReportSubmitViewModel
        //                {
        //                    ProjectId = Item.ProjectId,
        //                    FileName = x.FileName,
        //                    RowNumber = x.RowNumber,
        //                    ColumnNumber = x.ColumnNumber,
        //                    QuestionId = x.QuestionId,
        //                    Answer = x.Answer

        //                }).ToList());
        //            };

        //            // AdminReportResult = 

        //            //Fetch Result
        //            BuilderReportViewModel ObjVm = new BuilderReportViewModel
        //            {
        //                QuestionList = QuestionList.ToList(),
        //                ProjectList = SelectedProjectForQuater.ToList(),
        //                SurveyId = QuestionList.FirstOrDefault() != null ? QuestionList.FirstOrDefault().SurveyId : 0,
        //                SubmitReport = AdminReportResult

        //            };
        //            string PrtialViewString = PartialView("_GenarateBuilderReport", ObjVm).RenderToString();
        //            return Json(new { IsSuccess = true, ProjectCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult BuilderReportView(Int64 ContractId, Int64 QuaterId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string id = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(id);
            // added by angshuman on 29-apr-2017
            //var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId);
            var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuaterHistory(BuilderId, QuaterId, ContractId);            
            if (SelectedProjectForQuater != null)
            {
                if (SelectedProjectForQuater.Count() > 0)
                {
                    Quater ObjQuater = _ObjQuaterService.GetQuaterById(QuaterId);
                    // added by angshuman on 29-apr-2017
                    var QuestionList = _ObjQuestionService.GetBuilderReportQuestionHistory(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());
                    //var QuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());
                    var AdminQuaterProjectReport = _ObjBuilderQuaterContractProjectReportService.GetBuilderSeletedProjectReportForQuater(BuilderId, QuaterId, ContractId);
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
                    // AdminReportResult = 
                    int ProjectCounter = 1;
                    var selectedProjectListForQtr = SelectedProjectForQuater.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        ProjectId = x.ProjectId,
                        ProjectName = x.ProjectName,
                        LotNo = x.LotNo,
                        State = x.State,
                        Address = x.Address,                        
                        rowcount = ProjectCounter++
                    }).ToList();

                    //Fetch Result
                    BuilderReportViewModel ObjVm = new BuilderReportViewModel
                    {
                        QuestionList = QuestionList.ToList(),
                        //ProjectList = SelectedProjectForQuater.ToList(),
                        ProjectListVM = selectedProjectListForQtr.ToList(),
                        SurveyId = QuestionList.FirstOrDefault() != null ? QuestionList.FirstOrDefault().SurveyId : 0,
                        SubmitReport = AdminReportResult

                    };
                    // string PrtialViewString = PartialView("_GenarateBuilderReport", ObjVm).RenderToString();
                    //  return Json(new { IsSuccess = true, ProjectCustomControl = PrtialViewString }, JsonRequestBehavior.AllowGet);
                    return View(ObjVm);
                }
            }
            BuilderReportViewModel ObjVm1 = new BuilderReportViewModel
            {
            };
            return View(ObjVm1);
        }

        public ActionResult DownLoadContractMarketBuilderView(Int64 ContractId, Int64 QuaterId)
        {

            Int64 BuilderId = 2;

            var SelectedProjectForQuater = _ObjProjectService.GetBuilderSeletedProjectForQuater(BuilderId, QuaterId, ContractId);
            if (SelectedProjectForQuater != null)
            {
                if (SelectedProjectForQuater.Count() > 0)
                {
                    Quater ObjQuater = _ObjQuaterService.GetQuaterById(QuaterId);
                    var QuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());
                    var AdminQuaterProjectReport = _ObjBuilderQuaterContractProjectReportService.GetBuilderSeletedProjectReportForQuater(BuilderId, QuaterId, ContractId);
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
                    // AdminReportResult = 
                    //Fetch Result
                    BuilderReportViewModel ObjVm = new BuilderReportViewModel
                    {
                        QuestionList = QuestionList.ToList(),
                        ProjectList = SelectedProjectForQuater.ToList(),
                        SurveyId = QuestionList.FirstOrDefault() != null ? QuestionList.FirstOrDefault().SurveyId : 0,
                        SubmitReport = AdminReportResult

                    };

                    //        StringBuilder SbBuilder = new StringBuilder();
                    //        SbBuilder.Append("<table>");
                    //        SbBuilder.Append("<thead>");
                    //        SbBuilder.Append("<tr>");
                    //        SbBuilder.Append("<th></th>");

                    //        foreach(var Item in SelectedProjectForQuater)
                    //        {
                    //              SbBuilder.Append("<tr>");
                    //              SbBuilder.Append("<td>"+Item.ProjectName + " ," + " ," + Item.LotNo + " ," + Item.State + " ," + Item.AddressZip+"</td>");
                    //            foreach(var Item in SelectedProjectForQuater)
                    //        {



                    //             SbBuilder.Append("</tr>");
                    //        }
                    //        SbBuilder.Append("</tr>");
                    //        SbBuilder.Append("</thead>");
                    //        SbBuilder.Append("</tbody>");
                    //        foreach(var Item in QuestionList)
                    //        {
                    //             SbBuilder.Append("<th>"+Item.QuestionValue+"</th>");
                    //        }


                    //        SbBuilder.Append("</tbody>");

                    //        SbBuilder.Append("</table>");

                    //        <tr>

                    //    @if (Model.ProjectList != null)
                    //    {
                    //        <th width="15%" class="text-left"></th>
                    //        foreach (var ItemChild in Model.QuestionList)
                    //        {
                    //            <th class="text-center">@ItemChild.QuestionValue</th>
                    //        }
                    //    }

                    //</tr>

                }
            }




            //StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/CustomTemplate/PdfTemplate/CntractBuilderTemplate.html"));
            //StringBuilder Sb = new StringBuilder(reader.ReadToEnd());
            //Sb.Replace("####ContractBuilderList####", SbBuilder.ToString());
            //string str = Sb.ToString();
            //Idownload Download = new PdfDownload();
            //Download.Download(Sb.ToString());
            return View();
        }

        public ActionResult ReportThankYou()
        {
            return View();
        }
        #region Created By Rajendar 3/14/2018
        
        #endregion

    }
}