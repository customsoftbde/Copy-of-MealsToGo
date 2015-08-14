#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.ServiceLocation;
using MealsToGo.Models;
using MealsToGo.Models.Binders;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Exceptions;
using SolrNet.Impl;
using System.Collections;
using System.Web.Http;
using WebMatrix.WebData;
using System.Web.Optimization;
//using GeoCoding;
//using GeoCoding.Google;
//using GeoCoding.Microsoft;
//using GeoCoding.Yahoo;
using System.Data.Entity;
using MealsToGo.Service;
using Microsoft.Practices.Unity;
using System.Collections.Generic;






namespace MealsToGo {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication {
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();
        private static readonly string solrURL = ConfigurationManager.AppSettings["solrUrl"];
        protected void Session_End(object sender, EventArgs e)
        {
            List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID).ToList();
            if (lstTempOrderList.Count > 0)
            {
                foreach (TempOrderList tempOrderList in lstTempOrderList)
                {
                    dbmeals.TempOrderLists.Remove(tempOrderList);
                }
                dbmeals.SaveChanges();
            }
        }

       

        protected void Application_Start() {
        

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleMobileConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
           
          
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
           
          
            Startup.Init<Product>(solrURL);
            ModelBinders.Binders[typeof(SearchParam)] = new SearchParamBinder();

            AutoMapperConfiguration.Configure();
           
        }
      
      
      

        /// <summary>
        /// Gets a controller instance with its dependencies injected.
        /// Picks the first constructor on the controller.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="t">Controller type</param>
        /// <returns></returns>
        public IController GetContainerRegistration(IServiceProvider container, Type t) {
            var constructor = t.GetConstructors()[0];
            var dependencies = constructor.GetParameters().Select(p => container.GetService(p.ParameterType)).ToArray();
            return (IController) constructor.Invoke(dependencies);
        }

        public string GetControllerName(Type t) {
            return Regex.Replace(t.Name, "controller$", "", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Registers controllers in the DI container
        /// </summary>
        public void RegisterAllControllers()
        {

          


            var controllers = typeof (MvcApplication).Assembly.GetTypes().Where(t => typeof (IController).IsAssignableFrom(t));
            foreach (var controller in controllers) {
                if (controller.Name == "HomeController")
                {

                    Startup.Container.Register(GetControllerName(controller), controller, c => GetContainerRegistration(c, controller));


                }
              

                          

               }
          
           
           
            
            
        }
    }
}