[assembly: WebActivator.PreApplicationStartMethod(typeof(MealsToGo.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MealsToGo.App_Start.NinjectWebCommon), "Stop")]

namespace MealsToGo.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using MealsToGo.Service;
    using System.Web.Http;
    using SolrNet;
    using SolrNet.Impl;
    using System.Configuration;
    using MealsToGo.Models;
    using Ninject.Integration.SolrNet;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        private static readonly string solrURL = ConfigurationManager.AppSettings["solrUrl"];


        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {


            var kernel = new StandardKernel(new MealItemModule(), new MealAdModule(),new SettingsModule(),new ContactModule(), new SolrNetModule(solrURL));
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Get<IMealAdService>();
            kernel.Get<IUserSettingService>();
            kernel.Get<IMealItemService>();
            kernel.Get<IContactService>();
            kernel.Get<ISolrOperations<Product>>();


            
           // RegisterServices(kernel);
            // Install our Ninject-based IDependencyResolver into the Web API config
           // GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            return kernel;
        }

            



    }
}
