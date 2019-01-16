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
using System.Configuration;

namespace CBUSA.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ContractController : Controller
    {
        // GET: Admin/Contract
        readonly IContractStatusService _ObjContractStatusServices;
        readonly IResourceCategoryService _ObjResourceCategoryService;
        readonly IResourceService _ObjResourceService;
        readonly IManufacturerService _ObjManufactureService;
        readonly IContractServices _ObjContractService;
        readonly IContractRebateService _ObjContractRebateService;
        readonly IContractBuilderService _ObjContractBuilderService;
        readonly ISurveyService _ObjSurveyService;
        readonly IBuilderService _ObjBuilderService;

        public ContractController(IContractStatusService ObjContractStatusServices, IResourceCategoryService ObjResourceCategoryService,
            IResourceService ObjResourceService, IManufacturerService ObjManufactureService,
            IContractServices ObjContractService, IContractRebateService ObjContractRebateService, IContractBuilderService ObjContractBuilderService,
            ISurveyService ObjSurveyService, IBuilderService ObjBuilderService
            )
        {
            _ObjContractStatusServices = ObjContractStatusServices;
            _ObjResourceCategoryService = ObjResourceCategoryService;
            _ObjResourceService = ObjResourceService;
            _ObjManufactureService = ObjManufactureService;
            _ObjContractService = ObjContractService;
            _ObjContractRebateService = ObjContractRebateService;
            _ObjContractBuilderService = ObjContractBuilderService;
            _ObjSurveyService = ObjSurveyService;
            _ObjBuilderService = ObjBuilderService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ActiveContract()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        #region Active Contract
        public ActionResult ActiveContract_Read([DataSourceRequest] DataSourceRequest request)
        {

            //  var ActiveContractList = _ObjContractService.GetActiveContract();
            var ActiveContractList = _ObjContractService.GetActiveContract().Select(
                x => new ActiveContractViewModel
                {
                    ConractId = 1,
                    ConractName = "Office Supplies",
                    ManuFacturerName = "Point Nationwide",
                    ContractStatus = "Active",
                    ContractFrom = "07/15/2016",
                    ContractTo = "08/25/2016"

                });


            //_ObjResourceCategoryService.GetResourceCategoryAll().Where(x => x.RowStatusId == (int)RowActiveStatus.Active)
            //.OrderByDescending(x => x.ResourceCategoryId)
            //.Select(z => new ResourceCategoryViewModel
            //{
            //    ResourceCategoryId = z.ResourceCategoryId,
            //    ResourceCategoryName = z.ResourceCategoryName

            //});
            return Json(ActiveContractList.ToDataSourceResult(request));
        }

        #endregion

        #region Contract
        public JsonResult GetManufacturer()
        {

            var ManufacturerList = _ObjManufactureService.GetManufacturer().Where(x => x.RowStatusId == (int)RowActiveStatus.Active)
                .Select(x => new { ManufacturerId = x.ManufacturerId, ManufacturerName = x.ManufacturerName }).OrderBy(x => x.ManufacturerName);
            return Json(ManufacturerList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsContractNameAvailable(string ContractName)
        {

            bool IsAvail = _ObjContractService.IsContractNameAvailable(ContractName);
            return Json(new { IsContractNameAvailable = IsAvail }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsContractLabelAvailable(string ContractLabelName)
        {

            bool IsAvail = _ObjContractService.IsContractLabelAvailable(ContractLabelName);
            return Json(new { IsContractLabelAvailable = IsAvail }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveContract(ContractViewModel _ObjContractVM)
        {

            //   int Flag = 0;

            if (_ObjContractVM.Products != null)
            {
                bool IsValid = !_ObjContractVM.Products.Split(',').ToArray().Where(x => string.IsNullOrEmpty(x)).Any();
                if (!IsValid)
                    ModelState.AddModelError("", "Please enter product in correct format");

            }



            if (_ObjContractVM.ContractStatusId == 1) ///for active Status Id
            {
                if (_ObjContractVM.ContractDeliverables == string.Empty)
                {
                    ModelState.AddModelError("", "Contract deliverables are required");
                }


                if (_ObjContractVM.ContractFrom.HasValue && _ObjContractVM.ContractTo.HasValue)
                {


                    if (_ObjContractVM.ContractFrom >= _ObjContractVM.ContractTo)
                    {
                        ModelState.AddModelError("", "Invalid start and end date");
                    }
                    if (_ObjContractVM.ContractTo <= DateTime.Now)
                    {
                        ModelState.AddModelError("", "Invalid end date");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid start and end date");
                }

                if (_ObjContractVM.ManufacturerId == 0)
                {
                    ModelState.AddModelError("", "Manufacturer Required");
                }



            }
            if (_ObjContractVM.ContractStatusId != 1) ///for active Status Id
            {
                if (_ObjContractVM.EstimatedStartDate.HasValue && _ObjContractVM.EntryDeadline.HasValue)
                {
                    if (_ObjContractVM.EstimatedStartDate < DateTime.Now)
                    {
                        ModelState.AddModelError("", "Invalid 'Estimated start date");
                    }
                    if (_ObjContractVM.EntryDeadline < DateTime.Now)
                    {
                        ModelState.AddModelError("", " Invalid 'Early bird entry deadline");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid EstimatedStart and EntryDeadline date");
                }



            }

            if (ModelState.IsValid)
            {

                byte[] LogoImageByte = null;
                // string LogoImageBase64 = "";
                HttpFileCollectionBase files = Request.Files;

                for (int i = 0; i < files.Count; i++)
                {
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    HttpPostedFileBase file = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    // Get the complete folder path and store the file inside it.  
                    // fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                    // file.SaveAs(fname);
                    BinaryReader rdr = new BinaryReader(file.InputStream);
                    LogoImageByte = rdr.ReadBytes((int)file.ContentLength);

                }










                Contract _ObjContract = new Contract();

                _ObjContract.ContractName = _ObjContractVM.ContractName;
                _ObjContract.Label = _ObjContractVM.Label;
                _ObjContract.ContractStatusId = _ObjContractVM.ContractStatusId;
                _ObjContract.Website = _ObjContractVM.Website;
                _ObjContract.ContractIcon = LogoImageByte;



                if (_ObjContractVM.ContractStatusId == 1) ///for active Status Id
                {
                    _ObjContract.ContrctFrom = _ObjContractVM.ContractFrom;
                    _ObjContract.ContrctTo = _ObjContractVM.ContractTo;
                    _ObjContract.ContractDeliverables = _ObjContractVM.ContractDeliverables;
                    _ObjContract.ContractDeliverables = _ObjContractVM.ContractDeliverables;
                    _ObjContract.ManufacturerId = _ObjContractVM.ManufacturerId;
                }
                else
                {
                    _ObjContract.EstimatedStartDate = _ObjContractVM.EstimatedStartDate;
                    _ObjContract.EntryDeadline = _ObjContractVM.EntryDeadline;
                    _ObjContract.PrimaryManufacturer = _ObjContractVM.PrimaryManufacturer;

                }
                _ObjContract.IsReportable = true;
                _ObjContract.RowStatusId = (int)RowActiveStatus.Active;
                _ObjContract.CreatedBy = 1;
                _ObjContract.ModifiedBy = 1;
                _ObjContract.RowGUID = Guid.NewGuid();

                List<string> ProductList = new List<string>();

                if (_ObjContractVM.Products != "")
                {
                    string[] Products = _ObjContractVM.Products.Split(',');
                    for (int i = 0; i < Products.Length; i++)
                    {
                        ProductList.Add(Products[i]);
                    }
                }


                ContractStatusHistory _ObjContractStatusHistory = new ContractStatusHistory();
                _ObjContractStatusHistory.ContractStatusId = _ObjContractVM.ContractStatusId;
                _ObjContractStatusHistory.EntryDate = DateTime.Now;


                List<Resource> ObjResourceList = new List<Resource>();
                if (_ObjContractVM.DumpId != string.Empty)
                {
                    ObjResourceList = _ObjContractService.GetResourceofDump(_ObjContractVM.DumpId).ToList();
                }


                _ObjContractService.SaveContract(_ObjContract, ProductList, _ObjContractStatusHistory, ObjResourceList);


                //ModelState.AddModelError("", "Please select atleast on market");


                //string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                //       .SelectMany(E => E.Errors)
                //       .Select(E => E.ErrorMessage)
                //       .ToArray();

                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                 .SelectMany(E => E.Errors)
                 .Select(E => E.ErrorMessage)
                 .ToArray();


                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region Contract Manage status

        public ActionResult LoadManageStatus()
        {
            return PartialView("_ManageStatus");
        }
        [HttpPost]
        public JsonResult ContractStatus_Create(ContractStatusViewModel ObjContractStaus)
        {

            if (ModelState.IsValid)
            {
                ContractStatus _ObjContractStatus = new ContractStatus();
                _ObjContractStatus.ContractStatusName = ObjContractStaus.ContractStatusName;
                _ObjContractStatus.RowStatusId = (int)RowActiveStatus.Active;
                _ObjContractStatus.IsNonEditable = false;
                _ObjContractStatus.Order = 1;
                _ObjContractStatus.CreatedBy = 1;
                _ObjContractStatus.ModifiedBy = 1;
                _ObjContractStatusServices.SaveContractStatus(_ObjContractStatus);
                return Json(new { IsSuccess = true });
            }
            return Json(new { IsSuccess = false });
        }
        public ActionResult ContractStatus_Read([DataSourceRequest] DataSourceRequest request)
        {
            var UseContractStatusList = _ObjContractStatusServices.GetUseContractStatus();
            var ContractStatusList = _ObjContractStatusServices.GetContractStatusAll().Where(x => x.RowStatusId == (int)RowActiveStatus.Active)
                .OrderBy(x => x.Order).Select(z => new ContractStatusViewModel
                {
                    ContractStatusId = z.ContractStatusId,
                    ContractStatusName = z.ContractStatusName,
                    IsNonEditable = z.IsNonEditable,
                    IsNonDeletable = UseContractStatusList.Where(x => x.ContractStatusId == z.ContractStatusId).Count() > 0 ? true : false
                });
            return Json(ContractStatusList.ToDataSourceResult(request));
        }
        public ActionResult ContractStatus_Edit([DataSourceRequest] DataSourceRequest request, ContractStatusViewModel ObjContractStatus)
        {
            ContractStatus _ObjContractStatus = _ObjContractStatusServices.GetContractStatus(ObjContractStatus.ContractStatusId);
            _ObjContractStatus.ContractStatusName = ObjContractStatus.ContractStatusName;
            _ObjContractStatus.ModifiedOn = DateTime.Now;
            _ObjContractStatus.ModifiedBy = 1;
            _ObjContractStatusServices.EditContractStatus(_ObjContractStatus);



            return Json(new[] { ObjContractStatus }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult ContractStatus_Delete([DataSourceRequest] DataSourceRequest request, ContractStatusViewModel ObjContractStatus)
        {
            ContractStatus _ObjContractStatus = _ObjContractStatusServices.GetContractStatus(ObjContractStatus.ContractStatusId);

            _ObjContractStatus.RowStatusId = (int)RowActiveStatus.Archived;
            _ObjContractStatus.ModifiedOn = DateTime.Now;
            _ObjContractStatus.ModifiedBy = 1;
            _ObjContractStatusServices.DeleteContractStatus(_ObjContractStatus);

            return Json(new[] { ObjContractStatus }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult GetContractStatus()
        {
            var ContractStatusList = _ObjContractStatusServices.GetContractStatusAll().Where(x => x.RowStatusId == (int)RowActiveStatus.Active)
                .OrderBy(x => x.Order).Select(z => new ContractStatusViewModel
                {
                    ContractStatusId = z.ContractStatusId,
                    ContractStatusName = z.ContractStatusName

                });
            return Json(ContractStatusList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Contract Resource

        public ActionResult LoadContractResource(Int64 ContractResourceId)
        {
            if (ContractResourceId > 0)
            {
                var Obj = _ObjResourceService.GetResource(ContractResourceId);

                ResourceViewModel ObjVm = new ResourceViewModel();
                ObjVm.ResourceCategoryId = Obj.ResourceCategoryId;
                ObjVm.Title = Obj.Title;
                ObjVm.Description = Obj.Description;
                ObjVm.ResourceId = Obj.ResourceId;
                ObjVm.EditMode = 1;

                return PartialView("_ContractResource", ObjVm);
            }
            else
            {
                ResourceViewModel ObjVm = new ResourceViewModel();
                return PartialView("_ContractResource", ObjVm);
            }
        }
        public ActionResult LoadContractResourceWithCategory(Int64 ContractResourceId)
        {


            return PartialView("_ResourceCategory");

        }
        public ActionResult BuilderDetails(Int64 ContractId)
        {

            ViewBag.ContractId = ContractId;
            TempData["contract"] = ContractId;
            return PartialView("_BuilderDetails");

        }
        public ActionResult ListOfBuilders([DataSourceRequest] DataSourceRequest request)
        {
            Int64 ContractId = Convert.ToInt64(TempData["contract"]);
            var ContractBuilderList = _ObjContractBuilderService.GetBuilderofContract(ContractId)
                                        .Select(x => new BuildersViewModel
                                        {
                                            BuilderId = x.BuilderId,
                                            BuilderName = getBuilderCompany(x.BuilderId), // change by rabi on 05 jan - Builder Name take as company name
                                            // BuilderName = getBuilder(x.BuilderId),
                                            MarketName = getMarket(x.BuilderId)
                                        });
            return Json(ContractBuilderList.ToDataSourceResult(request));
        }

        public string getBuilderCompany(Int64 BuiulderId)
        {
            string CompanyName = "";
            if (_ObjBuilderService.BuilderDetails(BuiulderId).Count() > 0)
            {
                var value = _ObjBuilderService.BuilderDetails(BuiulderId).FirstOrDefault();
                CompanyName = value.BuilderName;
            }
            return CompanyName;
        }

        public string getBuilder(Int64 BuiulderId)
        {
            string BuilderName = "";
            if (_ObjBuilderService.BuilderDetails(BuiulderId).Count() > 0)
            {
                var value = _ObjBuilderService.BuilderDetails(BuiulderId).FirstOrDefault();
                BuilderName = value.FirstName + " " + value.LastName;
            }


            return BuilderName;
        }
        public string getMarket(Int64 BuiulderId)
        {
            string MarketName = "";
            if (_ObjBuilderService.BuilderDetails(BuiulderId).Count() > 0)
            {
                MarketName = _ObjBuilderService.BuilderDetails(BuiulderId).FirstOrDefault().Market.MarketName;
            }

            return MarketName;
        }
        public JsonResult GetContractResourceCategory()
        {
            var ContractResourceCategoryList = _ObjResourceCategoryService.GetResourceCategoryAll().Where(x => x.RowStatusId == (int)RowActiveStatus.Active)
                .OrderByDescending(x => x.ResourceCategoryId).Select(z => new ResourceCategoryViewModel
                {
                    ResourceCategoryId = z.ResourceCategoryId,
                    ResourceCategoryName = z.ResourceCategoryName

                });
            return Json(ContractResourceCategoryList, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult IsResourceLableValid(string LabelName, string ResourceDumpId, int EditMode, Int64 ContractId)
        {

            if (EditMode == 0)
            {
                if (ContractId != 0)
                {
                    bool IsAvail = _ObjResourceService.IsResourceLableUniueWithContract(LabelName, ContractId);
                    return Json(new { IsLabelAvailable = !IsAvail }, JsonRequestBehavior.AllowGet);
                }
                else if (ResourceDumpId == "0")
                {
                    return Json(new { IsLabelAvailable = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool IsAvail = _ObjResourceService.IsResourceLableUniueInContract(LabelName, ResourceDumpId);
                    return Json(new { IsLabelAvailable = !IsAvail }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                bool IsAvail = _ObjResourceService.IsResourceLableUniueWithContract(LabelName, ContractId);
                return Json(new { IsLabelAvailable = !IsAvail }, JsonRequestBehavior.AllowGet);
            }


        }
        
        public JsonResult SaveResource(ResourceViewModel _ObjResourceVm)
        {

            int Flag = 0;
            int File = 0;
            string physicalPath = "";
            string FileNameToSave = "";
            var _container = CBUSA.App_Start.UnityConfig.GetConfiguredContainer();
            var ObjRandom = _container.Resolve<IRandom>();
            string FileNameRandom = ObjRandom.StringRandom("File");
            HttpFileCollectionBase files = Request.Files;


            //  string[] Markets=

            if (_ObjResourceVm.EditMode == 0)
            {
                if (_ObjResourceVm.Markets == null)
                {
                    ModelState.AddModelError("", "Please select atleast on market");

                    string[] ModelErrorMarket = ModelState.Values.Where(E => E.Errors.Count > 0)
                  .SelectMany(E => E.Errors)
                  .Select(E => E.ErrorMessage)
                  .ToArray();

                    return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelErrorMarket) }, JsonRequestBehavior.AllowGet);
                }



            }


            //  if (_ObjResourceVm.Markets.Length > 0)
            //  {
            string[] Markets = new string[] { };
            if (_ObjResourceVm.Markets != null)
            {
                Markets = _ObjResourceVm.Markets.Substring(0, _ObjResourceVm.Markets.Length - 1).Split(',');
            }





            for (int i = 0; i < files.Count; i++)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "ApplicationDocument/";

                HttpPostedFileBase file = files[i];
                string fname;

                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                //  string MimeType = DocumentType.GetMimeType(fname);
                string MimeType = MimeMapping.GetMimeMapping(fname);

                bool IsValidFile = DocumentType.CheckMimeTypeFiles(MimeType.ToLower());

                if (IsValidFile)
                {
                    string extension = System.IO.Path.GetExtension(fname);
                    FileNameToSave = FileNameRandom + extension;
                    physicalPath = Path.Combine(Server.MapPath("~/ApplicationDocument"), FileNameToSave);
                    file.SaveAs(physicalPath);
                    Flag = 1;
                }
                else
                {
                    ModelState.AddModelError("", "Application doesn't support this file");
                }
                File = 1;
            }

            if (File == 0)
            {
                if (_ObjResourceVm.EditMode == 0)
                {
                    ModelState.AddModelError("", "Please Upload file");
                }
                else
                {
                    Flag = 1;
                }
            }


            if (Flag == 1)
            {
                List<ResourceMarket> ResourceMarketList = new List<ResourceMarket>();
                for (int i = 0; i < Markets.Length; i++)
                {
                    ResourceMarket ObjResourcemarket = new ResourceMarket();
                    ObjResourcemarket.MarketId = Convert.ToInt32(Markets[i]);
                    ResourceMarketList.Add(ObjResourcemarket);
                }

                if (_ObjResourceVm.ResourceId > 0)
                {
                    string HistoryFilePath = "";
                    Resource ObjResource = _ObjResourceService.GetResource(_ObjResourceVm.ResourceId);

                    if (File == 1)
                    {

                        HistoryFilePath = ObjResource.FileLocation;
                        ObjResource.FileName = FileNameToSave;
                        ObjResource.FileLocation = physicalPath;

                    }


                    ObjResource.ResourceCategoryId = _ObjResourceVm.ResourceCategoryId;
                    ObjResource.Description = _ObjResourceVm.Description;
                    ObjResource.Title = _ObjResourceVm.Title;

                    ObjResource.ModifiedOn = DateTime.Now;
                    ObjResource.ModifiedBy = 1;


                    _ObjResourceService.UpdateResource(ObjResource, ResourceMarketList);




                    if (HistoryFilePath != "")
                    {
                        //    string completePath = Server.MapPath("~/PDF/Document/" + Session["ID"].ToString()) + "SimpleTable" + index + ".pdf";
                        if (System.IO.File.Exists(HistoryFilePath))
                        {
                            System.IO.File.Delete(HistoryFilePath);
                        }
                    }
                    return Json(new { IsSuccess = true, DumpId = ObjResource.DumpId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Resource ObjResource = new Resource();
                    ObjResource.ResourceCategoryId = _ObjResourceVm.ResourceCategoryId;
                    ObjResource.Description = _ObjResourceVm.Description;
                    ObjResource.Title = _ObjResourceVm.Title;
                    ObjResource.FileName = FileNameToSave;
                    ObjResource.FileLocation = physicalPath;



                    if (_ObjResourceVm.ContractId > 0)
                    {
                        ObjResource.ContractId = _ObjResourceVm.ContractId;
                    }
                    // }



                    if (_ObjResourceVm.DumpId == "0")
                    {
                        ObjResource.DumpId = ObjRandom.StringRandom("Dump");

                    }
                    else
                    {
                        ObjResource.DumpId = _ObjResourceVm.DumpId;

                    }

                    ObjResource.RowStatusId = (int)RowActiveStatus.Active;
                    ObjResource.CreatedOn = DateTime.Now;
                    ObjResource.ModifiedOn = DateTime.Now;
                    ObjResource.CreatedBy = 1;
                    ObjResource.ModifiedBy = 1;
                    ObjResource.RowGUID = Guid.NewGuid();




                    _ObjResourceService.SaveResource(ObjResource, ResourceMarketList);
                    return Json(new { IsSuccess = true, DumpId = ObjResource.DumpId }, JsonRequestBehavior.AllowGet);
                    //}
                }
            }


            //  }
            // else
            //  {
            //   ModelState.AddModelError("", "Please select atleast on market");
            //  }




            string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                   .SelectMany(E => E.Errors)
                   .Select(E => E.ErrorMessage)
                   .ToArray();

            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DeleteResource(Int64 ResourceId)
        {
            if (ResourceId > 0)
            {
                Resource _ObjResource = _ObjResourceService.GetResource(ResourceId);

                _ObjResource.RowStatusId = (int)RowActiveStatus.Deleted;
                _ObjResource.ModifiedBy = 1;
                _ObjResource.ModifiedOn = DateTime.Now;
                _ObjResourceService.DeleteResource(_ObjResource);

                if (_ObjResource.FileLocation != "")
                {
                    //    string completePath = Server.MapPath("~/PDF/Document/" + Session["ID"].ToString()) + "SimpleTable" + index + ".pdf";
                    if (System.IO.File.Exists(_ObjResource.FileLocation))
                    {
                        System.IO.File.Delete(_ObjResource.FileLocation);
                    }
                }


                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            ModelState.AddModelError("", "Please select atleast on market");
            string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                     .SelectMany(E => E.Errors)
                     .Select(E => E.ErrorMessage)
                     .ToArray();
            return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewResource(Int64 ResourceId)
        {
            Resource ObjResource = _ObjResourceService.GetResource(ResourceId);
            return PartialView("_ViewContractResource", ObjResource);
        }

        public FileResult DownloadResourceFile(Int64 ResourceId)
        {
            Resource ObjResource = _ObjResourceService.GetResource(ResourceId);
            return File(ObjResource.FileLocation, MimeMapping.GetMimeMapping(ObjResource.FileName), ObjResource.FileName);

        }

        //Started active contract details and pending contract details by prasenjit adhikari
        public ActionResult ListBind()
        {

            return View();
        }

        public ActionResult ListBind3([DataSourceRequest] DataSourceRequest request)
        {
            List<Contract> lstcon = new List<Contract>();
            lstcon.Add(new Contract() { ContractName = "prasen" });
            return Json(lstcon.ToDataSourceResult(request));
        }
        public ActionResult ManageArchievedContracts()
        {

            return View();
        }
        public ActionResult ManagePendingContracts()
        {

            return View();
        }
        public ActionResult ManageActiveContracts()
        {

            return View();
        }

        public ActionResult ViewBuilderContract()
        {
            return View();
        }

        public ActionResult ActiveContractListViewAscCon([DataSourceRequest] DataSourceRequest request, string Type, int PageValue)
        {
            int RowNo = 2;
            int ContractCounter = 1;
            if (Type == "asccon")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderBy(x => x.ContractName).
                Select(x => new ActiveContractViewModel
                {
                    ConractId = x.ContractId,
                    ConractName = x.ContractName,
                    ContractFrom = FromDate(x.ContrctFrom.ToString()),
                    ContractTo = ToDate(x.ContrctTo.ToString()),
                    BuilderCount = BuilderCount(x.ContractId),
                    ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                    // Website = x.Website,
                    Website = getWebsite(x.ContractId),
                    ManuFacturerName = x.Manufacturer.ManufacturerName,
                    ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                    // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())  comment by Rabi on 30 march
                });

                var newList = ActiveContractList.ToList()
                    .Select(x => new ActiveContractViewModel
                    {
                        ConractId = x.ConractId,
                        ConractName = x.ConractName,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        BuilderCount = x.BuilderCount,
                        ContractStatus = x.ContractStatus,
                        Website = x.Website,
                        ManuFacturerName = x.ManuFacturerName,
                        ProductList = x.ProductList,
                        rowcount = ContractCounter++
                    }).ToList();

                return Json(newList.ToDataSourceResult(request));

            }

            else if (Type == "desccon")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderByDescending(x => x.ContractName).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                        .Select(x => new ActiveContractViewModel
                        {
                            ConractId = x.ConractId,
                            ConractName = x.ConractName,
                            ContractFrom = x.ContractFrom,
                            ContractTo = x.ContractTo,
                            BuilderCount = x.BuilderCount,
                            ContractStatus = x.ContractStatus,
                            Website = x.Website,
                            ManuFacturerName = x.ManuFacturerName,
                            ProductList = x.ProductList,
                            rowcount = ContractCounter++
                        }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }

            else if (Type == "ascncp")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderBy(x => x.Manufacturer.ManufacturerName).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                        .Select(x => new ActiveContractViewModel
                        {
                            ConractId = x.ConractId,
                            ConractName = x.ConractName,
                            ContractFrom = x.ContractFrom,
                            ContractTo = x.ContractTo,
                            BuilderCount = x.BuilderCount,
                            ContractStatus = x.ContractStatus,
                            Website = x.Website,
                            ManuFacturerName = x.ManuFacturerName,
                            ProductList = x.ProductList,
                            rowcount = ContractCounter++
                        }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descncp")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderByDescending(x => x.Manufacturer.ManufacturerName).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                        .Select(x => new ActiveContractViewModel
                        {
                            ConractId = x.ConractId,
                            ConractName = x.ConractName,
                            ContractFrom = x.ContractFrom,
                            ContractTo = x.ContractTo,
                            BuilderCount = x.BuilderCount,
                            ContractStatus = x.ContractStatus,
                            Website = x.Website,
                            ManuFacturerName = x.ManuFacturerName,
                            ProductList = x.ProductList,
                            rowcount = ContractCounter++
                        }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascpc")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           }).OrderBy(x => x.ProductList);

                var newList = ActiveContractList.ToList()
                       .Select(x => new ActiveContractViewModel
                       {
                           ConractId = x.ConractId,
                           ConractName = x.ConractName,
                           ContractFrom = x.ContractFrom,
                           ContractTo = x.ContractTo,
                           BuilderCount = x.BuilderCount,
                           ContractStatus = x.ContractStatus,
                           Website = x.Website,
                           ManuFacturerName = x.ManuFacturerName,
                           ProductList = x.ProductList,
                           rowcount = ContractCounter++
                       }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descpc")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               //   ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           }).OrderByDescending(x => x.ProductList);

                var newList = ActiveContractList.ToList()
                      .Select(x => new ActiveContractViewModel
                      {
                          ConractId = x.ConractId,
                          ConractName = x.ConractName,
                          ContractFrom = x.ContractFrom,
                          ContractTo = x.ContractTo,
                          BuilderCount = x.BuilderCount,
                          ContractStatus = x.ContractStatus,
                          Website = x.Website,
                          ManuFacturerName = x.ManuFacturerName,
                          ProductList = x.ProductList,
                          rowcount = ContractCounter++
                      }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascbj")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderBy(x => x.ContractBuilder.Count).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         ContractFrom = x.ContractFrom,
                         ContractTo = x.ContractTo,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Website = x.Website,
                         ManuFacturerName = x.ManuFacturerName,
                         ProductList = x.ProductList,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }

            else if (Type == "descbj")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderByDescending(x => x.ContractBuilder.Count).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         ContractFrom = x.ContractFrom,
                         ContractTo = x.ContractTo,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Website = x.Website,
                         ManuFacturerName = x.ManuFacturerName,
                         ProductList = x.ProductList,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascst")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderBy(x => x.ContrctTo).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                   .Select(x => new ActiveContractViewModel
                   {
                       ConractId = x.ConractId,
                       ConractName = x.ConractName,
                       ContractFrom = x.ContractFrom,
                       ContractTo = x.ContractTo,
                       BuilderCount = x.BuilderCount,
                       ContractStatus = x.ContractStatus,
                       Website = x.Website,
                       ManuFacturerName = x.ManuFacturerName,
                       ProductList = x.ProductList,
                       rowcount = ContractCounter++
                   }).ToList();
                
                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descst")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderByDescending(x => x.ContrctTo).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         ContractFrom = x.ContractFrom,
                         ContractTo = x.ContractTo,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Website = x.Website,
                         ManuFacturerName = x.ManuFacturerName,
                         ProductList = x.ProductList,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascct")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderBy(x => x.ContrctFrom).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         ContractFrom = x.ContractFrom,
                         ContractTo = x.ContractTo,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Website = x.Website,
                         ManuFacturerName = x.ManuFacturerName,
                         ProductList = x.ProductList,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descct")
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderByDescending(x => x.ContrctFrom).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });
                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         ContractFrom = x.ContractFrom,
                         ContractTo = x.ContractTo,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Website = x.Website,
                         ManuFacturerName = x.ManuFacturerName,
                         ProductList = x.ProductList,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "arch")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().Where(x => x.ContrctTo < archdate).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         ContractFrom = x.ContractFrom,
                         ContractTo = x.ContractTo,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Website = x.Website,
                         ManuFacturerName = x.ManuFacturerName,
                         ProductList = x.ProductList,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == null)
            {
                var ActiveContractList = _ObjContractService.GetActiveContractDescending().OrderByDescending(x => x.ContractId).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = FromDate(x.ContrctFrom.ToString()),
                               ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = StatusAginstDate(x.ContrctTo.ToString()),
                               // Website = x.Website,
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         ContractFrom = x.ContractFrom,
                         ContractTo = x.ContractTo,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Website = x.Website,
                         ManuFacturerName = x.ManuFacturerName,
                         ProductList = x.ProductList,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            
            var value = "";
            return Json(value.ToDataSourceResult(request));
        }

        public string getWebsite(Int64 ContractId)
        {
            string website = _ObjContractService.GetContract(ContractId).Website;
            if (website.Contains("http"))
            {
                website = website.Substring(website.LastIndexOf('/') + 1);
            }
            return website;
        }

        public int BuilderCount(Int64 ContractId)
        {
            int Count = _ObjContractBuilderService.GetBuilderofContract(ContractId).Count();
            return Count;

        }
        public string Manufacterer(List<Product> h)
        {
            List<string> product = h.Select(x => x.ProductCategory).Select(x => x.ProductCategoryName).Distinct().ToList();

            string products = string.Join(",", product);


            return products.ToString();
        }
        public ActionResult PendingContractListView([DataSourceRequest] DataSourceRequest request, string Type, int PageValue)
        {
            int RowNo = 2;
            int ContractCounter = 1;
            int TotalMarket = _ObjContractService.GetMarketCount();
            
            if (Type == "asccon")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderBy(x => x.ContractName).
                Select(x => new ActiveContractViewModel
                {
                    ConractId = x.ContractId,
                    ConractName = x.ContractName,

                    Websiteslist = getwebsitedata(x.ContractId),
                    ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                    //ContractTo = ToDate(x.ContrctTo.ToString()),
                    BuilderCount = BuilderCount(x.ContractId),

                    ContractStatus = x.ContractStatus.ContractStatusName,
                    Manufactererlist = getManufacterer(x.ContractId),
                    //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                    ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                    IsAllRowVisible = RowNo * PageValue >= 2
                });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));

            }

            else if (Type == "desccon")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderByDescending(x => x.ContractName).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),

                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               //    ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }

            else if (Type == "ascncp")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderBy(x => x.PrimaryManufacturer).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               //    ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descncp")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderByDescending(x => x.PrimaryManufacturer).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascpc")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           }).OrderBy(x => x.ProductList);

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descpc")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           }).OrderByDescending(x => x.ProductList);

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascbj")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderBy(x => x.ContractBuilder.Count).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               //    ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }

            else if (Type == "descbj")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderByDescending(x => x.ContractBuilder.Count).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascst")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderBy(x => x.ContrctTo).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               //   ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descst")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderByDescending(x => x.ContrctTo).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();


                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascct")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderBy(x => x.EstimatedStartDate).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               //   ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descct")
            {
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderByDescending(x => x.EstimatedStartDate).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,
                               Manufactererlist = getManufacterer(x.ContractId),
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == null)
            {
                var ActiveContractList45 = _ObjContractService.GetActivePendingContract();
                var ActiveContractList = _ObjContractService.GetActivePendingContract().OrderByDescending(x => x.ContractId).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,

                               Websiteslist = getwebsitedata(x.ContractId),
                               ContractFrom = FromDate(x.EstimatedStartDate.ToString()),
                               //ContractTo = ToDate(x.ContrctTo.ToString()),
                               BuilderCount = BuilderCount(x.ContractId),

                               ContractStatus = x.ContractStatus.ContractStatusName,

                               Manufactererlist = getManufacterer(x.ContractId),
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList()),
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                               IsAllRowVisible = RowNo * PageValue >= 2
                           });

                var newList = ActiveContractList.ToList()
                     .Select(x => new ActiveContractViewModel
                     {
                         ConractId = x.ConractId,
                         ConractName = x.ConractName,
                         Websiteslist = x.Websiteslist,
                         ContractFrom = x.ContractFrom,
                         BuilderCount = x.BuilderCount,
                         ContractStatus = x.ContractStatus,
                         Manufactererlist = x.Manufactererlist,
                         ProductList = x.ProductList,
                         IsAllRowVisible = x.IsAllRowVisible,
                         rowcount = ContractCounter++
                     }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            var value = "";
            return Json(value.ToDataSourceResult(request));
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
        public string PrimaryContracts(string con)
        {
            if (con == "undefined" || con == null)
            {
                con = "";
            }
            return con;
        }

        public ActionResult ArchievedContractListView([DataSourceRequest] DataSourceRequest request, string Type, int PageValue)
        {

            int RowNo = 2;
            int ContractCounter = 1;
            
            if (Type == "asccon")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderBy(x => x.ContractName).
                Select(x => new ActiveContractViewModel
                {
                    ConractId = x.ContractId,
                    ConractName = x.ContractName,
                    ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                    ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                    BuilderCount = BuilderCount(x.ContractId),
                    ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                    Website = getWebsite(x.ContractId),
                    ManuFacturerName = x.Manufacturer.ManufacturerName,
                    // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                    ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                });

                var newList = ActiveContractList.ToList()
                    .Select(x => new ActiveContractViewModel
                    {
                        ConractId = x.ConractId,
                        ConractName = x.ConractName,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        BuilderCount = x.BuilderCount,
                        ContractStatus = x.ContractStatus,
                        Website = x.Website,
                        ManuFacturerName = x.ManuFacturerName,
                        ProductList = x.ProductList,
                        rowcount = ContractCounter++
                    }).ToList();

                return Json(newList.ToDataSourceResult(request));

            }

            else if (Type == "desccon")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().OrderByDescending(x => x.ContractName).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               // ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                    .Select(x => new ActiveContractViewModel
                    {
                        ConractId = x.ConractId,
                        ConractName = x.ConractName,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        BuilderCount = x.BuilderCount,
                        ContractStatus = x.ContractStatus,
                        Website = x.Website,
                        ManuFacturerName = x.ManuFacturerName,
                        ProductList = x.ProductList,
                        rowcount = ContractCounter++
                    }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }

            else if (Type == "ascncp")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderBy(x => x.Manufacturer.ManufacturerName).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                    .Select(x => new ActiveContractViewModel
                    {
                        ConractId = x.ConractId,
                        ConractName = x.ConractName,
                        ContractFrom = x.ContractFrom,
                        ContractTo = x.ContractTo,
                        BuilderCount = x.BuilderCount,
                        ContractStatus = x.ContractStatus,
                        Website = x.Website,
                        ManuFacturerName = x.ManuFacturerName,
                        ProductList = x.ProductList,
                        rowcount = ContractCounter++
                    }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descncp")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderByDescending(x => x.Manufacturer.ManufacturerName).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                   .Select(x => new ActiveContractViewModel
                   {
                       ConractId = x.ConractId,
                       ConractName = x.ConractName,
                       ContractFrom = x.ContractFrom,
                       ContractTo = x.ContractTo,
                       BuilderCount = x.BuilderCount,
                       ContractStatus = x.ContractStatus,
                       Website = x.Website,
                       ManuFacturerName = x.ManuFacturerName,
                       ProductList = x.ProductList,
                       rowcount = ContractCounter++
                   }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascbj")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderBy(x => x.ContractBuilder.Count).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //   ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                   .Select(x => new ActiveContractViewModel
                   {
                       ConractId = x.ConractId,
                       ConractName = x.ConractName,
                       ContractFrom = x.ContractFrom,
                       ContractTo = x.ContractTo,
                       BuilderCount = x.BuilderCount,
                       ContractStatus = x.ContractStatus,
                       Website = x.Website,
                       ManuFacturerName = x.ManuFacturerName,
                       ProductList = x.ProductList,
                       rowcount = ContractCounter++
                   }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }

            else if (Type == "descbj")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderByDescending(x => x.ContractBuilder.Count).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                  .Select(x => new ActiveContractViewModel
                  {
                      ConractId = x.ConractId,
                      ConractName = x.ConractName,
                      ContractFrom = x.ContractFrom,
                      ContractTo = x.ContractTo,
                      BuilderCount = x.BuilderCount,
                      ContractStatus = x.ContractStatus,
                      Website = x.Website,
                      ManuFacturerName = x.ManuFacturerName,
                      ProductList = x.ProductList,
                      rowcount = ContractCounter++
                  }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascst")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderBy(x => x.ContrctTo).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                  .Select(x => new ActiveContractViewModel
                  {
                      ConractId = x.ConractId,
                      ConractName = x.ConractName,
                      ContractFrom = x.ContractFrom,
                      ContractTo = x.ContractTo,
                      BuilderCount = x.BuilderCount,
                      ContractStatus = x.ContractStatus,
                      Website = x.Website,
                      ManuFacturerName = x.ManuFacturerName,
                      ProductList = x.ProductList,
                      rowcount = ContractCounter++
                  }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descst")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderByDescending(x => x.ContrctTo).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                  .Select(x => new ActiveContractViewModel
                  {
                      ConractId = x.ConractId,
                      ConractName = x.ConractName,
                      ContractFrom = x.ContractFrom,
                      ContractTo = x.ContractTo,
                      BuilderCount = x.BuilderCount,
                      ContractStatus = x.ContractStatus,
                      Website = x.Website,
                      ManuFacturerName = x.ManuFacturerName,
                      ProductList = x.ProductList,
                      rowcount = ContractCounter++
                  }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "ascct")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderBy(x => x.ContrctFrom).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                 .Select(x => new ActiveContractViewModel
                 {
                     ConractId = x.ConractId,
                     ConractName = x.ConractName,
                     ContractFrom = x.ContractFrom,
                     ContractTo = x.ContractTo,
                     BuilderCount = x.BuilderCount,
                     ContractStatus = x.ContractStatus,
                     Website = x.Website,
                     ManuFacturerName = x.ManuFacturerName,
                     ProductList = x.ProductList,
                     rowcount = ContractCounter++
                 }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "descct")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).OrderByDescending(x => x.ContrctFrom).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //   ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                 .Select(x => new ActiveContractViewModel
                 {
                     ConractId = x.ConractId,
                     ConractName = x.ConractName,
                     ContractFrom = x.ContractFrom,
                     ContractTo = x.ContractTo,
                     BuilderCount = x.BuilderCount,
                     ContractStatus = x.ContractStatus,
                     Website = x.Website,
                     ManuFacturerName = x.ManuFacturerName,
                     ProductList = x.ProductList,
                     rowcount = ContractCounter++
                 }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == "arch")
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                 .Select(x => new ActiveContractViewModel
                 {
                     ConractId = x.ConractId,
                     ConractName = x.ConractName,
                     ContractFrom = x.ContractFrom,
                     ContractTo = x.ContractTo,
                     BuilderCount = x.BuilderCount,
                     ContractStatus = x.ContractStatus,
                     Website = x.Website,
                     ManuFacturerName = x.ManuFacturerName,
                     ProductList = x.ProductList,
                     rowcount = ContractCounter++
                 }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            else if (Type == null)
            {
                DateTime archdate = DateTime.Now.AddDays(-30);
                var ActiveContractList = _ObjContractService.GetArchievedContractDescending().Where(x => x.ContrctTo < archdate).
                           Select(x => new ActiveContractViewModel
                           {
                               ConractId = x.ContractId,
                               ConractName = x.ContractName,
                               ContractFrom = x.ContrctFrom != null ? FromDate(x.ContrctFrom.ToString()) : "",
                               ContractTo = x.ContrctTo != null ? ToDate(x.ContrctTo.ToString()) : "",
                               BuilderCount = BuilderCount(x.ContractId),
                               ContractStatus = x.ContrctTo != null ? StatusAginstDate(x.ContrctTo.ToString()) : "",
                               Website = getWebsite(x.ContractId),
                               ManuFacturerName = x.Manufacturer.ManufacturerName,
                               //  ProductList = ProductCategory(x.ContractProduct.Select(y => y.Product).ToList())
                               ProductList = string.Join(",", x.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName)),
                           });

                var newList = ActiveContractList.ToList()
                 .Select(x => new ActiveContractViewModel
                 {
                     ConractId = x.ConractId,
                     ConractName = x.ConractName,
                     ContractFrom = x.ContractFrom,
                     ContractTo = x.ContractTo,
                     BuilderCount = x.BuilderCount,
                     ContractStatus = x.ContractStatus,
                     Website = x.Website,
                     ManuFacturerName = x.ManuFacturerName,
                     ProductList = x.ProductList,
                     rowcount = ContractCounter++
                 }).ToList();

                return Json(newList.ToDataSourceResult(request));
            }
            
            var value = "";
            return Json(value.ToDataSourceResult(request));
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
        public string ToDate(string date)
        {
            string dat = "";
            if (date != "")
            {
                DateTime fromdate = Convert.ToDateTime(date);
                dat = fromdate.ToString("MM/dd/yy");
            }
            return dat;
        }
        public string StatusAginstDate(string todate)
        {
            string Status = "";
            if (todate != "")
            {
                DateTime TDate = Convert.ToDateTime(todate);
                if (TDate < DateTime.Now)
                {
                    Status = "Ended";
                }
                else
                {
                    Status = "Started";
                }
            }
            return Status;
        }

        public string ProductCategory(List<Product> h)
        {

            List<string> product = h.Select(x => x.ProductCategory).Select(x => x.ProductCategoryName).Distinct().ToList();

            string products = string.Join(",", product);

            //var lastItem = h.Last();
            //var firstOrDefault = h.FirstOrDefault();

            //string kk = "";

            //List<string> p = new List<string>();
            //StringBuilder concatenatedString = new StringBuilder();
            //int i=1;
            //foreach (var b in h)
            //{


            //    kk = b.ProductCategory.ProductCategoryName;




            //    if (!concatenatedString.ToString().Contains(kk))
            //    {
            //        if (i<h.Count)
            //        {
            //            kk = kk + ",";
            //        }



            //        concatenatedString.Append(kk);


            //    }
            //    i++;

            //}

            return products.ToString();
        }

        //end of active contract details and pending contract details

        public ActionResult ContractResourceListview_Read([DataSourceRequest] DataSourceRequest request, Int64 ContractId, int PageValue, String SearchText)
        {
            int RowNo = 5;
            int TotalMarket = _ObjContractService.GetMarketCount();

            if (SearchText == "")
            {
                int MaxContractResourceCount = _ObjContractService.GetContract(ContractId).Resource.Where(x => x.RowStatusId == (int) RowActiveStatus.Active).Count();

                var ResourceList = _ObjContractService.GetContract(ContractId).Resource
                .Where(x => x.RowStatusId == (int) RowActiveStatus.Active).OrderBy(x => x.Title).Take(RowNo * PageValue).
                Select(x => new ContractResourceViewModel
                {
                    ResourceId = x.ResourceId,
                    ResourceName = x.Title,
                    ResourceTitle = x.Description,
                    ResourceMarketList = x.ResourceMarket.Count() >= TotalMarket ? "All Market" : string.Join(",", x.ResourceMarket.OrderBy(z => z.Market.MarketName).Select(y => y.Market.MarketName)),
                    IsAllRowVisible = RowNo * PageValue >= MaxContractResourceCount
                });

                return Json(ResourceList.ToDataSourceResult(request));
            }
            else
            {
                int MaxContractResourceCount = _ObjContractService.GetContract(ContractId).Resource.Where(x => x.RowStatusId == (int) RowActiveStatus.Active && x.Title.ToLower().Contains(SearchText.ToLower())).Count();

                var ResourceList = _ObjContractService.GetContract(ContractId).Resource
                .Where(x => x.RowStatusId == (int) RowActiveStatus.Active && x.Title.ToLower().Contains(SearchText.ToLower())).OrderBy(x => x.Title).Take(RowNo * PageValue).
                Select(x => new ContractResourceViewModel
                {
                    ResourceId = x.ResourceId,
                    ResourceName = x.Title,
                    ResourceTitle = x.Description,
                    ResourceMarketList = x.ResourceMarket.Count() >= TotalMarket ? "All Market" : string.Join(",", x.ResourceMarket.OrderBy(z => z.Market.MarketName).Select(y => y.Market.MarketName)),
                    IsAllRowVisible = RowNo * PageValue >= MaxContractResourceCount
                });

                return Json(ResourceList.ToDataSourceResult(request));
            }
        }
        //.OrderByDescending(x => x.ResourceId)
        #endregion

        #region Contract Resource Category

        public ActionResult LoadContractResourceCategory()
        {
            return PartialView("_ContractResourceCategory");
        }

        public JsonResult ContractResourceCategory_Create(ResourceCategoryViewModel ObjResourceCategory)
        {

            if (ModelState.IsValid)
            {
                ResourceCategory _ObjResourceCategory = new ResourceCategory();
                _ObjResourceCategory.ResourceCategoryName = ObjResourceCategory.ResourceCategoryName;
                _ObjResourceCategory.RowStatusId = (int)RowActiveStatus.Active;

                _ObjResourceCategory.CreatedBy = 1;
                _ObjResourceCategory.ModifiedBy = 1;
                _ObjResourceCategoryService.SaveResourceCategory(_ObjResourceCategory);
                return Json(new { IsSuccess = true });
            }
            return Json(new { IsSuccess = false });
        }
        public ActionResult ContractResourceCategory_Read([DataSourceRequest] DataSourceRequest request)
        {

            var ResourceCategoryList = _ObjResourceCategoryService.GetResourceCategoryAll().Where(x => x.RowStatusId == (int)RowActiveStatus.Active)
                .OrderByDescending(x => x.ResourceCategoryId)
                .Select(z => new ResourceCategoryViewModel
                {
                    ResourceCategoryId = z.ResourceCategoryId,
                    ResourceCategoryName = z.ResourceCategoryName,
                    IsActive = (z.Resource.Where(x => x.ResourceMarket.Count > 0).Count()) > 0 ? false : true

                });
            return Json(ResourceCategoryList.ToDataSourceResult(request));
        }
        public ActionResult ContractResourceCategory_Edit([DataSourceRequest] DataSourceRequest request, ResourceCategoryViewModel ObjResourceCategory)
        {
            if (ModelState.IsValid)
            {
                ResourceCategory _ObjResourceCategory = _ObjResourceCategoryService.GetResourceCategory(ObjResourceCategory.ResourceCategoryId);
                _ObjResourceCategory.ResourceCategoryName = ObjResourceCategory.ResourceCategoryName;
                _ObjResourceCategory.ModifiedOn = DateTime.Now;
                _ObjResourceCategory.ModifiedBy = 1;
                _ObjResourceCategoryService.EditResourceCategory(_ObjResourceCategory);
            }
            return Json(new[] { ObjResourceCategory }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult ContractResourceCategory_Delete([DataSourceRequest] DataSourceRequest request, ResourceCategoryViewModel ObjResourceCategory)
        {
            if (ModelState.IsValid)
            {
                ResourceCategory _ObjResourceCategory = _ObjResourceCategoryService.GetResourceCategory(ObjResourceCategory.ResourceCategoryId);
                _ObjResourceCategory.RowStatusId = (int)RowActiveStatus.Archived;
                _ObjResourceCategory.ModifiedOn = DateTime.Now;
                _ObjResourceCategory.ModifiedBy = 1;
                _ObjResourceCategoryService.DeleteResourceCategory(_ObjResourceCategory);
            }
            return Json(new[] { ObjResourceCategory }.ToDataSourceResult(request, ModelState));
        }


        #endregion

        #region ViewContract

        public ActionResult ViewContract(Int64 ContrcatId)
        {



            var _ObjContract = _ObjContractService.GetContract(ContrcatId);
            ContractViewModel _ObjContractVM = new ContractViewModel();
            _ObjContractVM.ContractId = _ObjContract.ContractId;
            _ObjContractVM.ContractName = _ObjContract.ContractName;
            _ObjContractVM.Label = _ObjContract.Label;
            _ObjContractVM.ContractStatusId = Convert.ToInt32(_ObjContract.ContractStatusId);
            _ObjContractVM.Website = _ObjContract.Website;
            _ObjContractVM.ContractFrom = _ObjContract.ContrctFrom;
            _ObjContractVM.ContractTo = _ObjContract.ContrctTo;
            _ObjContractVM.ContractDeliverables = _ObjContract.ContractDeliverables;
            _ObjContractVM.ContractDeliverables = _ObjContract.ContractDeliverables;
            _ObjContractVM.ManufacturerId = _ObjContract.ManufacturerId;
            _ObjContractVM.EstimatedStartDate = _ObjContract.EstimatedStartDate;
            _ObjContractVM.EntryDeadline = _ObjContract.EntryDeadline;
            _ObjContractVM.PrimaryManufacturer = _ObjContract.PrimaryManufacturer;
            _ObjContractVM.ContractIcon = _ObjContract.ContractIcon;


            return View(_ObjContractVM);
        }
                
        public JsonResult EditContract(ContractViewModel _ObjContractVM)
        {

            //   int Flag = 0;
            string LogoImageBase64 = "";

            if (_ObjContractVM.ContractStatusId == 1) ///for active Status Id
            {
                if (_ObjContractVM.ContractDeliverables == string.Empty)
                {
                    ModelState.AddModelError("", "Contract deliverables are required");
                }


                if (_ObjContractVM.ContractFrom.HasValue && _ObjContractVM.ContractTo.HasValue)
                {


                    if (_ObjContractVM.ContractFrom >= _ObjContractVM.ContractTo)
                    {
                        ModelState.AddModelError("", "Invalid start and end date");
                    }
                    if (_ObjContractVM.ContractTo < DateTime.Now)
                    {
                        ModelState.AddModelError("", "Invalid end date");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid start and end date");
                }

                if (_ObjContractVM.ManufacturerId == 0)
                {
                    ModelState.AddModelError("", "Manufacturer Required");
                }



            }
            if (_ObjContractVM.ContractStatusId != 1) ///for active Status Id
            {
                if (_ObjContractVM.EstimatedStartDate.HasValue && _ObjContractVM.EntryDeadline.HasValue)
                {
                    if (_ObjContractVM.EstimatedStartDate < DateTime.Now)
                    {
                        ModelState.AddModelError("", "Invalid 'Estimated start date");
                    }
                    if (_ObjContractVM.EntryDeadline < DateTime.Now)
                    {
                        ModelState.AddModelError("", " Invalid 'Early bird entry deadline");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid EstimatedStart and EntryDeadline date");
                }
            }

            // var _ObjContract = _ObjContractService.GetContract(_ObjContractVM.ContractId);

            //_ObjContractVM
            ModelState.Remove("Products");
            if (ModelState.IsValid)
            {

                byte[] LogoImageByte = null;
                // string LogoImageBase64 = "";
                HttpFileCollectionBase files = Request.Files;

                for (int i = 0; i < files.Count; i++)
                {
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    HttpPostedFileBase file = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    // Get the complete folder path and store the file inside it.  
                    // fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                    // file.SaveAs(fname);
                    BinaryReader rdr = new BinaryReader(file.InputStream);
                    LogoImageByte = rdr.ReadBytes((int)file.ContentLength);

                }

                var _ObjContract = _ObjContractService.GetContract(_ObjContractVM.ContractId);
                //  Contract _ObjContract = new Contract();

                _ObjContract.ContractName = _ObjContractVM.ContractName;
                _ObjContract.Label = _ObjContractVM.Label;

                _ObjContract.ContractStatusId = _ObjContractVM.ContractStatusId;
                _ObjContract.Website = _ObjContractVM.Website;

                if (LogoImageByte != null)
                {
                    _ObjContract.ContractIcon = LogoImageByte;


                    LogoImageBase64 = Convert.ToBase64String(LogoImageByte);
                    LogoImageBase64 = string.Format("data:image/png;base64,{0}", LogoImageBase64);

                }


                if (_ObjContractVM.ContractStatusId == 1) ///for active Status Id
                {
                    _ObjContract.ContrctFrom = _ObjContractVM.ContractFrom;
                    _ObjContract.ContrctTo = _ObjContractVM.ContractTo;
                    _ObjContract.ContractDeliverables = _ObjContractVM.ContractDeliverables;
                    _ObjContract.ContractDeliverables = _ObjContractVM.ContractDeliverables;
                    _ObjContract.ManufacturerId = _ObjContractVM.ManufacturerId;
                }
                else
                {
                    _ObjContract.EstimatedStartDate = _ObjContractVM.EstimatedStartDate;
                    _ObjContract.EntryDeadline = _ObjContractVM.EntryDeadline;
                    _ObjContract.PrimaryManufacturer = _ObjContractVM.PrimaryManufacturer;

                }

                //  _ObjContract.RowStatusId = (int)RowActiveStatus.Active;
                // _ObjContract.CreatedBy = 1;
                _ObjContract.ModifiedBy = 1;

                ContractStatusHistory _ObjContractStatusHistory = new ContractStatusHistory();
                _ObjContractStatusHistory.ContractId = _ObjContract.ContractId;
                _ObjContractStatusHistory.ContractStatusId = _ObjContractVM.ContractStatusId;
                _ObjContractStatusHistory.EntryDate = DateTime.Now;

                _ObjContractService.EditContract(_ObjContract, _ObjContractStatusHistory);
                return Json(new { IsSuccess = true, LogoImageBase64 = LogoImageBase64 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                 .SelectMany(E => E.Errors)
                 .Select(E => E.ErrorMessage)
                 .ToArray();


                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);
            }


        }
        
        public ActionResult LoadContractGenaralInformation()
        {
            return PartialView("_ContractResource");
        }

        public JsonResult LoadContractBuilderInformation()
        {
            // return PartialView("_ContractResource");
            return Json("");
        }

        public JsonResult LoadContractProductInformation()
        {
            return Json("");
        }
        
        #region ContractReabte
        public ActionResult LoadContractRebate(Int64 ContractId)
        {
            Contract ObjContract = _ObjContractService.GetContract(ContractId);
            IEnumerable<ContractStatus> ObjContractServices = _ObjContractStatusServices.GetContractStatusAll().Where(x => x.RowStatusId == (int)RowActiveStatus.Active);

            var ObjContractReabteStatus = from Status in ObjContractServices
                                          join ConStatus in ObjContract.ContractRebate on Status.ContractStatusId equals ConStatus.ContractStatusId into gj
                                          from ContractRebateStatus in gj.DefaultIfEmpty()
                                          orderby Status.ContractStatusId descending
                                          select new ContractRebateViewModel
                                          {
                                              ContractStatusId = Status.ContractStatusId,
                                              ContractStatusName = Status.ContractStatusName,
                                              RebatePercentage = (ContractRebateStatus == null) ? 0 : ContractRebateStatus.RebatePercentage

                                          };
            return PartialView("_ConfigureRebate", ObjContractReabteStatus);
        }


        public JsonResult SaveContractRebate(List<ContractRebateViewModel> StatusRebateList)
        {
            List<ContractRebate> _ObjContractRebate = StatusRebateList.Select(x => new ContractRebate
            {
                ContractId = x.ContractId,
                ContractStatusId = x.ContractStatusId,
                RebatePercentage = x.RebatePercentage,
                CreatedOn = DateTime.Now,
                ModifiedBy = 1,
                CreatedBy = 1,
                ModifiedOn = DateTime.Now,
                RowStatusId = (int)RowActiveStatus.Active,
                RowGUID = Guid.NewGuid()
            }).ToList();

            _ObjContractRebateService.SaveContractRebate(_ObjContractRebate);
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        #region RebateOverride

        public ActionResult LoadContractRebateOverride()
        {
            return PartialView("_ContractRebateOverride");
        }


        public JsonResult GetAssociatedBuilder(string MarketList, Int64 ContractId)
        {
            string[] Markets = MarketList.Split(',');
            Int64[] ConvertMarketList = Array.ConvertAll(Markets, s => long.Parse(s));
            var Bulider = _ObjContractBuilderService.GetBuilderofContract(ContractId).Where(x => ConvertMarketList.Contains(x.Builder.MarketId)).Select
                (x => new
                {
                    BuilderId = x.Builder.BuilderId,
                    BuilderName = x.Builder.BuilderName
                });
            return Json(Bulider, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuilderRebateInformation(Int64 ContractId, Int64 BuilderId)
        {
            var Bulider = _ObjContractBuilderService.GetBuilderContractInformation(ContractId, BuilderId);

            var BuilderPreviousRebateHistory = _ObjContractRebateService.GetContactBuilderRebateAll(ContractId, BuilderId);

            string ContractJoinOn = Bulider.FirstOrDefault().CreatedOn.ToShortDateString();
            Int64 ContactStatusID = Bulider.FirstOrDefault().ContractStatus.ContractStatusId;
            decimal ApplicableRebate = 0;

            var ContractRebateList = Bulider.FirstOrDefault().ContractStatus.ContractRebate.Where(x => x.ContractId == ContractId && x.ContractStatusId == ContactStatusID);
            if (ContractRebateList != null)
            {
                if (ContractRebateList.Count() > 0)
                {
                    ApplicableRebate = ContractRebateList.FirstOrDefault().RebatePercentage;
                }


            }



            // decimal ApplicableRebate = Bulider.FirstOrDefault().ContractStatus.ContractRebate.Where(x => x.ContractId == ContractId && x.ContractStatusId == ContactStatusID).FirstOrDefault().RebatePercentage; ;

            string ContractStatusWhenJoin = Bulider.FirstOrDefault().ContractStatus.ContractStatusName;


            string LastUpdate = "";
            decimal ApplicableRebateToday = 0;
            if (BuilderPreviousRebateHistory.Count() == 0)
            {
                ApplicableRebateToday = ApplicableRebate;
                LastUpdate = "Never";

            }
            else
            {
                ApplicableRebateToday = BuilderPreviousRebateHistory.FirstOrDefault().RebatePercentage;
                LastUpdate = BuilderPreviousRebateHistory.FirstOrDefault().ModifiedOn.ToShortDateString();
            }

            return Json(new
            {
                IsSuccess = true,
                ContractJoinOn = ContractJoinOn,
                ApplicableRebate = ApplicableRebate,
                ContractStatusWhenJoin = ContractStatusWhenJoin,
                ApplicableRebateToday = ApplicableRebateToday,
                LastUpdate = LastUpdate
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveContractBuilderReabte(ContractRebateViewModel ObjContractRebateViewModel)
        {

            if (ObjContractRebateViewModel.RebatePercentage > 0)
            {
                ContractRebateBuilder _Obj = new ContractRebateBuilder();
                _Obj.ContractId = ObjContractRebateViewModel.ContractId;
                _Obj.BuilderId = ObjContractRebateViewModel.BuilderId;
                _Obj.RebatePercentage = ObjContractRebateViewModel.RebatePercentage;
                _Obj.RowStatusId = (int)RowActiveStatus.Active;
                _Obj.CreatedBy = 1;
                _Obj.ModifiedBy = 1;
                _Obj.CreatedOn = DateTime.Now;
                _Obj.ModifiedOn = DateTime.Now;
                _Obj.RowGUID = Guid.NewGuid();

                _ObjContractRebateService.OverrideContractRebate(_Obj);
                return Json(new
                {
                    IsSuccess = true,
                    ApplicableRebateToday = ObjContractRebateViewModel.RebatePercentage,
                    LastUpdate = DateTime.Now.ToShortDateString()
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ModelState.AddModelError("", "Invalid EstimatedStart and EntryDeadline date");
                string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                .SelectMany(E => E.Errors)
                .Select(E => E.ErrorMessage)
                .ToArray();


                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);
            }

            //   IContractRebateService  _obj



        }

        #endregion
        
        #endregion
        
        #region ContractBuilder

        public JsonResult LoadContractBuilderData(Int64 ContractId)
        {
            var Contract = _ObjContractService.GetContract(ContractId);

            var AssociateBuilder = Contract.ContractBuilder.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.Builder)
                                   .GroupBy(x => x.MarketId);

            var AllBuilders = _ObjBuilderService.GetBuilder();

            List<MarketBuilderViewModel> ObjMarketBuilderList = new List<MarketBuilderViewModel>();
            foreach (IGrouping<long, Builder> BuilderGrp in AssociateBuilder)
            {
                MarketBuilderViewModel ObjTemp = new MarketBuilderViewModel();

                ObjTemp.MarketId = BuilderGrp.Key;
                ObjTemp.MarketName = BuilderGrp.Where(x => x.Market.MarketId == BuilderGrp.Key).FirstOrDefault().Market.MarketName;
                ObjTemp.Builders = (from asb in BuilderGrp
                                    join cb in Contract.ContractBuilder on asb.BuilderId equals cb.BuilderId
                                    join allb in AllBuilders on cb.BuilderId equals allb.BuilderId
                                    where cb.ContractId == ContractId && cb.RowStatusId == (int) RowActiveStatus.Active
                                    select new Builder
                                    {
                                        BuilderId = cb.BuilderId,
                                        BuilderName = String.Concat(allb.BuilderName, " (", cb.JoiningDate.ToShortDateString(), ")")
                                    }).OrderBy(y => y.BuilderName).ToList();

                ObjMarketBuilderList.Add(ObjTemp);
            }

            string PrtialViewString = PartialView("_ContractBuilder", ObjMarketBuilderList.OrderBy(m => m.MarketName)).RenderToString();

            return Json(new { IsSuccess = true, PartialView = PrtialViewString }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateContractBuilder(List<ContractBuilder> ObjContractBuilder)
        {
            _ObjContractService.UpdateContractBuilder(ObjContractBuilder);
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OpenMarketBuilderDetailsPopup(Int64 ContractId)
        {
            var Contract = _ObjContractService.GetContract(ContractId);
            var AssociateBuilder = Contract.ContractBuilder.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.Builder).OrderBy(x => x.Market.MarketName).ThenBy(x => x.BuilderName).GroupBy(x => x.MarketId);

            List<MarketViewModel> ObjMarketBuilderList = new List<MarketViewModel>();

            foreach (IGrouping<long, Builder> BuilderGrp in AssociateBuilder)
            {

                MarketViewModel ObjTemp = new MarketViewModel
                {
                    MarketId = BuilderGrp.Key,
                    MarketName = BuilderGrp.Where(x => x.Market.MarketId == BuilderGrp.Key).FirstOrDefault().Market.MarketName,
                    BuildersList = BuilderGrp.Select(x => new BuildersViewModel
                    {
                        BuilderName = x.BuilderName,
                        BuilderId = x.BuilderId,
                        JoiningDate = Contract.ContractBuilder.Where(y => y.RowStatusId == (int)RowActiveStatus.Active
                            && y.BuilderId == x.BuilderId && y.ContractId == ContractId).FirstOrDefault().JoiningDate.ToShortDateString()

                    }).ToList()
                };

                ObjMarketBuilderList.Add(ObjTemp);
            }
            return PartialView("_ContractBuilderViewDetails", ObjMarketBuilderList);
        }

        public void DownLoadContractMarketBuilderView(Int64 ContractId)
        {
            var Contract = _ObjContractService.GetContract(ContractId);
            var AssociateBuilder = Contract.ContractBuilder.Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Select(x => x.Builder).OrderBy(x => x.Market.MarketName).ThenBy(x => x.BuilderName).GroupBy(x => x.MarketId);

            List<MarketViewModel> ObjMarketBuilderList = new List<MarketViewModel>();

            foreach (IGrouping<long, Builder> BuilderGrp in AssociateBuilder)
            {
                MarketViewModel ObjTemp = new MarketViewModel
                {
                    MarketId = BuilderGrp.Key,
                    MarketName = BuilderGrp.Where(x => x.Market.MarketId == BuilderGrp.Key).Select(x => x.Market.MarketName).First().ToString(),
                    BuildersList = BuilderGrp.Select(x => new BuildersViewModel
                    {
                        BuilderName = x.BuilderName,
                        BuilderId = x.BuilderId,
                        JoiningDate = Contract.ContractBuilder.Where(y => y.RowStatusId == (int)RowActiveStatus.Active
                            && y.BuilderId == x.BuilderId && y.ContractId == ContractId).FirstOrDefault().JoiningDate.ToShortDateString()

                    }).ToList()
                };

                ObjMarketBuilderList.Add(ObjTemp);
            }

            string attachment = "attachment; filename=ContractMarketBuilder.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";

            string tab = "";
            tab = "\t";

            Response.Write("Market");
            Response.Write(tab + "Builder");
            Response.Write(tab + "Joining Date");
            Response.Write("\n");

            foreach (var Item in ObjMarketBuilderList)
            {
                if (Item.BuildersList.Count() > 0)
                {
                    foreach (var Itemchild in Item.BuildersList)
                    {
                        Response.Write(Item.MarketName);
                        Response.Write(tab + Itemchild.BuilderName);
                        Response.Write(tab + Itemchild.JoiningDate);
                        Response.Write("\n");
                    }
                }
            }

            Response.End();

            //StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/CustomTemplate/PdfTemplate/CntractBuilderTemplate.html"));
            //StringBuilder Sb = new StringBuilder(reader.ReadToEnd());
            //Sb.Replace("####ContractBuilderList####", SbBuilder.ToString());
            //string str = Sb.ToString();
            //Idownload Download = new PdfDownload();
            //Download.Download(Sb.ToString());
            ////return View();            
        }

        public ActionResult GetAllBuilders()
        {
            var AllBuilderList = (new List<BuildersViewModel>
                                 {
                                    new BuildersViewModel { BuilderId = 0, BuilderName = "--Select--" }
                                 }).ToList().Union(_ObjBuilderService.GetBuilder()
                                .Select(x => new BuildersViewModel
                                {
                                    BuilderId = x.BuilderId,
                                    BuilderName = x.BuilderName
                                }).OrderBy(x => x.BuilderName));
            return Json(AllBuilderList, JsonRequestBehavior.AllowGet);
        }

        //Method for fetching Non-Responders list data
        public ActionResult GetBuilderContractList([DataSourceRequest] DataSourceRequest request, Int64 BuilderId)
        {
            var BuilderContractList = _ObjContractBuilderService.GetAllContractofBuilder(BuilderId)
                                    .Select(x => new ActiveContractViewModel
                                    {
                                        ConractId = x.ContractId,
                                        ConractName = x.Contract.ContractName,
                                        ContractStatus = (x.Contract.ContrctTo == null ? "-" : StatusAginstDate(x.Contract.ContrctTo.ToString())),
                                        ManuFacturerName = (x.Contract.PrimaryManufacturer == null ? x.Contract.Manufacturer.ManufacturerName : x.Contract.PrimaryManufacturer),
                                        ContractTerm = (x.Contract.ContrctTo == null ? "-" : x.Contract.ContrctTo.Value.Date.ToShortDateString()),
                                        ProductList = string.Join(",", x.Contract.ContractProduct.OrderBy(y => y.Product.ProductName).Select(y => y.Product.ProductName))
                                    });

            return Json(BuilderContractList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private string FormatEndDate(DateTime dtDate)
        {
            return "09-07-1980";
        }
        #endregion

        #region ContractProduct


        public JsonResult LoadContractProductData(Int64 ContractId)
        {
            // var Contract = ;

            /*Close by Rabi 
            var AssociatedProduct = _ObjContractService.GetContract(ContractId).ContractProduct.Select(x => x.Product).GroupBy(x => x.ProductCategoryId);

            List<ContractProductViewModel> objProductCategory = new List<ContractProductViewModel>();

            foreach (IGrouping<long, Product> ProductGrp in AssociatedProduct)
            {
                ContractProductViewModel ObjTemp = new ContractProductViewModel();

                ObjTemp.ProductCategoryId = ProductGrp.Key;
                ObjTemp.ProductCategoryName = ProductGrp.Where(x => x.ProductCategoryId == ProductGrp.Key).FirstOrDefault().ProductCategory.ProductCategoryName;
                ObjTemp.Products = ProductGrp.ToList();

                objProductCategory.Add(ObjTemp);
            }
            // string PrtialViewString = RenderPartialToString("_ContractBuilder", ObjMarketBuilderList);

            string PrtialViewString = PartialView("_ContractProduct", objProductCategory).RenderToString();*/

            var AssociatedProduct = _ObjContractService.GetContract(ContractId).ContractProduct.OrderBy(x => x.Product.ProductName).Select(x => x.Product);
            string PrtialViewString = PartialView("_ContractProduct", AssociatedProduct).RenderToString();



            return Json(new { IsSuccess = true, PartialView = PrtialViewString }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadAddContractProductPoup()
        {
            return PartialView("_AddContractProduct");

        }

        public JsonResult AddContractProductFromPopUp(string ProductList, Int64 ContractId)
        {

            /* Added by Rabi 29 March  List<ContractProduct> ObjContractProduct as parameter
           _ObjContractService.SaveContractProduct(ObjContractProduct);
           return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            */

            //Added by Rabi on 29th March
            if (ProductList != null)
            {
                bool IsValid = !ProductList.Split(',').ToArray().Where(x => string.IsNullOrEmpty(x)).Any();
                if (!IsValid)
                    ModelState.AddModelError("", "Please enter product in correct format");

            }

            if (ModelState.IsValid)
            {
                List<string> Products = ProductList.Split(',').ToList<string>();
                _ObjContractService.SaveContractProduct(Products, ContractId);
                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                                .SelectMany(E => E.Errors)
                                .Select(E => E.ErrorMessage)
                                .ToArray();
                return Json(new { IsSuccess = false, ModelError = BuildModelError.GetModelError(ModelError) }, JsonRequestBehavior.AllowGet);
            }

            ///Close
        }

        public JsonResult UpdateContractProduct(List<ContractProduct> ObjContractProduct)
        {

            //

            _ObjContractService.UpdateContractProduct(ObjContractProduct);
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);



        }

        public ActionResult OpenProductDetailsPopup(Int64 ContractId)
        {
            /*Rabi -Closed on March 29 
            var AssociatedProduct = _ObjContractService.GetContract(ContractId).ContractProduct.Select(x => x.Product).GroupBy(x => x.ProductCategoryId);
            List<ContractProductViewModel> objProductCategory = new List<ContractProductViewModel>();
            foreach (IGrouping<long, Product> ProductGrp in AssociatedProduct)
            {
                ContractProductViewModel ObjTemp = new ContractProductViewModel();
                ObjTemp.ProductCategoryId = ProductGrp.Key;
                ObjTemp.ProductCategoryName = ProductGrp.Where(x => x.ProductCategoryId == ProductGrp.Key).FirstOrDefault().ProductCategory.ProductCategoryName;
                ObjTemp.Products = ProductGrp.ToList();
                objProductCategory.Add(ObjTemp);
            }
            return PartialView("_ContractProductViewDetails", objProductCategory);
            */

            var AssociatedProduct = _ObjContractService.GetContract(ContractId).ContractProduct.OrderBy(x => x.Product.ProductName).Select(x => x.Product);
            return PartialView("_ContractProductViewDetails", AssociatedProduct);

        }
        public void DownLoadContractProductView(Int64 ContractId)
        {

            /*Closed by Rabi on 29 march

            var AssociatedProduct = _ObjContractService.GetContract(ContractId).ContractProduct.Select(x => x.Product).GroupBy(x => x.ProductCategoryId);

            List<ContractProductViewModel> objProductCategory = new List<ContractProductViewModel>();
            foreach (IGrouping<long, Product> ProductGrp in AssociatedProduct)
            {
                ContractProductViewModel ObjTemp = new ContractProductViewModel();
                ObjTemp.ProductCategoryId = ProductGrp.Key;
                ObjTemp.ProductCategoryName = ProductGrp.Where(x => x.ProductCategoryId == ProductGrp.Key).FirstOrDefault().ProductCategory.ProductCategoryName;
                ObjTemp.Products = ProductGrp.ToList();
                objProductCategory.Add(ObjTemp);
            }

            StringBuilder SbBuilder = new StringBuilder();
            int Flag = 0;
            foreach (var Item in objProductCategory)
            {
                if (Item.Products.Count() > 0)
                {
                    Flag = 0;
                    foreach (var Itemchild in Item.Products)
                    {
                        SbBuilder.Append("<tr>");
                        SbBuilder.Append("<td></td>"); //-- Default td --
                        if (Flag == 0)
                        {
                            SbBuilder.Append("<td>" + Item.ProductCategoryName + "</td>");
                            //SbBuilder.Append("<td style=\"width:8%\">" + Item.ProductCategoryName + "</td>");
                        }
                        else
                        {
                            SbBuilder.Append("<td>&nbsp;</td>");
                        }
                        SbBuilder.Append("<td></td>"); //-- Default td --
                        SbBuilder.Append("<td>" + Itemchild.ProductName + "</td>");
                        SbBuilder.Append("<td></td>"); //-- Default td --
                        SbBuilder.Append("</tr>");
                        Flag = 1;
                    }

                }
            }

            StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/CustomTemplate/PdfTemplate/ContractProductTemplate.html"));
            StringBuilder Sb = new StringBuilder(reader.ReadToEnd());
            Sb.Replace("####ContractProductList####", SbBuilder.ToString());

            Idownload Download = new PdfDownload();
            Download.Download(Sb.ToString());
            return View();*/
            var AssociatedProduct = _ObjContractService.GetContract(ContractId).ContractProduct.OrderBy(x => x.Product.ProductName).Select(x => x.Product);

            StringBuilder SbBuilder = new StringBuilder();
            // int Flag = 0;


            //Flag = 0;
            foreach (var Itemchild in AssociatedProduct)
            {
                SbBuilder.Append("<tr>");
                SbBuilder.Append("<td></td>"); //-- Default td --
                SbBuilder.Append("<td>" + Itemchild.ProductName + "</td>");
                SbBuilder.Append("<td></td>"); //-- Default td --
                SbBuilder.Append("</tr>");

            }




            StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/CustomTemplate/PdfTemplate/ContractProductTemplate.html"));
            StringBuilder Sb = new StringBuilder(reader.ReadToEnd());
            Sb.Replace("####ContractProductList####", SbBuilder.ToString());

            Idownload Download = new PdfDownload();
            Download.Download(Sb.ToString());
            //  return View();


        }

        #endregion
        
        #region contrct survey

        public ActionResult ContractSurveyListview_Read([DataSourceRequest] DataSourceRequest request, Int64 ContractId)
        {
            IEnumerable<Survey> _ObjSurvey = _ObjSurveyService.GetContractSurvey(ContractId);
            List<SurveyViewModel> ObjVm = _ObjSurvey.Select(x => new SurveyViewModel
            {
                ContractId = ContractId,
                SurveyId = x.SurveyId,
                SurveyName = x.SurveyName,
                IsPublished = x.IsPublished,
                SurveyStatus = x.RowStatusId == 1 ? "Live" : "Closed",
                PublishDate = x.IsPublished == true ? x.ModifiedOn.ToShortDateString() : x.CreatedOn.ToShortDateString(),
                IsNcpSurvey = x.IsNcpSurvey
            }).ToList();
            return Json(ObjVm.ToDataSourceResult(request));
        }


        public ActionResult CopySurvey(Int64 ContractId, Int64 SurveyId, bool? IsNcp)
        {

            _ObjSurveyService.CopySurvey(ContractId, SurveyId, IsNcp);
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }



        #endregion
        
    }
}