using Net.Helpers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Net.Helpers.Implements
{
    public class SecurityHelper : ISecurityHelper
    {
        /// <summary>
        /// Encrypt EAS128
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public  string EncryptEAS128(string toEncrypt, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key); // 256-AES key
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7; // better lang support
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Decrypt EAS128
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public  string DecryptEAS128(String text, String key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key); // AES-256 key
            byte[] toEncryptArray = Convert.FromBase64String(text);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7; // better lang support
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        public  string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        /// <summary>
        /// MD5 Encrypt
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public  string MD5(string strInput)
        {
            var algorithmType = default(HashAlgorithm);
            var enCoder = new ASCIIEncoding();
            byte[] valueByteArr = enCoder.GetBytes(strInput);
            byte[] hashArray = null;

            // Encrypt Input string 
            algorithmType = new MD5CryptoServiceProvider();
            hashArray = algorithmType.ComputeHash(valueByteArr);

            //Convert byte hash to HEX
            var sb = new StringBuilder();
            foreach (byte b in hashArray)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        #region Private Fields

        private const int KEY_SIZE = 2048; // The size of the RSA key to use in bits.
        private static bool fOAEP = false;
        private  RSACryptoServiceProvider rsaProvider = null;

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes a new instance of the System.Security.Cryptography.CspParameters class.
        /// </summary>
        /// <returns>An instance of the System.Security.Cryptography.CspParameters class.</returns>
        private  CspParameters GetCspParameters()
        {
            // Create a new key pair on target CSP
            CspParameters cspParams = new CspParameters();
            cspParams.ProviderType = 1; // PROV_RSA_FULL 
            // cspParams.ProviderName; // CSP name
            // cspParams.Flags = CspProviderFlags.UseArchivableKey;
            cspParams.KeyNumber = (int)KeyNumber.Exchange;

            return cspParams;
        }

        /// <summary>  
        /// Gets the maximum data length for a given key  
        /// </summary>         
        /// <param name="keySize">The RSA key length  
        /// <returns>The maximum allowable data length</returns>  
        public  int GetMaxDataLength()
        {
            if (fOAEP)
                return ((KEY_SIZE - 384) / 8) + 7;
            return ((KEY_SIZE - 384) / 8) + 37;
        }

        /// <summary>  
        /// Checks if the given key size if valid  
        /// </summary>         
        /// <param name="keySize">The RSA key length  
        /// <returns>True if valid; false otherwise</returns>  
        public  bool IsKeySizeValid()
        {
            return KEY_SIZE >= 384 &&
                   KEY_SIZE <= 16384 &&
                   KEY_SIZE % 8 == 0;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generate a new RSA key pair.
        /// </summary>
        /// <param name="publicKey">An XML string containing ONLY THE PUBLIC RSA KEY.</param>
        /// <param name="privateKey">An XML string containing a PUBLIC AND PRIVATE RSA KEY.</param>
        public  void GenerateRSAKeys(out string publicKey, out string privateKey)
        {
            try
            {
                CspParameters cspParams = GetCspParameters();
                cspParams.Flags = CspProviderFlags.UseArchivableKey;

                rsaProvider = new RSACryptoServiceProvider(KEY_SIZE, cspParams);

                // Export public key
                publicKey = rsaProvider.ToXmlString(false);

                // Export private/public key pair 
                privateKey = rsaProvider.ToXmlString(true);
            }
            catch (Exception ex)
            {
                // Any errors? Show them
                throw new Exception("Exception generating a new RSA key pair! More info: " + ex.Message);
            }
            finally
            {
                // Do some clean up if needed
            }

        } // GenerateKeys method

        /// <summary>
        /// Encrypts data with the System.Security.Cryptography.RSA algorithm.
        /// </summary>
        /// <param name="publicKey">An XML string containing the public RSA key.</param>
        /// <param name="plainText">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        public  string EncryptRSA(string publicKey, string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                throw new ArgumentException("Data are empty");

            int maxLength = GetMaxDataLength();
            if (Encoding.Unicode.GetBytes(plainText).Length > maxLength)
                throw new ArgumentException("Maximum data length is " + maxLength / 2);

            if (!IsKeySizeValid())
                throw new ArgumentException("Key size is not valid");

            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Key is null or empty");

            byte[] plainBytes = null;
            byte[] encryptedBytes = null;
            string encryptedText = "";

            try
            {
                CspParameters cspParams = GetCspParameters();
                cspParams.Flags = CspProviderFlags.NoFlags;

                rsaProvider = new RSACryptoServiceProvider(KEY_SIZE, cspParams);

                // [1] Import public key
                rsaProvider.FromXmlString(publicKey);

                // [2] Get plain bytes from plain text
                plainBytes = Encoding.Unicode.GetBytes(plainText);

                // Encrypt plain bytes
                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

                // Get encrypted text from encrypted bytes
                // encryptedText = Encoding.Unicode.GetString(encryptedBytes); => NOT WORKING
                encryptedText = Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                // Any errors? Show them
                throw new Exception("Exception encrypting file! More info: " + ex.Message);
            }
            finally
            {
                // Do some clean up if needed
            }

            return encryptedText;

        }
        // Encrypt method

        /// <summary>
        /// Decrypts data with the System.Security.Cryptography.RSA algorithm.
        /// </summary>
        /// <param name="privateKey">An XML string containing a public and private RSA key.</param>
        /// <param name="encryptedText">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        public  string DecryptRSA(string privateKey, string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                throw new ArgumentException("Data are empty");

            if (!IsKeySizeValid())
                throw new ArgumentException("Key size is not valid");

            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Key is null or empty");

            byte[] encryptedBytes = null;
            byte[] plainBytes = null;
            string plainText = "";

            try
            {
                CspParameters cspParams = GetCspParameters();
                cspParams.Flags = CspProviderFlags.NoFlags;

                rsaProvider = new RSACryptoServiceProvider(KEY_SIZE, cspParams);

                // [1] Import private/public key pair
                rsaProvider.FromXmlString(privateKey);

                // [2] Get encrypted bytes from encrypted text
                // encryptedBytes = Encoding.Unicode.GetBytes(encryptedText); => NOT WORKING
                encryptedBytes = Convert.FromBase64String(encryptedText);

                // Decrypt encrypted bytes
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

                // Get decrypted text from decrypted bytes
                plainText = Encoding.Unicode.GetString(plainBytes);
            }
            catch (Exception ex)
            {
                // Any errors? Show them
                throw new Exception("Exception decrypting file! More info: " + ex.Message);
            }
            finally
            {
                // Do some clean up if needed
            }

            return plainText;

        }
        // Decrypt method

        #endregion

        public  bool CheckSignRSA(string data, string sign, string publicKey)
        {
            try
            {
                RSACryptoServiceProvider rsacp = new RSACryptoServiceProvider();
                rsacp.FromXmlString(publicKey);
                return rsacp.VerifyData(Encoding.UTF8.GetBytes(data), "SHA1",
                Convert.FromBase64String(sign));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Encrypt Triple DES
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public  string EncryptTripleDES(string key, string data)
        {
            data = data.Trim();
            byte[] keydata = Encoding.ASCII.GetBytes(key);
            string md5String = BitConverter.ToString(new
            MD5CryptoServiceProvider().ComputeHash(keydata)).Replace("-", "").ToLower();
            byte[] tripleDesKey = Encoding.ASCII.GetBytes(md5String.Substring(0, 24));
            TripleDES tripdes = TripleDES.Create();
            tripdes.Mode = CipherMode.ECB;
            tripdes.Key = tripleDesKey;
            tripdes.GenerateIV();
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms,
            tripdes.CreateEncryptor(),
            CryptoStreamMode.Write);
            encStream.Write(Encoding.ASCII.GetBytes(data), 0,
            Encoding.ASCII.GetByteCount(data));
            encStream.FlushFinalBlock();
            byte[] cryptoByte = ms.ToArray();
            ms.Close();
            encStream.Close();
            return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0)).Trim();
        }

        /// <summary>
        /// Descrypt Triple DES
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public  string DecryptTripleDES(string key, string data)
        {
            byte[] keydata = Encoding.ASCII.GetBytes(key);
            string md5String = BitConverter.ToString(new
            MD5CryptoServiceProvider().ComputeHash(keydata)).Replace("-",
            "").Replace(" ", "+").ToLower();
            byte[] tripleDesKey = Encoding.ASCII.GetBytes(md5String.Substring(0,
            24));
            TripleDES tripdes = TripleDES.Create();
            tripdes.Mode = CipherMode.ECB;
            tripdes.Key = tripleDesKey;
            byte[] cryptByte = Convert.FromBase64String(data);
            MemoryStream ms = new MemoryStream(cryptByte, 0, cryptByte.Length);
            ICryptoTransform cryptoTransform = tripdes.CreateDecryptor();
            CryptoStream decStream = new CryptoStream(ms, cryptoTransform,
            CryptoStreamMode.Read);
            StreamReader read = new StreamReader(decStream);
            return (read.ReadToEnd());
        }

        /// <summary>
        /// EncryptKeyIv
        /// </summary>
        /// <param name="text"></param>
        /// <param name="_key"></param>
        /// <param name="_iv"></param>
        /// <returns></returns>
        public  string EncryptKeyIv(string text, string _key, string _iv)
        {
            byte[] key = Encoding.UTF8.GetBytes(_key);
            byte[] iv = Encoding.UTF8.GetBytes(_iv);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = iv;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(text);
            byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// DecryptKeyIv
        /// </summary>
        /// <param name="text"></param>
        /// <param name="_key"></param>
        /// <param name="_iv"></param>
        /// <returns></returns>
        public  string DecryptKeyIv(string text, string _key, string _iv)
        {
            byte[] key = Encoding.UTF8.GetBytes(_key);
            byte[] iv = Encoding.UTF8.GetBytes(_iv);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            byte[] encryptedData = Convert.FromBase64String(text);
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = iv;
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }

        public  bool ValidateReCaptcha(string recaptchaResponse, string secretKey)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + secretKey + "&response=" + recaptchaResponse);
                WebResponse response = req.GetResponse();
                using (StreamReader readStream = new StreamReader(response.GetResponseStream()))
                {
                    string jsonResponse = readStream.ReadToEnd();
                    ReCaptchaResponse jobj = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonResponse);
                    return jobj.success;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  string SHA1Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Generates a hash for the given plain text value and returns a
        /// base64-encoded result. Before the hash is computed, a random salt
        /// is generated and appended to the plain text. This salt is stored at
        /// the end of the hash value, so it can be used later for hash
        /// verification.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be hashed. The function does not check whether
        /// this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt
        /// value will be generated.
        /// </param>
        /// <returns>
        /// Hash value formatted as a base64-encoded string.
        /// </returns>
        public  string ComputeHash(string plainText,
                                         string hashAlgorithm,
                                         byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
              
                int saltSize = 16;

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash
        /// value. Plain text is hashed with the same salt value as the original
        /// hash.
        /// </summary>
        /// <param name="plainText">
        /// Plain text to be verified against the specified hash. The function
        /// does not check whether this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="hashValue">
        /// Base64-encoded hash value produced by ComputeHash function. This value
        /// includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash mathes the specified hash the function the return
        /// value is true; otherwise, the function returns false.
        /// </returns>
        public  bool VerifyHash(string plainText,
                                      string hashAlgorithm,
                                      string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hashSizeInBits = 160;
                    break;

                case "SHA256":
                    hashSizeInBits = 256;
                    break;

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString =
                        ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Base64Encode(plainTextBytes);
        }
        public string Base64Encode(byte[] plainText)
        {
            return System.Convert.ToBase64String(plainText);
        }
        /// <summary>
        /// Base64Decode
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return Base64Decode(base64EncodedBytes);
        }
        public string Base64Decode(byte[] base64EncodedData)
        {
            return System.Text.Encoding.UTF8.GetString(base64EncodedData);
        }

        public string BCryptPasswordEncoder(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool BCryptPasswordVerifier(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public string SHA1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Base64Encode(hash);
            }
        }

        public string PasswordHash(string password)
        {
            return SHA1(SHA1Hash(password));
        }

        // from JWT spec
        public string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }
        public string Base64UrlEncode(string input)
        {
            return Base64UrlEncode(Encoding.UTF8.GetBytes(input));
        }
        // from JWT spec
        public byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        private class ReCaptchaResponse
        {
            public bool success { get; set; }
            [JsonProperty("error-codes")]
            public List<string> errorcodes { get; set; }
        }
    }
}
