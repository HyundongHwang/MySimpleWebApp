using MySimpleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.Owin;

namespace MySimpleWebApp.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : Controller
    {
        private mydbEntities _db = new mydbEntities();

        [AllowAnonymous]
        // GET: Home
        public async Task<ActionResult> Index()
        {
            if (this.User.Identity != null &&
                this.User.Identity.GetUserId() != null)
            {
                var id = this.User.Identity.GetUserId();
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    this.ViewBag.userStr = $"user : {JsonConvert.SerializeObject(user, Formatting.Indented)}";
                }

                var myUser = _db.MyUsers.FirstOrDefault(u => u.Id == id);

                if (myUser != null)
                {
                    this.ViewBag.myUserStr = $"myUser : {JsonConvert.SerializeObject(myUser, Formatting.Indented)}";
                }
            }

            return View();
        }

        [Route("printnum/{num}")]
        public ActionResult PrintNum(int num)
        {
            return this.Content($"{num} 입니다.");
        }

        [Route("printtime/{dt}")]
        public ActionResult PrintTime(DateTime dt)
        {
            return this.Content($"{dt} 입니다.");
        }

        public ActionResult ThrowException()
        {
            throw new Exception("my exception");
        }

        public async Task<ActionResult> Orders()
        {
            return this.View();
        }

        public async Task<ActionResult> Chat()
        {
            return this.View();
        }

        public async Task<ActionResult> TestSignalRMore()
        {
            return this.View();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}