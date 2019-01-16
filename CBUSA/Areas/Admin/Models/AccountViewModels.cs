using CBUSA.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Areas.Admin.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "New password and Confirm password does not match")]
        public string ConfirmPassword { get; set; }
        public string UserID { get; set; }
    }

    public class LoginViewModel : Login
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }

    public class RegisterViewModel : Register
    {
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
    }


    public class ContactUsRegisterViewModel
    {
        [Required]
        public string FirstName { get; set; }

        public string MiddleInit { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]

        public string ContactUsOnBehalf { get; set; }

        [Required]

        public string CompanyTypeName { get; set; }

        [Required]

        public string AddressLine1 { get; set; }


        public string AddressLine2 { get; set; }

        [Required]

        public string Zip { get; set; }

        [Required]

        public string City { get; set; }

        [Required]

        public string State { get; set; }

        [Required]
        [EmailAddress]

        public string Email { get; set; }


        [Required]

        public string Phone { get; set; }
        public string NoteBox { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [RegularExpression(@"(?=.*[0-9])(?=.*[A-Z])(?=.*[a-zA-Z0-9])(?=.*[~!@#$%^&*])[a-zA-Z0-9~!@#$%^&*]{8,15}", ErrorMessage = "<div style='text-align: left;'><p>Password should have at least 1 number and 1 letter.</p><p>Password should be 8 characters long.</p><p>Password should have at least 1 upper case and 1 lower case letter.</p><p>Password should have at least one special character, e.g. !, #, @, % etc.</p><p>Passwords used before will not be allowed.</p></div>")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "New password and Confirm password does not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Code { get; set; }
        public string UserId { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]

        public string UserName { get; set; }
    }

    public class RecoverUserNameViewModel
    {
        [Required]
        [EmailAddress]
        public string EmailId { get; set; }

        public int SelectedQuestionId1 { get; set; }
        public string Answare1 { get; set; }

        public int SelectedQuestionId2 { get; set; }
        public string Answare2 { get; set; }

        public int SelectedQuestionId3 { get; set; }
        public string Answare3 { get; set; }

    }


    public class EulaConfirmationViewModel
    {

        public int SelectedQuestionId1 { get; set; }
        [Required]
        public string Answare1 { get; set; }

        public int SelectedQuestionId2 { get; set; }
        [Required]
        public string Answare2 { get; set; }

        public int SelectedQuestionId3 { get; set; }
        [Required]
        public string Answare3 { get; set; }

        [Required]
        public bool IsEulaChecked { get; set; }

        public bool IsConsentAgreementChecked { get; set; }

    }

    public class ConcentAgreementViewModel
    {
        public string DefendantName { get; set; }
        public string DefendantDOB { get; set; }
        public string DefendantCourtName { get; set; }
        public string ConsentAgreementDateString { get; set; }
    }
}
