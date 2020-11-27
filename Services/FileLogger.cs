using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace KisaRisaMusicCore.Services
{
    public class FileLogger: ILogger
    {
        private string filePath;
        private static string fileErrorPath = Path.Combine(Directory.GetCurrentDirectory(), "loggerError.txt");
        private static object _lock = new object();
        public FileLogger(string path)
        {
            filePath = path;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
 
        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }
 
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                    if (logLevel >= LogLevel.Error)
                    {
                        File.AppendAllText(fileErrorPath, formatter(state, exception) + Environment.NewLine);
                    }
                }
            }
        }
    }
}