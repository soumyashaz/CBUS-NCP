using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface IContentRepository : IRepository<Content>
    {

    }

    public interface IContentMarketRepository : IRepository<ContentMarket>
    {

    }

    public interface IContentImageRepository : IRepository<ContentImage>
    {

    }

    public interface IContentAttachmentRepository : IRepository<ContentAttachment>
    {

    }
}
