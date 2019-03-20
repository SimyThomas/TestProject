using Owin;
using System.Web.Http;
using Ninject.Web.Common.OwinHost;
using Ninject;
using Ninject.Web.WebApi.OwinHost;

namespace SalesService
{
   public class Startup
    {
        /// <summary>
        /// Configuration Method 
        /// This code configures Web API. The Startup class is specified as a type
        /// parameter in the WebApp.Start method.
        /// </summary>
        /// <param name="appBuilder"></param>
        public void Configuration(IAppBuilder appBuilder)
        {
            IKernel kernel = new StandardKernel();
            Configuration(appBuilder, kernel);
        }

        /// <summary>
        /// Configuration Method 
        /// This code configures Web API. The Startup class is specified as a type
        /// parameter in the WebApp.Start method and Kernel.
        /// </summary>
        public void Configuration(IAppBuilder appBuilder, IKernel kernel)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always
            };

            // Attribute routing.
            config.MapHttpAttributeRoutes();         
            appBuilder.UseNinjectMiddleware(() => kernel);
            appBuilder.UseNinjectWebApi(config);
        }  
    }
}
