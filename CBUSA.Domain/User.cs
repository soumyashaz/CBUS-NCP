using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class User
    {
      public int Id { get; set; }
      public string FirstName { get; set; }
      public string MiddleInit { get; set; }
      public string LastName { get; set; }
      public string Gender { get; set; }
      public DateTime DOB { get; set; }
      public string ContactUsOnBehalf { get; set; }
      public string CompanyTypeName { get; set; }
      public string AddressLine1 { get; set; }
      public string AddressLine2 { get; set; }
      public string Zip { get; set; }
      public string City { get; set; }
      public string State { get; set; }
      public string Phone { get; set; }
      public string NoteBox { get; set; }
      public DateTime CreatedOn { get; set; }
      public int CreatedBy { get; set; }
      public DateTime ModifiedOn { get; set; }
      public int ModifiedBy { get; set; }
      public int RowStatusId { get; set; }
      public Guid RowGUID { get; set; }
      public string Email { get; set; }
      public bool EmailConfirmed { get; set; }
      public string PasswordHash { get; set; }
      public string SecurityStamp { get; set; }
      public string PhoneNumber { get; set; }
      public bool PhoneNumberConfirmed { get; set; }
      public bool TwoFactorEnabled { get; set; }

      public DateTime? LockoutEndDateUtc { get; set; }
      public bool LockoutEnabled { get; set; }
      public int AccessFailedCount { get; set; }
      public string UserName { get; set; }
      public Int64? CourtId { get; set; }

      public bool IsEulaChecked { get; set; }
      public string Fax { get; set; }
      public string SSN { get; set; }
      public string DrivingLic { get; set; }
      public string Other { get; set; }
      public bool IsSecondTimeLogin { get; set; }
     
        public ICollection<UserProfile> UserProfile { get; set; }

        //public ICollection<GeneralTermsOfProbationSetup> GeneralTermsOfProbationSetups { get; set; }

    }
}
