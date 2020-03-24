using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using FolderWatcherService.Logger;
using System.Diagnostics;


namespace FolderWatcherService
{
    class InputFolderCleanerTest
    {
        public static void FileToGarbage(string fileFullPath, string fileName)
        {
            MyLogger.Log.Info($"Sending file {fileName} to Grabage folder");
            string garbageFolderPath = "";

            if (ConfigurationManager.AppSettings.Get("GarbageFolder") == "")
            {
                garbageFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"\FolderWatcherService\Garbage");
                MyLogger.Log.Info($"Path to folder Garbage not set in App.config, path to Garbage folder {garbageFolderPath}");
            }
            else
            {
                garbageFolderPath = ConfigurationManager.AppSettings.Get("GarbageFolder");
            }

            DirectoryCheckerTest.CreateDirectoryIfNotExist(garbageFolderPath);

            MoveFile(fileFullPath, garbageFolderPath, fileName);

        }
        public static void FileToComplete(string fileFullPath, string fileName)
        {
            MyLogger.Log.Info($"Sending file {fileName} to Complete folder");

            string completeFolderPath = "";

            if (ConfigurationManager.AppSettings.Get("CompleteFolder") == "")
            {

                completeFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"\FolderWatcherService\Complete");
                MyLogger.Log.Info($"Path to folder Complete not set in App.config, path to Complete folder: {completeFolderPath}");
            }
            else
            {
                completeFolderPath = ConfigurationManager.AppSettings.Get("CompleteFolder");
            }

            DirectoryCheckerTest.CreateDirectoryIfNotExist(completeFolderPath);

            MoveFile(fileFullPath, completeFolderPath, fileName);

        }
        private static void MoveFile(string currentPath, string destenationFolder, string fileName)
        {
            DirectoryInfo diSource = new DirectoryInfo(currentPath);
            DirectoryInfo diDestenation = new DirectoryInfo((Path.Combine(destenationFolder, fileName)));

            if (CheckIsFile(diSource))
            {
                MyLogger.Log.Info($"Object {diSource.Name} is a File ");
                try
                {
                    CopyFile(currentPath, destenationFolder, fileName);
                    MyLogger.Log.Info($"File {currentPath} copy to {destenationFolder}");
                    File.Delete(currentPath);
                    MyLogger.Log.Info($"File {currentPath} deleted");
                }
                catch (Exception ex) { MyLogger.Log.Error($"File {currentPath} cant moved to {diDestenation.FullName} {ex}"); }
            }
            else
            {

                MyLogger.Log.Info($"Object {diSource.Name} is a Directory ");
                try
                {
                    CopyDirectory(diSource, diDestenation);
                    MyLogger.Log.Info($"Directory {currentPath} copy to {destenationFolder}");
                    Directory.Delete(currentPath, true);
                    MyLogger.Log.Info($"Directory {currentPath} deleted");
                }
                catch (Exception ex) { MyLogger.Log.Error($"Directory {diSource.FullName} cant moved to {diDestenation.FullName} {ex}"); }
            }

        }
        private static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {

            MyLogger.Log.Info($"Starting copy directory {source.FullName} to {target.FullName} ");

            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            int copyCount = 1;
            string createdDirectory = "";

            if (Directory.Exists(target.FullName) == false)
            {
                createdDirectory = target.FullName;
                Directory.CreateDirectory(createdDirectory);
            }
            else
            {
                while (Directory.Exists(Path.Combine(target.FullName + copyCount.ToString())) == true)
                {
                    copyCount++;
                }
                createdDirectory = (Path.Combine(target.FullName + copyCount.ToString()));
                Directory.CreateDirectory(createdDirectory);
            }

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(createdDirectory, fi.Name), true);
            }


            //рекурсия в случае воженных подпапок
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                              target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDirectory(diSourceSubDir, nextTargetSubDir);
            }
        }
        private static void CopyFile(string currentPath, string destenationPath, string fileName)
        {
            MyLogger.Log.Info($"Starting copy file {currentPath} to {Path.Combine(destenationPath, fileName)} ");

            if (currentPath.ToLower() == (Path.Combine(destenationPath, fileName)).ToLower())
            {
                return;
            }

            int copyCount = 1;

            if (File.Exists(Path.Combine(destenationPath, fileName)) == false)
            {
                File.Copy(currentPath, (Path.Combine(destenationPath, fileName)));
            }
            else
            {
                while (File.Exists(Path.Combine(destenationPath, (fileName.Replace(".", $"{copyCount}.")))) == true)
                {
                    copyCount++;
                }
                File.Copy(currentPath, (Path.Combine(destenationPath, (fileName.Replace(".", $"{copyCount}.")))));
            }
        }
        private static bool CheckIsFile(DirectoryInfo item)
        {
            MyLogger.Log.Info($"Cheking object {item.Name} is a file or a directory ");
            return File.Exists(item.FullName) ? true : false;
        }

    }
}
