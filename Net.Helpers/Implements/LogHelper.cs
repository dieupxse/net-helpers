using System;
using System.IO;
using Net.Helpers.Interfaces;

namespace Net.Helpers.Implements
{
    public class LogHelper : ILogHelper
    {
        private readonly IStringHelper _stringHelper;
        public LogHelper() {
        }
        public LogHelper(IStringHelper stringHelper)
        {
            _stringHelper = stringHelper;
        }
        public void Log(string logMessage, string type = "info", string className="")
        {
            try
            {
                string strLogMessage = string.Empty;
                String fName = DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                StreamWriter swLog;

                strLogMessage = string.Format("[{0}]: {1}", DateTime.Now, logMessage);
                var logItem = (!string.IsNullOrEmpty(className) ? $"{className}/" : "");
                var originalDirectory = new DirectoryInfo($"wwwroot/{logItem}Logs/{type}/");
                string pathString = Path.Combine(originalDirectory.ToString(), DateTime.Now.ToString("yyyy-MM"));
                //var fileName1 = Path.GetFileName(fName);
                //create directory if does not exist
                bool isExists = Directory.Exists(pathString);
                if (!isExists) Directory.CreateDirectory(pathString);
                //generate local path to save file
                var path = $"{pathString}/{fName}";
                if (!File.Exists(path))
                {

                    FileStream fs = new FileStream(path, FileMode.CreateNew);
                    StreamWriter swr = new StreamWriter(fs);
                    try
                    {
                        swr.Close();
                        fs.Close();
                    }
                    catch (Exception )
                    {
                        swr.Close();
                        fs.Close();
                    }
                }

                if (!File.Exists(path))
                {
                    swLog = new StreamWriter(path);
                }
                else
                {
                    swLog = File.AppendText(path);
                }

                swLog.WriteLine(strLogMessage);
                swLog.WriteLine();
                swLog.Close();
            }
            catch (Exception)
            {

            }
        }
        public void Log(object logMessage, string type = "info", string className="")
        {
            Log(_stringHelper.SerializeCamelCase(logMessage), type, className);
        }
    }
}
