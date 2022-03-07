namespace Httpd;

public class ReadHTML
{
    private string CurrentDirectory;
    private string[] FilesInDirectory;
    
    public string[] ReadHTMLFromRoute(string route)
    {
        CurrentDirectory = Directory.GetCurrentDirectory();
        FilesInDirectory = Directory.GetFiles(CurrentDirectory);
        return FilesInDirectory;
    }
}