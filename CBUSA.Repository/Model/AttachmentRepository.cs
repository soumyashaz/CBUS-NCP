using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(CBUSADbContext Context): base(Context)
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

    class AttachmentTagRepository : Repository<AttachmentTag>, IAttachmentTagRepository
    {
        public AttachmentTagRepository(CBUSADbContext Context) : base(Context)
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
