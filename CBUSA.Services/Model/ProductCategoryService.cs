using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;

namespace CBUSA.Services.Model
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ProductCategoryService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<ProductCategory> GetProductCategory()
        {
            return _ObjUnitWork.ProductCategory.Search(x => x.RowStatusId == (int)RowActiveStatus.Active);
        }
    }
}
