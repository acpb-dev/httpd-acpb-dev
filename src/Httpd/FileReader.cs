﻿using System.Collections;
using System.Configuration;

namespace Httpd;

public static class FileReader
{
    private static readonly Dictionary<string, string> ImagesFormat = new();
    private static readonly Dictionary<string, string> FileFormat = new();
    public static readonly Dictionary<string, string> Routes = new();
    public static bool DirectoryListing;

    public static void ReadAppConfig()
    {
        var routesSupported = (Hashtable)ConfigurationManager.GetSection("routesSupported");
        var routeDirectoory = routesSupported.Cast<DictionaryEntry>().ToDictionary(d => (string)d.Key, d => (string)d.Value!);
        
        foreach (var (key, value) in routeDirectoory)
        {
            Routes.Add(key, value);
        }

        
        var fileSupported = (Hashtable)ConfigurationManager.GetSection("fileSupported");
        var fileDictionary = fileSupported.Cast<DictionaryEntry>().ToDictionary(d => (string)d.Key, d => (string)d.Value!);
        
        foreach (var (key, value) in fileDictionary)
        {
            ImagesFormat.Add(key, value);
        }

        Console.WriteLine();
        var textSupported = (Hashtable)ConfigurationManager.GetSection("testFileSupported");
        var textDictionary = textSupported.Cast<DictionaryEntry>().ToDictionary(d => (string)d.Key, d => (string)d.Value!);
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
    
    public static (byte[], string, string) ReadSpecifiedFiles(string path)
    {
        var temp = path.Split(".");
        if (temp.Length < 2)
        {
            return DirectoryListing ? ResponseBuilder.DirectoryListingCreator(path)
                : (ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404 Not Found", "text/html");
        }
        var extension = temp[^1];
        if (!CheckExtension(FileFormat, extension).Equals("N/A"))
        {
            return DirectoryReader.CheckFileExistence(path) ? (ByteReader.ConvertFileToByte(path.TrimStart('/')), "200 OK", CheckExtension(FileFormat, extension))
                : (ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404 Not Found", "text/html");
        }
        else if (!CheckExtension(ImagesFormat, extension).Equals("N/A"))
        {
            if (DirectoryReader.CheckFileExistence(path))
            {
                return (ByteReader.ConvertBytes(path.TrimStart('/')), "200 OK", CheckExtension(ImagesFormat, extension));
            }
        }
        return (ByteReader.ConvertTextToByte(HtmlBuilder.Page415()), "415 Unsupported Media Type", "text/html");
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