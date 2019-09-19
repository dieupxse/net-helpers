## Description
This lib contain a lot of Helper, Utilities Functions
Repository: [https://github.com/dieupxse/net-helpers](https://github.com/dieupxse/net-helpers)

###SecurityHelper
Security method, hash generate, hash verify, hash encrypt, encrypt algorithm
```
string EncryptEAS128(string toEncrypt, string key);
string Base64Encode(string plainText);
string Base64Encode(byte[] plainText);
string Base64Decode(string base64EncodedData);
string Base64Decode(byte[] base64EncodedData);
string DecryptEAS128(String text, String key);
string UppercaseFirst(string s);
string MD5(string strInput);
int GetMaxDataLength();
bool IsKeySizeValid();
void GenerateRSAKeys(out string Key, out string privateKey);
string EncryptRSA(string Key, string plainText);
string DecryptRSA(string privateKey, string encryptedText);
bool CheckSignRSA(string data, string sign, string Key);
string BCryptPasswordEncoder(string password);
bool BCryptPasswordVerifier(string password, string hash);
string EncryptTripleDES(string key, string data);
string DecryptTripleDES(string key, string data);
string EncryptKeyIv(string text, string _key, string _iv);
string DecryptKeyIv(string text, string _key, string _iv);
bool ValidateReCaptcha(string recaptchaResponse, string secretKey);
string SHA1Hash(string input);
string SHA1(string input);
string PasswordHash(string password);
string ComputeHash(string plainText,string hashAlgorithm,byte[] saltBytes);
bool VerifyHash(string plainText,string hashAlgorithm,string hashValue);
string Base64UrlEncode(byte[] input);
string Base64UrlEncode(string input);
byte[] Base64UrlDecode(string input);
```
###StringHelper
String Util
```
string Serialize(object obj, bool isFormating = false);
string SerializeCamelCase(object obj);
string GetPagination(int current, int pageSize, int rowPerPage, long total, string url);
int CountTotalPage(int total, int rowPerPage);
string HtmlEncode(string st);
string HtmlDecode(string st);
string StripHtml(string html);
string RemoveWhiteSpace(string text);
string RandomCode(int len, bool spechar, bool number);
string RandomOTP(int len);
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
```
###ApiHelper
This help to call RestApi inside Net Core App using RestSharp
```
Task<T> CallApi<T> (string url, RestRequest request = null);
```
###CacheHelper
CacheHelper help to Improve Application Performances  using Memory Caching, this helper contain those methods
```
T GetCache<T>(string key);
void SetCache(string key, object value, int exp = 30);
void ClearCache(string key);
void ClearAllCache(string prefix = "");
```
###EmailHelper
Send Email using SMTP server. This allow to send email in HTML format using any SMTP server like GMail, Yandex, Outlook, Custom SMTP Server...

```
void Init(string User, string Password, string Server, int Port, bool EnableSsl);
void Init(string User, string Password, string Server, int Port, bool EnableSsl, string SendFrom);
void Init(string User, string Password, string Server, int Port, bool EnableSsl, string SendFrom, bool EnableHtml);
void Init(string User, string Password, string Server, int Port, bool EnableSsl, string SendFrom, bool EnableHtml, string SendFromName);
bool SendMail(string to, string cc, string bcc, string subject, string content);
```

###ImageHelper
Scale, Crop, Convert Base64 to Image ....
```
Image<Rgba32> ResizeImage(Image<Rgba32> image, int width);
bool SaveResizeImage(Image<Rgba32> img, int width, string path);
bool SaveImage(Image<Rgba32> img, string path);
bool SaveCroppedImage(Image<Rgba32> image, int maxWidth, int maxHeight, string filePath, long compress = 100);
Image<Rgba32> Base64ToImage(string base64String);
string ImageToBase64(Image<Rgba32> image, IImageFormat format);
byte[] ConvertStreamToByteArray(Stream input);
Image<Rgba32> CropImage(Stream content, int x, int y, int width, int height);
Image<Rgba32> CropImage(byte[] content, int x, int y, int width, int height);
Image<Rgba32> CropImage(Image<Rgba32> image, int x, int y, int width, int height);
```

###LogHelper
Write Log to file
```
void Log(string logMessage, string type = "info", string className = "");
void Log(object logMessage, string type = "info", string className = "");
```
