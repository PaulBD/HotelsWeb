using ServiceStack.Text;

namespace triperoo.apis.Configuration
{
    /// <summary>
    /// Static class used to register any custom serialiser behaviour
    /// </summary>
    public static class Serialiser
    {
        /// <summary>
        /// Register a custom serialiser for enums
        /// </summary>
        public static void Configure()
        {
            // Set JSON web services to return idiomatic JSON camelCase properties
            JsConfig.EmitCamelCaseNames = true;

            // Enums should be serialised in camel case

            // Ensure all dates and times are serialised in UTC format
            JsConfig.AssumeUtc = true;
            JsConfig.DateHandler = DateHandler.ISO8601;

        }
    }
}