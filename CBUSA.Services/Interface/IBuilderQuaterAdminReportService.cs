using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IBuilderQuaterAdminReportService
    {
        void SaveBuilderQaterReport(BuilderQuaterAdminReport ObjReport, bool disposeDbContext = true);
        IEnumerable<BuilderQuaterAdminReport> CheckBuilderQuaterReport(Int64 BuilderId, Int64 QuaterId);
        IEnumerable<BuilderQuaterAdminReport> GetId(Int64 AdminReportId);
        void UpdateBuilderAdminQuaterReport(BuilderQuaterAdminReport ObjReport);
        IEnumerable<BuilderQuaterAdminReport> GetBuilderQuaterReport(Int64 BuilderId, Int64 QuaterId);
        IEnumerable<BuilderQuaterAdminReport> GetBuilderReport(Int64 BuilderId);
        bool IsReportAllreadySubmited(Int64 BuilderId, Int64 QuaterId);
        bool IsReportInitiated(Int64 BuilderId, Int64 QuaterId);        
    }
}
