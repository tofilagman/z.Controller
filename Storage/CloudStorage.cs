using Microsoft.WindowsAzure.Storage;
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
    /// <summary>
    /// LJ 20170711
    /// </summary>
    public class CloudStorage : IDisposable
    {
        CloudStorageAccount storageAccount { get; set; }
        CloudBlobClient blobClient { get; set; }

        public CloudStorage(string ConnString)
        {
            storageAccount = CloudStorageAccount.Parse(ConnString);
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public void Validate()
        {
            blobClient.GetRootContainerReference();
        }

        public CloudBlobContainer Container(string Name)
        {
            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference(ToURLSlug(Name));

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return container;
        }

        public CloudBlobDirectory Container(CloudBlobContainer Parent, string Name)
        {
            // Retrieve a reference to a container.
            CloudBlobDirectory container = Parent.GetDirectoryReference(ToURLSlug(Name));

            return container;
        }

        public void Upload(CloudBlobContainer container, string Name, Stream fs)
        {
            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(ToURLSlug(Name));

            // Create or overwrite the "myblob" blob with contents from a local file. 
            blockBlob.UploadFromStream(fs);
        }

        public void Upload(CloudBlobContainer container, string Name, string Content)
        {
            using (MemoryStream ms = new MemoryStream(ASCIIEncoding.Default.GetBytes(Content)))
            {
                Upload(container, Name, ms);
            }
        }

        public void Upload(CloudBlobDirectory dir, string Name, Stream fs)
        {
            CloudBlockBlob bb = dir.GetBlockBlobReference(ToURLSlug(Name));
            bb.UploadFromStream(fs);
        }

        public void DownloadToStream(CloudBlobContainer container, string Name, Stream memoryStream)
        {
            CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(ToURLSlug(Name));
            blockBlob2.DownloadToStream(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
        }

        public void DownloadToStream(CloudBlobDirectory container, string Name, Stream memoryStream)
        {
            CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(ToURLSlug(Name));
            blockBlob2.DownloadToStream(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
        }

        public string DownloadString(CloudBlobContainer container, string Name)
        {
            CloudBlockBlob blockBlob2 = container.GetBlockBlobReference(ToURLSlug(Name));

            using (var memoryStream = new MemoryStream())
            {
                blockBlob2.DownloadToStream(memoryStream);
                var str = Encoding.ASCII.GetString(memoryStream.ToArray());
                return str;
            }
        }

        public void DownloadToFile(CloudBlobContainer container, string Name, string Path)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(ToURLSlug(Name));

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(Path))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }

        public void Delete(CloudBlobContainer container, string Name)
        {
            // Retrieve reference to a blob named "myblob.txt".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(ToURLSlug(Name));

            // Delete the blob.
            blockBlob.Delete();
        }

        public void Delete(CloudBlobContainer container, IListBlobItem item)
        {
            if (item.GetType() == typeof(CloudBlockBlob))
            {
                //CloudBlockBlob blob = (CloudBlockBlob)item;
                //Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri); 
                ((CloudBlockBlob)item).DeleteIfExists();
            }
            else if (item.GetType() == typeof(CloudPageBlob))
            {
                //CloudPageBlob pageBlob = (CloudPageBlob)item;

                //Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

                ((CloudPageBlob)item).DeleteIfExists();

            }
            else if (item.GetType() == typeof(CloudBlobDirectory))
            {
                //CloudBlobDirectory directory = (CloudBlobDirectory)item;
                throw new Exception("Cloud Blob directory could not delete");
                //Console.WriteLine("Directory: {0}", directory.Uri);
            }
        }

        public IEnumerable<IListBlobItem> ListBlob(CloudBlobContainer container)
        {
            return container.ListBlobs(null, false);
        }

        public IEnumerable<IListBlobItem> ListBlob(CloudBlobDirectory container)
        {
            return container.ListBlobs();
        }

        public IEnumerable<T> ListBlob<T>(CloudBlobContainer container)
        {
            return container.ListBlobs(null, false).Cast<T>();
        }

        public IEnumerable<T> ListBlob<T>(CloudBlobDirectory dir)
        {
            return dir.ListBlobs().Cast<T>();
        }

        public string ToURLSlug(string s)
        {
            return Regex.Replace(s, @"[^a-z0-9/.]+", "-", RegexOptions.IgnoreCase)
              .Trim(new char[] { '-' })
              .ToLower();
        }

        public void Dispose()
        {
            storageAccount = null;
            blobClient = null;

            GC.Collect();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<CloudBlobContainer> ListContainers()
        {
            return blobClient.ListContainers();
        }
    }
}
