using System.Collections;
using System.Configuration;

namespace Httpd;

public class FileReader
{
    private static Dictionary<string, string> ImagesFormat = new();
    private static Dictionary<string, string> FileFormat = new();
    public static bool DirectoryListing;

    public static (byte[], string, string) ReadSpecifiedFiles(string path)
    {
        var temp = path.Split(".");
        if (temp.Length < 2)
        {
            if (DirectoryListing)
            {
                return ResponseBuilder.HtmlBuilder(path);
            }
            return (ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404", "text/html"); 
        }
        var extension = temp[^1];
        if (!CheckExtension(FileFormat, extension).Equals("N/A"))
        {
            if (DirectoryFileReader.CheckFileExistance(path))
            {
                return (ByteReader.ConvertFileToByte(path.TrimStart('/')), "200", CheckExtension(FileFormat, extension));
            }
            else
            {
                return (ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404", "text/html");
            }
        }
        
        if (!CheckExtension(ImagesFormat, extension).Equals("N/A"))
        {
            if (DirectoryFileReader.CheckFileExistance(path))
            {
                return (ByteReader.ConvertBytes(path.TrimStart('/')), "200", CheckExtension(ImagesFormat, extension));
            }
        }
        return (ByteReader.ConvertTextToByte(HtmlStringBuilder.Page404()), "404", "text/html");
    }

    public static void ReadAppConfig()
    {
        var fileSupported = (Hashtable)ConfigurationManager.GetSection("fileSupported");
        Dictionary<string,string> fileDictionary = fileSupported.Cast<DictionaryEntry>().ToDictionary(d => (string)d.Key, d => (string)d.Value);
        
        foreach (var (key, value) in fileDictionary)
        {
            ImagesFormat.Add(key, value);
        }

        Console.WriteLine();
        var textSupported = (Hashtable)ConfigurationManager.GetSection("testFileSupported");
        Dictionary<string,string> textDictionary = textSupported.Cast<DictionaryEntry>().ToDictionary(d => (string)d.Key, d => (string)d.Value);
        foreach (var (key, value) in textDictionary)
        {
            FileFormat.Add(key, value);
        }
        var directoryListing = ConfigurationManager.AppSettings["Directory_Listing"];
        if (directoryListing != null && directoryListing.Equals("true"))
        {
            DirectoryListing = true;
        }
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