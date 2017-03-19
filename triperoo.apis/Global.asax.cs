using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using triperoo.apis.Configuration;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Caching;
using ServiceStack.Validation;
using ServiceStack.Text;

namespace triperoo.apis
{
    public class Global : HttpApplication
    {
        /// <summary>
        /// ApiHost is the global Service Stack singleton, responsible for initialisation and configuration
        /// of the Service Stack platform
        /// </summary>        
        public class CmsHost : AppHostBase
        {
            /// <summary>
            /// Initializes a new instance of the ServiceStack application, with the specified name and assembly containing the services.
            /// </summary>
            public CmsHost() : base("triperoo.apis", typeof(CmsHost).Assembly)
            {
                Trace.Listeners.Add(new DefaultTraceListener());
            }

            /// <summary>
            /// Global exception logging
            /// </summary>
            private object HandleException(IResolver iocResolver, object request, Exception ex)
            {
                return new HttpResult(ex.Message, (HttpStatusCode)ex.ToStatusCode());
            }

            /// <summary>
            /// Configure the FunQ container, register the services, repositories, authorisation, etc..
            /// </summary>
            /// <param name="container">The built-in IoC used with ServiceStack.</param>
            public override void Configure(Container container)
            {
                container.Register<ICacheClient>(new MemoryCacheClient());

                // Configure the global exception handlers
                ServiceExceptionHandlers.Add((httpReq, request, exception) => HandleException(this, request, exception));

                // Default to JSON responses, no nasty XML here
                SetConfig(new HostConfig
                {
                    EnableFeatures = Feature.All.Remove(Feature.Jsv | Feature.Soap),
                    DefaultContentType = "application/json",
                    DebugMode = false,
                    ApiVersion = "v1",
                    ReturnsInnerException = true
                });

                // Set JSON web services to return idiomatic JSON camelCase properties
                //JsConfig.EmitCamelCaseNames = true;

                // Ensure all dates and times are serialised in UTC format
                JsConfig.AssumeUtc = true;
                JsConfig.DateHandler = DateHandler.ISO8601;
                //JsConfig.PropertyConvention = PropertyConvention.Lenient;

                // Configure the default ServiceStack serialiser
                Serialiser.Configure();

                // Set the reuse scope for the DI container
                container.DefaultReuse = ReuseScope.Request;

                // Register every required dependency with FUNQ
                Repositories.Register(container);
                Services.Register(container);

                // Register all our AutoMapper maps
                Mappings.Register();

                // Register the required Service Stack validators
                Plugins.Add(new ValidationFeature());
                container.RegisterValidators(typeof(CmsHost).Assembly);

                // Add the required Service Stack plugins
                Plugins.Add(new RequestLogsFeature());
                Plugins.Add(new CorsFeature());
            }
        }

        /// <summary>
        /// Main application entry point
        /// </summary>
        protected void Application_Start(object sender, EventArgs e)
        {
            (new CmsHost()).Init();
        }
    }
}