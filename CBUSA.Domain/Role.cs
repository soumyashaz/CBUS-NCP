using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Role
    {
        public int Id { get; set; }

        
        public string Description { get; set; }
        [Required,StringLength(256)]
        public string Name { get; set; }
        [Required]
        public int SystemRole { get; set; }
      
    }
}
