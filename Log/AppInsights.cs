using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.Controller.Log
{
    public class AppInsights
    {
        private TelemetryClient ai;

        public AppInsights(string appSettingsTiKey)
        {
            ai = new TelemetryClient();
            if (string.IsNullOrEmpty(ai.InstrumentationKey))
            {
                if (!string.IsNullOrEmpty(appSettingsTiKey))
                {
                    TelemetryConfiguration.Active.InstrumentationKey = appSettingsTiKey;
                    ai.InstrumentationKey = appSettingsTiKey;
                    ai.Context.Cloud.RoleName = "InSys";
                }
                else
                {
                    throw new Exception("Could not find instrumentation key for Application Insights");
                }
            }
        }
        public void LogException(Exception ex)
        {
            ai?.TrackException(ex);
            // ai.Flush();
        }

        public void LogInfo(string Info, SeverityLevel lvl = SeverityLevel.Information)
        {
            ai?.TrackTrace(Info, lvl);
        }


        public static AppInsights logger;

        public static void Initiate(string key)
        {
            logger = new AppInsights(key);
        }

        public static void LogEx(Exception ex)
        {
            logger?.LogException(ex);
        }

        public static void Log(string Info, SeverityLevel lvl = SeverityLevel.Information)
        {
            logger?.LogInfo(Info, lvl);
        }

    }
}
