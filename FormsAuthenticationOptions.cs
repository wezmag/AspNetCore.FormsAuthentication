using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCore.FormsAuthentication
{
    public class FormsAuthenticationOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// FormsAuthentication Cookie Name (Default value is ".ASPXAUTH")
        /// </summary>
        public string FormsAuthenticationCookieName { get; set; } = ".ASPXAUTH";
        /// <summary>
        /// SHA version (Default value is ShaVersion.Sha1)
        /// </summary>
        public ShaVersion ShaVersion { get; set; } = ShaVersion.Sha1;
        /// <summary>
        /// Compatibility Mode (Default value is CompatibilityMode.Framework20SP2)
        /// </summary>
        public CompatibilityMode CompatibilityMode { get; set; } = CompatibilityMode.Framework20SP2;
        /// <summary>
        /// Validation Key 
        /// </summary>
        public string ValidationKey { get; set; }
        /// <summary>
        /// Decryption Key
        /// </summary>
        public string DecryptionKey { get; set; }
        /// <summary>
        /// Return Url Parameter (Default value is "ReturnUrl")
        /// </summary>
        public string ReturnUrlParameter { get; set; } = "ReturnUrl";
        /// <summary>
        /// Login Url
        /// </summary>
        public string LoginUrl { get; set; }
        /// <summary>
        /// Function to process FormsAuthticationTicket.UserData
        /// </summary>
        public Func<string, List<Claim>, List<Claim>> UserDataHandler { get; set; } = (string userData, List<Claim> claims) => { return claims; };

        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrWhiteSpace(ValidationKey))
                throw new ArgumentNullException($"{nameof(ValidationKey)} can not be null or empty.");

            if (string.IsNullOrWhiteSpace(DecryptionKey))
                throw new ArgumentNullException($"{nameof(DecryptionKey)} can not be null or empty.");

            if (string.IsNullOrWhiteSpace(LoginUrl))
                throw new ArgumentNullException($"{nameof(LoginUrl)} can not be null or empty.");

        }
    }
}
