using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;

namespace WcfDbEchoLib.Logger
{
    class MyLogger
    {
        private static ILog log = LogManager.GetLogger("MyLogger");

        
        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        { 
            GlobalContext.Properties["LogPath"] = GetLoggerPath(); 
            XmlConfigurator.Configure();
            MyLogger.Log.Info($"LoggerIninting");
        }
        private static string  GetLoggerPath()
        {
            string loggerPath = "";

            if (ConfigurationManager.AppSettings.Get("WcfLogFolder") == "")
            {

                loggerPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"\WcfEchoService\Logs");
            }
            else
            {
                loggerPath = ConfigurationManager.AppSettings.Get("WcfLogFolder");
            }
            return loggerPath;
        }

    }
}
