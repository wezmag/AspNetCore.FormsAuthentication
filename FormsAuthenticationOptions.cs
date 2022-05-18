using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCore.FormsAuthentication
{
    public class FormsAuthenticationOptions : AuthenticationSchemeOptions
    {
        public FormsAuthenticationOptions()
        {
            FormsAuthenticationCookieName = ".ASPXAUTH";
            ShaVersion = ShaVersion.Sha1;
            CompatibilityMode = CompatibilityMode.Framework20SP2;
            ReturnUrlParameter = "ReturnUrl";
            UserDataHandler = (string userData, List<Claim> claims) => { return claims; };
            Path = "/";
        }

        /// <summary>
        /// FormsAuthentication Cookie Name (Default value is ".ASPXAUTH")
        /// </summary>
        public string FormsAuthenticationCookieName { get; set; }
        /// <summary>
        /// SHA version (Default value is ShaVersion.Sha1)
        /// </summary>
        public ShaVersion ShaVersion { get; set; }
        /// <summary>
        /// Compatibility Mode (Default value is CompatibilityMode.Framework20SP2)
        /// </summary>
        public CompatibilityMode CompatibilityMode { get; set; }
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
        public string ReturnUrlParameter { get; set; }
        /// <summary>
        /// Login Url
        /// </summary>
        public string LoginUrl { get; set; }
        /// <summary>
        /// Cookie Domain
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// Cookie Path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Function to process FormsAuthticationTicket.UserData
        /// </summary>
        public Func<string, List<Claim>, List<Claim>> UserDataHandler { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (string.IsNullOrWhiteSpace(ValidationKey))
                throw new ArgumentNullException($"{nameof(ValidationKey)} can not be null or empty.");

            if (string.IsNullOrWhiteSpace(DecryptionKey))
                throw new ArgumentNullException($"{nameof(DecryptionKey)} can not be null or empty.");

            if (string.IsNullOrWhiteSpace(LoginUrl))
                throw new ArgumentNullException($"{nameof(LoginUrl)} can not be null or empty.");

            if (string.IsNullOrWhiteSpace(Domain))
                throw new ArgumentNullException($"{nameof(Domain)} can not be null or empty.");

        }
    }
}
