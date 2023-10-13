using System;
using System.IO;
using System.Security.Cryptography;

namespace LIM.Encryption
{
    public static class AES256
    {
        public static byte[] Encrypt(byte[] plainData, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = AdjustKeySize(key, aes.KeySize);
                aes.Mode = CipherMode.ECB; // Electronic Codebook mode
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return PerformCryptography(plainData, encryptor);
                }
            }
        }

        public static byte[] Decrypt(byte[] encryptedData, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = AdjustKeySize(key, aes.KeySize);
                aes.Mode = CipherMode.ECB; // Electronic Codebook mode
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    return PerformCryptography(encryptedData, decryptor);
                }
            }
        }

        private static byte[] PerformCryptography(byte[] inputData, ICryptoTransform cryptoTransform)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    cs.Write(inputData, 0, inputData.Length);
                    cs.FlushFinalBlock();
                }
                return ms.ToArray();
            }
        }

        private static byte[] AdjustKeySize(byte[] key, int keySize)
        {
            int bytesNeeded = keySize / 8;
            byte[] adjustedKey = new byte[bytesNeeded];
            int minKeyLength = Math.Min(key.Length, bytesNeeded);
            Array.Copy(key, adjustedKey, minKeyLength);
            return adjustedKey;
        }
    }
}