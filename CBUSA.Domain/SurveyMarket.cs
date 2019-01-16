using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class SurveyMarket
    {
        public Int64 SurveyMarketId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 MarketId { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual Market Market { get; set; }
       
    }
}
