using System.Collections.Generic;
using System;
using core.customers.dtos;

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

        public TokenDto AuthorizeCustomer(string token)
        {
            try
            {
                var json = JWT.JsonWebToken.Decode(token, _secretKey);

                if (json != null)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<TokenDto>(json);
                }

                return null;
            }
            catch (JWT.SignatureVerificationException)
            {
                throw new UnauthorizedAccessException("You do not have access");
            }
        }
    }
}
