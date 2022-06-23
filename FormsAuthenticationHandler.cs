using AspNetCore.LegacyAuthCookieCompat;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AspNetCore.FormsAuthentication
{
    public class FormsAuthenticationHandler : AuthenticationHandler<FormsAuthenticationOptions>, IAuthenticationSignOutHandler
    {
        public FormsAuthenticationHandler(
            IOptionsMonitor<FormsAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
             : base(options, logger, encoder, clock)
        {

        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var loginUri = Options.LoginUrl + QueryString.Create(Options.ReturnUrlParameter, CurrentUri);

            if (IsAjaxRequest(Context.Request))
            {
                Context.Response.Headers["Location"] = loginUri;
                Context.Response.StatusCode = 401;
            }
            else
            {
                Context.Response.Redirect(loginUri);
            }
            return Task.CompletedTask;
        }

        private static bool IsAjaxRequest(HttpRequest request)
        {
            return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.ContainsKey(Options.FormsAuthenticationCookieName))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            try
            {
                var cookie = Request.Cookies[Options.FormsAuthenticationCookieName];
                var decryptionKeyBytes = HexUtils.HexToBinary(Options.DecryptionKey);
                var validationKeyBytes = HexUtils.HexToBinary(Options.ValidationKey);
                var legacyFormsAuthenticationTicketEncryptor = new LegacyFormsAuthenticationTicketEncryptor(decryptionKeyBytes, validationKeyBytes, EnumHelper.ShaVersionConverter(Options.ShaVersion), EnumHelper.CompatibilityModeConverter(Options.CompatibilityMode));
                var decryptedTicket = legacyFormsAuthenticationTicketEncryptor.DecryptCookie(cookie);

                if (decryptedTicket.Expired)
                    return Task.FromResult(AuthenticateResult.Fail("Ticket expired."));

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, decryptedTicket.Name),
                    new Claim(ClaimTypes.Name, decryptedTicket.Name)
                };

                Options.UserDataHandler(decryptedTicket.UserData, claims);

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new System.Security.Principal.GenericPrincipal(identity, null);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return Task.FromResult(AuthenticateResult.Fail("Invalid ticket."));
            }
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            if (Request.Cookies.ContainsKey(Options.FormsAuthenticationCookieName))
            {
                Response.Cookies.Delete(Options.FormsAuthenticationCookieName, new CookieOptions() {
                    Domain = Options.Domain,
                    Path = Options.Path
                });
            }
            return Task.CompletedTask;
        }
    }
}
