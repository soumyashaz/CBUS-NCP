using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System.Data.SqlClient;
using System.Reflection.Emit;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Dynamic;

namespace CBUSA.Repository.Model
{
    public class CategoryMappingRepository : Repository<CategoryMapping>, ICategoryMappingRepository
    {
        public CategoryMappingRepository(CBUSADbContext Context)
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



        public IEnumerable<dynamic> GetAllCategoryMappingList(Int64 BuilderId)
        {
            var data = Context.CategoryMapping.Join(Context.DbProductCategory, x => x.CBUSACategoryId, y => y.ProductCategoryId, (x, y) => new { x, y })
                .Join(Context.DBQBCategoryDataReceived, s => s.x.BuilderCategoryId, p => p.TranId, (s, p) => new { s, p })
                .Where(n => n.s.x.BuilderId == BuilderId &&  n.p.BuilderId == BuilderId && n.s.x.RowStatusId == (int) RowActiveStatus.Active)
                .GroupBy(z => z.s.x.BuilderCategoryId).
                Select(m => new { m });
            List<dynamic> ObjResult = new List<dynamic>();
            foreach (var Item in data)
            {
                ObjResult.Add(new
                {
                    TranId = Item.m.Select(a => a.s.x.TranId).FirstOrDefault(),
                    BuilderId = Item.m.Select(a => a.s.x.BuilderId).FirstOrDefault(),
                    CBUSACategoryId = Item.m.Select(a => a.s.x.CBUSACategoryId).FirstOrDefault(),
                    BuilderCategoryId = Item.m.Select(a => a.s.x.BuilderCategoryId).FirstOrDefault(),
                    CBUSACategoryName = Item.m.Select(a => a.s.y.ProductCategoryName).FirstOrDefault(),
                    BuilderCategoryName = Item.m.Select(a => a.p.AccountNumber + " - " + a.p.Name).FirstOrDefault(),
                });
            }
            return ObjResult;
        }
        public IEnumerable<dynamic> GetBuilderCategoryRemovedList(Int64 BuilderId)
        {
            var data = Context.BuilderCategoryRemoved.Join(Context.DBQBCategoryDataReceived, x => x.BuilderCategoryId, y => y.TranId, (x, y) => new { x, y })
                .Where(n => n.x.BuilderId == BuilderId && n.y.BuilderId == BuilderId && n.x.RowStatusId == (int) RowActiveStatus.Active)
                .GroupBy(z => z.x.BuilderCategoryId).
                Select(m => new { m });
            List<dynamic> ObjResult = new List<dynamic>();
            foreach (var Item in data)
            {
                ObjResult.Add(new
                {
                    TranId = Item.m.Select(a => a.x.TranId).FirstOrDefault(),
                    BuilderId = Item.m.Select(a => a.x.BuilderId).FirstOrDefault(),
                    BuilderCategoryId = Item.m.Select(a => a.x.BuilderCategoryId).FirstOrDefault(),
                    BuilderCategoryName = Item.m.Select(a => a.y.AccountNumber + " - " + a.y.Name).FirstOrDefault()
                });
            }
            return ObjResult;
        }
        public IEnumerable<dynamic> GetCBUSACategoryListUnMapped(Int64 BuilderId)
        {
            var temp = Context.DbProductCategory
                             .Where(x => !Context.CategoryMapping.Any(y => y.CBUSACategoryId == x.ProductCategoryId && y.BuilderId == BuilderId))
                             .Select(x => new { CategoryId = x.ProductCategoryId, CompanyName = x.ProductCategoryName, RowStatusId = x.RowStatusId }).ToList();
            return temp;
        }
        public IEnumerable<dynamic> GetBuilderCategoryListUnMapped(Int64 BuilderId)
        {
            var temp = Context.DBQBCategoryDataReceived
                             .Where(x => !Context.CategoryMapping.Any(y => y.BuilderCategoryId == x.TranId && y.BuilderId == BuilderId) && !Context.BuilderCategoryRemoved.Any(z => z.BuilderCategoryId == x.TranId && z.BuilderId == BuilderId) && x.BuilderId == BuilderId)
                             .Select(x => new { CategoryId = x.TranId, CompanyName = x.AccountNumber + " - " + x.Name, RowStatusId = x.RowStatusId }).ToList();
            return temp;
        }
    }
}
