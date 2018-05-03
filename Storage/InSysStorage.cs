using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.Controller.Storage
{
    public class InSysStorage : IDisposable
    {
        public string RootPath { get; private set; }

        public InSysStorage(string path)
        {
            this.RootPath = path;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        public IEnumerable<T> ListBlob<T>(InSysContainer cntr)
        {
            if (typeof(T) == typeof(InSysBlob))
            {
                return Directory.GetFiles(Path.Combine(RootPath, cntr.Name)).Select(x => new InSysBlob(cntr, Path.GetFileName(x))).Cast<T>();
            }
            else if (typeof(T) == typeof(InSysDirectory))
            {
                return Directory.GetDirectories(Path.Combine(RootPath, cntr.Name)).Select(x => new InSysDirectory(cntr, Path.GetDirectoryName(x))).Cast<T>();
            }
            else
                return null;
        }

        public IEnumerable<InSysBlob> ListBlob(InSysContainer cntr)
        {
            var gh = Path.Combine(RootPath, cntr.Name);
            return Directory.GetFiles(gh).Select(x => new InSysBlob(cntr, Path.GetFileName(x)));
        }

        public IEnumerable<InSysBlob> ListBlob(InSysDirectory Dr)
        {
            var gh = Path.Combine(RootPath, Dr.inSysContainer.Name, Dr.Name);
            return Directory.GetFiles(gh).Select(x => new InSysBlob(Dr, Path.GetFileName(x)));
        }

        public void Delete(InSysContainer cntr, string str)
        {
            var gf = Path.Combine(RootPath, cntr.Name, str);
            if (File.Exists(gf))
                File.Delete(gf);
        }

        public string DownloadString(InSysContainer cntr, string str)
        {
            var gf = Path.Combine(RootPath, cntr.Name, str);
            using (var fs = File.OpenRead(gf))
            {
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    return Encoding.ASCII.GetString(ms.ToArray());
                }
            }
        }

        public void DownloadToStream(InSysContainer cntr, string name, Stream target)
        {
            var gf = Path.Combine(RootPath, cntr.Name, name);
            using (var fs = File.OpenRead(gf))
            {
                fs.CopyTo(target);
            }
        }

        public void Upload(InSysContainer cntr, string filename, Stream fp)
        {
            var gf = Path.Combine(RootPath, cntr.Name, filename).CheckDir().DeleteFileIfExists();
             
            using (var fs = File.OpenWrite(gf))
            {
                fp.Seek(0, SeekOrigin.Begin);
                fp.CopyTo(fs);
                fs.Flush();
            }

        }

        public void Upload(InSysContainer cntr, string filename, string base64)
        {
            using (MemoryStream ms = new MemoryStream(ASCIIEncoding.Default.GetBytes(base64)))
            {
                var gg = Path.Combine(RootPath, cntr.Name, filename).CheckDir().DeleteFileIfExists();
                using (var fs = File.OpenWrite(gg))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fs);
                    fs.Flush();
                }
            }
        }

        public void DownloadToFile(InSysContainer cntr, string filename, string fg)
        {
            var gf = Path.Combine(RootPath, cntr.Name, filename);
            using (var fs = File.OpenRead(gf))
            {
                using (var fd = File.OpenWrite(fg))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.CopyTo(fd);
                    fd.Flush();
                }
            }
        }

    }
}
