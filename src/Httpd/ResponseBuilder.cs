using System.Text;

namespace Httpd;

public class ResponseBuilder
{
    private readonly FileReader _fileReader = new FileReader();
    private string _valueType = "";
    private bool _error404 = false;
    
    public static byte[] HtmlBuilder(string path)
    {
        IDictionary<string, string> fileNames = new Dictionary<string, string>();
        IDictionary<string, string> directoriesNames = new Dictionary<string, string>();
        var topHtml = HtmlStringBuilder.Header();
        var bottomHtml = HtmlStringBuilder.Footer();
        //Console.WriteLine(path + "/\\/\\");
        if (path.Equals("/source"))
        {
            path = "/";
        }
        else
        {
            if (!Directory.Exists(path.Trim('/')))
            {
                //_error404 = true;
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
    
    public byte[] Response(string path, IDictionary<string, string> request)
    {
        string contentType = "";
        byte[] responseBytes;
        if (path.Equals("/"))
        {
            contentType = "text/html";
            responseBytes = SearchIndex(request);
        }
        else
        {
            responseBytes = _fileReader.ReadSpecifiedFiles(path, request);
            contentType = CheckFileExtension(path);
            if (contentType.Equals("N/A"))
            {
                
            }

        }
        var response = !_error404 ? "HTTP/1.1 200 OK\r\n" : "HTTP/1.1 404 OK\r\n";
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
        // _error404 = false;
        //_valueType = "text/html";
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

    private string CheckFileExtension(string path)
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