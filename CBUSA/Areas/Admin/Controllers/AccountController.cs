using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using CBUSA.Models;
using CBUSA.Areas.Admin.Models;
using CBUSA.Services;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using System.Web.Security;
using System.IO;
using System.Text;
using System.Configuration;

namespace CBUSA.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;

        readonly IChallengeQuestionServices _ObjChallangeQuestion;
        private IEulaServices _ObjEulaServices;
        // Neyaz private IFaqServices _ObjFaqService;
        private IStateService _ObjStateService;
        // Neyaz private IUserProfileServices _ObjUserProfileService;
        // Neyaz private ICourtWorksSettingServices _ObjCourtWorksSettingsService;
        private IPasswordLogServices _ObjPasswordLogServices;
        readonly IUserInRoleServices _ObjUserInRoleServices;
        // Neyaz readonly IOffenderServices _ObjOffenderServices;
        readonly IRoleServices _ObjRoleServices;

        public AccountController(IChallengeQuestionServices ObjChallangeQuestion, IEulaServices ObjEulaServices,
            // Neyaz IFaqServices ObjFaqService, 
            IStateService ObjStateService,
            // Neyaz IUserProfileServices ObjUserProfileService, ICourtWorksSettingServices ObjCourtWorksSettingsService, 
            IPasswordLogServices ObjPasswordLogServices,IUserInRoleServices ObjUserInRoleServices,
            // Neyaz IOffenderServices ObjOffenderServices, 
            IRoleServices ObjRoleServices
            )
        {
            _ObjChallangeQuestion = ObjChallangeQuestion;
            _ObjEulaServices = ObjEulaServices;
            // Neyaz _ObjFaqService = ObjFaqService;
            _ObjStateService = ObjStateService;
            // Neyaz _ObjUserProfileService = ObjUserProfileService;
            // Neyaz _ObjCourtWorksSettingsService = ObjCourtWorksSettingsService;
            _ObjPasswordLogServices = ObjPasswordLogServices;
            _ObjUserInRoleServices = ObjUserInRoleServices;
            // Neyaz _ObjOffenderServices = ObjOffenderServices;
            _ObjRoleServices = ObjRoleServices;
        }

        //public AccountController(ApplicationUserManager userManager)
        //{
        //    UserManager = userManager;
        //}

        //readonly IPasswordLogServices _ObjStorePasswordServices;
        //public AccountController(IPasswordLogServices ObjStorePasswordServices)
        //{
        //    _ObjStorePasswordServices = ObjStorePasswordServices;
        //}

        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager != null)
                {
                    _userManager.PasswordHasher = new CustomPassword();
                    return _userManager;
                }
                else
                {
                    var Manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    Manager.PasswordHasher = new CustomPassword();
                    return Manager;
                }
                //return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                // _userManager.PasswordHasher
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (User.Identity.IsAuthenticated && User.Identity.Name == "superadmin")
            {
                //return RedirectToLocal(returnUrl);
                if (returnUrl == "/" || returnUrl == null || returnUrl == "null")
                {
                    return RedirectToAction("Index", "Home", new { Area = "" });
                    //return RedirectToLocal("~/Admin/Account/Login");
                }
                else
                {
                    return RedirectToLocal(returnUrl);
                }
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var CheckUserActive = UserManager.FindByName(model.UserName);
                if (CheckUserActive != null)
                {
                    if (CheckUserActive.RowStatusId == (Int32)RowActiveStatus.Active)
                    {
                        var user = await UserManager.FindAsync(model.UserName, model.Password);
                        if (user != null)
                        {
                            await SignInAsync(user, model.RememberMe);
                            if (returnUrl != null && returnUrl != "/")
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                //return Redirect(Url.Content("~/"));
                                return RedirectToAction("login", "Account", new { Area = "Admin" });
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid username or password.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Not Active. Contact CBUSA Admin.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username.");
                }
            }
            //else
            //{
            //    string[] ModelErrorChild = ModelState.Values.Where(E => E.Errors.Count > 0)
            //   .SelectMany(E => E.Errors)
            //   .Select(E => E.ErrorMessage)
            //   .ToArray();

            //    StringBuilder Sb = new StringBuilder();
            //    foreach (string Error in ModelErrorChild)
            //    {
            //        Sb.Append(Error);
            //    }                

            //    ModelState.AddModelError("", Sb.ToString());
            //}

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LoggedOut()
        {
            //  await UserManager.UpdateSecurityStampAsync(Convert.ToInt32(User.Identity.GetUserId));
            AuthenticationManager.SignOut();

            return View();
        }


        public ActionResult Logoff()
        {
            //  await UserManager.UpdateSecurityStampAsync(Convert.ToInt32(User.Identity.GetUserId));
            AuthenticationManager.SignOut();
            return RedirectToAction("LoggedOut", "Account");
            // return View();
        }

         [AllowAnonymous]
        public ActionResult Sessiontimeout()
        {
            return View();
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //ViewBag.State = _ObjStateService.GetState().Where(x => x.RowStatusId == (int)RowActiveStatus.Active);
            ViewBag.State = _ObjStateService.GetState().Where(x => x.IsActive == (int)RowActiveStatus.Active);
            return View();
        }

        //
        // POST: /Account/Register


        // Neyaz

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[RecapchaFilter]
        //public ActionResult Register(ContactUsRegisterViewModel model, bool CaptchaValid)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        string EmailSubject = "Contact CourtWorks";
        //        StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/MailTemplate/RegisterWithCourtWorks.html"));
        //        StringBuilder Sb = new StringBuilder(reader.ReadToEnd());
        //        string Name = model.FirstName + (model.MiddleInit != "" ? " " + model.MiddleInit + " " : " ") + model.LastName;
        //        string ContactUsOnBehalfof = "";

        //        switch (model.ContactUsOnBehalf)
        //        {
        //            case "1":
        //                ContactUsOnBehalfof = "On behalf of a court?";
        //                break;
        //            case "2":
        //                ContactUsOnBehalfof = "On behalf of an Agency of Court Vendor?";
        //                break;
        //            case "3":
        //                ContactUsOnBehalfof = "On behalf of any other?";
        //                break;
        //        }


        //        Sb.Replace("####Name####", Name);
        //        Sb.Replace("####CompanyName####", model.CompanyTypeName);
        //        Sb.Replace("####AddressLine1####", model.AddressLine1);
        //        Sb.Replace("####AddressLine2####", model.AddressLine2);
        //        Sb.Replace("####Zip####", model.Zip);
        //        Sb.Replace("####City####", model.City);
        //        Sb.Replace("####State####", model.State);
        //        Sb.Replace("####Email####", model.Email);
        //        Sb.Replace("####Phone####", model.Phone);
        //        Sb.Replace("####Message####", model.NoteBox);
        //        Sb.Replace("####ContactinUs####", ContactUsOnBehalfof);

        //        string StrBody = Sb.ToString();
        //        IEmailSend Send = new EmailSendHtml();

        //        string CourtWorksContactUsEmail = _ObjCourtWorksSettingsService.GetCourtWorksSetting().FirstOrDefault().ContactUsEmail;
        //        bool IsEmailSendSuccessfull = Send.Send(EmailSubject, StrBody, CourtWorksContactUsEmail);

        //        if (IsEmailSendSuccessfull)
        //        {
        //            return RedirectToAction("RegisterSuccess", "Account");
        //        }

        //    }


        //    // If we got this far, something failed, redisplay form
        //    ViewBag.State = _ObjStateService.GetState().Where(x => x.RowStatusId == (int)RowActiveStatus.Active);
        //    return View(model);
        //}

        public ActionResult RegisterSuccess()
        {
            // ViewBag.State = _ObjStateService.GetState().Where(x => x.RowStatusId == (int)RowActiveStatus.Active);
            return View();
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == default(int) || code == null)
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RecapchaFilter]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model, bool CaptchaValid)
        {
            if (!CaptchaValid)
            {
                //Captcha failed to validate
                ModelState.AddModelError("reCaptcha", "Invalid reCaptcha");
            }

            if (ModelState.IsValid)
            {
                // UserManager.PasswordHasher = new CustomPassword();
                // var user = await UserManager.FindByNameAsync(model.Email);
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = Encryption.EncodeTo64(user.Id.ToString()), code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");


                string EmailSubject = "CBUSA Forgot Password";
                StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/MailTemplate/ForgotPassword.html"));
                StringBuilder Sb = new StringBuilder(reader.ReadToEnd());

                Sb.Replace("####Link####", callbackUrl);

                string StrBody = Sb.ToString();
                IEmailSend Send = new EmailSendHtml();
                bool IsEmailSendSuccessfull = Send.Send(EmailSubject, StrBody, user.Email);

                if (IsEmailSendSuccessfull)
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string code, string userId)
        {
            if (code == null && userId == null)
            {
                return View("Error");
            }

            int UserId = Convert.ToInt32(Encryption.DecodeFrom64(userId));
            var RolesList = await UserManager.GetRolesAsync(UserId);
            // List<string> bb = RolesList.Result.ToList();

            if (RolesList.Count > 0)
            {
                ViewBag.UserRole = RolesList[0];
            }

            var user = await UserManager.FindByIdAsync(UserId);
            ViewBag.FirstName = user.FirstName;
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                int UserId = Convert.ToInt32(Encryption.DecodeFrom64(model.UserId));
                var user = await UserManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }

                if (_ObjPasswordLogServices.IsPreviousPassword(UserId, model.Password))
                {
                    ModelState.AddModelError("", "Can't use previous password");
                    return View(model);
                }


                IdentityResult result = await UserManager.ResetPasswordAsync(UserId, model.Code, model.Password);

                if (result.Succeeded)
                {
                    PasswordLog PwdLog = new PasswordLog();
                    PwdLog.UserId = UserId;
                    PwdLog.CreateDate = DateTime.Now;
                    PwdLog.PasswordHash = Encrypt.EncryptString(model.Password);
                    _ObjPasswordLogServices.SavePasswordLog(PwdLog);
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(int.Parse(User.Identity.GetUserId()), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(int.Parse(User.Identity.GetUserId()));
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message, string UserID)
        {
            int User_ID = 0;
            string UserId = Encryption.DecodeFrom64(UserID).ToString();
            UserID = UserID.ToString();
            if (int.TryParse(UserId, out User_ID))
            {
                // It was assigned.
            }
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword(User_ID);
            ViewBag.ReturnUrl = Url.Action("Manage");
            
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            //int RequestedUserID = Convert.ToInt32(model.UserID);
            int RequestedUserID = Convert.ToInt32(Encryption.DecodeFrom64(model.UserID));  
            bool hasPassword = HasPassword(RequestedUserID);
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            /*model.UserID = RequestedUserID;*/
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {

                    IdentityResult result = await UserManager.ChangePasswordAsync(RequestedUserID, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(RequestedUserID);
                        //await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess, UserID= model.UserID });
                    }
                    else
                    {
                        AddErrors(result);
                    }

                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(RequestedUserID, model.NewPassword);
                    if (result.Succeeded)
                    {
                        ////Save Password to another table for check if password exist - Hipaa Law

                        //var GetUserPasswordHash = UserManager.FindById(RequestedUserID).PasswordHash;

                        //PasswordLog ObjStorePassword = new PasswordLog();
                        //ObjStorePassword.UserId = RequestedUserID;
                        //ObjStorePassword.PasswordHash = Convert.ToString(GetUserPasswordHash);
                        //_ObjStorePasswordServices.SavePasswordLog(ObjStorePassword);

                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(int.Parse(User.Identity.GetUserId()), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new CBUSA.Domain.CustomIdentityModel.ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account", new { Area = "Admin" });
            //return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account", new { Area = "Admin" });
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //This action is not in use now. Previously RemoveAccountList was acception 0 arguments
        [ChildActionOnly]
        public ActionResult RemoveAccountList(int UserID)
        {
            var linkedAccounts = UserManager.GetLogins(int.Parse(User.Identity.GetUserId()));
            ViewBag.ShowRemoveButton = HasPassword(UserID) || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        [AllowAnonymous]
        public ActionResult EmailUserName()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotUserName()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult RecoverUserName()
        {

            var ChallangeQuestion = _ObjChallangeQuestion.GetChallengeQuestion();
            ViewBag.Question = ChallangeQuestion.Select(x => new { QuestioId = x.ChallengeQuestionId, QuestionDescription = x.ChallengeQuestionDescription });
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RecapchaFilter]
        public async Task<ActionResult> RecoverUserName(RecoverUserNameViewModel ObjRecoverUserName, bool CaptchaValid)
        {
            if (!CaptchaValid)
            {
                //Captcha failed to validate
                ModelState.AddModelError("reCaptcha", "Invalid reCaptcha");

            }
            else
            {
                if (ModelState.IsValid)
                {

                    var user = await UserManager.FindByEmailAsync(ObjRecoverUserName.EmailId);
                    if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    {
                        ModelState.AddModelError("", "The user either does not exist or is not confirmed.");

                    }
                    else
                    {

                        if ((ObjRecoverUserName.SelectedQuestionId1 != 0 && !string.IsNullOrEmpty(ObjRecoverUserName.Answare1))
                            || (ObjRecoverUserName.SelectedQuestionId2 != 0 && !string.IsNullOrEmpty(ObjRecoverUserName.Answare2))
                            || (ObjRecoverUserName.SelectedQuestionId3 != 0 && !string.IsNullOrEmpty(ObjRecoverUserName.Answare3)))
                        {
                            Dictionary<int, string> AnswerList = new Dictionary<int, string>();
                            if (ObjRecoverUserName.SelectedQuestionId1 != 0 && ObjRecoverUserName.Answare1 != string.Empty)
                            {
                                AnswerList.Add(ObjRecoverUserName.SelectedQuestionId1, ObjRecoverUserName.Answare1);
                            }
                            if (ObjRecoverUserName.SelectedQuestionId2 != 0 && ObjRecoverUserName.Answare2 != string.Empty)
                            {
                                AnswerList.Add(ObjRecoverUserName.SelectedQuestionId2, ObjRecoverUserName.Answare2);
                            }
                            if (ObjRecoverUserName.SelectedQuestionId3 != 0 && ObjRecoverUserName.Answare3 != string.Empty)
                            {
                                AnswerList.Add(ObjRecoverUserName.SelectedQuestionId3, ObjRecoverUserName.Answare3);
                            }

                            bool IsAnswerCorrect = _ObjChallangeQuestion.IsAnswareCorrect(user.Id, AnswerList);
                            ///send mail to user user name
                            ///
                            if (IsAnswerCorrect)
                            {
                                string EmailSubject = "CBUSA Recover UserName";
                                StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/MailTemplate/RecoverUserName.html"));
                                StringBuilder Sb = new StringBuilder(reader.ReadToEnd());
                                Sb.Replace("####UserName####", user.UserName);
                                string StrBody = Sb.ToString();
                                IEmailSend Send = new EmailSendHtml();
                                bool IsEmailSendSuccessfull = Send.Send(EmailSubject, StrBody, ObjRecoverUserName.EmailId);

                                return RedirectToAction("RecoverUserNameConfirmation", "Account");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Please check your answer");
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Please answer one of the following question");
                        }
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            var ChallangeQuestion = _ObjChallangeQuestion.GetChallengeQuestion();
            ViewBag.Question = ChallangeQuestion.Select(x => new { QuestioId = x.ChallengeQuestionId, QuestionDescription = x.ChallengeQuestionDescription });

            return View(ObjRecoverUserName);
        }

        [AllowAnonymous]
        public ActionResult RecoverUserNameConfirmation()
        {
            return View();
        }




        public ActionResult EulaConfirmation()
        {
            string UserId = User.Identity.GetUserId();

           // UserInRole ObjUserInRole = _ObjUserInRoleServices.GetUserInRole().Where(x => x.UserId == Convert.ToInt32(UserId) && x.RoleId == (Int32)GetRoleName.Offender).FirstOrDefault();
            UserInRole ObjUserInRole = _ObjUserInRoleServices.RoleInformationByUserId(Convert.ToInt32(User.Identity.GetUserId()));

            Role _ObjRole = _ObjRoleServices.FindRole(ObjUserInRole.RoleId);

            // Neyaz 
            //if (_ObjRole.SystemRole ==(int) GetRoleName.Offender)
            //{
            //    ViewBag.IsDefendant = true;
            //    // Offender ObjOffender = _ObjOffenderServices.GetOffender().Where(w => w.OffenderUserId == Convert.ToInt32(UserId)).SingleOrDefault();

            //    ViewBag.OffenderId = _ObjOffenderServices.OffenderByUserId(Convert.ToInt32(User.Identity.GetUserId())).OffenderId;
            //}
            //else
            //{
            //    ViewBag.IsDefendant = false;
            //    ViewBag.OffenderId = 0;
            //}

            // var user = UserManager.FindById(Convert.ToInt32(UserId));
           // int Id = 0;
           // Id = Convert.ToInt32(UserId);
          //  List<int> t = UserManager.FindById(Id).Roles.Select(r => r.RoleId).ToList();


            // UserManager.getr//  var roles = UserManager.FindById(User.I).Roles.Select(r => r.RoleId);
            //  var RolesList = await UserManager.GetRolesAsync(Convert.ToInt32(UserId));
            /// var RolesList = await UserManager.g(Convert.ToInt32(UserId));
            //var RolesList = Roles.GetRolesForUser(User.Identity.GetUserName());

           // string EulaDescription = _ObjEulaServices.GetRoleWiseEula(t[0]);
            string EulaDescription = _ObjEulaServices.GetRoleWiseEula(ObjUserInRole.RoleId);
            ViewBag.EulaText = EulaDescription;
            var ChallangeQuestion = _ObjChallangeQuestion.GetChallengeQuestion().Where(x => x.RowStatusId == (int)RowActiveStatus.Active);
            ViewBag.Question = ChallangeQuestion.Select(x => new { QuestioId = x.ChallengeQuestionId, QuestionDescription = x.ChallengeQuestionDescription });
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EulaConfirmation(EulaConfirmationViewModel ObjEula)
        {
            var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));

           // UserInRole ObjUserInRole = _ObjUserInRoleServices.GetUserInRole().Where(x => x.UserId == Convert.ToInt32(LoginUser.Id) && x.RoleId == (Int32)GetRoleName.Offender).FirstOrDefault();
            UserInRole ObjUserInRole = _ObjUserInRoleServices.RoleInformationByUserId(Convert.ToInt32(User.Identity.GetUserId()));
            Role _ObjRole = _ObjRoleServices.FindRole(ObjUserInRole.RoleId);
            // Neyaz Offender ObjOffender = _ObjOffenderServices.GetOffender().Where(w => w.OffenderUserId == Convert.ToInt32(LoginUser.Id)).SingleOrDefault();

            //if (ObjUserInRole != null)
            //{
            // Neyaz 
            //if (_ObjRole.SystemRole == (int)GetRoleName.Offender)
            //{
            //    ViewBag.IsDefendant = true;
            //    // ViewBag.OffenderId = ObjOffender.OffenderId;
            //    ViewBag.OffenderId = _ObjOffenderServices.OffenderByUserId(Convert.ToInt32(User.Identity.GetUserId())).OffenderId;
            //}
            //else
            //{
            //    ViewBag.IsDefendant = false;
            //    ViewBag.OffenderId = 0;
            //}



           // string LoginId = User.Identity.GetUserId();
            
            //int Id = 0;
           // Id = Convert.ToInt32(LoginId);
          //  List<int> t = UserManager.FindById(LoginUser.Id).Roles.Select(r => r.RoleId).ToList();

          //  string EulaDescription = _ObjEulaServices.GetRoleWiseEula(t[0]);
            string EulaDescription = _ObjEulaServices.GetRoleWiseEula(ObjUserInRole.RoleId);
            ViewBag.EulaText = EulaDescription;
            var ChallangeQuestion = _ObjChallangeQuestion.GetChallengeQuestion().Where(w=>w.RowStatusId==(int)RowActiveStatus.Active);
            ViewBag.Question = ChallangeQuestion.Select(x => new { QuestioId = x.ChallengeQuestionId, QuestionDescription = x.ChallengeQuestionDescription });


            if (ModelState.IsValid)
            {
                if ((ObjEula.SelectedQuestionId1 == ObjEula.SelectedQuestionId2) || (ObjEula.SelectedQuestionId1 == ObjEula.SelectedQuestionId3) || (ObjEula.SelectedQuestionId2 == ObjEula.SelectedQuestionId3))
                {
                    ModelState.AddModelError("", "Please select diffarent eula question");
                }                
                else if (ObjEula.IsEulaChecked == false)
                {
                    ModelState.AddModelError("", "Please check EULA agreement");
                }                
                else
                {
                    if (ViewBag.IsDefendant)
                    {
                        if (ObjEula.IsConsentAgreementChecked == false)
                        {
                            ModelState.AddModelError("", "Please check Consent agreement");
                            return View(ObjEula);
                        }
                    }

                    string UserId = User.Identity.GetUserId();
                    var user = await UserManager.FindByIdAsync(Convert.ToInt32(User.Identity.GetUserId()));

                    List<UserChallangeQuestion> UserChallangeQuestionList = new List<UserChallangeQuestion>();
                    for (int i = 0; i < 3; i++)
                    {

                        UserChallangeQuestion obj = new UserChallangeQuestion();
                        obj.Id = user.Id;
                        obj.RowGUID = Guid.NewGuid();
                        obj.CreatedBy = LoginUser.Id;
                        obj.ModifiedBy = LoginUser.Id;

                        switch (i)
                        {
                            case 0:
                                obj.ChallengeQuestionId = ObjEula.SelectedQuestionId1;
                                obj.Answer = ObjEula.Answare1;
                                break;
                            case 1:
                                obj.ChallengeQuestionId = ObjEula.SelectedQuestionId2;
                                obj.Answer = ObjEula.Answare2;
                                break;
                            case 2:
                                obj.ChallengeQuestionId = ObjEula.SelectedQuestionId3;
                                obj.Answer = ObjEula.Answare3;
                                break;
                        }
                        UserChallangeQuestionList.Add(obj);
                    }
                    bool IsSaveSuccessfully = _ObjChallangeQuestion.SaveUserChallangeQuestion(UserChallangeQuestionList);
                    if (IsSaveSuccessfully)
                    {
                        user.IsEulaChecked = true;
                        user.IsSecondTimeLogin = true;
                        await UserManager.UpdateAsync(user);

                        // Neyaz 
                        //if (_ObjRole.SystemRole == (int)GetRoleName.Offender)
                        //{
                        //    Offender _objOffender = _ObjOffenderServices.OffenderByUserId(LoginUser.Id);
                        //    _objOffender.ConsentAgreementDate = CWHelper.SetUniversalDatetime();
                        //    _ObjOffenderServices.EditOffender(_objOffender);
                        //}


                        return RedirectToAction("MyProfile", "Account", new { SecondTimeLogin = false });
                        //return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(ObjEula);
        }

        // Neyaz 
        //[AllowAnonymous]
        //public ActionResult PublicFaq()
        //{            
        //    IEnumerable<Faq> FaqList = _ObjFaqService.GetFaq().Where(x => x.RowStatusId == (int)RowActiveStatus.Active);
        //    return View(FaqList);
        //}

        // Neyaz 
        //public async Task< ActionResult> MyProfile()
        //{

        //    Boolean SecondTimeLogin = Convert.ToBoolean(Request.Params["SecondTimeLogin"]);
        //    if (SecondTimeLogin)
        //    {
        //      //  ViewBag._SecondTimeLogin = "true";
        //        return RedirectToAction("MyDashboard", "Home");
        //    }
        //    else
        //    {
        //      //  ViewBag._SecondTimeLogin = "false";
        //    }

        //    var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));

        //    //Get Eula Text - Ritesh - 13th April 2017
        //    int Id = 0;
        //    Id = Convert.ToInt32(LoginUser.Id);
        //    List<int> t = UserManager.FindById(Id).Roles.Select(r => r.RoleId).ToList();
        //    string EulaDescription = _ObjEulaServices.GetRoleWiseEula(t[0]);
        //    ViewBag.EulaTextInMyProfile = EulaDescription;
        //    //Get Eula Text - Ritesh - 13th April 2017

        //    //Get If Logged IN User profile is of Defendant - Ritesh - 13th April 2017

        //    //UserInRole ObjUserInRole = _ObjUserInRoleServices.GetUserInRole().Where(x => x.UserId == Convert.ToInt32(LoginUser.Id) && x.RoleId == (Int32)GetRoleName.Offender).FirstOrDefault();
        //    // Offender ObjOffender = _ObjOffenderServices.GetOffender().Where(w => w.OffenderUserId == Convert.ToInt32(LoginUser.Id)).SingleOrDefault();

        //    // UserInRole ObjUserInRole = _ObjUserInRoleServices.RoleInformationByUserId(Convert.ToInt32(User.Identity.GetUserId()));
        //    IEnumerable< UserInRole> ObjUserInRole = await _ObjUserInRoleServices.RoleInformation_ByUserId(Convert.ToInt32(User.Identity.GetUserId()));


        //  //  Role _ObjRole = _ObjRoleServices.FindRole(ObjUserInRole.RoleId);


        //    // if (ObjUserInRole != null)
        //    if (ObjUserInRole.FirstOrDefault().Role.SystemRole == (int)GetRoleName.Offender)
        //    {
        //        ViewBag.IsDefendantInMyProfile = true;
        //       // ViewBag.OffenderIdInMyProfile = ObjOffender.OffenderId;
        //       ViewBag.OffenderIdInMyProfile= _ObjOffenderServices.OffenderByUserId(Convert.ToInt32(User.Identity.GetUserId())).OffenderId;
        //    }
        //    else
        //    {
        //        ViewBag.IsDefendantInMyProfile = false;
        //        ViewBag.OffenderIdInMyProfile = 0;
        //    }
        //    //Get If Logged IN User profile is of Defendant - Ritesh - 13th April 2017

        //    // int t = Convert.ToInt32("dassd");
        //    // IEnumerable<Faq> FaqList = _ObjFaqService.GetFaq().Where(x => x.RowStatusId == (int)RowActiveStatus.Active);
        //    ViewData["State"] = _ObjStateService.GetState().Where(x => x.RowStatusId == (int)RowActiveStatus.Active);
        //    var ChallangeQuestion = _ObjChallangeQuestion.GetChallengeQuestion().Where(w=>w.RowStatusId==(int)RowActiveStatus.Active);
        //    ViewData["Question"] = ChallangeQuestion.Select(x => new { QuestioId = x.ChallengeQuestionId, QuestionDescription = x.ChallengeQuestionDescription });

        //    MyProfileViewModel ObjMyProfile = new MyProfileViewModel();

        //   // UserInRole UserInRoledata = _ObjUserInRoleServices.GetUserInRole().Where(x => x.UserId == LoginUser.Id).SingleOrDefault();

        //    int SystemRoleIs = ObjUserInRole.FirstOrDefault().Role.SystemRole;
        //    bool IsOffender = (SystemRoleIs == (Int32)GetRoleName.Offender) ? true : false;
        //    var RolesList = UserManager.GetRoles(LoginUser.Id);
        //    // List<string> bb = RolesList.Result.ToList();
        //    string RoleName = "";

        //    if (RolesList.Count > 0)
        //    {
        //        RoleName = RolesList[0];
        //    }

        //    PersonalInfoViewModel ObjPersonalInfo = new PersonalInfoViewModel
        //     {
        //         UserId = LoginUser.Id,
        //         FirstName = LoginUser.FirstName,
        //         MiddleName = LoginUser.MiddleInit,
        //         LastName = LoginUser.LastName,
        //         CompanyName = LoginUser.CompanyTypeName,
        //         DateOfBirth = LoginUser.DOB,
        //         AddressLine1 = LoginUser.AddressLine1,
        //         AddressLine2 = LoginUser.AddressLine2,
        //         zip = LoginUser.Zip,
        //         City = LoginUser.City,
        //         State = LoginUser.State,
        //         Email = LoginUser.Email ,
        //         WorkPhone = LoginUser.Phone,
        //         CellPhone = LoginUser.PhoneNumber,
        //         RoleName = RoleName,
        //         Gender = LoginUser.Gender,
        //         IsOffender=IsOffender

        //     };

        //    ///Code Commented By Ritesh - 100616
        //    //UserProfile ObjUserProfile = _ObjUserProfileService.GetUserProfile(LoginUser.Id);
        //    ///Replaced Code By Ritesh - 100616
        //    UserProfile ObjUserProfile = _ObjUserProfileService.GetUserProfileByUserId(LoginUser.Id);


        //    if (ObjUserProfile != null && ObjUserProfile.ProfilePicture!=null)
        //    {
        //        ObjMyProfile.UserProfilePic = ObjUserProfile.ProfilePicture;
        //    }

        //    IEnumerable<UserChallangeQuestion> ObjUserChallangeQuestion =
        //        _ObjChallangeQuestion.GetUserChallengeQuestion(LoginUser.Id).OrderBy(x => x.Id);

        //    AccountSecurityViewModel ObjAccVM = new AccountSecurityViewModel();
        //    int Flag = 1;
        //    foreach (UserChallangeQuestion obj in ObjUserChallangeQuestion)
        //    {
        //        switch (Flag)
        //        {
        //            case 1:
        //                ObjAccVM.SelectedQuestionId1 = obj.ChallengeQuestionId;
        //                ObjAccVM.Answare1 = obj.Answer;
        //                break;
        //            case 2:
        //                ObjAccVM.SelectedQuestionId2 = obj.ChallengeQuestionId;
        //                ObjAccVM.Answare2 = obj.Answer;
        //                break;
        //            case 3:
        //                ObjAccVM.SelectedQuestionId3 = obj.ChallengeQuestionId;
        //                ObjAccVM.Answare3 = obj.Answer;
        //                break;
        //        }

        //        Flag = Flag + 1;

        //    }

        //    ObjMyProfile.PersonalInfo = ObjPersonalInfo;
        //    ObjMyProfile.AccountSecurity = ObjAccVM;
        //    return View(ObjMyProfile);
        //}

        // Neyaz 
        //[HttpPost]
        //public JsonResult SaveUserProfilePicture(int UserId)
        //{
        //    byte[] LogoImageByte = null;
        //    string LogoImageBase64 = "";
        //    HttpFileCollectionBase files = Request.Files;

        //    var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));

        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
        //        //string filename = Path.GetFileName(Request.Files[i].FileName);  

        //        HttpPostedFileBase file = files[i];
        //        string fname;

        //        // Checking for Internet Explorer  
        //        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //        {
        //            string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //            fname = testfiles[testfiles.Length - 1];
        //        }
        //        else
        //        {
        //            fname = file.FileName;
        //        }

        //        // Get the complete folder path and store the file inside it.  
        //        // fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
        //        // file.SaveAs(fname);
        //        BinaryReader rdr = new BinaryReader(file.InputStream);
        //        LogoImageByte = rdr.ReadBytes((int)file.ContentLength);
        //    }


        //    if (LogoImageByte != null)
        //    {
        //        LogoImageBase64 = Convert.ToBase64String(LogoImageByte);
        //        LogoImageBase64 = string.Format("data:image/png;base64,{0}", LogoImageBase64);
        //    }

        //    if (!_ObjUserProfileService.IsProfilePictureAvalaible(UserId))
        //    {
        //        UserProfile obj = new UserProfile();
        //        obj.ProfilePicture = LogoImageByte;
        //        obj.UserId = LoginUser.Id;
        //        obj.CreatedBy = LoginUser.Id;
        //        obj.ModifiedBy = LoginUser.Id;
        //        obj.RowGUID = Guid.NewGuid();
        //        _ObjUserProfileService.SaveUserProfile(obj);

        //    }
        //    else
        //    {
        //        UserProfile obj = _ObjUserProfileService.GetUserProfileByUserId(LoginUser.Id);

        //        obj.ProfilePicture = LogoImageByte;
        //        obj.ModifiedBy = LoginUser.Id;
        //        obj.ModifiedOn = DateTime.Now;

        //        _ObjUserProfileService.EditUserProfile(obj);

        //    }



        //    return Json(new { LogoImageBase64 = LogoImageBase64 });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveAccountSecurityDetails(AccountSecurityViewModel ObjAccountSecurity)
        {
            // string OldPassword=UserManager.pa


            if (ModelState.IsValid)
            {
                var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));
                bool IsOldPasswordValid = UserManager.CheckPassword(LoginUser, ObjAccountSecurity.OldPassword);

                int Flag = 0;
                if ((ObjAccountSecurity.SelectedQuestionId1 == ObjAccountSecurity.SelectedQuestionId2) || (ObjAccountSecurity.SelectedQuestionId1 == ObjAccountSecurity.SelectedQuestionId3) || (ObjAccountSecurity.SelectedQuestionId2 == ObjAccountSecurity.SelectedQuestionId3))
                {
                    ModelState.AddModelError("", "Please select diffarent question");
                    Flag = Flag + 1;
                }
                if (!IsOldPasswordValid)
                {
                    ModelState.AddModelError("", "Old password didn't match");
                    Flag = Flag + 1;
                }

                if (_ObjPasswordLogServices.IsPreviousPassword(LoginUser.Id, ObjAccountSecurity.NewPassword))
                {
                    ModelState.AddModelError("", "Can't use previous password");
                    Flag = Flag + 1;
                }


                if (Flag == 0)
                {
                    string code = UserManager.GeneratePasswordResetToken(LoginUser.Id);
                    UserManager.ResetPassword(LoginUser.Id, code, ObjAccountSecurity.NewPassword);

                    PasswordLog PwdLog = new PasswordLog();
                    PwdLog.UserId = LoginUser.Id;
                    PwdLog.CreateDate = DateTime.Now;
                    PwdLog.PasswordHash = Encrypt.EncryptString(ObjAccountSecurity.NewPassword);
                    _ObjPasswordLogServices.SavePasswordLog(PwdLog);



                    List<UserChallangeQuestion> UserChallangeQuestionList = _ObjChallangeQuestion.GetUserChallengeQuestion(LoginUser.Id).OrderBy(x => x.Id).ToList();
                    for (int i = 0; i < 3; i++)
                    {
                        UserChallangeQuestion obj = UserChallangeQuestionList[i];

                        obj.ModifiedOn = DateTime.Now;
                        obj.ModifiedBy = LoginUser.Id;

                        switch (i)
                        {
                            case 0:
                                obj.ChallengeQuestionId = ObjAccountSecurity.SelectedQuestionId1;
                                obj.Answer = ObjAccountSecurity.Answare1;
                                break;
                            case 1:
                                obj.ChallengeQuestionId = ObjAccountSecurity.SelectedQuestionId2;
                                obj.Answer = ObjAccountSecurity.Answare2;
                                break;
                            case 2:
                                obj.ChallengeQuestionId = ObjAccountSecurity.SelectedQuestionId3;
                                obj.Answer = ObjAccountSecurity.Answare3;
                                break;
                        }

                    }
                    bool IsSaveSuccessfully = _ObjChallangeQuestion.EditUserChallangeQuestion(UserChallangeQuestionList);
                    return Json(new { SuccessStatus = true });
                }
                else
                {
                    string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();
                    return Json(new { SuccessStatus = false, ModelError = BuildModelError.GetModelError(ModelError) });
                }
            }
            else
            {
                string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();

                return Json(new { SuccessStatus = false, ModelError = BuildModelError.GetModelError(ModelError) });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SavePersonalInfoDetails(PersonalInfoViewModel ObjPersonalInfo , string command="")
        {
            if (command == "Edit Email")
            {
               
                var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));
                LoginUser.Email = string.IsNullOrEmpty(ObjPersonalInfo.Email) ? "" : ObjPersonalInfo.Email;
                IdentityResult result = UserManager.Update(LoginUser);
                if (result.Succeeded)
                {
                    return Json(new { SuccessStatus = true });
                }
                else
                {
                    return Json(new { SuccessStatus = false, ModelError = "<div>Please try again, some unexpected error has occured</div>" });
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));

                    LoginUser.FirstName = ObjPersonalInfo.FirstName;
                    LoginUser.LastName = ObjPersonalInfo.LastName;
                    LoginUser.MiddleInit = ObjPersonalInfo.MiddleName;
                    LoginUser.DOB = ObjPersonalInfo.DateOfBirth;
                    LoginUser.Gender = ObjPersonalInfo.Gender;
                    LoginUser.AddressLine1 = ObjPersonalInfo.AddressLine1;
                    LoginUser.AddressLine2 = ObjPersonalInfo.AddressLine2;
                    LoginUser.Zip = ObjPersonalInfo.zip;
                    LoginUser.City = ObjPersonalInfo.City;
                    LoginUser.State = ObjPersonalInfo.State;

                    LoginUser.Email = string.IsNullOrEmpty(ObjPersonalInfo.Email) ? "" : ObjPersonalInfo.Email;
                    LoginUser.Phone = ObjPersonalInfo.WorkPhone;
                    LoginUser.PhoneNumber = ObjPersonalInfo.CellPhone;

                    IdentityResult result = UserManager.Update(LoginUser);
                    // UserManager.Update(LoginUser);
                    if (result.Succeeded)
                    {
                        return Json(new { SuccessStatus = true });
                    }
                    else
                    {
                        return Json(new { SuccessStatus = false, ModelError = "<div>Please try again, some unexpected error has occured</div>" });
                    }

                }
                else
                {
                    string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                        .SelectMany(E => E.Errors)
                        .Select(E => E.ErrorMessage)
                        .ToArray();

                    return Json(new { SuccessStatus = false, ModelError = BuildModelError.GetModelError(ModelError) });
                }
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditEmailDetails(string Email)
        {
            if (ModelState.IsValid)
            {
                var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));

                //LoginUser.FirstName = ObjPersonalInfo.FirstName;
                //LoginUser.LastName = ObjPersonalInfo.LastName;
                //LoginUser.MiddleInit = ObjPersonalInfo.MiddleName;
                //LoginUser.DOB = ObjPersonalInfo.DateOfBirth;
                //LoginUser.Gender = ObjPersonalInfo.Gender;
                //LoginUser.AddressLine1 = ObjPersonalInfo.AddressLine1;
                //LoginUser.AddressLine2 = ObjPersonalInfo.AddressLine2;
                //LoginUser.Zip = ObjPersonalInfo.zip;
                //LoginUser.City = ObjPersonalInfo.City;
                //LoginUser.State = ObjPersonalInfo.State;

                LoginUser.Email = Email;
                //LoginUser.Phone = ObjPersonalInfo.WorkPhone;
                //LoginUser.PhoneNumber = ObjPersonalInfo.CellPhone;

                IdentityResult result = UserManager.Update(LoginUser);
                // UserManager.Update(LoginUser);
                if (result.Succeeded)
                {
                    return Json(new { SuccessStatus = true });
                }
                else
                {
                    return Json(new { SuccessStatus = false, ModelError = "<div>Please try again, some unexpected error has occured</div>" });
                }

            }
            else
            {
                string[] ModelError = ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();

                return Json(new { SuccessStatus = false, ModelError = BuildModelError.GetModelError(ModelError) });
            }


        }

        // Neyaz 
        //public ActionResult ContactUs()
        //{

        //    // String hashedNewPassword = UserManager.PasswordHasher.HashPassword("Rabi");
        //    //  String hashedNewPassword = UserManager.PasswordHasher.            
        //    //  ADrs7mQjUpwNSB0pucBs5EjSymVFJh8iscMc3pDJY5Iqtw/D0w0AFvcKWIdSgqt2Yw==
        //    // Microsoft.AspNet.Identity.PasswordVerificationResult asd = UserManager.PasswordHasher.VerifyHashedPassword("", "");



        //    UserContactUs ObjUserContactUs = new UserContactUs();
        //    var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));
        //    ViewBag.UserName = LoginUser.UserName;
        //    return View(ObjUserContactUs);
        //}

        // Neyaz 
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public ActionResult ContactUs(UserContactUs ContacUs)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var LoginUser = UserManager.FindById(Convert.ToInt32(User.Identity.GetUserId()));
        //        string EmailSubject = "CourtWorks Contact Us";
        //        StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/MailTemplate/UserContactUs.html"));
        //        StringBuilder Sb = new StringBuilder(reader.ReadToEnd());
        //        Sb.Replace("####Email####", LoginUser.Email);
        //        Sb.Replace("####Query####", ContacUs.UserQuery);
        //        string StrBody = Sb.ToString();
        //        IEmailSend Send = new EmailSendHtml();
        //        //ConfigurationManager.AppSettings["CourtWorksAdminEmail"]
        //        string CourtWorksContactUsEmail = _ObjCourtWorksSettingsService.GetCourtWorksSetting().FirstOrDefault().ContactUsEmail;
        //        bool IsEmailSendSuccessfull = Send.Send(EmailSubject, StrBody, CourtWorksContactUsEmail);
        //        if (IsEmailSendSuccessfull)
        //        {
        //            return RedirectToAction("ContactUsMessage", "Account");
        //        }
        //    }

        //    return View(ContacUs);
        //}

        public ActionResult ContactUsMessage()
        {
            return View();
        }       

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(CBUSA.Domain.CustomIdentityModel.ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword(int ContactedUserID)
        {
            //receive and set the userID below to allow Concern person to create contacted user password            
            var user = UserManager.FindById(ContactedUserID);

            //var user = UserManager.FindById(int.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            //if (Url.IsLocalUrl(returnUrl))
            //{
            //    //return Redirect(returnUrl);
            //    if (Url.IsLocalUrl(returnUrl))
            //return RedirectToAction("Index", "Home", new { returnUrl = returnUrl });
            return RedirectToAction("Index", "Dashboard", new { returnUrl = returnUrl });
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}