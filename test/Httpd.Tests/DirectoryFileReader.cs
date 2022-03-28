namespace Httpd.Tests;

public class DirectoryFileReader
{
    [Theory]
    [InlineData("Gzip.cs", "404", "text/html")]
    public void ReadSpecifiedFilesTest(string path , string expected1, string expected2)
    {
        var result = FileReader.ReadSpecifiedFiles(path);
        Assert.Equal(expected1, result.Item2);
        Assert.Equal(expected2, result.Item3);
    }
    
}