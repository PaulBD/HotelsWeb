using System.Collections.Generic;
using System;

namespace core.customers.services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly string _secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        public string AuthorizeCustomer(string emailAddress, string password)
        {
            //TODO: Check username & Password exists
            var customerId = 0;
            var payload = new Dictionary<string, object>()
            {
                { "emailAddress", emailAddress },
                { "customerId", customerId }
            };

            return JWT.JsonWebToken.Encode(payload, _secretKey, JWT.JwtHashAlgorithm.HS256);

        }

        public string AuthorizeCustomer(string token)
        {
            try
            {
                return JWT.JsonWebToken.Decode(token, _secretKey);
            }
            catch (JWT.SignatureVerificationException)
            {
                throw new UnauthorizedAccessException("You do not have access");
            }
        }
    }
}
