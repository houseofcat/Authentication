using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Library
{
    public static class Utils
    {
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public static string Env => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Debug";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Write(string messageTemplate, params object[] parameters)
        {
            return string.Format(CultureInfo.InvariantCulture, messageTemplate, parameters);
        }
    }
}
