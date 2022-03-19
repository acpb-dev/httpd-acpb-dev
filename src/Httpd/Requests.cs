﻿using System.Net.Mime;
using System.Text;
using Httpd;

public class Requests
{
    
    private readonly ReadHTML _readHtml = new();
    public byte[] ManageRequest(string request)
    {
        var strReader = new StringReader(request);
        var aLine = strReader.ReadLine();
        var splitStream = aLine.Split();
        if (splitStream[0].Equals("GET") && splitStream[1].Equals("/"))
        {
            var test = _readHtml.ReadHtmlFromRoute();
            return Html("default", splitStream[1]);
        }
        else
        {
            return Html("!default", splitStream[1]);
        }
    }

    private byte[] Html(string type, string path)
    {
        var text = type.Equals("default") ? SearchIndex() : ReadSpecifiedFiles(path);
        var response = @"HTTP/1.1 200 OK\r\n";
        response += "Content-Length: 44";
        response += "Content-Type: text/html";
        response += "Connection: close\r\n";
        response += "\r\n";
        var temp = Encoding.UTF8.GetBytes(response);
        var z = new byte[temp.Length + text.Length];
        temp.CopyTo(z, 0);
        text.CopyTo(z, temp.Length);
        return z;
    }

    private byte[] ReadSpecifiedFiles(string path)
    {
        var temp = path.Split(".");
        var extension = temp[^1];
        if (!extension.Equals("js") && !extension.Equals("html") && !extension.Equals("txt") && !extension.Equals("css"))
        {
            path.TrimStart('/');
            return File.ReadAllBytes(path.TrimStart('/'));
        }
        else if (path[0].Equals('/'))
        {
            return Encoding.UTF8.GetBytes(File.ReadAllText(path.TrimStart('/')));
        }
        return Encoding.UTF8.GetBytes(File.ReadAllText(path));
    }

    private byte[] SearchIndex()
    {
        var test = _readHtml.ReadHtmlFromRoute();
        var valid = false;
        foreach (var variable in test)
        {
            var result = variable.Substring(variable.Length - 10);
            if (result.Equals("home.html"))
            {
                valid = true;
            }
        }

        return Encoding.UTF8.GetBytes(valid ? File.ReadAllText("home.html") : "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>404</title> </head> <body> <div class=\"div\">404 Page not found</div> </body> </html>");
    }
}