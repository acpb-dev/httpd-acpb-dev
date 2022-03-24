namespace Httpd;

public class FileReader
{
    
    private Dictionary<string, string> _imagesFormat = new()
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
    private Dictionary<string, string> _fileFormat = new()
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
    
    
    public byte[] ReadSpecifiedFiles(string path)
    {
        ResponseBuilder.Error404 = false;
        var temp = path.Split(".");
        if (temp.Length < 2 || temp[0].Equals("/source"))
        {
            return ResponseBuilder.HtmlBuilder2(path);
        }
        var extension = temp[^1];

        
        if (!CheckExtension(_fileFormat, extension).Equals("N/A"))
        {
            ResponseBuilder.ValueType = CheckExtension(_fileFormat, extension);
            return ByteReader.ConvertFileToByte(ReadHTML.CheckFileExistance(path) ? path.TrimStart('/') : HtmlStringBuilder.Page404());
            // _error404 = true;
        }
        
        if (!CheckExtension(_imagesFormat, extension).Equals("N/A"))
        {
            ResponseBuilder.ValueType = CheckExtension(_imagesFormat, extension);
            if (ReadHTML.CheckFileExistance(path))
            {
                return ByteReader.ConvertBytes(path.TrimStart('/'));
            }
        }
        
        return !ReadHTML.CheckFileExistance(path) ? 
            ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()) : Array.Empty<byte>();
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