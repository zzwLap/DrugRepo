using System;
using System.IO;
using System.IO.Compression;

public class CommonHelper
{
    public static string GzipAsBase64String(Stream inStream)
    {
        return CompressHelper.GzipAsBase64String(inStream);
    }

    public static byte[] UnGzipFromBase64String(string data)
    {
        return CompressHelper.UnGzipFromBase64String(data);
    }

    public static string EncryptString(string plainText)
    {
        return AesOperation.EncryptString(plainText);
    }

    public static string DecryptString(string plainText)
    {
        return AesOperation.DecryptString(plainText);
    }
}

public partial class CompressHelper
{
    public static string GzipAsBase64String(Stream inStream)
    {
        var outputStream = new MemoryStream();
        var gzipStream = new GZipStream(outputStream, CompressionMode.Compress);
        inStream.CopyTo(gzipStream);
        gzipStream.Close();
        var res = outputStream.ToArray();
        outputStream.Close();
        return Convert.ToBase64String(res);
    }

    public static byte[] UnGzipFromBase64String(string data)
    {
        var bytes = Convert.FromBase64String(data);
        using (var inStream = new MemoryStream(bytes))
        using (var zipStream = new GZipStream(inStream, CompressionMode.Decompress))
        using (var outStream = new MemoryStream())
        {
            zipStream.CopyTo(outStream);
            return outStream.ToArray();
        }
    }
}