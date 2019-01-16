using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Domain
{

    public class Product :BaseColumnField
    {
        [Key]
        public Int64 ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        public Int64 ProductCategoryId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
      
    }


    public class ProductCategory : BaseColumnField
    {
        [Key]
        public Int64 ProductCategoryId { get; set; }

        [Required]
        public string ProductCategoryName { get; set; }

        public Int64 ParentId { get; set; }

        public virtual ICollection<Product> Product { get; set; } 

    }
}
