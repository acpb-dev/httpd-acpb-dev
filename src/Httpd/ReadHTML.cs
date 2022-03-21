namespace Httpd;

public class ReadHTML
{
    public string[] ReadFilesInDirectory()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var filesInDirectory = Directory.GetFiles(currentDirectory);
        return filesInDirectory;
    }

    public string[] ReadDirectories()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var directories = Directory.GetDirectories(currentDirectory);
        return directories;
    }
}