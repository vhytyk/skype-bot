using Microsoft.Practices.Unity;
using SkypeBotRulesLibrary;
using SkypeBotRulesLibrary.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SkypeBotRulesLibrary.Implementations;
using SkypeBotRulesLibrary.Interfaces;

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
            container.RegisterType<ISkypeNameDal, SkypeNameDal>(new PerResolveLifetimeManager());
            container.RegisterType<ISkypeNameService, SkypeNameService>(new PerResolveLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
            );


        }
    }
}
