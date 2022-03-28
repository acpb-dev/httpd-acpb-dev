using System.IO.Compression;

namespace Httpd;

public static class Gzip
{
    const int BUFFER_SIZE = 0x4000;
    
    private static void Compress(Stream inputStream, Stream outputStream)
    {
        byte[] buffer = new byte[BUFFER_SIZE];
        using var gzip = new GZipStream(outputStream, CompressionMode.Compress);
        int count;
        while ((count = inputStream.Read(buffer, 0, BUFFER_SIZE)) > 0)
        {
            gzip.Write(buffer, 0, count);
        }
    }
    
    private static byte[] Compress(byte[] input)
    {
        using var inputStream = new MemoryStream(input);
        using var outputStream = new MemoryStream();
        Compress(inputStream, outputStream);
        return outputStream.ToArray();
    }
    
    public static byte[] CheckGZip(IDictionary<string, string> request, byte[] data)
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
                        using var compressedStream = new MemoryStream();
                        using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
                        zipStream.Write(data, 0, data.Length);
                        zipStream.Close();
                        //return compressedStream.ToArray();
                        return data;
                    }
                }
            }
        }
        return data;
    }
}