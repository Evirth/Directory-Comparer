using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileChecker
{
    public class FileListLoader
    {
        public List<string> SourceFiles { get; set; }
        public List<string> TargetFiles { get; set; }
        public List<string> OverFiles { get; set; }
        public List<string> MissingFiles { get; set; }

        public static List<string> LoadFiles(string pPath)
        {
            List<string> files = Directory.GetFiles(pPath, "*.*", SearchOption.AllDirectories).ToList();

            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Substring(pPath.Length + 1);
            }
                

            return files;
        }
    }
}
