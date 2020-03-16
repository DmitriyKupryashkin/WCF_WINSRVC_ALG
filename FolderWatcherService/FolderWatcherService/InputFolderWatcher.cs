using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FolderWatcherService.Logger;
using FolderWatcherService.WcfEchoServiceReference;

namespace FolderWatcherService
{
    public class InputFolderWatcher
    {
       
        ChequeServiceContractClient _client = new ChequeServiceContractClient();
        FileSystemWatcher _watcher;
        bool _enabled = true;
        int lastChequeCount = 4; 

        public InputFolderWatcher()
        {

            //Путь к папке InputFolder задан явно в App.Config,  F:\\Temp  согласно заданию
            var inputFolderPath = ConfigurationManager.AppSettings.Get("InputFolder");
            _watcher = new FileSystemWatcher(inputFolderPath);

            //  _watcher.Filter = "*.txt"; -- Можно было сделать так, чтоб работать только с .txt

            MyLogger.Log.Info("Signed to event");
            _watcher.Deleted += Watcher_Deleted;
            _watcher.Created += Watcher_Created;
            _watcher.Changed += Watcher_Changed;
            _watcher.Renamed += Watcher_Renamed;
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;

            while (_enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _enabled = false;
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
                    var deserializedCheque = InputFolderFileReader.ReadFile(filePath);
                    _client.SaveCheque(deserializedCheque);
                    InputFolderCleaner.FileToComplete(filePath, fileName);
                }
                catch
                {
                    InputFolderCleaner.FileToGarbage(filePath, fileName);
                    MyLogger.Log.Error($"Cant read file {fileName}");
                }

                var lastCheque = _client.GetLastCheques(lastChequeCount);
                WriteLastCheques(lastCheque);
            }
            else
            {
                InputFolderCleaner.FileToGarbage(filePath, fileName);
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
                    var deserializedCheque = InputFolderFileReader.ReadFile(filePath);
                    _client.SaveCheque(deserializedCheque);
                    InputFolderCleaner.FileToComplete(filePath, fileName);
                }
                catch
                {
                    InputFolderCleaner.FileToGarbage(filePath, fileName);
                    MyLogger.Log.Error($"Cant read file {fileName}");
                }

                var lastCheque = _client.GetLastCheques(lastChequeCount);
                WriteLastCheques(lastCheque);
            }
            else
            {
                InputFolderCleaner.FileToGarbage(filePath, fileName);
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
                    var deserializedCheque = InputFolderFileReader.ReadFile(filePath);
                    _client.SaveCheque(deserializedCheque);
                    InputFolderCleaner.FileToComplete(filePath, fileName);
                }
                catch
                {
                    
                    InputFolderCleaner.FileToGarbage(filePath, fileName);
                    MyLogger.Log.Error($"Cant read file {fileName}");
                }

                var lastCheque = _client.GetLastCheques(lastChequeCount);
                WriteLastCheques(lastCheque);
            }
            else
            {
                InputFolderCleaner.FileToGarbage(filePath, fileName);
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


        
        /// <summary>
        /// записывает последние чеки в файл. Сделал для наглядности
        /// </summary>
        /// <param name="cheques"></param>
        private void WriteLastCheques(Cheque[] cheques)
        {
            string writePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LastCheqes.txt";

            foreach (var e in cheques)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        StringBuilder articles = new StringBuilder();
                        foreach (var val in e.Articles) { articles.Append(val + ';'); }
                        streamWriter.WriteLine($"Id = {e.Id}, Number = {e.Number}, Simm = {e.Summ}, Descount = {e.Discount}, Articles = {e.Articles[0]}, Articles = {articles}");
                        MyLogger.Log.Info($"Cheqe added to file {writePath}");
                    }
                }
                catch (IOException ex) {
                    MyLogger.Log.Error($"Cant write cheques collecttion, IOException {ex}");
                    throw; }
                catch (Exception ex) {
                    MyLogger.Log.Error($"Cant write cheques collecttion, Exception {ex}");
                    throw; }

            }
            MyLogger.Log.Info($"LastCheqes added to file {writePath}");
            Console.ReadKey();
        }

    }
}
