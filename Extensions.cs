using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Data;

namespace z.Controller
{
    public static class Extensions
    {
        [System.Diagnostics.DebuggerHidden]
        public static T TryParse<T>(this string strObj) where T : new()
        {
            try
            {
                return strObj.ToObject<T>();
            }
            catch
            {
                return default(T);
            }
        }

        public static string StripSlashes(this object str)
        {
            return str.ToString().Trim(new char[] { '\'', '\r', '\n', '\t' });
        }

        public static Pair ToPair(this IEnumerable<KeyValuePair<string, object>> keyObjects)
        {
            var p = new Pair();
            foreach (var g in keyObjects)
                p.Add(g.Key, g.Value);
            return p;
        }

        public static string CheckDir(this string dr)
        {
            var g = dr;
            if (Path.HasExtension(g))
            {
                g = Path.GetDirectoryName(dr);
            }
            if (!Directory.Exists(g)) Directory.CreateDirectory(g);
            return dr;
        }

        public static string DeleteFileIfExists(this string file)
        {
            if (File.Exists(file))
                File.Delete(file);

            return file;
        }
    }
}
