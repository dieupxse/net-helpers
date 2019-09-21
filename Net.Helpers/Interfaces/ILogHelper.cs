using System;

namespace Net.Helpers.Interfaces
{
    public interface ILogHelper
    {
        void Setup(string logPath);
        void Log(string logMessage, string type = "info", string className = "");
        void Log(object logMessage, string type = "info", string className = "");
    }
}
