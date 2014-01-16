using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Utils;

namespace QStrategyGUILib
{
    public class QStrategyException : ApplicationException
    {
        private ExceptionType exceptionType;
        private string sourceLocation;

        public ExceptionType ExceptionType
        {
            get { return exceptionType; }
            set { exceptionType = value; }
        }

        public string SourceLocation
        {
            get { return sourceLocation; }
            set { sourceLocation = value; }
        }

        public string StackTarceInfo
        {
            get
            {
                if (this.InnerException != null)
                {
                    return this.InnerException.StackTrace;
                }
                return string.Empty;
            }
        }


        public QStrategyException()
        {
        }

        public QStrategyException(string message, ExceptionType exceptionType, string sourceLocation)
            : this(message, null, exceptionType, sourceLocation)
        {
        }


        public QStrategyException(string message, Exception innerException, ExceptionType exceptionType, string sourceLocation)
            : base(message, innerException)
        {
            this.exceptionType = exceptionType;
            this.sourceLocation = sourceLocation;
            string messageDetail = message;
            message = exceptionType.ToString();
            if (innerException != null)
            {
                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }
                message = innerException.Message;
                messageDetail = innerException.StackTrace;
            }

            string messageLog = string.Format("ExceptionType: {0}, Source:{1}, Exception:{2} MessageDetails: {3}", ExceptionType.ToString(), sourceLocation, message, messageDetail);
            LogUtil.WriteLog(LogLevel.ERROR, messageLog);

        }
}
    public enum ExceptionType
    {
        ConnectionException,
        ServerConnectionException,
        ReceiveSymbolUpdateException,
        ProcessSymbolException,
        ProcessAllSymbolException,
        DeserializationException,
        QueryConfigException,
        StrategyName,
    }

}
