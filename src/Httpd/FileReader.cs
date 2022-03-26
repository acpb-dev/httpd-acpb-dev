using System.Configuration;

namespace Httpd;

public class FileReader
{
    public static Dictionary<string, string> ImagesFormat = new();
    public static Dictionary<string, string> FileFormat = new();
    public static bool DirectoryListing = false;



    public static void ReadAppConfig()
    {
        var fileSupported = ConfigurationManager.AppSettings["fileSupported"];
        var splitFileSupported = fileSupported.Split();
        for (int i = 0; i < splitFileSupported.Length; i++)
        {
            var split = splitFileSupported[i].Split(':');
            ImagesFormat.Add(split[0], split[1]);
        }
        fileSupported = ConfigurationManager.AppSettings["testFileSupported"];
        splitFileSupported = fileSupported.Split();
        for (int i = 0; i < splitFileSupported.Length; i++)
        {
            var split = splitFileSupported[i].Split(':');
            FileFormat.Add(split[0], split[1]);
        }
        var directoryListing = ConfigurationManager.AppSettings["Directory_Listing"];
        if (directoryListing.Equals("true"))
        {
            DirectoryListing = true;
        }
    }

    public byte[] ReadSpecifiedFiles(string path, IDictionary<string, string> requests)
    {
        var temp = path.Split(".");
        if (temp[0].Equals("/source") || temp.Length < 2)
        {
            if (DirectoryListing)
            {
                return ResponseBuilder.HtmlBuilder(path);
            }
            ResponseBuilder._error404 = true;
            
            return ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()); 
        }
        var extension = temp[^1];
        if (!CheckExtension(FileFormat, extension).Equals("N/A"))
        {
            if (ReadHTML.CheckFileExistance(path))
            {
                return ByteReader.ConvertFileToByte(path.TrimStart('/'));
            }
            else
            {
                ResponseBuilder._error404 = true;
                
                return ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404());
            }
        }
        
        if (!CheckExtension(ImagesFormat, extension).Equals("N/A"))
        {
            if (ReadHTML.CheckFileExistance(path))
            {
                return ByteReader.ConvertBytes(path.TrimStart('/'));
            }
        }
        ResponseBuilder._error404 = true;
        
        return ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404());
    }
    
    public static string CheckExtension(Dictionary<string, string> dictionary, string extension)
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