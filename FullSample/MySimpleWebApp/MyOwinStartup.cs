using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MySimpleWebApp.Auth;
using MySimpleWebApp.Filters;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(MySimpleWebApp.MyOwinStartup))]
namespace MySimpleWebApp
{
    public class MyOwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            _Configure_OwinContext(app);
            _Configure_WebApi(app);
            _Configure_Mvc(app);
            _Configure_Signalr(app);
            _Configure_Bundles();
        }

        private void _Configure_OwinContext(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
        }

        private void _Configure_Mvc(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();

            var routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



            GlobalFilters.Filters.Add(new MyMvcExceptionFilterAttribute());
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            GlobalFilters.Filters.Add(new System.Web.Mvc.AuthorizeAttribute());
            GlobalFilters.Filters.Add(new RequireHttpsAttribute());



            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/LoginOrSignIn"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            app.UseGoogleAuthentication(
                clientId: "520679049346-m7at30ohs2ufmhg3b51l8k9omn9lep6e.apps.googleusercontent.com",
                clientSecret: "aorG0GzyT2vaoFNQjZ4joB0o");
        }

        private void _Configure_WebApi(IAppBuilder app)
        {
            GlobalConfiguration.Configure((config) =>
            {
                // Web API configuration and services

                // Web API routes
                config.MapHttpAttributeRoutes();

                //config.Filters.Add(new MyLogFilterAttribute());
                config.Filters.Add(new MyWebApiExceptionFilterAttribute());
                //config.Filters.Add(new System.Web.Http.AuthorizeAttribute());
                config.Filters.Add(new MyWebApiRequireHttpsAttribute());

                //http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api
                //In the WebApiConfig.Register method, the following code sets up authentication for the Web API pipeline:
                config.SuppressDefaultHostAuthentication();
                config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            });



            //http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api
            //Configuring the Authorization Server
            //In StartupAuth.cs, the following code configures the OAuth2 authorization server.
            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider("self"),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // Note: Remove the following line before you deploy to production:
                AllowInsecureHttp = true,
            });
        }

        private void _Configure_Signalr(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(new HubConfiguration() { EnableJSONP = true, });
        }

        private void _Configure_Bundles()
        {
            var bundles = BundleTable.Bundles;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.signalR").Include(
                "~/Scripts/jquery.signalR-{version}.js"));
        }

    }



}