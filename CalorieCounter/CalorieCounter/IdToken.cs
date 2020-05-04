using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
    /// <summary>
    /// The values returned with the IdToken retrieved from Google auth
    /// </summary>
    [JsonObject]
    public class IdToken
    {
        [JsonProperty("iss")]
        public string Iss { get; set; }

        [JsonProperty("azp")]
        public string Azp { get; set; }

        [JsonProperty("aud")]
        public string Aud { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("hd")]
        public string Hd { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public string EmailVerified { get; set; }

        [JsonProperty("at_hash")]
        public string AtHash { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }
        
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("iat")]
        public string Iat { get; set; }

        [JsonProperty("exp")]
        public string Exp { get; set; }




    }
}
