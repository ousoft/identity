using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.Infrastructure.Utility
{
    public class HashAlgorithmHelper
    {
        public static string Create(string value, HashMode hashMode)
        {
            HashAlgorithm hashAlgorithm = null;
            switch (hashMode)
            {
                case HashMode.MD5:
                    hashAlgorithm = MD5.Create();
                    break;
                case HashMode.SHA256:
                    hashAlgorithm = SHA256.Create();
                    break;
                default:
                    break;
            }
            byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] bytes = hashAlgorithm.ComputeHash(valueBytes);
            string result = BitConverter.ToString(bytes);
            result = result.Replace("-", "");
            return result;
        }

        public static string CreateHMAC(string key, string value, HashMode hashMode)
        {
            KeyedHashAlgorithm keyedHashAlgorithm = null;
            switch (hashMode)
            {
                case HashMode.MD5:
                    keyedHashAlgorithm = new HMACMD5();
                    break;
                case HashMode.SHA256:
                    keyedHashAlgorithm = new HMACSHA256();
                    break;
                default:
                    break;
            }
            keyedHashAlgorithm.Key = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
            byte[] bytes = keyedHashAlgorithm.ComputeHash(valueBytes);
            string result = BitConverter.ToString(bytes);
            result = result.Replace("-", "");
            return result;
        }
    }

    public enum HashMode
    {
        MD5,
        SHA256,
    }
}
