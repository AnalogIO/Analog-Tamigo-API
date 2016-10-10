using System;
using System.Configuration;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Analog_Tamigo_API.Mappers;
using Analog_Tamigo_API.Models;
using TamigoServices;
using TamigoServices.Models.Responses;
using Unity.WebApi;

namespace Analog_Tamigo_API
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration configuration)
        {
            var container = new UnityContainer();

            container.RegisterType<IMapper<Shift, ShiftDto>, ShiftMapper>(new ContainerControlledLifetimeManager())
                .RegisterType<IMapper<Contact, VolunteerDto>, VolunteerMapper>(new ContainerControlledLifetimeManager())
                .RegisterType<ITamigoUserClient, TamigoUserClient>(new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(
                        new Uri(ConfigurationManager.AppSettings["TamigoApi"]),
                        ConfigurationManager.AppSettings["TamigoUsername"],
                        ConfigurationManager.AppSettings["TamigoPassword"]
                    )
                );

            configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}