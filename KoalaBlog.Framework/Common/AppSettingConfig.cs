using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace KoalaBlog.Framework.Common
{
    public static class AppSettingConfig
    {
        private static string AppSettingValue([CallerMemberName]string key = null)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string BaseAddress
        {
            get
            {
                return AppSettingValue();
            }
        }

    }
}