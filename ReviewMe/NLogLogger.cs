using Infrastructure;
using System;

namespace Scheduler
{
    public class NLogLogger : ILogger
    {
        private readonly NLog.ILogger _internaLogger;

        public NLogLogger(NLog.ILogger internaLogger)
        {
            _internaLogger = internaLogger;
        }

        public void Debug(string message)
        {
            _internaLogger.Debug(message);
        }

        public void Info(string message)
        {
            _internaLogger.Info(message);
        }

        public void Error(string message)
        {
            _internaLogger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _internaLogger.Error(exception, message);
        }

        public void Fatal(string message)
        {
            _internaLogger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            _internaLogger.Fatal(exception, message);
        }
    }
}