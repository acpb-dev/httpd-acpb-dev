using System.Text;

namespace Httpd;

public class Requests
{
    private ResponseBuilder _responseBuilder = new ResponseBuilder();
    public IDictionary<string, string> _requests = new Dictionary<string, string>();
    
    public byte[] ManageRequest(string request)
    {
        _requests.Clear();
        var strReader = new StringReader(request);
        // Console.WriteLine(request);
        var count = 0;
        while (null != (request = strReader.ReadLine()))
        {
            string[] keyVal;
            if (count == 0)
            {
                keyVal = request.Split();
            }
            else
            {
                keyVal = request.Split(": ");
 
            }
            if (keyVal.Length > 1)
            {
                _requests.Add(keyVal[0], keyVal[1]);
            }
            count++;
        }
        var path = "";
        foreach (var (key, value) in _requests)
        {
            if (key.Equals("GET"))
            {
                path = value;
            }

            if (key.Equals("content"))
            {
                
            }
        }
        return ResponseCreator(path);
    }

    private byte[] ResponseCreator(string path)
    {
        var byteResponse = _responseBuilder.Response(path, _requests);
        return byteResponse;
    }







}