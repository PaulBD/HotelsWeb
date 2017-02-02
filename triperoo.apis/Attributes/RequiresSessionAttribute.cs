using core.customers.services;
using ServiceStack;
using ServiceStack.Web;
using System;
using System.Net;

namespace triperoo.apis.Attributes
{
    public class RequiresSessionAttribute : Attribute, IHasRequestFilter
    {
        // Dependencies
        public IAuthorizeService AuthorizeService { get; set; }

        /// <summary>
        /// Attributes with a negative priority get executed before any global
        /// filters, this makes them good for things like authentication...
        /// </summary>
        public int Priority
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// Returns a copy of the attribute
        /// </summary>
        public IHasRequestFilter Copy()
        {
            return new RequiresSessionAttribute();
        }

        public void RequestFilter(IRequest req, IResponse res, object requestDto)
        {        
            var securityToken = req.Headers.Get("securityToken");
            
            if (string.IsNullOrEmpty(securityToken))
            {
                throw new HttpError(HttpStatusCode.Unauthorized, string.Format("Unable to resolve a valid security token {0}", securityToken));
            }
            
            var user = AuthorizeService.AuthorizeCustomer(securityToken);
            if (user == null)
            {
                throw new HttpError(HttpStatusCode.Unauthorized, "The specified account does not exist");
            }
        }
    }
}