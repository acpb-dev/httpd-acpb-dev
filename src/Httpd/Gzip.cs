using System.IO.Compression;

namespace Httpd;

public static class Gzip
{

    public static bool IsGzipEncode(IDictionary<string, string> request)
    {
        foreach (var (key, value) in request)
        {
            //Console.WriteLine(key + " " + value);
            if (key.Equals("Accept-Encoding"))
            {
                var split = value.Split(",");
                foreach (var val in split)
                {
                    var valTrimmed = val.Trim();
                    if (valTrimmed.Equals("gzip"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static byte[] GZip(byte[] data)
    {
        
        using var compressedStream = new MemoryStream();
        using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
        zipStream.Write(data, 0, data.Length);
        zipStream.Close();
        return compressedStream.ToArray();
        return data;

    }
}