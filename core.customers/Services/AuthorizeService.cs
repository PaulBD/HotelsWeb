using System.Collections.Generic;
using System;

namespace core.customers.services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly string _secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrkTriperoo";

        public string AssignToken(string emailAddress, string reference)
        {
            var payload = new Dictionary<string, object>()
            {
                { "emailAddress", emailAddress },
                { "reference", reference }
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
