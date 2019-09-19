using System;

namespace Net.Helpers.Interfaces
{
    public interface IEmailHelper
    {
        void Init(string User, string Password, string Server, int Port, bool EnableSsl);
        void Init(string User, string Password, string Server, int Port, bool EnableSsl, string SendFrom);
        void Init(string User, string Password, string Server, int Port, bool EnableSsl, string SendFrom, bool EnableHtml);
        void Init(string User, string Password, string Server, int Port, bool EnableSsl, string SendFrom, bool EnableHtml, string SendFromName);
        bool SendMail(string to, string cc, string bcc, string subject, string content);
    }
}
