using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Httpd;



public class Server
{
    
    private string CurrentDirectory;
    private string[] FilesInDirectory;

    private TcpListener _listenner;

    private static int _requestCount = 0;
    
    public int Port { get; set; }
    public Requests requests = new Requests();
    public Server(int port)
    {
        Port = port;
        _listenner = TcpListener.Create(Port);
        
    }

    public async Task Start()
    {
        _listenner.Start();
        Console.WriteLine($"Server has started on port {Port}.");
        while (true)
        {
            var client = await _listenner.AcceptTcpClientAsync();
            HandleRequest(client);
        }
    }

    private void HandleRequest(TcpClient client)
    {
        Thread.Sleep(50);
        var stream = client.GetStream();
        requests.ManageRequest("temporary string");
        
        
        var socket = stream.Socket;
        var buffer = new byte[socket.Available];
        stream.Read(buffer, 0, buffer.Length);
        
        var data = Encoding.UTF8.GetString(buffer);
        _requestCount++;
        Console.WriteLine($"--- Request Count #{_requestCount} ---");
        Console.WriteLine(data);
        
        Console.WriteLine($"--- End of request ---");
        
        string[] test = ReadHtmlFromRoute();
        foreach (var VARIABLE in test)
        {
            //Console.WriteLine(VARIABLE);
        }
        var responsesByte = Encoding.UTF8.GetBytes(Html(test));
        socket.Send(responsesByte);
        socket.Close();
    }

    private string Html(string[] test)
    {
        string text = "";
        bool valide = false;
        foreach (var VARIABLE in test)
        {
            var result = VARIABLE.Substring(VARIABLE.Length - 10);
            if (result.Equals("index.html"))
            {
                valide = true;
            }
        }
        if (valide)
        {
            text = File.ReadAllText("index.html");
        }
        else
        {
            text = "<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"UTF-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>404</title> </head> <body> <div class=\"div\">404 Page not found</div> </body> </html>";
        }
        var response = @"HTTP/1.1 200 OK\r\n";
        response += "Content-Length: 44";
        response += "Content-Type: text/html";
        response += "Connection: close\r\n";
        response += "\r\n";
        response += text;
        return response;
    }

    private string[] ReadHtmlFromRoute()
    {
        CurrentDirectory = Directory.GetCurrentDirectory();
        FilesInDirectory = Directory.GetFiles(CurrentDirectory);
        return FilesInDirectory;
    }
}