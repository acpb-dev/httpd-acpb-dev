namespace Httpd;

public class ReadHTML
{
    public string[] ReadHtmlFromRoute()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var filesInDirectory = Directory.GetFiles(currentDirectory);
        return filesInDirectory;
    }
}