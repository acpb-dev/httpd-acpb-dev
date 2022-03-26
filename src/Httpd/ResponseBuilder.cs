using System;
using System.Collections.Generic;
using System.IO;

namespace Httpd;

public class ResponseBuilder
{
    private readonly FileReader _fileReader = new();
    public static bool Error404 = false;
    
    public static byte[] HtmlBuilder(string path)
    {
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();
        var topHtml = HtmlStringBuilder.HeaderDl();
        var bottomHtml = HtmlStringBuilder.Footer();
        if (path.Equals("/source"))
        {
            path = "/";
        }
        else
        {
            if (!Directory.Exists(path.Trim('/')))
            {
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
        //Console.WriteLine(path);
        var debug = "/debug";
        if (path.Length >= debug.Length)
        {
            // var pathUpdated = path.Remove(path.Length - debug.Length, debug.Length);
            var test = path.Remove(0, path.Length-debug.Length);
            if (test.Equals(debug))
            {
                return true;
            }
        }
        return false;
    }
    
    public byte[] Response(string path, IDictionary<string, string> request)
    {
        
        var contentType = CheckFileExtension(path);;
        byte[] responseBytes;

        if (path.Equals("/"))
        {
            contentType = "text/html";
            responseBytes = SearchIndex(request);
        }
        else if (DebugMode(path))
        {
            var topHtml = HtmlStringBuilder.Header();
            var bottomHtml = HtmlStringBuilder.Footer();
            topHtml += HtmlStringBuilder.Debug(request);
            responseBytes =  ByteReader.ConvertTextToByte(topHtml + bottomHtml);
        }
        else
        {
            responseBytes = _fileReader.ReadSpecifiedFiles(path, request);
            if (contentType.Equals("N/A"))
            {
                contentType = "text/html";
            }
        }
        var response = !Error404 ? "HTTP/1.1 200 OK\r\n" : "HTTP/1.1 404 OK\r\n";
        response += $"Content-Length: {responseBytes.Length}\r\n";
        response += $"Content-Type: {contentType}\r\n";
        response += "Connection: close\r\n";
        response += "\r\n";
        return AddTwoByteArrays(ByteReader.ConvertTextToByte(response), responseBytes);
    }   

    private byte[] AddTwoByteArrays(byte[] array1, byte[] array2)
    {
        var z = new byte[array1.Length + array2.Length];
        array1.CopyTo(z, 0);
        array2.CopyTo(z, array1.Length);
        return z;
    }
    
    private static byte[] SearchIndex(IDictionary<string, string> request)
    {
        var test = ReadHTML.ReadFilesInDirectory();
        foreach (var variable in test)
        {
            var result = variable[^10..];
            if (result.Equals("index.html"))
            {
                return ByteReader.ConvertFileToByte(result);
            }
        }
        return Array.Empty<byte>();
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