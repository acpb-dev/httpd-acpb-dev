using System.Text;

namespace Httpd;

public class Requests
{
    private readonly ResponseBuilder _responseBuilder = new();
    private readonly IDictionary<string, string> _requests = new Dictionary<string, string>();
    
    private byte[] HandleRequest(string verb, string ressource, IDictionary<string, string> headers, string body)
    {
        return verb switch
        {
            "GET" => GetResponseCreator(ressource),
            "POST" => PostResponseCreator(),
            "PUT" => PutResponseCreator(),
            "PATCH" => PatchResponseCreator(),
            "DELETE" => DeleteResponseCreator(),
            _ => GetResponseCreator(ressource)
        };
    }
    public byte[] SeparatedRequest(string request)
    {
        _requests.Clear();
        var strReader = new StringReader(request);
        var verb = "";
        var resource = "";
        var body = "";
        var count = 0;
        var isContent = false;
        while (null != (request = strReader.ReadLine()))
        {
            if (request.Equals(""))
            {
                isContent = true;
            }
            if (isContent)
            {
                body += request;
            }

            string[] keyVal;
            if (count == 0)
            {
                keyVal = request.Split();
                resource = keyVal[1];
                verb = keyVal[0];
            }
            else
            {
                keyVal = request.Split(": ");
 
            }
            if (keyVal.Length > 1 && count > 0)
            {
                
                _requests.Add(keyVal[0], keyVal[1]);
            }
            count++;
        }
        return HandleRequest(verb, resource, _requests, body);
    }

    private byte[] GetResponseCreator(string path)
    {
        var byteResponse = _responseBuilder.Response(path, _requests);
        return byteResponse;
    }
    private byte[] PostResponseCreator()
    {
        return Array.Empty<byte>();
    }
    private byte[] PutResponseCreator()
    {
        return Array.Empty<byte>();
    }
    private byte[] PatchResponseCreator()
    {
        return Array.Empty<byte>();
    }
    private byte[] DeleteResponseCreator()
    {
        return Array.Empty<byte>();
    }
}