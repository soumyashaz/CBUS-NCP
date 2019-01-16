using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Interface
{
    public interface INonResponderReportService
    {
        IEnumerable<dynamic> GetNonResponderBuilderList(Int64 QuarterId);
    }
}
