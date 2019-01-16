using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface ISubmitReportRepository : IRepository<SubmitReport>
    {
        IEnumerable<dynamic> GetMappedSumitReportData(Int64 BuilderId, string FromDate, string ToDate);
        IEnumerable<dynamic> GetUnMappedSumitReportData(Int64 BuilderId, string FromDate, string ToDate);
    }
}
