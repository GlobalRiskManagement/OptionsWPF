using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Simple logging class 
    /// </summary>
    public class Logger
    {
        public void Log(String logMessage)
        {
            var directory = @"C:\Logs\";
            var filename = "NominationSwaps.txt";
            var path = directory + filename;
            if (!Directory.Exists(path)) Directory.CreateDirectory(directory);
            if (!File.Exists(path)) File.Create(path);
            try
            {
                File.AppendAllText(path, DateTime.Now + " " + logMessage + "\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Logger: " + e);
            }

#if DEBUG
            Console.WriteLine(logMessage);
#endif
        }


    }
}
