namespace Httpd;

public class Requests
{
    private readonly ResponseBuilder _responseBuilder = new();
    
    public byte[] HandleRequest(string verb, string resource, IDictionary<string, string> requestDictionary, char[] body, SeriLog serilog)
    {
        serilog.HttpMethod = verb;
        serilog.Path = resource;
        switch (verb)
        {
            case "GET":
            {
                var (bytes, status) = GetResponseCreator(resource, requestDictionary);
                serilog.Status = status;
                return bytes;
            }
            case "POST":
            {
                var (bytes, status) = PostResponseCreator(resource, requestDictionary, body);
                serilog.Status = status;
                return bytes;
            }
            case "PUT":
            {
                var (bytes, status) = PutResponseCreator();
                serilog.Status = status;
                return bytes;
            }
            case "PATCH":
            {
                var (bytes, status) = PatchResponseCreator();
                serilog.Status = status;
                return bytes;
            }
            case "DELETE":
            {
                var (bytes, status) = DeleteResponseCreator();
                serilog.Status = status;
                return bytes;
            }
            default:
            {
                var (bytes, status) = GetResponseCreator(resource, requestDictionary);
                serilog.Status = status;
                return bytes;
            }
        }
    }

    private (byte[], string) GetResponseCreator(string path, IDictionary<string, string> request)
    {
        var byteResponse = _responseBuilder.ResponseManager(path, request, "");
        return byteResponse;
    }
    private (byte[], string) PostResponseCreator(string path, IDictionary<string, string> request, char[] body)
    {
        var bodyS = new string(body);
        var byteResponse = _responseBuilder.ResponseManager(path, request, bodyS);
        return byteResponse;
    }
    private (byte[], string) PutResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404 Not Found");
    }
    private (byte[], string) PatchResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404 Not Found");
    }
    private (byte[], string) DeleteResponseCreator()
    {
        return(ByteReader.ConvertTextToByte(HtmlBuilder.Page404()), "404 Not Found");
    }
}