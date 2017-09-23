using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace HoneywellHackathonFileShare
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new CustomIdProvider());
        }
    }

    //public class CustomIdProvider : IUserIdProvider
    //{
    //    public string GetUserId(IRequest request)
    //    {
    //        Cookie cookie;
    //        request.Cookies.TryGetValue("_usertoken", out cookie);

    //        if (cookie != null)
    //        {
    //            return cookie.Value;
    //        }

    //        throw new Exception("user not found");
    //    }
    //}
}
