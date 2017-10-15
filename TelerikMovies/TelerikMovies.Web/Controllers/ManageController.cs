using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TelerikMovies.Web.Models;
using TelerikMovies.Services.Contracts.Auth;
using Bytes2you.Validation;
using Common;

namespace TelerikMovies.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ISignInManagerService signInService;
        private readonly IUserManagerService userService;

        public ManageController(IUserManagerService userManager, ISignInManagerService signInManager)
        {
            Guard.WhenArgument(userManager, Common.Constants.UserManager).IsNull().Throw();
            Guard.WhenArgument(signInManager, Common.Constants.SignInManager).IsNull().Throw();

            this.signInService = signInManager;
            this.userService = userManager;
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = this.User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await this.userService.GetPhoneNumberAsync(userId),
                TwoFactor = await this.userService.GetTwoFactorEnabledAsync(userId),
                Logins = await this.userService.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }
  
        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userName = User.Identity.GetUserId();
            var result = await this.userService.ChangePasswordAsync(userName, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await this.userService.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await this.signInService.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.userService != null)
                {
                    this.userService.Dispose();
                }

                if (this.signInService != null)
                {
                    this.signInService.Dispose();
                }
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = this.userService.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

#endregion
    }
}