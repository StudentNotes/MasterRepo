using System.Web.Mvc;
using System.Web.Routing;
using StudentNotes.Web;


namespace StudentNotes.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
