using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
   public abstract class Register
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "MI")]
        public string MiddleInit { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Gender *")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Date Of Birth *")]
        [Column(TypeName = "datetime2")] 
        public DateTime DOB { get; set; }

        [Required]
        [Display(Name = "Are you contacting us *")]
        public string ContactUsOnBehalf { get; set; }

        [Required]
        [Display(Name = "<Company Type> Name *")]
        public string CompanyTypeName { get; set; }

        [Required]
        [Display(Name = "Address Line 1 *")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Phone *")]
        public string Phone { get; set; }
        public string NoteBox { get; set; }
       [Column(TypeName = "datetime2")] 
       public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
       [Column(TypeName = "datetime2")] 
       public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }        
        public int RowStatusId { get; set; }
        public Guid RowGUID { get; set; }
        
    }
}
