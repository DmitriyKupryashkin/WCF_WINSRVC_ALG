using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderWatcherService.Logger;
using Newtonsoft.Json;

namespace FolderWatcherService
{
    static class InputFolderFileReader
    {
        public static WcfEchoServiceReference.Cheque ReadFile(string filePath)
        {
            MyLogger.Log.Info($"Start read file:{filePath}");

            string value;
            try
            {
                using (StreamReader reader = File.OpenText(filePath))
                {
                    value = reader.ReadToEnd();
                }

                if (value == "") { MyLogger.Log.Info($"File {filePath} is empty"); return null; }

                var deserializedFile = JsonConvert.DeserializeObject<WcfEchoServiceReference.Cheque>(value);
                return deserializedFile;
            }
            catch (Exception ex)
            {
                MyLogger.Log.Error($"File cant read: {ex} ");
                return null;
            }
        }
    }
}
