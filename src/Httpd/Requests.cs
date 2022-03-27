using System.IO.Compression;

namespace Httpd;

public class Requests
{
    private readonly ResponseBuilder _responseBuilder = new();
    private readonly IDictionary<string, string> _requests = new Dictionary<string, string>();
    public byte[] SeparatedRequest(string request, SeriLog serilog)
    {
        _requests.Clear();
        var strReader = new StringReader(request);
        var verb = "";
        var resource = "";
        var body = "";
        var contentLenght = 0;
        var count = 0;
        var isContent = false;
        while (null != (request = strReader.ReadLine()))
        {
            if (request.Equals(""))
            {
                isContent = true;
            }
            if (isContent)
            {
                body += request;
            }

            string[] keyVal;
            if (count == 0)
            {
                keyVal = request.Split();
                resource = keyVal[1];
                verb = keyVal[0];
            }
            else
            {
                keyVal = request.Split(": ");
 
            }
            if (keyVal.Length > 1 && count > 0)
            {
                
                _requests.Add(keyVal[0], keyVal[1]);
            }
            count++;
        }
        Console.WriteLine(body);
        var (bytes, status) = HandleRequest(verb, resource, _requests, body);
        
        serilog.HttpMethod = verb;
        serilog.Path = resource;
        serilog.Status = status;
        // if (CheckGZip())
        // {
        //     return Compress(bytes);
        // }


        return bytes;
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
    
    private static byte[] Compress(byte[] data)
    {
        using var compressedStream = new MemoryStream();
        using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
        zipStream.Write(data, 0, data.Length);
        zipStream.Close();
        return compressedStream.ToArray();
    }
    
    private (byte[], string) HandleRequest(string verb, string resource, IDictionary<string, string> headers, string body)
    {
        return verb switch
        {
            "GET" => GetResponseCreator(resource),
            "POST" => PostResponseCreator(headers),
            "PUT" => PutResponseCreator(),
            "PATCH" => PatchResponseCreator(),
            "DELETE" => DeleteResponseCreator(),
            _ => GetResponseCreator(resource)
        };
    }

    private (byte[], string) GetResponseCreator(string path)
    {
        var byteResponse = _responseBuilder.ResponseManager(path, _requests);
        return byteResponse;
    }
    private (byte[], string) PostResponseCreator(IDictionary<string, string> body)
    {
        //Console.WriteLine(body);
        foreach (var (key, value) in body)
        {
            //Console.WriteLine(key + "\t " + value);
        }
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
}