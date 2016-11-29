using System.Web.Http;

namespace Analog_Tamigo_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(UnityConfig.RegisterComponents);
        }
    }
}
