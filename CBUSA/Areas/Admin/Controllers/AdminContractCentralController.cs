using CBUSA.Domain;
using CBUSA.Repository.Model;
using CBUSA.Services;
using CBUSA.Services.Interface;
using CBUSA.Areas.Admin.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

using System.Web.Mvc;
using System.IO;

namespace CBUSA.Areas.Admin.Controllers
{
    public class AdminContractCentralController : Controller
    {
        readonly IMarketService _ObjMarketService;
        readonly IContractServices _ObjContractService;
        readonly IContractCentralService _ObjContractCentralService;

        public AdminContractCentralController(IMarketService ObjMarketService, IContractServices ObjContractService, IContractCentralService ObjContractCentralService)
        {
            _ObjMarketService = ObjMarketService;
            _ObjContractService = ObjContractService;
            _ObjContractCentralService = ObjContractCentralService;
        }

        // GET: Admin/ContractCentral
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadContractLogoList()
        {
            PopulateAllActiveContracts();
            return PartialView("_ContractLogoList");
        }

        public List<dynamic> PopulateAllActiveContracts()
        {
            List<dynamic> lstContracts = new List<dynamic>();

            var AllContracts = _ObjContractService.GetActiveContract();

            int TotalContracts = AllContracts.Count();
            int RemainderPage = 0;

            if (TotalContracts % 5 > 0)
                RemainderPage = 1;

            int TotalPages = (TotalContracts / 5) + RemainderPage;
            ViewBag.TotalContracts = TotalContracts;
            ViewBag.TotalPages = TotalPages;

            foreach (Contract objContract in AllContracts)
            {
                object Contract = new Contract
                {
                    ContractId = objContract.ContractId,
                    ContractName = objContract.ContractName,
                    ContractIcon = objContract.ContractIcon
                };

                lstContracts.Add(Contract);
            }

            ViewBag.AllActiveContractLogos = lstContracts;

            return lstContracts;
        }

        public ActionResult ContractContent(Int64 ContractId)
        {
            //*************** GET LIST OF ALL ACTIVE MARKETS **************
            IEnumerable<Market> AllActiveMarket = _ObjMarketService.GetMarket().Where(m => m.RowStatusId == (int)RowActiveStatus.Active).OrderBy(m => m.MarketName);
            ViewBag.AllMarketList = AllActiveMarket;
            //*************************************************************

            //*************** GET LIST OF ALL CONTENT SECTIONS **************
            IEnumerable<ContentSectionRepository> AboveBarContentSection = _ObjContractCentralService.GetAllContentSectionsForContract(ContractId).Where(cs => cs.AboveBar == 1);
            ViewBag.AboveBarContentSection = AboveBarContentSection;

            IEnumerable<ContentSectionRepository> BelowBarContentSection = _ObjContractCentralService.GetAllContentSectionsForContract(ContractId).Where(cs => cs.AboveBar == 2);
            ViewBag.BelowBarContentSection = BelowBarContentSection;
            //*************************************************************

            PopulateAllActiveContracts();

            Contract C = _ObjContractService.GetContract(ContractId);
            ViewBag.ContractId = ContractId;
            ViewBag.ContractName = C.ContractName;
            ViewBag.ContractIcon = C.ContractIcon;

            return View();
        }

        public JsonResult GetVariationList(Int64 ContractId, Int64 SectionId)
        {
            var VariationList = _ObjContractCentralService.GetContentVariationsForContractSection(ContractId, SectionId).ToList();

            return Json(VariationList, JsonRequestBehavior.AllowGet);
        }

        public string GetVariationContent(Int64 ContentId)
        {
            string VariationContent = _ObjContractCentralService.GetVariationContent(ContentId);

            return VariationContent;
        }

        public JsonResult GetContentMarket(Int64 ContentId)
        {
            var ContentMarketList = _ObjContractCentralService.GetMarketsForContent(ContentId).ToList();

            return Json(ContentMarketList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarketListForOtherVariations(Int64 ContentId)
        {
            var ContentMarketList = _ObjContractCentralService.GetMarketListForOtherVariations(ContentId).ToList();

            return Json(ContentMarketList, JsonRequestBehavior.AllowGet);
        }

        public long SaveVariation(Int64 ContractId, Int64 SectionId, Int64 ContentId, string DisplayValue)
        {
            long rContentId = _ObjContractCentralService.SaveVariation(ContractId, SectionId, ContentId, DisplayValue);

            return rContentId;
        }

        public void SaveContent(Int64 ContentId, string ContentText)
        {
            _ObjContractCentralService.SaveContent(ContentId, ContentText);
        }

        public void SaveContentMarket(Int64 ContentId, Int64[] MarketIdList)
        {
            List<ContentMarket> ContentMarketList = new List<ContentMarket>();
            foreach (Int64 MarketId in MarketIdList)
            {
                ContentMarket CM = new ContentMarket();

                CM.ContentId = ContentId;
                CM.MarketId = MarketId;
                ContentMarketList.Add(CM);
            }
            _ObjContractCentralService.SaveContentMarket(ContentId, ContentMarketList);
        }

        public void DisassociateContentMarket(Int64 ContentId, Int64 MarketId)
        {
            _ObjContractCentralService.DisassociateContentMarket(ContentId, MarketId);
        }

        public void DeleteVariation(Int64 ContentId)
        {
            _ObjContractCentralService.DeleteVariation(ContentId);
        }

        public void DeleteSection(Int64 ContractId, Int64 SectionId)
        {
            _ObjContractCentralService.DeleteSection(ContractId, SectionId);
        }

        public void CopyVariation(Int64 CopyFromContentId, Int64 CopyToContentId)
        {
            _ObjContractCentralService.CopyVariation(CopyFromContentId, CopyToContentId);
        }

        #region "Content Attachments"
        public int GetContentAttachmentCount(Int64 ContentId)
        {
            int ContentAttachmentCount = _ObjContractCentralService.GetAttachmentListForContract(ContentId).Count();

            return ContentAttachmentCount;
        }

        public ActionResult GetContentAttachmentListHTML(Int64 ContentId)
        {
            var ContentAttachmentList = _ObjContractCentralService.GetAttachmentListForContract(ContentId)
                                                            .Select(obj => new ContentAttachmentViewModel
                                                                    {
                                                                        ContentAttachmentId = obj.ContentAttachmentId,
                                                                        ContentId = obj.ContentId,
                                                                        AttachmentId = obj.AttachmentId,
                                                                        FileName = (obj.VirtualAttachment == false ? (_ObjContractCentralService.GetAttachment(obj.AttachmentId).FileName) : System.IO.Path.GetFileName(obj.AbsolutePath)),
                                                                        FileSize = (obj.VirtualAttachment == false ? String.Concat((_ObjContractCentralService.GetAttachment(obj.AttachmentId).SizeInBytes / 1024).ToString(), " KB") : "Unknown"),
                                                                        DisplayValue = obj.DisplayValue,
                                                                        Description = obj.Description,
                                                                        VersionName = obj.VersionName,
                                                                        VirtualAttachment = obj.VirtualAttachment,
                                                                        AbsolutePath = obj.AbsolutePath,
                                                                        CreatedDate = obj.CreatedOn
                                                                    });

            ViewBag.ContentAttachmentList = ContentAttachmentList;

            return PartialView("_ContentAttachmentInfo");
        }

        public void SaveContentAttachment(Int64 ContentAttachmentId, Int64 ContentId, Int64 AttachmentId, string ContentAttachmentTitle, string ContentAttachmentDescription, string ContentAttachmentVersion, string ExternalURL)
        {
            _ObjContractCentralService.SaveContentAttachment(ContentAttachmentId, ContentId, AttachmentId, ContentAttachmentTitle, ContentAttachmentDescription, ContentAttachmentVersion, ExternalURL);
        }
        
        public void DeleteContentAttachment(Int64 ContentAttachmentId)
        {
            _ObjContractCentralService.DeleteContentAttachment(ContentAttachmentId);
        }
        #endregion

        #region "Attachments"
        public ActionResult GetAttachmentLibraryHTML(string SearchText)
        {
            var AttachmentList = _ObjContractCentralService.GetAttachmentList(SearchText).ToList();
            ViewBag.AttachmentList = AttachmentList;

            return PartialView("_AttachmentList");
        }

        public ActionResult GetAttachmentLibraryHTMLFilterFileType(string SearchText)
        {
            var AttachmentList = _ObjContractCentralService.FilterAttachmentListFileType(SearchText).ToList();
            ViewBag.AttachmentList = AttachmentList;

            return PartialView("_AttachmentList");
        }

        public long UploadFileToLibrary(IEnumerable<HttpPostedFileBase> UploadAttachment)
        {
            long AttachmentId = 0;

            if (UploadAttachment != null)
            {
                var fileName = "";
                var fileExt = "";

                foreach (var file in UploadAttachment)
                {
                    fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    fileExt = Path.GetExtension(file.FileName);

                    var dir = Server.MapPath("~/App_Data/");
                    var subDir = Server.MapPath("~/App_Data/ContractCentralLibrary");

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        Directory.CreateDirectory(subDir);
                    }

                    if (!Directory.Exists(subDir))
                    {
                        Directory.CreateDirectory(subDir);
                    }

                    var physicalPath = Path.Combine(subDir, file.FileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                    
                    file.SaveAs(physicalPath);
                    
                    long FileSize = file.ContentLength;

                    AttachmentId = _ObjContractCentralService.SaveAttachment(0, "","", file.FileName, physicalPath, fileExt, FileSize, "");
                }
            }

            return AttachmentId;
        }

        public long ReplaceFileInLibrary(IEnumerable<HttpPostedFileBase> ReplaceAttachment, Int64 AttachmentId)
        {
            if (AttachmentId > 0)                               //REPLACE EXISTING FILE
            {
                Attachment Att = _ObjContractCentralService.GetAttachment(AttachmentId);
                System.IO.File.Delete(Att.AbsolutePath);
            }

            if (ReplaceAttachment != null)
            {
                var fileName = "";
                var fileExt = "";

                foreach (var file in ReplaceAttachment)
                {
                    fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    fileExt = Path.GetExtension(file.FileName);

                    var dir = Server.MapPath("~/App_Data/");
                    var subDir = Server.MapPath("~/App_Data/ContractCentralLibrary");

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        Directory.CreateDirectory(subDir);
                    }

                    if (!Directory.Exists(subDir))
                    {
                        Directory.CreateDirectory(subDir);
                    }

                    var physicalPath = Path.Combine(subDir, file.FileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }

                    file.SaveAs(physicalPath);

                    long FileSize = file.ContentLength;

                    AttachmentId = _ObjContractCentralService.SaveAttachment(AttachmentId, "", "", file.FileName, physicalPath, fileExt, FileSize, "");
                }
            }

            return AttachmentId;
        }

        public FileResult DownloadAttachment(Int64 AttachmentId)
        {
            Attachment Att = _ObjContractCentralService.GetAttachment(AttachmentId);
            string FileName = Att.FileName;

            return File(Att.AbsolutePath, MimeMapping.GetMimeMapping(FileName), FileName);
        }

        public void SaveAttachmentDetails(Int64 AttachmentId, string AttachmentTitle, string AttachmentDescription, string AttachmentVersion)
        {
            _ObjContractCentralService.SaveAttachmentDetails(AttachmentId, AttachmentTitle, AttachmentDescription, AttachmentVersion);
        }

        public void DeleteAttachment(Int64 AttachmentId)
        {
            _ObjContractCentralService.DeleteAttachment(AttachmentId);
        }

        #endregion
    }
}