using System;
using System.Net.Mail;
using Net.Helpers.Interfaces;

namespace Net.Helpers.Implements {
    public class EmailHelper : IEmailHelper {
        private string _user;
        private string _password;
        private string _server;
        private int _port;
        private bool _enableSsl;
        private string _sendFrom;
        private bool _enableHtml;
        private string _sendFromName;
        private bool _isInit = false;
        private readonly ILogHelper _logHelper;
        public EmailHelper (ILogHelper logHelper) {
            _logHelper = logHelper;
        }
        /// <summary>
        /// Init Email Setting
        /// </summary>
        /// <param name="user">SMTP user</param>
        /// <param name="password">SMTP Password</param>
        /// <param name="server">SMTP User</param>
        /// <param name="port">SMTP Port</param>
        /// <param name="enableSsl">SMTP Enable SSL</param>
        public void Init (string user, string password, string server, int port, bool enableSsl) {
            this._user = user;
            this._password = password;
            this._server = server;
            this._port = port;
            this._enableSsl = enableSsl;
            this._isInit = true;
        }
        /// <summary>
        /// Init Email Setting
        /// </summary>
        /// <param name="user">SMTP user</param>
        /// <param name="password">SMTP Password</param>
        /// <param name="server">SMTP User</param>
        /// <param name="port">SMTP Port</param>
        /// <param name="enableSsl">SMTP Enable SSL</param>
        /// <param name="sendFrom">SMTP SendFrom Email</param>
        public void Init (string user, string password, string server, int port, bool enableSsl, string sendFrom) {
            this.Init (user, password, server, port, enableSsl);
            this._sendFrom = sendFrom;
        }
        /// <summary>
        /// Init Email Setting
        /// </summary>
        /// <param name="user">SMTP user</param>
        /// <param name="password">SMTP Password</param>
        /// <param name="server">SMTP User</param>
        /// <param name="port">SMTP Port</param>
        /// <param name="enableSsl">SMTP Enable SSL</param>
        /// <param name="sendFrom">SMTP SendFrom name</param>
        /// <param name="enableHtml">Enable HTML support</param>
        public void Init (string user, string password, string server, int port, bool enableSsl, string sendFrom, bool enableHtml) {
            this.Init (user, password, server, port, enableSsl, sendFrom);
            this._enableHtml = enableHtml;
        }
        /// <summary>
        /// Init Email Setting
        /// </summary>
        /// <param name="user">SMTP user</param>
        /// <param name="password">SMTP Password</param>
        /// <param name="server">SMTP User</param>
        /// <param name="port">SMTP Port</param>
        /// <param name="enableSsl">SMTP Enable SSL</param>
        /// <param name="sendFrom">SMTP SendFrom name</param>
        /// <param name="enableHtml">Enable HTML support</param>
        /// <param name="sendFromName">SMTP SendFrom Name</param>
        public void Init (string user, string password, string server, int port, bool enableSsl, string sendFrom, bool enableHtml, string sendFromName) {
            this.Init (user, password, server, port, enableSsl, sendFrom, enableHtml);
            this._sendFromName = sendFromName;
        }
        /// <summary>
        /// Send An Email
        /// </summary>
        /// <param name="to">Email receive, can add multiple emails using comma, or semi-coin seprate (, ;)</param>
        /// <param name="cc">Email CC, can add multiple emails using comma, or semi-coin seprate (, ;)</param>
        /// <param name="bcc">Email BCC, can add multiple emails using comma, or semi-coin seprate (, ;)</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="content">Email content</param>
        /// <returns></returns>
        public bool SendMail (string to, string cc, string bcc, string subject, string content) {
            if (!this._isInit) {
                throw new Exception ("Please init setting before sending email.");
            }
            SmtpClient SmtpServer = new SmtpClient ();
            SmtpServer.Credentials = new System.Net.NetworkCredential (this._user, this._password);
            SmtpServer.Port = this._port;
            SmtpServer.Host = this._server;
            SmtpServer.EnableSsl = this._enableSsl;

            MailMessage mail = new MailMessage ();
            content = content.Replace (Environment.NewLine, "<br/>");
            try {
                mail.From = new MailAddress (this._sendFrom, this._sendFromName, System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty (to)) {
                    to = to.Replace (',', ';');
                    string[] emails = to.Split (';');
                    foreach (string e in emails) {
                        mail.To.Add (e);
                    }
                }

                if (!string.IsNullOrEmpty (cc)) {
                    string[] ccs = cc.Split (';');
                    foreach (string c in ccs) {
                        mail.CC.Add (c);
                    }
                }

                if (!string.IsNullOrEmpty (bcc)) {
                    string[] bccs = bcc.Split (';');
                    foreach (string c in bccs) {
                        mail.Bcc.Add (c);
                    }
                }

                mail.Subject = subject;
                mail.IsBodyHtml = this._enableHtml;
                mail.Body = content;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                SmtpServer.Send (mail);
                return true;
            } catch (Exception e) {
                _logHelper.Log (e.Message, "error", "EmailHelper");
                return false;
            }
        }
    }
}