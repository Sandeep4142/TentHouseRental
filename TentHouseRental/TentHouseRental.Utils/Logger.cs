using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentHouseRental.Utils
{
    public class Logger : ILogger
    {
        private readonly string logDirectory;
        public Logger(string logDirectory)
        {
            this.logDirectory = logDirectory;
        }

        public void WriteLog(Exception ex)
        {
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + "_ErrorLog.txt";
            string logPath = Path.Combine(logDirectory, filename);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine($"{DateTime.Now} | {ex.Message}");
                if (ex.InnerException != null)
                {
                    writer.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                writer.WriteLine();
            }
        }

    }
}
