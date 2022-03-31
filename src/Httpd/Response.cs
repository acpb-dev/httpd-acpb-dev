using System.Text;

namespace Httpd;

public class ResponseBuilder
{
    private readonly IDictionary<string, string> _parameters = new Dictionary<string, string>();
    private readonly IDictionary<string, string> _postValues = new Dictionary<string, string>();
    private static bool _debug;

    public (byte[], string) ResponseManager(string path, IDictionary<string, string> request, string postResponse)
    {
        _debug = false;
        _parameters.Clear();
        (byte[], string, string) responseBytes;
        string fullPath = path;
        if (!postResponse.Equals(""))
        {
            WriteParamsOrPost(postResponse, true);
        }
        if (path.Contains('?'))
        {
            path = Params(path);
        }

        if (!IsRouteInPath(path, request).Item2.Equals(""))
        {
            responseBytes = IsRouteInPath(path, request);
        }
        else if (path.Equals("/"))
        {
            responseBytes = SearchIndex();
        }
        else
        {
            responseBytes = FileReader.ReadSpecifiedFiles(path);
        }

        if (GzipEncoding.IsGzipEncode(request))
        {
            responseBytes.Item1 = GzipEncoding.GZipEncode(responseBytes.Item1);
        }

        var test = ResponseHeader(responseBytes.Item1, responseBytes.Item2, responseBytes.Item3, GzipEncoding.IsGzipEncode(request));
        if (_debug)
        {
            var temper = HtmlBuilder.Response(test);
            
            if (test.Contains("gzip"))
            {
                responseBytes.Item1 = GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(Encoding.UTF8.GetString(GzipEncoding.GZipDecode(AddTwoByteArrays(responseBytes.Item1, GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(temper.Trim() + Httpd.HtmlBuilder.Footer())))))));
                return (AddTwoByteArrays(ByteReader.ConvertTextToByte(test), GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(Encoding.UTF8.GetString(GzipEncoding.GZipDecode(responseBytes.Item1))))), responseBytes.Item2);
            }

            Console.WriteLine("nope");
            return (AddTwoByteArrays(ByteReader.ConvertTextToByte(test), AddTwoByteArrays(responseBytes.Item1, ByteReader.ConvertTextToByte(temper))), responseBytes.Item2);
        }
        return (AddTwoByteArrays(ByteReader.ConvertTextToByte(test), responseBytes.Item1), responseBytes.Item2);
    }

    private (byte[], string, string) IsRouteInPath(string path, IDictionary<string, string> request)
    {
        foreach (var (key, value) in FileReader.Routes)
        {
            if (path.Length > key.Length)
            {
                var test = path.Remove(0, path.Length-key.Length);
                if (test.Equals(key))
                {
                    return ExecuteRoute(value, path, request);
                }
            }
        }
        return (Array.Empty<byte>(), "", "");
    }

    private (byte[], string, string) ExecuteRoute(string route, string path, IDictionary<string, string> request)
    {
        if (route.Equals("debug"))
        {
            _debug = true;
            return DebugBuilder(path, request);
        }else if (route.Equals("page404"))
        {
            return (ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404 Not Found", "text/html");
        }
        return (Array.Empty<byte>(), "", "");
    }
    
    private static string ResponseHeader(byte[] responseBytes, string status, string contentType, bool gzip)
    {
        var response1 = $"HTTP/1.1 {status}\r\n";
        response1 += $"Content-Length: {responseBytes.Length}\r\n";
        response1 += $"Content-Type: {contentType}\r\n";
        if (gzip)
        {
            response1 += "Content-Encoding: gzip\r\n";
        }
        if (_debug)
        {
            var response = $"HTTP/1.1 {status}\r\n";
            
            
            response += $"Content-Type: {contentType}\r\n";
            if (gzip)
            {
                response += $"Content-Length: {GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(Encoding.UTF8.GetString(GzipEncoding.GZipDecode(AddTwoByteArrays(responseBytes, GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(HtmlBuilder.Response(response1.Trim()) + HtmlBuilder.Footer()))))))).Length}\r\n";
                response += "Content-Encoding: gzip\r\n";
                
            }
            else
            {
                response += $"Content-Length: {ByteReader.ConvertTextToByte(Encoding.UTF8.GetString(GzipEncoding.GZipDecode(AddTwoByteArrays(responseBytes, GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(HtmlBuilder.Response(response1.Trim()) + HtmlBuilder.Footer())))))).Length}\r\n";
            }
            response1 = response;
        }
        // response += "Connection: close\r\n";
        response1 += "\r\n";
        return response1;
    }

    private (byte[], string, string) DebugBuilder(string path, IDictionary<string, string> request)
    {
        var topHtml = HtmlBuilder.HeaderDebug();
        topHtml += "GET " + path + " HTTP/1.1";
        topHtml += HtmlBuilder.Debug(request);
        if (_parameters.Count > 0)
        {
            topHtml += HtmlBuilder.Parameters(_parameters);
        }

        if (_postValues.Count > 0)
        {
            topHtml += HtmlBuilder.Parameters(_parameters);
        }
        
        return (ByteReader.ConvertTextToByte(topHtml), "200 OK", "text/html");
    }

    private (byte[], string, string) SearchIndex()
    {
        var test = DirectoryReader.ReadFilesInDirectory();
        foreach (var variable in test)
        {
            var result = variable[^10..];
            if (result.Equals("index.html"))
            {
                return (ByteReader.ConvertFileToByte(result), "200 OK", "text/html");
            }
        }
        return FileReader.DirectoryListing ? FileReader.ReadSpecifiedFiles("/") : (ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404 Not Found", "text/html");
    }
    
    
    public static (byte[], string, string) DirectoryListingCreator(string path)
    {
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();
        var topHtml = HtmlBuilder.HeaderDirectoryListing();
        var bottomHtml = HtmlBuilder.Footer();
        if (!Directory.Exists(path.Trim('/')) && !path.Equals("/"))
        {
            return (ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404 Not Found", "text/html");
        }
        var files = DirectoryReader.ReadFilesInSpecifiedDirectory(path);
        var directories = DirectoryReader.ReadSpecifiedDirectories(path);
        if (files.Length > 0)
        {
            foreach (var file in files)
            {
                var fileSplit = file.Split('\\');
                fileNames.Add(fileSplit[^1], file);
            }
        }
        if (directories.Length > 0)
        {
            foreach (var directory in directories)
            {
                var directorySplit = directory.Split('\\');
                directoriesNames.Add(directorySplit[^1], directory);
            }
        }

        if (!path.Equals("/"))
        {
            var test = path.TrimEnd('/');
            var index = test.LastIndexOf('/');
            test = test.Substring(0, index);
            if (test.Equals(""))
            {
                test = "/";
            }
            topHtml += HtmlBuilder.ParentDirectory(test, "Parent Directory");
        }

        foreach (var (key, value) in directoriesNames)
        {
            var dir = new DirectoryInfo(value);
            
            topHtml += HtmlBuilder.DirectoryListingItem(DirectoryReader.CleanPath(value), DirectoryReader.CleanString(key), true, dir.LastAccessTime, 0);
        }

        foreach (var (key, value) in fileNames)
        {
            var dir = new DirectoryInfo(value);
            var fi1 = new FileInfo(value);
            
            topHtml += HtmlBuilder.DirectoryListingItem(DirectoryReader.CleanPath(value), DirectoryReader.CleanString(key), false, dir.LastAccessTime, fi1.Length);
        }
        return (ByteReader.ConvertTextToByte(topHtml + bottomHtml), "200 OK", "text/html");
    }
    


    private string Params(string path)
    {
        var indexQ = path.LastIndexOf('?');
        var indexS = path.LastIndexOf('/');
        if (indexS > indexQ)
        {
            if (indexQ <= -1 || indexS <= 1) return path;
            var param = path.Remove(indexS);
            param = param.Remove(0, indexQ+1);
            path = path.Remove(indexQ, indexS - indexQ);
            WriteParamsOrPost(param, true);
            return path;
        }
        else
        {
            if (indexQ <= -1) return path;
            var param = path.Remove(0, indexQ+1);
            path = path.Remove(indexQ);
            WriteParamsOrPost(param, true);
            return path;
        }
    }

    private void WriteParamsOrPost(string param, bool isParam)
    {
        var seperated = param.Split('&');
        foreach (var separate in seperated)
        {
            var temp = separate.Split('=');
            if (temp.Length <= 1) continue;
            if (isParam)
            {
                _parameters.TryAdd(temp[0], temp[1]);
            }
            else
            {
                // Console.WriteLine(temp[0] + " & " + temp[1]);
                _postValues.TryAdd(temp[0], temp[1]);
            }
        }
    }
    
    private static byte[] AddTwoByteArrays(byte[] array1, byte[] array2)
    {
        var z = new byte[array1.Length + array2.Length];
        array1.CopyTo(z, 0);
        array2.CopyTo(z, array1.Length);
        return z;
    }
}