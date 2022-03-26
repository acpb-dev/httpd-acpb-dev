using System;
using System.Collections.Generic;
using System.IO;

namespace Httpd;

public class ResponseBuilder
{
    private readonly FileReader _fileReader = new();
    
    public (byte[], string) ResponseManager(string path, IDictionary<string, string> request)
    {
        Console.WriteLine(path);
        (byte[], string, string) responseBytes;
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
            responseBytes = _fileReader.ReadSpecifiedFiles(path);

        }
        return (AddTwoByteArrays(ResponseHeader(responseBytes.Item1, responseBytes.Item2, responseBytes.Item3), responseBytes.Item1), responseBytes.Item2);
    }


    

    private byte[] ResponseHeader(byte[] responseBytes, string error, string contentType)
    {
        var response = $"HTTP/1.1 {error} OK\r\n";
        response += $"Content-Length: {responseBytes.Length}\r\n";
        response += $"Content-Type: {contentType}\r\n";
        response += "Connection: close\r\n";
        response += "\r\n";
        return ByteReader.ConvertTextToByte(response);
    }

    (byte[], string, string) DebugBuilder(IDictionary<string, string> request)
    {
        var topHtml = HtmlStringBuilder.Header();
        var bottomHtml = HtmlStringBuilder.Footer();
        topHtml += HtmlStringBuilder.Debug(request);
        return (ByteReader.ConvertTextToByte(topHtml + bottomHtml), "200", "text/html");
    }

    (byte[], string, string) SearchIndex()
    {
        var test = ReadHTML.ReadFilesInDirectory();
        foreach (var variable in test)
        {
            var result = variable[^10..];
            if (result.Equals("index.html"))
            {
                return (ByteReader.ConvertFileToByte(result), "200", "text/html");
            }
        }
        if (FileReader._directoryListing)
        {
            return _fileReader.ReadSpecifiedFiles("/");
        }
        return (ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404", "text/html");
    }
    
    
    public static byte[] HtmlBuilder(string path)
    {
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();
        var topHtml = HtmlStringBuilder.HeaderDl();
        var bottomHtml = HtmlStringBuilder.Footer();
        if (!Directory.Exists(path.Trim('/')))
        {
            return ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404());
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

        if (!path.Equals("/"))
        {
            var test = path.TrimEnd('/');
            var index = test.LastIndexOf('/');
            Console.WriteLine(index);
            test = test.Substring(0, index);
            if (test.Equals(""))
            {
                test = "/";
            }
            topHtml += HtmlStringBuilder.ParentDirectory(test, "Parent Directory");
        }

        
        foreach (var (key, value) in directoriesNames)
        {
            DirectoryInfo dir = new DirectoryInfo(value);
            
            topHtml += HtmlStringBuilder.Alink(ReadHTML.CleanPath(value), ReadHTML.CleanString(key), true, dir.LastAccessTime, 0);
        }

        foreach (var (key, value) in fileNames)
        {
            DirectoryInfo dir = new DirectoryInfo(value);
            var fi1 = new FileInfo(value);
            
            topHtml += HtmlStringBuilder.Alink(ReadHTML.CleanPath(value), ReadHTML.CleanString(key), false, dir.LastAccessTime, fi1.Length);
        }
        return ByteReader.ConvertTextToByte(topHtml + bottomHtml);
    }
    
    private bool DebugMode(string path)
    {
        const string debug = "/debug";
        if (path.Length >= debug.Length)
        {
            var test = path.Remove(0, path.Length-debug.Length);
            if (test.Equals(debug))
            {
                return true;
            }
        }
        return false;
    }
    
    private byte[] AddTwoByteArrays(byte[] array1, byte[] array2)
    {
        var z = new byte[array1.Length + array2.Length];
        array1.CopyTo(z, 0);
        array2.CopyTo(z, array1.Length);
        return z;
    }

    private static string CheckFileExtension(string path)
    {
        var temp = path.Split(".");
        var extension = temp[^1];
        var fileExtension = FileReader.CheckExtension(FileReader.ImagesFormat, extension);
        if (!fileExtension.Equals("N/A"))
        {
            return fileExtension;
        }
        fileExtension = FileReader.CheckExtension(FileReader.FileFormat, extension);
        if(!fileExtension.Equals("N/A"))
        {
            return fileExtension;
        }
        return "text/html";
    }
}