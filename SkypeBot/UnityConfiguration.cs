using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using SkypeBot.BotEngine;
using SkypeBot.BotEngine.EngineImplementations._7._0;

namespace SkypeBot
{
    public class UnityConfiguration
    {
        #region Singleton
        static UnityConfiguration()
        {
        }

        static readonly UnityConfiguration instance = new UnityConfiguration();

        public static UnityConfiguration Instance
        {
            get { return instance; }
        }

        #endregion

        IUnityContainer _container = new UnityContainer();

        public void RegisterTypes()
        {
            _container.RegisterType<IBotCoreService, BotCoreService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISkypeInitService, SkypeInitService70>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISkypeSendMessageService, SkypeSendMessageService70>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISkypeListener, SkypeListener70>(new ContainerControlledLifetimeManager());
        }

        public T Reslove<T>()
        {
            return (T)_container.Resolve(typeof (T));
        }

    }
}
