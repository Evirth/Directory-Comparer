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
        public List<FileInfo> SourceFiles { get; set; }
        public List<FileInfo> TargetFiles { get; set; }
        public List<FileInfo> OverFiles { get; set; }
        public List<FileInfo> MissingFiles { get; set; }

        public static List<FileInfo> LoadFiles(string pPath)
        {
            DirectoryInfo dir = new DirectoryInfo(pPath);
            List<FileInfo> files = dir.GetFiles().ToList();

            //for (int i = 0; i < files.Count; i++)
            //{
            //    files[i] = files[i].Substring(pPath.Length + 1);
            //}
            //files.Sort();
            return files;
        }

        public List<FileInfo> ShowOverFiles()
        {
            List<FileInfo> files = new List<FileInfo>();
            files = SourceFiles.Except(TargetFiles).ToList();
            //files.Sort();
            return files;
        }

        public List<FileInfo> ShowMissingFiles()
        {
            List<FileInfo> files = new List<FileInfo>();

            //foreach (var item in TargetFiles)
            //{
            //    if (item.Value.Name.Except(SourceFiles[item.i].Name,))
            //    {
            //        files.Add(item); 
            //    }
            //}
            //List<FileInfo> allItems = TargetFiles.Union(SourceFiles, new FileInfoComparer()).ToList();
            //List<FileInfo> commonItems = TargetFiles.Intersect(SourceFiles, new FileInfoEqualityComparer()).ToList();
            //List<FileInfo> difference = allItems.Except(commonItems, new FileInfoEqualityComparer()).ToList();

            //files = SourceFiles.Union(TargetFiles, new FileInfoEqualityComparer()).ToList();
            //var x = files.Distinct().ToList();
            //var y = 

            int[] it = new int[TargetFiles.Count];
            int[] js = new int[SourceFiles.Count];
            string[] t = new string[TargetFiles.Count];
            string[] s = new string[SourceFiles.Count];
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = TargetFiles[i].FullName;
            }
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = SourceFiles[i].FullName;
            }
            var asd = s.Except(t).ToArray();

            //files.Sort();
            return files;
        }

        //public void DirectoryCopy(string pSourceDirName, string pDestDirName, bool pCopySubDirs)
        //{
        //    // Get the subdirectories for the specified directory.
        //    DirectoryInfo dir = new DirectoryInfo(pSourceDirName);

        //    if (!dir.Exists)
        //    {
        //        throw new DirectoryNotFoundException(
        //            "Source directory does not exist or could not be found: "
        //            + pSourceDirName);
        //    }

        //    DirectoryInfo[] dirs = dir.GetDirectories();
        //    // If the destination directory doesn't exist, create it.
        //    if (!Directory.Exists(pDestDirName))
        //    {
        //        Directory.CreateDirectory(pDestDirName);
        //    }

        //    // Get the files in the directory and copy them to the new location.
        //    string[] tempMissings = new string[MissingFiles.Count];
        //    for (int i = 0; i < MissingFiles.Count; i++)
        //    {
        //        tempMissings[i] = MissingFiles[i];
        //        if (tempMissings[i].IndexOf('\\') > 0)
        //        {
        //            tempMissings[i] = ReverseString(tempMissings[i]);
        //            tempMissings[i] = tempMissings[i].Remove(tempMissings[i].IndexOf('\\'));
        //            tempMissings[i] = ReverseString(tempMissings[i]);
        //        }
        //    }
        //    FileInfo[] files = dir.GetFiles();
        //    //foreach (var file in tempMissings.Select((Value, i) => new { Value, i }))
        //    //{
        //    for (int i = 0; i < tempMissings.Length; i++)
        //    {
        //        File.Copy(MainWindow.TrgPth + '\\' + MissingFiles[i], pDestDirName + '\\' + tempMissings[i]); 
        //    }
        //        //string temppath = Path.Combine(pDestDirName, tempMissings[file.i]);
        //        //file.Value.CopyTo(temppath, false);
        //    //}

        //    // If copying subdirectories, copy them and their contents to new location.
        //    if (pCopySubDirs)
        //    {
        //        foreach (DirectoryInfo subdir in dirs)
        //        {
        //            string temppath = Path.Combine(pDestDirName, subdir.Name);
        //            DirectoryCopy(subdir.FullName, temppath, pCopySubDirs);
        //        }
        //    }
        //}

        private static void DirectoryCopy1(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy1(subdir.FullName, temppath, copySubDirs);
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

    public class FileInfoEqualityComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            return x.FullName.Equals(y.FullName);
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.FullName.GetHashCode();
        }
    }

    public class FileInfoComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            return x == null ? y == null : (x.Name.Equals(y.FullName, StringComparison.CurrentCultureIgnoreCase) && x.Length == y.Length);
        }

        public int GetHashCode(FileInfo obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
