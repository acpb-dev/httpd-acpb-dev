namespace Httpd.Tests;

public class FileReaderTests
{
    private static Dictionary<string, string> testDictionary = new()
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
        { "ico", "image/vnd.microsoft.icon" },
        { "tif", "image/tiff" },
        { "tiff", "image/tiff" },
        { "webp", "image/webp" },
        { "html", "text/html" },
        { "htm", "text/html" },
        { "js", "text/javascript" },
        { "css", "text/css" },
        { "mjs", "text/javascript" },
        { "txt", "text/plain" },
        { "php", "application/x-httpd-php" }
    };
    
    [Theory]
    [InlineData("html", "text/html")]
    [InlineData("jpeg", "image/jpeg")]
    [InlineData("php", "application/x-httpd-php")]
    [InlineData("nothing", "N/A")]
    [InlineData("", "N/A")]
    public void CheckExtensionTest( string valSearched , string expected)
    {
        var result = FileReader.CheckExtension(testDictionary, valSearched);
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("Gzip.cs", "404 Not Found", "text/html")]
    public void ReadSpecifiedFilesTest(string path , string expected1, string expected2)
    {
        var result = FileReader.ReadSpecifiedFiles(path);
        Assert.Equal(expected1, result.Item2);
        Assert.Equal(expected2, result.Item3);
    }
}