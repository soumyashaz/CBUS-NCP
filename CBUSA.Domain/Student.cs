using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CBUSA.Domain
{
  public  class Student
    {
      
      [Key]
      public Int64 StudentId { get; set; }
      [Required]
      public string StudentName { get; set; }
    
    }
}
