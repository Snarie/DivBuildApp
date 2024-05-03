using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal static class Logger
    {
        private static readonly string LogFilePath = "log.txt";
        private static readonly SemaphoreSlim LogSemaphore = new SemaphoreSlim(1);

        // Log methods
        public static async Task LogDebug(string message)
        {
            await LogAsync("DEBUG", message);
        }

        public static async Task LogInfo(string message)
        {
            await LogAsync("INFO", message);
        }

        public static async Task LogWarning(string message)
        {
            await LogAsync("WARNING", message);
        }

        public static async Task LogError(string message)
        {
            await LogAsync("ERROR", message);
        }

        public static async Task LogEvent(string message)
        {
            await LogAsync("EVENT", message);
        }

        // Asynchronous logging
        private static async Task LogAsync(string logLevel, string message)
        {
            await LogSemaphore.WaitAsync();
            try
            {
                string logEntry = $"{DateTime.Now} [{logLevel}] {GetCallingMethodInfo()} - {message}";
                //Console.WriteLine(logEntry); // Print to console for debugging

                // Append the log entry to the log file asynchronously
                try
                {
                    using (StreamWriter writer = File.AppendText(LogFilePath))
                    {
                        await writer.WriteLineAsync(logEntry);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write to log file: {ex.Message}");
                }
            }
            finally
            {
                LogSemaphore.Release();
            }
        }

        // Get calling method information
        private static string GetCallingMethodInfo()
        {
            // Get the stack trace of the calling method
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();

            // Find the first stack frame that is not in the Logger class
            foreach (StackFrame frame in stackFrames)
            {
                MethodBase method = frame.GetMethod();
                if (method.DeclaringType != typeof(Logger))
                {
                    return $"{method.DeclaringType.Name}.{method.Name}() Line {frame.GetFileLineNumber()}";
                }
            }

            return "Unknown";
        }
    }
}
