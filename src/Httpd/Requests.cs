using System.Text;
using Httpd;

public class Requests
{
    private FileReader _fileReader = new FileReader();
    private IDictionary<string, string> _requests = new Dictionary<string, string>();
    
    

    
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
        var link = "";
        foreach (var (key, value) in _requests)
        {
            if (key.Equals("GET"))
            {
                link = value;
            }

            if (key.Equals("content"))
            {
                
            }
        }
        return Html(link);
    }

    private byte[] Html(string path)
    {
        
        var text = path.Equals("/") ? ResponseBuilder.SearchIndex() : _fileReader.ReadSpecifiedFiles(path);
        var response = ResponseBuilder.Response(text.Length);
        var temp = Encoding.UTF8.GetBytes(response);
        var z = new byte[temp.Length + text.Length];
        temp.CopyTo(z, 0);
        text.CopyTo(z, temp.Length);
        return z;
    }







}