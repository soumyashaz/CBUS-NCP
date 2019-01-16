using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Services.Interface;
using CBUSA.Domain;
using CBUSA.Repository;
using System.Data.Entity;

namespace CBUSA.Services.Model
{
    public class BuilderIntegrationService : IBuilderIntegrationService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        private readonly CBUSADbContext dbContext = new CBUSADbContext();
        public BuilderIntegrationService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public string GetBuilderFirstNameLastName(Int64 BuilderId)
        {
            string data = _ObjUnitWork.BuilderUser.Search(x => x.BuilderId == BuilderId && x.RowStatusId == (int) RowActiveStatus.Active).Select(y=> y.FirstName + " " + y.LastName).FirstOrDefault();
            return data;
        }
        public IEnumerable<VendorMapping> GetVendorMapping(Int64 BuilderId)
        {
            return _ObjUnitWork.VendorMapping.Search(x => x.BuilderId == BuilderId && x.RowStatusId == (int) RowActiveStatus.Active);
        }
        public IEnumerable<dynamic> GetAllVendorMappingList(Int64 BuilderId)
        {
            var MappData = _ObjUnitWork.VendorMapping.GetAllVendorMappingList(BuilderId);
            return MappData; // _ObjUnitWork.Vendor.Search(x => x.RowStatusId == (int) RowActiveStatus.Active);
        }
        //public IEnumerable<BuilderVendorRemoved> GetBuilderRemovedVendors(Int64 BuilderId)
        //{
        //    return _ObjUnitWork.BuilderVendorRemoved.Search(x => x.BuilderId == BuilderId && x.RowStatusId == (int) RowActiveStatus.Active);
        //}
        public IEnumerable<dynamic> GetCBUSAVendorList(Int64 BuilderId)
        {

            var UnMappData = _ObjUnitWork.VendorMapping.GetCBUSAVendorListUnMapped(BuilderId);
            return UnMappData; // _ObjUnitWork.Vendor.Search(x => x.RowStatusId == (int) RowActiveStatus.Active);
        }
        
        public void SaveVendorMapping(Int64 BuilderId, List<VendorMapping> objMappedData, List<BuilderVendorRemoved> objRemovedData)
        {
            foreach (var data in objMappedData)
            {
                if (data.TranId > 0)
                {
                    //if (dbContext.VendorMapping.Any(e => e.BuilderVendorId == data.BuilderVendorId))
                    //{
                    //    dbContext.VendorMapping.Attach(data);
                    //    dbContext.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    //    data.ModifiedOn = System.DateTime.Now;
                    //    data.ModifiedBy = 1;
                    //}
                    dbContext.SaveChanges();
                }
                else
                {
                    //_ObjUnitWork.VendorMapping.Add(data);
                }
            }
           
            foreach (var data in objRemovedData)
            {
                if (data.TranId > 0)
                {
                    //if (dbContext.BuilderVendorRemoved.Any(e => e.BuilderVendorId == data.BuilderVendorId))
                    //{
                    //    dbContext.BuilderVendorRemoved.Attach(data);
                    //    dbContext.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    //    data.ModifiedOn = System.DateTime.Now;
                    //    data.ModifiedBy = 1;
                    //}
                    dbContext.SaveChanges();
                }
                else
                {
                    //_ObjUnitWork.BuilderVendorRemoved.Add(data);
                }
            }
            //}
            _ObjUnitWork.Complete();
        }
        public IEnumerable<dynamic> GetBuilderVendorRemovedList(Int64 BuilderId)
        {
            var RemovedData = _ObjUnitWork.VendorMapping.GetBuilderVendorRemovedList(BuilderId);
            return RemovedData;
        }
        public IEnumerable<dynamic> GetBuilderVendorList(Int64 BuilderId)
        {

            var UnMappData = _ObjUnitWork.VendorMapping.GetBuilderVendorListUnMapped(BuilderId);
            return UnMappData; 
        }




        public IEnumerable<CategoryMapping> GetCategoryMapping(Int64 BuilderId)
        {
            return _ObjUnitWork.CategoryMapping.Search(x => x.BuilderId == BuilderId && x.RowStatusId == (int) RowActiveStatus.Active);
        }
        public IEnumerable<dynamic> GetAllCategoryMappingList(Int64 BuilderId)
        {
            var MappData = _ObjUnitWork.CategoryMapping.GetAllCategoryMappingList(BuilderId);
            return MappData;
        }
        
        public IEnumerable<dynamic> GetCBUSACategoryList(Int64 BuilderId)
        {

            var UnMappData = _ObjUnitWork.CategoryMapping.GetCBUSACategoryListUnMapped(BuilderId);
            return UnMappData; 
        }
        
        public void SaveCategoryMapping(Int64 BuilderId, List<CategoryMapping> objMappedData, List<BuilderCategoryRemoved> objRemovedData)
        {
            foreach (var data in objMappedData)
            {
                if (data.TranId > 0)
                {
                    //if (dbContext.CategoryMapping.Any(e => e.BuilderCategoryId == data.BuilderCategoryId))
                    //{
                    //    dbContext.CategoryMapping.Attach(data);
                    //    dbContext.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    //    data.ModifiedOn = System.DateTime.Now;
                    //    data.ModifiedBy = 1;
                    //}
                    dbContext.SaveChanges();
                }
                else
                {
                    _ObjUnitWork.CategoryMapping.Add(data);
                }
            }
           
            foreach (var data in objRemovedData)
            {
                if (data.TranId > 0)
                {
                    //if (dbContext.BuilderCategoryRemoved.Any(e => e.BuilderCategoryId == data.BuilderCategoryId))
                    //{
                    //    dbContext.BuilderCategoryRemoved.Attach(data);
                    //    dbContext.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    //    data.ModifiedOn = System.DateTime.Now;
                    //    data.ModifiedBy = 1;
                    //}
                    dbContext.SaveChanges();
                }
                else
                {
                    _ObjUnitWork.BuilderCategoryRemoved.Add(data);
                }
            }

            _ObjUnitWork.Complete();
        }
        public IEnumerable<dynamic> GetBuilderCategoryRemovedList(Int64 BuilderId)
        {
            var RemovedData = _ObjUnitWork.CategoryMapping.GetBuilderCategoryRemovedList(BuilderId);
            return RemovedData;
        }
        public IEnumerable<dynamic> GetBuilderCategoryList(Int64 BuilderId)
        {

            var UnMappData = _ObjUnitWork.CategoryMapping.GetBuilderCategoryListUnMapped(BuilderId);
            return UnMappData; // _ObjUnitWork.Category.Search(x => x.RowStatusId == (int) RowActiveStatus.Active);
        }
        public IEnumerable<dynamic> GetReportingQuarterYar(string FromDate, string ToDate)
        {
            DateTime dtFrom = Convert.ToDateTime(FromDate);
            DateTime dtTo = Convert.ToDateTime(ToDate);
            return _ObjUnitWork.Quater.Search(a => dtFrom >= a.StartDate && dtFrom <= a.EndDate);
        }
        public IEnumerable<dynamic> GetMappedSubmitReportData(Int64 BuilderId, string FromDate, string ToDate)
        {
            var data = _ObjUnitWork.SubmitReport.GetMappedSumitReportData(BuilderId, FromDate, ToDate);
            return data;
        }
        public IEnumerable<dynamic> GetUnMappedSubmitReportData(Int64 BuilderId, string FromDate, string ToDate)
        {
            var data = _ObjUnitWork.SubmitReport.GetUnMappedSumitReportData(BuilderId, FromDate, ToDate);
            return data;
        }
        public void SaveSubmitReport(Int64 BuilderId, List<SubmitReport> objData)
        {
            foreach (var data in objData)
            {
                //if (data.SubmitReportTranId > 0)
                //{
                //    dbContext.SubmitReport.Attach(data);
                //    dbContext.Entry(data).State = System.Data.Entity.EntityState.Modified;
                //    data.ModifiedOn = System.DateTime.Now;
                //    data.ModifiedBy = 1;
                //    dbContext.SaveChanges();
                //}
                //else
                //{
                //    _ObjUnitWork.SubmitReport.Add(data);
                //}
            }            
            _ObjUnitWork.Complete();
        }
    }
}
