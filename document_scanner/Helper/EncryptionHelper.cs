using System;
using System.IO;
using System.Security.Cryptography;

namespace document_scanner.Helpers
{
    public static class EncryptionHelper
    {
        private static readonly byte[] key;
        private static readonly byte[] iv;

        // Constructor to initialize key and IV
        static EncryptionHelper()
        {
            // Generate new key and IV
            (key, iv) = GenerateKeyAndIV();
        }

        private static (byte[], byte[]) GenerateKeyAndIV()
        {
            using var aes = Aes.Create();
            return (aes.Key, aes.IV);
        }

        public static byte[] EncryptData(byte[] data)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

        public static byte[] DecryptData(byte[] data)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(data);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new MemoryStream();
            cs.CopyTo(reader);
            return reader.ToArray();
        }
    }
}
