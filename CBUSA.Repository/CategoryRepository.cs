using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(CBUSADbContext Context)
            : base(Context)
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
