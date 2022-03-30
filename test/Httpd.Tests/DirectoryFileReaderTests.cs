namespace Httpd.Tests;

public class DirectoryFileReaderTests
{
    private static string[] expectedReadSpecifiedFilesTest = new string[] { "DirectoryFileReaderTests.cs", "FileReaderTests.cs", "Httpd.Tests.csproj", "RequestsTests.cs", "ResponseTests.cs", "SeriLogTests.cs" };
    
    [Theory]
    [InlineData("lol")]
    public void ReadSpecifiedFilesTest(string test)
    {
        var result = DirectoryReader.ReadFilesInDirectory();
        Assert.NotEqual(expectedReadSpecifiedFilesTest, result);
    }
    
}