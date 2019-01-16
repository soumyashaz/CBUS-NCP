using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class AccountSecurityViewModel
    {

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [RegularExpression(@"(?=.*[0-9])(?=.*[A-Z])(?=.*[a-zA-Z0-9])(?=.*[~!@#$%^&*])[a-zA-Z0-9~!@#$%^&*]{8,15}", ErrorMessage = "<div style='text-align: left;'><p>Password should have at least 1 number and 1 letter.</p><p>Password should be 8 characters long.</p><p>Password should have at least 1 upper case and 1 lower case letter.</p><p>Password should have at least one special character, e.g. !, #, @, % etc.</p><p>Passwords used before will not be allowed.</p></div>")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "*")]
        [Compare("NewPassword")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "*")]
        public int SelectedQuestionId1 { get; set; }
        [Required(ErrorMessage = "*")]
        public string Answare1 { get; set; }
        [Required(ErrorMessage = "*")]
        public int SelectedQuestionId2 { get; set; }
        [Required(ErrorMessage = "*")]
        public string Answare2 { get; set; }
        [Required(ErrorMessage = "*")]
        public int SelectedQuestionId3 { get; set; }
        [Required(ErrorMessage = "*")]
        public string Answare3 { get; set; }
    }
}

