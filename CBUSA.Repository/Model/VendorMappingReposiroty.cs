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
    class VendorMappingReposiroty : Repository<VendorMapping>, IVendorMappingRepository
    {
        public VendorMappingReposiroty(CBUSADbContext Context)
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
        public IEnumerable<dynamic> GetAllVendorMappingList(Int64 BuilderId)
        {
            var data = Context.VendorMapping.Join(Context.Vendor, x => x.CBUSAVendorId, y => y.VendorId, (x, y) => new { x, y })
                .Join(Context.DBQBVendorDataReceived, s => s.x.BuilderVendorId, p => p.TranId, (s, p) => new { s, p })
                .Where(n => n.s.x.BuilderId == BuilderId && n.p.BuilderId == BuilderId && n.s.x.RowStatusId == (int) RowActiveStatus.Active)
                .GroupBy(z => z.s.x.BuilderVendorId).
                Select(m => new { m });
            List<dynamic> ObjResult = new List<dynamic>();
            foreach (var Item in data)
            {
                ObjResult.Add(new
                {
                    TranId = Item.m.Select(a => a.s.x.TranId).FirstOrDefault(),
                    BuilderId = Item.m.Select(a => a.s.x.BuilderId).FirstOrDefault(),
                    CBUSAVendorId = Item.m.Select(a => a.s.x.CBUSAVendorId).FirstOrDefault(),
                    BuilderVendorId = Item.m.Select(a => a.s.x.BuilderVendorId).FirstOrDefault(),
                    CBUSAVendorName = Item.m.Select(a => a.s.y.CompanyName).FirstOrDefault(),
                    BuilderVendorName = Item.m.Select(a => a.p.Name).FirstOrDefault(),
                });
            }
            return ObjResult;
        }
        public IEnumerable<dynamic> GetBuilderVendorRemovedList(Int64 BuilderId)
        {
            var data = Context.BuilderVendorRemoved.Join(Context.DBQBVendorDataReceived, x => x.BuilderVendorId, y => y.TranId, (x, y) => new { x, y })
                .Where(n => n.x.BuilderId == BuilderId && n.y.BuilderId == BuilderId && n.x.RowStatusId == (int) RowActiveStatus.Active)
                .GroupBy(z => z.x.BuilderVendorId).
                Select(m => new { m });
            List<dynamic> ObjResult = new List<dynamic>();
            foreach (var Item in data)
            {
                ObjResult.Add(new
                {
                    TranId = Item.m.Select(a => a.x.TranId).FirstOrDefault(),
                    BuilderId = Item.m.Select(a => a.x.BuilderId).FirstOrDefault(),
                    BuilderVendorId = Item.m.Select(a => a.x.BuilderVendorId).FirstOrDefault(),
                    BuilderVendorName = Item.m.Select(a => a.y.Name).FirstOrDefault()
                });
            }
            return ObjResult;
        }
        public IEnumerable<dynamic> GetCBUSAVendorListUnMapped(Int64 BuilderId)
        {
            var temp = Context.Vendor
                             .Where(x => !Context.VendorMapping.Any(y => y.CBUSAVendorId == x.VendorId && y.BuilderId == BuilderId))
                             .Select(x => new { VendorId = x.VendorId, CompanyName = x.CompanyName, RowStatusId = x.RowStatusId }).ToList();
            return temp;
        }
        public IEnumerable<dynamic> GetBuilderVendorListUnMapped(Int64 BuilderId)
        {
            var temp = Context.DBQBVendorDataReceived
                             .Where(x => !Context.VendorMapping.Any(y => y.BuilderVendorId == x.TranId && y.BuilderId == BuilderId) && !Context.BuilderVendorRemoved.Any(z => z.BuilderVendorId == x.TranId && z.BuilderId == BuilderId) && x.BuilderId == BuilderId)
                             .Select(x => new { VendorId = x.TranId, CompanyName = x.Name, RowStatusId = x.RowStatusId }).ToList();
            return temp;
        }
    }
}
