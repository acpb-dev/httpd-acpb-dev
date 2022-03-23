namespace Httpd;

public class ReadHTML
{
    public string[] ReadFilesInDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine(currentDirectory);
        var filesInDirectory = Directory.GetFiles(currentDirectory);
        return filesInDirectory;
    }
    
    public string[] ReadDirectories()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine(currentDirectory);
        var directories = Directory.GetDirectories(currentDirectory);
        return directories;
    }
    
    public string[] ReadFilesInSpecifiedDirectory(string path)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine(path);
        var filesInDirectory = Directory.GetFiles(currentDirectory + path);
        return filesInDirectory;
    }
    
    public string[] ReadSpecifiedDirectories(string path)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine(path);
        var directories = Directory.GetDirectories(currentDirectory + path);
        return directories;
    }
}