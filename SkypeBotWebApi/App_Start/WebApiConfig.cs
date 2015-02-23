using Microsoft.Practices.Unity;
using SkypeBotRulesLibrary;
using SkypeBotRulesLibrary.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SkypeBotWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<IRuleDal, FakeRuleDal>(new PerResolveLifetimeManager());
            container.RegisterType<IRuleService, BasicRuleService>(new PerResolveLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


        }
    }
}
