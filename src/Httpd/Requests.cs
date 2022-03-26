using System.Text;

namespace Httpd;

public class Requests
{
    private ResponseBuilder _responseBuilder = new ResponseBuilder();
    public IDictionary<string, string> _requests = new Dictionary<string, string>();
    
    public byte[] HandleRequest(string verb, string ressource, IDictionary<string, string> headers, string body)
    {
        return ResponseCreator(ressource);
    }
    public byte[] SeperateRequest(string request)
    {
        _requests.Clear();
        var strReader = new StringReader(request);
        string[] keyVal;
        var verb = "";
        var resource = "";
        var body = "";
        var count = 0;
        bool isContent = false;
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

    private byte[] ResponseCreator(string path)
    {
        var byteResponse = _responseBuilder.Response(path, _requests);
        return byteResponse;
    }
}