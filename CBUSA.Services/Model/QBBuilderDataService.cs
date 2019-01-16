using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Model
{
    public class QBBuilderDataService : IQBBuilderDataService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        private readonly CBUSADbContext dbContext = new CBUSADbContext();
        public QBBuilderDataService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public void UpdateQBBuilderVendorData(QBVendorDataReceived ObjQBVendorDataReceived, bool DisposeConn = false)
        {
            if (DisposeConn == true)
            {
                _ObjUnitWork.Dispose();
            }
            else
            {

                if (!dbContext.DBQBVendorDataReceived.Any(e => e.ListID == ObjQBVendorDataReceived.ListID))
                {
                    //_ObjUnitWork.QBVendorDataReceived.Add(ObjQBVendorDataReceived);
                    _ObjUnitWork.Complete();
                }
                //foreach(var data in Obj)
                //{                    
                //}
                //if (TranId != null)
                //{
                //    dbContext.DBQBVendorDataReceived.Attach(ObjQBVendorDataReceived);
                //    dbContext.Entry(ObjQBVendorDataReceived).State = System.Data.Entity.EntityState.Modified;
                //    ObjQBVendorDataReceived.ModifiedOn = System.DateTime.Now;
                //    ObjQBVendorDataReceived.ModifiedBy = 1;
                //    //ObjQBVendorDataReceived.TranId = Convert.ToInt64(TranId);
                //    dbContext.SaveChanges();
                //}
                //else
                //{
                //    _ObjUnitWork.QBVendorDataReceived.Add(ObjQBVendorDataReceived);
                //    _ObjUnitWork.Complete();
                //}                            
            }
        }
        public bool CheckBuilderAuthentication(Int64 BuilderId, string Passwrd)
        {
            Int32 rec = _ObjUnitWork.Builder.Find(a=> a.BuilderId == BuilderId).Count();
            if (rec >0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpdateQBBuilderBillData(QBBillDataReceived ObjQBBillDataReceived, bool DisposeConn = false)
        {
            if (DisposeConn == true)
            {
                _ObjUnitWork.Dispose();
            }
            else
            {
                if (dbContext.DBQBBillDataReceived.Any(e => e.QBTxnID == ObjQBBillDataReceived.QBTxnID))
                {
                    //dbContext.DBQBBillDataReceived.Attach(ObjQBBillDataReceived);
                    //dbContext.Entry(ObjQBBillDataReceived).State = System.Data.Entity.EntityState.Modified;
                    //ObjQBBillDataReceived.ModifiedOn = System.DateTime.Now;
                    //ObjQBBillDataReceived.ModifiedBy = 1;
                    //dbContext.SaveChanges();
                }
                else
                {
                   // _ObjUnitWork.QBBillDataReceived.Add(ObjQBBillDataReceived);
                    _ObjUnitWork.Complete();
                }
            }
        }
        public void UpdateQBBuilderCategoryData(QBCategoryDataReceived ObjQBCategoryDataReceived, bool DisposeConn = false)
        {
            if (DisposeConn == true)
            {
                _ObjUnitWork.Dispose();
            }
            else
            {
                if (dbContext.DBQBCategoryDataReceived.Any(e => e.ListID == ObjQBCategoryDataReceived.ListID))
                {
                    //dbContext.DBQBCategoryDataReceived.Attach(ObjQBCategoryDataReceived);
                    //dbContext.Entry(ObjQBCategoryDataReceived).State = System.Data.Entity.EntityState.Modified;
                    //ObjQBCategoryDataReceived.ModifiedOn = System.DateTime.Now;
                    //ObjQBCategoryDataReceived.ModifiedBy = 1;
                    //dbContext.SaveChanges();
                }
                else
                {
                   // _ObjUnitWork.QBCategoryDataReceived.Add(ObjQBCategoryDataReceived);
                    _ObjUnitWork.Complete();
                }
                //_ObjUnitWork.QBCategoryDataReceived.Add(ObjQBCategoryDataReceived);
                //_ObjUnitWork.Complete();
            }
        }
        public void UpdateQBBuilderInvoiceData(QBInvoiceDataReceived ObjQBInvoiceDataReceived, bool DisposeConn = false)
        {
            if (DisposeConn == true)
            {
                _ObjUnitWork.Dispose();
            }
            else
            {
                //if (dbContext.DBQBInvoiceDataReceived.Any(e => e.ListID == ObjQBInvoiceDataReceived.ListID))
                //{
                //    //dbContext.DBQBCategoryDataReceived.Attach(ObjQBCategoryDataReceived);
                //    //dbContext.Entry(ObjQBCategoryDataReceived).State = System.Data.Entity.EntityState.Modified;
                //    //ObjQBCategoryDataReceived.ModifiedOn = System.DateTime.Now;
                //    //ObjQBCategoryDataReceived.ModifiedBy = 1;
                //    //dbContext.SaveChanges();
                //}
                //else
                //{
                //    _ObjUnitWork.QBInvoiceDataReceived.Add(ObjQBInvoiceDataReceived);
                //    _ObjUnitWork.Complete();
                //}               
            }
        }
    }
}
