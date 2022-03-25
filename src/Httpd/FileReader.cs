namespace Httpd;

public class FileReader
{
    private readonly Dictionary<string, string> _imagesFormat = new()
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
    private readonly Dictionary<string, string> _fileFormat = new()
    {
        { "html", "text/html" },
        { "htm", "text/html" },
        { "js", "text/javascript" },
        { "css", "text/css" },
        { "mjs", "text/javascript" },
        { "txt", "text/plain" },
        { "php", "application/x-httpd-php" },
        { "json", "application/json" },
        { "jsonld", "application/ld+json" }
    };

    private bool DebugMode(string path)
    {
        var debug = "/debug";
        if (path.Length >= debug.Length)
        {
            var pathUpdated = path.Remove(path.Length - debug.Length, debug.Length);
            var test = path.Remove(0, path.Length-debug.Length);
            if (test.Equals(debug))
            {
                return true;
            }
        }
        return false;
    }
    
    public byte[] ReadSpecifiedFiles(string path)
    {
        ResponseBuilder.Error404 = false;
        if (DebugMode(path))
        {
            var topHtml = HtmlStringBuilder.Header();
            var bottomHtml = HtmlStringBuilder.Footer();
            topHtml += HtmlStringBuilder.Debug(Requests._requests);
            ResponseBuilder.ValueType = "text/html";
            return ByteReader.ConvertTextToByte(topHtml + bottomHtml);
        }
        var temp = path.Split(".");
        if (temp[0].Equals("/source"))
        {
            return ResponseBuilder.HtmlBuilder(path);
        }
        var extension = temp[^1];
        if (!CheckExtension(_fileFormat, extension).Equals("N/A"))
        {
            ResponseBuilder.ValueType = CheckExtension(_fileFormat, extension);
            if (ReadHTML.CheckFileExistance(path))
            {
                return ByteReader.ConvertFileToByte(path.TrimStart('/'));
            }
            else
            {
                ResponseBuilder.Error404 = true;
                return ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404());
            }
        }
        
        if (!CheckExtension(_imagesFormat, extension).Equals("N/A"))
        {
            ResponseBuilder.ValueType = CheckExtension(_imagesFormat, extension);
            if (ReadHTML.CheckFileExistance(path))
            {
                return ByteReader.ConvertBytes(path.TrimStart('/'));
            }
        }
        // if (!ReadHTML.CheckFileExistance(path))
        // {
        //     ResponseBuilder.Error404 = true;
        // }
        //return
        ResponseBuilder.ValueType = "text/html";
        return ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404());
    }
    
    private static string CheckExtension(Dictionary<string, string> dictionary, string extension)
    {
        foreach (var (key, value) in dictionary)
        {
            if (key.Equals(extension))
            {
                return value;
            }
        }
        return "N/A";
    }
}