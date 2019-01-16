using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Interface
{
    public interface INonResponderReportRepository
    {
        IEnumerable<dynamic> GetNonResponderBuilderList(Int64 QuarterId);
    }
}
