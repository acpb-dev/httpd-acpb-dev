using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Httpd;



public class Server
{
    
    private string CurrentDirectory;
    private string[] FilesInDirectory;

    private TcpListener _listenner;
    
    public int Port { get; set; }
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
            // Task<TcpClient> client = _listenner.AcceptTcpClientAsync();
            var client = await _listenner.AcceptTcpClientAsync();
            HandleRequest(client);
        }
    }

    private void HandleRequest(TcpClient client)
    {
        Thread.Sleep(50);
        var stream = client.GetStream();
        var socket = stream.Socket;
        var buffer = new byte[socket.Available];
        
        stream.Read(buffer, 0, buffer.Length);
        
        //Console.WriteLine(BitConverter.ToString(buffer));
        var data = Encoding.UTF8.GetString(buffer);
        //Console.WriteLine(data);
        string[] test = ReadHTMLFromRoute();
        string text = "";
        bool valide = false;
        foreach (var VARIABLE in test)
        {
            var result = VARIABLE.Substring(VARIABLE.Length - 10);
            //Console.WriteLine(result);
            if (result.Equals("index.html"))
            {
                valide = true;

            }


            //Console.WriteLine(VARIABLE);
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
        
        
        
        //response += "<html><body><h1>It works!</h1></body></html>";
        var responsesByte = Encoding.UTF8.GetBytes(response);
        socket.Send(responsesByte);
        socket.Close();

          //  
    }
    
    public string[] ReadHTMLFromRoute()
    {
        CurrentDirectory = Directory.GetCurrentDirectory();
        FilesInDirectory = Directory.GetFiles(CurrentDirectory);
        return FilesInDirectory;
    }
}