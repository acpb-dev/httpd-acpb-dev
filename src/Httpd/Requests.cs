using System.Net.Mime;
using System.Text;
using Httpd;

public class Requests
{
    private readonly ReadHTML _readHtml = new();
    private readonly HtmlBuilder _htmlBuilder = new();
    private IDictionary<string, string> _requests = new Dictionary<string, string>();
    private bool _error404 = false;
    private Dictionary<string, string> _imagesFormat = new Dictionary<string, string>
    {
        { "apng", "image/apng" },
        { "avif", "image/avif" },
        { "gif", "image/gif" },
        { "jpg", "image/jpeg" },
        { "jpeg", "image/jpeg" },
        { "jfif", "image/jpeg" },
        { "pjpeg", "image/jpeg" },
        { "pjp", "image/jpeg" },
        { "png", "image/png" },
        { "svg", "image/svg+xml" },
        { "bmp", "image/bmp" },
        { "ttf", "font/ttf" },
        { "tif", "image/tiff" },
        { "tiff", "image/tiff" },
        { "webp", "image/webp" }//,
        // { "ico", "image/vnd.microsoft.icon" }
    };
    private Dictionary<string, string> _fileFormat = new Dictionary<string, string>
    {
        { "html", "text/html" },
        { "htm", "text/html" },
        { "js", "text/javascript" },
        { "css", "text/css" },
        { "mjs", "text/javascript" },
        { "txt", "text/plain" },
        { "php", "application/x-httpd-php" },
    };
    
    public byte[] ManageRequest(string request)
    {
        _requests.Clear();
        var strReader = new StringReader(request);
        // Console.WriteLine(request);
        var count = 0;
        while (null != (request = strReader.ReadLine()))
        {
            string[] keyVal;
            if (count == 0)
            {
                keyVal = request.Split();
            }
            else
            {
                keyVal = request.Split(": ");
 
            }
            if (keyVal.Length > 1)
            {
                _requests.Add(keyVal[0], keyVal[1]);
            }
            count++;
        }
        var link = "";
        foreach (var (key, value) in _requests)
        {
            if (key.Equals("GET"))
            {
                link = value;
            }
        }
        return Html(link);
    }

    private byte[] Html(string path)
    {
        _error404 = false;
        var text = path.Equals("/") ? SearchIndex() : ReadSpecifiedFiles(path);
        var response = Response(text.Length);
        var temp = Encoding.UTF8.GetBytes(response);
        var z = new byte[temp.Length + text.Length];
        temp.CopyTo(z, 0);
        text.CopyTo(z, temp.Length);
        return z;
    }

    private string Response(int length)
    {
        var response = !_error404 ? @"HTTP/1.1 200 OK\r\n" : @"HTTP/1.1 404 OK\r\n";
        response += "Content-Length: " + length;
        response += "Content-Type: text/html";
        response += "Connection: close\r\n";
        response += "\r\n";
        return response;
    }

    private byte[] ReadSpecifiedFiles(string path)
    {
        var temp = path.Split(".");
        if (temp.Length < 2)
        {
            return HtmlBuilder(path);
        }
        var extension = temp[^1];


        foreach (var(key, value) in _fileFormat)
        {
            if (key.Equals(extension))
            {
                if (ReadHTML.CheckFileExistance(path))
                {
                    return Encoding.UTF8.GetBytes(File.ReadAllText(path.TrimStart('/')));
                }
            }
        }
        foreach (var(key, value) in _imagesFormat)
        {
            if (key.Equals(extension))
            {
                if (ReadHTML.CheckFileExistance(path))
                {
                    return File.ReadAllBytes(path.TrimStart('/'));
                }
            }
        }
        return Array.Empty<byte>();
    }

    private byte[] SearchIndex()
    {
        var test = ReadHTML.ReadFilesInDirectory();
        var valid = false;
        foreach (var variable in test)
        {
            var result = variable[^10..];
            if (result.Equals("index.html"))
            {
                valid = true;
            }
        }
        return Encoding.UTF8.GetBytes(valid ? File.ReadAllText("index.html") : _htmlBuilder.Page404());
    }

    private byte[] HtmlBuilder(string path)
    {
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();
        var topHtml = _htmlBuilder.Header();
        var bottomHtml = _htmlBuilder.Footer();
        if (path.Equals("/source"))
        {
            path = "/";
        }
        else
        {
            if (!Directory.Exists(path.Trim('/')))
            {
                _error404 = true;
                return Encoding.UTF8.GetBytes(_htmlBuilder.Page404());
            }
        }
        var files = ReadHTML.ReadFilesInSpecifiedDirectory(path);
        var directories = ReadHTML.ReadSpecifiedDirectories(path);
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
        foreach (var (key, value) in directoriesNames)
        {
            topHtml += _htmlBuilder.Alink(CleanPath(value), key, true);
        }

        foreach (var (key, value) in fileNames)
        {
            topHtml += _htmlBuilder.Alink(CleanPath(value), key, false);
        }
        return Encoding.UTF8.GetBytes(topHtml + bottomHtml);
    }

    private static string CleanPath(string sourceString)
    {
        var test = Directory.GetCurrentDirectory();
        var index = sourceString.IndexOf(test, StringComparison.Ordinal);
        var cleanPath = (index < 0)
            ? sourceString
            : sourceString.Remove(index, test.Length);
        return cleanPath.TrimStart('\\');
    }
}