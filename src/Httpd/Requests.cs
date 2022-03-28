using System.IO.Compression;

namespace Httpd;

public class Requests
{
    private readonly ResponseBuilder _responseBuilder = new();
    private IDictionary<string, string> _requests = new Dictionary<string, string>();
    
    public byte[] HandleRequest(string verb, string resource, IDictionary<string, string> header, char[] body, SeriLog serilog)
    {
        if (verb is "GET")
        {
            var (bytes, status) = GetResponseCreator(resource);
            serilog.HttpMethod = verb;
            serilog.Path = resource;
            serilog.Status = status;
            return bytes;
        }
        else if (verb is "POST")
        {
            
            var (bytes, status) = PostResponseCreator(header, body);
            serilog.HttpMethod = verb;
            serilog.Path = resource;
            serilog.Status = status;
            return bytes;
            
        }
        else if (verb is "PUT")
        {
            var (bytes, status) = PutResponseCreator();
            serilog.HttpMethod = verb;
            serilog.Path = resource;
            serilog.Status = status;
            return bytes;
            
        }
        else if (verb is "PATCH")
        {
            var (bytes, status) = PatchResponseCreator();
            serilog.HttpMethod = verb;
            serilog.Path = resource;
            serilog.Status = status;
            return bytes;
            
        }
        else if (verb is "DELETE")
        {
            var (bytes, status) = DeleteResponseCreator();
            serilog.HttpMethod = verb;
            serilog.Path = resource;
            serilog.Status = status;
            return bytes;
            
        }
        else
        {
            var (bytes, status) = GetResponseCreator(resource);
            serilog.HttpMethod = verb;
            serilog.Path = resource;
            serilog.Status = status;
            return bytes;
        }
        return Array.Empty<byte>();
    }

    private (byte[], string) GetResponseCreator(string path)
    {
        var byteResponse = _responseBuilder.ResponseManager(path, _requests);
        return byteResponse;
    }
    private (byte[], string) PostResponseCreator(IDictionary<string, string> header, char[] body)
    {
        //Console.WriteLine(body);
        // foreach (var (key, value) in body)
        // {
        //     Console.WriteLine(key + "\t " + value);
        // }
        return(ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404");
    }
    private (byte[], string) PutResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404");
    }
    private (byte[], string) PatchResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404");
    }
    private (byte[], string) DeleteResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404");
    }
    
    private static byte[] Compress(byte[] data)
    {
        using var compressedStream = new MemoryStream();
        using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
        zipStream.Write(data, 0, data.Length);
        zipStream.Close();
        return compressedStream.ToArray();
    }
    
    // private bool CheckGZip()
    // {
    //     foreach (var (key, value) in _requests)
    //     {
    //         //Console.WriteLine(key + " " + value);
    //         if (key.Equals("Accept-Encoding"))
    //         {
    //             var split = value.Split(",");
    //             foreach (var val in split)
    //             {
    //                 var valTrimmed = val.Trim();
    //                 if (valTrimmed.Equals("gzip"))
    //                 {
    //                     return true;
    //                 }
    //             }
    //         }
    //     }
    //     return false;
    // }
}