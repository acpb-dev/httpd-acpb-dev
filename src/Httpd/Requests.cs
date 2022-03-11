using Httpd;

public class Requests
{
    private string _typeOf = "0";
    
    private ReadHTML _readHtml = new ReadHTML();
    public string ManageRequest(string request)
    {
        StringReader strReader = new StringReader(request);
        string aLine = strReader.ReadLine();
        //Console.WriteLine(aLine);
        string[] splittedStream = aLine.Split();
        if (splittedStream[0].Equals("GET") && splittedStream[1].Equals("/"))
        {
            string[] test = _readHtml.ReadHtmlFromRoute();
            return Html("default", splittedStream[1]) + _typeOf;
        }
        else
        {
            return Html("!default", splittedStream[1]) + _typeOf;
        }
        return "";
    }

    private string Html(string type, string path)
    {
        string text = "";
        if (type.Equals("default"))
        {
            text = SearchIndex();
        }
        else
        {
            //Console.WriteLine(path);
            text = ReadSpecifiedFiles(path);
        }
        
        var response = @"HTTP/1.1 200 OK\r\n";
        response += "Content-Length: 44";
        response += "Content-Type: text/html";
        response += "Connection: close\r\n";
        response += "\r\n";
        response += text;
        return response;
    }
    

    private string ReadSpecifiedFiles(string path)
    {
        string[] temp = path.Split(".");
        string extension = temp[temp.Length - 1];
        if (extension.Equals("jpg"))
        {
            _typeOf = "1";
            Console.WriteLine("SADGSADGASDGASDGSADGASDGASDGASDG");
            path.TrimStart('/');
        }
        else if (path[0].Equals('/'))
        {
            return File.ReadAllText(path.TrimStart('/'));
        }
        return File.ReadAllText(path);
    }

    private string SearchIndex()
    {
        string[] test = _readHtml.ReadHtmlFromRoute();
        bool valide = false;
        foreach (var variable in test)
        {
            var result = variable.Substring(variable.Length - 10);
            if (result.Equals("index.html"))
            {
                valide = true;
            }
        }
        if (valide)
        {
            return File.ReadAllText("index.html");
        }
        return "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>404</title> </head> <body> <div class=\"div\">404 Page not found</div> </body> </html>";
    }
}