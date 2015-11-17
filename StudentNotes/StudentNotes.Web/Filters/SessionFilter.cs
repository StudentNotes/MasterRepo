using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudentNotes.Web.Filters
{
    public class SessionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["CurrentUserId"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                    //{
                    //    //Data = "SessionExpired",
                    //    //JsonRequestBehavior = JsonRequestBehavior.AllowGet

                    //};
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"Controller", "LoggedIn"},
                        {"Action", "SessionExpired"}
                    });
                }
                
            }
        }
    }
}