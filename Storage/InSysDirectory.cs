using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.Controller.Storage
{
    public class InSysDirectory
    {
        public InSysContainer inSysContainer { get; private set; }

        public CloudBlobDirectory Directory { get; private set; }

        public InSysDirectory(InSysContainer inSysContainer, string filename)
        {
            this.inSysContainer = inSysContainer;
            this.Name = filename;

            if (this.inSysContainer.Container != null)
                this.Directory = this.inSysContainer.Container.GetDirectoryReference(this.Name);
        }

        public string Name { get; internal set; }

        public string Prefix
        {
            get
            {
                if (this.inSysContainer.Container != null)
                    return this.Directory.Prefix;
                else
                    return $"{this.Name}\\";
            }
        }
    }
}
