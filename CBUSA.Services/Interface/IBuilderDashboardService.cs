using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Interface
{
    interface IBuilderDashboardService
    {
        int GetCountOfContractsCanJoin(Int64 QuaterId);
        int GetCountOfInProgressReportForQuarter(Int64 QuaterId);
        int GetCountOfCompletedReportForQuarter(Int64 QuaterId);
        int GetCountOfReportsSubmittedForQuarterForDay(Int64 QuaterId, DateTime ReportDate);
        IEnumerable<dynamic> GetBuilderDetailsOfNotStartedBuilders(Int64 QuaterId);
        IEnumerable<dynamic> GetBuilderDetailsOfInProgressBuilders(Int64 QuaterId);
        IEnumerable<dynamic> GetBuilderDetailsOfReportSubmittedBuilders(Int64 QuaterId);
        IEnumerable<dynamic> GetBuilderDetailsOfAllParticipatingBuilders(Int64 QuaterId);
        IEnumerable<dynamic> GetDetailsOfAllProjectByBuilderContractsForCurrentQuarter(Int64 QuaterId, Int64 BuilderId, Int64 ContractId);
        IEnumerable<dynamic> GetDetailsOfAllBuilderContractsForCurrentQuarter(Int64 QuaterId);
        IEnumerable<dynamic> GetDetailsOfAllProjectByBuilderContractsForCurrentQuarter(Int64 QuaterId);
    }
}
