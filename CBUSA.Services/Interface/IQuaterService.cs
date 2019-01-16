using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IQuaterService
    {
        IEnumerable<Quater> GetQuater();
        IEnumerable<Quater> GetQuaterByDate(DateTime currentdate);
        IEnumerable<Quater> GetQuatersUptoCurrentQuater(Int64 QuaterId,Int64 Year);
        Quater GetQuaterById(Int64 QuaterId);
        IEnumerable<Quater> GetAllPreviousQuater(Int64 QuaterId);
        Quater GetQuaterWithYearQuaterName(Int64 Year, string QuaterName);
        IEnumerable<Quater> GetNCPReportingQuarterByCurrentQuarter(Int64 CurrentQuarterId);
        bool IsReportingPeriodOn(DateTime currentdate);
        void UpdateQuarterReportingWindow(Int64 QuarterID, DateTime StartDate, DateTime EndDate);
        Quater GetLastReportingQuarter();
    }
}
