using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class AesOperation
{
    public static string key = "b14ca5898a4e4133bbce2ea2315a1916";
    public static Encoding encoding = Encoding.UTF8;
    public static string EncryptString(string key, string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;
        var aesKey = encoding.GetBytes(key);

        using (Aes aes = Aes.Create())
        {
            aes.Key = aesKey;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
            {
                streamWriter.Write(plainText);
                array = memoryStream.ToArray();
                return Convert.ToBase64String(array);
            }
        }
    }

    public static string DecryptString(string key, string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        var aesKey = encoding.GetBytes(key);

        using (Aes aes = Aes.Create())
        {
            aes.Key = aesKey;
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    public static string EncryptString(string plainText)
    {
        return EncryptString(key, plainText);
    }

    public static string DecryptString(string plainText)
    {
        return DecryptString(key, plainText);
    }
}
