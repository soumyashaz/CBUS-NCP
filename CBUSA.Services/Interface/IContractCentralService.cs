using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository;

namespace CBUSA.Services.Interface
{
    public interface IContractCentralService
    {
        IEnumerable<Lu_App_ContentSection> GetAllContentSections();
        IEnumerable<CBUSA.Repository.Model.ContentSectionRepository> GetAllContentSectionsForContract(long ContractId);
        IEnumerable<Lu_App_ContentSection> GetContentSectionsForContractMarket(long ContractId, long MarketId);
        IEnumerable<Content> GetContentForContractMarket(long ContractId, long MarketId);
        IEnumerable<Content> GetContentForNonJoineeContractMarket(long ContractId, long MarketId);
        IEnumerable<dynamic> GetContentVariationsForContractSection(long ContractId, long SectionId);
        string GetVariationContent(long ContentId);
        IEnumerable<ContentMarket> GetMarketsForContent(long ContentId);
        IEnumerable<ContentMarket> GetMarketListForOtherVariations(long ContentId);
        long SaveVariation(long ContractId, long SectionId, long ContentId, string DisplayValue);
        void SaveContent(long ContentId, string ContentText);
        void SaveContentMarket(long ContentId, List<ContentMarket> ContentMarketList);
        void DisassociateContentMarket(long ContentId, long MarketId);
        void DeleteVariation(long ContentId);
        void DeleteSection(long ContractId, long SectionId);
        void CopyVariation(long CopyFromContentId, long CopyToContentId);
        IEnumerable<Attachment> GetAttachmentList(string SearchText);
        IEnumerable<Attachment> FilterAttachmentListFileType(string SearchText);
        IEnumerable<ContentAttachment> GetAttachmentListForContract(long ContentId);
        Attachment GetAttachment(long AttachmentId);
        long SaveAttachment(long AttachmentId, string Title, string Description, string FileName, string FilePath, string FileFormat, long SizeInBytes, string Version);
        void SaveAttachmentDetails(long AttachmentId, string AttachmentTitle, string AttachmentDescription, string AttachmentVersion);
        void DeleteAttachment(long AttachmentId);
        void SaveContentAttachment(long ContentAttachmentId, long ContentId, long AttachmentId, string ContentAttachmentTitle, string ContentAttachmentDescription, string ContentAttachmentVersion, string ExternalURL);
        void DeleteContentAttachment(long ContentAttachmentId);
    }
}
