using System.Collections.Generic;
using System;
using core.customers.dtos;
using JWT;
using JWT.Serializers;
using JWT.Algorithms;

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

            var encoder = new JwtEncoder(new HMACSHA256Algorithm(), new JsonNetSerializer(), new JwtBase64UrlEncoder());
            return encoder.Encode(payload, _secretKey);
        }

        public TokenDto AuthorizeCustomer(string token)
        {
            try
            {
                IJwtDecoder decoder = new JwtDecoder(new JsonNetSerializer(), new JwtValidator(new JsonNetSerializer(), new UtcDateTimeProvider()), new JwtBase64UrlEncoder());

                var json = decoder.Decode(token, _secretKey, verify: true);

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
