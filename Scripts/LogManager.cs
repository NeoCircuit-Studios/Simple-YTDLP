using System;
using System.IO;

namespace APPLogManager
{
    public static class LogManager
    {
        private static readonly string logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NeoCircuit-Studios", "Simple-YTDLP", "logs", "Simple-YTDLP.log");

        static LogManager()
        {
            string? directoryPath = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static void LogToFile(string message, string logLevel = "INFO")
        {
            string logMessage = $"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: {logLevel}: {message}";

            // Write to Console (Real-time feedback)
            Console.WriteLine(logMessage);
            Console.Out.Flush(); // 🔹 Ensures console writes immediately

            // Write to log file with forced flush
            try
            {
                using (FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(logMessage);
                    writer.Flush(); // 🔹 Force immediate file write
                    fs.Flush(); // 🔹 Extra security
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to write log: {ex.Message}");
            }
        }

        public static void LogError(string message)
        {
            LogToFile(message, "ERROR");
        }

        internal static void Logtofile(string v)
        {
            throw new NotImplementedException();
        }
    }
}
