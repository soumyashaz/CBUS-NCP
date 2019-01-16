using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    class ImageRepository: Repository<Image>, IImageRepository
    {
        public ImageRepository(CBUSADbContext Context): base(Context)
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

    class ImageTagRepository : Repository<ImageTag>, IImageTagRepository
    {
        public ImageTagRepository(CBUSADbContext Context) : base(Context)
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
