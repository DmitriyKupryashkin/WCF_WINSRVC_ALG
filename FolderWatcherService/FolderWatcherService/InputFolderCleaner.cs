using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using FolderWatcherService.Logger;


namespace FolderWatcherService
{
   public static class InputFolderCleaner
    {
        public static void FileToGarbage(string fileFullPath, string fileName)
        {

            string garbageFolderPath;

            if (ConfigurationManager.AppSettings.Get("GarbageFolder") == "")
            {
                garbageFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Garbage";
                MyLogger.Log.Info($"Path to folder Garbage not set in App.config, path to Garbage folder {garbageFolderPath}");
            }
            else
            {
                garbageFolderPath = ConfigurationManager.AppSettings.Get("GarbageFolder") + "\\Garbage";
            }

            if (!Directory.Exists(garbageFolderPath))
            {

                try
                {
                    Directory.CreateDirectory(garbageFolderPath);
                    MyLogger.Log.Info($"Directory {garbageFolderPath} created");
                }
                catch { MyLogger.Log.Error($"Directory {garbageFolderPath} not created"); }
            }

            MoveFile(fileFullPath, Path.Combine(garbageFolderPath, fileName));


        }
        public static void FileToComplete(string fileFullPath, string fileName)
        {

            string completeFolderPath;

            if (ConfigurationManager.AppSettings.Get("CompleteFolder") == "")
            {

                completeFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Complete";
                MyLogger.Log.Info($"Path to folder Complete not set in App.config, path to Complete folder {completeFolderPath}");
            }
            else
            {
                completeFolderPath = ConfigurationManager.AppSettings.Get("CompleteFolder") + "\\Complete";
            }

            if (!Directory.Exists(completeFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(completeFolderPath);
                    MyLogger.Log.Info($"Directory {completeFolderPath} created");
                }
                catch
                {
                    MyLogger.Log.Error($"Directory { completeFolderPath} cant created");
                }

                MoveFile(fileFullPath, Path.Combine(completeFolderPath, fileName));

            }

        }
        private static void MoveFile(string currentPath, string destenationPath)
        {

            File.Move(currentPath, destenationPath);
            MyLogger.Log.Info($"File {currentPath} moved to {destenationPath}");
        }
    }
}
