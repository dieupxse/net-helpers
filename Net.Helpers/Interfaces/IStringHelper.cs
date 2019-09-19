using System;
using System.Collections.Generic;

namespace Net.Helpers.Interfaces
{
    public interface IStringHelper
    {
        string Serialize(object obj, bool isFormating = false);
        string SerializeCamelCase(object obj);




        /// <summary>
        /// Get Pagination
        /// </summary>
        /// <param name="current"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="total"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetPagination(int current, int pageSize, int rowPerPage, long total, string url);
        //End pagination

        int CountTotalPage(int total, int rowPerPage);

        /// <summary>
        /// HTML Endcode
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        string HtmlEncode(string st);

        /// <summary>
        /// HTML Decode
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
         string HtmlDecode(string st);

        /// <summary>
        /// Strip HTML Tag
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
         string StripHtml(string html);


         string RemoveWhiteSpace(string text);
        /// <summary>
        /// Random Code
        /// </summary>
        /// <param name="len"></param>
        /// <param name="spechar"></param>
        /// <param name="number"></param>
        /// <returns></returns>
         string RandomCode(int len, bool spechar, bool number);

        string RandomOTP(int len);

        /// <summary>
        /// Validate Email
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
         bool IsEmail(string inputEmail);

         bool IsUsername(string userName);

         string GetToken();

         string GenerateNiceUrl(string s);

         string StripVietnamese(string s);

         string StripSpace(string s);
          string StripSpecialchar(string txt);
        string GetNewPathForDupes(string path, ref string fileName);

         string MinutesToHourMinute(int minutes);

          int GetRandomNumber(int min, int max);

          string GetRandomName();
         string GetRandomAvatar();


         string GetRandomGender();
         List<T> GetList<T>(string data,char mark);
         Tuple<DateTime, DateTime> GetDateRangeQuery(string start,string end);
    }
}
