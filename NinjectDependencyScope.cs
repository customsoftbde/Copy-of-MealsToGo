using System;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;
using MealsToGo.Service;
using Ninject.Modules;
using System.Collections.Generic;
using System.Configuration;
using Ninject.Integration.SolrNet.Config;

//using SolrNet;
//using SolrNet.Impl;
//using SolrNet.Impl.DocumentPropertyVisitors;
//using SolrNet.Impl.FacetQuerySerializers;
//using SolrNet.Impl.FieldParsers;
//using SolrNet.Impl.FieldSerializers;
//using SolrNet.Impl.QuerySerializers;
//using SolrNet.Impl.ResponseParsers;
//using SolrNet.Mapping;
//using SolrNet.Mapping.Validation;
//using SolrNet.Mapping.Validation.Rules;
//using SolrNet.Schema;
//using SolrNet.Utils;
using MealsToGo.Models;

namespace MealsToGo.App_Start
{
   
    
    // Provides a Ninject implementation of IDependencyScope
   // which resolves services using the Ninject container.
   public class NinjectDependencyScope : IDependencyScope
   {
      IResolutionRoot resolver;

      public NinjectDependencyScope(IResolutionRoot resolver)
      {
         this.resolver = resolver;
      }

      public object GetService(Type serviceType)
      {
         if (resolver == null)
            throw new ObjectDisposedException("this", "This scope has been disposed");

         return resolver.TryGet(serviceType);
      }

      public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
      {
         if (resolver == null)
            throw new ObjectDisposedException("this", "This scope has been disposed");

         return resolver.GetAll(serviceType);
      }

      public void Dispose()
      {
         IDisposable disposable = resolver as IDisposable;
         if (disposable != null)
            disposable.Dispose();

         resolver = null;
      }
   }

   // This class is the resolver, but it is also the global scope
   // so we derive from NinjectScope.
   public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
   {
       IKernel kernel;

       public NinjectDependencyResolver(IKernel kernel)
           : base(kernel)
       {
           this.kernel = kernel;
       }

       public IDependencyScope BeginScope()
       {
           return new NinjectDependencyScope(kernel.BeginBlock());
       }
   }

   public class MealItemModule : NinjectModule
   {
       public override void Load()
       {
           Bind<IMealItemService>().To<MealItemService>();
         
       }
   }

   public class MealAdModule : NinjectModule
   {
       public override void Load()
       {
           Bind<IMealAdService>().To<MealAdService>();
       }
   }
   public class SettingsModule : NinjectModule
   {
       public override void Load()
       {
           Bind<IUserSettingService>().To<UserSettingService>();
       }
   }
   public class ContactModule : NinjectModule
   {
       public override void Load()
       {
           Bind<IContactService>().To<ContactService>();
       }
   }
  
}

