# FormsAuthentication cookie validation for ASP.NET Core

## Intro

Let's say if you have a Single Sign-On (SSO) server that uses previous generation ASP.NET FormsAuthentication, and you want to create a web application with ASP.NET Core, that uses the SSO server to authenticate users. This is the project for you. 

Otherwise, you should use ASP.NET Core Cookie Authentication or ASP.NET Core Identity instead.

If you are looking for a way to implement SSO Auth server, this project is also NOT for you.

## Reference NuGet Package

This project uses [AspNetCore.LegacyAuthCookieCompat 2.0.3](https://www.nuget.org/packages/AspNetCore.LegacyAuthCookieCompat/) NuGet package to decrypt the FormsAuthentication ticket.

You can find the source code of AspNetCore.LegacyAuthCookieCompat [here](https://github.com/dazinator/AspNetCore.LegacyAuthCookieCompat).

## Usage

1. Add this project to your solution.
2. Add reference to your website project.
3. Open Startup.cs, and modify as below.

``` csharp
using AspNetCore.FormsAuthentication;
```

``` csharp
services
    .AddAuthentication(FormsAuthenticationDefaults.AuthenticationScheme)
    .AddFormsAuthentication(FormsAuthenticationDefaults.AuthenticationScheme, options => {
        options.FormsAuthenticationCookieName = ".YOUR_COOKIE_NAME";
        options.ShaVersion = ShaVersion.Sha512;
        options.CompatibilityMode = CompatibilityMode.Framework20SP2;
        options.ValidationKey = "BABDFE50AA92635D82648CE163FA72B5319A6D9A33584283F6E6843583F4C5FD8B858D15E3D0687A98F592588A3FE6F1687C3946843317523295552840197DCA";
        options.DecryptionKey = "960018AAD852DA413E15141EB9453801D7C659EC50FD530015F860065D4CFA73";
        options.LoginUrl = "https://your.sso.server.url/loginpage.aspx";
        options.ReturnUrlParameter = "ReturnUrl";
        options.UserDataHandler = (string userData, List<Claim> claims) =>
        {
            //just an example, please write your own handler.
            var member = JsonConvert.DeserializeObject<MemberAuthData>(userData);
            claims.Add(new Claim(ClaimTypes.Email, member.Email));
            claims.Add(new Claim(ClaimTypes.GivenName, member.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, member.LastName));
            return claims;
        };
    });
```

``` csharp
app.UseRouting();
app.UseAuthentication(); // add this line
app.UseAuthorization();
```

4. Add `[Authorize]` attribute to the page/action/controller that requires login.

5. To logout, just call `HttpContext.SignOutAsync();`. The FormsAuthentication cookie will be removed. 

## Options

You will need the web.config file from your SSO Server to set these options.

### FormsAuthenticationCookieName

This is the name of your FormsAuthentication cookie issued by the SSO Server. 

The default value is `.ASPXAUTH`.

You can find it in your web.config file.

``` xml
<authentication mode="Forms">
    <forms name=".YOUR_COOKIE_NAME" .... />
</authentication>
```

### ShaVersion

This setting is from your web.config machineKey section validation value.



``` xml
<machineKey validation="SHA1" ... />
```

| Validation Value | ShaVerison |
| --- | --- |
| SHA1 | ShaVersion.Sha1 |
| HMACSHA256 | ShaVersion.Sha256 |
| HMACSHA512 | ShaVersion.Sha512 |

The default value is `ShaVersion.Sha1`.

### CompatibilityMode

This setting is from your web.config machineKey section compatibilityMode value.



``` xml
<machineKey validation="SHA1" validationKey="XXXXX" decryption="AES" decryptionKey="XXXXX" compatibilityMode="Framework20SP2" />
```

Available options are `CompatibilityMode.Framework20SP2` and `CompatibilityMode.Framework45`.

The default value is `Framework20SP2`.

### ValidationKey [required]

This setting is from your web.congfig machineKey section validationKey value.

``` xml
<machineKey validationKey="BABDFE50AA92635D82648CE163FA72B5319A6D9A33584283F6E6843583F4C5FD8B858D15E3D0687A98F592588A3FE6F1687C3946843317523295552840197DCA" ... />
```

### DecryptionKey [required]

This setting is from your web.config machineKey section descrptionKey value.

``` xml
<machineKey decryptionKey="960018AAD852DA413E15141EB9453801D7C659EC50FD530015F860065D4CFA73" ... />
```

### LoginUrl [required]

This is the login page url of the SSO server.

If an amonymous user visits a page that requires login, he will be redirect to this page. 

### ReturnUrlParameter

This is the QueryString parameter used by the SSO server to indicate the redirect url after a success login.

For example:
If your login url is `https://sso.server/login.aspx?retUrl=https%3A%2F%2Fother.website%2Fadmin%2F`, then the ReturnUrlParameter is `retUrl`.

The default value is `ReturnUrl`.

### UserDataHandler

This is the function to process [FormsAuthenticationTicket.UserData](https://docs.microsoft.com/en-us/dotnet/api/system.web.security.formsauthenticationticket.userdata?view=netframework-4.8).

If you have additional data that is stored in `FormsAuthenticationTicket.UserData`, you can set the handler here to process it into claims.

By default, two claims `NameIdentifier` and `Name` will be set to the value of `FormsAuthenticationTicket.Name`. If you want to override them, you can also do it here.

You can just leave it, if you do not have data stored in `FormsAuthenticationTicket.UserData`.































