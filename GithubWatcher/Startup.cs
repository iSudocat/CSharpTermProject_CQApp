using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Owin;

namespace GithubWatcher
{
    public class Startup
    {
        /// <summary>
        /// Configures Web API
        /// </summary>
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{code}",
                defaults: new { code = RouteParameter.Optional }
                );

            appBuilder.UseWebApi(config);
        }
    }
}