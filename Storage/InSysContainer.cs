using Microsoft.WindowsAzure.Storage.Blob;

namespace z.Controller.Storage
{
    public class InSysContainer
    {
        public CloudBlobContainer Container { get; private set; }

        public string Name { get; private set; }

        public InSysContainer(string Name)
        {
            this.Name = Name;
        }

        public InSysContainer(CloudBlobContainer container)
        {
            this.Container = container;
        }

        public InSysDirectory GetDirectoryReference(string filename)
        {
            return new InSysDirectory(this, filename);
        }

        public static InSysContainer Create(string Name)
        {
            return new InSysContainer(Name);
        }

        public static InSysContainer Create(CloudBlobContainer container)
        {
            return new InSysContainer(container);
        }

    }
}
