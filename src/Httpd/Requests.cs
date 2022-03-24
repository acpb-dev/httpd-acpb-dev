using System.Net.Mime;
using System.Text;
using Httpd;

public class Requests
{
    private readonly ReadHTML _readHtml = new();
    private readonly HtmlBuilder _htmlBuilder = new();
    public bool Error404 = false;
    public byte[] ManageRequest(string request)
    {
        var strReader = new StringReader(request);
        var aLine = strReader.ReadLine();
        if (aLine != null)
        {
            var splitStream = aLine.Split();
            if (splitStream[0].Equals("GET") && splitStream[1].Equals("/") || splitStream.Length == 0)
            {
                return Html(true, splitStream[1]);
            }
            else
            {
                return Html(false, splitStream[1]);
            }
        }
        return Array.Empty<byte>();
    }

    private byte[] Html(bool type, string path)
    {
        Error404 = false;
        var text = type ? SearchIndex() : ReadSpecifiedFiles(path);
        var response = Response(text.Length);
        var temp = Encoding.UTF8.GetBytes(response);
        var z = new byte[temp.Length + text.Length];
        temp.CopyTo(z, 0);
        text.CopyTo(z, temp.Length);
        return z;
    }

    private string Response(int length)
    {
        var response = !Error404 ? @"HTTP/1.1 200 OK\r\n" : @"HTTP/1.1 404 OK\r\n";
        response += "Content-Length: " + length;
        response += "Content-Type: text/html";
        response += "Connection: close\r\n";
        response += "\r\n";
        return response;
    }

    private byte[] ReadSpecifiedFiles(string path)
    {
        var temp = path.Split(".");
        string extension;
        if (temp.Length > 1)
        {
            extension = temp[^1];
        }
        else
        {
            return HtmlBuilder(path);
        }
        if (extension.Equals("ico"))
        {
            return Array.Empty<byte>();
        }
        if (extension.Equals("js") && extension.Equals("html") && extension.Equals("txt") && extension.Equals("css"))
        {
            return Encoding.UTF8.GetBytes(File.ReadAllText(path.TrimStart('/')));
        }
        return File.ReadAllBytes(path.TrimStart('/'));
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
                Error404 = true;
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
                string[] directorySplit = directory.Split('\\');
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