using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Controller.Log;

namespace z.Controller
{
    /// <summary>
    /// Must add Under web.config 
    /// <handlers>  
    /// <add name = "Owin" verb="" path="InSysStorage/*" type="Microsoft.Owin.Host.SystemWeb.OwinHttpHandler, Microsoft.Owin.Host.SystemWeb" />
    /// </handlers> 
    /// </summary>
    public class StartupBase
    {
        public bool EnableDirectoryBrowsing { get; set; } = false;

        public virtual void Configuration(IAppBuilder app)
        {
            if (!Config.IsCloudApp)
            {
                AppInsights.Initiate(Config.Get("APPINSIGHTS_INSTRUMENTATIONKEY").ToString());

                var fileSystem = new PhysicalFileSystem(Config.Get("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString").ToString());
                var options = new FileServerOptions
                {
                    FileSystem = fileSystem,
                    EnableDirectoryBrowsing = EnableDirectoryBrowsing,
                    RequestPath = PathString.FromUriComponent(Storage.Storage.RequestPath)
                };

                app.UseFileServer(options);
            }
        }
    }
}
