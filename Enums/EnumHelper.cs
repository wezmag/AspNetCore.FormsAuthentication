namespace AspNetCore.FormsAuthentication
{
    public static class EnumHelper
    {
        public static LegacyAuthCookieCompat.ShaVersion ShaVersionConverter(ShaVersion shaVersion)
        {
            return shaVersion switch
            {
                ShaVersion.Sha1 => LegacyAuthCookieCompat.ShaVersion.Sha1,
                ShaVersion.Sha256 => LegacyAuthCookieCompat.ShaVersion.Sha256,
                ShaVersion.Sha512 => LegacyAuthCookieCompat.ShaVersion.Sha512,
                ShaVersion.Sha384 => LegacyAuthCookieCompat.ShaVersion.Sha384,
                _ => LegacyAuthCookieCompat.ShaVersion.Sha1,
            };
        }

        public static LegacyAuthCookieCompat.CompatibilityMode CompatibilityModeConverter(CompatibilityMode compatibilityMode)
        {
            return compatibilityMode switch
            {
                CompatibilityMode.Framework20SP2 => LegacyAuthCookieCompat.CompatibilityMode.Framework20SP2,
                CompatibilityMode.Framework45 => LegacyAuthCookieCompat.CompatibilityMode.Framework45,
                _ => LegacyAuthCookieCompat.CompatibilityMode.Framework20SP2,
            };
        }
    }
}
