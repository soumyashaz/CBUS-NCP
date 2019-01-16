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
using iTextSharp.text;
using Newtonsoft.Json;
using System.Security.Claims;
using CBUSA.Models;
using System.Threading.Tasks;

namespace CBUSA.Areas.CbusaBuilder.Controllers
{
    public class ProjectController : Controller
    {
        // GET: CbusaBuilder/Project
        readonly IContractStatusService _ObjContractStatusServices;
        readonly IResourceCategoryService _ObjResourceCategoryService;
        readonly IResourceService _ObjResourceService;
        readonly IManufacturerService _ObjManufactureService;
        readonly IContractServices _ObjContractService;
        readonly IContractRebateService _ObjContractRebateService;
        readonly IContractBuilderService _ObjContractBuilderService;
        readonly ISurveyService _ObjSurveyService;
        readonly ISurveyBuilderService _ObjSurveyBuilderService;
        readonly IStateService _ObjStateService;
        readonly ICityService _ObjCityService;
        readonly IProjectService _ObjProjectService;
        readonly IQuaterService _ObjQuaterService;
        readonly IProjectStatusService _ObjProjectStatusService;
        readonly IBuilderQuaterAdminReportService _ObjQuaterAdminReportService;
        readonly IBuilderQuarterContractStatusService _ObjBuilderQuaterContractStatusService;
        readonly IBuilderQuaterContractProjectReportService _ObjQuaterContractProjectReport;
        public ProjectController(IContractStatusService ObjContractStatusServices, IResourceCategoryService ObjResourceCategoryService,
            IResourceService ObjResourceService, IManufacturerService ObjManufactureService,
            IContractServices ObjContractService, IContractRebateService ObjContractRebateService, IContractBuilderService ObjContractBuilderService,
            ISurveyService ObjSurveyService, ISurveyBuilderService ObjSurveyBuilderService, IStateService ObjStateService, ICityService ObjCityService, IProjectService ObjProjectService, IQuaterService ObjQuaterService, IProjectStatusService ObjProjectStatusService, IBuilderQuaterAdminReportService ObjQuaterAdminReportService, IBuilderQuaterContractProjectReportService ObjQuaterContractProjectReport
           , IBuilderQuarterContractStatusService ObjBuilderQuaterContractStatusService)
        {
            _ObjContractStatusServices = ObjContractStatusServices;
            _ObjResourceCategoryService = ObjResourceCategoryService;
            _ObjResourceService = ObjResourceService;
            _ObjManufactureService = ObjManufactureService;
            _ObjContractService = ObjContractService;
            _ObjContractRebateService = ObjContractRebateService;
            _ObjContractBuilderService = ObjContractBuilderService;
            _ObjSurveyService = ObjSurveyService;
            _ObjSurveyBuilderService = ObjSurveyBuilderService;
            _ObjStateService = ObjStateService;
            _ObjCityService = ObjCityService;
            _ObjProjectService = ObjProjectService;
            _ObjQuaterService = ObjQuaterService;
            _ObjProjectStatusService = ObjProjectStatusService;
            _ObjQuaterAdminReportService = ObjQuaterAdminReportService;
            _ObjQuaterContractProjectReport = ObjQuaterContractProjectReport;
            _ObjBuilderQuaterContractStatusService = ObjBuilderQuaterContractStatusService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RowCount()
        {
            ProjectViewModel ObjProjectView = new ProjectViewModel();
            ObjProjectView.rowcount = 0;
            return View(ObjProjectView);
        }

        [Authorize(Roles = "Builder")]
        public ActionResult ActiveProjectList([DataSourceRequest] DataSourceRequest request, string Type)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            //Int64 BuilderId = 2; //putbuilderid here
            //var ProjectList = _ObjProjectService.BuilderProjectList(BuilderId).Where(x => x.RowStatusId == (int)RowActiveStatus.Active).

            int ProjectCounter = 1;

            //IEnumerable<dynamic> projlist = _ObjProjectService.BuilderProjectListUsingProc(BuilderId, 1);

            //var ProjectList = _ObjProjectService.BuilderProjectList(BuilderId).
            var ProjectList = _ObjProjectService.BuilderProjectListUsingProc(BuilderId, 1).
               Select(x => new ProjectViewModel
               {
                   rowNum = ProjectCounter++,
                   IconList = GetContractIconFromIdList(x.ReportedContractIds),
                   ProjectId = x.ProjectId,
                   BuilderId = x.BuilderId,
                   ProjectName = x.ProjectName,
                   LotNo = x.LotNo,
                   Address = x.Address,
                   City = x.City,
                   State = x.State,
                   StateId = x.StateId,
                   Zip = x.Zip,
                   RowStatusId = x.RowStatusId
               }).OrderBy(x => x.RowStatusId).ToList();

            var jsonResult = Json(ProjectList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [Authorize(Roles = "Builder")]
        public ActionResult CloseProjectList([DataSourceRequest] DataSourceRequest request, string Type)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            int ProjectCounter = 1;

            var ProjectList = _ObjProjectService.BuilderProjectList(BuilderId).Where(x => x.RowStatusId == (int)RowActiveStatus.Deleted).
               Select(x => new ProjectViewModel
               {
                   IconList = GetContractIcon(x.BuilderId, x.ProjectId),
                   ProjectId = x.ProjectId,
                   BuilderId = x.BuilderId,
                   ProjectName = x.ProjectName,
                   LotNo = x.LotNo,
                   Address = x.Address,
                   City = x.City,
                   State = x.State,
                   StateId = getstateid(x.State),
                   Zip = x.Zip,
                   RowStatusId = x.RowStatusId
               }).OrderBy(x => x.RowStatusId);
            //return Json(ProjectList.ToDataSourceResult(request));
            var newList = ProjectList.ToList()
                 .Select(x => new ProjectViewModel
                 {
                     rowNum = ProjectCounter++,
                     IconList = x.IconList,
                     ProjectId = x.ProjectId,
                     BuilderId = x.BuilderId,
                     ProjectName = x.ProjectName,
                     LotNo = x.LotNo,
                     Address = x.Address,
                     City = x.City,
                     State = x.State,
                     StateId = x.StateId,
                     Zip = x.Zip,
                     RowStatusId = x.RowStatusId
                 }).ToList();
            var jsonResult = Json(newList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public List<string> GetContractIcon(Int64 BuilderId, Int64 ProjectId)
        {
            byte[] ContractIcon = null;
            string VendorName = null;
            List<string> LogoImage = new List<string>();
            //var ContractId = _ObjQuaterContractProjectReport.GetLatestContractAgainstBuilderProject(BuilderId, ProjectId).Select(x=> x.ContractId).Distinct().ToList();
            var Contract = _ObjQuaterContractProjectReport.GetLatestContractAgainstBuilderProject(BuilderId, ProjectId).Where(w => w.ProjectStatusId != 2);
            var ContractId = Contract.Select(x => x.ContractId).Distinct().ToList();

            for (var i = 0; i < ContractId.Count; i++)
            {
                //Console.WriteLine("Amount is {0} and type is {1}", myMoney[i].amount, myMoney[i].type);

                if (ContractId[i] > 0)
                {
                    var contract = _ObjContractService.GetContract(ContractId[i]);
                    //ContractIcon = _ObjContractService.GetContract(ContractId[i]).ContractIcon;
                    ContractIcon = contract.ContractIcon;
                    VendorName = contract.Manufacturer.ManufacturerName;
                }
                string LogoImageBase64 = "";
                try
                {
                    if (ContractIcon != null)
                    {
                        LogoImageBase64 = Convert.ToBase64String(ContractIcon);
                        LogoImageBase64 = string.Format("data:image/png;base64,{0}", LogoImageBase64);
                        LogoImage.Add(LogoImageBase64);
                    }
                    else
                        LogoImage.Add(VendorName);
                }
                catch (Exception ee)
                {
                    //LogoImage = "";
                }
            }
            return LogoImage;
        }

        public List<string> GetContractIconFromIdList(string ReportedContractIds)
        {
            List<string> LogoImage = new List<string>();

            if (ReportedContractIds != "")
            {
                string ContractLogoImage = "";

                List<long> ContractIdList = ReportedContractIds.Split(',').Select(long.Parse).ToList();

                for (var i = 0; i < ContractIdList.Count; i++)
                {
                    if (ContractIdList[i] > 0)
                    {
                        switch (ContractIdList[i])
                        {
                            case 33:
                                ContractLogoImage = "/Content/Logos/Caesarstone.png";
                                break;

                            case 32:
                                ContractLogoImage = "/Content/Logos/Dacor.png";
                                break;

                            case 37:
                                ContractLogoImage = "/Content/Logos/DonaldGardner.png";
                                break;

                            case 25:
                                ContractLogoImage = "/Content/Logos/Electrolux.png";
                                break;

                            case 30:
                                ContractLogoImage = "/Content/Logos/Generac.png";
                                break;

                            case 22:
                                ContractLogoImage = "/Content/Logos/JamesHardie.png";
                                break;

                            case 29:
                                ContractLogoImage = "/Content/Logos/JeldWen.png";
                                break;

                            case 34:
                                ContractLogoImage = "/Content/Logos/Kohler.png";
                                break;

                            case 28:
                                ContractLogoImage = "/Content/Logos/KPBuilding.png";
                                break;

                            case 23:
                                ContractLogoImage = "/Content/Logos/Lennox.png";
                                break;

                            case 31:
                                ContractLogoImage = "/Content/Logos/LP.png";
                                break;

                            case 26:
                                ContractLogoImage = "/Content/Logos/Rinnai.png";
                                break;

                            case 27:
                                ContractLogoImage = "/Content/Logos/Schlage.png";
                                break;

                            case 24:
                                ContractLogoImage = "/Content/Logos/WayneDalton.png";
                                break;

                            case 38:
                                ContractLogoImage = "/Content/Logos/Maxim.png";
                                break;
                        }

                        LogoImage.Add(ContractLogoImage);
                    }
                }
            }

            return LogoImage;
        }

        //public string GetContractIcon(Int64 BuilderId, Int64 ProjectId)
        //{
        //    byte[] ContractIcon = null;
        //    var ContractId = _ObjQuaterContractProjectReport.GetLatestContractAgainstBuilderProject(BuilderId, ProjectId).Select(x=> x.ContractId).FirstOrDefault();
        //    if(ContractId > 0)
        //    {
        //        //Console.WriteLine("Amount is {0} and type is {1}", myMoney[i].amount, myMoney[i].type);

        //        if (ContractId[i] > 0)
        //        {
        //            var contract = _ObjContractService.GetContract(ContractId[i]);
        //            //ContractIcon = _ObjContractService.GetContract(ContractId[i]).ContractIcon;
        //            ContractIcon = contract.ContractIcon;
        //            VendorName = contract.Manufacturer.ManufacturerName;
        //        }
        //        string LogoImageBase64 = "";
        //        try
        //        {
        //            if (ContractIcon != null)
        //            {
        //                LogoImageBase64 = Convert.ToBase64String(ContractIcon);
        //                LogoImageBase64 = string.Format("data:image/png;base64,{0}", LogoImageBase64);
        //                LogoImage.Add(LogoImageBase64);
        //            }
        //            else
        //                LogoImage.Add(VendorName);
        //        }
        //        catch (Exception ee)
        //        {
        //            //LogoImage = "";
        //        }
        //    }
        //    return LogoImage;
        //}

        //    return LogoImageBase64;
        //}

        public int getstateid(string state)
        {
            int StateId = _ObjStateService.GetStateByName(state).Select(x => x.StateId).FirstOrDefault();
            return StateId;
        }
        public int getcityid(string city)
        {
            int CityId = _ObjCityService.GetCityByName(city).Select(x => x.CityId).FirstOrDefault();
            return CityId;
        }
        [Authorize(Roles = "Builder")]
        public ActionResult AddProject(int? status, Int64? ContractId)
        {
            if (status == 1)
            {
                ViewBag.redirect = "addstatus";
                ViewBag.ContractId = ContractId;
            }
            else if (status == 2)
            {
                ViewBag.redirect = "report";
                ViewBag.ContractId = ContractId;
            }

            //TempData["ContractId"] = ContractId;

            ProjectViewModel ObjProjectView = new ProjectViewModel();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            DateTime currentdate = DateTime.Now.Date;
            var Quater = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();

            bool IsQuarterReportSubmitted = _ObjQuaterAdminReportService.IsReportAllreadySubmited(BuilderId, Quater.QuaterId);

            if (!IsQuarterReportSubmitted)
            {
                ViewBag.ReportedContractList = GetReportedContractNames(BuilderId);
            }
            else
            {
                ViewBag.ReportedContractList = "";
            }

            //Int64 BuilderId = 2; //putbuilderid here
            //IEnumerable<Project> Project = _ObjProjectService.BuilderProjectList(BuilderId);
            //IEnumerable<Project> Project = _ObjProjectService.BuilderProjectListUsingProc(BuilderId, 1);
            //List<ProjectViewModel> ObjVm = Project.Select(x => new ProjectViewModel
            //{
            //    BuilderId = x.BuilderId,
            //    ProjectName = x.ProjectName,
            //    LotNo = x.LotNo,
            //    StateId = getstateid(x.State),
            //    CityId = getcityid(x.City),
            //    State = x.State,
            //    City = x.City,
            //    Address = x.Address,
            //    Zip = x.Zip,
            //    rowcount = 0
            //}).ToList();

            return View();

            //var BuilderProject = Project.ContractBuilder.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.Builder)
            //  .GroupBy(x => x.MarketId);

            //List<ProjectViewModel> ObjMarketBuilderList = new List<ProjectViewModel>();
            //foreach (IGrouping<long, Project> BuilderGrp in Project)
            //{
            //    ProjectViewModel ObjTemp = new ProjectViewModel();

            //    ObjTemp.BuilderId = BuilderGrp.Where(x=>x.BuilderId);
            //    ObjTemp.MarketName = BuilderGrp.Where(x => x.Market.MarketId == BuilderGrp.Key).FirstOrDefault().Market.MarketName;
            //    ObjTemp.Builders = BuilderGrp.ToList();

            //    ObjMarketBuilderList.Add(ObjTemp);
            //}

            //ObjProjectView.rowcount = 0;
            //ViewBag.rowcount = 0;

            //return View(ObjProjectView);
            //return View(Project);
        }

        private string GetReportedContractNames(Int64 BuilderId)
        {
            string ReportedContractList = "";

            DateTime currentdate = DateTime.Now.Date;
            var Quater = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();

            if (currentdate >= Quater.ReportingStartDate && currentdate <= Quater.ReportingEndDate)
            {
                IEnumerable<BuilderQuarterContractStatus> ReportedContracts = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderQuater(BuilderId, Quater.QuaterId)
                                                                            .Where(bqcs => bqcs.ProjectReportStatusId == (long)ProjectReportStatusEnum.Completed);

                if (ReportedContracts.Count() > 0)
                {
                    ReportedContractList = String.Concat(ReportedContractList, "<ul>");
                    foreach (BuilderQuarterContractStatus bqcs in ReportedContracts)
                    {
                        ReportedContractList = String.Concat(ReportedContractList, "<li>", bqcs.Contract.ContractName, "</li>");
                    }
                    ReportedContractList = String.Concat(ReportedContractList, "</ul>");
                }
            }
            return ReportedContractList;
        }

        [Authorize(Roles = "Builder")]
        public ActionResult ManageClosedProject(int? status, Int64? ContractId)
        {
            if (status == 1)
            {
                ViewBag.redirect = "addstatus";
                ViewBag.ContractId = ContractId;
            }
            else if (status == 2)
            {
                ViewBag.redirect = "report";
                ViewBag.ContractId = ContractId;
            }

            ProjectViewModel ObjProjectView = new ProjectViewModel();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            //Int64 BuilderId = 2; //putbuilderid here
            IEnumerable<Project> Project = _ObjProjectService.BuilderProjectList(BuilderId).Where(x => x.RowStatusId == (int)RowActiveStatus.Deleted);
            List<ProjectViewModel> ObjVm = Project.Select(x => new ProjectViewModel
            {
                BuilderId = x.BuilderId,
                ProjectName = x.ProjectName,
                LotNo = x.LotNo,
                StateId = getstateid(x.State),
                CityId = getcityid(x.City),
                State = x.State,
                City = x.City,
                Address = x.Address,
                Zip = x.Zip,
                rowcount = 0
            }).ToList();
            return View(ObjVm);

            //ViewBag.rowcount = 0;
            //return View(ObjProjectView);
        }

        public ActionResult CopyProject(Int64 ProjectId, Int64? ContractId, bool MarkNTRForReportedContracts)
        {
            Project Objproj = _ObjProjectService.CopyProject(ProjectId).FirstOrDefault();
            Project ObjProject = new Project();

            Int64 LastProjectId = 0;
            IEnumerable<Project> ProjectList = null;
            string SQLQuery = "";
            ProjectList = _ObjProjectService.ProjectList().Where(x => x.BuilderId == Objproj.BuilderId);
            if (ProjectList.Count() > 0)
            {
                //LastProjectId = _ObjProjectService.ProjectList().Where(x => x.BuilderId == Objproj.BuilderId).Max(x => x.ProjectId);

                SQLQuery = " Select max(ProjectId) ProjectId ";
                SQLQuery += " From Project a ";
                SQLQuery += " where a.builderid = " + Objproj.BuilderId;

                var MaxProjectIdBefore = _ObjProjectService.GetDataIntoListQuery(SQLQuery);
                if (MaxProjectIdBefore != null && MaxProjectIdBefore.Count() > 0)
                {
                    foreach (var item in MaxProjectIdBefore)
                    {
                        LastProjectId = Convert.ToInt64(item[0]);
                    }
                }
            }

            string strProjectName = Objproj.ProjectName;

            if (strProjectName.LastIndexOf("_") >= 0)
            {
                //string CopyCounter = strProjectName.Substring(strProjectName.LastIndexOf("_"), 1);
                int CopyCounter = strProjectName.Split('_').Length - 1;
                strProjectName = String.Concat(strProjectName, "_", CopyCounter);
            }
            else
            {
                strProjectName = String.Concat(strProjectName, "_1");
            }

            ObjProject.BuilderId = Objproj.BuilderId;
            //ObjProject.ProjectName = Objproj.ProjectName + "_Copy";
            ObjProject.ProjectName = strProjectName;
            ObjProject.LotNo = Objproj.LotNo;
            ObjProject.State = Objproj.State;
            ObjProject.City = Objproj.City;
            ObjProject.Address = Objproj.Address;
            ObjProject.Zip = Objproj.Zip;
            ObjProject.RowStatusId = Objproj.RowStatusId;
            _ObjProjectService.SaveProject(ObjProject, false);

            DateTime currentdate = DateTime.Now.Date;
            var Quater = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
            Int64 LastestProjectId = 0;
            //LastestProjectId = _ObjProjectService.ProjectList().Where(x => x.BuilderId == Objproj.BuilderId).Max(x => x.ProjectId);
            SQLQuery = " Select max(ProjectId) ProjectId ";
            SQLQuery += " From Project a ";
            SQLQuery += " where a.builderid = " + Objproj.BuilderId;

            var MaxProjectIdAfter = _ObjProjectService.GetDataIntoListQuery(SQLQuery);
            if (MaxProjectIdAfter != null && MaxProjectIdAfter.Count() > 0)
            {
                foreach (var item in MaxProjectIdAfter)
                {
                    LastestProjectId = Convert.ToInt64(item[0]);
                }
            }
            if (LastestProjectId > LastProjectId)
            {
                if (currentdate >= Quater.ReportingStartDate && currentdate <= Quater.ReportingEndDate)
                {
                    if (Objproj.RowStatusId == (int)RowActiveStatus.Active)
                    {
                        AssociateProjectToContract(Objproj.BuilderId, ContractId, LastProjectId, MarkNTRForReportedContracts);
                    }
                }
            }

            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Builder")] //---- added authorization on 2-may-2018 - angshuman as MSTN tickets for bull references.
        public ActionResult CloseProject(Int64 ProjectId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            var BuilderReport = _ObjQuaterAdminReportService.GetBuilderReport(BuilderId);
            if (BuilderReport.Count() > 0)
            {
                //  bool IsProjectInUse = false;
                List<Int64> UseProject = new List<long>();
                foreach (var Item in BuilderReport)
                {
                    UseProject.AddRange(Item.BuilderQuaterContractProjectReport.Where(y => y.ProjectStatusId == (int)EnumProjectStatus.ReportUnit).Select(x => x.ProjectId));
                }
                if (!UseProject.Contains(ProjectId))
                {

                    //   Int64 BuilderQuaterAdminReportId = _ObjQuaterAdminReportService.GetBuilderReport(BuilderId).FirstOrDefault().BuilderQuaterAdminReportId;
                    //  int projectscount = _ObjQuaterContractProjectReport.GetRepotDetailsofSpecificAdminContractId(BuilderQuaterAdminReportId).Count();
                    //  var IsProjectInUse=_ObjQuaterAdminReportService.BuilderQuaterContractProjectReport
                    // if (projectscount == 0)
                    //  {
                    Project Objproj = _ObjProjectService.CopyProject(ProjectId).FirstOrDefault();
                    Project ObjProject = new Project();

                    Objproj.RowStatusId = (int)RowActiveStatus.Deleted;
                    _ObjProjectService.UpdateProject(Objproj);
                    return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {

                Project Objproj = _ObjProjectService.CopyProject(ProjectId).FirstOrDefault();
                Project ObjProject = new Project();

                Objproj.RowStatusId = (int)RowActiveStatus.Deleted;
                _ObjProjectService.UpdateProject(Objproj);
                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                // return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }




        }
        public ActionResult ReopenProject(Int64 ProjectId)
        {
            Project Objproj = _ObjProjectService.CopyProject(ProjectId).FirstOrDefault();
            Project ObjProject = new Project();

            Objproj.RowStatusId = (int)RowActiveStatus.Active;
            _ObjProjectService.UpdateProject(Objproj);
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetState()
        {

            Int64 conid = 0;
            var Statelist = _ObjStateService.GetState().OrderBy(x => x.StateName).Select(x => new { stateid = x.StateId, statename = x.StateName }).ToList();




            return Json(Statelist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FillCity(string state)
        {
            int StateId = _ObjStateService.GetStateByName(state).Select(x => x.StateId).FirstOrDefault();
            var Citylist = _ObjCityService.FindCityByState(StateId).Select(x => new { cityid = x.CityId, cityname = x.CityName }).ToList();
            return Json(Citylist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FillCityByState(int StateId)
        {

            var Citylist = _ObjCityService.FindCityByState(StateId).Select(x => new { cityid = x.CityId, cityname = x.CityName }).ToList();
            return Json(Citylist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCity()
        {

            Int64 conid = 0;
            var Citylist = _ObjCityService.GetCity().Select(x => new { cityid = x.CityId, cityname = x.CityName }).ToList();




            return Json(Citylist, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Builder")] //---- added authorization on 2-may-2018 - angshuman as MSTN tickets for bull references.
        public JsonResult SaveProject(string[][] array, Int64? ContractId, bool MarkNTRForReportedContracts)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            //int BuilderId = 2; //pubuilderid here
            DateTime currentdate = DateTime.Now.Date;
            var Quater = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
            List<Project> ObjProjectList = new List<Project>();
            //    bool Flag = false;
            bool Message = false;
            //   _ObjProjectService.Flag(Flag);
            int arrcount = array.Count();
            //var count = _ObjProjectService.BuilderProjectList(BuilderId).Count();
            //int start = array.Length - Convert.ToInt32(count);
            //if (start >= 0)
            //{
            //    int m = 0;
            //    while (m < count)
            //    {
            //        int indexToRemove = 0;
            //        array = array.Where((source, index) => index != indexToRemove).ToArray();
            //        m++;
            //        //array.ToList().RemoveAt(m);
            //    }
            //}
            //if (arrcount != count)
            //{
            IEnumerable<Project> ProjectList = null;
            ProjectList = _ObjProjectService.ProjectList().Where(x => x.BuilderId == BuilderId);

            Int64 LastProjectId = 0;

            string SQLQuery = "";
            if (ProjectList.Count() > 0)
            {
                //LastProjectId = _ObjProjectService.ProjectList().Where(x => x.BuilderId == BuilderId).Max(x => x.ProjectId);


                SQLQuery = " Select max(ProjectId) ProjectId ";
                SQLQuery += " From Project a ";
                SQLQuery += " where a.builderid = " + BuilderId;
                //SQLQuery += "  and a.quaterid = " + Quater.QuaterId;
                //SQLQuery += "  and a.contractid =" + ContractId;

                var MaxProjectIdBefore = _ObjProjectService.GetDataIntoListQuery(SQLQuery);
                if (MaxProjectIdBefore != null && MaxProjectIdBefore.Count() > 0)
                {
                    foreach (var item in MaxProjectIdBefore)
                    {
                        LastProjectId = Convert.ToInt64(item[0]);
                    }
                }
            }

            foreach (var arr in array)
            {
                Project ObjProject = new Project();

                //for (int i = 0; i < arr.Length; i++)
                //{
                //}
                if (arr[0] != "")
                {
                    ObjProject.ProjectId = Convert.ToInt64(arr[8]);
                    ObjProject.ProjectName = arr[2];
                    ObjProject.LotNo = arr[3];
                    ObjProject.Address = arr[4];
                    ObjProject.City = arr[5];
                    ObjProject.State = arr[6];
                    ObjProject.Zip = arr[7];
                    ObjProject.BuilderId = BuilderId;
                    ObjProject.RowStatusId = (int)RowActiveStatus.Active;

                    ObjProjectList.Add(ObjProject);
                }
            }
            _ObjProjectService.SaveBuilderProject(ObjProjectList);
            //   Flag = true;
            // _ObjProjectService.Flag(Flag);

            Int64 LastestProjectId = 0;
            //LastestProjectId = _ObjProjectService.ProjectList().Where(x => x.BuilderId == BuilderId).Max(x => x.ProjectId);


            SQLQuery = " Select max(ProjectId) ProjectId ";
            SQLQuery += " From Project a ";
            SQLQuery += " where a.builderid = " + BuilderId;
            //SQLQuery += "  and a.quaterid = " + Quater.QuaterId;
            //SQLQuery += "  and a.contractid =" + ContractId;

            var MaxProjectIdAfter = _ObjProjectService.GetDataIntoListQuery(SQLQuery);
            if (MaxProjectIdAfter != null && MaxProjectIdAfter.Count() > 0)
            {
                foreach (var item in MaxProjectIdAfter)
                {
                    LastestProjectId = Convert.ToInt64(item[0]);
                }
            }

            if (LastestProjectId > LastProjectId)
            {
                if (currentdate >= Quater.ReportingStartDate && currentdate <= Quater.ReportingEndDate)
                {
                    AssociateProjectToContract(BuilderId, ContractId, LastProjectId, MarkNTRForReportedContracts);
                }
            }
            Message = true;
            return Json(Message, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(Message, JsonRequestBehavior.AllowGet);
            //}
        }
        [Authorize(Roles = "Builder")] //---- added authorization on 2-may-2018 - angshuman as MSTN tickets for bull references.
        public ActionResult SaveProjectData(string values)
        {

            dynamic jsonDe = JsonConvert.DeserializeObject(values);

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            //Int64 BuilderId = 2; //putbilderid here

            Int64 QuaterId = 0;
            DateTime currentdate = DateTime.Now.Date;
            var QuaterName = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterName;

            var Year = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().Year;

            QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            //foreach (var item in jsonDe)
            //{
            //    BuilderId = item.builderid;
            //    QuaterId = item.quaterid;
            //}
            var quateradminreportid = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, QuaterId).Select(x => x.BuilderQuaterAdminReportId).FirstOrDefault();
            BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();

            bool Flag = false;

            _ObjQuaterContractProjectReport.Flag(Flag);
            foreach (var item in jsonDe)
            {


                Int64 ProjectId = item.projectid;
                // -- angshuman on 24-april-2018.
                Int64 BuilderQuarterContractStatusId = 0;
                string SQLQuery = "";
                SQLQuery = " Select BuilderQuarterContractStatus ";
                SQLQuery += " From BuilderQuarterContractStatus a ";
                SQLQuery += " where a.builderid = " + BuilderId;
                SQLQuery += "  and a.quaterid = " + item.quaterid;
                SQLQuery += "  and a.contractid =" + item.contractid;
                SQLQuery += "  and a.RowStatusId = 1 ";

                var QuarterContractStatus = _ObjProjectService.GetDataIntoListQuery(SQLQuery);
                if (QuarterContractStatus != null && QuarterContractStatus.Count() > 0)
                {
                    foreach (var data in QuarterContractStatus)
                    {
                        BuilderQuarterContractStatusId = Convert.ToInt64(data[0]);
                    }
                }
                objProjectReport.BuilderQuaterAdminReportId = Convert.ToInt64(quateradminreportid);
                objProjectReport.CompleteDate = DateTime.Now.Date;

                int count = checkcount(BuilderId, QuaterId, ProjectId);
                if (count == 0)
                {
                    objProjectReport.ProjectId = item.projectid;
                    objProjectReport.BuilderId = item.builderid;
                    objProjectReport.QuaterId = item.quaterid;
                    objProjectReport.ContractId = item.contractid;
                    objProjectReport.ProjectStatusId = item.status;
                    objProjectReport.IsComplete = false;
                    objProjectReport.BuilderQuarterContractStatusId = BuilderQuarterContractStatusId;
                    _ObjQuaterContractProjectReport.SaveProjectReport(objProjectReport);
                }
                else
                {
                    objProjectReport = _ObjQuaterContractProjectReport.CheckExistProjectAgainstBuilderQuater(BuilderId, QuaterId, ProjectId).FirstOrDefault();
                    objProjectReport.BuilderId = BuilderId;
                    objProjectReport.QuaterId = QuaterId;
                    objProjectReport.ProjectId = ProjectId;
                    objProjectReport.BuilderQuarterContractStatusId = BuilderQuarterContractStatusId;
                    _ObjQuaterContractProjectReport.UpdateProjectReport(objProjectReport);
                }


            }
            Flag = true;
            _ObjQuaterContractProjectReport.Flag(Flag);
            // dynamic o = jsonDe[0];
            // string val = o.projectid;
            return Json(JsonRequestBehavior.AllowGet);
        }
        public int checkcount(Int64 BuilderId, Int64 QuaterId, Int64 ProjectId)
        {
            int count = _ObjQuaterContractProjectReport.CheckExistProjectAgainstBuilderQuater(BuilderId, QuaterId, ProjectId).Count();
            return count;
        }

        public int getCountAllContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId, Int64 ProjectId)
        {
            int count = _ObjQuaterContractProjectReport.CheckExistAllContractAgainstBuilderQuater(BuilderId, QuaterId, ContractId, ProjectId).Count();
            return count;
        }
        public IEnumerable<BuilderQuaterContractProjectReport> GetCountAllContractAgainstBuilderQuaterOnly(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjQuaterContractProjectReport.CheckExistAllContractAgainstBuilderQuater(BuilderId, QuaterId);
        }
        public JsonResult ManageTableRows()
        {

            Int64 conid = 0;
            List<int> rows = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                rows.Add(i);
            }




            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProject()
        {
            Int64 conid = 0;
            var ProjectList = _ObjProjectService.ProjectList().Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new { ProjectId = x.ProjectId, ProjectName = x.ProjectName }).ToList();


            ProjectList.Insert(0, new { ProjectId = conid, ProjectName = "Select Project" });



            return Json(ProjectList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProjectStatus()
        {
            Int64 conid = 0;
            var ProjectList = _ObjProjectStatusService.GetProjectStatus().Select(x => new { ProjectId = x.ProjectStatusId, ProjectName = x.StatusName }).ToList();


            ProjectList.Insert(0, new { ProjectId = conid, ProjectName = "Select Project Status" });



            return Json(ProjectList, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Builder")]
        public ActionResult ActiveProjetListWithStatus([DataSourceRequest] DataSourceRequest request, string Type, string Flag)
        {


            var ProjectStatuslist = _ObjProjectStatusService.GetProjectStatus();
            DateTime currentdate = DateTime.Now.Date;

            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;


            string Status1 = "";
            string Status2 = "";
            string Status3 = "";
            Int64 Status1Id = 0;
            Int64 Status2Id = 0;
            Int64 Status3Id = 0;

            for (int i = 0; i < ProjectStatuslist.Count(); i++)
            {
                if (i == 0)
                {
                    Status1 = ProjectStatuslist.Select(x => x.StatusName).ElementAt(0).ToString();
                    Status1Id = Convert.ToInt64(ProjectStatuslist.Select(x => x.ProjectStatusId).ElementAt(0));

                }
                else if (i == 1)
                {
                    Status2 = ProjectStatuslist.Select(x => x.StatusName).ElementAt(1).ToString();
                    Status2Id = Convert.ToInt64(ProjectStatuslist.Select(x => x.ProjectStatusId).ElementAt(1));
                }
                else
                {
                    Status3 = ProjectStatuslist.Select(x => x.StatusName).ElementAt(2).ToString();
                    Status3Id = Convert.ToInt64(ProjectStatuslist.Select(x => x.ProjectStatusId).ElementAt(2));
                }
            }
            IEnumerable<Project> ProjectList = null;
            IEnumerable<ProjectViewModel> list;
            if (Type == null && Flag == null)
            {
                ProjectList = _ObjProjectService.ProjectList();

            }
            else if (Type == "" && Flag == "")
            {
                ProjectList = _ObjProjectService.ProjectList();

            }
            else if (Type != "" && Type != null)
            {
                if (Type == "asccon")
                {
                    ProjectList = _ObjProjectService.ProjectList().OrderBy(x => x.ProjectName);
                }
                else if (Type == "desccon")
                {
                    ProjectList = _ObjProjectService.ProjectList().OrderByDescending(x => x.ProjectName);
                }

            }
            else if (Flag != "" && Flag != null)
            {
                ProjectList = _ObjProjectService.CopyProject(Convert.ToInt64(Flag));

            }
            list = ProjectList.Select(x => new ProjectViewModel
            {
                ProjectId = x.ProjectId,
                BuilderId = x.BuilderId,
                ProjectName = x.ProjectName + "," + x.LotNo + "," + x.Address,
                LotNo = x.LotNo,
                //StateId = x.StateId,
                //CityId = x.CityId,
                Address = x.Address,
                Status1 = Status1,
                StatusSelectId = checkstatus(x.BuilderId, QuaterId, x.ProjectId),
                Status2 = Status2,
                Status3 = Status3,
                Status1Id = Status1Id,
                Status2Id = Status2Id,
                Status3Id = Status3Id
            });
            return Json(list.ToDataSourceResult(request));
        }
        public Int64 checkstatus(Int64 BuilderId, Int64 QuaterId, Int64 ProjectId)
        {
            Int64 StatusId = 0;
            int count = _ObjQuaterContractProjectReport.CheckExistProjectAgainstBuilderQuater(BuilderId, QuaterId, ProjectId).Count();
            if (count > 0)
            {
                StatusId = _ObjQuaterContractProjectReport.CheckExistProjectAgainstBuilderQuater(BuilderId, QuaterId, ProjectId).FirstOrDefault().ProjectStatusId;
            }

            return StatusId;
        }
        [Authorize(Roles = "Builder")]
        public ActionResult ProjectStatus()
        {


            return PartialView("_ProjectStatus");

        }

        [Authorize(Roles = "Builder")] //---- added authorization on 2-may-2018 - angshuman as MSTN tickets for bull references.
        public ActionResult InsertQuaterAdminReport(string qauterid)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = "";
            bool data = false;
            if (claims != null && claims.Count() > 1)
            {
                bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            }
            if (bldid != "" && bldid.Length > 0)
            {
                Int64 BuilderId = Convert.ToInt64(bldid);

                if (qauterid != null && qauterid.Length > 0)
                {
                    // Int64 BuilderId=2;  //PutBuilderId
                    var count = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, Convert.ToInt64(qauterid)).Count();
                    if (Convert.ToInt32(count) == 0)
                    {
                        data = true;
                        BuilderQuaterAdminReport objReport = new BuilderQuaterAdminReport();
                        objReport.BuilderId = BuilderId;
                        objReport.QuaterId = Convert.ToInt64(qauterid);
                        objReport.SubmitDate = DateTime.Now.Date;
                        objReport.IsSubmit = false;
                        _ObjQuaterAdminReportService.SaveBuilderQaterReport(objReport);

                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }



        }
        #region Old Code For UpdateNoReportAllContract,UpdateNoReportToContract,InsertQuaterAdminReportIfNotExists 
        //public JsonResult UpdateNoReportToContract(Int64 QuaterId, Int64 ContractId)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);
        //    IEnumerable<Project> ProjectList = null;
        //    //            
        //    ProjectList = _ObjProjectService.ProjectList().Where(x => x.RowStatusId == (int)RowActiveStatus.Active && x.BuilderId == BuilderId);
        //    if (ProjectList.Count() == 0)
        //    {
        //        return Json(new { Success = false, DataMessage = "Project not added. Please add project." });
        //    }
        //    InsertQuaterAdminReportIfNotExists(QuaterId, BuilderId);
        //    var quateradminreportid = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, QuaterId).Select(x => x.BuilderQuaterAdminReportId).FirstOrDefault();
        //    if (quateradminreportid > 0)
        //    {
        //        var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();
        //        ProjectList = null;
        //        ProjectList = _ObjProjectService.GetContractBuilderCurrentQuaterProject(ContractId, BuilderId, PreviousQuaterList, 0);
        //        var CurrentQuaterSelectedProject = _ObjQuaterContractProjectReport.GetBuilderCurrentQuaterSelectedProject(BuilderId, ContractId, QuaterId);

        //        var ProjectStatusListAsc = ProjectList.GroupJoin(CurrentQuaterSelectedProject, x => x.ProjectId, y => y.ProjectId,
        //        (x, y) => new { AllAvailProject = x, SaveProject = y })
        //        .SelectMany(x => x.SaveProject.DefaultIfEmpty(), (x, y) => new
        //        {
        //            ProjectId = x.AllAvailProject.ProjectId,
        //            ProjectName = x.AllAvailProject.ProjectName + " ," + x.AllAvailProject.LotNo + " ," + x.AllAvailProject.State + " ," + x.AllAvailProject.Address,
        //            ProjectStatus = x.SaveProject != null ? (x.SaveProject.FirstOrDefault() != null ? x.SaveProject.FirstOrDefault().ProjectStatusId : 0) : 0
        //        }).OrderBy(x => x.ProjectName);

        //        if (ProjectStatusListAsc.Count() > 0)
        //        {
        //            List<BuilderQuaterContractProjectReport> Container = new List<BuilderQuaterContractProjectReport>();
        //            foreach (var item in ProjectStatusListAsc)
        //            {
        //                Int64 ProjectId = item.ProjectId;
        //                int count = getCountAllContractAgainstBuilderQuater(BuilderId, QuaterId, ContractId, ProjectId);
        //                //int count = checkcount(BuilderId, QuaterId, ProjectId);
        //                if (count > 0)
        //                {
        //                    BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
        //                    objProjectReport = _ObjQuaterContractProjectReport.CheckExistProjectAgainstBuilderQuaterContract(BuilderId, QuaterId, ContractId, ProjectId).FirstOrDefault();
        //                    objProjectReport.BuilderId = BuilderId;
        //                    objProjectReport.QuaterId = QuaterId;
        //                    objProjectReport.ProjectId = ProjectId;
        //                    objProjectReport.ProjectStatusId = 2; // Nothing to report this quarter
        //                    objProjectReport.IsComplete = true;
        //                    objProjectReport.ModifiedOn = DateTime.Now.Date;
        //                    Container.Add(objProjectReport);
        //                }
        //                else
        //                {
        //                    BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
        //                    objProjectReport.ProjectId = ProjectId;
        //                    objProjectReport.BuilderId = BuilderId;
        //                    objProjectReport.QuaterId = QuaterId;
        //                    objProjectReport.ContractId = ContractId;
        //                    objProjectReport.ProjectStatusId = 2; // Nothing to report this quarter
        //                    objProjectReport.BuilderQuaterAdminReportId = quateradminreportid;
        //                    objProjectReport.IsComplete = true;
        //                    objProjectReport.CompleteDate = DateTime.Now.Date;
        //                    objProjectReport.RowStatusId = 1;
        //                    objProjectReport.CreatedOn = DateTime.Now.Date;
        //                    objProjectReport.CreatedBy = 1;
        //                    objProjectReport.ModifiedOn = DateTime.Now.Date;
        //                    objProjectReport.ModifiedBy = 1;
        //                    objProjectReport.RowGUID = Guid.NewGuid();
        //                    _ObjQuaterContractProjectReport.SaveProjectReport(objProjectReport, false);
        //                }
        //            }
        //            if (Container.Count() > 0)
        //            {
        //                _ObjQuaterContractProjectReport.UpdateBuilderContractProjectStatus(Container, true);
        //            }
        //            return Json(new { Success = true, DataMessage = "Project status updated successfully." });
        //        }
        //        else
        //            return Json(new { Success = false, DataMessage = "There are no projects associated to this contract. Please add projects." });
        //    }
        //    else
        //    {
        //        ProjectList = null;
        //        ProjectList = _ObjProjectService.ProjectList().Where(x => x.RowStatusId == (int)RowActiveStatus.Active && x.BuilderId == BuilderId);
        //        if (ProjectList.Count() == 0)
        //        {
        //            return Json(new { Success = false, DataMessage = "Project not added. Please add project." });
        //        }
        //        else
        //            return Json(new { Success = false, DataMessage = "" });
        //    }
        //}

        //public JsonResult UpdateNoReportAllContract(Int64 QuaterId)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);
        //    IEnumerable<Project> ProjectList = null;
        //    //
        //    ProjectList = _ObjProjectService.ProjectList().Where(x => x.RowStatusId == (int)RowActiveStatus.Active && x.BuilderId == BuilderId);
        //    if (ProjectList.Count() == 0)
        //    {
        //        return Json(new { Success = false, DataMessage = "Project not added. Please add project." });
        //    }
        //    InsertQuaterAdminReportIfNotExists(QuaterId, BuilderId);
        //    var quateradminreportid = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, QuaterId).Select(x => x.BuilderQuaterAdminReportId).FirstOrDefault();
        //    if (quateradminreportid > 0)
        //    {
        //        var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();
        //        IEnumerable<Contract> ContractList = null;
        //        //ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReporting(BuilderId).ToList();
        //        ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).ToList();
        //        //
        //        List<BuilderQuaterContractProjectReport> Container = new List<BuilderQuaterContractProjectReport>();
        //        foreach (var Contract in ContractList)
        //        {
        //            ProjectList = null;
        //            ProjectList = _ObjProjectService.GetContractBuilderCurrentQuaterProject(Contract.ContractId, BuilderId, PreviousQuaterList, 0);
        //            var CurrentQuaterSelectedProject = _ObjQuaterContractProjectReport.GetBuilderCurrentQuaterSelectedProject(BuilderId, Contract.ContractId, QuaterId);
        //            var ProjectStatusListAsc = ProjectList.GroupJoin(CurrentQuaterSelectedProject, x => x.ProjectId, y => y.ProjectId,
        //                            (x, y) => new { AllAvailProject = x, SaveProject = y })
        //                            .SelectMany(x => x.SaveProject.DefaultIfEmpty(), (x, y) => new
        //                            {
        //                                ProjectId = x.AllAvailProject.ProjectId,
        //                                ProjectName = x.AllAvailProject.ProjectName + " ," + x.AllAvailProject.LotNo + " ," + x.AllAvailProject.State + " ," + x.AllAvailProject.Address,
        //                                ProjectStatus = x.SaveProject != null ? (x.SaveProject.FirstOrDefault() != null ? x.SaveProject.FirstOrDefault().ProjectStatusId : 0) : 0
        //                            }).OrderBy(x => x.ProjectName);
        //            foreach (var item in ProjectStatusListAsc)
        //            {
        //                Int64 ProjectId = item.ProjectId;
        //                int count = getCountAllContractAgainstBuilderQuater(BuilderId, QuaterId, Contract.ContractId, ProjectId);
        //                if (count > 0)
        //                {
        //                    BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
        //                    objProjectReport = _ObjQuaterContractProjectReport.CheckExistProjectAgainstBuilderQuaterContract(BuilderId, QuaterId, Contract.ContractId, ProjectId).FirstOrDefault();
        //                    objProjectReport.BuilderId = BuilderId;
        //                    objProjectReport.QuaterId = QuaterId;
        //                    objProjectReport.ContractId = Contract.ContractId;
        //                    objProjectReport.ProjectId = ProjectId;
        //                    objProjectReport.ProjectStatusId = 2; // Nothing to report this quarter
        //                    objProjectReport.IsComplete = true;
        //                    objProjectReport.ModifiedOn = DateTime.Now.Date;
        //                    Container.Add(objProjectReport);
        //                }
        //                else
        //                {
        //                    BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
        //                    objProjectReport.ProjectId = ProjectId;
        //                    objProjectReport.BuilderId = BuilderId;
        //                    objProjectReport.QuaterId = QuaterId;
        //                    objProjectReport.ContractId = Contract.ContractId;
        //                    objProjectReport.ProjectStatusId = 2; // Nothing to report this quarter
        //                    objProjectReport.BuilderQuaterAdminReportId = quateradminreportid;
        //                    objProjectReport.IsComplete = true;
        //                    objProjectReport.CompleteDate = DateTime.Now.Date;
        //                    objProjectReport.RowStatusId = 1;
        //                    objProjectReport.CreatedOn = DateTime.Now.Date;
        //                    objProjectReport.CreatedBy = 1;
        //                    objProjectReport.ModifiedOn = DateTime.Now.Date;
        //                    objProjectReport.ModifiedBy = 1;
        //                    objProjectReport.RowGUID = Guid.NewGuid();
        //                    _ObjQuaterContractProjectReport.SaveProjectReport(objProjectReport, false);
        //                }
        //            }
        //        }
        //        if (Container.Count() > 0)
        //        {
        //            _ObjQuaterContractProjectReport.UpdateBuilderContractProjectStatus(Container, true);
        //        }
        //        return Json(new { Success = true, DataMessage = "Project(s) For All Contract(s) Are Updated Successfully As 'Nothing To Report This Quarter'." });
        //    }
        //    else
        //    {
        //        ProjectList = null;
        //        ProjectList = _ObjProjectService.ProjectList().Where(x => x.RowStatusId == (int)RowActiveStatus.Active && x.BuilderId == BuilderId);
        //        if (ProjectList.Count() == 0)
        //        {
        //            return Json(new { Success = false, DataMessage = "Project not added. Please add project." });
        //        }
        //        else
        //            return Json(new { Success = false, DataMessage = "" });
        //    }
        //}
        //private void AssociateProjectToContract(Int64 BuilderId, Int64? ContractId, Int64 LastProjectId)
        //{
        //    Int64 QuaterId = 0;
        //    DateTime currentdate = DateTime.Now.Date;
        //    QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
        //    BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
        //    //
        //    Int64 quateradminreportid = 0;
        //    InsertQuaterAdminReportIfNotExists(QuaterId, BuilderId, ref quateradminreportid);
        //    if (quateradminreportid > 0)
        //    {
        //        IEnumerable<Project> ProjectList = null;
        //        ProjectList = _ObjProjectService.ProjectList().Where(x => x.RowStatusId == (int)RowActiveStatus.Active && x.BuilderId == BuilderId && x.ProjectId > LastProjectId);

        //        //var ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContractName)
        //        //    .Select(x => new { ContractId = x.Contract.ContractId, ContractName = x.Contract.ContractName }).ToList();           
        //        IEnumerable<Contract> ContractList = null;
        //        if (ContractId > 0)
        //        {
        //            //===== Neyaz on 04-Dec-2017====== VSO#9519
        //            //ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReporting(BuilderId).ToList().Where(x => x.ContractId != ContractId);
        //            ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).ToList().Where(x => x.ContractId != ContractId);

        //        }
        //        else
        //        {
        //            //===== Neyaz on 04-Dec-2017====== VSO#9519
        //            //ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReporting(BuilderId).ToList();
        //            ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).ToList();
        //        }
        //        //
        //        foreach (var Contract in ContractList)
        //        {
        //            foreach (var item in ProjectList)
        //            {
        //                Int64 ProjectId = item.ProjectId;
        //                objProjectReport.BuilderQuaterAdminReportId = Convert.ToInt64(quateradminreportid);
        //                objProjectReport.CompleteDate = DateTime.Now.Date;
        //                int count = getCountAllContractAgainstBuilderQuater(BuilderId, QuaterId, Contract.ContractId, ProjectId);
        //                if (count == 0)
        //                {
        //                    objProjectReport.ProjectId = item.ProjectId;
        //                    objProjectReport.BuilderId = BuilderId;
        //                    objProjectReport.QuaterId = QuaterId;
        //                    objProjectReport.ContractId = Contract.ContractId;
        //                    objProjectReport.ProjectStatusId = 2; // Nothing to report this quarter                                                        
        //                    objProjectReport.IsComplete = true;
        //                    objProjectReport.RowGUID = Guid.NewGuid();
        //                    _ObjQuaterContractProjectReport.SaveProjectReport(objProjectReport, false);
        //                }
        //            }
        //        }
        //        _ObjQuaterContractProjectReport.ForceDisposeDbContext();
        //    }
        //}
        private void InsertQuaterAdminReportIfNotExists(Int64 qauterid, Int64 BuilderId)
        {
            var count = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, qauterid).Count();
            if (Convert.ToInt32(count) == 0)
            {
                BuilderQuaterAdminReport objReport = new BuilderQuaterAdminReport();
                objReport.BuilderId = BuilderId;
                objReport.QuaterId = qauterid;
                objReport.SubmitDate = DateTime.Now.Date;
                objReport.IsSubmit = false;
                objReport.RowGUID = Guid.NewGuid();
                _ObjQuaterAdminReportService.SaveBuilderQaterReport(objReport, false);
            }
        }
        //public ActionResult ChangeQuaterAdminReport()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);

        //    //Int64 BuilderId = 2; //putbuilderid here
        //    DateTime currentdate = DateTime.Now.Date;
        //    var QuaterRecord = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
        //    var QuaterName = QuaterRecord.QuaterName;

        //    var Year = QuaterRecord.Year;

        //    Int64 QuaterId = QuaterRecord.QuaterId;
        //    BuilderQuaterAdminReport objreport = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, QuaterId).FirstOrDefault();
        //    // BuilderQuaterAdminReport objreport = new BuilderQuaterAdminReport();
        //    Int64 QuaterAdminReportId = objreport.BuilderQuaterAdminReportId;
        //    objreport.IsSubmit = true;
        //    objreport.SubmitDate = DateTime.Now.Date;
        //    _ObjQuaterAdminReportService.UpdateBuilderAdminQuaterReport(objreport);

        //    var AllprojectOfQuater = _ObjQuaterContractProjectReport.CheckAllContractReportSubmit(BuilderId, QuaterId);
        //    foreach (var item in AllprojectOfQuater)
        //    {
        //        //BuilderQuaterContractProjectReport objContractReport=new BuilderQuaterContractProjectReport();
        //        //objContractReport.BuilderQuaterAdminReportId=QuaterAdminReportId;
        //        item.BuilderQuaterAdminReportId = QuaterAdminReportId;
        //        _ObjQuaterContractProjectReport.UpdateMultipleProjectReport(item);

        //    }

        //    _ObjQuaterContractProjectReport.GetDispose();
        //    bool Status = true;
        //    return Json(Status, JsonRequestBehavior.AllowGet);
        //}
        //[Authorize(Roles = "Builder")]
        //public JsonResult SaveProjectStatus(string[][] array)
        //{
        //    if (array != null)
        //    {
        //        var ContractId = array[0][0];
        //        var BuilderId = array[0][1];
        //        var QuaterId = array[0][2];

        //        Int64 BuilderQuaterReportId = 0;
        //        var BuilderQuaterReport = _ObjQuaterAdminReportService.GetBuilderQuaterReport(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId)).FirstOrDefault();
        //        if (BuilderQuaterReport != null)
        //        {
        //            BuilderQuaterReportId = BuilderQuaterReport.BuilderQuaterAdminReportId;
        //        }
        //        else
        //        {
        //            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Error occurs due to server" }) }, JsonRequestBehavior.AllowGet);
        //        }
        //        List<BuilderQuaterContractProjectReport> Container = new List<BuilderQuaterContractProjectReport>();

        //        foreach (var arr in array)
        //        {
        //            Project ObjProject = new Project();

        //            BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
        //            objProjectReport.ProjectId = Convert.ToInt64(arr[3]);
        //            objProjectReport.BuilderId = Convert.ToInt64(arr[1]);
        //            objProjectReport.QuaterId = Convert.ToInt64(arr[2]);
        //            objProjectReport.ContractId = Convert.ToInt64(arr[0]);
        //            objProjectReport.ProjectStatusId = Convert.ToInt64(arr[4]);
        //            objProjectReport.IsComplete = Convert.ToInt64(arr[4]) == 1 ? false : true;
        //            objProjectReport.CreatedOn = DateTime.Now;
        //            objProjectReport.ModifiedOn = DateTime.Now;
        //            objProjectReport.CreatedBy = 1;
        //            objProjectReport.ModifiedBy = 1;
        //            objProjectReport.CompleteDate = DateTime.Now;
        //            objProjectReport.RowGUID = Guid.NewGuid();

        //            objProjectReport.BuilderQuaterAdminReportId = BuilderQuaterReportId;
        //            Container.Add(objProjectReport);
        //        }

        //        _ObjQuaterContractProjectReport.SaveBuilderProjectStatus(Container);
        //        return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(new { IsSuccess = false, ModelError = "Please select a project which you like to report for this contract" }, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region New Code For UpdateNoReportAllContract,UpdateNoReportToContract,InsertQuaterAdminReportIfNotExists
        [Authorize(Roles = "Builder")] //---- added authorization on 2-may-2018 - angshuman as MSTN tickets for bull references.
        public JsonResult UpdateNoReportToContract(Int64 QuaterId, Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            Int64 quateradminreportid = 0;
            InsertQuaterAdminReportIfNotExists(QuaterId, BuilderId, ref quateradminreportid);
            if (quateradminreportid > 0)
            {
                var BuilderQuaterContractStatus = _ObjBuilderQuaterContractStatusService.CheckExistContractAgainstBuilderQuater(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId), Convert.ToInt64(ContractId)).FirstOrDefault();
                if (BuilderQuaterContractStatus != null)
                {
                    BuilderQuaterContractStatus.QuarterContractStatusId = (Int64)QuarterContractStatusEnum.NothingToReportThisQuarter;
                    BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed;
                    BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
                    _ObjBuilderQuaterContractStatusService.UpdateBuilderQuarterContractStatus(BuilderQuaterContractStatus);
                }
                else
                {
                    BuilderQuaterContractStatus = new BuilderQuarterContractStatus()
                    {
                        BuilderId = BuilderId,
                        ContractId = ContractId,
                        QuaterId = QuaterId,
                        QuarterContractStatusId = (Int64)QuarterContractStatusEnum.NothingToReportThisQuarter,
                        ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed,
                        BuilderQuaterAdminReportId = quateradminreportid,
                        SubmitDate = DateTime.Now.Date,

                    };
                    _ObjBuilderQuaterContractStatusService.AddBuilderQuarterContractStatus(BuilderQuaterContractStatus);
                }
                SaveProjectsOnNTRTQ(QuaterId, BuilderId, ContractId, quateradminreportid, BuilderQuaterContractStatus.BuilderQuarterContractStatusId);

                return Json(new { Success = true, DataMessage = "Project status updated successfully." });
            }
            else
                return Json(new { Success = false, DataMessage = "There are no projects associated to this contract. Please add projects." });


        }
        [Authorize(Roles = "Builder")] //---- added authorization on 2-may-2018 - angshuman as MSTN tickets for bull references.
        public ActionResult UpdateNoReportAllContract(Int64 QuaterId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            Int64 quateradminreportid = 0;
            InsertQuaterAdminReportIfNotExists(QuaterId, BuilderId, ref quateradminreportid);
            if (quateradminreportid > 0)
            {
                IEnumerable<Contract> ContractList = null;
                var NeverReportForThisContractList = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.QuarterContractStatusId == (Int64)QuarterContractStatusEnum.NeverReportForThisContract).Select(s => s.ContractId);
                ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).Where(w => !NeverReportForThisContractList.Contains(w.ContractId));

                List<BuilderQuarterContractStatus> Container = new List<BuilderQuarterContractStatus>();
                foreach (var Contract in ContractList)
                {
                    var BuilderQuaterContractStatus = _ObjBuilderQuaterContractStatusService.CheckExistContractAgainstBuilderQuater(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId), Convert.ToInt64(Contract.ContractId)).FirstOrDefault();
                    if (BuilderQuaterContractStatus != null)
                    {
                        BuilderQuaterContractStatus.QuarterContractStatusId = (Int64)QuarterContractStatusEnum.NothingToReportThisQuarter;
                        BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed;
                        BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
                        _ObjBuilderQuaterContractStatusService.UpdateBuilderQuarterContractStatus(BuilderQuaterContractStatus);
                    }
                    else
                    {
                        BuilderQuaterContractStatus = new BuilderQuarterContractStatus()
                        {
                            BuilderId = BuilderId,
                            ContractId = Contract.ContractId,
                            QuaterId = QuaterId,
                            QuarterContractStatusId = (Int64)QuarterContractStatusEnum.NothingToReportThisQuarter,
                            ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed,
                            BuilderQuaterAdminReportId = quateradminreportid,
                            SubmitDate = DateTime.Now.Date,
                        };
                        Container.Add(BuilderQuaterContractStatus);
                    }
                }

                if (Container.Count() > 0)
                {
                    _ObjBuilderQuaterContractStatusService.AddBuilderQuarterContractStatus(Container);
                }

                var ObjBuilderQuarterContractStatusList = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderQuater(BuilderId, QuaterId);
                if (ObjBuilderQuarterContractStatusList != null && ObjBuilderQuarterContractStatusList.Count() > 0)
                {
                    foreach (var Item in ObjBuilderQuarterContractStatusList)
                    {
                        SaveProjectsOnNTRTQ(Item.QuaterId, Item.BuilderId, Item.ContractId, Item.BuilderQuaterAdminReportId, Item.BuilderQuarterContractStatusId);
                    }
                }

                //----------- Send Mail To Notify admin about NTRTQ button click -------------
                //SendNotifyNTRTQMail(BuilderId);
                //----------------------------------------------------------------------------

                ChangeQuaterAdminReport();

                return Json(new { Success = true, DataMessage = "Project(s) For All Contract(s) Are Updated Successfully As 'Nothing To Report This Quarter'." });
            }
            else
            {
                return Json(new { Success = false, DataMessage = "" });
            }
        }

        public JsonResult NoQuarterAdminReport()
        {
            return Json(new { Success = false, DataMessage = "" });
        }

        private void SendNotifyNTRTQMail(Int64 BuilderId)
        {
            SendEmail obj = new SendEmail();
            IEmailSendApi _ObjEmail = new EmailSendApi();

            string Subject = BuilderId.ToString() + " clicked NTRTQ!";
            string Body = String.Concat("Builder ", BuilderId, " clicked NTRTQ");

            bool Status = _ObjEmail.Send(Subject, Body, "abasu@medullus.com", "abasu@medullus.com");
            Status = _ObjEmail.Send(Subject, Body, "pdas@medullus.com", "abasu@medullus.com");
            Status = _ObjEmail.Send(Subject, Body, "cwagner@medullus.com", "abasu@medullus.com");
            Status = _ObjEmail.Send(Subject, Body, "april@cbusa.us", "abasu@medullus.com");
        }

        private void InsertQuaterAdminReportIfNotExists(Int64 qauterid, Int64 BuilderId, ref Int64 BuilderQuaterAdminReportId)
        {
            var resultset = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, qauterid);
            if (resultset.Count() == 0)
            {
                BuilderQuaterAdminReport objReport = new BuilderQuaterAdminReport();
                objReport.BuilderId = BuilderId;
                objReport.QuaterId = qauterid;
                objReport.SubmitDate = DateTime.Now.Date;
                objReport.IsSubmit = false;
                objReport.RowGUID = Guid.NewGuid();

                _ObjQuaterAdminReportService.SaveBuilderQaterReport(objReport, false);
                BuilderQuaterAdminReportId = objReport.BuilderQuaterAdminReportId;
            }
            else
            {
                BuilderQuaterAdminReportId = resultset.FirstOrDefault().BuilderQuaterAdminReportId;
            }
        }
        public ActionResult ChangeQuaterAdminReport()
        {
            bool Status = false;
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            if (claims != null && claims.Count() > 1)
            {
                string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
                Int64 BuilderId = Convert.ToInt64(bldid);

                DateTime currentdate = DateTime.Now.Date;
                var QuaterRecord = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
                var QuaterName = QuaterRecord.QuaterName;

                var Year = QuaterRecord.Year;

                Int64 QuaterId = QuaterRecord.QuaterId;
                BuilderQuaterAdminReport objreport = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, QuaterId).FirstOrDefault();
                if (objreport != null)
                {
                    objreport.IsSubmit = true;
                    objreport.SubmitDate = DateTime.Now.Date;
                    _ObjQuaterAdminReportService.UpdateBuilderAdminQuaterReport(objreport);
                }
                List<BuilderQuarterContractStatus> ObjBuilderQuarterContractStatusList = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderQuater(BuilderId, QuaterId).ToList();
                var AllprojectOfQuater = _ObjQuaterContractProjectReport.CheckAllContractReportSubmit(BuilderId, QuaterId);
                if (AllprojectOfQuater.Count() > 0)
                {
                    foreach (var ProjItem in AllprojectOfQuater)
                    {
                        ProjItem.IsComplete = true;
                        ProjItem.CompleteDate = DateTime.Now.Date;
                        _ObjQuaterContractProjectReport.UpdateMultipleProjectReport(ProjItem);
                    }
                }
                if (ObjBuilderQuarterContractStatusList.Count() > 0)
                {
                    foreach (var item in ObjBuilderQuarterContractStatusList)
                    {

                        item.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed;

                        _ObjBuilderQuaterContractStatusService.UpdateBuilderQuarterContractStatus(item);
                    }
                }
                Status = true;
            }
            else
            {
                Status = false;
            }
            return Json(Status, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Builder")]
        public JsonResult SaveProjectStatus(string[][] array)
        {
            if (array != null)
            {
                var ContractId = array[0][0];
                var BuilderId = array[0][1];
                var QuaterId = array[0][2];

                Int64 BuilderQuaterReportId = 0;
                Int64 BuilderQuaterContractStatusId = 0;
                var BuilderQuaterReport = _ObjQuaterAdminReportService.GetBuilderQuaterReport(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId)).FirstOrDefault();
                if (BuilderQuaterReport != null)
                {
                    BuilderQuaterReportId = BuilderQuaterReport.BuilderQuaterAdminReportId;
                }
                else
                {
                    return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Error occurs due to server" }) }, JsonRequestBehavior.AllowGet);
                }
                var BuilderQuaterContractStatus = _ObjBuilderQuaterContractStatusService.CheckExistContractAgainstBuilderQuater(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId), Convert.ToInt64(ContractId)).FirstOrDefault();
                if (BuilderQuaterContractStatus != null)
                {
                    BuilderQuaterContractStatusId = BuilderQuaterContractStatus.BuilderQuarterContractStatusId;
                    BuilderQuaterContractStatus.QuarterContractStatusId = (Int64)QuarterContractStatusEnum.ReportForThisQuarter;
                    _ObjBuilderQuaterContractStatusService.UpdateBuilderQuarterContractStatus(BuilderQuaterContractStatus);
                }
                else
                {
                    BuilderQuarterContractStatus Obj = new BuilderQuarterContractStatus()
                    {
                        BuilderId = Convert.ToInt64(BuilderId),
                        ContractId = Convert.ToInt64(ContractId),
                        QuaterId = Convert.ToInt64(QuaterId),
                        QuarterContractStatusId = (Int64)QuarterContractStatusEnum.ReportForThisQuarter,
                        ProjectReportStatusId = (Int64)ProjectReportStatusEnum.InProgress,
                        BuilderQuaterAdminReportId = BuilderQuaterReportId,
                        SubmitDate = DateTime.Now.Date,

                    };
                    _ObjBuilderQuaterContractStatusService.AddBuilderQuarterContractStatus(Obj);
                    BuilderQuaterContractStatusId = Obj.BuilderQuarterContractStatusId;
                    // return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Error occurs due to server" }) }, JsonRequestBehavior.AllowGet);
                }
                List<BuilderQuaterContractProjectReport> Container = new List<BuilderQuaterContractProjectReport>();
                if (BuilderQuaterContractStatusId <= 0)
                {
                    string SQLQuery = "";
                    SQLQuery = " select b.* ";
                    SQLQuery += " from BuilderQuarterContractStatus b ";
                    SQLQuery += " where b.builderid = " + BuilderId;
                    SQLQuery += "  and b.quaterid = " + QuaterId;
                    SQLQuery += "  and b.contractid =" + ContractId;
                    SQLQuery += "  and b.BuilderQuaterAdminReportId = " + BuilderQuaterReportId;
                    SQLQuery += "  and b.RowStatusId = 1 ";
                    var TempData = _ObjProjectService.GetDataIntoListQuery(SQLQuery);
                    if (TempData != null && TempData.Count() > 0)
                    {
                        foreach (var item in TempData)
                        {
                            BuilderQuaterContractStatusId = Convert.ToInt64(item[0]);
                        }
                    }
                }
                foreach (var arr in array)
                {
                    Project ObjProject = new Project();

                    BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
                    objProjectReport.ProjectId = Convert.ToInt64(arr[3]);
                    objProjectReport.BuilderId = Convert.ToInt64(arr[1]);
                    objProjectReport.QuaterId = Convert.ToInt64(arr[2]);
                    objProjectReport.ContractId = Convert.ToInt64(arr[0]);
                    objProjectReport.ProjectStatusId = Convert.ToInt64(arr[4]);
                    objProjectReport.IsComplete = Convert.ToInt64(arr[4]) == 1 ? false : true;
                    objProjectReport.CreatedOn = DateTime.Now;
                    objProjectReport.ModifiedOn = DateTime.Now;
                    objProjectReport.CreatedBy = 1;
                    objProjectReport.ModifiedBy = 1;
                    objProjectReport.CompleteDate = DateTime.Now;
                    objProjectReport.RowGUID = Guid.NewGuid();
                    objProjectReport.BuilderQuarterContractStatusId = BuilderQuaterContractStatusId;
                    objProjectReport.BuilderQuaterAdminReportId = BuilderQuaterReportId;
                    Container.Add(objProjectReport);
                }
                _ObjQuaterContractProjectReport.SaveBuilderProjectStatus(Container);
                _ObjProjectService.CheckProjectReportStatus(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId), Convert.ToInt64(ContractId));
                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { IsSuccess = false, ModelError = "Please select a project which you like to report for this contract" }, JsonRequestBehavior.AllowGet);
        }
        private void AssociateProjectToContract(Int64 BuilderId, Int64? ContractId, Int64 LastProjectId, bool MarkProjectsAsNTRTQ)
        {
            Int64 QuaterId = 0;
            DateTime currentdate = DateTime.Now.Date;
            QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;

            //
            Int64 quateradminreportid = 0;
            InsertQuaterAdminReportIfNotExists(QuaterId, BuilderId, ref quateradminreportid);

            bool IsReportAlreadySubmitted = _ObjQuaterAdminReportService.IsReportAllreadySubmited(BuilderId, QuaterId);

            if (IsReportAlreadySubmitted)
            {
                MarkProjectsAsNTRTQ = true;
            }

            if (quateradminreportid > 0)
            {
                IEnumerable<Project> ProjectList = null;
                ProjectList = _ObjProjectService.ProjectList().Where(x => x.RowStatusId == (int)RowActiveStatus.Active && x.BuilderId == BuilderId && x.ProjectId > LastProjectId);
                Int64 BuilderQuaterContractStatusId = 0;
                //var ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContractName)
                //    .Select(x => new { ContractId = x.Contract.ContractId, ContractName = x.Contract.ContractName }).ToList();           
                IEnumerable<Contract> ContractList = null;
                if (ContractId > 0)
                {
                    var NeverReportForThisContractList = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.QuarterContractStatusId == (Int64)QuarterContractStatusEnum.NeverReportForThisContract).Select(s => s.ContractId);
                    ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).Where(w => !NeverReportForThisContractList.Contains(w.ContractId));       // && w.ContractId != ContractId

                    //===== Neyaz on 04-Dec-2017====== VSO#9519
                    //ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReporting(BuilderId).ToList().Where(x => x.ContractId != ContractId);
                    //ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).ToList().Where(x => x.ContractId != ContractId);

                }
                else
                {
                    //===== Neyaz on 04-Dec-2017====== VSO#9519
                    //ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReporting(BuilderId).ToList();
                    var NeverReportForThisContractList = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.QuarterContractStatusId == (Int64)QuarterContractStatusEnum.NeverReportForThisContract).Select(s => s.ContractId);
                    //================ Following line changed on 27.04.2018 by Apala ==============
                    //ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReporting(BuilderId).Where(w => !NeverReportForThisContractList.Contains(w.ContractId));
                    ContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).Where(w => !NeverReportForThisContractList.Contains(w.ContractId));

                }
                if (ContractList != null && ContractList.Count() > 0)
                {
                    var BuilderQuaterContractStatusList = _ObjBuilderQuaterContractStatusService.CheckExistContractAgainstBuilderQuater(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId), null);
                    var ThisQuarterProjectReportList = GetCountAllContractAgainstBuilderQuaterOnly(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId));
                    foreach (var Contract in ContractList)
                    {
                        // changed on 12-april-2018 - angshuman
                        var BuilderQuaterContractStatus = BuilderQuaterContractStatusList.Where(x => x.BuilderId == Convert.ToInt64(BuilderId) && x.QuaterId == Convert.ToInt64(QuaterId) && x.ContractId == Convert.ToInt64(Contract.ContractId)).FirstOrDefault();
                        if (BuilderQuaterContractStatus != null)
                        {
                            //******************** Changed by Apala for VSO#15628 ***********************
                            if (BuilderQuaterContractStatus.ProjectReportStatusId == (long)ProjectReportStatusEnum.Completed)
                            {
                                BuilderQuaterContractStatusId = BuilderQuaterContractStatus.BuilderQuarterContractStatusId;

                                foreach (var item in ProjectList)
                                {
                                    BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
                                    Int64 ProjectId = item.ProjectId;
                                    objProjectReport.BuilderQuaterAdminReportId = quateradminreportid;
                                    // BuilderQuaterContractStatus.BuilderQuarterContractStatusId;
                                    objProjectReport.CompleteDate = DateTime.Now.Date;
                                    int count = ThisQuarterProjectReportList.Where(x => x.ProjectId == ProjectId).Count();  //getCountAllContractAgainstBuilderQuater(BuilderId, QuaterId, Contract.ContractId, ProjectId);
                                    if (count == 0)
                                    {
                                        if (BuilderQuaterContractStatusId <= 0)
                                        {
                                            string SQLQuery = "";
                                            SQLQuery = " select b.* ";
                                            SQLQuery += " from BuilderQuarterContractStatus b ";
                                            SQLQuery += " where b.builderid = " + BuilderId;
                                            SQLQuery += "  and b.quaterid = " + QuaterId;
                                            SQLQuery += "  and b.contractid =" + Contract.ContractId;
                                            SQLQuery += "  and b.BuilderQuaterAdminReportId = " + quateradminreportid;
                                            SQLQuery += "  and b.RowStatusId = 1 ";
                                            var TempData = _ObjProjectService.GetDataIntoListQuery(SQLQuery);
                                            if (TempData != null && TempData.Count() > 0)
                                            {
                                                foreach (var data in TempData)
                                                {
                                                    BuilderQuaterContractStatusId = Convert.ToInt64(data[0]);
                                                }
                                            }
                                        }

                                        objProjectReport.ProjectId = item.ProjectId;
                                        objProjectReport.BuilderId = BuilderId;
                                        objProjectReport.QuaterId = QuaterId;
                                        objProjectReport.ContractId = Contract.ContractId;
                                        objProjectReport.ProjectStatusId = 2; // Nothing to report this quarter                                                        
                                        objProjectReport.IsComplete = true;
                                        objProjectReport.BuilderQuarterContractStatusId = BuilderQuaterContractStatusId;
                                        objProjectReport.RowGUID = Guid.NewGuid();

                                        if (MarkProjectsAsNTRTQ)
                                        {
                                            _ObjQuaterContractProjectReport.SaveProjectReport(objProjectReport, false);
                                        }
                                    }
                                }
                                // check newly created project if not reported then update BuilderQuarterContractStatus with ProjectReportStausId as in progress / not completed.-angshuman on 12-apr-2018.
                                _ObjProjectService.CheckProjectReportStatus(BuilderId, QuaterId, Contract.ContractId);

                                if (!MarkProjectsAsNTRTQ)
                                {
                                    BuilderQuaterContractStatus.QuarterContractStatusId = (long)QuarterContractStatusEnum.ReportForThisQuarter;
                                    BuilderQuaterContractStatus.ProjectReportStatusId = (long)ProjectReportStatusEnum.InProgress;
                                    _ObjBuilderQuaterContractStatusService.UpdateBuilderQuarterContractStatus(BuilderQuaterContractStatus);
                                }
                            }
                            //****************************************************************************
                        }
                        else
                        {
                            //********************** FOLLOWING CODE COMMENTED OUT FOR VSO#15628 *****************************
                            //BuilderQuarterContractStatus BuilderQuaterContractStatusToSave = new BuilderQuarterContractStatus()
                            //{
                            //    BuilderId = BuilderId,
                            //    ContractId = Contract.ContractId,
                            //    QuaterId = QuaterId,
                            //    QuarterContractStatusId = (Int64)QuarterContractStatusEnum.ReportForThisQuarter,
                            //    ProjectReportStatusId = (Int64)ProjectReportStatusEnum.InProgress,
                            //    BuilderQuaterAdminReportId = quateradminreportid,
                            //    SubmitDate = DateTime.Now.Date,
                            //    RowStatusId = (int)RowActiveStatus.Active,
                            //};
                            //BuilderQuaterContractStatusId = 0;
                            //_ObjBuilderQuaterContractStatusService.AddBuilderQuarterContractStatus(BuilderQuaterContractStatusToSave);
                            //**********************************************************************************************************
                        }
                    }
                }
                _ObjQuaterContractProjectReport.ForceDisposeDbContext();
            }
        }
        //public void CheckProjectReportStatus(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        //{

        //    var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();

        //    int ProjListCount = _ObjProjectService.GetBuilderProject(ContractId, BuilderId, PreviousQuaterList).Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new { ProjectId = x.ProjectId, ProjectName = x.ProjectName }).Count();



        //    int QuaterContractProjectReportCount = _ObjQuaterContractProjectReport.CheckCompleteBuilderQuaterContractProjectReport(BuilderId, QuaterId,ContractId).Count();

        //    if (ProjListCount== QuaterContractProjectReportCount)
        //    {
        //        var BuilderQuaterContractStatus =  _ObjBuilderQuaterContractStatusService.CheckExistContractAgainstBuilderQuater(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId), Convert.ToInt64(ContractId)).FirstOrDefault();
        //        if (BuilderQuaterContractStatus != null)
        //        {
        //            BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.Completed;
        //            BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
        //            _ObjBuilderQuaterContractStatusService.UpdateBuilderQuarterContractStatus(BuilderQuaterContractStatus);
        //        }
        //    }
        //    else
        //    {
        //        var BuilderQuaterContractStatus = _ObjBuilderQuaterContractStatusService.CheckExistContractAgainstBuilderQuater(Convert.ToInt64(BuilderId), Convert.ToInt64(QuaterId), Convert.ToInt64(ContractId)).FirstOrDefault();

        //        if (BuilderQuaterContractStatus != null)
        //        {
        //            BuilderQuaterContractStatus.ProjectReportStatusId = (Int64)ProjectReportStatusEnum.InProgress;
        //            BuilderQuaterContractStatus.SubmitDate = DateTime.Now.Date;
        //            _ObjBuilderQuaterContractStatusService.UpdateBuilderQuarterContractStatus(BuilderQuaterContractStatus);
        //        }
        //    }

        //    // return new EmptyResult();
        //}
        #endregion
        /* Add */

        #region Add Project Status
        public ActionResult FillProjectDropDown(Int64 BuilderId, Int64 ContractId)
        {
            Int64 conid = 0;
            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();

            var ProjectList = _ObjProjectService.GetBuilderProject(ContractId, BuilderId, PreviousQuaterList).Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new { ProjectId = x.ProjectId, ProjectName = x.ProjectName }).ToList();
            ProjectList.Insert(0, new { ProjectId = conid, ProjectName = "Select Project" });
            return Json(ProjectList, JsonRequestBehavior.AllowGet);
        }

        //[Authorize(Roles = "Builder")]
        //public ActionResult listViewActiveProject_read([DataSourceRequest] DataSourceRequest request, Int64 BuilderId, Int64 ContractId, Int64 FilterProjectId, string SortType)
        //{     
        //    var ProjectStatuslist = _ObjProjectStatusService.GetProjectStatus();
        //    DateTime currentdate = DateTime.Now.Date;
        //    Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
        //    var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();
        //    var AvailableProject = _ObjProjectService.GetContractBuilderCurrentQuaterProject(ContractId, BuilderId, PreviousQuaterList, FilterProjectId);
        //    var CurrentQuaterSelectedProject = _ObjQuaterContractProjectReport.GetBuilderCurrentQuaterSelectedProject(BuilderId, ContractId, QuaterId);
        //    int ProjectCounter = 1;

        //    if (SortType == "ASC")
        //    {
        //        var ProjectStatusListAsc = AvailableProject.GroupJoin(CurrentQuaterSelectedProject, x => x.ProjectId, y => y.ProjectId,
        //        (x, y) => new { AllAvailProject = x, SaveProject = y })
        //        .SelectMany(x => x.SaveProject.DefaultIfEmpty(), (x, y) => new 
        //        {
        //            ProjectId = x.AllAvailProject.ProjectId,
        //            ProjectName = x.AllAvailProject.ProjectName + " ," + x.AllAvailProject.LotNo + " ," + x.AllAvailProject.State + " ," + x.AllAvailProject.Address,
        //            ProjectStatus = x.SaveProject != null ? (x.SaveProject.FirstOrDefault() != null ? x.SaveProject.FirstOrDefault().ProjectStatusId : 0) : 0                   
        //        }).OrderBy(x => x.ProjectName);

        //        var newList = ProjectStatusListAsc.ToList()
        //            .Select(x => new
        //            {
        //                ProjectId = x.ProjectId,
        //                ProjectName = x.ProjectName,
        //                ProjectStatus= x.ProjectStatus,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(newList.ToDataSourceResult(request));
        //    }
        //    else
        //    {
        //        var ProjectStatusList = AvailableProject.GroupJoin(CurrentQuaterSelectedProject, x => x.ProjectId, y => y.ProjectId,
        //       (x, y) => new { AllAvailProject = x, SaveProject = y })
        //       .SelectMany(x => x.SaveProject.DefaultIfEmpty(), (x, y) => new
        //       {
        //           ProjectId = x.AllAvailProject.ProjectId,
        //           ProjectName = x.AllAvailProject.ProjectName + " ," + x.AllAvailProject.LotNo + " ," + x.AllAvailProject.State + " ," + x.AllAvailProject.Address,
        //           ProjectStatus = x.SaveProject != null ? (x.SaveProject.FirstOrDefault() != null ? x.SaveProject.FirstOrDefault().ProjectStatusId : 0) : 0,                   
        //       }).OrderByDescending(x => x.ProjectName);

        //        var newList = ProjectStatusList.ToList()
        //            .Select(x => new
        //            {
        //                ProjectId = x.ProjectId,
        //                ProjectName = x.ProjectName,
        //                ProjectStatus = x.ProjectStatus,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(newList.ToDataSourceResult(request));
        //    }

        //}

        [Authorize(Roles = "Builder")]
        public ActionResult listViewActiveProject_read([DataSourceRequest] DataSourceRequest request, Int64 BuilderId, Int64 ContractId, Int64 FilterProjectId, string SortType, List<AddProjectStatusViewModel> ObjPrevProjList)
        {
            var ProjectStatuslist = _ObjProjectStatusService.GetProjectStatus();
            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();
            var AvailableProject = _ObjProjectService.GetContractBuilderCurrentQuaterProject(ContractId, BuilderId, PreviousQuaterList, FilterProjectId);
            var CurrentQuaterSelectedProject = _ObjQuaterContractProjectReport.GetBuilderCurrentQuaterSelectedProject(BuilderId, ContractId, QuaterId);
            int ProjectCounter = 1;
            List<Int64> PreveProject = new List<long>();

            if (ObjPrevProjList != null)
            {
                if (ObjPrevProjList.Count > 0)
                {
                    foreach (var Item in ObjPrevProjList)
                    {
                        if (Item != null)
                            PreveProject.Add(Item.ProjectId);
                    }
                }
            }
            var ReportedProject = _ObjProjectService.GetSelectedProjectbyProjectList(PreveProject);

            var PrevProjList = ReportedProject.Select(x => new
            {
                ProjectId = x.ProjectId,
                ProjectName = x.ProjectName + " ," + x.LotNo + " ," + x.State + " ," + x.Address,
                ProjectStatus = Convert.ToInt64(0)
            }).OrderBy(x => x.ProjectName);

            if (SortType == "ASC")
            {
                var ProjectStatusListAsc = AvailableProject.GroupJoin(CurrentQuaterSelectedProject, x => x.ProjectId, y => y.ProjectId,
                (x, y) => new { AllAvailProject = x, SaveProject = y })
                .SelectMany(x => x.SaveProject.DefaultIfEmpty(), (x, y) => new
                {
                    ProjectId = x.AllAvailProject.ProjectId,
                    ProjectName = x.AllAvailProject.ProjectName + " ," + x.AllAvailProject.LotNo + " ," + x.AllAvailProject.State + " ," + x.AllAvailProject.Address,
                    ProjectStatus = x.SaveProject != null ? (x.SaveProject.FirstOrDefault() != null ? x.SaveProject.FirstOrDefault().ProjectStatusId : 0) : 0
                }).OrderBy(x => x.ProjectName);

                var newList1 = ProjectStatusListAsc.ToList()
                         .Select(x => new
                         {
                             ProjectId = x.ProjectId,
                             ProjectName = x.ProjectName,
                             ProjectStatus = x.ProjectStatus,
                         }).ToList();

                var newList2 = newList1.Concat(PrevProjList);

                var newList3 = newList2.ToList()
                   .Select(x => new
                   {
                       ProjectId = x.ProjectId,
                       ProjectName = x.ProjectName,
                       ProjectStatus = x.ProjectStatus,
                       rowcount = ProjectCounter++
                   }).ToList();

                return Json(newList3.ToDataSourceResult(request));
            }
            else
            {
                var ProjectStatusList = AvailableProject.GroupJoin(CurrentQuaterSelectedProject, x => x.ProjectId, y => y.ProjectId,
               (x, y) => new { AllAvailProject = x, SaveProject = y })
               .SelectMany(x => x.SaveProject.DefaultIfEmpty(), (x, y) => new
               {
                   ProjectId = x.AllAvailProject.ProjectId,
                   ProjectName = x.AllAvailProject.ProjectName + " ," + x.AllAvailProject.LotNo + " ," + x.AllAvailProject.State + " ," + x.AllAvailProject.Address,
                   ProjectStatus = x.SaveProject != null ? (x.SaveProject.FirstOrDefault() != null ? x.SaveProject.FirstOrDefault().ProjectStatusId : 0) : 0,
               }).OrderByDescending(x => x.ProjectName);

                var newList1 = ProjectStatusList.ToList()
                         .Select(x => new
                         {
                             ProjectId = x.ProjectId,
                             ProjectName = x.ProjectName,
                             ProjectStatus = x.ProjectStatus,
                         }).ToList();

                var newList2 = newList1.Concat(PrevProjList);

                var newList3 = newList2.ToList()
                   .Select(x => new
                   {
                       ProjectId = x.ProjectId,
                       ProjectName = x.ProjectName,
                       ProjectStatus = x.ProjectStatus,
                       rowcount = ProjectCounter++
                   }).ToList();

                return Json(newList3.ToDataSourceResult(request));
            }

        }



        //[Authorize(Roles = "Builder")]
        //public ActionResult SaveProjectStatus(List<AddProjectStatusViewModel> ObjVm)
        //{

        //    if (ObjVm != null)
        //    {
        //        if (ObjVm.Count > 0)
        //        {
        //            var ContractId = ObjVm.FirstOrDefault().ContractId;
        //            var BuilderId = ObjVm.FirstOrDefault().BuilderId;
        //            var QuaterId = ObjVm.FirstOrDefault().QuaterId;
        //            Int64 BuilderQuaterReportId = 0;
        //            var BuilderQuaterReport = _ObjQuaterAdminReportService.GetBuilderQuaterReport(BuilderId, QuaterId).FirstOrDefault();
        //            if (BuilderQuaterReport != null)
        //            {
        //                BuilderQuaterReportId = BuilderQuaterReport.BuilderQuaterAdminReportId;
        //            }
        //            else
        //            {
        //                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(new string[] { "Error occurs due to server" }) }, JsonRequestBehavior.AllowGet);
        //            }
        //            List<BuilderQuaterContractProjectReport> Container = new List<BuilderQuaterContractProjectReport>();
        //            foreach (var Item in ObjVm)
        //            {

        //                BuilderQuaterContractProjectReport objProjectReport = new BuilderQuaterContractProjectReport();
        //                objProjectReport.ProjectId = Item.ProjectId;
        //                objProjectReport.BuilderId = BuilderId;
        //                objProjectReport.QuaterId = QuaterId;
        //                objProjectReport.ContractId = ContractId;
        //                objProjectReport.ProjectStatusId = Item.ProjectStatusId;
        //                objProjectReport.IsComplete = Item.ProjectStatusId == 1 ? false : true;
        //                objProjectReport.CreatedOn = DateTime.Now;
        //                objProjectReport.ModifiedOn = DateTime.Now;
        //                objProjectReport.CreatedBy = 1;
        //                objProjectReport.ModifiedBy = 1;
        //                objProjectReport.CompleteDate = DateTime.Now;
        //                objProjectReport.RowGUID = Guid.NewGuid();

        //                objProjectReport.BuilderQuaterAdminReportId = BuilderQuaterReportId;

        //                Container.Add(objProjectReport);

        //            }

        //            _ObjQuaterContractProjectReport.SaveBuilderProjectStatus(Container);

        //            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    return Json(new { IsSuccess = false, ModelError = "Please select a project which you like to report for this contract" }, JsonRequestBehavior.AllowGet);
        //}

        [Authorize(Roles = "Builder")]
        public ActionResult LoadPrevRprPrj(Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            AddProjectStatusViewModel ObjVm = new AddProjectStatusViewModel();
            ObjVm.BuilderId = BuilderId;
            ObjVm.ContractId = ContractId;

            return PartialView("_SelectPrevRptPrj", ObjVm);

        }

        [Authorize(Roles = "Builder")]
        public ActionResult listViewPrevRptProject_read([DataSourceRequest] DataSourceRequest request, Int64 BuilderId, Int64 ContractId, string SortType)
        {
            var ProjectStatuslist = _ObjProjectStatusService.GetProjectStatus();
            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();
            var PrevReportedProject = _ObjProjectService.GetContractBuilderPriviousQuaterProject(ContractId, BuilderId, PreviousQuaterList);
            int ProjectCounter = 1;
            if (SortType == "ASC")
            {
                var ProjectStatusListAsc = PrevReportedProject.Select(x => new
                {
                    ProjectId = x.ProjectId,
                    ProjectName = x.ProjectName + " ," + x.LotNo + " ," + x.State + " ," + x.Address,
                    ProjectStatus = 0,
                    BuilderId = x.BuilderId,
                    QuaterId = QuaterId
                }).OrderBy(x => x.ProjectName);

                var newList = ProjectStatusListAsc.ToList()
                    .Select(x => new
                    {
                        ProjectId = x.ProjectId,
                        ProjectName = x.ProjectName,
                        ProjectStatus = x.ProjectStatus,
                        BuilderId = x.BuilderId,
                        QuaterId = x.QuaterId,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else
            {
                var ProjectStatusList = PrevReportedProject.Select(x => new
                {
                    ProjectId = x.ProjectId,
                    ProjectName = x.ProjectName + " ," + x.LotNo + " ," + x.State + " ," + x.Address,
                    ProjectStatus = 0,
                    BuilderId = x.BuilderId,
                    QuaterId = QuaterId,
                }).OrderByDescending(x => x.ProjectName);

                var newList = ProjectStatusList.ToList()
                    .Select(x => new
                    {
                        ProjectId = x.ProjectId,
                        ProjectName = x.ProjectName,
                        ProjectStatus = x.ProjectStatus,
                        BuilderId = x.BuilderId,
                        QuaterId = x.QuaterId,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
        }

        #endregion
        #region Created By Rajendar On 3/15/2018
        private void SaveProjectsOnNTRTQ(Int64 QuaterId, Int64 BuilderId, Int64 ContractId, Int64 quateradminreportid, Int64 BuilderQuarterContractStatusId)
        {
            IEnumerable<Quater> PreviousQuaterList = null;

            if (QuaterId != 0)
            {
                PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId);
            }
            var ProjectList = _ObjProjectService.GetBuilderProject(ContractId, BuilderId, PreviousQuaterList.Select(x => x.QuaterId).ToList()).Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new { ProjectId = x.ProjectId, ProjectName = x.ProjectName });
            var ProjectListReported = _ObjQuaterContractProjectReport.GetAllProjectCountForQuater(ContractId, BuilderId, QuaterId);
            if (ProjectList != null && ProjectList.Count() > 0)
            {
                List<BuilderQuaterContractProjectReport> ObjProjectReportList = new List<BuilderQuaterContractProjectReport>();
                foreach (var item in ProjectList)
                {

                    BuilderQuaterContractProjectReport objProjectReport = ProjectListReported.Where(w => w.ProjectId == item.ProjectId).FirstOrDefault();
                    if (objProjectReport != null)
                    {
                        objProjectReport.ProjectStatusId = 2;
                        objProjectReport.IsComplete = true;
                        _ObjQuaterContractProjectReport.UpdateProjectReport(objProjectReport);
                    }
                    else
                    {
                        objProjectReport = new BuilderQuaterContractProjectReport();
                        Int64 ProjectId = item.ProjectId;
                        objProjectReport.BuilderQuaterAdminReportId = quateradminreportid;
                        objProjectReport.BuilderQuarterContractStatusId = BuilderQuarterContractStatusId;
                        objProjectReport.CompleteDate = DateTime.Now.Date;
                        objProjectReport.ProjectId = item.ProjectId;
                        objProjectReport.BuilderId = BuilderId;
                        objProjectReport.QuaterId = QuaterId;
                        objProjectReport.ContractId = ContractId;
                        objProjectReport.ProjectStatusId = 2; // Nothing to report this quarter                                                        
                        objProjectReport.IsComplete = true;
                        objProjectReport.RowGUID = Guid.NewGuid();
                        ObjProjectReportList.Add(objProjectReport);


                    }

                }
                if (ObjProjectReportList.Count() > 0)
                {
                    _ObjQuaterContractProjectReport.SaveProjectReportInBulk(ObjProjectReportList);
                }
            }

        }

        #endregion

    }
}