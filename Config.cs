using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.Controller
{
    /// <summary>
    /// Web Configuration
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        /// Get Config
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static object Get(string Name)
        {
            if (IsCloudApp)
                return RoleEnvironment.GetConfigurationSettingValue(Name);
            else
                return ConfigurationManager.AppSettings.Get(Name);
        }

        /// <summary>
        /// Check if Cloud Service
        /// </summary>
        public static bool IsCloudApp
        {
            get
            {
                try
                {
                    return RoleEnvironment.IsAvailable;
                }
                catch
                {
                    return false;
                }
            }
        }

    }
}
