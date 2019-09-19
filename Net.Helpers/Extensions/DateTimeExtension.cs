using System;
using System.Globalization;

namespace Net.Helpers.Extensions {
    public static class DateTimeExtension {
        public static DateTime AbsoluteStart (this DateTime dateTime) {
            return dateTime.Date;
        }
        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd (this DateTime dateTime) {
            return AbsoluteStart (dateTime).AddDays (1).AddTicks (-1);
        }
        public static DateTime MidDate (this DateTime dateTime) {
            return new DateTime (dateTime.Year, dateTime.Month, dateTime.Day, 12, 0, 0);
        }
        public static DateTime ParseFormat (string value, string format) {

            var date = DateTime.Now;
            DateTime.TryParseExact (value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }
        public static DateTime ParseToDateFormat (this string value, string format) {

            return ParseFormat (value, format);
        }
        /// <summary>
        /// Convert UnixTime to DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime.ToLocalTime();
        }

        /// <summary>
        /// Convert Datetime To UnixTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double ToUnixTimestamp(DateTime dateTime)
        {
            dateTime = dateTime.ToLocalTime();
            return (long)Math.Round((dateTime - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime()).TotalSeconds, 0);
        }
    }
}