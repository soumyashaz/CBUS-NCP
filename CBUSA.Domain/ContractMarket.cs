using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContractMarket
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ContractMarketId { get; set; }

        public Int64 ContractId { get; set; }
        public Int64 MarketId { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Market Market { get; set; }

    }
}
