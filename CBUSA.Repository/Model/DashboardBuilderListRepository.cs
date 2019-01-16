using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Model
{
    public class DashboardBuilderListRepository
    {
        public Int64 BuilderId { get; set; }
        public string BuilderName { get; set; }
        public Int64 MarketId { get; set; }
        public Int64 QuaterId { get; set; }
        public string MarketName { get; set; }
        public string ContractStatus { get; set; }
        public string LastActivityDate { get; set; }
    }
}
