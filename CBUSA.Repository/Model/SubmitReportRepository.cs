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
    public class SubmitReportRepository : Repository<SubmitReport>, ISubmitReportRepository
    {
        public SubmitReportRepository(CBUSADbContext Context)
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
        public IEnumerable<dynamic> GetMappedSumitReportData(Int64 BuilderId, string FromDate, string ToDate)
        {
            //var data = Context.DBQBBillDataReceived.Join(Context.CategoryMapping, x => x.c, y => y.ProductCategoryId, (x, y) => new { x, y })
            //    .Join(Context.DBQBCategoryDataReceived, s => s.x.BuilderCategoryId, p => p.TranId, (s, p) => new { s, p })
            //    .Where(n => n.s.x.BuilderId == BuilderId && n.s.x.RowStatusId == (int) RowActiveStatus.Active)
            //    .GroupBy(z => z.s.x.BuilderCategoryId).
            //    Select(m => new { m });
            //List<dynamic> ObjResult = new List<dynamic>();
            //foreach (var Item in data)
            //{
            //    ObjResult.Add(new
            //    {
            //        TranId = Item.m.Select(a => a.s.x.TranId).FirstOrDefault(),
            //        BuilderId = Item.m.Select(a => a.s.x.BuilderId).FirstOrDefault(),
            //        CBUSACategoryId = Item.m.Select(a => a.s.x.CBUSACategoryId).FirstOrDefault(),
            //        BuilderCategoryId = Item.m.Select(a => a.s.x.BuilderCategoryId).FirstOrDefault(),
            //        CBUSACategoryName = Item.m.Select(a => a.s.y.ProductCategoryName).FirstOrDefault(),
            //        BuilderCategoryName = Item.m.Select(a => a.p.Name).FirstOrDefault(),
            //    });
            //}

            //var data = Context.DBQBBillDataReceived.Where(n => n.BuilderId == BuilderId && n.TxnDate >= Convert.ToDateTime(FromDate) && n.TxnDate <= Convert.ToDateTime(ToDate))
            //                    .Select(m => new { m });
            var data = Context.DBQBBillDataReceived.Where(n => n.BuilderId == BuilderId )
                                .Select(m => new { m });
            List<dynamic> ObjResult = new List<dynamic>();
            foreach (var Item in data)
            {
                ObjResult.Add(new
                {
                    SubmitReportTranId = Item.m.QBTxnID,
                    SubmitReportTranDate = Item.m.TxnDate,
                    BuilderId = Item.m.BuilderId,
                    VendorId = Item.m.VendorRefListID,
                    VendorName = Item.m.VendorRefFullName,
                    CategoryId = 10001,
                    CategoryName = "Test Category",
                    BillAmountEntered = Item.m.AmountDue,
                    BillAmountPaid = Item.m.AmountDue,
                    SalesTaxAmount = 0,
                    BillAmountReported = Item.m.AmountDue,
                    Comments = "",
                    VendorType = "Mapped"
                });
            }
            return ObjResult;
        }
        public IEnumerable<dynamic> GetUnMappedSumitReportData(Int64 BuilderId, string FromDate, string ToDate)
        {
            //var data = Context.DBQBBillDataReceived.Join(Context.CategoryMapping, x => x.c, y => y.ProductCategoryId, (x, y) => new { x, y })
            //    .Join(Context.DBQBCategoryDataReceived, s => s.x.BuilderCategoryId, p => p.TranId, (s, p) => new { s, p })
            //    .Where(n => n.s.x.BuilderId == BuilderId && n.s.x.RowStatusId == (int) RowActiveStatus.Active)
            //    .GroupBy(z => z.s.x.BuilderCategoryId).
            //    Select(m => new { m });
            //List<dynamic> ObjResult = new List<dynamic>();
            //foreach (var Item in data)
            //{
            //    ObjResult.Add(new
            //    {
            //        TranId = Item.m.Select(a => a.s.x.TranId).FirstOrDefault(),
            //        BuilderId = Item.m.Select(a => a.s.x.BuilderId).FirstOrDefault(),
            //        CBUSACategoryId = Item.m.Select(a => a.s.x.CBUSACategoryId).FirstOrDefault(),
            //        BuilderCategoryId = Item.m.Select(a => a.s.x.BuilderCategoryId).FirstOrDefault(),
            //        CBUSACategoryName = Item.m.Select(a => a.s.y.ProductCategoryName).FirstOrDefault(),
            //        BuilderCategoryName = Item.m.Select(a => a.p.Name).FirstOrDefault(),
            //    });
            //}

            //var data = Context.DBQBBillDataReceived.Where(n => n.BuilderId == BuilderId && n.TxnDate >= Convert.ToDateTime(FromDate) && n.TxnDate <= Convert.ToDateTime(ToDate))
            //                    .Select(m => new { m });
            var data = Context.DBQBBillDataReceived.Where(n => n.BuilderId == BuilderId)
                                .Select(m => new { m });
            List<dynamic> ObjResult = new List<dynamic>();
            foreach (var Item in data)
            {
                ObjResult.Add(new
                {
                    SubmitReportTranId = Item.m.QBTxnID,
                    SubmitReportTranDate = Item.m.TxnDate,
                    BuilderId = Item.m.BuilderId,
                    VendorId = Item.m.VendorRefListID,
                    VendorName = Item.m.VendorRefFullName,
                    CategoryId = 10002,
                    CategoryName = "not found",
                    BillAmountEntered = Item.m.AmountDue,
                    BillAmountPaid = Item.m.AmountDue,
                    SalesTaxAmount = 0,
                    BillAmountReported = Item.m.AmountDue,
                    Comments = "",
                    VendorType = "Mapped"
                });
            }
            return ObjResult;
        }
    }
}
