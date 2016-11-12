using System.Configuration;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Analog_Tamigo_API.Logic;
using Unity.WebApi;

namespace Analog_Tamigo_API
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration configuration)
        {
			var container = new UnityContainer();
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ITamigoClient, CachedTamigoClient>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new TamigoClient(
                        ConfigurationManager.AppSettings["TamigoUsername"],
                        ConfigurationManager.AppSettings["TamigoPassword"]
                    )
                )
            );
            
            configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}