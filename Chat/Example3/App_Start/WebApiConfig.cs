using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Example3
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );




            // Validation
            config.Filters.Add(new ValidateModelStateAttribute());

            // Security
            config.Filters.Add(new BasicAuthenticationAttribute("Chat"));
            config.Filters.Add(new AuthorizeAttribute());

            // Tracing
            config.MessageHandlers.Add(new TraceHandler());
        }
    }
}
