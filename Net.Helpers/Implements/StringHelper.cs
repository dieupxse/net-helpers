using Net.Helpers.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Net.Helpers.Implements
{
    public class StringHelper : IStringHelper
    {

        public string Serialize(object obj, bool isFormating = false)
        {
            var JsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            if (isFormating)
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented, JsonSerializerSettings);
            }
            return JsonConvert.SerializeObject(obj, JsonSerializerSettings);
        }
        public  string SerializeCamelCase(object obj)
        {
            return JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }




        /// <summary>
        /// Get Pagination
        /// </summary>
        /// <param name="current"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowPerPage"></param>
        /// <param name="total"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetPagination(int current, int pageSize, int rowPerPage, long total, string url)
        {
            // TODO Auto-generated method stub
            String page = "";
            long totalPage;
            long start = 0;
            long pageNumber = 1;
            long i = 1;
            if (current == 0) current = 1;
            if (total % rowPerPage > 0)
            {
                totalPage = (total / rowPerPage) + 1;
            }
            else
            {
                totalPage = (total / rowPerPage);
            }
            page += "<ul class=\"pager pull-left hidden-xs\"><li  class=\"disabled\"><a>Từ " + ((current - 1) * rowPerPage + 1) + " đến " + ((current * rowPerPage) > total ? total : (current * rowPerPage)) + " trong tổng số " + total + "</a></li></ul>";
            if (totalPage > 1)
            {
                page += "<ul class=\"pagination pull-right\">";
                if (current <= totalPage)
                {
                    if (current == 1)
                    {
                        pageNumber = pageSize;
                        if (pageNumber > totalPage) pageNumber = totalPage;
                        start = 1;
                    }
                    else
                    {
                        page += "<li><a href=\"" + url + "\" title=\"Trang đầu\" id=\"" + 1 + "\">&laquo;&laquo;</a></li>";
                        page += "<li><a href=\"" + (url.Contains("?") ? (url + "&page=" + (current - 1)) : url + "?page=" + (current - 1)) + "\" id=\"" + (current - 1) + "\">&laquo;</a></li>";
                        if ((totalPage - current) < (pageSize / 2))
                        {
                            start = (totalPage - pageSize) + 1;
                            if (start <= 0) start = 1;
                            pageNumber = totalPage;
                        }
                        else
                        {
                            start = current - (pageSize / 2);
                            if (start <= 0) start = 1;
                            pageNumber = current + (pageSize / 2);
                            if (totalPage < pageNumber)
                            {
                                pageNumber = totalPage;
                            }
                            else if (pageNumber < pageSize)
                            {
                                pageNumber = pageSize;
                            }
                        }
                    }
                    i = start;
                    while (i <= pageNumber)
                    {
                        if (i == current)
                        {
                            page += "<li class=\"active\"><a href=\"\" id=\"" + i + "\">" + i + "</a></li>";
                        }
                        else
                        {
                            page += "<li><a href=\"" + (url.Contains("?") ? (url + "&page=" + i) : url + "?page=" + i) + "\" id=\"" + i + "\">" + i + "</a></li>";
                        }
                        i++;
                    }
                    if (current < totalPage)
                    {
                        page += "<li><a href=\"" + (url.Contains("?") ? (url + "&page=" + (current + 1)) : url + "?page=" + (current + 1)) + "\" id=\"" + (current + 1) + "\">&raquo;</a></li>";
                        page += "<li><a href=\"" + (url.Contains("?") ? (url + "&page=" + totalPage) : url + "?page=" + totalPage) + "\" title=\"Trang cuối\" id=\"" + (totalPage) + "\">&raquo;&raquo;</a></li>";
                    }
                }
                page += "</ul>";
                page += "<div class='clearfix'></div>";
            }
            return page;
        }
        //End pagination

        public int CountTotalPage(int total, int rowPerPage)
        {
            if (total % rowPerPage > 0)
            {
                return (total / rowPerPage) + 1;
            }
            else
            {
                return (total / rowPerPage);
            }
        }

        /// <summary>
        /// HTML Endcode
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public string HtmlEncode(string st)
        {
            return WebUtility.HtmlEncode(st);
        }

        /// <summary>
        /// HTML Decode
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public string HtmlDecode(string st)
        {
            return WebUtility.HtmlDecode(st);
        }

        /// <summary>
        /// Strip HTML Tag
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string StripHtml(string html)
        {
            html += "";
            html = Regex.Replace(html, "<.*?>", "");
            return html;
        }


        public string RemoveWhiteSpace(string text)
        {
            text = Regex.Replace(text, @"\s+", "");
            return text;
        }
        /// <summary>
        /// Random Code
        /// </summary>
        /// <param name="len"></param>
        /// <param name="spechar"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public string RandomCode(int len, bool spechar, bool number)
        {
            var alpha = "QWERTYUIOPLKJHGFDSAZXCVBNM";
            var num = "1234567890";
            var spec = "~!@#$%^&*()`<>,.?'\"\\}{][+_|";
            var str = alpha;
            var rs = "";
            if (spechar) str += spec;
            if (number) str += num;
            Random rd = new Random();
            for (int i = 1; i <= len; i++)
            {
                rs += str[rd.Next(0, str.Length - 1)];
            }
            return rs;
        }

        public string RandomOTP(int len)
        {
            var str = "1234567890";
            var rs = "";
            Random rd = new Random();
            for (int i = 1; i <= len; i++)
            {
                rs += str[rd.Next(0, str.Length - 1)];
            }
            return rs;
        }

        /// <summary>
        /// Validate Email
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public bool IsEmail(string inputEmail)
        {
            const string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            //Regular expression object
            var check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            //boolean variable to return to calling method
            bool valid = false;
            if (string.IsNullOrEmpty(inputEmail))
            {
                valid = false;
            }
            else
            {
                //use IsMatch to validate the address
                valid = check.IsMatch(inputEmail);
            }
            //return the value to the calling method
            return valid;
        }

        public bool IsUsername(string userName)
        {
            const string pattern = @"^[a-zA-Z0-9._@]+$";
            //Regular expression object
            var check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            //boolean variable to return to calling method
            bool valid = false;
            if (string.IsNullOrEmpty(userName))
            {
                valid = false;
            }
            else
            {
                //use IsMatch to validate the address
                valid = check.IsMatch(userName);
            }
            //return the value to the calling method
            return valid;
        }

        public string GetToken()
        {
            string loginToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return Regex.Replace(loginToken, @"[^0-9a-zA-Z]+", "");
        }

        public string GenerateNiceUrl(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            char[] ch = { '-' };
            temp = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            temp = Regex.Replace(temp, "[^0-9a-zA-Z-\\s]+", "");
            temp = string.Join("-", temp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            return Regex.Replace(temp, @"\-+", @"-").TrimStart(ch).TrimEnd(ch).ToLower();
        }

        public string StripVietnamese(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public string StripSpace(string s)
        {
            return string.Join("-", s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public  string StripSpecialchar(string txt)
        {
            var regex = "[^0-9a-zA-Z]+";
            return Regex.Replace(txt, regex, "");
        }
        public  string GetNewPathForDupes(string path, ref string fileName)
        {
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int counter = 1;
            string newFullPath = path;
            string new_file_name = GenerateNiceUrl(filename) + extension;
            while (File.Exists(newFullPath))
            {
                string newFilename = string.Format("{0}({1}){2}", new_file_name, counter, extension);
                new_file_name = newFilename;
                newFullPath = Path.Combine(directory, newFilename);
                counter++;
            };
            fileName = new_file_name;
            return string.Format("{0}/{1}", directory, new_file_name);
        }

        public string MinutesToHourMinute(int minutes)
        {
            var span = TimeSpan.FromMinutes(minutes);
            return span.ToString(@"hh\:mm");
        }

        private  readonly Random random = new Random();
        private  readonly object syncLock = new object();
        public  int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min, max);
            }

        }

        public  string GetRandomName()
        {
            var firstNames = new string[] { "Nguyễn", "Phạm", "Đoàn", "Chu", "Mai", "Lê", "Huỳnh", "Hoàng", "Cao", "Đặng", "Tô", "Đinh", "Mạc" };
            var middleNames = new string[] { "Thị", "Mỹ", "Khắc", "Quốc", "Huyền", "Hiền", "Bích", "Ngọc", "Khánh", "Gia", "Bảo", "Văn", "Đức", "Sỹ", "Mạnh", "Hoàng" };
            var lastnames = new string[] { "Hoa", "Huệ", "Liên", "Quỳnh", "Nam", "Nhật", "My", "Linh", "Yến", "Đào", "Mận", "Hùng", "Dũng", "Mạnh", "Đức", "Phúc", "Diệu", "Tùng", "Lâm", "Minh", "Mẫn", "Nhã", "Hân", "Huy", "Hiếu", "Chính", "Nghĩa", "Trang", "Trinh" };
            return $"{firstNames[GetRandomNumber(0, firstNames.Length - 1)]} {middleNames[GetRandomNumber(0, middleNames.Length - 1)]} {lastnames[GetRandomNumber(0, lastnames.Length - 1)]}";
        }
        public string GetRandomAvatar()
        {
            var avartar = new string[] {
                "https://ssl.gstatic.com/images/branding/product/1x/avatar_square_blue_512dp.png",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRiQCcwQmlUDuaYrIPizxr_VOie1HFYY44r2gTTRMEBTzjFY5zC",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSc-0NAT7MNCs6YbDrAVHnRXNHFXb1S61-zh5lbO_gmruE8xpcnqA",
                "https://ssl.gstatic.com/images/branding/product/1x/avatar_square_grey_512dp.png",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTbiUCBiDlaN0u2MxC4P2iBNBnbecs2xnUoJ_cyDLtktVpGqx-K",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQdVshDgm8kKqiqTsOY_5ektnidYZ-bjA6ORu2pb-TU1GMFditVhw",
                "https://image.shutterstock.com/z/stock-vector-jewish-man-character-flat-style-square-head-style-avatar-icon-vector-illustration-444277237.jpg",
                "https://thumb7.shutterstock.com/display_pic_with_logo/3070865/457022725/stock-vector-happy-afro-american-worker-engineer-flat-style-character-square-head-style-avatar-icon-vector-457022725.jpg"
            };
            return avartar[GetRandomNumber(0, avartar.Length - 1)];
        }


        public string GetRandomGender()
        {
            var genders = new string[] { "Nam", "Nữ", "Không rõ" };
            return genders[GetRandomNumber(0, genders.Length - 1)];
        }
        public List<T> GetList<T>(string data,char mark)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            var list = new List<T>();
            var t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                t = Nullable.GetUnderlyingType(t);
            }
            foreach (var item in data.Split(mark))
            {
                var datax = (T)Convert.ChangeType(item, t);
                list.Add(datax);
            }
            return list;
        }

    public  Tuple<DateTime, DateTime> GetDateRangeQuery(
      string start,
      string end)
        {
        start = start ?? "";
        end = end ?? "";
        DateTime dateTime = DateTime.Now.AddDays(-30.0);
        DateTime now = DateTime.Now;
        if (!string.IsNullOrEmpty(start))
            dateTime = DateTime.Parse(start);
        if (!string.IsNullOrEmpty(end))
            now = DateTime.Parse(end);
        return Tuple.Create<DateTime, DateTime>(dateTime, now);
        }
    }
}