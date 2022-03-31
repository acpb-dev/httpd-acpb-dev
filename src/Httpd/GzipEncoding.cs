using System.IO.Compression;

namespace Httpd;

public static class GzipEncoding
{

    public static bool IsGzipEncode(IDictionary<string, string> request)
    {
        foreach (var (key, value) in request)
        {
            //Console.WriteLine(key + " " + value);
            if (!key.Equals("Accept-Encoding")) continue;
            var split = value.Split(",");
            if (split.Select(val => val.Trim()).Any(valTrimmed => valTrimmed.Equals("gzip")))
            {
                return true;
            }
        }
        return false;
    }

    public static byte[] GZipEncode(byte[] data)
    {
        using var compressedStream = new MemoryStream();
        using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
        zipStream.Write(data, 0, data.Length);
        zipStream.Close();
        return compressedStream.ToArray();
    }
    
    public static byte[] GZipDecode(byte[] data)
    {
        using (var compressedStream = new MemoryStream(data))
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
        using (var resultStream = new MemoryStream())
        {
            zipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }
    }
}