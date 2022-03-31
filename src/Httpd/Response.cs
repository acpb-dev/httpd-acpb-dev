using System.Text;

namespace Httpd;

public class ResponseBuilder
{
    private readonly IDictionary<string, string> _parameters = new Dictionary<string, string>();
    private readonly IDictionary<string, string> _postValues = new Dictionary<string, string>();
    private static bool _debug = false;

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
        
        if (DebugMode(path))
        {
            _debug = true;
            responseBytes = DebugBuilder(fullPath, request);
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
            var temper = Httpd.HtmlBuilder.Response(test);
            
            if (test.Contains("gzip"))
            {
                responseBytes.Item1 = GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(Encoding.UTF8.GetString(GzipEncoding.GZipDencode(AddTwoByteArrays(responseBytes.Item1, GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(temper.Trim() + Httpd.HtmlBuilder.Footer())))))));
                return (AddTwoByteArrays(ByteReader.ConvertTextToByte(test), GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(Encoding.UTF8.GetString(GzipEncoding.GZipDencode(responseBytes.Item1))))), responseBytes.Item2);
            }

            Console.WriteLine("nope");
            return (AddTwoByteArrays(ByteReader.ConvertTextToByte(test), AddTwoByteArrays(responseBytes.Item1, ByteReader.ConvertTextToByte(temper))), responseBytes.Item2);
        }

        
        return (AddTwoByteArrays(ByteReader.ConvertTextToByte(test), responseBytes.Item1), responseBytes.Item2);
    }

    private string CheckForRoute()
    {
        foreach (var (key, value) in FileReader.Routes)
        {
            
        }
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
                response += $"Content-Length: {GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(Encoding.UTF8.GetString(GzipEncoding.GZipDencode(AddTwoByteArrays(responseBytes, GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(Httpd.HtmlBuilder.Response(response1.Trim()) + Httpd.HtmlBuilder.Footer()))))))).Length}\r\n";
                response += "Content-Encoding: gzip\r\n";
                
            }
            else
            {
                response += $"Content-Length: {ByteReader.ConvertTextToByte(Encoding.UTF8.GetString(GzipEncoding.GZipDencode(AddTwoByteArrays(responseBytes, GzipEncoding.GZipEncode(ByteReader.ConvertTextToByte(Httpd.HtmlBuilder.Response(response1.Trim()) + Httpd.HtmlBuilder.Footer())))))).Length}\r\n";
            }
            response1 = response;
        }
        // response += "Connection: close\r\n";
        response1 += "\r\n";
        return response1;
    }

    private (byte[], string, string) DebugBuilder(string path, IDictionary<string, string> request)
    {
        var topHtml = Httpd.HtmlBuilder.HeaderDebug();
        topHtml += "GET " + path + " HTTP/1.1";
        topHtml += Httpd.HtmlBuilder.Debug(request);
        if (_parameters.Count > 0)
        {
            topHtml += Httpd.HtmlBuilder.Parameters(_parameters);
        }

        if (_postValues.Count > 0)
        {
            topHtml += Httpd.HtmlBuilder.Parameters(_parameters);
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
        return FileReader.DirectoryListing ? FileReader.ReadSpecifiedFiles("/") : (ByteReader.ConvertTextToByte(Httpd.HtmlBuilder.Page404()), "404 Not Found", "text/html");
    }
    
    
    public static (byte[], string, string) HtmlBuilder(string path)
    {
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();
        var topHtml = Httpd.HtmlBuilder.HeaderDirectoryListing();
        var bottomHtml = Httpd.HtmlBuilder.Footer();
        if (!Directory.Exists(path.Trim('/')) && !path.Equals("/"))
        {
            return (ByteReader.ConvertTextToByte(Httpd.HtmlBuilder.Page404()), "404 Not Found", "text/html");
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
            topHtml += Httpd.HtmlBuilder.ParentDirectory(test, "Parent Directory");
        }

        foreach (var (key, value) in directoriesNames)
        {
            var dir = new DirectoryInfo(value);
            
            topHtml += Httpd.HtmlBuilder.DirectoryListingItem(DirectoryReader.CleanPath(value), DirectoryReader.CleanString(key), true, dir.LastAccessTime, 0);
        }

        foreach (var (key, value) in fileNames)
        {
            var dir = new DirectoryInfo(value);
            var fi1 = new FileInfo(value);
            
            topHtml += Httpd.HtmlBuilder.DirectoryListingItem(DirectoryReader.CleanPath(value), DirectoryReader.CleanString(key), false, dir.LastAccessTime, fi1.Length);
        }
        return (ByteReader.ConvertTextToByte(topHtml + bottomHtml), "200 OK", "text/html");
    }
    
    private static bool DebugMode(string path)
    {
        const string debug = "/debug";
        if (path.Length < debug.Length) return false;
        var test = path.Remove(0, path.Length-debug.Length);
        return test.Equals(debug);
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