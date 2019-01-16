using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{

    public class Builder : BaseColumnField
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 BuilderId { get; set; }

        [Required]
        [MaxLength(100)]
        public string BuilderName { get; set; } // OR Company Name

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(15)]
        public string PhoneNo { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public Int64 MarketId { get; set; }
        public Int64 HistoricId { get; set; }
        public virtual Market Market { get; set; }

    }
}
