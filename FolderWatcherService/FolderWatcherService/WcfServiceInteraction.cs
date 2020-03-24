using FolderWatcherService.Logger;
using FolderWatcherService.WcfEchoServiceReference;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderWatcherService
{
    class WcfServiceInteraction
    {
        ChequeServiceContractClient _client;
        int _lastChequeCount;

        public WcfServiceInteraction()
        {
            _lastChequeCount = GetChequesPackCount();
            _client = new ChequeServiceContractClient();
        }

        public void RunInteraction(string fileName, string filePath)
        {
            SendChequeToWcfServie(fileName, filePath);
            AddLastChequesFromWcfServiceToFile(_lastChequeCount); 
        }
        private void SendChequeToWcfServie(string fileName, string filePath)
        {

            if (ValidateFile(fileName))
            {
                try
                {
                    var deserializedCheque = InputFolderFileReader.ReadFile(filePath);
                    if (deserializedCheque != null)
                    {
                        _client.SaveCheque(deserializedCheque); 
                        InputFolderCleanerTest.FileToComplete(filePath, fileName);
                        MyLogger.Log.Info($"File {filePath} was sending to WcfService");
                    }
                    else { InputFolderCleanerTest.FileToGarbage(filePath, fileName); }
                }
                catch (Exception ex)
                {
                    InputFolderCleanerTest.FileToGarbage(filePath, fileName);
                    MyLogger.Log.Error($"Cant sending file {fileName} to WcfService  {ex}");
                }
            }
            else
            {
                InputFolderCleanerTest.FileToGarbage(filePath, fileName);
            }
        }
        private void AddLastChequesFromWcfServiceToFile(int packCount)
        {
            MyLogger.Log.Info($"Start added {packCount} last cheques from WcfService to file ");

            try
            {
                Cheque[] cheques = _client.GetChequesPack(packCount);
                if (cheques?.Length > 0)
                {
                    WriteCheques(cheques);
                }
                else
                {
                    MyLogger.Log.Info("WcfService not returned last cheques ");
                }

            }
            catch (Exception ex)
            {
                MyLogger.Log.Error($"Cant getting last cheques from wcf service {ex}");
            }
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
        private void WriteCheques(Cheque[] cheques)
        {
            string writePath;

            if (ConfigurationManager.AppSettings.Get("SavedChequesFolder") == "")
            {

                writePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"\SavedCheqes\LastCheqes.txt");
                MyLogger.Log.Info($"Path to folder SavedCheqes not set in App.config, path to SavedCheqes folder {writePath}");
            }
            else
            {
                writePath = Path.Combine(ConfigurationManager.AppSettings.Get("SavedChequesFolder"), @"LastCheques.txt");
            }

            foreach (var e in cheques)
            {
                try
                {
                    DirectoryCheckerTest.CreateDirectoryIfNotExist(ConfigurationManager.AppSettings.Get("SavedChequesFolder"));

                    using (StreamWriter streamWriter = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        StringBuilder articles = new StringBuilder();
                        foreach (var val in e.Articles) { articles.Append(val + ';'); }
                        streamWriter.WriteLine($"Id = {e.Id}, Number = {e.Number}, Simm = {e.Summ}, Descount = {e.Discount},  Articles = {articles}");

                        MyLogger.Log.Info($"Cheque added to file {writePath}");
                    }
                }
                catch (IOException ex) { MyLogger.Log.Error($"Cant write cheques collecttion, {ex}"); }
                catch (Exception ex) { MyLogger.Log.Error($"Cant write cheques collecttion, {ex}"); }

            }
            AddDelimetr(writePath);
            MyLogger.Log.Info($"Cheques pack added to file {writePath}");
        }
        private int GetChequesPackCount()
        {
            int chequePackCount;
            if (ConfigurationManager.AppSettings.Get("ChequesPackCount") == "")
            {
                chequePackCount = 4;
                MyLogger.Log.Info($"Cheques Pack count not set in App.config, LastChequesCount = {4}");
            }
            else
            {
                chequePackCount = Convert.ToInt32(ConfigurationManager.AppSettings.Get("ChequesPackCount"));
            }

            return chequePackCount;
        }
        private void AddDelimetr(string writePath) 
        {
            try { 
            
                using (StreamWriter streamWriter = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {

                    streamWriter.WriteLine($"Cheques added [{DateTime.Now}]");
                    streamWriter.WriteLine($"************************************************");

                    MyLogger.Log.Info($"Write delimetr after addedd objects to SaveCheques data file {writePath}");
                }
            }
            catch (IOException ex) { MyLogger.Log.Error($"Cant write delimetr, {ex}"); }
            catch (Exception ex) { MyLogger.Log.Error($"Cant write delimetr, {ex}"); }
        }
    }
}
