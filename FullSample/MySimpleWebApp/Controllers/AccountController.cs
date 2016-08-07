using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MySimpleWebApp.Auth;
using MySimpleWebApp.Models;
using MySimpleWebApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MySimpleWebApp.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        private ApplicationUserManager _userManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private mydbEntities _db;

        private const string _KAKAO_APP_KEY = "12597124990e39ac2d964b38b3fe87fa";

        public AccountController()
        {
            _db = new mydbEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _userManager.Dispose();
            _signInManager.Dispose();
            _db.Dispose();

            base.Dispose(disposing);
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
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            var loginInfo = await authenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return _RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account

                    return View("SignIn", new AccountSignInViewModel
                    {
                        Email = loginInfo.Email,
                        Name = loginInfo.DefaultUserName,
                        LoginProvider = loginInfo.Login.LoginProvider,
                        ReturnUrl = returnUrl,
                    });
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        private ActionResult _RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/LoginOrSignIn
        [AllowAnonymous]
        public ActionResult LoginOrSignIn(string returnUrl)
        {
            var model = new AccountLoginOrSignInModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(AccountSignInViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var info = await authenticationManager.GetExternalLoginInfoAsync();

                //if (info == null)
                //{
                //    return View("ExternalLoginFailure");
                //}

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);


                var myUser = _db.MyUsers.FirstOrDefault(u => u.Id == user.Id);

                if (myUser == null)
                {
                    myUser = new MyUser();
                    myUser.Id = user.Id;
                    _db.MyUsers.Add(myUser);
                    await _db.SaveChangesAsync();
                }

                myUser.Name = model.Name;
                myUser.Address = model.Address;
                myUser.PhoneNumber = model.PhoneNumber;
                myUser.BirthDate = model.BirthDate;
                await _db.SaveChangesAsync();

                if (result.Succeeded)
                {
                    if (info == null)
                    {
                        result = await _userManager.AddLoginAsync(user.Id, new UserLoginInfo(model.LoginProvider, model.LoginProvider));
                    }
                    else
                    {
                        result = await _userManager.AddLoginAsync(user.Id, info.Login);
                    }

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return _RedirectToLocal(model.ReturnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("", error);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> Edit()
        {
            var id = this.User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(id);
            var myUser = _db.MyUsers.FirstOrDefault(u => u.Id == user.Id);
            var model = new AccountEditViewModel();
            model.Address = myUser.Address;
            model.BirthDate = myUser.BirthDate ?? (DateTime)myUser.BirthDate;
            model.Email = user.Email;
            model.Name = myUser.Name;
            model.PhoneNumber = myUser.PhoneNumber;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AccountEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);



            var userId = this.User.Identity.GetUserId();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(userId);
            var myUser = _db.MyUsers.FirstOrDefault(u => u.Id == user.Id);

            myUser.Address = model.Address;
            myUser.BirthDate = model.BirthDate;
            myUser.Name = model.Name;
            myUser.PhoneNumber = model.PhoneNumber;
            await _db.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error);
                    }

                    return View(model);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/LoginWithPassword
        [AllowAnonymous]
        public ActionResult LoginWithPassword()
        {
            var model = new AccountLoginWithPasswordViewModel();
            return this.View(model);
        }

        //
        // POST: /Account/LoginWithPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginWithPassword(AccountLoginWithPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                default:
                    this.ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // POST: /Account/ExternalLoginKakao
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginKakao(string returnUrl)
        {
            this.Session.Add("returnUrl", returnUrl);

            using (var client = new HttpClient())
            {
                var redirect_uri = "";

                if ((this.Request.Url.Scheme == "http" && this.Request.Url.Port == 80) ||
                    (this.Request.Url.Scheme == "https" && this.Request.Url.Port == 443))
                {
                    redirect_uri = $"{this.Request.Url.Scheme}://{this.Request.Url.Host}/account/oauth_kakao";
                }
                else
                {
                    redirect_uri = $"{this.Request.Url.Scheme}://{this.Request.Url.Host}:{this.Request.Url.Port}/account/oauth_kakao";
                }

                Trace.TraceInformation("redirect_uri : " + redirect_uri);
                redirect_uri = HttpUtility.UrlEncode(redirect_uri);
                var url = $"https://kauth.kakao.com/oauth/authorize?client_id={_KAKAO_APP_KEY}&redirect_uri={redirect_uri}&response_type=code";
                Trace.TraceInformation($"ExternalLoginKakao Redirect url : {url}");
                return this.Redirect(url);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> oauth_kakao(string code)
        {
            Trace.TraceInformation($"oauth_kakao code : {code}");
            KakaoTokenResponse kakaoTokenRes;
            KakaoMeResponse kakaoMeRes;

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", _KAKAO_APP_KEY),
                    new KeyValuePair<string, string>("code", code),
                });

                var res = await client.PostAsync("https://kauth.kakao.com/oauth/token", content);
                var resStr = await res.Content.ReadAsStringAsync();
                kakaoTokenRes = JsonConvert.DeserializeObject<KakaoTokenResponse>(resStr);
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", kakaoTokenRes.access_token);
                var resStr = await client.GetStringAsync("https://kapi.kakao.com/v1/user/me");
                kakaoMeRes = JsonConvert.DeserializeObject<KakaoMeResponse>(resStr);
            }

            var email = $"{kakaoMeRes.id}@kakao.com";
            var user = await _userManager.FindByEmailAsync(email);
            var returnUrl = this.Session["returnUrl"].ToString();
            returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;

            if (user == null)
            {
                return this.View("SignIn", new AccountSignInViewModel
                {
                    Email = $"{kakaoMeRes.id}@kakao.com",
                    Name = kakaoMeRes.properties.nickname,
                    LoginProvider = "Kakao",
                    ReturnUrl = returnUrl,
                });
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return this.Redirect(returnUrl);
            }
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (UserId != null)
                {
                    // Used for XSRF protection when adding external logins
                    properties.Dictionary["XsrfId"] = UserId;
                }

                var authenticationManager = context.HttpContext.GetOwinContext().Authentication;
                authenticationManager.Challenge(properties, LoginProvider);
            }
        }
    }




    public class KakaoTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
    }

    public class KakaoMeResponse
    {
        public int id { get; set; }
        public Properties properties { get; set; }

        public class Properties
        {
            public string nickname { get; set; }
            public string thumbnail_image { get; set; }
            public string profile_image { get; set; }
        }
    }



}