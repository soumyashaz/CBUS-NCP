using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class PersonalInfoViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }
       // [Required(ErrorMessage = "*")]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "*")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "*")]
        public string AddressLine1 { get; set; }
        //[Required(ErrorMessage = "*")]
        public string AddressLine2 { get; set; }
        [Required(ErrorMessage = "*")]
        public string zip { get; set; }
        [Required(ErrorMessage = "*")]
        public string City { get; set; }
        [Required(ErrorMessage = "*")]
        public string State { get; set; }

        [EmailAddress]
      //  [Required(ErrorMessage = "*")]
        public string Email { get; set; }
       // [Required(ErrorMessage = "*")]
        public string WorkPhone { get; set; }
        [Required(ErrorMessage = "*")]
        public string CellPhone { get; set; }

        public string RoleName { get; set; }

        public int RoleId { get; set; }

        public string CompanyName { get; set; }

        //public bool IsOffender { get; set; }

    }
}