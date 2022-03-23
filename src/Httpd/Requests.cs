using System.Net.Mime;
using System.Text;
using Httpd;

public class Requests
{
    private readonly ReadHTML _readHtml = new();
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
        return Encoding.UTF8.GetBytes(valid ? File.ReadAllText("index.html") : "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>404</title> </head> <body> <div class=\"div\">404 Page not found</div> </body> </html>");
    }

    private byte[] Htmlbuilder(string path)
    {
        Console.WriteLine(path);
        if (path.Equals("/source"))
        {
            path = "/";
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
                // Console.WriteLine(fileSplit[^1]);
                fileNames.Add(fileSplit[^1], file);
            }
        }
        if (directories.Length > 0)
        {
            foreach (var directory in directories)
            {
                string[] directorySplit = directory.Split('\\');
                // Console.WriteLine(directorySplit[^1]);
                directoriesNames.Add(directorySplit[^1], directory);
            }
        }

        if (directories.Length == 0 && files.Length == 0)
        {
            return Encoding.UTF8.GetBytes("<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>404</title> </head> <body> <div class=\"div\">404 Page not found</div> </body> </html>");
        }
        string topHtml =
            "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\"> <meta http-equiv=\"X-UA-Compatible\" content=\"ie=edge\"> <title id=\"pageTitle\">File Finder</title> </head> <body>";
        string bottomHtml = "</body> </html>";
        foreach (var dirNameKey in directoriesNames)
        {
            topHtml += $"<div><a href=\"{CleanPth(dirNameKey.Value)}\">{dirNameKey.Key.ToUpper()}</a></div>";
        }

        foreach (var fileNameKey in fileNames)
        {
            topHtml += $"<div><a href=\"{CleanPth(fileNameKey.Value)}\">{fileNameKey.Key.ToLower()}</a></div>";
        }
        return Encoding.UTF8.GetBytes(topHtml + bottomHtml);
    }

    private string CleanPth(string sourceString)
    {
        string test = Directory.GetCurrentDirectory();
        int index = sourceString.IndexOf(test);
        string cleanPath = (index < 0)
            ? sourceString
            : sourceString.Remove(index, test.Length);
        return cleanPath.TrimStart('\\');
    }
}