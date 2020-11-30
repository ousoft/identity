using System;
using System.Security.Cryptography;
using System.Text;

namespace Oyang.Identity.Infrastructure.Utility
{
    public class HashAlgorithmHelper
    {
        private static string ComputeHash(string value, HashAlgorithm hashAlgorithm)
        {
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] bytes = hashAlgorithm.ComputeHash(valueBytes);
            string result = BitConverter.ToString(bytes);
            result = result.Replace("-", "");
            return result;
        }

        private static string ComputeHash(string key, string value, KeyedHashAlgorithm keyedHashAlgorithm)
        {
            keyedHashAlgorithm.Key = Encoding.UTF8.GetBytes(key);
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] bytes = keyedHashAlgorithm.ComputeHash(valueBytes);
            string result = BitConverter.ToString(bytes);
            result = result.Replace("-", "");
            return result;
        }

        public static string ComputeMD5(string value) => ComputeHash(value, MD5.Create());
        public static string ComputeSHA256(string value) => ComputeHash(value, SHA256.Create());
        public static string ComputeHMACMD5(string key, string value) => ComputeHash(key, value, new HMACMD5());
        public static string ComputeHMACSHA256(string key, string value) => ComputeHash(key, value, new HMACSHA256());
    }


}
