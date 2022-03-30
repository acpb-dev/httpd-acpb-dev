namespace Httpd;

public class ResponseBuilder
{
    private IDictionary<string, string> Parameters = new Dictionary<string, string>();
    private IDictionary<string, string> PostValues = new Dictionary<string, string>();

    public (byte[], string) ResponseManager(string path, IDictionary<string, string> request, string postResponse)
    {
        Parameters.Clear();
        (byte[], string, string) responseBytes;
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
            responseBytes = DebugBuilder(request);
        }
        else if (path.Equals("/"))
        {
            responseBytes = SearchIndex();
        }
        else
        {
            responseBytes = FileReader.ReadSpecifiedFiles(path);
        }

        if (Gzip.IsGzipEncode(request))
        {
            responseBytes.Item1 = Gzip.GZip(responseBytes.Item1);
        }
        
        return (AddTwoByteArrays(ResponseHeader(responseBytes.Item1, responseBytes.Item2, responseBytes.Item3, Gzip.IsGzipEncode(request)), responseBytes.Item1), responseBytes.Item2);
    }
    
    private static byte[] ResponseHeader(byte[] responseBytes, string error, string contentType, bool gzip)
    {
        var response = $"HTTP/1.1 {error} OK\r\n";
        response += $"Content-Length: {responseBytes.Length}\r\n";
        response += $"Content-Type: {contentType}\r\n";
        if (gzip)
        {
            response += "Content-Encoding: gzip\r\n";
        }
        // response += "Connection: close\r\n";
        response += "\r\n";
        return ByteReader.ConvertTextToByte(response);
    }

    private (byte[], string, string) DebugBuilder(IDictionary<string, string> request)
    {
        var topHtml = HtmlStringBuilder.Header();
        var bottomHtml = HtmlStringBuilder.Footer();
        topHtml += HtmlStringBuilder.Debug(request);
        if (Parameters.Count > 0)
        {
            topHtml += HtmlStringBuilder.Params(Parameters);
        }

        if (PostValues.Count > 0)
        {
            topHtml += HtmlStringBuilder.Params(Parameters);
        }
        
        return (ByteReader.ConvertTextToByte(topHtml + bottomHtml), "200", "text/html");
    }

    private (byte[], string, string) SearchIndex()
    {
        var test = DirectoryFileReader.ReadFilesInDirectory();
        foreach (var variable in test)
        {
            var result = variable[^10..];
            if (result.Equals("index.html"))
            {
                return (ByteReader.ConvertFileToByte(result), "200", "text/html");
            }
        }
        return FileReader.DirectoryListing ? FileReader.ReadSpecifiedFiles("/") : (ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404", "text/html");
    }
    
    
    public static (byte[], string, string) HtmlBuilder(string path)
    {
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();
        var topHtml = HtmlStringBuilder.HeaderDl();
        var bottomHtml = HtmlStringBuilder.Footer();
        if (!Directory.Exists(path.Trim('/')) && !path.Equals("/"))
        {
            return (ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404", "text/html");
        }
        var files = DirectoryFileReader.ReadFilesInSpecifiedDirectory(path);
        var directories = DirectoryFileReader.ReadSpecifiedDirectories(path);
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
            topHtml += HtmlStringBuilder.ParentDirectory(test, "Parent Directory");
        }

        foreach (var (key, value) in directoriesNames)
        {
            var dir = new DirectoryInfo(value);
            
            topHtml += HtmlStringBuilder.Alink(DirectoryFileReader.CleanPath(value), DirectoryFileReader.CleanString(key), true, dir.LastAccessTime, 0);
        }

        foreach (var (key, value) in fileNames)
        {
            var dir = new DirectoryInfo(value);
            var fi1 = new FileInfo(value);
            
            topHtml += HtmlStringBuilder.Alink(DirectoryFileReader.CleanPath(value), DirectoryFileReader.CleanString(key), false, dir.LastAccessTime, fi1.Length);
        }
        return (ByteReader.ConvertTextToByte(topHtml + bottomHtml), "200", "text/html");
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
                Parameters.TryAdd(temp[0], temp[1]);
            }
            else
            {
                Console.WriteLine(temp[0] + " & " + temp[1]);
                PostValues.TryAdd(temp[0], temp[1]);
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