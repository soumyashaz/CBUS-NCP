using CBUSA.Domain;
using CBUSA.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public class CategoryServices : CBUSA.Services.ICategoryServices
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public CategoryServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Category> GetCategory()
        {
            var list = _ObjUnitWork.Categories.GetAll();
            return list;
        }

        public void SaveCategory(Category ObjCategory)
        {
            _ObjUnitWork.Categories.Add(ObjCategory);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void EditCategory(Category ObjCategory)
        {
            _ObjUnitWork.Categories.Update(ObjCategory);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteCategory(Category ObjCategory)
        {
            _ObjUnitWork.Categories.Update(ObjCategory);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }
    }
}
