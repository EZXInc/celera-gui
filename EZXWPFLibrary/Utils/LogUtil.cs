using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace EZXWPFLibrary.Utils
{
    public class LogUtil
    {
        private static ILog logger = LogManager.GetLogger(typeof(LogUtil));

        static LogUtil()
        {
            XmlConfigurator.Configure();
        }

        public static void WriteLog(LogLevel logLevel, string log)
        {
            if (logLevel.Equals(LogLevel.DEBUG))
            {
                logger.Debug(log);
            }
            else if (logLevel.Equals(LogLevel.ERROR))
            {
                logger.Error(log);
            }
            else if (logLevel.Equals(LogLevel.FATAL))
            {
                logger.Fatal(log);
            }
            else if (logLevel.Equals(LogLevel.INFO))
            {
                logger.Info(log);
            }
            else if (logLevel.Equals(LogLevel.WARN))
            {
                logger.Warn(log);
            }

        }

    }

    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL,
    }

}
