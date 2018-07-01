using System;
using System.Configuration;
using System.IO;

namespace SkillTracker.Common.Exception
{
    public class LogManager : ILogManager
    {
        public void WriteLog(System.Exception ex)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string path = ConfigurationManager.AppSettings["ErrorLogFilePath"].ToString();
            string fileName = ConfigurationManager.AppSettings["ErrorLogFileName"].ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                if (!File.Exists(fileName))
                    File.Create(fileName);
            }
            using (StreamWriter writer = new StreamWriter(string.Concat(path, fileName), true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }
}
