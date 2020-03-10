using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using MonitoringFolderSevice.Models;
using Newtonsoft.Json;
using System.Configuration;
using MonitoringFolderSevice.Logger;
using MonitoringFolderSevice.ServiceReference1;

namespace MonitoringFolderSevice
{
    public class FolderWatcher
    {
        
        ChequeServiceClient client = new ChequeServiceClient();
        FileSystemWatcher watcher;
        object obj = new object(); // для LOCK
        bool enabled = true;
        int lastChequeCount = 4;
        public FolderWatcher()
        {
            MyLogger.Log.Info("FolderWatcher initialization");

            //Путь к папке InputFolder задан явно в App.Config,  F:\\Temp  согласно заданию
            var inputFolderPath = ConfigurationManager.AppSettings.Get("InputFolder");
            watcher = new FileSystemWatcher(inputFolderPath);

            //  watcher.Filter = "*.txt"; -- Можно было сделать так, чтоб работать только с .txt

            MyLogger.Log.Info("Signed to event");
            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }


        // создание файлов
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            string fileName = e.Name;
            MyLogger.Log.Info($" файл {filePath} был {fileEvent}");


            
            if (ValidateFile(fileName))
            {
                
                try
                {
                    var deserializedCheque = JsonReader.ReadJsonFile(filePath);
                    client.SaveCheque(deserializedCheque);
                    FolderCleaner.FileToComplete(filePath, fileName);
                }
                catch
                {
                    FolderCleaner.FileToGarbage(filePath, fileName);
                    MyLogger.Log.Error($"Cant read file {fileName}");
                }

                var lastCheque = client.GetLastCheques(lastChequeCount);
                ShowLastCheques(lastCheque);
            }
            else
            {
                FolderCleaner.FileToGarbage(filePath, fileName);
            }



        }
        // переименование файлов
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            string fileName = e.Name;
            MyLogger.Log.Info($" файл {filePath} был {fileEvent}");


            if (ValidateFile(fileName))
            {

                try
                {
                    var deserializedCheque = JsonReader.ReadJsonFile(filePath);
                    client.SaveCheque(deserializedCheque);
                    FolderCleaner.FileToComplete(filePath, fileName);
                }
                catch
                {
                    FolderCleaner.FileToGarbage(filePath, fileName);
                    MyLogger.Log.Error($"Cant read file {fileName}");
                }

                var lastCheque = client.GetLastCheques(lastChequeCount);
                ShowLastCheques(lastCheque);
            }
            else
            {
                FolderCleaner.FileToGarbage(filePath, fileName);
            }

        }
        // изменение файлов
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            string fileName = e.Name;
            MyLogger.Log.Info($" файл {filePath} был {fileEvent}");

            if (ValidateFile(fileName))
            {

                try
                {
                    var deserializedCheque = JsonReader.ReadJsonFile(filePath);
                    client.SaveCheque(deserializedCheque);
                    FolderCleaner.FileToComplete(filePath, fileName);
                }
                catch
                {
                    
                    FolderCleaner.FileToGarbage(filePath, fileName);
                    MyLogger.Log.Error($"Cant read file {fileName}");
                }

                var lastCheque = client.GetLastCheques(lastChequeCount);
                ShowLastCheques(lastCheque);
            }
            else
            {
                FolderCleaner.FileToGarbage(filePath, fileName);
            }

        }
        // удаление файлов
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            MyLogger.Log.Info($" файл {filePath} был {fileEvent}");

        }

        private bool ValidateFile(string fileName)
        {
            MyLogger.Log.Info($"Start validate {fileName}");

            bool isValid;
            isValid = (fileName.Contains(".txt") && (fileName.Substring(fileName.Length - 4, 4)).Contains(".txt")) == true ? true : false;
            MyLogger.Log.Info($"File {fileName} valid state = {isValid}");
            return isValid;
        }

        private void ShowLastCheques(ServiceReference1.Cheque[] cheques)
        {
            foreach (var e in cheques)
            {
                Console.WriteLine($"{e.Id} {e.Number} {e.Summ} {e.Discount} {e.Articles[0]} {e.Articles[1]} {e.Articles[2]} {e.Articles[3]}");
            }
            MyLogger.Log.Info($"Show cheques");
            Console.ReadKey();
        }



    }
}
