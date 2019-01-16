using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using CBUSA.Models;
using System.Net.Mail;
using System.Security.Principal;

namespace CBUSA
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<CBUSA.Domain.CustomIdentityModel.ApplicationUser, int> //UserManager<ApplicationUser>
    {
        //public ApplicationUserManager(IUserStore<ApplicationUser> store)
        //    : base(store)
        //{
        //}

        public ApplicationUserManager(IUserStore<CBUSA.Domain.CustomIdentityModel.ApplicationUser, int> store)
        : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            var manager = new ApplicationUserManager(new CBUSA.Domain.CustomIdentityModel.ApplicationUserStore(context.Get<CBUSA.Domain.CustomIdentityModel.ApplicationDbContext>()));
            // Configure validation logic for usernames
            //manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            manager.UserValidator = new UserValidator<CBUSA.Domain.CustomIdentityModel.ApplicationUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            //manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            manager.RegisterTwoFactorProvider("PhoneCode",new PhoneNumberTokenProvider<CBUSA.Domain.CustomIdentityModel.ApplicationUser, int>
               {
                MessageFormat = "Your security code is: {0}"
            });
            //manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            manager.RegisterTwoFactorProvider("EmailCode",new EmailTokenProvider<CBUSA.Domain.CustomIdentityModel.ApplicationUser, int>
                {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                //manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
                manager.UserTokenProvider =
                   new DataProtectorTokenProvider<CBUSA.Domain.CustomIdentityModel.ApplicationUser, int>(
                       dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public string[] PasswordPolicyMessage()
        {
            string[] PasswordPolicyErrorMessage = { "Password should have at least 1 number and 1 letter.",
                                                      "Password should be 8 characters long.",
                                                      "Password should have at least 1 upper case and 1 lower case letter.",
                                                      "Password should have at least one special character, e.g. !, #, @, % etc.",
                                                  "Passwords used before will not be allowed."};


            return PasswordPolicyErrorMessage;
        }
    }

    public class ApplicationRoleManager : RoleManager<CBUSA.Domain.CustomIdentityModel.ApplicationRole, int>
    {
        // PASS CUSTOM APPLICATION ROLE AND INT AS TYPE ARGUMENTS TO CONSTRUCTOR:
        public ApplicationRoleManager(IRoleStore<CBUSA.Domain.CustomIdentityModel.ApplicationRole, int> roleStore)
            : base(roleStore)
        {
        }

        // PASS CUSTOM APPLICATION ROLE AS TYPE ARGUMENT:
        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(
                new CBUSA.Domain.CustomIdentityModel.ApplicationRoleStore(context.Get<CBUSA.Domain.CustomIdentityModel.ApplicationDbContext>()));
        }
    }

    public class EmailService : IIdentityMessageService
    {
        //public Task SendAsync(IdentityMessage message)
        //{
        //    // Plug in your email service here to send an email.
        //    return Task.FromResult(0);
        //}
        public async Task SendAsync(IdentityMessage message)
        {
            /// Plug in your email service here to send an email.

            await configSMTPasync(message);

            //return Task.FromResult(0);
        }

        private async Task configSMTPasync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            var credentialUserName = "kneyaz@medullus.com";
            var sentFrom = "noreply@medullus.com";
            var pwd = "p@ssw0rd";
            var sentTo = "kneyaz@medullus.com";

            // Configure the client:
            SmtpClient client = new SmtpClient();

            client.Port = 25;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Creatte the credentials:
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(credentialUserName, pwd);
            client.EnableSsl = false;
            client.Credentials = credentials;

            // Create the message:
            //var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination);
            var mail = new System.Net.Mail.MailMessage(sentFrom, sentTo);
            mail.Subject = message.Subject;
            mail.Body = message.Body;

            await client.SendMailAsync(mail);
        }

    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class ApplicationSignInManager : SignInManager<CBUSA.Domain.CustomIdentityModel.ApplicationUser, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(CBUSA.Domain.CustomIdentityModel.ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

}
