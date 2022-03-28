using System.Text;

namespace Httpd;

public static class ByteReader
{
    public static byte[] ConvertFileToByte(string path) => Encoding.UTF8.GetBytes(File.ReadAllText(path));
    public static byte[] ConvertTextToByte(string text) => Encoding.UTF8.GetBytes(text);
    public static byte[] ConvertBytes(string path) => File.ReadAllBytes(path);
    public static string ReadFileToString(string path) => File.ReadAllText(path);

}