using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class CustomIdentityModel
    {


        public class ApplicationUserRole : IdentityUserRole<int>
        {
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public Int64 UserRoleId { get; set; }

        }
        public class ApplicationUserClaim : IdentityUserClaim<int> { }
        public class ApplicationUserLogin : IdentityUserLogin<int> { }

        public class ApplicationRole : IdentityRole<int, ApplicationUserRole>, IRole<int>
        {
            public string Description { get; set; }
            //This firld is mapped with the enum named as "GetRoleName"
            public int SystemRole { get; set; }

            public ApplicationRole() { }
            public ApplicationRole(string name)
                : this()
            {
                this.Name = name;
            }

            public ApplicationRole(string name, string description)
                : this(name)
            {
                this.Description = description;
            }
        }

        public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<int>
        {
            public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
            {
                // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                // Add custom user claims here
                return userIdentity;
            }

            public string FirstName { get; set; }
            public string MiddleInit { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            [Column(TypeName = "datetime2")]
            public DateTime DOB { get; set; }
            public string ContactUsOnBehalf { get; set; }
            public string CompanyTypeName { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string Zip { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Phone { get; set; }
            //public string Fax { get; set; }
            public string NoteBox { get; set; }
            public bool IsEulaChecked { get; set; }
            public bool IsSecondTimeLogin { get; set; }

            [Column(TypeName = "datetime2")]
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            [Column(TypeName = "datetime2")]
            public DateTime ModifiedOn { get; set; }
            public int ModifiedBy { get; set; }
            [ForeignKey("RowStatus")]
            public int RowStatusId { get; set; }
            //[ForeignKey("Court")]
            //public Int64? CourtId { get; set; }
            public Guid RowGUID { get; set; }
            public string Fax { get; set; }
            public string SSN { get; set; }
            public string DrivingLic { get; set; }
            public string Other { get; set; }
          
            public virtual RowStatus RowStatus { get; set; }
            //public virtual Court Court { get; set; }
        }


        public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
        {
            public ApplicationDbContext()
                : base("CBUSA")
            {
            }

            //static ApplicationDbContext()
            //{
            //    Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
            //}
            //protected override void OnModelCreating(DbModelBuilder modelBuilder)
            //{

            //}

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }


        }

        public class ApplicationUserStore :
          UserStore<ApplicationUser, ApplicationRole, int,
          ApplicationUserLogin, ApplicationUserRole,
          ApplicationUserClaim>, IUserStore<ApplicationUser, int>,
          IDisposable
        {
            public ApplicationUserStore()
                : this(new IdentityDbContext())
            {
                base.DisposeContext = true;
            }

            public ApplicationUserStore(DbContext context)
                : base(context)
            {
            }
        }


        public class ApplicationRoleStore
            : RoleStore<ApplicationRole, int, ApplicationUserRole>,
            IQueryableRoleStore<ApplicationRole, int>,
            IRoleStore<ApplicationRole, int>, IDisposable
        {
            public ApplicationRoleStore()
                : base(new IdentityDbContext())
            {
                base.DisposeContext = true;
            }

            public ApplicationRoleStore(DbContext context)
                : base(context)
            {
            }
        }
        
    }
    
    public class CustomPassword : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return Encrypt.EncryptString(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword != null)
            {
                if (Encrypt.DecryptString(hashedPassword).Equals(providedPassword))
                    return PasswordVerificationResult.Success;
                else return PasswordVerificationResult.Failed;
            }
            else return PasswordVerificationResult.Failed;
        }
    }
}
