using System.Text;

namespace Httpd;

public class ResponseBuilder
{
    public static string ValueType = "";
    public static bool Error404 = false;
    
    public static byte[] HtmlBuilder(string path)
    {
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();
        var topHtml = HtmlStringBuilder.Header();
        var bottomHtml = HtmlStringBuilder.Footer();
        if (path.Equals("/source"))
        {
            path = "/";
        }
        else
        {
            if (!Directory.Exists(path.Trim('/')))
            {
                Error404 = true;
                return ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404());
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
            topHtml += HtmlStringBuilder.Alink(ReadHTML.CleanPath(value), ReadHTML.CleanString(key), true);
        }

        foreach (var (key, value) in fileNames)
        {
            topHtml += HtmlStringBuilder.Alink(ReadHTML.CleanPath(value), ReadHTML.CleanString(key), false);
        }
        return ByteReader.ConvertTextToByte(topHtml + bottomHtml);
    }
    
    public static string Response(int length)
    {
        var response = !Error404 ? @"HTTP/1.1 200 OK\r\n" : @"HTTP/1.1 404 OK\r\n";
        response += $"Content-Length: {length}\t\n";
        response += $"Content-Type: {ValueType}\r\n";
        response += "Connection: close\r\n";
        response += "\r\n";
        return response;
    }
    
    public static byte[] SearchIndex()
    {
        Error404 = false;
        ValueType = "text/html";
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
        if (valid)
        {
            return ByteReader.ConvertFileToByte("index.html");
        }
        else
        {
            Error404 = true;
            return ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404());
        }
    }
}