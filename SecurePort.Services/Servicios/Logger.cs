using log4net;
using System;
using log4net.Config;

namespace SecurePort
{
    public class Logger : ILogger
    {
        private static readonly ILog mLog = LogManager.GetLogger(typeof(Logger));
        private const string MessageFormat = "{0} | {1}";
        private const int MaxCategoryNameLength = 25;

        static Logger()
        {
        }

        public Logger()
        {
            XmlConfigurator.Configure();
        }

        public void PublishException(Exception exception)
        {
            if (Logger.mLog == null)
                return;
            Logger.mLog.Error((object)"Exception", exception);
        }

        public void WriteVerbose(string category, string message)
        {
            if (Logger.mLog == null)
                return;
            Logger.mLog.Debug((object)Logger.FormatMessage(category, message));
        }

        public void WriteInfo(string category, string message)
        {
            if (Logger.mLog == null)
                return;
            Logger.mLog.Info((object)Logger.FormatMessage(category, message));
        }

        public void WriteWarning(string category, string message)
        {
            if (Logger.mLog == null)
                return;
            Logger.mLog.Warn((object)Logger.FormatMessage(category, message));
        }

        public void TraceError(string category, string message)
        {
            if (Logger.mLog == null)
                return;
            Logger.mLog.Error((object)Logger.FormatMessage(category, message));
        }

        private static string FormatMessage(string category, string message)
        {
            return string.Format("{0} | {1}", (object)Logger.FormatName(category, 25), (object)message);
        }

        private static string FormatName(string name, int minLength)
        {
            string str = name != null ? name.Trim() : string.Empty;
            return str.Length < minLength ? str.PadRight(minLength) : str;
        }
    }
}
