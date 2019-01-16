using CBUSA.Models;
using CBUSA.Services.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CBUSA.Domain;
namespace CBUSA.Areas.CbusaBuilder.Controllers
{
    public class AccountController : Controller
    {
        // GET: CbusaBuilder/Account
        readonly IBuilderService _ObjBuilderService;
        public AccountController(IBuilderService ObjBuilderService)
        {
            _ObjBuilderService = ObjBuilderService;
        }


        [AllowAnonymous]
        public ActionResult Login(string UserId, string Flag)
        {
            /*change have been made when go to live*/
            //   string BuilderId
            string DecryptBuilderUserId = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(UserId));
            Int64 CBUSABuilderUserId = Convert.ToInt64(DecryptBuilderUserId);
            var User = _ObjBuilderService.IsUserAuthenticate(CBUSABuilderUserId);
            /*End*/
            // Int64 BuilderId
            //   var User = _ObjBuilderService.IsBuilderAuthenticate(BuilderId);
            //  var User = _ObjBuilderService.IsUserAuthenticate(UserId);


            if (User != null)
            {

                var Builder = User.Builder;
                if (Builder.RowStatusId == (int)RowActiveStatus.Active)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Sid, User.BuilderId.ToString()));
                    claims.Add(new Claim(ClaimTypes.PrimarySid, User.BuilderUserId.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, User.FirstName + " " + User.LastName));
                    claims.Add(new Claim(ClaimTypes.Email, User.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Builder"));
                    // claims.Add(new Claim(ClaimTypes.id, "brockallen@gmail.com"));
                    var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                    var ctx = Request.GetOwinContext();
                    var authenticationManager = ctx.Authentication;
                    authenticationManager.SignIn(id);

                    if (Flag != null)
                    {

                        if (Flag == "SubmitReport")
                        {
                            return RedirectToAction("RegularReporting", "Builder", new { Area = "CbusaBuilder" });
                        }

                    }

                    return RedirectToAction("Dashboard", "Builder", new { Area = "CbusaBuilder" });
                }
                else
                {
                    if (Builder.RowStatusId == (int)RowActiveStatus.Archived)
                    {
                        ViewBag.IsArchiveBuilder = true;
                        ViewBag.IsUnauthorizeBuilder = false;
                    }
                    else
                    {
                        ViewBag.IsArchiveBuilder = false;
                        ViewBag.IsUnauthorizeBuilder = true;
                    }
                }
            }
            else
            {
                ViewBag.IsArchiveBuilder = false;
                ViewBag.IsUnauthorizeBuilder = true;
            }
            return View();
        }

        public ActionResult Home(string returnUrl)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var Name = claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
            string ccc = claims.FirstOrDefault(p => p.Type == ClaimTypes.Email).Value;
            string id = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel_Builder model, string returnUrl)
        {
            return View();
        }

        private List<Claim> GetClaims()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, "assilabdulrahim@gmail.com"));
            claims.Add(new Claim(CustomUserIdentity.IPClaimType, "128.236.333.2"));
            claims.Add(new Claim(CustomUserIdentity.IdClaimType, "12345"));
            claims.Add(new Claim(ClaimTypes.Name, "Assil"));
            claims.Add(new Claim(ClaimTypes.Name, "Abdulrahim"));

            var roles = new[] { "Admin", "Citizin", "Worker" };
            var groups = new[] { "Admin", "Citizin", "Worker" };

            foreach (var item in roles)
            {
                claims.Add(new Claim(CustomUserIdentity.RolesClaimType, item));
            }
            foreach (var item in groups)
            {
                claims.Add(new Claim(CustomUserIdentity.GroupClaimType, item));
            }
            return claims;
        }
        private void SignIn(List<Claim> claims)//Mind!!! This is System.Security.Claims not WIF claims
        {

            var claimsIdentity = new CustomUserIdentity(claims,
            DefaultAuthenticationTypes.ApplicationCookie);
            //This uses OWIN authentication

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, claimsIdentity);


            // HttpContext.User = new CustomUserIdentity(AuthenticationManager.AuthenticationResponseGrant.Principal);
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}