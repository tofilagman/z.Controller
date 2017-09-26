using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.Controller.Storage
{
    public class InSysBlob
    {
        private CloudBlockBlob cloudBlockBlob;
        //private InSysDirectory dr;
        private InSysContainer Cntr;

        public InSysBlob(CloudBlockBlob cloudBlockBlob)
        {
            this.cloudBlockBlob = cloudBlockBlob;
            this.Name = cloudBlockBlob.Name;
        }

        public InSysBlob(InSysDirectory dr, string v)
        {
            //this.dr = dr;
            this.Cntr = dr.inSysContainer;
            this.Name = Path.Combine(dr.Name, v);
        }

        public InSysBlob(InSysContainer Cntr, string Name)
        {
            this.Cntr = Cntr;
            this.Name = Name;
        }

        public string Name { get; set; }

        public string GetSharedAccessSignature(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers)
        {
            if (cloudBlockBlob != null)
                return cloudBlockBlob.GetSharedAccessSignature(policy, headers);
            return $"?v={ DateTime.Now.ToString("yyyyMMdd") }";
        }

        public string AbsoluteUri
        {
            get
            {
                if (cloudBlockBlob != null)
                    return cloudBlockBlob.Uri.AbsoluteUri;
                else
                {
                    return Path.Combine(Storage.RequestPath, Cntr.Name, this.Name).Replace('\\', '/');
                }
            }
        }
    }
}
