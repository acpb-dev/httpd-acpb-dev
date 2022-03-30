namespace Httpd;

public static class DirectoryReader
{
    public static string[] ReadFilesInDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var filesInDirectory = Directory.GetFiles(currentDirectory);
        return filesInDirectory;
    }
    
    public static string[] ReadDirectories()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var directories = Directory.GetDirectories(currentDirectory);
        return directories;
    }
    
    public static string[] ReadFilesInSpecifiedDirectory(string path)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var filesInDirectory = Directory.GetFiles(currentDirectory + path);
        return filesInDirectory;
    }
    
    public static string[] ReadSpecifiedDirectories(string path)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var directories = Directory.GetDirectories(currentDirectory + path);
        return directories;
    }

    public static bool CheckDirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public static bool CheckFileExistence(string fileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        return File.Exists(currentDirectory + fileName);
    }
    
    public static string CleanPath(string sourceString)
    {
        var test = Directory.GetCurrentDirectory();
        var removed = sourceString.Remove(0, test.Length);
        return removed;
    }

    public static string CleanString(string sourceString)
    {
        var split = sourceString.Split('/');
        return split[^1];
    }
}