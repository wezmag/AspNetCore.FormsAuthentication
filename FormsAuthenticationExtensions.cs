using Microsoft.AspNetCore.Authentication;
using System;

namespace AspNetCore.FormsAuthentication
{
    public static class FormsAuthenticationExtensions
    {
        public static AuthenticationBuilder AddFormsAuthentication(this AuthenticationBuilder builder, Action<FormsAuthenticationOptions> configureOptions)
            => builder.AddFormsAuthentication(FormsAuthenticationDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddFormsAuthentication(this AuthenticationBuilder builder, string authenticationScheme, Action<FormsAuthenticationOptions> configureOptions)
            => builder.AddFormsAuthentication(authenticationScheme, displayName: null, configureOptions);

        public static AuthenticationBuilder AddFormsAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<FormsAuthenticationOptions> configureOptions)
            => builder.AddScheme<FormsAuthenticationOptions, FormsAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}
