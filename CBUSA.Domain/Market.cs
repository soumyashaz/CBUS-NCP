using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{

    public class Market : BaseColumnField
    {

        public Int64 MarketId { get; set; }
        [Required]
        [MaxLength(250)]
        public string MarketName { get; set; }

        public Int32 ZoneId { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<Builder> Bulder { get; set; }

    }
}
