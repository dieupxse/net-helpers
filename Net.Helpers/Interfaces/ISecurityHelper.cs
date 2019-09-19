using System;

namespace Net.Helpers.Interfaces
{
    public interface ISecurityHelper
    {

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
    }
}
