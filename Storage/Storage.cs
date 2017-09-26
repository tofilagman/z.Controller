using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace z.Controller.Storage
{
    public class Storage : IDisposable
    {
        private CloudStorage AzureStorage;
        private InSysStorage InSysStorage;
        private bool IsCloud;

        public const string RequestPath = "/InSysStorage";

        public Storage()
        {
            IsCloud = Config.IsCloudApp;
            var g = Config.Get("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString").ToString();
            if (IsCloud)
                AzureStorage = new CloudStorage(g);
            else
                InSysStorage = new InSysStorage(g);
        }

        public InSysContainer Container(string Name)
        {
            if (IsCloud)
            {
                return InSysContainer.Create(AzureStorage.Container(Name));
            }
            else
                return InSysContainer.Create(Name);
        }
         
        public IEnumerable<InSysBlob> ListBlob(InSysContainer Cntr)
        {
            if (IsCloud)
            {
                return AzureStorage.ListBlob(Cntr.Container).Where(x => x is CloudBlockBlob).Select(x => new InSysBlob(x as CloudBlockBlob));
            }
            else
                return InSysStorage.ListBlob(Cntr);
        }

        public IEnumerable<InSysBlob> ListBlob(InSysDirectory Dr)
        {
            if (IsCloud)
                return AzureStorage.ListBlob(Dr.Directory).Select(x => new InSysBlob(x as CloudBlockBlob));
            else
                return InSysStorage.ListBlob(Dr);
        }

        public string DownloadString(InSysContainer Cntr, string str)
        {
            if (IsCloud)
            {
                return AzureStorage.DownloadString(Cntr.Container, str);
            }
            else
            {
                return InSysStorage.DownloadString(Cntr, str);
            }
        }

        public void DownloadToStream(InSysContainer Cntr, string Name, Stream Target)
        {
            if (IsCloud)
            {
                AzureStorage.DownloadToStream(Cntr.Container, Name, Target);
            }
            else
            {
                InSysStorage.DownloadToStream(Cntr, Name, Target);
            }
        }

        public void Dispose()
        {
            AzureStorage?.Dispose();
            InSysStorage?.Dispose();
        }

        public void Delete(InSysContainer cntr, string str)
        {
            if (IsCloud)
            {
                AzureStorage.Delete(cntr.Container, str);
            }
            else
            {
                InSysStorage.Delete(cntr, str);
            }
        }

        public void Upload(InSysContainer cntr, string filename, Stream fp)
        {
            if (IsCloud)
            {
                AzureStorage.Upload(cntr.Container, filename, fp);
            }
            else
            {
                InSysStorage.Upload(cntr, filename, fp);
            }
        }

        public void Upload(InSysContainer cntr, string filename, string base64)
        {
            if (IsCloud)
                AzureStorage.Upload(cntr.Container, filename, base64);
            else
                InSysStorage.Upload(cntr, filename, base64);
        }

        public string ToURLSlug(string s)
        {
            if (IsCloud)
                return Regex.Replace(s, @"[^a-z0-9/.]+", "-", RegexOptions.IgnoreCase)
                  .Trim(new char[] { '-' })
                  .ToLower();
            else
                return s;
        }

        public void DownloadToFile(InSysContainer cntr, string filename, string fg)
        {
            if (IsCloud)
            {
                AzureStorage.DownloadToFile(cntr.Container, filename, fg);
            }
            else
            {
                InSysStorage.DownloadToFile(cntr, filename, fg);
            }
        }
    }
}
