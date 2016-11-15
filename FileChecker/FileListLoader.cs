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

        public void DirectoryCopy(string pSourceDirName, string pDestDirName, bool pCopySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(pSourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + pSourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(pDestDirName))
            {
                Directory.CreateDirectory(pDestDirName);
            }

            // Get the files in the directory and copy them to the new location.
            string[] tempMissings = new string[MissingFiles.Count];
            for (int i = 0; i < MissingFiles.Count; i++)
            {
                tempMissings[i] = MissingFiles[i];
                if (tempMissings[i].IndexOf('\\') > 0)
                {
                    tempMissings[i] = ReverseString(tempMissings[i]);
                    tempMissings[i] = tempMissings[i].Remove(tempMissings[i].IndexOf('\\'));
                    tempMissings[i] = ReverseString(tempMissings[i]);
                }
            }
            FileInfo[] files = dir.GetFiles();
            //foreach (var file in tempMissings.Select((Value, i) => new { Value, i }))
            //{
            for (int i = 0; i < tempMissings.Length; i++)
            {
                File.Copy(MainWindow.TrgPth + '\\' + MissingFiles[i], pDestDirName + '\\' + tempMissings[i]); 
            }
                //string temppath = Path.Combine(pDestDirName, tempMissings[file.i]);
                //file.Value.CopyTo(temppath, false);
            //}

            // If copying subdirectories, copy them and their contents to new location.
            if (pCopySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(pDestDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, pCopySubDirs);
                }
            }
        }

        public void Copy(string SourcePath, string DestinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
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
