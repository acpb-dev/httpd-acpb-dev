namespace Httpd;

public class ReadHTML
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

    // public static bool CheckIfFileExists()
    // {
    //     
    //     return File.Exists(curFile);
    // }
}