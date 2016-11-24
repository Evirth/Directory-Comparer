using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DirectoryComparer
{
    public class FileListLoader
    {
        public List<string> SourceFiles { get; set; }
        public List<string> TargetFiles { get; set; }
        public List<string> OverFiles { get; set; }
        public List<string> MissingFiles { get; set; }
        public List<string> MissFilesFilter { get; set; }

        public static List<string> LoadFiles(string pPath)
        {
            List<string> files = Directory.GetFiles(pPath, "*.*", SearchOption.AllDirectories).ToList();

            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Substring(pPath.Length + 1);
            }
            files.Sort();
            return files;
        }

        public List<string> ShowOverFiles()
        {
            List<string> files = new List<string>();
            files = SourceFiles.Except(TargetFiles).ToList();
            files.Sort();
            return files;
        }

        public List<string> ShowMissingFiles()
        {
            List<string> files = new List<string>();
            files = TargetFiles.Except(SourceFiles).ToList();
            files.Sort();
            return files;
        }

        public void Copy(string pSourcePath, string pDestinationPath, string[] pMiss)
        {
            List<string> pMissings = pMiss.ToList();
            List<string> missDirs = new List<string>();

            for (int i = 0; i < pMissings.Count; i++)
            {
                if (pMissings[i].LastIndexOf('\\') > 0)
                {
                    missDirs.Add(pMissings[i].Remove(pMissings[i].LastIndexOf('\\')));
                }
            }

            //Now Create all of the directories
            List<string> dirName = Directory.GetDirectories(pSourcePath, "*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < dirName.Count; i++)
            {
                if (dirName[i].LastIndexOf('\\') > 0)
                {
                    dirName[i] = dirName[i].Substring(pSourcePath.Length + 1);
                }
            }

            foreach (string dir in dirName)
            {
                foreach (string item in missDirs)
                {
                    if (dir.Equals(item))
                    {
                        Directory.CreateDirectory(pDestinationPath + '\\' + dir);
                    }
                }
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string file in pMissings)
            {
                File.Copy(pSourcePath +'\\' + file, pDestinationPath + '\\' + file, true);
            }
        }
    }
}
