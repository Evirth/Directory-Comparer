using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectoryComparer
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

        public void Copy(string SourcePath, string DestinationPath)
        {
            List<string> missDirs = new List<string>();

            for (int i = 0; i < MissingFiles.Count; i++)
            {
                if (MissingFiles[i].LastIndexOf('\\') > 0)
                {
                    missDirs.Add(MissingFiles[i].Remove(MissingFiles[i].LastIndexOf('\\')));
                }
            }

            //Now Create all of the directories
            List<string> dirName = Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < dirName.Count; i++)
            {
                if (dirName[i].LastIndexOf('\\') > 0)
                {
                    dirName[i] = dirName[i].Substring(SourcePath.Length + 1);
                }
            }

            foreach (string dir in dirName)
            {
                foreach (string item in missDirs)
                {
                    if (dir.Equals(item))
                    {
                        Directory.CreateDirectory(DestinationPath + '\\' + dir);
                    }
                }
            }

            //Copy all the files & Replaces any files with the same name
            List<string> files = Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories).ToList();
            
            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Substring(SourcePath.Length + 1);
            }
            files = files.Except(SourceFiles).ToList();

            foreach (string file in files)
            {
                File.Copy(SourcePath +'\\' + file, DestinationPath + '\\' + file, true);
            }
        }

        public static string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
