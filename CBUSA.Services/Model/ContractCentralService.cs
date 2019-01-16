using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;
using CBUSA.Repository.Model;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Entity;

namespace CBUSA.Services.Model
{
    public class ContractCentralService : IContractCentralService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public ContractCentralService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Lu_App_ContentSection> GetAllContentSections()
        {
           return _ObjUnitWork.LuAppContentSection.GetAll().Where(cs => cs.RowStatusId == (int)RowActiveStatus.Active);
        }

        public IEnumerable<CBUSA.Repository.Model.ContentSectionRepository> GetAllContentSectionsForContract(long ContractId)
        {
            var result = _ObjUnitWork.LuAppContentSection.GetAll()
                                        .Where(cs => cs.RowStatusId == (int)RowActiveStatus.Active)
                                        .Select(
                                            cs => new ContentSectionRepository
                                            {
                                                    SectionId = cs.SectionId,
                                                    DisplayValue = cs.DisplayValue,
                                                    SortValue = cs.SortValue,
                                                    Icon = cs.Icon,
                                                    AutoAdd = cs.AutoAdd,
                                                    AboveBar = cs.AboveBar,
                                                    ToolTip = cs.ToolTip,
                                                    JoinedOnly = cs.JoinedOnly,
                                                    InternalOnly = cs.InternalOnly,
                                                    ContentCount = _ObjUnitWork.Content.GetAll().Where(c => c.ContractId == ContractId && c.SectionId == cs.SectionId && c.RowStatusId == (int)RowActiveStatus.Active).Count()
                                                }).OrderBy(cs => cs.SortValue).ToList();

            return result;
        }

        public IEnumerable<Lu_App_ContentSection> GetContentSectionsForContractMarket(long ContractId, long MarketId)
        {
            var SectionIdList = _ObjUnitWork.ContentMarket.GetAll().Where(cm => cm.MarketId == MarketId)
                                                            .Join(_ObjUnitWork.Content.GetAll()
                                                                    .Where(c => c.ContractId == ContractId), x => x.ContentId, y => y.ContentId, 
                                                                        (x, y) => new { y.SectionId }).ToList();

            IEnumerable<Lu_App_ContentSection> result = _ObjUnitWork.LuAppContentSection.GetAll()
                                                                        .Where(cs => cs.RowStatusId == (int)RowActiveStatus.Active && cs.InternalOnly == false)
                                                                        .Join(SectionIdList, x => x.SectionId, y => y.SectionId, (x, y) => x).OrderBy(x => x.SortValue);
            return result;
        }

        public IEnumerable<Content> GetContentForContractMarket(long ContractId, long MarketId)
        {
            IEnumerable<Content> ContentList = _ObjUnitWork.ContentMarket.GetAll().Where(cm => cm.MarketId == MarketId)
                                                            .Join(_ObjUnitWork.Content.GetAll()
                                                                    .Where(c => c.ContractId == ContractId), x => x.ContentId, y => y.ContentId, (x, y) => y)
                                                                        .Join(_ObjUnitWork.LuAppContentSection.GetAll()
                                                                                            .Where(cs => cs.InternalOnly == false && cs.RowStatusId == (int)RowActiveStatus.Active),
                                                                                            m => m.SectionId, n => n.SectionId, (m, n) => m).OrderBy(c => _ObjUnitWork.LuAppContentSection.Get(c.SectionId).SortValue);

            return ContentList;
        }

        public IEnumerable<Content> GetContentForNonJoineeContractMarket(long ContractId, long MarketId)
        {
            IEnumerable<Content> ContentList = _ObjUnitWork.ContentMarket.GetAll().Where(cm => cm.MarketId == MarketId)
                                                            .Join(_ObjUnitWork.Content.GetAll()
                                                                    .Where(c => c.ContractId == ContractId), x => x.ContentId, y => y.ContentId, (x, y) => y)
                                                                        .Join(_ObjUnitWork.LuAppContentSection.GetAll()
                                                                                            .Where(cs => cs.InternalOnly == false && cs.JoinedOnly == false && cs.RowStatusId == (int)RowActiveStatus.Active),
                                                                                            m => m.SectionId, n => n.SectionId, (m, n) => m).OrderBy(c => _ObjUnitWork.LuAppContentSection.Get(c.SectionId).SortValue);

            return ContentList;
        }

        public IEnumerable<dynamic> GetContentVariationsForContractSection(long ContractId, long SectionId)
        {
            var result = _ObjUnitWork.Content.GetAll().Where(c => c.ContractId == ContractId
                                                        && c.SectionId == SectionId
                                                        && c.RowStatusId == (int)RowActiveStatus.Active)
                                                        .Select(c => new { ContentId = c.ContentId, DisplayValue = c.DisplayValue }).ToList();
            return result;
        }

        public string GetVariationContent(long ContentId)
        {
            return _ObjUnitWork.Content.GetAll().Where(c => c.ContentId == ContentId).FirstOrDefault().ContentText;
        }

        public IEnumerable<ContentMarket> GetMarketsForContent(long ContentId)
        {
            return _ObjUnitWork.Content.GetAll().Where(c => c.ContentId == ContentId).FirstOrDefault().ContentMarket;
        }

        public IEnumerable<ContentMarket> GetMarketListForOtherVariations(long ContentId)
        {
            Content C = _ObjUnitWork.Content.GetAll().Where(c => c.ContentId == ContentId).FirstOrDefault();
            var AllOtherContentVariations = _ObjUnitWork.Content.GetAll().Where(c => c.ContentId != ContentId && c.SectionId == C.SectionId && c.ContractId == C.ContractId).Select(c => c.ContentId);

            return _ObjUnitWork.ContentMarket.GetAll().Where(cm => AllOtherContentVariations.Contains(cm.ContentId));
        }

        public long SaveVariation(long ContractId, long SectionId, long ContentId, string DisplayValue)
        {
            Content C = new Content();

            if (ContentId != 0)
            {
                C = _ObjUnitWork.Content.Search(c => c.ContentId == ContentId).FirstOrDefault();
                C.DisplayValue = DisplayValue;
                C.SortValue = DisplayValue;
                C.ModifiedOn = DateTime.Now;

                _ObjUnitWork.Content.Update(C);
            }
            else
            {
                C.ContractId = ContractId;
                C.SectionId = SectionId;
                C.DisplayValue = DisplayValue;
                C.SortValue = DisplayValue;

                C.RowStatusId = (int)RowActiveStatus.Active;
                C.CreatedBy = 1;
                C.ModifiedBy = 1;
                C.CreatedOn = DateTime.Now;
                C.ModifiedOn = DateTime.Now;
                C.RowGUID = Guid.NewGuid();

                _ObjUnitWork.Content.Add(C);
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();

            return C.ContentId;
        }

        public void SaveContent(long ContentId, string ContentText)
        {
            Content C = _ObjUnitWork.Content.Search(c => c.ContentId == ContentId).FirstOrDefault();

            C.ContentText = ContentText;
            C.ModifiedOn = DateTime.Now;

            _ObjUnitWork.Content.Update(C);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void SaveContentMarket(long ContentId, List<ContentMarket> ContentMarketList)
        {
            Content C = _ObjUnitWork.Content.Search(c => c.ContentId == ContentId).FirstOrDefault();

            foreach (ContentMarket CM in C.ContentMarket.ToList())
            {
                _ObjUnitWork.ContentMarket.Remove(CM);
            }

            List<ContentMarket> CMList = new List<ContentMarket>();
            
            foreach (ContentMarket CM in ContentMarketList)
            {
                CMList.Add(CM);
            }

            C.ContentMarket = CMList;

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DisassociateContentMarket(long ContentId, long MarketId)
        {
            ContentMarket CM = _ObjUnitWork.ContentMarket.GetAll().Where(cm => cm.ContentId == ContentId && cm.MarketId == MarketId).FirstOrDefault();

            _ObjUnitWork.ContentMarket.Remove(CM);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteVariation(long ContentId)
        {
            Content C = _ObjUnitWork.Content.Search(c => c.ContentId == ContentId).FirstOrDefault();

            foreach (ContentMarket CM in C.ContentMarket.ToList())
            {
                _ObjUnitWork.ContentMarket.Remove(CM);
            }

            foreach (ContentAttachment CA in C.ContentAttachment.ToList())
            {
                _ObjUnitWork.ContentAttachment.Remove(CA);
            }

            foreach (ContentImage CI in C.ContentImage.ToList())
            {
                _ObjUnitWork.ContentImage.Remove(CI);
            }

            _ObjUnitWork.Content.Remove(C);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteSection(long ContractId, long SectionId)
        {
            List<Content> ContentList = _ObjUnitWork.Content.Search(c => c.ContractId == ContractId && c.SectionId == SectionId).ToList();

            foreach (Content objC in ContentList)
            {
                foreach (ContentAttachment CA in objC.ContentAttachment.ToList())
                {
                    _ObjUnitWork.ContentAttachment.Remove(CA);
                }

                foreach (ContentImage CI in objC.ContentImage.ToList())
                {
                    _ObjUnitWork.ContentImage.Remove(CI);
                }

                foreach (ContentMarket CM in objC.ContentMarket.ToList())
                {
                    _ObjUnitWork.ContentMarket.Remove(CM);
                }

                _ObjUnitWork.Content.Remove(objC);
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void CopyVariation(long CopyFromContentId, long CopyToContentId)
        {
            Content ConCopyFrom = _ObjUnitWork.Content.Get(CopyFromContentId);          //GET CONTENT SPECS TO COPY FROM
            Content ConCopyTo = _ObjUnitWork.Content.Get(CopyToContentId);              //GET CONTENT SPECS TO COPY TO

            //DELETE ALL CONTENT ATTACHMENTS AND IMAGES BEFORE COPYING
            foreach (ContentAttachment CA in ConCopyTo.ContentAttachment.ToList())
            {
                _ObjUnitWork.ContentAttachment.Remove(CA);
            }

            foreach (ContentImage CI in ConCopyTo.ContentImage.ToList())
            {
                _ObjUnitWork.ContentImage.Remove(CI);
            }

            //COPY ATTACHMENTS, IMAGES FROM ORIGINAL CONTENT 
            foreach (ContentAttachment CA in ConCopyFrom.ContentAttachment.ToList())
            {
                ContentAttachment CACopyTo = new ContentAttachment();

                CACopyTo.ContentId = CopyToContentId;
                CACopyTo.AttachmentId = CA.AttachmentId;
                CACopyTo.AbsolutePath = CA.AbsolutePath;
                CACopyTo.Description = CA.Description;
                CACopyTo.DisplayValue = CA.DisplayValue;
                CACopyTo.VersionName = CA.VersionName;
                CACopyTo.VirtualAttachment = CA.VirtualAttachment;

                CACopyTo.RowStatusId = (int)RowActiveStatus.Active;
                CACopyTo.CreatedBy = 1;
                CACopyTo.ModifiedBy = 1;
                CACopyTo.CreatedOn = DateTime.Now;
                CACopyTo.ModifiedOn = DateTime.Now;
                CACopyTo.RowGUID = Guid.NewGuid();

                _ObjUnitWork.ContentAttachment.Add(CACopyTo);
            }

            foreach (ContentImage CI in ConCopyFrom.ContentImage.ToList())
            {
                ContentImage CICopyTo = new ContentImage();
                CICopyTo.ContentId = CopyToContentId;
                CICopyTo.ImageId = CI.ImageId;

                _ObjUnitWork.ContentImage.Add(CICopyTo);
            }

            ConCopyTo.ContentText = ConCopyFrom.ContentText;

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public IEnumerable<Attachment> GetAttachmentList(string SearchText)
        {
            IEnumerable<Attachment> AttachmentList;

            if (SearchText == null || SearchText == "")
            {
                AttachmentList = _ObjUnitWork.Attachment.GetAll()
                                                        .Where(att => att.RowStatusId == (int)RowActiveStatus.Active)
                                                        .OrderBy(att => att.FileName);
            }
            else
            {                
                AttachmentList = _ObjUnitWork.Attachment.GetAll()
                                                        .Where(att => att.RowStatusId == (int)RowActiveStatus.Active
                                                                && (att.DisplayValue.ToLower().Contains(SearchText.ToLower())
                                                                || att.Description.ToLower().Contains(SearchText.ToLower())
                                                                || att.VersionName.ToLower().Contains(SearchText.ToLower()))
                                                                || att.FileName.ToLower().Contains(SearchText.ToLower()))
                                                        .OrderBy(att => att.FileName);
            }

            return AttachmentList;
        }

        public IEnumerable<Attachment> FilterAttachmentListFileType(string SearchText)
        {
            IEnumerable<Attachment> AttachmentList;

            if (SearchText == null || SearchText == "")
            {
                AttachmentList = _ObjUnitWork.Attachment.GetAll()
                                                        .Where(att => att.RowStatusId == (int)RowActiveStatus.Active)
                                                        .OrderBy(att => att.Format).ThenBy(att => att.FileName);
            }
            else
            {
                AttachmentList = _ObjUnitWork.Attachment.GetAll()
                                                        .Where(att => att.RowStatusId == (int)RowActiveStatus.Active
                                                                && (att.DisplayValue.ToLower().Contains(SearchText.ToLower())
                                                                || att.Description.ToLower().Contains(SearchText.ToLower())
                                                                || att.VersionName.ToLower().Contains(SearchText.ToLower()))
                                                                || att.FileName.ToLower().Contains(SearchText.ToLower()))
                                                        .OrderBy(att => att.Format).ThenBy(att => att.FileName);
            }

            return AttachmentList;
        }

        public Attachment GetAttachment(long AttachmentId)
        {
            return _ObjUnitWork.Attachment.Get(AttachmentId);
        }

        public IEnumerable<ContentAttachment> GetAttachmentListForContract(long ContentId)
        {
            IEnumerable<ContentAttachment> AttachmentList = _ObjUnitWork.ContentAttachment.GetAll()
                                                            .Where(att => att.ContentId == ContentId && att.RowStatusId == (int)RowActiveStatus.Active)
                                                            .OrderBy(att => att.DisplayValue);

            return AttachmentList;
        }

        public long SaveAttachment(long AttachmentId, string Title, string Description, string FileName, string FilePath, string FileFormat, long SizeInBytes, string Version)
        {
            Attachment objAttachment = new Attachment();

            if (AttachmentId != 0)
            {
                objAttachment = _ObjUnitWork.Attachment.Get(AttachmentId);

                objAttachment.DisplayValue = System.IO.Path.GetFileNameWithoutExtension(FileName);
                objAttachment.FileName = FileName;
                objAttachment.AbsolutePath = FilePath;
                objAttachment.Format = FileFormat;
                objAttachment.SizeInBytes = SizeInBytes;
                objAttachment.VirtualAttachment = false;
                objAttachment.ModifiedOn = DateTime.Now;

                _ObjUnitWork.Attachment.Update(objAttachment);
            }
            else
            {
                objAttachment.DisplayValue = System.IO.Path.GetFileNameWithoutExtension(FileName);
                objAttachment.FileName = FileName;
                objAttachment.AbsolutePath = FilePath;
                objAttachment.Format = FileFormat;
                objAttachment.SizeInBytes = SizeInBytes;
                objAttachment.VirtualAttachment = false;

                objAttachment.RowStatusId = (int)RowActiveStatus.Active;
                objAttachment.CreatedBy = 1;
                objAttachment.ModifiedBy = 1;
                objAttachment.CreatedOn = DateTime.Now;
                objAttachment.ModifiedOn = DateTime.Now;
                objAttachment.RowGUID = Guid.NewGuid();

                _ObjUnitWork.Attachment.Add(objAttachment);
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();

            return objAttachment.AttachmentId;
        }

        public void SaveAttachmentDetails(long AttachmentId, string AttachmentTitle, string AttachmentDescription, string AttachmentVersion)
        {
            Attachment objAtt = _ObjUnitWork.Attachment.Search(att => att.AttachmentId == AttachmentId).FirstOrDefault();

            objAtt.DisplayValue = AttachmentTitle;
            objAtt.Description = AttachmentDescription;
            objAtt.VersionName = AttachmentVersion;
            objAtt.ModifiedOn = DateTime.Now;

            _ObjUnitWork.Attachment.Update(objAtt);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteAttachment(long AttachmentId)
        {
            List<ContentAttachment> ContentAttachmentList = _ObjUnitWork.ContentAttachment.Search(att => att.AttachmentId == AttachmentId).ToList();

            foreach (ContentAttachment objCA in ContentAttachmentList)
            {
                _ObjUnitWork.ContentAttachment.Remove(objCA);
            }

            Attachment objAtt = _ObjUnitWork.Attachment.Get(AttachmentId);
            _ObjUnitWork.Attachment.Remove(objAtt);

            System.IO.File.Delete(objAtt.AbsolutePath);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void SaveContentAttachment(long ContentAttachmentId, long ContentId, long AttachmentId, string ContentAttachmentTitle, string ContentAttachmentDescription, string ContentAttachmentVersion, string ExternalURL)
        {
            ContentAttachment objConAtt = new ContentAttachment();

            if (ContentAttachmentId != 0)
            {
                objConAtt = _ObjUnitWork.ContentAttachment.Get(ContentAttachmentId);

                if (AttachmentId > 0)
                {
                    objConAtt.AttachmentId = AttachmentId;
                }

                objConAtt.DisplayValue = ContentAttachmentTitle;
                objConAtt.Description = ContentAttachmentDescription;
                objConAtt.VersionName = ContentAttachmentVersion;

                if (ExternalURL != "")
                {
                    objConAtt.AbsolutePath = ExternalURL;
                    objConAtt.VirtualAttachment = true;
                }
                
                objConAtt.ModifiedOn = DateTime.Now;

                _ObjUnitWork.ContentAttachment.Update(objConAtt);
            }
            else
            {
                objConAtt.ContentId = ContentId;

                if (AttachmentId > 0)
                {
                    objConAtt.AttachmentId = AttachmentId;
                }

                objConAtt.DisplayValue = ContentAttachmentTitle;
                objConAtt.Description = ContentAttachmentDescription;
                objConAtt.VersionName = ContentAttachmentVersion;

                if (ExternalURL != "")
                {
                    objConAtt.AbsolutePath = ExternalURL;
                    objConAtt.VirtualAttachment = true;
                }
                else
                {
                    objConAtt.VirtualAttachment = false;
                }

                objConAtt.RowStatusId = (int)RowActiveStatus.Active;
                objConAtt.CreatedBy = 1;
                objConAtt.ModifiedBy = 1;
                objConAtt.CreatedOn = DateTime.Now;
                objConAtt.ModifiedOn = DateTime.Now;
                objConAtt.RowGUID = Guid.NewGuid();

                _ObjUnitWork.ContentAttachment.Add(objConAtt);
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteContentAttachment(long ContentAttachmentId)
        {
            ContentAttachment objAtt = _ObjUnitWork.ContentAttachment.Get(ContentAttachmentId);
            _ObjUnitWork.ContentAttachment.Remove(objAtt);

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public IEnumerable<Attachment> GetAttachmentList()
        {
            IEnumerable<Attachment> AttachmentList = _ObjUnitWork.Attachment.GetAll()
                                                                .Where(att => att.RowStatusId == (int)RowActiveStatus.Active)
                                                                .OrderBy(att => att.FileName);

            return AttachmentList;
        }
    }
}
