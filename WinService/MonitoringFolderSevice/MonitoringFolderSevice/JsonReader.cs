using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringFolderSevice
{
   static class JsonReader
    {
        public static ServiceReference1.Cheque ReadJsonFile(string filePath)
        {
            String json;
            using (StreamReader reader = File.OpenText(filePath))
            {
                
                json = reader.ReadToEnd();
            }
 
            var deserializedFile = JsonConvert.DeserializeObject<ServiceReference1.Cheque>(json);
            return deserializedFile;
        }
    }
}
