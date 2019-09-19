using System;

namespace Net.Helpers.Interfaces
{
    public interface ILogHelper
    {
        void Log(string logMessage, string type = "info", string className = "");
        void Log(object logMessage, string type = "info", string className = "");
    }
}
