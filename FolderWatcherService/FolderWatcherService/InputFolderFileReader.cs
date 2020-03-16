using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FolderWatcherService
{
   static class InputFolderFileReader
    {
            public static WcfEchoServiceReference.Cheque ReadFile(string filePath)
            {
               
                string value;
                using (StreamReader reader = File.OpenText(filePath))
                {
                    value = reader.ReadToEnd();
                }

                var deserializedFile = JsonConvert.DeserializeObject<WcfEchoServiceReference.Cheque>(value);
                return deserializedFile;
            }
    }
}
