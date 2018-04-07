using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GroceryCo
{
    public static class Utils
    {
        public static T SafeParse<T>(this string value, T defaultValue)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));

                if (converter != null)
                {
                    return (T)converter.ConvertFromString(value);
                }

                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static string SafeGetConfigString(string configAppSettingsKey, string defaultValueIfNotFound)
        {
            string config = ConfigurationManager.AppSettings[configAppSettingsKey];
            if (false == String.IsNullOrEmpty(config))
                return config;
            else
                return defaultValueIfNotFound;
        }

        public static bool IsCurrentDateTimeWithinRange(DateTime fromDateTime, DateTime toDateTime)
        {
            DateTime currentDateTime = DateTime.Now;
            return (currentDateTime >= fromDateTime &&
                        currentDateTime <= toDateTime);
        }


    }
}
