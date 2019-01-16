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
    class ProductService : IProductService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ProductService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }



        public IEnumerable<Product> GetProduct()
        {
            throw new NotImplementedException();
        }
    }
}
