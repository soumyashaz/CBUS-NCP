using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    class ContentRepository: Repository<Content>, IContentRepository
    {
        public ContentRepository(CBUSADbContext Context): base(Context)
        {
        }
        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }

    }

    class ContentAttachmentRepository : Repository<ContentAttachment>, IContentAttachmentRepository
    {
        public ContentAttachmentRepository(CBUSADbContext Context) : base(Context)
        {
        }
        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }
    }

    class ContentMarketRepository : Repository<ContentMarket>, IContentMarketRepository
    {
        public ContentMarketRepository(CBUSADbContext Context) : base(Context)
        {
        }
        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }
    }

    class ContentImageRepository : Repository<ContentImage>, IContentImageRepository
    {
        public ContentImageRepository(CBUSADbContext Context) : base(Context)
        {
        }
        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }
    }
}
