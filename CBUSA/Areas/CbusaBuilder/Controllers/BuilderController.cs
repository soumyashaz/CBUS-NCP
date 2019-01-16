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

namespace CBUSA.Areas.CbusaBuilder.Controllers
{
    public class BuilderController : Controller
    {
        readonly IContractStatusService _ObjContractStatusServices;
        readonly IResourceCategoryService _ObjResourceCategoryService;
        readonly IResourceService _ObjResourceService;
        readonly IManufacturerService _ObjManufactureService;
        readonly IContractServices _ObjContractService;
        readonly IContractRebateService _ObjContractRebateService;
        readonly IContractBuilderService _ObjContractBuilderService;
        readonly ISurveyService _ObjSurveyService;
        readonly ISurveyBuilderService _ObjSurveyBuilderService;
        readonly IQuaterService _ObjQuaterService;
        readonly IBuilderQuaterAdminReportService _ObjQuaterAdminReportService;
        readonly IBuilderQuaterContractProjectReportService _ObjQuaterContractProjectReport;
        readonly IContractComplianceService _ObjContractCompliance;
        readonly IBuilderService _ObjBuilderService;
        readonly IBuilderQuarterContractStatusService _ObjBuilderQuaterContractStatusService;
        readonly IBuilderQuaterContractProjectDetailsService _ObjBuilderQuaterContractProjectDetails;
        readonly IProjectService _ObjProjectService;
        readonly IQuestionService _ObjQuestionService;

        public BuilderController(IContractStatusService ObjContractStatusServices, IResourceCategoryService ObjResourceCategoryService,
            IResourceService ObjResourceService, IManufacturerService ObjManufactureService,
            IContractServices ObjContractService, IContractRebateService ObjContractRebateService, IContractBuilderService ObjContractBuilderService,
            ISurveyService ObjSurveyService, ISurveyBuilderService ObjSurveyBuilderService, IQuaterService ObjQuaterService,
            IBuilderQuaterContractProjectReportService ObjQuaterContractProjectReport, IBuilderQuaterAdminReportService ObjQuaterAdminReportService,
            IContractComplianceService ObjContractCompliance,
            IBuilderQuaterContractProjectDetailsService ObjBuilderQuaterContractProjectDetails, IProjectService ObjProjectService, IBuilderService ObjBuilderService, IQuestionService ObjQuestionService
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
            _ObjQuaterService = ObjQuaterService;
            _ObjQuaterContractProjectReport = ObjQuaterContractProjectReport;
            _ObjQuaterAdminReportService = ObjQuaterAdminReportService;
            _ObjContractCompliance = ObjContractCompliance;
            _ObjBuilderQuaterContractProjectDetails = ObjBuilderQuaterContractProjectDetails;
            _ObjProjectService = ObjProjectService;
            _ObjBuilderService = ObjBuilderService;
            _ObjQuestionService = ObjQuestionService;
            _ObjBuilderQuaterContractStatusService = ObjBuilderQuaterContractStatusService;
        }

        // GET: CbusaBuilder/Builder
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Builder")]
        public ActionResult ContractMayJoin()
        {
            return View();
        }

        [Authorize(Roles = "Builder")]
        public ActionResult PendingContractMayJoin()
        {

            return View();
        }

        [Authorize(Roles = "Builder")]
        public ActionResult PendingContractListMayJoin([DataSourceRequest] DataSourceRequest request, string Type, int? PageValue)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            IEnumerable<Contract> ContractList = null;
            IEnumerable<Contract> DeclinedContractList = null;

            IEnumerable<ResourceCategory> CatList = null;
            string ContractTypeFlag = "pen";
            var ContractBuilderId = _ObjContractBuilderService.GetAllContractofBuilder(BuilderId).Select(x => x.ContractId);

            IEnumerable<ContractBuilderViewModel> list;
            var resource = _ObjResourceCategoryService.GetResourceCategoryAll().Where(X => X.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.ResourceCategoryId);
            var Id = resource.FirstOrDefault();

            if (Type == null)
            {
                ContractList = _ObjContractService.GetNonAssociateContractWithBuilder(BuilderId, ContractTypeFlag).Distinct();
                DeclinedContractList = _ObjContractBuilderService.GetDeclinedContractsofBuilder(BuilderId, 2);

                list = ContractList.Select(x => new ContractBuilderViewModel
                {
                    ContractId = x.ContractId,
                    BuilderId = BuilderId,
                    ContractName = x.ContractName,
                    Manufactererlist = getManufacterer(x.ContractId),
                    Websiteslist = getwebsitedata(x.ContractId),
                    ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                    ContractFrom = x.EstimatedStartDate != null ? FromDate(x.EstimatedStartDate.ToString()) : "",
                    EntryDeadline = x.EntryDeadline != null ? EntryDate(x.EntryDeadline.ToString()) : "",
                    DayToGo = x.EntryDeadline != null ? DayToGo(x.EntryDeadline.ToString()) : "",
                    ContractTo = x.ContrctTo != null ? FromDate(x.ContrctTo.ToString()) : "",
                    ActionStatus = CheckStatusMayJoin(x.ContractId, BuilderId),
                    SurveyId = CheckSurveyMayJoin(x.ContractId, BuilderId),
                    BuilderJoin = BuilderJoin(x.ContractId),
                    ContractStatus = x.ContractStatus.ContractStatusName,
                    ResourceCategoryId = Convert.ToInt64(Id)
                }).Union(DeclinedContractList.Select(y => new ContractBuilderViewModel
                {
                    ContractId = y.ContractId,
                    BuilderId = BuilderId,
                    ContractName = y.ContractName,
                    Manufactererlist = getManufacterer(y.ContractId),
                    Websiteslist = getwebsitedata(y.ContractId),
                    ProductList = string.Join(",", y.ContractProduct.OrderBy(x => x.Product.ProductName).Select(x => x.Product.ProductName)),
                    ContractFrom = y.EstimatedStartDate != null ? FromDate(y.EstimatedStartDate.ToString()) : "",
                    EntryDeadline = y.EntryDeadline != null ? EntryDate(y.EntryDeadline.ToString()) : "",
                    DayToGo = y.EntryDeadline != null ? DayToGo(y.EntryDeadline.ToString()) : "",
                    ContractTo = y.ContrctTo != null ? FromDate(y.ContrctTo.ToString()) : "",
                    ActionStatus = "DECLINED",
                    SurveyId = CheckSurveyMayJoin(y.ContractId, BuilderId),
                    BuilderJoin = BuilderJoin(y.ContractId),
                    ContractStatus = y.ContractStatus.ContractStatusName,
                    ResourceCategoryId = Convert.ToInt64(Id)
                }));

                int ProjectCounter = 1;
                var newList = list.ToList().Select(x => new ContractBuilderViewModel
                {
                    ContractId = x.ContractId,
                    BuilderId = x.BuilderId,
                    ContractName = x.ContractName,
                    Manufactererlist = x.Manufactererlist,
                    Websiteslist = x.Websiteslist,
                    ProductList = x.ProductList,
                    ContractFrom = x.ContractFrom,
                    EntryDeadline = x.EntryDeadline,
                    DayToGo = x.DayToGo,
                    ContractTo = x.ContractTo,
                    ActionStatus = x.ActionStatus,
                    SurveyId = x.SurveyId,
                    BuilderJoin = x.BuilderJoin,
                    ContractStatus = x.ContractStatus,
                    ResourceCategoryId = x.ResourceCategoryId,
                    rowcount = ProjectCounter++
                }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }

            else
            {
                ContractList = _ObjContractService.GetNonAssociateContractWithBuilder(BuilderId, ContractTypeFlag).Distinct();
                DeclinedContractList = _ObjContractBuilderService.GetDeclinedContractsofBuilder(BuilderId, 2);

                list = ContractList.Select(x => new ContractBuilderViewModel
                {
                    ContractId = x.ContractId,
                    BuilderId = BuilderId,
                    ContractName = x.ContractName,
                    Manufactererlist = getManufacterer(x.ContractId),
                    Websiteslist = getwebsitedata(x.ContractId),
                    ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                    ContractFrom = x.EstimatedStartDate != null ? FromDate(x.EstimatedStartDate.ToString()) : "",
                    EntryDeadline = x.EntryDeadline != null ? EntryDate(x.EntryDeadline.ToString()) : "",
                    DayToGo = x.EntryDeadline != null ? DayToGo(x.EntryDeadline.ToString()) : "",
                    ContractTo = x.ContrctTo != null ? FromDate(x.ContrctTo.ToString()) : "",
                    ActionStatus = CheckStatusMayJoin(x.ContractId, BuilderId),
                    SurveyId = CheckSurveyMayJoin(x.ContractId, BuilderId),
                    BuilderJoin = BuilderJoin(x.ContractId),
                    ContractStatus = x.ContractStatus.ContractStatusName,
                    ResourceCategoryId = Convert.ToInt64(Id)
                }).Union(DeclinedContractList.Select(y => new ContractBuilderViewModel
                {
                    ContractId = y.ContractId,
                    BuilderId = BuilderId,
                    ContractName = y.ContractName,
                    Manufactererlist = getManufacterer(y.ContractId),
                    Websiteslist = getwebsitedata(y.ContractId),
                    ProductList = string.Join(",", y.ContractProduct.OrderBy(x => x.Product.ProductName).Select(x => x.Product.ProductName)),
                    ContractFrom = y.EstimatedStartDate != null ? FromDate(y.EstimatedStartDate.ToString()) : "",
                    EntryDeadline = y.EntryDeadline != null ? EntryDate(y.EntryDeadline.ToString()) : "",
                    DayToGo = y.EntryDeadline != null ? DayToGo(y.EntryDeadline.ToString()) : "",
                    ContractTo = y.ContrctTo != null ? FromDate(y.ContrctTo.ToString()) : "",
                    ActionStatus = "DECLINED",
                    SurveyId = CheckSurveyMayJoin(y.ContractId, BuilderId),
                    BuilderJoin = BuilderJoin(y.ContractId),
                    ContractStatus = y.ContractStatus.ContractStatusName,
                    ResourceCategoryId = Convert.ToInt64(Id)
                }));

                if (Type == "ascpcon")
                {
                    list = list.OrderBy(c => c.ContractName);
                }
                else if (Type == "descpcon")
                {
                    list = list.OrderByDescending(c => c.ContractName);
                }
                else if (Type == "ascpncp")
                {
                    list = list.OrderBy(c => c.Manufactererlist);
                }
                else if (Type == "descpncp")
                {
                    list = list.OrderByDescending(c => c.Manufactererlist);
                }
                else if (Type == "asceed")
                {
                    list = list.OrderBy(c => Convert.ToDateTime(c.EntryDeadline));
                }
                else if (Type == "desceed")
                {
                    list = list.OrderByDescending(c => Convert.ToDateTime(c.EntryDeadline));
                }
                else if (Type == "ascpct")
                {
                    list = list.OrderBy(c => c.ContractStatus);
                }
                else if (Type == "descpct")
                {
                    list = list.OrderByDescending(c => c.ContractStatus);
                }
                else if (Type == "ascpst")
                {
                    list = list.OrderBy(c => Convert.ToDateTime(c.ContractFrom));
                }
                else if (Type == "descpst")
                {
                    list = list.OrderByDescending(c => Convert.ToDateTime(c.ContractFrom));
                }
                else if (Type == "ascbj")
                {
                    list = list.OrderBy(c => Convert.ToInt32(c.BuilderJoin));
                }
                else if (Type == "descbj")
                {
                    list = list.OrderByDescending(c => Convert.ToInt32(c.BuilderJoin));
                }
                else if (Type == "ascpc")
                {
                    list = list.OrderBy(c => c.ProductList);
                }
                else if (Type == "descpc")
                {
                    list = list.OrderByDescending(c => c.ProductList);
                }

                int ProjectCounter = 1;
                var newList = list.ToList().Select(x => new ContractBuilderViewModel
                {
                    ContractId = x.ContractId,
                    BuilderId = x.BuilderId,
                    ContractName = x.ContractName,
                    Manufactererlist = x.Manufactererlist,
                    Websiteslist = x.Websiteslist,
                    ProductList = x.ProductList,
                    ContractFrom = x.ContractFrom,
                    EntryDeadline = x.EntryDeadline,
                    DayToGo = x.DayToGo,
                    ContractTo = x.ContractTo,
                    ActionStatus = x.ActionStatus,
                    SurveyId = x.SurveyId,
                    BuilderJoin = x.BuilderJoin,
                    ContractStatus = x.ContractStatus,
                    ResourceCategoryId = x.ResourceCategoryId,
                    rowcount = ProjectCounter++
                }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            //var value = "";
        }

        public List<string> getwebsitedata(Int64 ContractId)
        {
            string Deliverable = _ObjContractService.GetContract(ContractId).Website;
            string result = "";
            List<string> primes = new List<string>();
            if (Deliverable.Contains(","))
            {
                string[] values = Deliverable.Split(',');
                for (int i = 0; i < values.Length; i++)
                {

                    if (values[i].Contains("http"))
                    {
                        values[i] = values[i].Substring(values[i].LastIndexOf('/') + 1);
                    }
                    primes.Add(values[i]);
                }
                return primes;
            }
            else
            {
                primes.Add(Deliverable);
                return primes;
            }
            //return null;

        }

        public List<string> getManufacterer(Int64 ContractId)
        {
            string Deliverable = _ObjContractService.GetContract(ContractId).PrimaryManufacturer;
            string result = "";
            List<string> primes = new List<string>();

            if (Deliverable == "undefined" || Deliverable == null)
            {
                Deliverable = "";
                return primes;
            }
            if (Deliverable.Contains(","))
            {
                string[] values = Deliverable.Split(',');
                for (int i = 0; i < values.Length; i++)
                {

                    primes.Add(values[i]);

                }
                return primes;
            }
            else
            {
                primes.Add(Deliverable);
                return primes;
            }
            //return null;

        }
        [Authorize(Roles = "Builder")]

        public ActionResult ActiveContractMayJoinList([DataSourceRequest] DataSourceRequest request, string Type, int? PageValue)
        {
            string ContractFlag = "act";
            var resource = _ObjResourceCategoryService.GetResourceCategoryAll().Where(X => X.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.ResourceCategoryId);
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            //int BuilderId = 2;  //putbuilderid here
            var Id = resource.FirstOrDefault();
            var ContractBuilderId = _ObjContractBuilderService.GetAllContractofBuilder(BuilderId).Select(x => x.ContractId);

            IEnumerable<Contract> ContractList = null;
            IEnumerable<Contract> DeclinedContractList = null;

            IEnumerable<ContractBuilderViewModel> list;

            if (Type == null)
            {
                ContractList = _ObjContractService.GetNonAssociateContractWithBuilder(BuilderId, ContractFlag).Distinct();
                DeclinedContractList = _ObjContractBuilderService.GetDeclinedContractsofBuilder(BuilderId, 1).OrderByDescending(x => x.ContractId);

                list = ContractList.Select(x => new ContractBuilderViewModel
                {
                    ContractId = x.ContractId,
                    BuilderId = BuilderId,
                    ContractName = x.ContractName,
                    Website = (x.Website),
                    NationalContractPartner = x.Manufacturer.ManufacturerName,
                    ContractIcon = GetContractIcon(x.ContractIcon),
                    ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                    ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                    ContractTo = x.ContrctTo != null ? FromDate(x.ContrctTo.ToString()) : "",
                    SurveyId = CheckSurveyMayJoin(x.ContractId, BuilderId),
                    ActionStatus = CheckStatusMayJoin(x.ContractId, BuilderId),
                    BuilderJoin = BuilderJoin(x.ContractId),
                    ContractDeliverableslist = getdeliverable(x.ContractId),
                    ResourceCategoryId = Convert.ToInt64(Id)
                }).Union(DeclinedContractList.Select(y => new ContractBuilderViewModel
                {
                    ContractId = y.ContractId,
                    BuilderId = BuilderId,
                    ContractName = y.ContractName,
                    Website = (y.Website),
                    NationalContractPartner = y.Manufacturer.ManufacturerName,
                    ContractIcon = GetContractIcon(y.ContractIcon),
                    ProductList = string.Join(",", y.ContractProduct.OrderBy(p => p.Product.ProductName).Select(p => p.Product.ProductName)),
                    ContractFrom = y.ContrctFrom != null ? FromDate(y.ContrctFrom.ToString()) : "",
                    ContractTo = y.ContrctTo != null ? FromDate(y.ContrctTo.ToString()) : "",
                    SurveyId = CheckSurveyMayJoin(y.ContractId, BuilderId),
                    ActionStatus = "DECLINED",
                    BuilderJoin = BuilderJoin(y.ContractId),
                    ContractDeliverableslist = getdeliverable(y.ContractId),
                    ResourceCategoryId = Convert.ToInt64(Id)
                }));

                int ProjectCounter = 1;
                var newList = list.ToList()
                    .Select(x => new
                    {
                        ContractId = x.ContractId,
                        BuilderId = x.BuilderId,
                        ContractName = x.ContractName,
                        Website = (x.Website),
                        NationalContractPartner = x.NationalContractPartner,
                        ContractIcon = x.ContractIcon,
                        ProductList = x.ProductList,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        SurveyId = x.SurveyId,
                        ActionStatus = x.ActionStatus,
                        BuilderJoin = x.BuilderJoin,
                        ContractDeliverableslist = x.ContractDeliverableslist,
                        ResourceCategoryId = x.ResourceCategoryId,
                        rowcount = ProjectCounter++
                    }).ToList();

                var jsonResult = Json(newList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

                //return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else
            {
                ContractList = _ObjContractService.GetNonAssociateContractWithBuilder(BuilderId, ContractFlag).Distinct();
                DeclinedContractList = _ObjContractBuilderService.GetDeclinedContractsofBuilder(BuilderId, 1);

                list = ContractList.Select(x => new ContractBuilderViewModel
                {
                    ContractId = x.ContractId,
                    BuilderId = BuilderId,
                    ContractName = x.ContractName,
                    Website = (x.Website),
                    NationalContractPartner = x.Manufacturer.ManufacturerName,
                    ContractIcon = GetContractIcon(x.ContractIcon),
                    ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName.Trim())),
                    ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                    ContractTo = x.ContrctTo != null ? FromDate(x.ContrctTo.ToString()) : "",
                    SurveyId = CheckSurveyMayJoin(x.ContractId, BuilderId),
                    ActionStatus = CheckStatusMayJoin(x.ContractId, BuilderId),
                    BuilderJoin = BuilderJoin(x.ContractId),
                    ContractDeliverableslist = getdeliverable(x.ContractId),
                    ResourceCategoryId = Convert.ToInt64(Id)
                }).Union(DeclinedContractList.Select(y => new ContractBuilderViewModel
                {
                    ContractId = y.ContractId,
                    BuilderId = BuilderId,
                    ContractName = y.ContractName,
                    Website = (y.Website),
                    NationalContractPartner = y.Manufacturer.ManufacturerName,
                    ContractIcon = GetContractIcon(y.ContractIcon),
                    ProductList = string.Join(",", y.ContractProduct.OrderBy(p => p.Product.ProductName).Select(p => p.Product.ProductName.Trim())),
                    ContractFrom = y.ContrctFrom != null ? FromDate(y.ContrctFrom.ToString()) : "",
                    ContractTo = y.ContrctTo != null ? FromDate(y.ContrctTo.ToString()) : "",
                    SurveyId = CheckSurveyMayJoin(y.ContractId, BuilderId),
                    ActionStatus = "DECLINED",
                    BuilderJoin = BuilderJoin(y.ContractId),
                    ContractDeliverableslist = getdeliverable(y.ContractId),
                    ResourceCategoryId = Convert.ToInt64(Id)
                }));

                if (Type == "asccon")
                {
                    list = list.OrderBy(c => c.ContractName);
                }
                else if (Type == "desccon")
                {
                    list = list.OrderByDescending(c => c.ContractName);
                }
                else if (Type == "ascncp")
                {
                    list = list.OrderBy(c => c.NationalContractPartner);
                }
                else if (Type == "descncp")
                {
                    list = list.OrderByDescending(c => c.NationalContractPartner);
                }
                else if (Type == "ascyr")
                {
                    list = list.OrderBy(c => Convert.ToDateTime(c.ContractTo));
                }
                else if (Type == "descyr")
                {
                    list = list.OrderByDescending(c => Convert.ToDateTime(c.ContractTo));
                }
                else if (Type == "ascbj")
                {
                    list = list.OrderBy(c => Convert.ToInt32(c.BuilderJoin));
                }
                else if (Type == "descbj")
                {
                    list = list.OrderByDescending(c => Convert.ToInt32(c.BuilderJoin));
                }
                else if (Type == "ascpc")
                {
                    list = list.OrderBy(c => c.ProductList);
                }
                else if (Type == "descpc")
                {
                    list = list.OrderByDescending(c => c.ProductList);
                }
                int ProjectCounter = 1;

                var newList = list.ToList()
                    .Select(x => new
                    {
                        ContractId = x.ContractId,
                        BuilderId = x.BuilderId,
                        ContractName = x.ContractName,
                        Website = (x.Website),
                        NationalContractPartner = x.NationalContractPartner,
                        ContractIcon = x.ContractIcon,
                        ProductList = x.ProductList,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        SurveyId = x.SurveyId,
                        ActionStatus = x.ActionStatus,
                        BuilderJoin = x.BuilderJoin,
                        ContractDeliverableslist = x.ContractDeliverableslist,
                        ResourceCategoryId = x.ResourceCategoryId,
                        rowcount = ProjectCounter++
                    }).ToList();

                var jsonResult = Json(newList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

                //return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Builder")]

        public ActionResult Dashboard()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string id = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            return View();
        }
        [Authorize(Roles = "Builder")]

        public ActionResult ArchivedContract()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string id = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            return View();
        }
        [Authorize(Roles = "Builder")]

        public ActionResult ActiveContractList([DataSourceRequest] DataSourceRequest request, string Type, int? PageValue)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string id = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;

            DateTime currentdate = DateTime.Now.Date;
            var Quater = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
            var Year = Quater.Year;

            Int64 QuaterId = Quater.QuaterId;

            var resource = _ObjResourceCategoryService.GetResourceCategoryAll().Where(X => X.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.ResourceCategoryId).ToList();
            var Id = resource.FirstOrDefault();

            Int64 BuilderId = Convert.ToInt64(id);//putbuilderid here
            IEnumerable<ContractBuilder> ContractList = null;
            IEnumerable<ContractBuilderViewModel> list = null;
            IEnumerable<ContractBuilderViewModel> listFinal = null;
            int ProjectCounter = 1;
            string msg = "";
            if (Type == null)
            {
                try
                {
                    ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w=> Quater.StartDate<=Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30)).OrderByDescending(x => x.Contract.ContractId);
                    list = ContractList.Select(x => new ContractBuilderViewModel
                    {
                        ContractId = x.ContractId,
                        BuilderId = BuilderId,
                        ContractName = x.Contract.ContractName,
                        Website = (x.Contract.Website),
                        ContractIcon = GetContractIcon(x.Contract.ContractIcon),
                        NationalContractPartner = x.Contract.Manufacturer.ManufacturerName,
                        //  ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                        ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                        ContractFrom = x.Contract.ContrctFrom != null ? FromDate(x.Contract.ContrctFrom.ToString()) : "",
                        ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                        Estimated = estimated(x.ContractId, QuaterId),
                        Percentage = percentage(x.ContractId, QuaterId),
                        // ContractDeliverables = x.Contract.ContractDeliverables,
                        ContractDeliverableslist = getdeliverable(x.ContractId),
                        ResourceCategoryId = Convert.ToInt64(Id)
                    });
                    listFinal = list.ToList()
                    .Select(x => new ContractBuilderViewModel
                    {
                        ContractId = x.ContractId,
                        BuilderId = BuilderId,
                        ContractName = x.ContractName,
                        Website = x.Website,
                        ContractIcon = x.ContractIcon,
                        NationalContractPartner = x.NationalContractPartner,
                        ProductList = x.ProductList,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        Estimated = x.Estimated,
                        Percentage = x.Percentage,
                        ContractDeliverableslist = x.ContractDeliverableslist,
                        ResourceCategoryId = x.ResourceCategoryId,
                        rowcount = ProjectCounter++
                    }).ToList();
                }
                catch (Exception ee)
                {
                    msg = ee.Message.ToString();
                }
                

                var jsonResult = Json(listFinal.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }

            else
            {
                if (Type == "ascpc" || Type == "descpc")
                {
                    if (Type == "ascpc")
                    {
                        ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w => Quater.StartDate <= Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30));
                        list = ContractList.Select(x => new ContractBuilderViewModel
                        {
                            ContractId = x.ContractId,
                            BuilderId = BuilderId,
                            ContractName = x.Contract.ContractName,
                            Website = (x.Contract.Website),
                            NationalContractPartner = x.Contract.Manufacturer.ManufacturerName,
                            ContractIcon = GetContractIcon(x.Contract.ContractIcon),
                            //ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                            ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                            ContractFrom = x.Contract.ContrctFrom != null ? FromDate(x.Contract.ContrctFrom.ToString()) : "",
                            ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                            Estimated = estimated(x.ContractId, QuaterId),
                            Percentage = percentage(x.ContractId, QuaterId),
                            // ContractDeliverables = x.Contract.ContractDeliverables,
                            ContractDeliverableslist = getdeliverable(x.ContractId),
                            ResourceCategoryId = Convert.ToInt64(Id)
                        }).OrderBy(x => x.ProductList);

                        listFinal = list.ToList().Select(x => new ContractBuilderViewModel
                        {
                            ContractId = x.ContractId,
                            BuilderId = BuilderId,
                            ContractName = x.ContractName,
                            Website = x.Website,
                            ContractIcon = x.ContractIcon,
                            NationalContractPartner = x.NationalContractPartner,
                            ProductList = x.ProductList,
                            ContractFrom = x.ContractFrom,
                            ContractTo = x.ContractTo,
                            Estimated = x.Estimated,
                            Percentage = x.Percentage,
                            ContractDeliverableslist = x.ContractDeliverableslist,
                            ResourceCategoryId = x.ResourceCategoryId,
                            rowcount = ProjectCounter++
                        }).ToList();
                    }
                    else
                    {
                        ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w => Quater.StartDate <= Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30));
                        list = ContractList.Select(x => new ContractBuilderViewModel
                        {
                            ContractId = x.ContractId,
                            BuilderId = BuilderId,
                            ContractName = x.Contract.ContractName,
                            Website = (x.Contract.Website),
                            NationalContractPartner = x.Contract.Manufacturer.ManufacturerName,
                            ContractIcon = GetContractIcon(x.Contract.ContractIcon),
                            // ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                            ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                            ContractFrom = x.Contract.ContrctFrom != null ? FromDate(x.Contract.ContrctFrom.ToString()) : "",
                            ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                            Estimated = estimated(x.ContractId, QuaterId),
                            Percentage = percentage(x.ContractId, QuaterId),
                            // ContractDeliverables = x.Contract.ContractDeliverables,
                            ContractDeliverableslist = getdeliverable(x.ContractId),
                            ResourceCategoryId = Convert.ToInt64(Id)
                        }).OrderByDescending(x => x.ProductList);

                        listFinal = list.ToList().Select(x => new ContractBuilderViewModel
                        {
                            ContractId = x.ContractId,
                            BuilderId = BuilderId,
                            ContractName = x.ContractName,
                            Website = x.Website,
                            ContractIcon = x.ContractIcon,
                            NationalContractPartner = x.NationalContractPartner,
                            ProductList = x.ProductList,
                            ContractFrom = x.ContractFrom,
                            ContractTo = x.ContractTo,
                            Estimated = x.Estimated,
                            Percentage = x.Percentage,
                            ContractDeliverableslist = x.ContractDeliverableslist,
                            ResourceCategoryId = x.ResourceCategoryId,
                            rowcount = ProjectCounter++
                        }).ToList();
                    }

                    var jsonResult = Json(listFinal.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                else
                {
                    if (Type == "asccon")
                    {
                        ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w => Quater.StartDate <= Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30)).OrderBy(x => x.Contract.ContractName);
                    }
                    else if (Type == "desccon")
                    {
                        ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w => Quater.StartDate <= Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30)).OrderByDescending(x => x.Contract.ContractName);
                    }
                    else if (Type == "ascncp")
                    {
                        ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w => Quater.StartDate <= Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30)).OrderBy(x => x.Contract.Manufacturer.ManufacturerName);
                    }
                    else if (Type == "descncp")
                    {
                        ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w => Quater.StartDate <= Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30)).OrderByDescending(x => x.Contract.Manufacturer.ManufacturerName);
                    }
                    else if (Type == "ascyr")
                    {
                        ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w => Quater.StartDate <= Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30)).OrderBy(x => x.Contract.ContrctFrom);
                    }
                    else if (Type == "descyr")
                    {
                        ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(w => Quater.StartDate <= Convert.ToDateTime(w.Contract.ContrctTo).AddDays(30)).OrderBy(x => x.Contract.ContrctTo);
                    }

                    list = ContractList.Select(x => new ContractBuilderViewModel
                    {
                        ContractId = x.ContractId,
                        BuilderId = BuilderId,
                        ContractName = x.Contract.ContractName,
                        Website = (x.Contract.Website),
                        NationalContractPartner = x.Contract.Manufacturer.ManufacturerName,
                        ContractIcon = GetContractIcon(x.Contract.ContractIcon),
                        //  ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                        ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                        ContractFrom = x.Contract.ContrctFrom != null ? FromDate(x.Contract.ContrctFrom.ToString()) : "",
                        ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                        Estimated = estimated(x.ContractId, QuaterId),
                        Percentage = percentage(x.ContractId, QuaterId),
                        // ContractDeliverables = x.Contract.ContractDeliverables,
                        ContractDeliverableslist = getdeliverable(x.ContractId),
                        ResourceCategoryId = Convert.ToInt64(Id)
                    });

                    listFinal = list.ToList().Select(x => new ContractBuilderViewModel
                    {
                        ContractId = x.ContractId,
                        BuilderId = BuilderId,
                        ContractName = x.ContractName,
                        Website = x.Website,
                        ContractIcon = x.ContractIcon,
                        NationalContractPartner = x.NationalContractPartner,
                        ProductList = x.ProductList,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        Estimated = x.Estimated,
                        Percentage = x.Percentage,
                        ContractDeliverableslist = x.ContractDeliverableslist,
                        ResourceCategoryId = x.ResourceCategoryId,
                        rowcount = ProjectCounter++
                    }).ToList();

                    var jsonResult = Json(listFinal.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }
        }

        public List<string> getdeliverable(Int64 ContractId)
        {
            string Deliverable = _ObjContractService.GetBuilderCount(ContractId).ContractDeliverables;
            string result = "";
            List<string> primes = new List<string>();
            if (Deliverable == "" || Deliverable == null)
            {
                result = "";
            }
            if (Deliverable.Contains(","))
            {
                string[] values = Deliverable.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    //values[i] ="<p>"+values[i].Trim()+"</p>";
                    //result = result + values[i];
                    primes.Add(values[i]);

                }
                return primes;
            }
            else
            {
                primes.Add(Deliverable);
                return primes;
            }
            //return null;

        }

        public string actual(Int64 ContractId)
        {
            string Actual = "";
            return Actual;
        }

        //public string estimated(Int64 ContractId)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);

        //    // Int64 BuilderId=2; //putnuilderid here
        //    string result = "";
        //    int count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count();
        //    var Answar = "";
        //    if (count > 0)
        //    {
        //        var QuestioId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().QuestionId;
        //        var SurveyId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().SurveyId;
        //        int count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).Count();
        //        if (count3 > 0)
        //        {
        //            Answar = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).FirstOrDefault().Answer;
        //            string estimated = Answar;
        //        }
        //    }
        //    int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
        //    if (count1 > 0)
        //    {
        //        Decimal percentage = 0;
        //        var ActualQuestionId = _ObjContractCompliance.GetActualValueCompliance(ContractId).FirstOrDefault().QuestionId;
        //        if (_ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).Count() > 0)
        //        {
        //            var BuilderQuaterAdminRepotId = _ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).FirstOrDefault().BuilderQuaterAdminReportId;
        //            var list = _ObjBuilderQuaterContractProjectDetails.GetAllProjectDetailsOfContract(Convert.ToInt64(ActualQuestionId), Convert.ToInt64(BuilderQuaterAdminRepotId));
        //            Decimal value = 0;
        //            foreach (var item in list)
        //            {
        //                value = value + Convert.ToDecimal(item.Answer);

        //            }
        //            if (Answar != "")
        //            {
        //                percentage = (value / Convert.ToDecimal(Answar)) * 100;
        //                result = value.ToString() + "/" + Answar + "Annually";
        //            }
        //            else
        //            {
        //                int ans = 0;
        //                percentage = 0;
        //                result = value.ToString() + "/" + ans.ToString() + "Annually";
        //            }

        //        }
        //    }
        //    return result;
        //}
        [Authorize(Roles = "Builder")]
        public string estimated(Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            DateTime currentdate = DateTime.Now.Date;

            var Year = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().Year;
            //ViewBag.Year = Year;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            var quaterlist = _ObjQuaterService.GetQuatersUptoCurrentQuater(QuaterId, Convert.ToInt64(Year)).Select(x => x.QuaterId);

            // Int64 BuilderId=2; //putnuilderid here
            string result = "";
            int count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count();
            var Answar = "";
            if (count > 0)
            {
                var QuestioId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().QuestionId;
                var SurveyId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().SurveyId;
                int count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).Count();
                if (count3 > 0)
                {
                    Answar = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).FirstOrDefault().Answer;
                    string estimated = Answar;
                }
            }
            int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
            if (count1 > 0)
            {
                Decimal percentage = 0;
                List<string> questionlist = new List<string>();
                List<string> ActualAnswarlist = new List<string>();
                //var ActualQuestionId = _ObjContractCompliance.GetActualValueCompliance(ContractId).FirstOrDefault().QuestionId;
                foreach (var quater in quaterlist)
                {
                    var QuestionId = _ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(quater)).FirstOrDefault().QuestionId;
                    questionlist.Add(QuestionId.ToString());


                    if (_ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).Count() > 0)
                    {
                        var BuilderQuaterContractProjectReportId = _ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).FirstOrDefault().BuilderQuaterContractProjectReportId;
                        foreach (var actualquestion in questionlist)
                        {
                            var ActualAnswar = _ObjBuilderQuaterContractProjectDetails.GetAllProjectDetailsOfContract(Convert.ToInt64(actualquestion), Convert.ToInt64(BuilderQuaterContractProjectReportId));
                            ActualAnswarlist.Add(ActualAnswar.ToString());
                        }


                        Decimal value = 0;
                        foreach (var item in ActualAnswarlist)
                        {
                            value = value + Convert.ToDecimal(item);

                        }
                        if (Answar != "")
                        {
                            percentage = (value / Convert.ToDecimal(Answar)) * 100;
                            result = value.ToString() + "/" + Answar + "Annually";
                        }
                        else
                        {
                            int ans = 0;
                            percentage = 0;
                            result = value.ToString() + "/" + ans.ToString() + "Annually";
                        }

                    }
                }
            }
            return result;
        }
        [Authorize(Roles = "Builder")]
        public string percentage(Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            Decimal percentage = 0;
            DateTime currentdate = DateTime.Now.Date;
            //var QuaterName = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterName;
            //ViewBag.QuaterName = QuaterName;
            var Year = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().Year;
            //ViewBag.Year = Year;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            var quaterlist = _ObjQuaterService.GetQuatersUptoCurrentQuater(QuaterId, Convert.ToInt64(Year)).Select(x => x.QuaterId);

            // Int64 BuilderId=2; //putnuilderid here
            string result = "";
            int count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count();
            var Answar = "";
            if (count > 0)
            {
                var QuestioId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().QuestionId;
                var SurveyId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().SurveyId;
                int count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).Count();
                if (count3 > 0)
                {
                    Answar = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).FirstOrDefault().Answer;
                    string estimated = Answar;
                }
            }
            int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
            if (count1 > 0)
            {

                List<string> questionlist = new List<string>();
                List<string> ActualAnswarlist = new List<string>();
                //var ActualQuestionId = _ObjContractCompliance.GetActualValueCompliance(ContractId).FirstOrDefault().QuestionId;
                foreach (var quater in quaterlist)
                {
                    var QuestionId = _ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(quater)).FirstOrDefault().QuestionId;
                    questionlist.Add(QuestionId.ToString());


                    if (_ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).Count() > 0)
                    {
                        var BuilderQuaterContractProjectReportId = _ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).FirstOrDefault().BuilderQuaterContractProjectReportId;
                        foreach (var actualquestion in questionlist)
                        {
                            var ActualAnswar = _ObjBuilderQuaterContractProjectDetails.GetAllProjectDetailsOfContract(Convert.ToInt64(actualquestion), Convert.ToInt64(BuilderQuaterContractProjectReportId));
                            ActualAnswarlist.Add(ActualAnswar.ToString());
                        }


                        Decimal value = 0;
                        foreach (var item in ActualAnswarlist)
                        {
                            value = value + Convert.ToDecimal(item);

                        }
                        if (Answar != "")
                        {
                            percentage = (value / Convert.ToDecimal(Answar)) * 100;
                            result = value.ToString() + "/" + Answar + "Annually";
                        }
                        else
                        {
                            int ans = 0;
                            percentage = 0;
                            result = value.ToString() + "/" + ans.ToString() + "Annually";
                        }

                    }
                }
            }
            return percentage.ToString();
        }

        //public string percentage(Int64 ContractId)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);

        //    // Int64 BuilderId = 2; //putnuilderid here
        //    int count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count();
        //    var Answar = "";
        //    if (count > 0)
        //    {
        //        var QuestioId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().QuestionId;
        //        var SurveyId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().SurveyId;
        //        int count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).Count();
        //        if (count3 > 0)
        //        {
        //            Answar = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).FirstOrDefault().Answer;
        //            string estimated = Answar;
        //        }
        //    }
        //    int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
        //    Decimal percentage = 0;
        //    if (count1 > 0)
        //    {
        //        var ActualQuestionId = _ObjContractCompliance.GetActualValueCompliance(ContractId).FirstOrDefault().QuestionId;
        //        if (_ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).Count() > 0)
        //        {
        //            var BuilderQuaterAdminRepotId = _ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).FirstOrDefault().BuilderQuaterAdminReportId;
        //            var list = _ObjBuilderQuaterContractProjectDetails.GetAllProjectDetailsOfContract(Convert.ToInt64(ActualQuestionId), Convert.ToInt64(BuilderQuaterAdminRepotId));
        //            Decimal value = 0;
        //            foreach (var item in list)
        //            {
        //                value = value + Convert.ToDecimal(item.Answer);

        //            }
        //            if (Answar != "")
        //            {

        //                percentage = (value / Convert.ToDecimal(Answar)) * 100;
        //            }
        //            else
        //            {
        //                percentage = 0;
        //            }
        //            string result = value.ToString() + "/" + Answar + "Annually";
        //        }
        //    }
        //    return percentage.ToString();
        //}

        [Authorize(Roles = "Builder")]
        public ActionResult ArchivedContractList([DataSourceRequest] DataSourceRequest request, string Type, int? PageValue)
        {
            var resource = _ObjResourceCategoryService.GetResourceCategoryAll().Where(X => X.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.ResourceCategoryId);
            var Id = resource.FirstOrDefault();

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            // int BuilderId = 2; //put builderid here
            IEnumerable<ContractBuilder> ContractList = null;

            IEnumerable<ContractBuilderViewModel> list;
            if (Type == null)
            {
                ContractList = _ObjContractBuilderService.GetArchiveContractsofBuilder(BuilderId).OrderByDescending(x => x.Contract.ContractId);

            }

            else
            {

                if (Type == "asccon")
                {
                    ContractList = _ObjContractBuilderService.GetArchiveContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContractName);


                }

                else if (Type == "desccon")
                {
                    ContractList = _ObjContractBuilderService.GetArchiveContractsofBuilder(BuilderId).OrderByDescending(x => x.Contract.ContractName);

                }

                else if (Type == "ascncp")
                {
                    ContractList = _ObjContractBuilderService.GetArchiveContractsofBuilder(BuilderId).OrderBy(x => x.Contract.Manufacturer.ManufacturerName);

                }
                else if (Type == "descncp")
                {
                    ContractList = _ObjContractBuilderService.GetArchiveContractsofBuilder(BuilderId).OrderByDescending(x => x.Contract.Manufacturer.ManufacturerName);

                }
                else if (Type == "ascyr")
                {
                    ContractList = _ObjContractBuilderService.GetArchiveContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContrctFrom);

                }
                else if (Type == "descyr")
                {
                    ContractList = _ObjContractBuilderService.GetArchiveContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContrctTo);

                }

            }
            list = ContractList.Select(x => new ContractBuilderViewModel
            {
                ContractId = x.ContractId,
                BuilderId = BuilderId,
                ContractName = x.Contract.ContractName,
                Website = (x.Contract.Website),
                NationalContractPartner = x.Contract.Manufacturer.ManufacturerName,
                ContractIcon = GetContractIcon(x.Contract.ContractIcon),
                ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                ContractFrom = x.Contract.ContrctFrom != null ? FromDate(x.Contract.ContrctFrom.ToString()) : "",
                ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                Estimated = estimated(x.ContractId),
                Percentage = percentage(x.ContractId),
                //ContractDeliverables = x.Contract.ContractDeliverables,
                ContractDeliverableslist = getdeliverable(x.ContractId),
                ResourceCategoryId = Convert.ToInt64(Id)

            });

            int ProjectCounter = 1;
            var newList = list.ToList().Select(x => new ContractBuilderViewModel
            {
                ContractId = x.ContractId,
                BuilderId = x.BuilderId,
                ContractName = x.ContractName,
                Website = (x.Website),
                NationalContractPartner = x.NationalContractPartner,
                ContractIcon = x.ContractIcon,
                ProductList = x.ProductList,
                ContractFrom = x.ContractFrom,
                ContractTo = x.ContractTo,
                Estimated = x.Estimated,
                Percentage = x.Percentage,
                ContractDeliverableslist = x.ContractDeliverableslist,
                ResourceCategoryId = x.ResourceCategoryId,
                rowcount = ProjectCounter++
            }).ToList();

            return Json(newList.ToDataSourceResult(request));
        }

        public ActionResult ResourceCategory()
        {

            return View();
        }

        public ActionResult PendingContract()
        {

            return View();
        }

        [Authorize(Roles = "Builder")]

        public ActionResult PendingContractList([DataSourceRequest] DataSourceRequest request, string Type, int? PageValue)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            int ProjectCounter = 1;
            //int BuilderId = 2; //putbuilderid here
            IEnumerable<ContractBuilder> ContractList = null;
            IEnumerable<ResourceCategory> CatList = null;

            IEnumerable<ContractBuilderViewModel> list;
            IEnumerable<ContractBuilderViewModel> listFinal = null;
            var resource = _ObjResourceCategoryService.GetResourceCategoryAll().Where(X => X.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.ResourceCategoryId);
            var Id = resource.FirstOrDefault();

            // var Id = _ObjResourceCategoryService.GetResourceCategoryAll().Take(1).Select(x => x.ResourceCategoryId);

            if (Type == null)
            {
                ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderByDescending(x => x.Contract.ContractId);
                list = ContractList.Select(x => new ContractBuilderViewModel
                {
                    ContractId = x.ContractId,
                    SurveyId = CheckSurveyId(x.ContractId),
                    BuilderId = BuilderId,
                    ContractName = x.Contract.ContractName,
                    Websiteslist = getwebsitedata(x.ContractId),
                    Manufactererlist = getManufacterer(x.ContractId),
                    //ManuFacturerName = PrimaryContracts(x.Contract.PrimaryManufacturer),
                    //Website = (x.Contract.Website),
                    //  ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                    ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                    ContractFrom = x.Contract.EstimatedStartDate != null ? FromDate(x.Contract.EstimatedStartDate.ToString()) : "",
                    ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                    ActionStatus = CheckStatus(x.ContractId),
                    ContractStatus = x.Contract.ContractStatus.ContractStatusName,
                    ResourceCategoryId = Convert.ToInt64(Id)
                });

                listFinal = list.ToList().Select(x => new ContractBuilderViewModel
                {
                    ContractId = x.ContractId,
                    SurveyId = x.SurveyId,
                    BuilderId = BuilderId,
                    ContractName = x.ContractName,
                    Websiteslist = x.Websiteslist,
                    Manufactererlist = x.Manufactererlist,
                    ProductList = x.ProductList,
                    ContractFrom = x.ContractFrom,
                    ContractTo = x.ContractTo,
                    ActionStatus = x.ActionStatus,
                    ContractStatus = x.ContractStatus,
                    ResourceCategoryId = x.ResourceCategoryId,
                    rowcount = ProjectCounter++
                }).ToList();

                return Json(listFinal.ToDataSourceResult(request));
            }

            else
            {
                if (Type == "ascpc" || Type == "descpc")
                {
                    if (Type == "ascpc")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContractName);
                        list = ContractList.Select(x => new ContractBuilderViewModel
                        {
                            ContractId = x.ContractId,
                            SurveyId = CheckSurveyId(x.ContractId),
                            BuilderId = BuilderId,
                            ContractName = x.Contract.ContractName,
                            Websiteslist = getwebsitedata(x.ContractId),
                            Manufactererlist = getManufacterer(x.ContractId),

                            //ManuFacturerName = PrimaryContracts(x.Contract.PrimaryManufacturer),
                            //Website = (x.Contract.Website),
                            // ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                            ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                            ContractFrom = x.Contract.EstimatedStartDate != null ? FromDate(x.Contract.EstimatedStartDate.ToString()) : "",
                            ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                            ActionStatus = CheckStatus(x.ContractId),
                            ContractStatus = x.Contract.ContractStatus.ContractStatusName,
                            ResourceCategoryId = Convert.ToInt64(Id)

                        }).OrderBy(x => x.ProductList);

                        listFinal = list.ToList().Select(x => new ContractBuilderViewModel
                        {
                            ContractId = x.ContractId,
                            SurveyId = x.SurveyId,
                            BuilderId = BuilderId,
                            ContractName = x.ContractName,
                            Websiteslist = x.Websiteslist,
                            Manufactererlist = x.Manufactererlist,
                            ProductList = x.ProductList,
                            ContractFrom = x.ContractFrom,
                            ContractTo = x.ContractTo,
                            ActionStatus = x.ActionStatus,
                            ContractStatus = x.ContractStatus,
                            ResourceCategoryId = x.ResourceCategoryId,
                            rowcount = ProjectCounter++
                        }).ToList();

                        return Json(listFinal.ToDataSourceResult(request));
                    }
                    else if (Type == "descpc")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId);
                        list = ContractList.Select(x => new ContractBuilderViewModel
                        {
                            ContractId = x.ContractId,
                            SurveyId = CheckSurveyId(x.ContractId),
                            BuilderId = BuilderId,
                            ContractName = x.Contract.ContractName,
                            Websiteslist = getwebsitedata(x.ContractId),
                            Manufactererlist = getManufacterer(x.ContractId),

                            //ManuFacturerName = PrimaryContracts(x.Contract.PrimaryManufacturer),
                            //Website = (x.Contract.Website),
                            //   ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                            ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                            ContractFrom = x.Contract.EstimatedStartDate != null ? FromDate(x.Contract.EstimatedStartDate.ToString()) : "",
                            ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                            ActionStatus = CheckStatus(x.ContractId),
                            ContractStatus = x.Contract.ContractStatus.ContractStatusName,
                            ResourceCategoryId = Convert.ToInt64(Id)

                        }).OrderByDescending(x => x.ProductList);

                        listFinal = list.ToList().Select(x => new ContractBuilderViewModel
                        {
                            ContractId = x.ContractId,
                            SurveyId = x.SurveyId,
                            BuilderId = BuilderId,
                            ContractName = x.ContractName,
                            Websiteslist = x.Websiteslist,
                            Manufactererlist = x.Manufactererlist,
                            ProductList = x.ProductList,
                            ContractFrom = x.ContractFrom,
                            ContractTo = x.ContractTo,
                            ActionStatus = x.ActionStatus,
                            ContractStatus = x.ContractStatus,
                            ResourceCategoryId = x.ResourceCategoryId,
                            rowcount = ProjectCounter++
                        }).ToList();

                        return Json(listFinal.ToDataSourceResult(request));
                    }

                }
                else
                {
                    if (Type == "ascpcon")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContractName);


                    }

                    else if (Type == "descpcon")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderByDescending(x => x.Contract.ContractName);

                    }

                    else if (Type == "ascpncp")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderBy(x => x.Contract.PrimaryManufacturer);

                    }
                    else if (Type == "descpncp")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderByDescending(x => x.Contract.PrimaryManufacturer);

                    }
                    else if (Type == "ascpct")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContractStatus.ContractStatusName);

                    }
                    else if (Type == "descpct")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderByDescending(x => x.Contract.ContractStatus.ContractStatusName);

                    }

                    else if (Type == "ascpst")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderBy(x => x.Contract.EstimatedStartDate);

                    }
                    else if (Type == "descpst")
                    {
                        ContractList = _ObjContractBuilderService.GetPendingContractsofBuilder(BuilderId).OrderByDescending(x => x.Contract.EstimatedStartDate);

                    }
                    list = ContractList.Select(x => new ContractBuilderViewModel
                    {
                        ContractId = x.ContractId,
                        SurveyId = CheckSurveyId(x.ContractId),
                        BuilderId = BuilderId,
                        ContractName = x.Contract.ContractName,
                        Websiteslist = getwebsitedata(x.ContractId),
                        Manufactererlist = getManufacterer(x.ContractId),

                        //ManuFacturerName = PrimaryContracts(x.Contract.PrimaryManufacturer),
                        //Website = (x.Contract.Website),
                        // ProductList = ProductCategory(x.Contract.ContractProduct.Select(y => y.Product).ToList()),
                        ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                        ContractFrom = x.Contract.EstimatedStartDate != null ? FromDate(x.Contract.EstimatedStartDate.ToString()) : "",
                        ContractTo = x.Contract.ContrctTo != null ? FromDate(x.Contract.ContrctTo.ToString()) : "",
                        ActionStatus = CheckStatus(x.ContractId),
                        ContractStatus = x.Contract.ContractStatus.ContractStatusName,
                        ResourceCategoryId = Convert.ToInt64(Id)

                    });

                    listFinal = list.ToList().Select(x => new ContractBuilderViewModel
                    {
                        ContractId = x.ContractId,
                        SurveyId = x.SurveyId,
                        BuilderId = BuilderId,
                        ContractName = x.ContractName,
                        Websiteslist = x.Websiteslist,
                        Manufactererlist = x.Manufactererlist,
                        ProductList = x.ProductList,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        ActionStatus = x.ActionStatus,
                        ContractStatus = x.ContractStatus,
                        ResourceCategoryId = x.ResourceCategoryId,
                        rowcount = ProjectCounter++
                    }).ToList();

                    return Json(listFinal.ToDataSourceResult(request));
                }

            }
            var res = "";
            return Json(res.ToDataSourceResult(request));

        }
        [Authorize(Roles = "Builder")]
        public Int64 CheckSurveyId(Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            Int64 SurveyId = 0;
            //IEnumerable<long> SurveyId = _ObjSurveyService.GetContractSurvey(ContractId).Select(x => x.SurveyId);
            IEnumerable<long> SurveyIdList = _ObjSurveyService.GetContractSurvey(ContractId).Where(x => x.IsEnrolment == false
               && x.IsPublished == true &&
               x.IsNcpSurvey == false).Select(x => x.SurveyId);
            string Status = "";
            int i = 0;
            if (_ObjSurveyService.GetContractSurvey(ContractId).Where(x => x.IsEnrolment == false
                && x.IsPublished == true &&
                x.IsNcpSurvey == false).Count() > 0)
            {
                foreach (var Id in SurveyIdList)
                {
                    if (_ObjSurveyBuilderService.FindSurveyOfBuilder(Id, BuilderId).Count() > 0)
                    {
                        if (_ObjSurveyBuilderService.FindInCompleteSurveyBySurveyIdBuilderId(Id, BuilderId).Count() > 0)
                        {
                            SurveyId = Id;
                            return SurveyId;
                        }
                        else
                        {
                            Status = "All Done";
                        }
                    }
                    else
                    {
                        SurveyId = Id;
                        return SurveyId;
                    }



                }
            }
            return SurveyId;
        }
        [Authorize(Roles = "Builder")]
        public string CheckStatus(Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            Int64 BuilderMarket = _ObjBuilderService.BuilderDetails(BuilderId).FirstOrDefault().MarketId;

            //** we have to do with respect to Market later **//

            IEnumerable<long> SurveyId = _ObjSurveyService.GetContractSurvey(ContractId).Where(x => x.IsEnrolment == false
                && x.IsPublished == true &&
                x.IsNcpSurvey == false).Select(x => x.SurveyId);
            string Status = "";
            int i = 0;
            if (_ObjSurveyService.GetContractSurvey(ContractId).Where(x => x.IsEnrolment == false
                && x.IsPublished == true &&
                x.IsNcpSurvey == false).Count() > 0)
            {
                foreach (var Id in SurveyId)
                {
                    Int64 MarketId = _ObjSurveyService.GetSurveyMarket(Id).FirstOrDefault().MarketId;
                    if (MarketId == BuilderMarket)
                    {
                        if (_ObjSurveyBuilderService.FindSurveyOfBuilder(Id, BuilderId).Count() > 0)
                        {
                            if (_ObjSurveyBuilderService.FindInCompleteSurveyBySurveyIdBuilderId(Id, BuilderId).Count() > 0)
                            {
                                Status = "Continue Survey";
                                return Status;
                            }
                            else
                            {
                                Status = "All Done";
                            }
                        }
                        else
                        {
                            Status = "Follow Up";
                        }

                    }
                    else
                    {
                        Status = "All Done";
                    }

                }
            }
            else
            {
                Status = "All Done";
            }


            return Status;

        }

        public string BuilderJoin(Int64 ContractId)
        {
            var List = _ObjContractBuilderService.GetBuilderofContract(ContractId).Count();
            return List.ToString();
        }

        public Int64 CheckSurveyMayJoin(Int64 ContractId, Int64 BuilderId)
        {
            IEnumerable<long> SurveyId = _ObjSurveyService.GetContractEnrolledSurvey(ContractId).Select(x => x.SurveyId);
            string Status = "";

            if (_ObjSurveyService.GetContractEnrolledSurvey(ContractId).Count() > 0)
            {
                foreach (var Id in SurveyId)
                {
                    IEnumerable<SurveyBuilder> SurveyListExist = _ObjSurveyBuilderService.FindSurveyOfBuilder(Id, BuilderId);
                    if (SurveyListExist.Count() == 0)
                    {
                        Status = "ENROLL";
                        return Id;
                    }
                    else
                    {

                        var check = SurveyListExist.Select(x => x.IsSurveyCompleted).FirstOrDefault();
                        if (check == false)
                        {
                            Status = "CONTINUE SURVEY";
                            return Id;
                        }
                        else
                        {

                            return 0;
                        }
                    }

                }
            }
            else
            {
                Status = "ENROLL";
                // return Status;
            }


            return 0;

        }

        public string CheckStatusMayJoin(Int64 ContractId, Int64 BuilderId)
        {
            IEnumerable<long> SurveyId = _ObjSurveyService.GetContractEnrolledSurvey(ContractId).Select(x => x.SurveyId);
            string Status = "";
            int i = 0;
            if (_ObjSurveyService.GetContractEnrolledSurvey(ContractId).Count() > 0)
            {
                foreach (var Id in SurveyId)
                {
                    IEnumerable<SurveyBuilder> SurveyListExist = _ObjSurveyBuilderService.FindSurveyOfBuilder(Id, BuilderId);
                    if (SurveyListExist.Count() == 0)
                    {
                        Status = "ENROLL";
                        return Status;
                    }
                    else
                    {

                        var check = SurveyListExist.Select(x => x.IsSurveyCompleted).FirstOrDefault();
                        if (check == false)
                        {
                            Status = "CONTINUE SURVEY";
                            return Status;
                        }
                        else
                        {
                            Status = "COMPLETE SURVEY";
                            return Status;
                        }
                    }

                }
            }
            else
            {
                Status = "ENROLL";
                return Status;
            }


            return Status;

        }

        public string PrimaryContracts(string con)
        {
            if (con == "undefined" || con == null)
            {
                con = "";
            }
            return con;
        }

        public string FromDate(string date)
        {
            string dat = "";
            if (date != "")
            {
                DateTime fromdate = Convert.ToDateTime(date);
                dat = fromdate.ToString("MM/dd/yy");

            }
            return dat;
        }

        public string EntryDate(string date)
        {
            string dat = "";
            string daytogo = "";
            if (date != "")
            {
                DateTime fromdate = Convert.ToDateTime(date);
                dat = fromdate.ToString("MM/dd/yy");
                //TimeSpan difference = DateTime.Now.Date - fromdate;
                TimeSpan difference = fromdate - DateTime.Now.Date;
                var days = difference.TotalDays;
                if (Convert.ToInt32(days) < 5)
                {
                    daytogo = days + "day to go";
                }
            }

            return dat;
        }

        public string EntryDateStatus(string date)
        {
            string dat = "";
            string daytogo = "";
            if (date != "")
            {
                DateTime fromdate = Convert.ToDateTime(date);
                dat = fromdate.ToString("MM/dd/yy");
                //TimeSpan difference = DateTime.Now.Date - fromdate;
                TimeSpan difference = fromdate - DateTime.Now.Date;
                var days = difference.TotalDays;
                if (Convert.ToInt32(days) < 5)
                {
                    daytogo = days + "day to go";
                }
            }

            return daytogo;
        }

        public string DayToGo(string date)
        {
            string dat = "";
            string daytogo = "";
            if (date != "")
            {
                DateTime fromdate = Convert.ToDateTime(date);
                dat = fromdate.ToString("MM/dd/yy");
                TimeSpan difference = fromdate - DateTime.Now.Date;
                var days = difference.TotalDays;
                if (Convert.ToInt32(days) == 0)
                {
                    daytogo = "Today";
                }
                else if (Convert.ToInt32(days) > 0)
                {
                    if (Convert.ToInt32(days) < 5)
                    {
                        daytogo = days + "day to go";
                    }
                }


            }
            return daytogo;
        }

        public string DateFormat(string date)
        {
            string dat = "";
            if (date != "")
            {
                DateTime fromdate = Convert.ToDateTime(date);
                dat = fromdate.ToString("dd MMMM yyyy");

            }
            return dat;
        }

        public string ProductCategory(List<Product> h)
        {
            List<string> product = h.Select(x => x.ProductCategory).Select(x => x.ProductCategoryName).Distinct().ToList();

            string products = string.Join(",", product);


            return products.ToString();
        }

        public ActionResult resource()
        {
            return View();
        }

        public FileResult DownloadResourceFile(Int64 ResourceId)
        {
            Resource ObjResource = _ObjResourceService.GetResource(ResourceId);
            return File(ObjResource.FileLocation, MimeMapping.GetMimeMapping(ObjResource.FileName), ObjResource.FileName);

        }
        [Authorize(Roles = "Builder")]

        public ActionResult ContractResourceListview_Read([DataSourceRequest] DataSourceRequest request, Int64 ContractId, Int64 CatId)
        {

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string Builder = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(Builder);

            var ObjBuilder = _ObjBuilderService.BuilderDetails(BuilderId).FirstOrDefault();

            var ResourceList = _ObjResourceService.GetResourcebyCategoryMarket(ContractId, CatId, ObjBuilder.MarketId).Select(x => new ContractResourceViewModel
            {
                ResourceId = x.ResourceId,
                ResourceName = x.Title,
                ResourceTitle = x.Description,
                css = ContractId == x.ResourceId ? "active" : "",
                upload = DateFormat(x.CreatedOn.ToString()),
                ResourceLocation = FileType(x.FileLocation)
            });

            return Json(ResourceList.ToDataSourceResult(request));
        }

        public string FileType(string location)
        {
            var result = location.Substring(location.LastIndexOf('.') + 1);
            string Type = "";
            if (result == "webm" || result == "mkv" || result == "flv" || result == "avi" || result == "wmv" || result == "mov" || result == "mp4" || result == "mpg")
            {
                Type = "Video";
            }
            else
            {
                Type = "Doc";
            }
            return Type;
        }
        [Authorize(Roles = "Builder")]
        public ActionResult ContractResourceCategory([DataSourceRequest] DataSourceRequest request, Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string Builder = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(Builder);

            var ObjBuilder = _ObjBuilderService.BuilderDetails(BuilderId).FirstOrDefault();

            var resource = _ObjResourceCategoryService.GetResourceCategoryListForContractMarket(ContractId, ObjBuilder.MarketId).Select(x => x.ResourceCategoryId);
            var Id = resource.FirstOrDefault();

            var ResourceList = _ObjResourceCategoryService.GetResourceCategoryListForContractMarket(ContractId, ObjBuilder.MarketId).Select(x => new ResourceCategoryViewModel
            {
                ResourceCategoryId = x.ResourceCategoryId,
                ResourceCategoryName = x.ResourceCategoryName,
                ContractId = ContractId,
                css = x.ResourceCategoryId == Convert.ToInt64(Id) ? "active" : ""
            });

            return Json(ResourceList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetYear()
        {
            Int64 conid = 0;
            int year = DateTime.Now.Year;
            List<Project> years = new List<Project>();
            string proj = "";
            string projname = "";

            for (int i = year; i > (year - 3); i--)
            {

                years.Add(new Project { ProjectId = i, ProjectName = i.ToString() });
            }

            ////  var ProjectList = _ObjProjectService.ProjectList().Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new { ProjectId = x.ProjectId, ProjectName = x.ProjectName }).ToList();

            // years.Insert(0, new { ProjectId = conid, ProjectName = "Select Project" });
            return Json(years, JsonRequestBehavior.AllowGet);
        }
        #region Old Code for ReportHistoryList
        //[Authorize(Roles = "Builder")]
        //public ActionResult ReportHistoryList([DataSourceRequest] DataSourceRequest request, string Type, string Flag)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);
        //    Int64 ContractId;
        //    int ProjectCounter = 1;
        //    // Int64 BuilderId = 2; //putbuilderid here
        //    if (Flag != null && Flag != "undefined")
        //    {
        //        ContractId = Int64.Parse(Flag); // Convert.ToInt64(Session["Contract"]);
        //    }
        //    else
        //    {
        //        ContractId = Convert.ToInt64(Session["Contract"]);
        //    }

        //    if (Type == "year")
        //    {
        //        var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
        //                        .Select(group => group.First());

        //        var newList = reportlist.Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = Quater(x.QuaterId),
        //            ContractId = ContractId,
        //            ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
        //            SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //            year = FindYear(x.QuaterId),
        //            estimatedactual = quaterestimated(ContractId, x.QuaterId),
        //            percentage = quaterpercentage(ContractId, x.QuaterId)
        //        }).OrderBy(x => x.Quater);

        //        var ListByYear = newList.Where(x => x.year == Flag);

        //        var histryList = ListByYear.ToList()
        //            .Select(x => new
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(histryList.ToDataSourceResult(request));
        //    }

        //    else if (Type == "asccon")
        //    {
        //        var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
        //                        .Select(group => group.First());

        //        var newList = reportlist.Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = Quater(x.QuaterId),
        //            ContractId = ContractId,
        //            ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
        //            SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //            estimatedactual = quaterestimated(ContractId, x.QuaterId),
        //            percentage = quaterpercentage(ContractId, x.QuaterId)
        //        }).OrderBy(x => x.Quater);

        //        var histryList = newList.ToList()
        //            .Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(histryList.ToDataSourceResult(request));
        //    }

        //    else if (Type == "desccon")
        //    {
        //        var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
        //                            .Select(group => group.First());

        //        var newList = reportlist.Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = Quater(x.QuaterId),
        //            ContractId = ContractId,
        //            ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
        //            SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //            estimatedactual = quaterestimated(ContractId, x.QuaterId),
        //            percentage = quaterpercentage(ContractId, x.QuaterId)
        //        }).OrderByDescending(x => x.Quater);

        //        var histryList = newList.ToList()
        //            .Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(histryList.ToDataSourceResult(request));
        //    }

        //    else if (Type == "ascncp")
        //    {
        //        var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
        //                            .Select(group => group.First());

        //        var newList = reportlist.Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = Quater(x.QuaterId),
        //            ContractId = ContractId,
        //            ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
        //            SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //            estimatedactual = quaterestimated(ContractId, x.QuaterId),
        //            percentage = quaterpercentage(ContractId, x.QuaterId)
        //        }).OrderBy(x => x.ProjectCount);

        //        var histryList = newList.ToList()
        //            .Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(histryList.ToDataSourceResult(request));
        //    }
        //    else if (Type == "descncp")
        //    {
        //        var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
        //                            .Select(group => group.First());

        //        var newList = reportlist.Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = Quater(x.QuaterId),
        //            ContractId = ContractId,
        //            ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
        //            SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //            estimatedactual = quaterestimated(ContractId, x.QuaterId),
        //            percentage = quaterpercentage(ContractId, x.QuaterId)
        //        }).OrderByDescending(x => x.ProjectCount);

        //        var histryList = newList.ToList()
        //            .Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(histryList.ToDataSourceResult(request));
        //    }
        //    else if (Type == "ascbj")
        //    {
        //        var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
        //                            .Select(group => group.First());

        //        var newList = reportlist.Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = Quater(x.QuaterId),
        //            ContractId = ContractId,
        //            ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
        //            SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //            estimatedactual = quaterestimated(ContractId, x.QuaterId),
        //            percentage = quaterpercentage(ContractId, x.QuaterId)
        //        }).OrderBy(x => x.SubmitDate);

        //        var histryList = newList.ToList()
        //            .Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(histryList.ToDataSourceResult(request));
        //    }

        //    else if (Type == "descbj")
        //    {
        //        var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
        //                            .Select(group => group.First());

        //        var newList = reportlist.Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = Quater(x.QuaterId),
        //            ContractId = ContractId,
        //            ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
        //            SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //            estimatedactual = quaterestimated(ContractId, x.QuaterId),
        //            percentage = quaterpercentage(ContractId, x.QuaterId)
        //        }).OrderByDescending(x => x.SubmitDate);

        //        var histryList = newList.ToList()
        //            .Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //        return Json(histryList.ToDataSourceResult(request));
        //    }
        //    else if (Flag != null)
        //    {
        //        if (Flag == "0")
        //        {
        //            var reportlist = _ObjQuaterContractProjectReport.GetRepotDetailsofAllContract(BuilderId).GroupBy(x => x.QuaterId)
        //                                .Select(group => group.First());

        //            var newList = reportlist.Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = Quater(x.QuaterId),
        //                ContractId = x.ContractId,
        //                ProjectCount = ProjectCount(x.QuaterId, BuilderId, x.ContractId),
        //                SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //                estimatedactual = quaterestimated(x.ContractId, x.QuaterId),
        //                percentage = quaterpercentage(x.ContractId, x.QuaterId)
        //            }).OrderByDescending(x => x.Quater);

        //            var histryList = newList.ToList()
        //            .Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //            return Json(histryList.ToDataSourceResult(request));
        //        }
        //        else
        //        {
        //            Int64 ConId = Convert.ToInt32(Flag);
        //            var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ConId).GroupBy(x => x.QuaterId)
        //                                .Select(group => group.First());

        //            var newList = reportlist.Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = Quater(x.QuaterId),
        //                ContractId = ContractId,
        //                ProjectCount = ProjectCount(x.QuaterId, BuilderId, ConId),
        //                SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //                estimatedactual = quaterestimated(ConId, x.QuaterId),
        //                percentage = quaterpercentage(ConId, x.QuaterId)
        //            }).OrderByDescending(x => x.Quater);

        //            var histryList = newList.ToList()
        //            .Select(x => new ProjectViewModel
        //            {
        //                QuaterId = x.QuaterId,
        //                Quater = x.Quater,
        //                ContractId = x.ContractId,
        //                ProjectCount = x.ProjectCount,
        //                SubmitDate = x.SubmitDate,
        //                year = x.year,
        //                estimatedactual = x.estimatedactual,
        //                percentage = x.percentage,
        //                rowcount = ProjectCounter++
        //            }).ToList();

        //            return Json(histryList.ToDataSourceResult(request));
        //        }
        //    }
        //    else if (Type == null)
        //    {
        //        var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
        //                            .Select(group => group.First());

        //        var newList = reportlist.Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = Quater(x.QuaterId),
        //            ContractId = ContractId,
        //            ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
        //            SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
        //            estimatedactual = quaterestimated(ContractId, x.QuaterId),
        //            percentage = quaterpercentage(ContractId, x.QuaterId)
        //        }).OrderBy(x => x.Quater);


        //        var histryList = newList.ToList()
        //        .Select(x => new ProjectViewModel
        //        {
        //            QuaterId = x.QuaterId,
        //            Quater = x.Quater,
        //            ContractId = x.ContractId,
        //            ProjectCount = x.ProjectCount,
        //            SubmitDate = x.SubmitDate,
        //            year = x.year,
        //            estimatedactual = x.estimatedactual,
        //            percentage = x.percentage,
        //            rowcount = ProjectCounter++
        //        }).ToList();

        //        return Json(histryList.ToDataSourceResult(request));
        //    }

        //    var value = "";
        //    return Json(value.ToDataSourceResult(request));
        //}
#endregion
        #region New Code for ReportHistoryList By Rajendar 3/13/2018
        [Authorize(Roles = "Builder")]
        public ActionResult ReportHistoryList([DataSourceRequest] DataSourceRequest request, string Type, string Flag)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            Int64 ContractId;
            int ProjectCounter = 1;
            // Int64 BuilderId = 2; //putbuilderid here
            if (Flag != null && Flag != "undefined")
            {   if (!string.IsNullOrWhiteSpace(Type))
                {
                    ContractId = Convert.ToInt64(Session["Contract"]);
                }
                else
                {
                    ContractId = Convert.ToInt64(Flag);
                }
            }
            else
            {
                ContractId = Convert.ToInt64(Session["Contract"]);
            }

            if (Type == "year")
            {
                //var reportlist = _ObjQuaterContractProjectReport.GetReportDetails(BuilderId, ContractId).GroupBy(x => x.QuaterId)
                //                .Select(group => group.First());
                var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);
                var newList = reportlist.Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = Quater(x.QuaterId),
                    ContractId = ContractId,
                    ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
                    SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                    year = FindYear(x.QuaterId),
                    estimatedactual = quaterestimated(ContractId, x.QuaterId),
                    percentage = quaterpercentage(ContractId, x.QuaterId)
                }).OrderBy(x => x.Quater);

                var ListByYear = newList.Where(x => x.year == Flag);

                var histryList = ListByYear.ToList()
                    .Select(x => new
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(histryList.ToDataSourceResult(request));
            }

            else if (Type == "asccon")
            {
                var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);
                

                var newList = reportlist.Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = Quater(x.QuaterId),
                    ContractId = ContractId,
                    ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
                    SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                    estimatedactual = quaterestimated(ContractId, x.QuaterId),
                    percentage = quaterpercentage(ContractId, x.QuaterId)
                }).OrderBy(x => x.Quater);

                var histryList = newList.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(histryList.ToDataSourceResult(request));
            }

            else if (Type == "desccon")
            {
                var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);
                var newList = reportlist.Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = Quater(x.QuaterId),
                    ContractId = ContractId,
                    ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
                    SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                    estimatedactual = quaterestimated(ContractId, x.QuaterId),
                    percentage = quaterpercentage(ContractId, x.QuaterId)
                }).OrderByDescending(x => x.Quater);

                var histryList = newList.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(histryList.ToDataSourceResult(request));
            }

            else if (Type == "ascncp")
            {
                var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);

                var newList = reportlist.Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = Quater(x.QuaterId),
                    ContractId = ContractId,
                    ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
                    SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                    estimatedactual = quaterestimated(ContractId, x.QuaterId),
                    percentage = quaterpercentage(ContractId, x.QuaterId)
                }).OrderBy(x => x.ProjectCount);

                var histryList = newList.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(histryList.ToDataSourceResult(request));
            }
            else if (Type == "descncp")
            {
                var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);
                var newList = reportlist.Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = Quater(x.QuaterId),
                    ContractId = ContractId,
                    ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
                    SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                    estimatedactual = quaterestimated(ContractId, x.QuaterId),
                    percentage = quaterpercentage(ContractId, x.QuaterId)
                }).OrderByDescending(x => x.ProjectCount);

                var histryList = newList.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(histryList.ToDataSourceResult(request));
            }
            else if (Type == "ascbj")
            {
                var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);

                var newList = reportlist.Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = Quater(x.QuaterId),
                    ContractId = ContractId,
                    ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
                    SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                    estimatedactual = quaterestimated(ContractId, x.QuaterId),
                    percentage = quaterpercentage(ContractId, x.QuaterId)
                }).OrderBy(x => x.SubmitDate);

                var histryList = newList.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(histryList.ToDataSourceResult(request));
            }

            else if (Type == "descbj")
            {
                var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);

                var newList = reportlist.Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = Quater(x.QuaterId),
                    ContractId = ContractId,
                    ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
                    SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                    estimatedactual = quaterestimated(ContractId, x.QuaterId),
                    percentage = quaterpercentage(ContractId, x.QuaterId)
                }).OrderByDescending(x => x.SubmitDate);

                var histryList = newList.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                return Json(histryList.ToDataSourceResult(request));
            }
            else if (Flag != null)
            {
                if (Flag == "0")
                {
                    var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);
                    var newList = reportlist.Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = Quater(x.QuaterId),
                        ContractId = x.ContractId,
                        ProjectCount = ProjectCount(x.QuaterId, BuilderId, x.ContractId),
                        SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                        estimatedactual = quaterestimated(x.ContractId, x.QuaterId),
                        percentage = quaterpercentage(x.ContractId, x.QuaterId)
                    }).OrderByDescending(x => x.Quater);

                    var histryList = newList.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                    return Json(histryList.ToDataSourceResult(request));
                }
                else
                {
                    Int64 ConId = Convert.ToInt32(Flag);
                    var reportlist = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ConId).GroupBy(x => x.QuaterId)
                                        .Select(group => group.First());

                    var newList = reportlist.Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = Quater(x.QuaterId),
                        ContractId = ContractId,
                        ProjectCount = ProjectCount(x.QuaterId, BuilderId, ConId),
                        SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                        estimatedactual = quaterestimated(ConId, x.QuaterId),
                        percentage = quaterpercentage(ConId, x.QuaterId)
                    }).OrderByDescending(x => x.Quater);

                    var histryList = newList.ToList()
                    .Select(x => new ProjectViewModel
                    {
                        QuaterId = x.QuaterId,
                        Quater = x.Quater,
                        ContractId = x.ContractId,
                        ProjectCount = x.ProjectCount,
                        SubmitDate = x.SubmitDate,
                        year = x.year,
                        estimatedactual = x.estimatedactual,
                        percentage = x.percentage,
                        rowcount = ProjectCounter++
                    }).ToList();

                    return Json(histryList.ToDataSourceResult(request));
                }
            }
            else if (Type == null)
            {
                var reportlist = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.ContractId == ContractId);

                var newList = reportlist.Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = Quater(x.QuaterId),
                    ContractId = ContractId,
                    ProjectCount = ProjectCount(x.QuaterId, BuilderId, ContractId),
                    SubmitDate = SubmitDate(x.BuilderQuaterAdminReportId.ToString()),
                    estimatedactual = quaterestimated(ContractId, x.QuaterId),
                    percentage = quaterpercentage(ContractId, x.QuaterId)
                }).OrderBy(x => x.Quater);


                var histryList = newList.ToList()
                .Select(x => new ProjectViewModel
                {
                    QuaterId = x.QuaterId,
                    Quater = x.Quater,
                    ContractId = x.ContractId,
                    ProjectCount = x.ProjectCount,
                    SubmitDate = x.SubmitDate,
                    year = x.year,
                    estimatedactual = x.estimatedactual,
                    percentage = x.percentage,
                    rowcount = ProjectCounter++
                }).ToList();

                return Json(histryList.ToDataSourceResult(request));
            }

            var value = "";
            return Json(value.ToDataSourceResult(request));
        }
        #endregion
        //public string quaterestimated(Int64 ContractId, Int64 QuaterId)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);
        //    string result = "";
        //    var Answar = "";
        //    Decimal value = 0;
        //    Decimal percentage = 0;
        //    var contractcompilancebuilder = _ObjContractCompliance.GetContractBuilderComplianceNew(ContractId, BuilderId);
        //    if (contractcompilancebuilder.Count() > 0) // Compliance override section
        //    {
        //        value = contractcompilancebuilder.FirstOrDefault().ActualValue;
        //        Answar = contractcompilancebuilder.FirstOrDefault().EstimatedValue.ToString();
        //        percentage = (value / Convert.ToDecimal(Answar)) * 100;
        //        result = value.ToString() + "/" + Answar + " Annually";
        //        return result;
        //    }
        //    else
        //    {

        //        Quater q = _ObjQuaterService.GetQuaterById(QuaterId);
        //        var Year = q.Year;

        //        DateTime currentdate = DateTime.Now.Date;


        //        var quaterlist = _ObjQuaterService.GetQuatersUptoCurrentQuater(QuaterId, Convert.ToInt64(Year)).Select(x => x.QuaterId);

        //        // Int64 BuilderId=2; //putnuilderid here

        //        /*Estimate Portion */
        //        int count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count();

        //        if (count > 0)
        //        {
        //            var QuestioId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().QuestionId;
        //            var SurveyId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().SurveyId;
        //            int count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).Count();
        //            if (count3 > 0)
        //            {
        //                Answar = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).FirstOrDefault().Answer;
        //                string estimated = Answar;
        //            }
        //        }

        //        /*End*/

        //        /*Actual Portion */
        //        int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
        //        if (count1 > 0)
        //        {

        //            List<string> questionlist = new List<string>();
        //            List<string> ActualAnswarlist = new List<string>();
        //            //var ActualQuestionId = _ObjContractCompliance.GetActualValueCompliance(ContractId).FirstOrDefault().QuestionId;
        //            foreach (var quater in quaterlist)
        //            {
        //                var QuestionId = _ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(quater)).FirstOrDefault().QuestionId;
        //                questionlist.Add(QuestionId.ToString());
        //            }

        //                if (_ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).Count() > 0)
        //                {
        //                    var BuilderQuaterContractProjectReportId = _ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).FirstOrDefault().BuilderQuaterContractProjectReportId;
        //                    foreach (var actualquestion in questionlist)
        //                    {
        //                        var ActualAnswar = _ObjBuilderQuaterContractProjectDetails.GetAllProjectDetailsOfContract(Convert.ToInt64(actualquestion), Convert.ToInt64(BuilderQuaterContractProjectReportId));
        //                        ActualAnswarlist.Add(ActualAnswar.ToString());
        //                    }



        //                    foreach (var item in ActualAnswarlist)
        //                    {
        //                        value = value + Convert.ToDecimal(item);

        //                    }
        //                    if (Answar != "")
        //                    {
        //                        percentage = (value / Convert.ToDecimal(Answar)) * 100;
        //                        result = value.ToString() + "/" + Answar + " Annually";
        //                    }
        //                    else
        //                    {
        //                        int ans = 0;
        //                        percentage = 0;
        //                        result = value.ToString() + "/" + ans.ToString() + " Annually";
        //                    }

        //                }

        //        }
        //    }
        //    return result;
        //}
        [Authorize(Roles = "Builder")]
        public string quaterestimated(Int64 ContractId, Int64 QuaterId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            string result = "";
            var Answar = "";
            Decimal value = 0;
            Decimal percentage = 0;
            var contractcompilancebuilder = _ObjContractCompliance.GetContractBuilderComplianceNew(ContractId, BuilderId);
            if (contractcompilancebuilder.Count() > 0) // Compliance override section
            {
                value = contractcompilancebuilder.FirstOrDefault().ActualValue;
                Answar = contractcompilancebuilder.FirstOrDefault().EstimatedValue.ToString();
                percentage = (value / Convert.ToDecimal(Answar)) * 100;
                result = value.ToString() + "/" + Answar + " Quaterly";
                return result;
            }
            else
            {

                Quater q = _ObjQuaterService.GetQuaterById(QuaterId);
                var Year = q.Year;

                DateTime currentdate = DateTime.Now.Date;



                List<Int64> quateridlist = new List<Int64>();

                //var quaterlist = _ObjQuaterService.GetQuatersUptoCurrentQuater(QuaterId, Convert.ToInt64(Year)).Select(x => x.QuaterId);
                //foreach(var item in quaterlist)
                //{
                //    quateridlist.Add(item);
                //}

                quateridlist.Add(QuaterId);

                // Int64 BuilderId=2; //putnuilderid here

                /*Estimate Portion */
                var count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId);

                if (_ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count() > 0)
                {
                    var QuestioId = count.FirstOrDefault().QuestionId;
                    var SurveyId = count.FirstOrDefault().SurveyId;
                    var count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId));
                    if (count3 != null)
                    {
                        Answar = count3.FirstOrDefault().Answer;
                        string estimated = Answar;
                    }
                }

                /*End*/

                /*Actual Portion */
                int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
                if (count1 > 0)
                {

                    List<Int64> questionlist = new List<Int64>();
                    List<Int64> ActualAnswarlist = new List<Int64>();

                    //foreach (var quater in quaterlist)
                    //{
                    if (_ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(QuaterId)).Count() > 0)
                    {
                        var QuestionId = _ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(QuaterId)).FirstOrDefault().QuestionId;
                        questionlist.Add(Convert.ToInt64(QuestionId));
                    }
                    // }
                    value = _ObjContractCompliance.GetBuilderActualValueWithQuater(BuilderId, ContractId, questionlist, quateridlist);
                    try
                    {
                        percentage = (value / Convert.ToDecimal(Answar)) * 100;
                        result = value.ToString() + "/" + Answar + " Quaterly";
                    }
                    catch
                    {

                        result = "Not Available";
                    }



                }
            }
            return result;
        }
        [Authorize(Roles = "Builder")]
        public string quaterpercentage(Int64 ContractId, Int64 QuaterId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            string result = "";
            var Answar = "";
            Decimal value = 0;
            Decimal percentage = 0;
            var contractcompilancebuilder = _ObjContractCompliance.GetContractBuilderComplianceNew(ContractId, BuilderId);
            if (contractcompilancebuilder.Count() > 0) // Compliance override section
            {
                value = contractcompilancebuilder.FirstOrDefault().ActualValue;
                Answar = contractcompilancebuilder.FirstOrDefault().EstimatedValue.ToString();
                percentage = (value / Convert.ToDecimal(Answar)) * 100;
                result = value.ToString() + "/" + Answar + " Annually";
                //percentage.ToString("#.##");
                return percentage.ToString("#.##");
            }
            else
            {

                Quater q = _ObjQuaterService.GetQuaterById(QuaterId);
                var Year = q.Year;

                DateTime currentdate = DateTime.Now.Date;



                List<Int64> quateridlist = new List<Int64>();
                quateridlist.Add(QuaterId);

                //var quaterlist = _ObjQuaterService.GetQuatersUptoCurrentQuater(QuaterId, Convert.ToInt64(Year)).Select(x => x.QuaterId);
                //foreach (var item in quaterlist)
                //{
                //    quateridlist.Add(item);
                //}



                /*Estimate Portion */
                var count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId);

                if (_ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count() > 0)
                {
                    var QuestioId = count.FirstOrDefault().QuestionId;
                    var SurveyId = count.FirstOrDefault().SurveyId;
                    var count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId));
                    if (_ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).Count() > 0)
                    {
                        Answar = count3.FirstOrDefault().Answer;
                        string estimated = Answar;
                    }
                }

                /*End*/

                /*Actual Portion */
                int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
                if (count1 > 0)
                {

                    List<Int64> questionlist = new List<Int64>();
                    List<Int64> ActualAnswarlist = new List<Int64>();

                    //foreach (var quater in quaterlist)
                    //{
                    if (_ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(QuaterId)).Count() > 0)
                    {
                        var QuestionId = _ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(QuaterId)).FirstOrDefault().QuestionId;
                        questionlist.Add(Convert.ToInt64(QuestionId));
                    }
                    //}

                    value = _ObjContractCompliance.GetBuilderActualValueWithQuater(BuilderId, ContractId, questionlist, quateridlist);
                    try
                    {
                        percentage = (value / Convert.ToDecimal(Answar)) * 100;
                        result = value.ToString() + "/" + Answar + " Annually";
                    }
                    catch
                    {
                        //percentage = 0;

                    }





                }
            }
            return percentage.ToString("#.##");
        }
        [Authorize(Roles = "Builder")]
        public string estimated(Int64 ContractId, Int64 QuaterId)
        {
            try
            {


                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
                Int64 BuilderId = Convert.ToInt64(bldid);
                string result = "";
                var Answar = "";
                Decimal value = 0;
                Decimal percentage = 0;
                var contractcompilancebuilder = _ObjContractCompliance.GetContractBuilderComplianceNew(ContractId, BuilderId);
                if (contractcompilancebuilder.Count() > 0) // Compliance override section
                {
                    value = contractcompilancebuilder.FirstOrDefault().ActualValue;
                    Answar = contractcompilancebuilder.FirstOrDefault().EstimatedValue.ToString();
                    percentage = (value / Convert.ToDecimal(Answar)) * 100;
                    result = value.ToString() + "/" + Answar + " Annually";
                    return result;
                }
                else
                {

                    Quater q = _ObjQuaterService.GetQuaterById(QuaterId);
                    var Year = q.Year;

                    DateTime currentdate = DateTime.Now.Date;


                    var quaterlist = _ObjQuaterService.GetQuatersUptoCurrentQuater(QuaterId, Convert.ToInt64(Year)).Select(x => x.QuaterId);
                    List<Int64> quateridlist = new List<Int64>();
                    bool chk = false;
                    foreach (var item in quaterlist)
                    {
                        int checkreportcount = _ObjQuaterContractProjectReport.CheckExistContractAgainstBuilderQuater(BuilderId, item, ContractId).Count();
                        if (checkreportcount > 0)
                        {
                            chk = true;
                        }
                        quateridlist.Add(item);
                    }
                    if (chk == false)
                    {
                        result = "Not Submitted";
                        return result;
                    }
                    // Int64 BuilderId=2; //putnuilderid here

                    /*Estimate Portion */
                    var count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId);

                    //if (_ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count() > 0)
                    try
                    {
                        if (count.Count() > 0)
                        {
                            var QuestioId = count.FirstOrDefault().QuestionId;
                            var SurveyId = count.FirstOrDefault().SurveyId;
                            var count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId));
                            if (count3.Count() > 0)
                            {
                                Answar = count3.FirstOrDefault().Answer;
                                string estimated = Answar;
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Answar = "";
                    }


                    /*End*/

                    /*Actual Portion */
                    int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
                    if (count1 > 0)
                    {

                        List<Int64> questionlist = new List<Int64>();
                        List<Int64> ActualAnswarlist = new List<Int64>();
                        //var ActualQuestionId = _ObjContractCompliance.GetActualValueCompliance(ContractId).FirstOrDefault().QuestionId;
                        foreach (var quater in quaterlist)
                        {
                            var actualvalueparquater = _ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(quater));
                            if (actualvalueparquater.Count() > 0)
                            {
                                var QuestionId = actualvalueparquater.FirstOrDefault().QuestionId;
                                questionlist.Add(Convert.ToInt64(QuestionId));
                            }
                        }
                        value = _ObjContractCompliance.GetBuilderActualValueWithQuater(BuilderId, ContractId, questionlist, quateridlist);
                        try
                        {
                            percentage = (value / Convert.ToDecimal(Answar)) * 100;
                            result = value.ToString() + "/" + Answar + " Annually";
                        }
                        catch
                        {

                            result = "Not Available";
                        }



                    }
                }

                return result;
            }
            catch (Exception ee)
            {
                return "";
            }
        }
        [Authorize(Roles = "Builder")]
        public string percentage(Int64 ContractId, Int64 QuaterId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            string result = "";
            var Answar = "";
            Decimal value = 0;
            Decimal percentage = 0;
            string msg = "";
            try
            {
                var contractcompilancebuilder = _ObjContractCompliance.GetContractBuilderComplianceNew(ContractId, BuilderId);
                if (contractcompilancebuilder.Count() > 0) // Compliance override section
                {
                    value = contractcompilancebuilder.FirstOrDefault().ActualValue;
                    Answar = contractcompilancebuilder.FirstOrDefault().EstimatedValue.ToString();
                    percentage = (value / Convert.ToDecimal(Answar)) * 100;
                    result = value.ToString() + "/" + Answar + " Annually";
                    return percentage.ToString("#.##");
                }
                else
                {

                    Quater q = _ObjQuaterService.GetQuaterById(QuaterId);
                    var Year = q.Year;

                    DateTime currentdate = DateTime.Now.Date;


                    var quaterlist = _ObjQuaterService.GetQuatersUptoCurrentQuater(QuaterId, Convert.ToInt64(Year)).Select(x => x.QuaterId);
                    List<Int64> quateridlist = new List<Int64>();

                    bool chk = false;
                    foreach (var item in quaterlist)
                    {
                        int checkreportcount = _ObjQuaterContractProjectReport.CheckExistContractAgainstBuilderQuater(BuilderId, item, ContractId).Count();
                        if (checkreportcount > 0)
                        {
                            chk = true;
                        }
                        quateridlist.Add(item);
                    }
                    if (chk == false)
                    {
                        result = " ";
                        return result;
                    }

                    //foreach (var item in quaterlist)
                    //{
                    //    quateridlist.Add(item);
                    //}

                    // Int64 BuilderId=2; //putnuilderid here

                    /*Estimate Portion */
                    var count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId);

                    if (count.Count() > 0)
                    {
                        var QuestioId = count.FirstOrDefault().QuestionId;
                        var SurveyId = count.FirstOrDefault().SurveyId;
                        var count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId));
                        if (count3.Count() > 0)
                        {
                            Answar = count3.FirstOrDefault().Answer;
                            string estimated = Answar;
                        }
                    }

                    /*End*/

                    /*Actual Portion */
                    int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
                    if (count1 > 0)
                    {

                        List<Int64> questionlist = new List<Int64>();
                        List<Int64> ActualAnswarlist = new List<Int64>();
                        //var ActualQuestionId = _ObjContractCompliance.GetActualValueCompliance(ContractId).FirstOrDefault().QuestionId;
                        foreach (var quater in quaterlist)
                        {
                            var GetActualValueCompliancePerQuater = _ObjContractCompliance.GetActualValueCompliancePerQuater(ContractId, Convert.ToInt64(quater));
                            if (GetActualValueCompliancePerQuater.Count() > 0)
                            {
                                var QuestionId = GetActualValueCompliancePerQuater.FirstOrDefault().QuestionId;
                                questionlist.Add(Convert.ToInt64(QuestionId));
                            }
                        }

                        value = _ObjContractCompliance.GetBuilderActualValueWithQuater(BuilderId, ContractId, questionlist, quateridlist);
                        try
                        {
                            percentage = (value / Convert.ToDecimal(Answar)) * 100;
                            result = value.ToString() + "/" + Answar + " Annually";
                        }
                        catch
                        {
                            //percentage = 0;

                        }





                    }
                }
            }
            catch (Exception ee)
            {
                msg = ee.Message.ToString();
            }

            return percentage.ToString("#.##");
        }

        //public string quaterpercentage(Int64 ContractId, Int64 QuaterId)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);

        //    // Int64 BuilderId = 2; //putnuilderid here
        //    int count = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).Count();
        //    var Answar = "";
        //    if (count > 0)
        //    {
        //        var QuestioId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().QuestionId;
        //        var SurveyId = _ObjContractCompliance.GetEstimatedValueCompliance(ContractId).FirstOrDefault().SurveyId;
        //        int count3 = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).Count();
        //        if (count3 > 0)
        //        {
        //            Answar = _ObjSurveyService.GetQuestionWiseBuilderSurveyResult(Convert.ToInt64(SurveyId), BuilderId, Convert.ToInt64(QuestioId)).FirstOrDefault().Answer;
        //            string estimated = Answar;
        //        }
        //    }
        //    int count1 = _ObjContractCompliance.GetActualValueCompliance(ContractId).Count();
        //    Decimal percentage = 0;
        //    if (count1 > 0)
        //    {
        //        var ActualQuestionId = _ObjContractCompliance.GetActualValueCompliance(ContractId).FirstOrDefault().QuestionId;
        //        //if (_ObjQuaterContractProjectReport.GetActiveQuaterRepotDetails(BuilderId, ContractId, QuaterId).Count() > 0)
        //        //{
        //        Decimal value = 0;
        //        Quater q = _ObjQuaterService.GetQuaterById(QuaterId);
        //        Int64 currentyear = q.Year;
        //        var listquaterid = _ObjQuaterService.GetQuatersUptoCurrentQuater(QuaterId, currentyear).Select(x => x.QuaterId);
        //        foreach (var individualquaterid in listquaterid)
        //        {
        //            if (_ObjQuaterContractProjectReport.GetActiveQuaterRepotDetails(BuilderId, ContractId, Convert.ToInt64(individualquaterid)).Count() > 0)
        //            {
        //                var BuilderQuaterContractProjectReportId = _ObjQuaterContractProjectReport.GetActiveQuaterRepotDetails(BuilderId, ContractId, Convert.ToInt64(individualquaterid)).FirstOrDefault().BuilderQuaterContractProjectReportId;
        //                var list = _ObjBuilderQuaterContractProjectDetails.GetAllProjectDetailsOfContract(Convert.ToInt64(ActualQuestionId), Convert.ToInt64(BuilderQuaterContractProjectReportId));

        //                foreach (var item in list)
        //                {
        //                    value = value + Convert.ToDecimal(item.Answer);

        //                }
        //            }
        //        }

        //        if (Answar != "")
        //        {

        //            percentage = (value / Convert.ToDecimal(Answar)) * 100;
        //        }
        //        else
        //        {
        //            percentage = 0;
        //        }
        //        string result = value.ToString() + "/" + Answar + "Annually";
        //        // }
        //    }
        //    return percentage.ToString();
        //}

        public string SubmitDate(string ReportId)
        {
            var date = _ObjQuaterAdminReportService.GetId(Convert.ToInt64(ReportId)).FirstOrDefault().SubmitDate;
            return date.ToString();
        }

        public string FindYear(Int64 QuaterId)
        {
            Quater q = _ObjQuaterService.GetQuaterById(QuaterId);
            string QuaterAndYear = q.Year.ToString();
            return QuaterAndYear;
        }

        public string Quater(Int64 QuaterId)
        {

            Quater q = _ObjQuaterService.GetQuaterById(QuaterId);
            string QuaterAndYear = q.QuaterName + "-" + q.Year;
            return QuaterAndYear;
        }

        public int ProjectCount(Int64 QuaterId, Int64 BuilderId, Int64 ContractId)
        {
            int Count = 0;
            Count = _ObjQuaterContractProjectReport.GetAllProjectCountForQuater(ContractId, BuilderId, QuaterId).Where(x => x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit).Count();    //Changed by Apala on 31.10.2017 - as requested by April
            return Count;
        }
        #region Old Code for RegularReporting
        //[Authorize(Roles = "Builder")]
        //public ActionResult RegularReporting()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);

        //    bool IsReportingPeriodOn = _ObjQuaterService.IsReportingPeriodOn(DateTime.Now);
        //    if (IsReportingPeriodOn)
        //    {
        //        ViewBag.EnableReporting = "";
        //    }
        //    else
        //    {
        //        ViewBag.EnableReporting = "disabled";
        //    }

        //    int ProjectCount = _ObjProjectService.GetBuilderProjectCount(BuilderId);
        //    if (ProjectCount == 0)
        //    {
        //        ViewBag.projectcount = "yes";
        //    }

        //    //Int64 BuilderId=2; //putbuilderid here
        //    DateTime currentdate = DateTime.Now.Date;
        //    var QuaterRecord = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
        //    ViewBag.QuaterName = QuaterRecord.QuaterName;
        //   // var Year = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().Year;
        //    ViewBag.Year = QuaterRecord.Year;
        //    //Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
        //    ViewBag.QuaterId = QuaterRecord.QuaterId;
        //    //var list = _ObjQuaterContractProjectReport.CheckAllContractReportSubmit(BuilderId, QuaterId).Select(x => x.IsComplete);
        //    //int count = _ObjQuaterContractProjectReport.CheckAllContractReportSubmit(BuilderId, QuaterId).Count();
        //    //if (count > 0)
        //    //{
        //    //    foreach (var item in list)
        //    //    {
        //    //        if (item == false)
        //    //        {
        //    //            ViewBag.Flag = "No";
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ViewBag.Flag = "No";
        //    //}

        //    // here have to open regular reporting if admin has re-opened the ncp survey - angshuman on 10-july-2017

        //    //var ActiveContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Select(x => x.ContractId);
        //    var ActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterRecord.QuaterId).Select(x => x.ContractId);
        //    int ContractCount = ActiveContractList.Count();
        //    if (ContractCount > 0)
        //    {
        //        foreach (var contractid in ActiveContractList)
        //        {
        //            var reportStatus = checkreportstatus(BuilderId, QuaterRecord.QuaterId, Convert.ToInt64(contractid));
        //            //if (reportStatus == "Edit" || reportStatus == "Continue")
        //            if (reportStatus != "Edit")
        //            {
        //                ViewBag.Flag = "No";
        //            }
        //            else
        //            {
        //                int Chkcount = _ObjQuaterContractProjectReport.CheckExistContractAgainstBuilderQuater(BuilderId, QuaterRecord.QuaterId, Convert.ToInt64(contractid)).Count();
        //                if (Chkcount > 0)
        //                {
        //                    int count = _ObjQuaterContractProjectReport.CheckCompleteContractAgainstBuilderQuater(BuilderId, QuaterRecord.QuaterId, Convert.ToInt64(contractid)).Count();
        //                    if (count > 0)
        //                    {
        //                        ViewBag.Flag = "No";
        //                    }
        //                }
        //                else
        //                {
        //                    ViewBag.Flag = "No";
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.data = "No";
        //    }
        //    bool check = _ObjQuaterAdminReportService.IsReportAllreadySubmited(BuilderId, QuaterRecord.QuaterId);
        //    if (check == true)
        //    {
        //        //var CheckAdminEdit = _ObjSurveyService.GetNCPSurveyResponseEditStatus(BuilderId, QuaterId, null).Select(x=> x.IsEditable).SingleOrDefault();
        //        //if(CheckAdminEdit != true)
        //        //{
        //        //    ViewBag.Flag = "No";
        //        //}
        //        ViewBag.Flag = "No";
        //        ViewBag.IsReportAllreadySubmited = "Yes";
        //    }
        //    else
        //        ViewBag.IsReportAllreadySubmited = "No";

        //    return View();
        //}
        #endregion
        #region New Code For RegularReporting By Rajendar 3/8/2018
        [Authorize(Roles = "Builder")]
        public ActionResult RegularReporting()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            bool IsReportingPeriodOn = _ObjQuaterService.IsReportingPeriodOn(DateTime.Now);
            if (IsReportingPeriodOn)
            {
                ViewBag.EnableReporting = "";
            }
            else
            {
                ViewBag.EnableReporting = "disabled";
            }
            int ProjectCount = _ObjProjectService.GetBuilderProjectCount(BuilderId);
            if (ProjectCount == 0)
            {
                ViewBag.projectcount = "yes";
            }
            DateTime currentdate = DateTime.Now.Date;
            var QuaterRecord = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
            ViewBag.QuaterName = QuaterRecord.QuaterName;
            ViewBag.Year = QuaterRecord.Year;
            ViewBag.QuaterId = QuaterRecord.QuaterId;
            var NeverReportForThisContractList= _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.QuarterContractStatusId == (Int64)QuarterContractStatusEnum.NeverReportForThisContract).Select(s=>s.ContractId);
            var ActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterRecord.QuaterId).Where(w=> !NeverReportForThisContractList.Contains(w.ContractId)).Select(x => x.ContractId);
            var ReportForThisContractListCount = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderQuater(BuilderId,QuaterRecord.QuaterId).Where(w => w.ProjectReportStatusId == (Int64)ProjectReportStatusEnum.Completed).Count();

            int ContractCount = ActiveContractList.Count();
            if (ContractCount > 0)
            {
                //foreach (var contractid in ActiveContractList)
                //{
                //    var reportStatus = CheckContractReportStatus(BuilderId, QuaterRecord.QuaterId, Convert.ToInt64(contractid));
                //    //if (reportStatus == "Edit" || reportStatus == "Continue")
                //    if (reportStatus != "Completed")
                //    {
                //        ViewBag.Flag = "No";
                //    }
                //    else
                //    {

                //    }
                //}
            }
            else
            {
                ViewBag.data = "No";
            }
            if(ActiveContractList.Count()!= ReportForThisContractListCount)
            {
                ViewBag.Flag="No";
            }
            bool check = _ObjQuaterAdminReportService.IsReportAllreadySubmited(BuilderId, QuaterRecord.QuaterId);
            if (check == true)
            {

                ViewBag.Flag = "No";
                ViewBag.IsReportAllreadySubmited = "Yes";
            }
            else
            {
                ViewBag.IsReportAllreadySubmited = "No";
            }

            return View();
        }
        #endregion

        public ActionResult NcpRebateReportHistory()
        {
            return View();
        }
        #region Old Code For GetNCPReportStatusCount
        //[Authorize(Roles = "Builder")]
        //public ActionResult ActiveContractListViewAscCon([DataSourceRequest] DataSourceRequest request)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);
        //    string RptStatus = "";
        //    //Int64 BuilderId = 2; //putbilderid here
        //    Int64 QuaterId = 0;
        //    DateTime currentdate = DateTime.Now.Date;

        //    try
        //    {
        //        var QuaterName = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterName;

        //        var Year = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().Year;

        //        QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;

        //        // var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderByDescending(x => x.ContractId).                

        //        // changes made by angshuman as requested by April(CBUSA) by email and slack dated 12-sep-2017. only active contracts should be displayed on builder's dashboard rebate port history.
        //        //var ActiveContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).OrderByDescending(x => x.ContractId).
        //        //var ActiveContractList = _ObjContractBuilderService.GetActiveContractsRegularReporting(BuilderId).OrderByDescending(x => x.ContractId).

        //        // Chnaged by Neyaz on 22-Nov-2017 ==== VSO#7109
        //        //var ActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReporting(BuilderId).OrderByDescending(x => x.ContractId).                
        //        // Chnaged by Neyaz on 14-Dec-2017 ==== VSO#9805
        //        //var ActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).OrderByDescending(x => x.ContractId).
        //        var AllActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).OrderByDescending(x => x.ContractId);
        //        int ContractCounter = 1;
        //        if (AllActiveContractList.Count() > 0)
        //        {
        //            ContractCounter = -(AllActiveContractList.Count() - 1);
        //        }
        //        var ActiveContractList = AllActiveContractList.
        //              Select(x => new ActiveContractViewModel
        //              {
        //                  rowcount = ContractCounter++,
        //                  ConractId = x.ContractId,
        //                  //ConractName = GetContractName(x.ContractId),
        //                  ConractName = x.ContractName,
        //                  ManuFacturerName = GetManufacturerName(x.ManufacturerId),
        //                  //Icon = GetContractIcon(x.ContractId),
        //                  Icon = GetContractIcon(x.ContractIcon),
        //                  ReportStatus = RptStatus = checkreportstatus(BuilderId, QuaterId, x.ContractId),
        //                  ReportingLabel = checkreportlabel(RptStatus, 1),
        //                  ReportingLabelColor = checkreportlabel(RptStatus, 2),
        //                  Estimated = estimated(x.ContractId, QuaterId),
        //                  Percentage = percentage(x.ContractId, QuaterId),
        //                  //CheckStatus = checkreportstatus(BuilderId, QuaterId, x.ContractId),
        //                  checksubmit = checksubmit(BuilderId, QuaterId)
        //              });
        //        var jsonResult = Json(ActiveContractList.ToDataSourceResult(request));
        //        jsonResult.MaxJsonLength = Int32.MaxValue;

        //        return jsonResult;
        //    }
        //    catch (Exception ee)
        //    {
        //        return Json("");
        //    }
        //}
        #endregion
        #region New Code For ActiveContractListViewAscCon By Rajendar 3/8/2018
        [Authorize(Roles = "Builder")]
        public ActionResult ActiveContractListViewAscCon([DataSourceRequest] DataSourceRequest request)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            string RptStatus = "";
            Int64 QuaterId = 0;
            DateTime currentdate = DateTime.Now.Date;

            try
            {
                var QuaterRecord = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
                var QuaterName = QuaterRecord.QuaterName;

                var Year = QuaterRecord.Year;

                QuaterId = QuaterRecord.QuaterId;
                var NeverReportForThisContractList = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.QuarterContractStatusId == (Int64)QuarterContractStatusEnum.NeverReportForThisContract).Select(s => s.ContractId);
                var AllActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).Where(w => !NeverReportForThisContractList.Contains(w.ContractId)).OrderByDescending(x => x.ContractId);
                int ContractCounter = 1;
                if (AllActiveContractList.Count() > 0)
                {
                    ContractCounter = -(AllActiveContractList.Count() - 1);
                }
                var ActiveContractList = AllActiveContractList.
                      Select(x => new ActiveContractViewModel
                      {
                          rowcount = ContractCounter++,
                          ConractId = x.ContractId,
                          //ConractName = GetContractName(x.ContractId),
                          ConractName = x.ContractName,
                          ManuFacturerName =x.Manufacturer.ManufacturerName,
                        
                          Icon = GetContractIcon(x.ContractIcon),
                          ReportStatus = RptStatus = CheckContractReportStatus(BuilderId, QuaterId, x.ContractId),
                          ReportingLabel = checkreportlabel(RptStatus, 1, checksubmit(BuilderId, QuaterId)),
                          ReportingLabelColor = checkreportlabel(RptStatus, 2, ""),
                          Estimated = estimated(x.ContractId, QuaterId),
                          Percentage = percentage(x.ContractId, QuaterId),
                         
                          checksubmit = checksubmit(BuilderId, QuaterId)
                      });
                var jsonResult = Json(ActiveContractList.ToDataSourceResult(request));
                jsonResult.MaxJsonLength = Int32.MaxValue;

                return jsonResult;
            }
            catch (Exception ee)
            {
                return Json("");
            }
        }
        public bool CheckProjectReportStatus(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {

            var PreviousQuaterList = _ObjQuaterService.GetAllPreviousQuater(QuaterId).Select(x => x.QuaterId).ToList<Int64>();

            int ProjListCount = _ObjProjectService.GetBuilderProject(ContractId, BuilderId, PreviousQuaterList).Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => new { ProjectId = x.ProjectId, ProjectName = x.ProjectName }).Count();
            int QuaterContractProjectReportCount = _ObjQuaterContractProjectReport.CheckCompleteBuilderQuaterContractProjectReport(BuilderId, QuaterId, ContractId).Count();
            if(ProjListCount==QuaterContractProjectReportCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
            #endregion
            public string checksubmit(Int64 BuilderId, Int64 QuaterId)
        {
            // here have to open regular reporting if admin has re-opened the ncp survey - angshuman on 10-july-2017
            string complete = "";
            try
            {
                bool check = _ObjQuaterAdminReportService.IsReportAllreadySubmited(BuilderId, QuaterId);

                if (check == true)
                {
                    //var CheckAdminEdit = _ObjSurveyService.GetNCPSurveyResponseEditStatus(BuilderId, QuaterId, null).Select(x => x.IsEditable).SingleOrDefault();
                    //if (CheckAdminEdit != true)
                    //{
                    //    complete = "Yes";
                    //}
                    complete = "Yes";
                }
            }
            catch (Exception ee)
            {
                complete = "";
            }

            return complete;
        }

        public string GetContractIcon(byte[] Icon)
        {
            string LogoImageBase64 = "";
            try
            {
                byte[] ContractIcon = Icon;
                if (ContractIcon != null)
                {
                    LogoImageBase64 = Convert.ToBase64String(ContractIcon);
                    LogoImageBase64 = string.Format("data:image/png;base64,{0}", LogoImageBase64);
                }
            }
            catch (Exception ee)
            {
                LogoImageBase64 = "";
            }

            return LogoImageBase64;
        }

        //public string GetContractIcon(Int64 ContractId)
        //{


        //    string LogoImageBase64 = "";
        //    byte[] ContractIcon = _ObjContractService.GetContract(ContractId).ContractIcon;
        //    if (ContractIcon != null)
        //    {
        //         LogoImageBase64 = Convert.ToBase64String(ContractIcon);
        //        LogoImageBase64 = string.Format("data:image/png;base64,{0}", LogoImageBase64);
        //    }

        //    return LogoImageBase64;
        //}

        public string GetContractName(Int64 ContractId)
        {

            string ContractName = _ObjContractService.GetContract(ContractId).ContractName;
            return ContractName;
        }

        public string GetManufacturerName(Int64? ManufaturerId)
        {
            if (ManufaturerId > 0)
            {
                string ManufacturerName = _ObjManufactureService.GetManufacturer().Where(x => x.ManufacturerId == ManufaturerId).FirstOrDefault().ManufacturerName;
                return ManufacturerName;
            }
            else
                return "";
        }

        public ActionResult LoadManageReport()
        {
            DateTime currentdate = DateTime.Now.Date;
            var QuaterRecord = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
            ViewBag.QuaterName = QuaterRecord.QuaterName;
            //var Year = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().Year;
            ViewBag.Year = QuaterRecord.Year;
            //Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;
            ViewBag.QuaterId = QuaterRecord.QuaterId;
            return PartialView("_ManageReport");
        }

        public ActionResult ProjectPopUp()
        {
            return PartialView("_ProjectStatusPop");
        }

        public string checkreportstatus(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            string status = "";
            try
            {
                //int count = _objquatercontractprojectreport.checkexistcontractagainstbuilderquater(builderid, quaterid, contractid).count();
                var data = _ObjQuaterContractProjectReport.CheckExistContractAgainstBuilderQuater(BuilderId, QuaterId, ContractId).Count();
                if (data > 0)
                {
                    //Changed by Apala for VSO#7118

                    bool flgNoProjStatusOrAnswer = false;
                    bool flgAllPrjNTRTQ = false;
                    bool flagAllPrjStsDone = false;
                    int NTRTQcount = 0;
                    int PrjCount = 0;
                    var ProjectList = _ObjProjectService.BuilderProjectList(BuilderId).Where(p => p.RowStatusId == (int)RowActiveStatus.Active);
                    // changes made by angshuman on 31/1/2018.
                    foreach (Project prj in ProjectList)
                    {
                        var ContractProjectReportStatus = _ObjQuaterContractProjectReport.CheckExistAllContractAgainstBuilderQuater(BuilderId, QuaterId, ContractId, prj.ProjectId);
                        var CheckIfNeverReportProject = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId).Where(x => x.ProjectId == prj.ProjectId && x.ProjectStatusId == 3);
                        var CheckIfProjectReportedInPrevQ = _ObjQuaterContractProjectReport.GetActiveRepotDetails(BuilderId, ContractId).Where(x => x.ProjectId == prj.ProjectId && x.ProjectStatusId == 1);
                        var CheckIfAllPrjNTRTQ = _ObjQuaterContractProjectReport.CheckExistAllContractAgainstBuilderQuater(BuilderId, QuaterId, ContractId, prj.ProjectId).Where(x => x.ProjectStatusId == 2);
                        if (CheckIfProjectReportedInPrevQ.Count() == 0 && CheckIfNeverReportProject.Count() == 0 && ContractProjectReportStatus.Count() == 0)
                        {
                            flgNoProjStatusOrAnswer = true;
                            break;
                        }
                        if (CheckIfAllPrjNTRTQ.Count() > 0)
                        {
                            NTRTQcount = (NTRTQcount + 1);
                        }
                        if ((CheckIfNeverReportProject.Count() + CheckIfProjectReportedInPrevQ.Count() + CheckIfAllPrjNTRTQ.Count()) > 0)
                        {
                            PrjCount = PrjCount + 1;
                        }
                    }

                    if (ProjectList.Count() == NTRTQcount)
                    {
                        flgAllPrjNTRTQ = true;
                    }
                    if (ProjectList.Count() == PrjCount)
                    {
                        flagAllPrjStsDone = true;
                    }

                    if (flgNoProjStatusOrAnswer == false)
                    {
                        var NoDataOrFileProjectCount = _ObjQuaterContractProjectReport.CheckExistContractAgainstBuilderQuater(BuilderId, QuaterId, ContractId);
                        //.Select(x => x.BuilderQuaterContractProjectReportId).ToList();

                        bool flgNoAnswerOrFileFound = false;

                        //for (var i = 0; i < NoDataOrFileProjectCount.Count(); i++)
                        foreach (BuilderQuaterContractProjectReport bqcpr in NoDataOrFileProjectCount)
                        {
                            var BQCPRId = bqcpr.BuilderQuaterContractProjectReportId; // NoDataOrFileProjectCount[i];
                            var ProjStatus = bqcpr.ProjectStatusId;

                            if (ProjStatus == 1 && bqcpr.IsComplete == false)
                            {
                                flgNoAnswerOrFileFound = true;
                            }
                            else
                            {
                                if (ProjStatus == 1)
                                {
                                    var ProjectDetails = _ObjBuilderQuaterContractProjectDetails.GetProjectDetailsForBuilderQuaterContractProjectReport(BQCPRId);

                                    if (ProjectDetails.Count() == 0)
                                    {
                                        flgNoAnswerOrFileFound = true;
                                        break;
                                    }
                                    else
                                    {
                                        foreach (BuilderQuaterContractProjectDetails bqcpd in ProjectDetails)
                                        {
                                            var q = _ObjQuestionService.GetQuestion(bqcpd.QuestionId);

                                            if ((bqcpd.Answer == null || bqcpd.Answer == "") || (q.IsFileNeedtoUpload == true && bqcpd.FileName == null))
                                            {
                                                flgNoAnswerOrFileFound = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (flgNoAnswerOrFileFound == true)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (flgNoAnswerOrFileFound == true)
                        {
                            flgNoProjStatusOrAnswer = true;
                        }
                    }

                    if (flagAllPrjStsDone == false)
                    {
                        status = "Set Statuses"; // "Report";
                    }
                    else if (flgNoProjStatusOrAnswer == true)
                    {
                        status = "Continue";
                    }
                    else if (flgAllPrjNTRTQ == true)
                    {
                        status = "Edit";
                    }
                    else
                    {
                        status = "Edit";
                    }

                    return status;

                    //var list = data.Select(x => x.IsComplete);
                    //foreach (var item in list)
                    //{
                    //    if (item == true)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        status = "Edit";
                    //    }
                    //}
                }
                else
                {
                    status = "Select Projects"; //"Report"
                }
            }
            catch (Exception ee)
            {
                status = "";
            }

            return status;
        }

        public string checkreportlabel(string ReportingButtonStatus, int labelorcolor, string CheckSubmit)
        {
            string status = "";
            if (ReportingButtonStatus != null)
            {
                if (ReportingButtonStatus == "Select Projects")
                {
                    if (labelorcolor == 1)
                        status = "Not Started";
                    else
                        status = "Red";
                }
                else if (ReportingButtonStatus == "Set Statuses") //Report
                {
                    if (labelorcolor == 1)
                        status = "Project Statuses Required"; // "Project Statuses Set";
                    else
                        status = "Orange";
                }
                else if (ReportingButtonStatus == "Continue")
                {
                    if (labelorcolor == 1)
                        status = "Reporting in Progress";
                    else
                        status = "Orange";
                }
                else if (ReportingButtonStatus == "Submit")
                {
                    if (labelorcolor == 1)
                        status = "Ready to Submit";
                    else
                        status = "Blue";
                }
                else if (ReportingButtonStatus == "Edit")
                {
                    if (labelorcolor == 1)
                    {
                        if (CheckSubmit == "Yes")
                        {
                            status = "Reporting Completed";
                        }
                        else
                        {
                            status = "Report Ready to Submit";
                        }
                    }
                    else
                        status = "Green";
                }
                else if (ReportingButtonStatus == "Complete") // For current case only (discripency :Submit, Edit )
                {
                    if (labelorcolor == 1)
                        status = "Reporting Completed";
                    else
                        status = "Green";
                }
                else
                {
                    if (labelorcolor == 1)
                        status = "Not Started";
                    else
                        status = "Red";
                }
            }
            return status;
        }

        [Authorize(Roles = "Builder")]
        public ActionResult AddProjectStatus(Int64 ContractId)
        {
            ViewBag.Contract = ContractId;
            var Contract = _ObjContractService.GetContract(ContractId);
            if (Contract != null)
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                string Id = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
                Int64 BuilderId = Convert.ToInt64(Id);
                AddProjectStatusViewModel ObjAddProjectVm = new AddProjectStatusViewModel();
                ObjAddProjectVm.BuilderId = BuilderId;
                ObjAddProjectVm.ContractId = ContractId;

                ObjAddProjectVm.ContractName = Contract.ContractName;
                ObjAddProjectVm.ContractIcon = Contract.ContractIcon;
                ObjAddProjectVm.ManuFacturerName = Contract.Manufacturer.ManufacturerName;

                DateTime currentdate = DateTime.Now.Date;
                Quater ObjQuater = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
                var QuaterName = ObjQuater.QuaterName;
                ObjAddProjectVm.QuaterName = QuaterName;
                var Year = ObjQuater.Year;
                ObjAddProjectVm.Year = Year.ToString();
                Int64 QuaterId = ObjQuater.QuaterId;
                ObjAddProjectVm.QuaterId = QuaterId;

                

                var QuestionList = _ObjQuestionService.GetBuilderReportQuestion(ContractId, ObjQuater.QuaterName, ObjQuater.Year.ToString());
              

                ViewBag.IsSurveyPublish = 1;

                return View(ObjAddProjectVm);

            }
            return View(new AddProjectStatusViewModel { });
        }

        [Authorize(Roles = "Builder")]
        public ActionResult ReportHistory(int ContractId)
        {
            var GetBuilderofContractList = _ObjContractBuilderService.GetBuilderofContract(ContractId);
            if (GetBuilderofContractList.Count() > 0)
            {
                var joiningdate = GetBuilderofContractList.FirstOrDefault().JoiningDate;
                ViewBag.joining = joiningdate;
            }
            Session["Contract"] = ContractId;
            var contractname = _ObjContractService.GetContract(ContractId);
            ViewBag.ContractName = contractname.ContractName;
            ViewBag.ContractIcon = contractname.ContractIcon;

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);
            Int64 conid = 0;
            Int32 ContractIndex = 0;
            var ContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).OrderBy(x => x.Contract.ContractName)
                .Select(x => new { ContractId = x.Contract.ContractId, ContractName = x.Contract.ContractName }).ToList();
            Int32 Cntr = 0;
            foreach (var data in ContractList)
            {
                Cntr += 1;
                if (data.ContractId == ContractId)
                {
                    ContractIndex = Cntr;
                }
            }

            ViewBag.FilterIndex = ContractIndex;
            DateTime currentdate = DateTime.Now.Date;
            var QuaterRecord = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
            var QuaterName = QuaterRecord.QuaterName;
            ViewBag.QuaterName = QuaterName;
            var Year = QuaterRecord.Year;
            ViewBag.Year = Year;
            Int64 QuaterId = QuaterRecord.QuaterId;
            ViewBag.QuaterId = QuaterId;
            ViewBag.ContractId = ContractId;
            return View();
        }
        #region Old Code For GetNCPReportStatusCount
        //public string GetNCPReportStatusCount()
        //{
        //    string ReportStatus;

        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
        //    Int64 BuilderId = Convert.ToInt64(bldid);

        //    DateTime currentdate = DateTime.Now.Date;
        //    Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;

        //    var QuarterRebateReportStatus = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, QuaterId).Where(x => x.IsSubmit == true && x.RowStatusId == (int)RowActiveStatus.Active).Count();

        //    //var TotalProjectContracts = _ObjQuaterContractProjectReport.CheckAllContractForBuilderQuarter(BuilderId, QuaterId).Select(x => x.ContractId).Distinct().Count();
        //    //========= Neyaz == on 30-Nov-2017 ====== VSO#9397 ====== Start
        //    //var ActiveContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Select(x => x.ContractId);
        //    var ActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).Select(x => x.ContractId);
        //    //========= Neyaz == on 30-Nov-2017 ====== VSO#9397 ====== End
        //    int TotalContractCount = ActiveContractList.Count();
        //    int CompletedContracts = 0;
        //    int IncompleteContracts = 0;
        //    #region Area to Change By Rajendar
        //    if (TotalContractCount > 0)
        //    {
        //        foreach (var contractid in ActiveContractList)
        //        {
        //            var reportStatus = checkreportstatus(BuilderId, QuaterId, Convert.ToInt64(contractid));

        //            if (reportStatus == "Edit")
        //            {
        //                CompletedContracts += 1;
        //            }
        //            else if (reportStatus == "Continue")
        //            {
        //                IncompleteContracts += 1;
        //            }
        //        }
        //    }
        //    #endregion
        //    ReportStatus = String.Concat(QuarterRebateReportStatus.ToString(), "/", CompletedContracts.ToString(), "/", TotalContractCount.ToString(), "/", IncompleteContracts.ToString());
        //    //ReportStatus = "0/3/12/0";
        //    return ReportStatus;
        //}
        #endregion
        #region New Code For GetNCPReportStatusCount By Rajendar 3/8/2018
        [Authorize(Roles = "Builder")]
        public string GetNCPReportStatusCount()
        {
            string ReportStatus;

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            DateTime currentdate = DateTime.Now.Date;
            Int64 QuaterId = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault().QuaterId;

            var QuarterRebateReportStatus = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, QuaterId).Where(x => x.IsSubmit == true && x.RowStatusId == (int)RowActiveStatus.Active).Count();

            //var TotalProjectContracts = _ObjQuaterContractProjectReport.CheckAllContractForBuilderQuarter(BuilderId, QuaterId).Select(x => x.ContractId).Distinct().Count();
            //========= Neyaz == on 30-Nov-2017 ====== VSO#9397 ====== Start
            //var ActiveContractList = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Select(x => x.ContractId);
           //var ActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).Select(x => x.ContractId);
            //========= Neyaz == on 30-Nov-2017 ====== VSO#9397 ====== End
            
            var NeverReportForThisContractList = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(w => w.QuarterContractStatusId == (Int64)QuarterContractStatusEnum.NeverReportForThisContract).Select(s => s.ContractId);
            var ActiveContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).Where(w => !NeverReportForThisContractList.Contains(w.ContractId)).OrderByDescending(x => x.ContractId);
            int TotalContractCount = ActiveContractList.Count();
            int CompletedContracts = 0;
            int IncompleteContracts = 0;
            var ActiveContractStatusList = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderQuater(BuilderId, QuaterId);
            if (ActiveContractStatusList != null)
            {
                if (ActiveContractStatusList.Count() > 0)
                {
                    CompletedContracts = ActiveContractStatusList.Where(w => w.ProjectReportStatusId == (Int64)ProjectReportStatusEnum.Completed).Count();
                    IncompleteContracts = ActiveContractStatusList.Where(w => w.ProjectReportStatusId == (Int64)ProjectReportStatusEnum.InProgress).Count();
                }
                else
                {
                    CompletedContracts = 0;
                    IncompleteContracts = 0;
                }
            }
            else
            {
                CompletedContracts = 0;
                IncompleteContracts = 0;
            }

            ReportStatus = String.Concat(QuarterRebateReportStatus.ToString(), "/", CompletedContracts.ToString(), "/", TotalContractCount.ToString(), "/", IncompleteContracts.ToString());
            //ReportStatus = "0/3/12/0";
            return ReportStatus;
        }
        #endregion
        public ActionResult LoadContractResourceWithCategory(Int64 ContractResourceId)
        {
            return PartialView("_ContractResourceCategory");
        }

        [Authorize(Roles = "Builder")]
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
        #region Changed By Rajendar
        public string CheckContractReportStatus(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            string status = string.Empty;
            var Resultset= _ObjBuilderQuaterContractStatusService.CheckExistContractAgainstBuilderQuater(BuilderId, QuaterId, ContractId).FirstOrDefault();
            if(Resultset!=null)
            {
                switch (Resultset.ProjectReportStatusId)
                {
                    case (Int64)ProjectReportStatusEnum.Completed:
                        if(Resultset.QuarterContractStatusId==(Int64)QuarterContractStatusEnum.NothingToReportThisQuarter)
                        {
                            status = "Edit";
                        }
                        else
                        {
                            status = "Edit";
                        }
                      
                        break;
                    case (Int64)ProjectReportStatusEnum.InProgress:
                        status = "Continue";
                        break;
                    case (Int64)ProjectReportStatusEnum.NotStarted:
                        status = "Select Projects";
                        break;
                    default:
                        status = "Select Projects";
                        break;
                }
            }
            else
            {
                status = "Select Projects";
            }

            return status;
        }
        
            #endregion
        }
}