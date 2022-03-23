using System.Net.Mime;
using System.Text;
using Httpd;

public class Requests
{
    private readonly ReadHTML _readHtml = new();
    private readonly HtmlBuilder _htmlBuilder = new();
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
        var text = type ? SearchIndex() : ReadSpecifiedFiles(path);
        var response = @"HTTP/1.1 200 OK\r\n";
        response += "Content-Length: " + text.Length;
        response += "Content-Type: text/html";
        response += "Connection: close\r\n";
        response += "\r\n";
        var temp = Encoding.UTF8.GetBytes(response);
        var z = new byte[temp.Length + text.Length];
        temp.CopyTo(z, 0);
        text.CopyTo(z, temp.Length);
        return z;
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
            extension = "9119";
        }
        if (extension.Equals("ico"))
        {
            return Array.Empty<byte>();
        }
        if (extension.Equals("js") && extension.Equals("html") && extension.Equals("txt") && extension.Equals("css"))
        {
            return Encoding.UTF8.GetBytes(File.ReadAllText(path.TrimStart('/')));
        }
        if (extension.Equals("9119"))
        {
            Console.WriteLine(path);
            return Htmlbuilder(path);
        }
        else
        {
            return File.ReadAllBytes(path.TrimStart('/'));
        }
    }

    private byte[] SearchIndex()
    {
        var test = _readHtml.ReadFilesInDirectory();
        var valid = false;
        foreach (var variable in test)
        {
            var result = variable.Substring(variable.Length - 10);
            if (result.Equals("index.html"))
            {
                valid = true;
            }
        }
        return Encoding.UTF8.GetBytes(valid ? File.ReadAllText("index.html") : _htmlBuilder.Page404());
    }

    private byte[] Htmlbuilder(string path)
    {
        Console.WriteLine(path);
        if (path.Equals("/source"))
        {
            path = "/";
        }
        else
        {
            if (!Directory.Exists(path.Trim('/')))
            {
                return Encoding.UTF8.GetBytes(_htmlBuilder.Page404());
            }
        }
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();

        var files = _readHtml.ReadFilesInSpecifiedDirectory(path);
        var directories = _readHtml.ReadSpecifiedDirectories(path);
        if (files.Length > 0)
        {
            foreach (var file in files)
            {
                string[] fileSplit = file.Split('\\');
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

        string topHtml = _htmlBuilder.Header();
        string bottomHtml = _htmlBuilder.Footer();
        foreach (var dirNameKey in directoriesNames)
        {
            topHtml += _htmlBuilder.Alink(CleanPath(dirNameKey.Value), dirNameKey.Key, true);
        }

        foreach (var fileNameKey in fileNames)
        {
            topHtml += _htmlBuilder.Alink(CleanPath(fileNameKey.Value), fileNameKey.Key, false);
        }
        return Encoding.UTF8.GetBytes(topHtml + bottomHtml);
    }

    private string CleanPath(string sourceString)
    {
        string test = Directory.GetCurrentDirectory();
        int index = sourceString.IndexOf(test);
        string cleanPath = (index < 0)
            ? sourceString
            : sourceString.Remove(index, test.Length);
        return cleanPath.TrimStart('\\');
    }
}