using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IBuilderIntegrationService
    {
        string GetBuilderFirstNameLastName(Int64 BuilderId);
        IEnumerable<VendorMapping> GetVendorMapping(Int64 BuilderId);
        IEnumerable<dynamic> GetAllVendorMappingList(Int64 BuilderId);
        IEnumerable<dynamic> GetCBUSAVendorList(Int64 BuilderId);
        //IEnumerable<QBVendorDataReceived> GetBuilderVendorList(Int64 BuilderId);
        IEnumerable<dynamic> GetBuilderVendorList(Int64 BuilderId);
        IEnumerable<dynamic> GetBuilderVendorRemovedList(Int64 BuilderId);
        //IEnumerable<BuilderVendorRemoved> GetBuilderVendorRemoved(Int64 BuilderId);
        void SaveVendorMapping(Int64 BuilderId, List<VendorMapping> objMappingData, List<BuilderVendorRemoved> objRemovedData);
        //VendorMapping GetVendorMappingList(Int64 BuilderId);

        IEnumerable<CategoryMapping> GetCategoryMapping(Int64 BuilderId);
        IEnumerable<dynamic> GetAllCategoryMappingList(Int64 BuilderId);
        IEnumerable<dynamic> GetCBUSACategoryList(Int64 BuilderId);
        //IEnumerable<QBVendorDataReceived> GetBuilderVendorList(Int64 BuilderId);
        IEnumerable<dynamic> GetBuilderCategoryList(Int64 BuilderId);
        IEnumerable<dynamic> GetBuilderCategoryRemovedList(Int64 BuilderId);
        //IEnumerable<BuilderVendorRemoved> GetBuilderVendorRemoved(Int64 BuilderId);
        void SaveCategoryMapping(Int64 BuilderId, List<CategoryMapping> objMappingData, List<BuilderCategoryRemoved> objRemovedData);
        IEnumerable<dynamic> GetReportingQuarterYar(string FromDate, string ToDate);
        IEnumerable<dynamic> GetMappedSubmitReportData(Int64 BuilderId, string FromDate, string ToDate);
        IEnumerable<dynamic> GetUnMappedSubmitReportData(Int64 BuilderId, string FromDate, string ToDate);
        void SaveSubmitReport(Int64 BuilderId, List<SubmitReport> objData);
    }
}
