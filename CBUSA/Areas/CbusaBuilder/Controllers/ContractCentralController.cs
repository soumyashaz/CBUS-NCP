using CBUSA.Domain;
using CBUSA.Areas.Admin.Models;
using CBUSA.Repository.Model;
using CBUSA.Services;
using CBUSA.Services.Interface;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using iTextSharp;
using System.IO;
using System.IO.Compression;

namespace CBUSA.Areas.CbusaBuilder.Controllers
{
    public class ContractCentralController : Controller
    {
        readonly IMarketService _ObjMarketService;
        readonly IContractServices _ObjContractService;
        readonly IBuilderService _ObjBuilderService;
        readonly IContractBuilderService _ObjContractBuilderService;
        readonly IContractCentralService _ObjContractCentralService;

        public ContractCentralController(IMarketService ObjMarketService, IContractServices ObjContractService, IBuilderService ObjBuilderService,
                                         IContractBuilderService ObjContractBuilderService, IContractCentralService ObjContractCentralService)
        {
            _ObjMarketService = ObjMarketService;
            _ObjContractService = ObjContractService;
            _ObjBuilderService = ObjBuilderService;
            _ObjContractBuilderService = ObjContractBuilderService;
            _ObjContractCentralService = ObjContractCentralService;
        }

        // GET: CbusaBuilder/ContractCentral
        public ActionResult Index(string UserId)
        {
            bool IsBuilderAuthenticated = false;
            long BuilderId = 0;

            //-------------------- IDENTITY ------------------------
            string DecryptBuilderUserId = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(UserId));
            Int64 CBUSABuilderUserId = Convert.ToInt64(DecryptBuilderUserId);
            var User = _ObjBuilderService.IsUserAuthenticate(CBUSABuilderUserId);

            if (User != null)
            {
                var Builder = User.Builder;

                if (Builder.RowStatusId == (int)RowActiveStatus.Active)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Sid, User.BuilderId.ToString()));
                    claims.Add(new Claim(ClaimTypes.PrimarySid, User.BuilderUserId.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, User.FirstName + " " + User.LastName));
                    claims.Add(new Claim(ClaimTypes.Email, User.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Builder"));

                    var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                    var ctx = Request.GetOwinContext();
                    var authenticationManager = ctx.Authentication;
                    authenticationManager.SignIn(id);

                    IsBuilderAuthenticated = true;

                    BuilderId = Builder.BuilderId;

                    ViewBag.BuilderId = BuilderId;
                    ViewBag.BuilderName = Builder.BuilderName;
                    ViewBag.AccountName = String.Concat(User.FirstName, " " , User.LastName);

                    Session["BuilderId"] = BuilderId;
                }
            }
            //---------------------------------------------------

            return View();
        }

        public ActionResult LoadContractLogoList(long BuilderId)
        {
            PopulateEnrolledContracts(BuilderId);
            PopulateNotEnrolledContracts(BuilderId);

            return PartialView("_ContractLogoList");
        }

        public List<dynamic> PopulateEnrolledContracts(long BuilderId)
        {
            List<dynamic> lstContracts = new List<dynamic>();

            var BuilderContracts = _ObjContractBuilderService.GetActiveContractsofBuilder(BuilderId).Where(c => c.Contract.RowStatusId == (int)RowActiveStatus.Active);

            int TotalContracts = BuilderContracts.Count();
            int RemainderPage = 0;

            if (TotalContracts % 4 > 0)
                RemainderPage = 1;

            int TotalPages = (TotalContracts / 4) + RemainderPage;
            ViewBag.TotalEnrolledContracts = TotalContracts;
            ViewBag.TotalEnrolledPages = TotalPages;
            
            foreach (ContractBuilder objConBuilder in BuilderContracts)
            {
                object Contract = new Contract
                                    {
                                        ContractId = objConBuilder.ContractId,
                                        ContractName = objConBuilder.Contract.ContractName,
                                        ContractIcon = objConBuilder.Contract.ContractIcon
                                    };

                lstContracts.Add(Contract);
            }

            ViewBag.EnrolledContractLogos = lstContracts;

            return lstContracts;
        }

        public List<dynamic> PopulateNotEnrolledContracts(long BuilderId)
        {
            List<dynamic> lstContracts = new List<dynamic>();

            var NonAssociateContracts = _ObjContractService.GetNonAssociateContractWithBuilder(BuilderId, "act").Where(c => c.RowStatusId == (int)RowActiveStatus.Active);

            int TotalContracts = NonAssociateContracts.Count();
            int RemainderPage = 0;

            if (TotalContracts % 5 > 0)
                RemainderPage = 1;

            int TotalPages = (TotalContracts / 5) + RemainderPage;
            ViewBag.TotalNonEnrolledContracts = TotalContracts;
            ViewBag.TotalNonEnrolledPages = TotalPages;

            foreach (Contract objContract in NonAssociateContracts)
            {
                object Contract = new Contract
                {
                    ContractId = objContract.ContractId,
                    ContractName = objContract.ContractName,
                    ContractIcon = objContract.ContractIcon
                };

                lstContracts.Add(Contract);
            }

            ViewBag.NonEnrolledContractLogos = lstContracts;

            return lstContracts;
        }
        
        public ActionResult ViewDetail(Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            Builder B = _ObjBuilderService.GetSpecificBuilder(BuilderId);

            ViewBag.BuilderId = BuilderId;
            ViewBag.BuilderName = B.BuilderName;
            ViewBag.AccountName = identity.Name;

            PopulateEnrolledContracts(BuilderId);   //For populating the Contract drop-down            

            //*************** GET CONTRACT-BUILDER ASSOCIATION STATUS **************
            bool IsBuilderContractAssociated = false;

            ContractBuilder CB = _ObjContractBuilderService.GetAllContractofBuilder(BuilderId).Where(cb => cb.ContractId == ContractId).FirstOrDefault();

            if (CB != null && CB.ContractStatusId == (int)ContractActiveStatus.Active)
            {
                IsBuilderContractAssociated = true;
            }

            //*************** GET LIST OF CONTENT SECTIONS FOR GIVEN CONTRACT **************
            IEnumerable<Lu_App_ContentSection> AboveBarContentSection;
            IEnumerable<Lu_App_ContentSection> BelowBarContentSection;
            IEnumerable<Content> Content;

            if (IsBuilderContractAssociated)
            {
                AboveBarContentSection = _ObjContractCentralService.GetContentSectionsForContractMarket(ContractId, B.MarketId).Where(cs => cs.AboveBar == 1).OrderBy(cs => cs.SortValue);
                ViewBag.AboveBarContentSection = AboveBarContentSection;

                BelowBarContentSection = _ObjContractCentralService.GetContentSectionsForContractMarket(ContractId, B.MarketId).Where(cs => cs.AboveBar == 2).OrderBy(cs => cs.SortValue);
                ViewBag.BelowBarContentSection = BelowBarContentSection;

                Content = _ObjContractCentralService.GetContentForContractMarket(ContractId, B.MarketId);
            }
            else
            {
                AboveBarContentSection = _ObjContractCentralService.GetContentSectionsForContractMarket(ContractId, B.MarketId).Where(cs => cs.AboveBar == 1 && cs.JoinedOnly == false).OrderBy(cs => cs.SortValue);
                ViewBag.AboveBarContentSection = AboveBarContentSection;

                BelowBarContentSection = _ObjContractCentralService.GetContentSectionsForContractMarket(ContractId, B.MarketId).Where(cs => cs.AboveBar == 2 && cs.JoinedOnly == false).OrderBy(cs => cs.SortValue);
                ViewBag.BelowBarContentSection = BelowBarContentSection;

                Content = _ObjContractCentralService.GetContentForNonJoineeContractMarket(ContractId, B.MarketId);
            }

            foreach (Content objCon in Content)
            {
                String ContentSectionTitle = _ObjContractCentralService.GetAllContentSections().Where(cs => cs.SectionId == objCon.SectionId).FirstOrDefault().DisplayValue;
                String ContentText = objCon.ContentText.Replace("&lt;", "<").Replace("&gt;", ">");

                ViewData["Content_" + ContentSectionTitle] = ContentText;

                IEnumerable <ContentAttachmentViewModel> ContentAttachmentList = _ObjContractCentralService.GetAttachmentListForContract(objCon.ContentId)
                                                                                .Select(obj => new ContentAttachmentViewModel
                                                                                {
                                                                                    ContentAttachmentId = obj.ContentAttachmentId,
                                                                                    ContentId = obj.ContentId,
                                                                                    AttachmentId = obj.AttachmentId,
                                                                                    FileName = (obj.VirtualAttachment == false ? _ObjContractCentralService.GetAttachment(obj.AttachmentId).FileName : System.IO.Path.GetFileName(obj.AbsolutePath)),
                                                                                    FileSize = (obj.VirtualAttachment == false ? String.Concat((_ObjContractCentralService.GetAttachment(obj.AttachmentId).SizeInBytes / 1024).ToString(), " KB") : "Unknown"),
                                                                                    DisplayValue = obj.DisplayValue,
                                                                                    Description = obj.Description,
                                                                                    VersionName = obj.VersionName,
                                                                                    VirtualAttachment = obj.VirtualAttachment,
                                                                                    AbsolutePath = (obj.AbsolutePath == null ? _ObjContractCentralService.GetAttachment(obj.AttachmentId).AbsolutePath : obj.AbsolutePath),
                                                                                    CreatedDate = obj.CreatedOn
                                                                                });
                ViewData["ContentAttachment_" + ContentSectionTitle] = ContentAttachmentList;
            }
            //******************************************************************************

            //*************** GET CONTRACT DETAILS **************
            Contract C = _ObjContractService.GetContract(ContractId);

            ViewBag.ContractId = ContractId;
            ViewBag.ContractName = C.ContractName;
            ViewBag.ContractTerm = String.Concat(C.ContrctFrom.Value.ToShortDateString(), " to ", C.ContrctTo.Value.ToShortDateString());

            if (DateTime.Now.Date > C.ContrctFrom.Value.Date && DateTime.Now.Date < C.ContrctTo.Value.Date)
            {
                ViewBag.IsContractActive = true;
            }
            else
            {
                ViewBag.IsContractActive = false;
            }

            string LogoImageBase64 = "";
            LogoImageBase64 = Convert.ToBase64String(C.ContractIcon);
            LogoImageBase64 = string.Format("data:image/png;base64,{0}", LogoImageBase64);

            ViewBag.ContractLogo = LogoImageBase64;
            //******************************************************************************

            return View();
        }

        public FileResult PrintContentPDF(Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            Builder B = _ObjBuilderService.GetSpecificBuilder(BuilderId);

            //*************** GET CONTRACT-BUILDER ASSOCIATION STATUS **************
            bool IsBuilderContractAssociated = false;

            ContractBuilder CB = _ObjContractBuilderService.GetAllContractofBuilder(BuilderId).Where(cb => cb.ContractId == ContractId).FirstOrDefault();

            if (CB != null && CB.ContractStatusId == (int)ContractActiveStatus.Active)
            {
                IsBuilderContractAssociated = true;
            }

            //*************** GET LIST OF CONTENT SECTIONS FOR GIVEN CONTRACT **************
            IEnumerable<Content> Content;

            if (IsBuilderContractAssociated)
            {
                Content = _ObjContractCentralService.GetContentForContractMarket(ContractId, B.MarketId);
            }
            else
            {
                Content = _ObjContractCentralService.GetContentForNonJoineeContractMarket(ContractId, B.MarketId);
            }

            string CompiledText = "";

            foreach (Content objCon in Content)
            {
                String ContentSectionTitle = _ObjContractCentralService.GetAllContentSections().Where(cs => cs.SectionId == objCon.SectionId).FirstOrDefault().DisplayValue;
                String ContentSectionHTML = String.Concat("<div><h2><u>", ContentSectionTitle, "</u></h2></div>");

                String ContentText = objCon.ContentText.Replace("&lt;", "<").Replace("&gt;", ">");
                String ContentTextHTML = String.Concat("<div style=\"margin-bottom:20px;\">", ContentText, "</div><br /><br />");

                CompiledText = String.Concat(CompiledText, ContentSectionHTML, "", ContentTextHTML);
            }
            //******************************************************************************

            //*************** GET CONTRACT DETAILS **************
            Contract C = _ObjContractService.GetContract(ContractId);
            //***************************************************

            String ContractContentFilePath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), "ContractCentral", C.ContractName + ".pdf");

            if (System.IO.File.Exists(ContractContentFilePath))
            {
                System.IO.File.Delete(ContractContentFilePath);
            }

            iTextSharp.text.Document document = new iTextSharp.text.Document();
            string filePath = Server.MapPath("~/App_Data/ContractCentral/");
            iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(ContractContentFilePath, FileMode.Create));

            document.Open();
            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            hw.Parse(new StringReader(CompiledText));
            document.Close();

            return File(ContractContentFilePath, MimeMapping.GetMimeMapping(ContractContentFilePath), C.ContractName + ".pdf");
        }

        public FileResult GetAttachmentsZipFile(Int64 ContractId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            Builder B = _ObjBuilderService.GetSpecificBuilder(BuilderId);
            
            //*************** GET CONTRACT DETAILS **************
            Contract C = _ObjContractService.GetContract(ContractId);

            //*************** CREATE DIR FOR ZIP FILE **************
            string FileRepoDestinationPath = Server.MapPath("~/App_Data/ContractCentralZip/" + String.Concat(C.ContractName, "_", B.Market.MarketName));
            if (!Directory.Exists(FileRepoDestinationPath))
            {
                System.IO.Directory.CreateDirectory(FileRepoDestinationPath);
            }
            else
            {
                System.IO.Directory.Delete(FileRepoDestinationPath, true);
                System.IO.Directory.CreateDirectory(FileRepoDestinationPath);
            }

            //*************** CREATE ZIP FILE **************
            string ZipFileName = String.Concat(C.ContractName, "_", B.Market.MarketName, ".zip");
            string ZipLocation = Server.MapPath("~/App_Data/ContractCentral/" + ZipFileName);

            if (System.IO.File.Exists(ZipLocation))
            {
                System.IO.File.Delete(ZipLocation);
            }

            //*************** GET CONTRACT-BUILDER ASSOCIATION STATUS **************
            bool IsBuilderContractAssociated = false;

            ContractBuilder CB = _ObjContractBuilderService.GetAllContractofBuilder(BuilderId).Where(cb => cb.ContractId == ContractId).FirstOrDefault();

            if (CB != null && CB.ContractStatusId == (int)ContractActiveStatus.Active)
            {
                IsBuilderContractAssociated = true;
            }

            //*************** GET LIST OF CONTENT ATTACHMENTS FOR GIVEN CONTRACT/MARKET **************
            string FileRepoSourcePath = Server.MapPath("~/App_Data/ContractCentralLibrary");

            IEnumerable<Content> AllContractMarketContent;

            if (IsBuilderContractAssociated)
            {
                AllContractMarketContent = _ObjContractCentralService.GetContentForContractMarket(ContractId, B.MarketId);
            }
            else
            {
                AllContractMarketContent = _ObjContractCentralService.GetContentForNonJoineeContractMarket(ContractId, B.MarketId);
            }

            foreach (Content objContent in AllContractMarketContent)
            {
                IEnumerable<ContentAttachment> ContractMarketContentAttachment = _ObjContractCentralService.GetAttachmentListForContract(objContent.ContentId).Where(con => con.VirtualAttachment == false);

                foreach (ContentAttachment objContentAttachment in ContractMarketContentAttachment)
                {
                    Attachment Att = _ObjContractCentralService.GetAttachment(objContentAttachment.AttachmentId);

                    string FileCopyFromPath = Path.Combine(FileRepoSourcePath, Att.FileName);
                    string FileCopyToPath = Path.Combine(FileRepoDestinationPath, Att.FileName);

                    if (!System.IO.File.Exists(FileCopyToPath))
                    {
                        System.IO.File.Copy(FileCopyFromPath, FileCopyToPath);
                    }
                }
            }

            ZipFile.CreateFromDirectory(FileRepoDestinationPath, ZipLocation, CompressionLevel.Optimal, true);
            if (System.IO.File.Exists(ZipLocation))
            {
                return File(ZipLocation, MimeMapping.GetMimeMapping(ZipLocation), ZipFileName);
            }
            else
            {
                return null;
            }
        }
    }
}