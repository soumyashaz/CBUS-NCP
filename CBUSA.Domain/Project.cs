using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Project :BaseColumnField
    {
        public Int64 ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string LotNo { get; set; }
        public string Address { get; set; }
        [MaxLength(50)]
        public string Zip { get; set; }
        [MaxLength(250)]
        public string State { get; set; }
        [MaxLength(250)]
        public string City { get; set; }
        public Int64 BuilderId { get; set; }
        public Builder Builder { get; set; }
      

    }
}
