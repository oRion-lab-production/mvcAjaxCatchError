using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            /*accept custom error validation for ajax request*/
            HttpException httpException = ex as HttpException;
            RequestContext requestContext = ((MvcHandler)Context.CurrentHandler).RequestContext;
            if (requestContext.HttpContext.Request.IsAjaxRequest())
            {
                Context.ClearError();

                Context.Response.ContentType = "application/json";
                Context.Response.StatusCode = 500;

                if (httpException != null)
                    Context.Response.StatusCode = httpException.GetHttpCode();

                // if debug mode return detail system error else return only client viewable error with log id for reference //security issue level
#if DEBUG
                Context.Response.Write(new JavaScriptSerializer().Serialize(new
                {
                    applicationError = new { logMessage = ex.Message, logSource = ex.Source, logStackTrace = ex.StackTrace }
#else 
                Context.Response.Write( new JavaScriptSerializer().Serialize( new {
                    applicationError = new { logMessage = "Something not right!." }
#endif
                }));
            }
            /*end of custom error*/
        }
    }
}
