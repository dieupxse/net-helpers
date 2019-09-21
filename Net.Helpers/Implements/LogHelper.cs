using System;
using System.IO;
using Net.Helpers.Interfaces;

namespace Net.Helpers.Implements
{
    public class LogHelper : ILogHelper
    {
        private readonly IStringHelper _stringHelper;
        private string _logPath;
        public LogHelper() { }
        public LogHelper(IStringHelper stringHelper)
        {
            _stringHelper = stringHelper;
        }
        /// <summary>
        /// Write String long to file
        /// Current Login file is root off the application
        /// </summary>
        /// <param name="logMessage">Content</param>
        /// <param name="type">Can be any string Ex: error, info, success, warning ...</param>
        /// <param name="className">Filter by ClassName</param>
        public void Log(string logMessage, string type = "info", string className = "")
        {
            try
            {
                string strLogMessage = string.Empty;
                String fName = $"{(!string.IsNullOrEmpty(type) ? $"{type}." : "")}{(!string.IsNullOrEmpty(className) ? $"{className}." : "")}{DateTime.Now.ToString("yyyy_MM_dd") }.txt";
                StreamWriter swLog;

                strLogMessage = string.Format("[{0}]: {1}", DateTime.Now, logMessage);
                var logItem = (!string.IsNullOrEmpty(className) ? $"{className}/" : "");
                var originalDirectory = new DirectoryInfo(!string.IsNullOrEmpty(_logPath) ? $"{_logPath}" : $"wwwroot/Logs/");
                string pathString = Path.Combine(originalDirectory.ToString());
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
                    catch (Exception)
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
        /// <summary>
        /// Write Object to Log File
        /// </summary>
        /// <param name="logMessage">Object content</param>
        /// <param name="type">Can be any string Ex: error, info, success, warning ...</param>
        /// <param name="className">Filter by ClassName</param>
        public void Log(object logMessage, string type = "info", string className = "")
        {
            Log(_stringHelper.SerializeCamelCase(logMessage), type, className);
        }

        public void Setup(string logPath)
        {
            _logPath = logPath;
        }
    }
}