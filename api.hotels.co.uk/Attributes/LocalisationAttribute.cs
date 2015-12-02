using System;
using System.Configuration;
using System.Globalization;
using System.Resources;
using System.Threading;
using ServiceStack.Web;

namespace hotels.triperoo.co.uk.Attributes
{
    /// <summary>
    /// Provides localisation
    /// </summary>
    public class LocalisationAttribute : Attribute, IHasRequestFilter
    {
        /// <summary>
        /// Attributes with a negative priority get executed before any global
        /// filters, this makes them good for things like authentication...
        /// </summary>
        public int Priority
        {
            get
            {
                return -10;
            }
        }

        /// <summary>
        /// Checks for a valid localisation cookie and sets the current thread context
        /// </summary>
        public void RequestFilter(IRequest req, IResponse res, object requestDto)
        {
            // Retrieve the locale from the cookie
            string locale = null;
            if (req.Cookies.ContainsKey(ConfigurationManager.AppSettings.Get("triperoo:localeCookieName")))
            {
                var localeCookie = req.Cookies[ConfigurationManager.AppSettings.Get("triperoo:localeCookieName")];
                if (localeCookie != null)
                {
                    locale = localeCookie.Value;
                }
            }
            
            // Do we now have a valid locale?
            if (string.IsNullOrEmpty(locale))
            {
                return;
            }

            // Apply the locale to the current threads resource manager
            var cinfo = new CultureInfo(locale);
            var rm = new ResourceManager(typeof(Translation.ApiErrors)).GetResourceSet(cinfo, true, false);
            if (rm != null)
            {
                Thread.CurrentThread.CurrentUICulture = cinfo;
            }
        }

        /// <summary>
        /// Returns a copy of the attribute
        /// </summary>
        public IHasRequestFilter Copy()
        {
            return new LocalisationAttribute();
        }
    }
}

